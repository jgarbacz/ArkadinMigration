using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MVM
{

    /// <summary>
    /// This message requests batchStart/machine for a node id. 
    /// </summary>
    public class RcnResponseMessage : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.RcnResponseMessage; }
        }

        public long workId;
        public long batchStart;
        public int batchSize;

        public RcnResponseMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public RcnResponseMessage(long workId, long batchStart, int batchSize)
            : base()
        {
            this.workId = workId;
            this.batchStart=batchStart;
            this.batchSize = batchSize;
        }

        protected RcnResponseMessage(long messageId, long workId, long batchStart, int batchSize)
            : base(messageId)
        {
            this.workId = workId;
            this.batchStart = batchStart;
            this.batchSize = batchSize;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return RcnResponseMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 39;


        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            long batchStart = breader.ReadInt64();
            int batchSize = breader.ReadInt32();
            RcnResponseMessage msg = new RcnResponseMessage(messageId, workId, batchStart, batchSize);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.workId);
            bwriter.Write(this.batchStart);
            bwriter.Write(this.batchSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            Tuple<long, int> outputs = new Tuple<long, int>(this.batchStart, this.batchSize);
            receiver.mvm.remoteWorkMgr.UpdateWorkStatus(this.workId, WorkStatus.Complete, outputs);
        }
    }
}
