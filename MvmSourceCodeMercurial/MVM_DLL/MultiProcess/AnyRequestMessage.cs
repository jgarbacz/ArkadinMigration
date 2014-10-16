using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{

    /// <summary>
    /// Requests anything but expect a result either way.
    /// </summary>
    public class AnyRequestMessage : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.AnyRequestMessage; }
        }
        
       
        public AnyRequestMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public AnyRequestMessage()
            : base()
        {
        }

        protected AnyRequestMessage(long messageId)
            : base(messageId)
        {
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return AnyRequestMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 13;


        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            AnyRequestMessage msg = new AnyRequestMessage(messageId);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
        }

        public override void ProcessMessage(MessageReceiver receiver)
        {
            bool isSent = receiver.messageOutbox.SendAnyItem();
        }
    }
}
