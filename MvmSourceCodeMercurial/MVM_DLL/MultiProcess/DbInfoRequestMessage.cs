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
    public class DbInfoRequestMessage : MvmMessage
    {
        public static Logger logger=LogManager.GetCurrentClassLogger();
        public override int Priority
        {
            get { return MessagePriority.DbInfoRequestMessage; }
        }
        public DbInfoRequestMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public DbInfoRequestMessage(long workId)
            : base()
        {
            this.workId = workId;
        }

        protected DbInfoRequestMessage(long messageId, long workId)
            : base(messageId)
        {
            this.workId = workId;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return DbInfoRequestMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 36;

        public long workId;

        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            DbInfoRequestMessage msg = new DbInfoRequestMessage(messageId, workId);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.workId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            var dbInfo = receiver.mvm.GetDefaultDbLogin();
            string encryptPw = TestCrypto.EncryptToHexString(dbInfo.pw);
            string[] dbInfoArr = new string[] { dbInfo.type, dbInfo.server, dbInfo.db, dbInfo.user, encryptPw };
            DbInfoResponseMessage response = new DbInfoResponseMessage(this.workId, dbInfoArr);
            //logger.Info("sending DbInfoResponse with workId={0},DbInfo.Length={1},DbInfo={2}", response.workId, dbInfoArr.Length, dbInfoArr.JoinStrings(","));
            receiver.messageOutbox.Add(response);
        }
    }
}
