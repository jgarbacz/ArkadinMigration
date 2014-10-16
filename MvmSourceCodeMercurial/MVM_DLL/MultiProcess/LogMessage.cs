using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
namespace MVM
{
     
    /// <summary>
    /// Placeholder class for messages that are requests.
    /// </summary>
    public class LogMessage : MvmMessage
    {
        public static Logger nlogger = LogManager.GetLogger("slave");
        public string logMsg;

        public override int Priority
        {
            get { return MessagePriority.Log; }
        }

        public LogMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        /// <summary>
        /// Constructor for new message
        /// </summary>
        /// <param name="logMsg"></param>
        public LogMessage(string logMsg):base()
        {
            this.logMsg = logMsg;
        }

        /// <summary>
        /// Constructor for deserialized message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="requestMessageId"></param>
        /// <param name="logMsg"></param>
        protected LogMessage(long messageId, string logMsg)
            : base(messageId)
        {
            this.logMsg = logMsg;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return LogMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 5;

        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            string logMsg = breader.ReadString();
            LogMessage msg = new LogMessage(messageId, logMsg);
            return msg;
        }

        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.logMsg);
        }

        public override void ProcessMessage(MessageReceiver receiver)
        {
            nlogger.Debug(this.logMsg);
        }

    }
}
