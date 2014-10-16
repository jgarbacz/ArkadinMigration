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
    public class ShutdownResponse : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.ShutdownResponse; }
        }

        public long workId;
        public int nodeId;


        public ShutdownResponse(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public ShutdownResponse(long workId,int nodeId)
            : base()
        {
            this.workId = workId;
            this.nodeId=nodeId;
         
        }

        protected ShutdownResponse(long messageId, long workId,int nodeId)
            : base(messageId)
        {
            this.workId = workId;
            this.nodeId = nodeId;
            
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return ShutdownResponse.StaticMessageType; } }
        public static readonly byte StaticMessageType = 19;


        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            int nodeId = breader.ReadInt32();
            ShutdownResponse msg = new ShutdownResponse(messageId, workId,nodeId);
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
            Dictionary<string, string> outputs = new Dictionary<string, string>();
            receiver.mvm.remoteWorkMgr.UpdateWorkStatus(this.workId, WorkStatus.Complete, outputs);
        }
    }
}
