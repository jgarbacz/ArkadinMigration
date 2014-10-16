using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
namespace MVM
{

    /// <summary>
    /// This message response with usage 
    /// </summary>
    public class UsageResponseMessage : MvmMessage
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public override int Priority
        {
            get { return MessagePriority.UsageResponseMessage; }
        }

        public long workId;
        public string hookId;
        public byte[] usageBuffer;

        public override int EstimatedMessageBytes
        {
            get
            {
                return usageBuffer.Length;
            }
        }

        public UsageResponseMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public UsageResponseMessage(long workId,string hookId,byte[] usageBuffer)
            : base()
        {
            this.workId = workId;
            this.hookId = hookId;
            this.usageBuffer = usageBuffer;
        }

        protected UsageResponseMessage(long messageId, long workId, string hookId, byte[] usageBuffer)
            : base(messageId)
        {
            this.workId = workId;
            this.hookId = hookId;
            this.usageBuffer = usageBuffer;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return UsageResponseMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 32;


        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            string hookId = breader.ReadString();
            byte[] usageBuffer=breader.ReadArrayOfByte();
            UsageResponseMessage msg = new UsageResponseMessage(messageId, workId, hookId, usageBuffer);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.workId);
            bwriter.Write(this.hookId);
            bwriter.WriteArrayOfByte(this.usageBuffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            logger.Debug("received response for usage workId={0} hookId={1} bufLen={2}", this.workId,this.hookId, this.usageBuffer.Length);
            
            // lookup the usage hook and write out the usage...
            string hookName = UsageHookObject.GetHookName(this.hookId);
            UsageHookObject usageHookObject = receiver.mvm.globalContext.GetNamedClassInst(hookName) as UsageHookObject;
            usageHookObject.WriteFetchedUsage(receiver.socketHandler.clientNodeIdInt,this.usageBuffer);

            // (0 - orig_work_id) means more usage coming else complete.
            if (this.workId >= 0)
            {
                Dictionary<string, string> outputs = new Dictionary<string, string>();
                outputs["hookId"] = this.hookId.ToString();
                receiver.mvm.remoteWorkMgr.UpdateWorkStatus(this.workId, WorkStatus.Complete, outputs);
                logger.Debug("usage fetch complete");
            }
            else
            {
                logger.Debug("more usage comming");
            }
        }
    }
}
