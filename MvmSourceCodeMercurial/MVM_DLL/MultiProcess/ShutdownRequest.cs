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
    public class ShutdownRequest : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.ShutdownRequest; }
        }

        public ShutdownRequest(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public ShutdownRequest(long workId,int nodeId)
            : base()
        {
            this.workId = workId;
            this.nodeId = nodeId;
        }

        protected ShutdownRequest(long messageId, long workId, int nodeId)
            : base(messageId)
        {
            this.workId = workId;
            this.nodeId = nodeId;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return ShutdownRequest.StaticMessageType; } }
        public static readonly byte StaticMessageType = 18;


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
            ShutdownRequest msg = new ShutdownRequest(messageId, workId, nodeId);
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
            receiver.mvmCluster.Shutdown(this.workId);
        }
    }
}
