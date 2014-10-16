using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MVM
{

    /// <summary>
    /// This message requests a message of a min priority. 
    /// </summary>
    public class RequestMessage : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.RequestMessage; }
        }
        
        public int MinPriority { get; set; }

        public RequestMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public RequestMessage(int minPriority)
            : base()
        {
            this.MinPriority=minPriority;
        }

        protected RequestMessage(long messageId,int minPriority)
            : base(messageId)
        {
            this.MinPriority = minPriority;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return RequestMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 4;


        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            int minPriority = breader.ReadInt32();
            RequestMessage msg = new RequestMessage(messageId, minPriority);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.MinPriority);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            bool isSent = receiver.messageOutbox.SendItemOverMinPriority(this.MinPriority);
        }
    }
}
