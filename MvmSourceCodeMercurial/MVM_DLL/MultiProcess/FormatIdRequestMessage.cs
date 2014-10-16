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
    public class FormatIdRequestMessage : MvmMessage
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public override int Priority
        {
            get { return MessagePriority.FormatIdRequestMessage; }
        }

        public FormatIdRequestMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public FormatIdRequestMessage(long workId,string formatString)
            : base()
        {
            this.workId = workId;
            this.formatString = formatString;
        }

        protected FormatIdRequestMessage(long messageId, long workId, string formatString)
            : base(messageId)
        {
            this.workId = workId;
            this.formatString = formatString;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return FormatIdRequestMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 24;

        public long workId;
        public string formatString;

        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            string formatString = breader.ReadString();
            FormatIdRequestMessage msg = new FormatIdRequestMessage(messageId, workId, formatString);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.workId);
            bwriter.Write(this.formatString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            int formatId=receiver.mvm.mvmClusterSuper.GetFormatId(this.formatString);
            logger.Info("responding with workId={0} formatId={1}", this.workId, formatId);
            FormatIdResponseMessage response = new FormatIdResponseMessage(this.workId,formatId);
            logger.Info("responding with workId={0} formatId={1}", response.workId, response.formatId);
            receiver.messageOutbox.Add(response);
        }
    }
}
