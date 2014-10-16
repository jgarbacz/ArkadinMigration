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
    public class NodeIdInfoResponseMessage : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.NodeIdInfoResponseMessage; }
        }

        public long workId;
        public int nodeId;
        public string machine;
        public int port;

        public NodeIdInfoResponseMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public NodeIdInfoResponseMessage(long workId,int nodeId,string machine, int port)
            : base()
        {
            this.workId = workId;
            this.nodeId=nodeId;
            this.machine = machine;
            this.port = port;
        }

        protected NodeIdInfoResponseMessage(long messageId, long workId,int nodeId, string machine, int port)
            : base(messageId)
        {
            this.workId = workId;
            this.nodeId = nodeId;
            this.machine = machine;
            this.port = port;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return NodeIdInfoResponseMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 12;


        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            int nodeId = breader.ReadInt32();
            string machine = breader.ReadString();
            int port = breader.ReadInt32();
            NodeIdInfoResponseMessage msg = new NodeIdInfoResponseMessage(messageId, workId,nodeId, machine, port);
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
            bwriter.Write(this.machine);
            bwriter.Write(this.port);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            Dictionary<string, string> outputs = new Dictionary<string, string>();
            outputs["node_id"] = this.nodeId.ToString();
            outputs["server"] = this.machine;
            outputs["port"] = this.port.ToString();
            receiver.mvm.remoteWorkMgr.UpdateWorkStatus(this.workId, WorkStatus.Complete, outputs);
        }
    }
}
