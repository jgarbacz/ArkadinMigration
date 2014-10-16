using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;

namespace MVM
{
    
    /// <summary>
    /// Message to queue a proc remotely
    /// </summary>
    public class NotifyMaxPriorityMessage : MvmMessage
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public int maxPriority;

        public override int Priority
        {
            get { return MessagePriority.NotifyMaxPriority; }
        }

        public NotifyMaxPriorityMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public NotifyMaxPriorityMessage(int maxPriority):base()
        {
            this.maxPriority = maxPriority;
        }

        protected NotifyMaxPriorityMessage(long messageId, int maxPriority)
            : base(messageId)
        {
            this.maxPriority = maxPriority;
        }

        public override void ProcessMessage(MessageReceiver receiver)
        {
            //logger.Debug("now maxpriority=" + this.maxPriority);
            // if this message can interupt get it now
            if (this.maxPriority >= MessagePriority.Interupt)
            {
                receiver.messageSender.SendImmediate(new RequestMessage(MessagePriority.Interupt));
                receiver.MaxWaitingPriority = null;
                return;
            }

            // Otherwise just update the receiver's max waiting priority
            receiver.MaxWaitingPriority = this.maxPriority;
            //logger.Debug("just update maxWaitingPriority to {0}", this.maxPriority.ToString());

            // If the process is idle, wake it up.
            receiver.mvm.workMgr.WakeUp();
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return NotifyMaxPriorityMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 1;


        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver,long messageId,  BinaryReader breader)
        {
            int maxPriority = breader.ReadInt32();
            NotifyMaxPriorityMessage msg = new NotifyMaxPriorityMessage(messageId, maxPriority);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.maxPriority);
        }

       

    }
}
