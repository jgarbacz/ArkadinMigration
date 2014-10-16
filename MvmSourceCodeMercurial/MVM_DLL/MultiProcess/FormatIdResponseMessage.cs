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
    public class FormatIdResponseMessage : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.FormatIdResponseMessage; }
        }

        public long workId;
        public int formatId;
        public string machine;
        public int port;

        public FormatIdResponseMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public FormatIdResponseMessage(long workId,int formatId)
            : base()
        {
            this.workId = workId;
            this.formatId=formatId;
        }

        protected FormatIdResponseMessage(long messageId, long workId,int formatId)
            : base(messageId)
        {
            this.workId = workId;
            this.formatId = formatId;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return FormatIdResponseMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 25;


        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            int formatId = breader.ReadInt32();
            FormatIdResponseMessage msg = new FormatIdResponseMessage(messageId,workId, formatId);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.workId);
            bwriter.Write(this.formatId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            Dictionary<string, string> outputs = new Dictionary<string, string>();
            outputs["format_id"] = this.formatId.ToString();
            receiver.mvm.remoteWorkMgr.UpdateWorkStatus(this.workId, WorkStatus.Complete, outputs);
        }
    }
}
