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
    public class GetSlaveNodeIdsResponseMessage : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.GetSlaveNodeIdsResponseMessage; }
        }
        
       
        public GetSlaveNodeIdsResponseMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public GetSlaveNodeIdsResponseMessage(long workId, List<MvmClusterNode> slaveNodes)
            : base()
        {
            this.workId = workId;
            this.slaveNodes = slaveNodes;
        }

        protected GetSlaveNodeIdsResponseMessage(long messageId, long workId, List<MvmClusterNode> slaveNodes)
            : base(messageId)
        {
            this.workId = workId;
            this.slaveNodes = slaveNodes;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return GetSlaveNodeIdsResponseMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 10;
        public int requestCount;
        public List<MvmClusterNode> slaveNodes = new List<MvmClusterNode>();
        public long workId;
       
        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            int slaveCount = breader.ReadInt32();
            List<MvmClusterNode> slaveNodes = new List<MvmClusterNode>();
            for (int i = 0; i < slaveCount; i++)
            {
                MvmClusterNode node = new MvmClusterNode(receiver.mvmCluster, breader);
                slaveNodes.Add(node);
            }
            GetSlaveNodeIdsResponseMessage msg = new GetSlaveNodeIdsResponseMessage(messageId, workId, slaveNodes);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.workId);
            bwriter.Write(this.slaveNodes.Count);
            foreach (MvmClusterNode node in this.slaveNodes) node.Serialize(bwriter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            Dictionary<string, string> outputs = new Dictionary<string, string>();
            for (int i = 0; i < this.slaveNodes.Count; i++)
            {
                foreach (var node in this.slaveNodes)
                {
                    outputs[node.nodeId.ToString()] = node.nodeId.ToString();
                }
            }
            receiver.mvm.remoteWorkMgr.UpdateWorkStatus(this.workId, WorkStatus.Complete, outputs);
        }
    }
}
