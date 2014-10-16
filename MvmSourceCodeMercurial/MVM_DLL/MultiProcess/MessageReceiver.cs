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
    /// This class/thread is dedicated to receiving message on the socket.
    /// </summary>
    public class MessageReceiver
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        #region Mapping of messageTypes to their handlers
        public static readonly Dictionary<byte, MvmMessage> messageHanders = new Dictionary<byte, MvmMessage>();
        static MessageReceiver()
        {
            // need to make sure these are assigned consistently.
            messageHanders[1] = new NotifyMaxPriorityMessage(null as MvmMessageDeserializer);
            messageHanders[2] = new QueueProcMessage(null as MvmMessageDeserializer);
            messageHanders[3] = new QueueProcResultsMessage(null as MvmMessageDeserializer);
            messageHanders[4] = new RequestMessage(null as MvmMessageDeserializer);
            messageHanders[5] = new LogMessage(null as MvmMessageDeserializer);
            //messageHanders[6] = new WorkStatusUpdateMessage(null as MvmMessageDeserializer);
            messageHanders[7] = new PushIndexMessage(null as MvmMessageDeserializer);
            messageHanders[8] = new SendGlobalObjectMessage(null as MvmMessageDeserializer);
            messageHanders[9] = new GetSlaveNodeIdsRequestMessage(null as MvmMessageDeserializer);
            messageHanders[10] = new GetSlaveNodeIdsResponseMessage(null as MvmMessageDeserializer);
            messageHanders[11] = new NodeIdInfoRequestMessage(null as MvmMessageDeserializer);
            messageHanders[12] = new NodeIdInfoResponseMessage(null as MvmMessageDeserializer);
            messageHanders[13] = new AnyRequestMessage(null as MvmMessageDeserializer);
            messageHanders[14] = new AnyNotFoundMessage(null as MvmMessageDeserializer);
            messageHanders[15] = new ShutdownAbortMessage(null as MvmMessageDeserializer);
            messageHanders[16] = new SpecificRequestMessage(null as MvmMessageDeserializer);
            messageHanders[17] = new SpecificNotFoundMessage(null as MvmMessageDeserializer);
            messageHanders[18] = new ShutdownRequest(null as MvmMessageDeserializer);
            messageHanders[19] = new ShutdownResponse(null as MvmMessageDeserializer);
            messageHanders[20] = new DisconnectRequest(null as MvmMessageDeserializer);
            //messageHanders[21] = new HungryMessage(null as MvmMessageDeserializer);
            messageHanders[22] = new CountingMessage(null as MvmMessageDeserializer);
            messageHanders[23] = new OutboxCountingMessage(null as MvmMessageDeserializer);
            messageHanders[24] = new FormatIdRequestMessage(null as MvmMessageDeserializer);
            messageHanders[25] = new FormatIdResponseMessage(null as MvmMessageDeserializer);
            messageHanders[26] = new FormatFieldsRequestMessage(null as MvmMessageDeserializer);
            messageHanders[27] = new FormatFieldsResponseMessage(null as MvmMessageDeserializer);
            messageHanders[28] = new BundleMessage(null as MvmMessageDeserializer);
            messageHanders[29] = new PortRequestMessage(null as MvmMessageDeserializer);
            messageHanders[30] = new PortResponseMessage(null as MvmMessageDeserializer);
            messageHanders[31] = new UsageRequestMessage(null as MvmMessageDeserializer);
            messageHanders[32] = new UsageResponseMessage(null as MvmMessageDeserializer);
            messageHanders[33] = new SlaveInitMessage(null as MvmMessageDeserializer);
            //messageHanders[34] = new ReallocateSlavesMessage(null as MvmMessageDeserializer);
            messageHanders[35] = new UpdateClusterInfoMessage(null as MvmMessageDeserializer);
            messageHanders[36] = new DbInfoRequestMessage(null as MvmMessageDeserializer);
            messageHanders[37] = new DbInfoResponseMessage(null as MvmMessageDeserializer);
            messageHanders[38] = new RcnRequestMessage(null as MvmMessageDeserializer);
            messageHanders[39] = new RcnResponseMessage(null as MvmMessageDeserializer);
            messageHanders[40] = new TfidUfnRequestMessage(null as MvmMessageDeserializer);
            messageHanders[41] = new TfidUfnResponseMessage(null as MvmMessageDeserializer);
            messageHanders[42] = new ProfilerInitMessage(null as MvmMessageDeserializer);
            messageHanders[43] = new ProfilerStartMessage(null as MvmMessageDeserializer);
            messageHanders[44] = new ProfilerStopMessage(null as MvmMessageDeserializer);
            messageHanders[45] = new ProfilerStatusMessage(null as MvmMessageDeserializer);
            messageHanders[46] = new UpdateNodeMessage(null as MvmMessageDeserializer);

            // make sure our indexes are correct
            foreach (var entry in messageHanders)
            {
                if (entry.Key != entry.Value.MessageType) throw new Exception("Coding error, mismatched message types for class:" + entry.Value.GetType().Name);
            }
        }
        #endregion

        public volatile int numRowsRequested = 0;

        public readonly SocketHandler socketHandler;
        public readonly string clientNodeId;
        public readonly string myNodeId;
        public readonly MvmEngine mvm;
        public readonly TcpClient clientSocket;
        public readonly NetworkStream clientNetworkStream;
        private readonly BinaryReader breader;
        public Thread messageReceiverThread;
        public readonly string messagReceiverThreadName;
        public MessageReceiver(SocketHandler socketHandler)
        {
            // Setup local references
            this.socketHandler = socketHandler;
            this.clientNodeId = this.socketHandler.clientNodeId;
            this.myNodeId = this.socketHandler.myNodeId;
            this.mvm = this.socketHandler.mvm;
            this.clientSocket = this.socketHandler.clientSocket;
            this.clientNetworkStream = this.clientSocket.GetStream();
            this.messagReceiverThreadName = "R" + this.clientNodeId + (this.socketHandler.isServer ? "s" : "c");
            this.breader = this.socketHandler.breader;
        }

        // shortcuts
        public GlobalContext globalContext { get { return this.mvm.workMgr.globalContext; } }
        public MessageSender messageSender { get { return this.socketHandler.messageSender; } }
        public MessageOutbox messageOutbox { get { return this.socketHandler.messageOutbox; } }
        public MvmClusterBase mvmCluster { get { return this.mvm.mvmCluster; } }
        public MvmClusterSuper mvmClusterSuper { get { return this.mvm.mvmClusterSuper; } }

        /// <summary>
        /// Start receiving messages in a separate thread
        /// </summary>
        public void StartReceiving()
        {
            this.messageReceiverThread = new Thread(Start);
            this.messageReceiverThread.Name = this.messagReceiverThreadName;
            this.messageReceiverThread.Start();
        }

        private object lock_MaxWaitingPriority = new object();
        private int? unsafe_MaxWaitingPriority;
        public int? MaxWaitingPriority
        {
            get
            {
                lock (lock_MaxWaitingPriority)
                {
                    return unsafe_MaxWaitingPriority;
                }
            }
            set
            {
                lock (lock_MaxWaitingPriority)
                {
                    unsafe_MaxWaitingPriority = value;
                }
            }
        }

        # region Overide the interupt priority

        public List<int> interuptOverridePriorities = new List<int>();
        public void AddInteruptPriorityOverride(int minPriority)
        {
            lock (interuptOverridePriorities)
            {
                interuptOverridePriorities.Add(minPriority);
                interuptOverridePriorities.Sort();
            }
        }
        public int? GetMinOverridePriority()
        {
            lock (interuptOverridePriorities)
            {
                if (interuptOverridePriorities.Count == 0) return null;
                int minOverridePriority = interuptOverridePriorities[0];
                return minOverridePriority;
            }
        }
        public void ConsumeMinOverridePriority(int priority)
        {
            lock (interuptOverridePriorities)
            {
                interuptOverridePriorities.Remove(priority);
            }
        }
        # endregion

        /// <summary>
        /// Thread Receiver thread. 
        /// </summary>
        private void Start()
        {
            //logger.Debug("started receiving...");
            while (!this.Stop)
            {
                try
                {
                    byte messageType = MvmMessage.GetMessageType(this.breader);
                    if (this.Stop)
                    {
                        //logger.Trace("MessageReciever thread {0} exiting", this.messageReceiverThread.Name);
                        return;
                    }
                    //logger.Trace("Got message of type {0} = {1}", messageType, messageHanders.ContainsKey(messageType)?messageHanders[messageType].GetType().ToString():"INVALIDTYPE-"+messageType);
                    MvmMessage messageHandler;
                    if (!messageHanders.TryGetValue(messageType, out messageHandler)) throw new Exception("Invalid messageType=" + messageType);
                    MvmMessage message = messageHandler.Deserialize(this, this.breader);
                    //logger.Trace("process msg from {0}: {1}", this.clientNodeId, message.GetType().Name);
                    message.ProcessMessage(this);
                    if (this.Stop)
                    {
                        //logger.Debug("MessageReciever thread {0} exiting", this.messageReceiverThread.Name);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    // Ignore any spurious errors from slaves closing their connections when we're aborting
                    if (!this.mvm.shutdownAbortInProcess)
                    {
                        string errMsg = "Error receiving messages: " + ex.Message.AppendLine(ex.GetStackTraceRecursive());
                        logger.Fatal(errMsg);
                        this.mvm.FlushNLog();
                        this.mvm.ShutdownAbort(errMsg);
                        this.mvm.workMgr.WorkerThreadsAbort(null);
                        return;
                    }
                }
            }
        }

        public void StopReceiving()
        {
            //logger.Debug("Set a flag on the receiver no longer processes messages.");
            this.Stop = true;
        }

        public void RequestAnyWork()
        {
            //logger.Debug("Requesting anything from {0}" ,this.clientNodeId);
            this.messageSender.SendImmediate(new AnyRequestMessage());
            this.MaxWaitingPriority = null;
        }

        private bool _stop = false;
        private object _stop_lock = new object();
        public bool Stop
        {
            get
            {
                lock (this._stop_lock)
                {
                    return this._stop;
                }
            }
            private set
            {
                lock (this._stop_lock)
                {
                    this._stop = value;
                }
            }
        }
    }

}
