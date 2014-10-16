using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
namespace MVM
{

    /// <summary>
    /// This message requests port/machine for a node id. 
    /// </summary>
    public class PortRequestMessage : MvmMessage
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public override int Priority
        {
            get { return MessagePriority.PortRequestMessage; }
        }

        public PortRequestMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public PortRequestMessage(long workId,string machine)
            : base()
        {
            this.workId = workId;
            this.machine = machine;
        }

        protected PortRequestMessage(long messageId, long workId, string machine)
            : base(messageId)
        {
            this.workId = workId;
            this.machine = machine;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return PortRequestMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 29;

        public long workId;
        public string machine;

        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            string machine = breader.ReadString();
            PortRequestMessage msg = new PortRequestMessage(messageId, workId, machine);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.workId);
            bwriter.Write(this.machine);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            int port=receiver.mvm.mvmClusterSuper.GetFreePort(receiver.clientNodeId.ToInt(),this.machine);
            PortResponseMessage response = new PortResponseMessage(this.workId,port);
            //logger.Debug("responding to slave port request with workId={0} port={1}", response.workId, response.port);
            receiver.messageOutbox.Add(response);
        }
    }
}
