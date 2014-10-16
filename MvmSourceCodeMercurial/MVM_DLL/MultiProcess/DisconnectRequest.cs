using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
namespace MVM
{

    /// <summary>
    /// This message requests port/machine for a node id. 
    /// </summary>
    public class DisconnectRequest : MvmMessage
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public override int Priority
        {
            get { return MessagePriority.DisconnectRequest; }
        }

        public DisconnectRequest(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public DisconnectRequest()
            : base()
        {
        }

        protected DisconnectRequest(long messageId)
            : base(messageId)
        {
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return DisconnectRequest.StaticMessageType; } }
        public static readonly byte StaticMessageType = 20;

        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            DisconnectRequest msg = new DisconnectRequest(messageId);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            receiver.socketHandler.ReceiveDisconnectRequest();
        }
    }
}
