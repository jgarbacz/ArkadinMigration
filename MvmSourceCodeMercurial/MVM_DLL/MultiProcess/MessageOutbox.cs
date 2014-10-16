using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using NLog;
namespace MVM
{
    /// <summary>
    /// The message outbox manages a priority queue of messages ready to be sent. 
    /// </summary>
    public class MessageOutbox
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly SocketHandler socketHandler;
        private readonly string clientNodeId;
        private MessageSender messageSender{get{return socketHandler.messageSender;}}

        // The message outbox implements flow control so that the outbox does not get
        // to large. This monitor is a gate keeper for adding to the outbox. When something
        // is cleared from the outbox this monitor is pulsed to let waiting objects
        // pass though the get. Since this gatekeeper can block it needs to happen
        // up stream of addOrSendMutex locking.
        private readonly object flowControlMonitor = new object();

        // Message outbox requres mutual exlusion when adding or sending. This is the 
        // lowest level lock. In otherwords once you aquire this lock you must not
        // block on any other lock.
        private readonly object addOrSendMutex=new object();

        // locking the priority queue should not be needed because we always have
        // addOrSendMutex when we lock it but I am leaving it in to avoid issues
        // if the code is refactored.


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messageSender"></param>
        public MessageOutbox(SocketHandler socketHandler)
        {
            this.socketHandler = socketHandler;
            this.clientNodeId = this.socketHandler.clientNodeId;
        }


        # region Priority Message Queue

        private ProducerConsumerPriorityQueue<MvmMessage> priorityQueue = new ProducerConsumerPriorityQueue<MvmMessage>();
        public int maxOutboxBytes = 3 * 1024 * 1024;
        public int curOutboxBytes = 0;

        // if this message has a nonzero size and the outbox is already over the 
        // maximum outbox size then we need to have this thread block until the 
        // enough messages are cleared from the outbox that is it below the max 
        // size.
        private void IncrementCurOutboxBytes(MvmMessage message)
        {
            if (message.EstimatedMessageBytes == 0) return;
            logger.Info("IncrementCurOutboxBytes.GetLock");
            lock (this.flowControlMonitor)
            {
                logger.Info("IncrementCurOutboxBytes.GotLock");

                while (message.EstimatedMessageBytes > 0 && curOutboxBytes>maxOutboxBytes)
                {
                    logger.Info("IncrementCurOutboxBytes.Wait");
                    Monitor.Wait(this.flowControlMonitor);
                    logger.Info("IncrementCurOutboxBytes.WokeUP");
                }
                this.curOutboxBytes += message.EstimatedMessageBytes;
                logger.Info("IncrementCurOutboxBytes.ReleasedLock");
            }
        }

        // Decements the message bytes. If this would put move us from over the max
        // to under the max, wake up waiting threads.
        private void DecrementCurOutboxBytes(MvmMessage message)
        {
            if (message.EstimatedMessageBytes == 0) return;
            // if this message puts us back under the limit, pulse the monitor
            logger.Info("DecrementCurOutboxBYtes.GetLock");
            lock (this.flowControlMonitor)
            {
                logger.Info("DecrementCurOutboxBYtes.GotLock");
                // see if the outbox is over the limit
                int prevOutputBytes = this.curOutboxBytes;
                this.curOutboxBytes -= message.EstimatedMessageBytes;
                if (this.curOutboxBytes <= maxOutboxBytes && prevOutputBytes >= maxOutboxBytes)
                {
                    logger.Info("DecrementCurOutboxBYtes.PulseAll");
                    Monitor.PulseAll(this.flowControlMonitor);
                }
            }
            logger.Info("DecrementCurOutboxBYtes.ReleasedLock");
        }

        /// <summary>
        /// Enqueues the message at a priority and blocks if queue is too large
        /// </summary>
        /// <param name="message"></param>
        /// <param name="priority"></param>
        private void EnqueueMessage(MvmMessage message, int priority)
        {
            lock (priorityQueue)
            {
                //logger.Debug("queue msg for {0} of type {1} with priority {2}",this.clientNodeId, message, priority);
                priorityQueue.Enqueue(message, priority);
            }
        }

        /// <summary>
        /// Dequeues a message >= the passed minPriority, or null.
        /// </summary>
        /// <param name="minPriority"></param>
        /// <returns></returns>
        private MvmMessage DequeueMin(int minPriority)
                {
            MvmMessage message;
            lock (priorityQueue)
            {
                message = this.priorityQueue.DequeueMin(minPriority);
                }
            if (message == null) return null;
            return message;
            }

        /// <summary>
        /// Dequeues an message at a specific priority or null
        /// </summary>
        /// <param name="specificPriority"></param>
        /// <returns></returns>
        private MvmMessage DequeueSpecific(int specificPriority)
        {
            MvmMessage message;
            lock (priorityQueue)
            {
                message = this.priorityQueue.DequeueSpecific(specificPriority);
        }
            if (message == null) return null;
            return message;
        }

        /// <summary>
        /// Dequeues the next item or null
        /// </summary>
        /// <param name="specificPriority"></param>
        /// <returns></returns>
        private MvmMessage DequeueNext()
        {
            MvmMessage message;
            lock (priorityQueue)
            {
                message = this.priorityQueue.Dequeue();
        }
            if (message == null) return null;
            return message;
        }
     
        /// <summary>
        /// Peeks at the next message or null
        /// </summary>
        /// <param name="specificPriority"></param>
        /// <returns></returns>
        private MvmMessage PeekNext()
        {
            lock (priorityQueue)
            {
                MvmMessage message = this.priorityQueue.Peek();
                return message;
            }
        }

