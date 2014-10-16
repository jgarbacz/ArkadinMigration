using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MVM
{
    /// <summary>
    /// This message requests port/machine for a node id. 
    /// </summary>
    public class FormatFieldsResponseMessage : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.FormatFieldsResponseMessage; }
        }

        public long workId;
        public string[] formatFields;

        public FormatFieldsResponseMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public FormatFieldsResponseMessage(long workId,string[] formatFields)
            : base()
        {
            this.workId = workId;
            this.formatFields = formatFields;
        }

        protected FormatFieldsResponseMessage(long messageId, long workId,string[] formatFields)
            : base(messageId)
        {
            this.workId = workId;
            this.formatFields = formatFields;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return FormatFieldsResponseMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 27;

        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            string[] formatFields = breader.ReadArrayOfString();
            FormatFieldsResponseMessage msg = new FormatFieldsResponseMessage(messageId, workId,formatFields);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.workId);
            bwriter.Write(this.formatFields);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            //receiver.mvm.Log("update works status for " + this.workId +",formatFields.Count="+formatFields.Length+ ", formatFields=" + this.formatFields.Join(","));
            receiver.mvm.remoteWorkMgr.UpdateWorkStatus(this.workId, WorkStatus.Complete, this.formatFields);
        }
    }
}
