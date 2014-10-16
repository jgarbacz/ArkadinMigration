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
    /// The profiler node sends this message to tell other nodes to stop their profiler threads
    /// </summary>
    public class ProfilerStopMessage : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.InteruptProcCall; }
        }

        public ProfilerStopMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        /// <summary>
        /// Constructor for a new message
        /// </summary>
        /// <param name="indexName"></param>
        public ProfilerStopMessage()
            : base()
        {
        }

        /// <summary>
        /// Constructor for deserialized message
        /// </summary>
        /// <param name="messageId"></param>
        protected ProfilerStopMessage(long messageId)
            : base(messageId)
        {
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return ProfilerStopMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 44;

        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            return new ProfilerStopMessage(messageId);
        }

        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
        }

        public override void ProcessMessage(MessageReceiver receiver)
        {
            receiver.mvmCluster.profilerActive = false;
        }
    }
}
