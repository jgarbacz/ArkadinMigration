using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{
    /// <summary>
    /// Requests a message with a specific priority. The receiver will
    /// respond with a matching message or StopReceivingMessage.
    /// </summary>
    public class StopReceivingMessage : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.StopReceivingMessage; }
        }
        
     
        public StopReceivingMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public StopReceivingMessage()
            : base()
        {
        }

        protected StopReceivingMessage(long messageId)
            : base(messageId)
        {
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return StopReceivingMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 20;


        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            StopReceivingMessage msg = new StopReceivingMessage(messageId);
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
            throw new StopReceivingException();
        }
    }

    public class StopReceivingException : Exception
    {
    }
}
