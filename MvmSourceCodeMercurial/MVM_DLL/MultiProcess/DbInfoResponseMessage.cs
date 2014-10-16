using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MVM
{
    /// <summary>
    /// </summary>
    public class DbInfoResponseMessage : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.DbInfoResponseMessage; }
        }

        public long workId;
        public string[] dbInfoArr;

        public DbInfoResponseMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public DbInfoResponseMessage(long workId,string[] dbInfoArr)
            : base()
        {
            this.workId = workId;
            this.dbInfoArr = dbInfoArr;
        }

        protected DbInfoResponseMessage(long messageId, long workId,string[] dbInfoArr)
            : base(messageId)
        {
            this.workId = workId;
            this.dbInfoArr = dbInfoArr;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return DbInfoResponseMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 37;

        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            string[] dbInfoArr = breader.ReadArrayOfString();
            DbInfoResponseMessage msg = new DbInfoResponseMessage(messageId, workId,dbInfoArr);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.workId);
            bwriter.Write(this.dbInfoArr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            receiver.mvm.Log("update works status for " + this.workId + ",dbInfoArr.Length=" + dbInfoArr.Length + ", dbInfoArr=" + this.dbInfoArr.Join(","));
            string encryptPw = this.dbInfoArr[4];
            string decryptPw = TestCrypto.DecryptHexString(encryptPw);
            this.dbInfoArr[4] = decryptPw;
            receiver.mvm.remoteWorkMgr.UpdateWorkStatus(this.workId, WorkStatus.Complete, this.dbInfoArr);
        }
    }
}
