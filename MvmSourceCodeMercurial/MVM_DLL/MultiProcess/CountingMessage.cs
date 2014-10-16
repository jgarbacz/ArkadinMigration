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
    public class CountingMessage : MvmMessage
    {
        public static Logger nlogger = LogManager.GetLogger("slave");
        public int ctr;
        public int max;

        public override int Priority
        {
            get { return MessagePriority.Log; }
        }

        public CountingMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        /// <summary>
        /// Constructor for new message
        /// </summary>
        /// <param name="logMsg"></param>
        public CountingMessage(int ctr, int max):base()
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
        protected CountingMessage(long messageId, int ctr, int max)
            : base(messageId)
        {
            this.ctr = ctr;
            this.max = max;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return CountingMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 22;

        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            int ctr = breader.ReadInt32();
            int max = breader.ReadInt32();
            CountingMessage msg = new CountingMessage(messageId, ctr,max);
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

            // simulate slave end sleep.
            //if (this.ctr % 2 == 1)System.Threading.Thread.Sleep(100);

            //Console.WriteLine("" + DateTime.Now + " ctr=" + this.ctr);
            if (this.ctr < this.max)
            {
                CountingMessage msg = new CountingMessage(ctr, max);
                receiver.messageSender.SendImmediate(msg);
            }
            else
            {
                Console.WriteLine("-------------- TestMessageSpeed end:" + DateTime.Now+ ", total messages="+ctr);
            }
        }

    }
}
