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
    public class NodeIdInfoRequestMessage : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.NodeIdInfoRequestMessage; }
        }

       

        public NodeIdInfoRequestMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public NodeIdInfoRequestMessage(long workId,int nodeId)
            : base()
        {
            this.workId = workId;
            this.nodeId = nodeId;
        }

        protected NodeIdInfoRequestMessage(long messageId, long workId, int nodeId)
            : base(messageId)
        {
            this.workId = workId;
            this.nodeId = nodeId;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return NodeIdInfoRequestMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 11;


        public long workId;
        public int nodeId;

        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            int nodeId = breader.ReadInt32();
            NodeIdInfoRequestMessage msg = new NodeIdInfoRequestMessage(messageId,workId, nodeId);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.workId);
            bwriter.Write(this.nodeId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            MvmClusterNode node = receiver.mvm.mvmClusterSuper.GetClusterNode(this.nodeId);
            NodeIdInfoResponseMessage response = new NodeIdInfoResponseMessage(this.workId,node.nodeId, node.machine, node.port);
            receiver.messageOutbox.Add(response);
        }
    }
}
