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
    public class UsageRequestMessage : MvmMessage
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public override int Priority
        {
            get { return MessagePriority.UsageRequestMessage; }
        }

        public UsageRequestMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public UsageRequestMessage(long workId,List<string> idAccs,string hookId)
            : base()
        {
            this.workId = workId;
            this.idAccs = idAccs;
            this.hookId = hookId;
        }

        protected UsageRequestMessage(long messageId, long workId, List<string> idAccs,string hookId)
            : base(messageId)
        {
            this.workId = workId;
            this.idAccs = idAccs;
            this.hookId = hookId;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return UsageRequestMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 31;

        public long workId;
        public List<string> idAccs;
        public string hookId;

        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            List<string> idAccs = breader.ReadListOfString();
            string hookId = breader.ReadString();
            UsageRequestMessage msg = new UsageRequestMessage(messageId, workId, idAccs,hookId);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.workId);
            bwriter.Write(this.idAccs);
            bwriter.Write(this.hookId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            logger.Debug("received request for usage from clientNodeId={0}, workId={1} hookId={2} num accts={3}", receiver.clientNodeId,this.workId, this.hookId, this.idAccs.Count);
            var asyncThread = new System.Threading.Thread(new UsageRequestServicer(receiver.socketHandler, workId, hookId, idAccs).Start);
            asyncThread.Name = "USG_" + receiver.clientNodeId;
            // Servicing a usage request can result in sending high volumes of data. Since the messages have a non-zero estimated size it is 
            // possible and likely that the outbox will block on add. It is important that we never cause the receiver thread to block so
            // we need to service usage requests in their own thread.
            asyncThread.Start();
        }
    }

    public class UsageRequestServicer
    {
        SocketHandler socketHandler;
        long workId;
        string hookId;
        List<string> idAccs;
        public UsageRequestServicer(SocketHandler socketHandler, long workId, string hookId, List<string> idAccs)
        {
            this.socketHandler = socketHandler;
            this.workId = workId;
            this.idAccs = idAccs;
            this.hookId = hookId;
}
        public void Start()
        {
            UsageHookObject.ServiceUsageRequest(socketHandler, workId, hookId, idAccs);
        }
    }
}
