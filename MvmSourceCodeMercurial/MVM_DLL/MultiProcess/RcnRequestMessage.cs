using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
namespace MVM
{

    /// <summary>
    /// This message requests port/batchSize for a node id. 
    /// </summary>
    public class RcnRequestMessage : MvmMessage
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public override int Priority
        {
            get { return MessagePriority.RcnRequestMessage; }
        }

        public RcnRequestMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public RcnRequestMessage(long workId,int batchSize)
            : base()
        {
            this.workId = workId;
            this.batchSize = batchSize;
        }

        protected RcnRequestMessage(long messageId, long workId, int batchSize)
            : base(messageId)
        {
            this.workId = workId;
            this.batchSize = batchSize;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return RcnRequestMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 38;

        public long workId;
        public int batchSize;

        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            int batchSize = breader.ReadInt32();
            RcnRequestMessage msg = new RcnRequestMessage(messageId, workId, batchSize);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.workId);
            bwriter.Write(this.batchSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            long responseBatchStart;
            int responseBatchSize;
            receiver.mvm.mvmClusterSuper.GetRcnBatch(this.batchSize,out responseBatchStart, out responseBatchSize);
            RcnResponseMessage response = new RcnResponseMessage(this.workId, responseBatchStart,responseBatchSize);
            //logger.Debug("responding to slave port request with workId={0} batchstart={1} batchsize={2}", response.workId, responseBatchStart, responseBatchSize);
            receiver.messageOutbox.Add(response);
        }
    }
}
