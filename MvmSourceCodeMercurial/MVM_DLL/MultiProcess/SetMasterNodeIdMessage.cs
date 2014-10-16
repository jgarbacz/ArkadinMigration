using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
namespace MVM
{
    /// <summary>
    /// Requests anything but expect a result either way.
    /// </summary>
    public class SetMasterNodeIdMessage : MvmMessage
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public override int Priority
        {
            get { return MessagePriority.SetMasterNodeIdMessage; }
        }

        public SetMasterNodeIdMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public SetMasterNodeIdMessage(int masterNodeId)
            : base()
        {
            this.masterNodeId = masterNodeId;
        }

        protected SetMasterNodeIdMessage(long messageId,int masterNodeId)
            : base(messageId)
        {
            this.masterNodeId = masterNodeId;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return SetMasterNodeIdMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 21;

        public int masterNodeId=-1;

        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            int nodeId = breader.ReadInt32();
            SetMasterNodeIdMessage msg = new SetMasterNodeIdMessage(messageId,nodeId);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(masterNodeId);
        }

        public override void ProcessMessage(MessageReceiver receiver)
        {
            logger.Info("Received SetMasterNodeId message from node {0}", receiver.socketHandler.clientNodeId);
            MvmClusterNode masterNode;
            if(receiver.mvmCluster.TryGetKnownNode(this.masterNodeId,out masterNode)){
                receiver.mvmCluster.MasterNode=masterNode;
            }else{
                throw new Exception("Error, cannot set master to an unknown node_id="+this.masterNodeId);
            }
        }
    }
}
