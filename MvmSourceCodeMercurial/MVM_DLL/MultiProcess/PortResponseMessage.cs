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
    public class PortResponseMessage : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.PortResponseMessage; }
        }

        public long workId;
        public int port;

        public PortResponseMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public PortResponseMessage(long workId,int port)
            : base()
        {
            this.workId = workId;
            this.port=port;
        }

        protected PortResponseMessage(long messageId, long workId, int port)
            : base(messageId)
        {
            this.workId = workId;
            this.port = port;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return PortResponseMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 30;


        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            int port = breader.ReadInt32();
            PortResponseMessage msg = new PortResponseMessage(messageId,workId, port);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.workId);
            bwriter.Write(this.port);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            Dictionary<string, string> outputs = new Dictionary<string, string>();
            outputs["port"] = this.port.ToString();
            receiver.mvm.remoteWorkMgr.UpdateWorkStatus(this.workId, WorkStatus.Complete, outputs);
        }
    }
}
