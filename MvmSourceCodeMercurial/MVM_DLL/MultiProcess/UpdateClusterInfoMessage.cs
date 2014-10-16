using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
namespace MVM
{

    /// <summary>
    /// This pushes out information about the computing cluster. It should be send
    /// from super to all other nodes anytime a new node is added to the system so that
    /// other nodes can communicate with it.
    /// </summary>
    public class UpdateClusterInfoMessage : MvmMessage
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public override int Priority
        {
            get { return MessagePriority.UpdateClusterInfoMessage; }
        }

        public UpdateClusterInfoMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public UpdateClusterInfoMessage(List<Dictionary<string, string>> properties)
            : base()
        {
            this.properties=properties;
        }

        protected UpdateClusterInfoMessage(long messageId, List<Dictionary<string, string>> properties)
            : base(messageId)
        {
            this.properties = properties;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return UpdateClusterInfoMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 35;

        public List<Dictionary<string, string>> properties;

        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            List<Dictionary<string, string>> properties = breader.ReadListOfDictionaryOfStringString();
            UpdateClusterInfoMessage msg = new UpdateClusterInfoMessage(messageId, properties);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.properties);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            foreach (var dic in this.properties)
            {
                //receiver.mvm.Log("I JUST UPDATED node_id to " + dic["node_id"]);
                receiver.mvmCluster.AddOrUpdateKnownNode(dic);

                // When the profiler node receives this message in attach mode, it means
                // we've gotten full node info so we can tell everyone to start profiling.
                if (receiver.mvmCluster.NodeId == dic["node_id"].ToInt() &&
                    receiver.mvmCluster.IsProfilerNode &&
                    (receiver.mvmCluster as MvmClusterProfiler).attachToExistingMVM)
                {
                    (receiver.mvmCluster as MvmClusterProfiler).nodesReadyEvent.Set();
                }
            }
        }
    }
}
