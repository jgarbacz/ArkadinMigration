using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
using System.Threading;

namespace MVM
{
    /// <summary>
    /// The profiler node sends this message to tell other nodes to start their profiler threads
    /// </summary>
    public class ProfilerStartMessage : MvmMessage
    {
        public int samplingPeriod;
        public int reportingCount;

        public override int Priority
        {
            get { return MessagePriority.InteruptProcCall; }
        }

        public ProfilerStartMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        /// <summary>
        /// Constructor for a new message
        /// </summary>
        /// <param name="indexName"></param>
        public ProfilerStartMessage(int samplingPeriod, int reportingCount)
            : base()
        {
            this.samplingPeriod = samplingPeriod;
            this.reportingCount = reportingCount;
        }

        /// <summary>
        /// Constructor for deserialized message
        /// </summary>
        /// <param name="messageId"></param>
        protected ProfilerStartMessage(long messageId, int samplingPeriod, int reportingCount)
            : base(messageId)
        {
            this.samplingPeriod = samplingPeriod;
            this.reportingCount = reportingCount;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return ProfilerStartMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 43;

        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            int samplingPeriod = breader.ReadInt32();
            int reportingCount = breader.ReadInt32();
            return new ProfilerStartMessage(messageId, samplingPeriod, reportingCount);
        }

        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.samplingPeriod);
            bwriter.Write(this.reportingCount);
        }

        // This message will start the profiler thread on this worker node
        public override void ProcessMessage(MessageReceiver receiver)
        {
            if (this.samplingPeriod >= 0)
            {
                receiver.mvmCluster.profilerThread.samplingPeriod = this.samplingPeriod;
            }
            if (this.reportingCount >= 0)
            {
                receiver.mvmCluster.profilerThread.reportingCount = this.reportingCount;
            }
            receiver.mvmCluster.profilerActive = true;
        }
    }
}
