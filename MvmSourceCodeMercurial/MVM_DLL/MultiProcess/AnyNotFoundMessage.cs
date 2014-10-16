using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{
    /// <summary>
    /// Requests a message with a specific priority. The receiver will
    /// respond with a matching message or AnyNotFoundMessage.
    /// </summary>
    public class AnyNotFoundMessage : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.AnyNotFoundMessage; }
        }
        
     
        public AnyNotFoundMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public AnyNotFoundMessage()
            : base()
        {
        }

        protected AnyNotFoundMessage(long messageId)
            : base(messageId)
        {
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return AnyNotFoundMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 14;


        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            AnyNotFoundMessage msg = new AnyNotFoundMessage(messageId);
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
            // Wake up the engine...
            receiver.mvm.workMgr.WakeUp();
        }
    }
}
