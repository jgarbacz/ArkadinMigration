using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{
    /// <summary>
    /// Message to queue a proc remotely
    /// </summary>
    public class SendGlobalObjectMessage : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.PushIndex; }
        }

        public SendGlobalObjectMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public SendGlobalObjectMessage(IObjectData objectData)
            : base()
        {
            this.serializedObjectData = objectData.Serialize();
        }

        protected SendGlobalObjectMessage(long messageId, IObjectData objectData)
            : base(messageId)
        {
            this.objectData = objectData;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return SendGlobalObjectMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 8;

        
        public IObjectData objectData;
        public byte[] serializedObjectData;

        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            IObjectData objectData = receiver.mvm.objectDataSerializer.Deserialize(breader);
            return new SendGlobalObjectMessage(messageId,objectData);
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.serializedObjectData);
        }

        /// <summary>
        /// Add the deserialized object to the object cache
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            receiver.mvm.objectCache.AddOrOverwriteObject(this.objectData);
        }
    }
}