        #endregion

        #region Track Max Priority Notifications
        // Tracks what the other side thinks our max priority is.
        private int? lastNotifiedPriority = null;

        private void UpdateMaxPriorityMessage()
        {
            var nextMsg = PeekNext();
            if (nextMsg != null)
            {
                lastNotifiedPriority = nextMsg.Priority;
                this.messageSender.SendImmediate(new NotifyMaxPriorityMessage(lastNotifiedPriority.Value));
            }
            else
            {
                lastNotifiedPriority = null;
            }
        }
        #endregion

        #region Adding messages to the outbox

        /// <summary>
        /// Queues up messages to send. Can be called by any thread that wants to send a message.
        /// If you add something at a higher priority then last notified priority, then you need
        /// to renotify.
        /// </summary>
        /// <param name="message"></param>
        public void Add(MvmMessage message, int priority)
        {
            // first get permission to add the message, if we are over the
            // max outbox size this will block.
            this.IncrementCurOutboxBytes(message);
            
            // now we that we have permission, make sure no one else is adding or sending
            // and add the message to the outbox.
            lock (this.addOrSendMutex)
            {
                EnqueueMessage(message, priority);
                if (lastNotifiedPriority == null || priority > lastNotifiedPriority.Value)
                {
                    lastNotifiedPriority = priority;
                    this.messageSender.SendImmediate(new NotifyMaxPriorityMessage(lastNotifiedPriority.Value));
                }
            }
        }

        public void Add(MvmMessage message)
        {
            this.Add(message, message.Priority);
        }

        #endregion

        # region Sending messages over the network
        public int maxInteruptMessageBatch=1000;
        public readonly bool enableBundling = true;

        /// <summary>
        /// Dequeue the highest priority message over the passed minimum priority or null.
        /// </summary>
        /// <param name="minPriority"></param>
        /// <returns></returns>
        public bool SendItemOverMinPriority(int minPriority)
        {
            MvmMessage theMessage = null;
            lock (this.addOrSendMutex)
            {
                var msg1 = DequeueMin(minPriority);
                if (msg1 == null) return false;
                bool madeBundle=false;
                    // see if we can send a message bundle
                    if (enableBundling && minPriority >= MessagePriority.Interupt)
                    {
                    var msg2 = DequeueMin(minPriority);
                        if (msg2 != null)
                        {
                        madeBundle=true;
                            List<MvmMessage> bundledMessages = new List<MvmMessage>(this.maxInteruptMessageBatch);
                            bundledMessages.Add(msg1);
                            bundledMessages.Add(msg2);
                            while (bundledMessages.Count<this.maxInteruptMessageBatch)
                            {
                            var msgN = DequeueMin(minPriority);
                                if (msgN == null) break;
                                bundledMessages.Add(msgN);
                            }
                        theMessage = new BundleMessage(bundledMessages);
                        }
                    }
                // if we did not make a bundle the message is msg1
                if(!madeBundle){
                    theMessage = msg1;
                }

                // send theMessage
                this.messageSender.SendImmediate(theMessage);
                    UpdateMaxPriorityMessage();
            }
            // now that we are outside of addOrSendMutex, let the gatekeeper know about
            // the message we've sent so it can let waiting thread put new messages
            // in the outbox.
            this.DecrementCurOutboxBytes(theMessage);
                    return true;
                }

        /// <summary>
        /// Dequeues a message at a specific priority or null
        /// </summary>
        /// <param name="specificPriority"></param>
        /// <returns></returns>
        public bool SendItemWithSpecificPriority(int specificPriority)
        {
            MvmMessage theMessage = null;
            lock (this.addOrSendMutex)
            {
                theMessage = DequeueSpecific(specificPriority);
                if (theMessage == null)
                {
                    var notFoundMsg = new SpecificNotFoundMessage(specificPriority);
                    this.messageSender.SendImmediate(notFoundMsg);
                    return false;
                }
                this.messageSender.SendImmediate(theMessage);
                UpdateMaxPriorityMessage();
            }
            // now that we are outside of addOrSendMutex, let the gatekeeper know about
            // the message we've sent so it can let waiting thread put new messages
            // in the outbox.
            this.DecrementCurOutboxBytes(theMessage);
            return true;
        }


        /// <summary>
        /// Dequeues a message at a any priority and responds with no work if that is the case
        /// </summary>
        /// <param name="specificPriority"></param>
        /// <returns></returns>
        public bool SendAnyItem()
        {
            //logger.Debug("Got request from {0} to send anything", this.clientNodeId);
            MvmMessage theMessage = null; 
            lock (this.addOrSendMutex)
            {
                theMessage =  DequeueNext();
                if (theMessage == null)
                {
                    //logger.Debug("Sending not found message to node={0}", this.clientNodeId);
                    var notFoundMsg = new AnyNotFoundMessage();
                    this.messageSender.SendImmediate(notFoundMsg);
                    return false;
                }
                //logger.Info("Sending real message to node={0}", this.clientNodeId);
                this.messageSender.SendImmediate(theMessage);
                UpdateMaxPriorityMessage();
            }
            // now that we are outside of addOrSendMutex, let the gatekeeper know about
            // the message we've sent so it can let waiting thread put new messages
            // in the outbox.
            this.DecrementCurOutboxBytes(theMessage);
            return true;
        }
        #endregion
    }
}
