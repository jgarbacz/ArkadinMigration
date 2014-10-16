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
    public class FormatFieldsRequestMessage : MvmMessage
    {
        public static Logger logger=LogManager.GetCurrentClassLogger();
        public override int Priority
        {
            get { return MessagePriority.FormatFieldsRequestMessage; }
        }
        public FormatFieldsRequestMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public FormatFieldsRequestMessage(long workId,int formatId)
            : base()
        {
            this.workId = workId;
            this.formatId = formatId;
        }

        protected FormatFieldsRequestMessage(long messageId, long workId, int formatId)
            : base(messageId)
        {
            this.workId = workId;
            this.formatId = formatId;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return FormatFieldsRequestMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 26;

        public long workId;
        public int formatId;

        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            int formatId = breader.ReadInt32();
            FormatFieldsRequestMessage msg = new FormatFieldsRequestMessage(messageId, workId, formatId);
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
            string[] formatFields=receiver.mvm.mvmClusterSuper.GetFormatFields(this.formatId);
            FormatFieldsResponseMessage response = new FormatFieldsResponseMessage(this.workId, formatFields);
            //logger.Debug("sending formatFIeldsResponse with workId={0},formatId={1},formatFields.Length={2},formatFields={3}", response.workId,this.formatId,formatFields.Length,formatFields.JoinStrings(","));
            receiver.messageOutbox.Add(response);
        }
    }
}
