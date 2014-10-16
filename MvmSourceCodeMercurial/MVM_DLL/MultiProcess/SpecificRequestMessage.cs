using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{

    /// <summary>
    /// Requests a message with a specific priority. The receiver will
    /// respond with a matching message or SpecificNotFoundMessage.
    /// </summary>
    public class SpecificRequestMessage : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.SpecificRequestMessage; }
        }
        
        public int SpecificPriority { get; set; }

        public SpecificRequestMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public SpecificRequestMessage(int minPriority)
            : base()
        {
            this.SpecificPriority=minPriority;
        }

        protected SpecificRequestMessage(long messageId,int specificPriority)
            : base(messageId)
        {
            this.SpecificPriority = specificPriority;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return SpecificRequestMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType =16;


        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            int minPriority = breader.ReadInt32();
            SpecificRequestMessage msg = new SpecificRequestMessage(messageId, minPriority);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.SpecificPriority);
        }

        public override void ProcessMessage(MessageReceiver receiver)
        {
            bool isSent = receiver.messageOutbox.SendItemWithSpecificPriority(this.SpecificPriority);
        }
    }
}
