using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
namespace MVM
{
     
    /// <summary>
    /// Placeholder class for messages that are requests.
    /// </summary>
    public class OutboxCountingMessage : MvmMessage
    {
        public static Logger nlogger = LogManager.GetLogger("slave");
        public int ctr;
        public int max;

        public override int Priority
        {
            get { return MessagePriority.Log; }
        }

        public OutboxCountingMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        /// <summary>
        /// Constructor for new message
        /// </summary>
        /// <param name="logMsg"></param>
        public OutboxCountingMessage(int ctr, int max):base()
        {
            this.ctr = ctr;
            this.max = max;
        }

        /// <summary>
        /// Constructor for deserialized message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="requestMessageId"></param>
        /// <param name="logMsg"></param>
        protected OutboxCountingMessage(long messageId, int ctr, int max)
            : base(messageId)
        {
            this.ctr = ctr;
            this.max = max;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return OutboxCountingMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 23;

        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            int ctr = breader.ReadInt32();
            int max = breader.ReadInt32();
            OutboxCountingMessage msg = new OutboxCountingMessage(messageId, ctr,max);
            return msg;
        }

        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.ctr);
            bwriter.Write(this.max);
        }

        public override void ProcessMessage(MessageReceiver receiver)
        {
            this.ctr++;
            Console.WriteLine("ctr=" + DateTime.Now + "- ctr=" + this.ctr);
            if (this.ctr < this.max)
            {
                OutboxCountingMessage msg = new OutboxCountingMessage(ctr, max);
                receiver.messageOutbox.Add(msg);
            }
            else
            {
                Console.WriteLine("-------------- TestOutboxSpeed end:" + DateTime.Now + ", total outbox messages=" + ctr);
            }
        }

    }
}
