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
    public class QueueProcResultsMessage : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.QueueProcResultsMessage; }
        }

        public QueueProcResultsMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public QueueProcResultsMessage(long workId, WorkStatus workStatus, IDictionary<string, string> outputs)
            : base()
        {
            this.workId = workId;
            this.workStatus = workStatus;
            this.outputs = outputs;
        }

        protected QueueProcResultsMessage(long messageId, WorkStatus workStatus, long workId, IDictionary<string, string> outputs)
            : base(messageId)
        {
            this.workId = workId;
            this.workStatus = workStatus;
            this.outputs = outputs;
        }

        public override void ProcessMessage(MessageReceiver receiver)
        {
            receiver.mvm.remoteWorkMgr.UpdateWorkStatus(this.workId,this.workStatus,this.outputs);
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return QueueProcResultsMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 3;

        public long workId;
        public WorkStatus workStatus;
        public IDictionary<string, string> outputs;

        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId,  BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            WorkStatus workStatus = (WorkStatus)breader.Read7BitEncodedInt();
            IDictionary<string, string> outputs = new Dictionary<string, string>().Deserialize(breader);
            QueueProcResultsMessage msg = new QueueProcResultsMessage(messageId, workStatus, workId, outputs);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.workId);
            bwriter.Write7BitEncodedInt((int)this.workStatus);
            this.outputs.Serialize(bwriter);
        }
    }
}
