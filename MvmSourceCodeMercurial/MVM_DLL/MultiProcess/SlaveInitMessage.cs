using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
namespace MVM
{

    /// <summary>
    /// This is the message the slave send to the parent indicating that it is initialized.
    /// </summary>
    public class SlaveInitMessage : MvmMessage
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        Dictionary<string, string> properies = new Dictionary<string, string>();
        public override int Priority
        {
            get { return MessagePriority.SlaveInitMessage; }
        }

        public SlaveInitMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public SlaveInitMessage(Dictionary<string, string> properies)
            : base()
        {
            this.properies = properies;
        }

        protected SlaveInitMessage(long messageId, Dictionary<string, string> properies)
            : base(messageId)
        {
            this.properies = properies;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return SlaveInitMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 33;

    

        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            Dictionary<string, string> properties = breader.ReadDictionaryOfStringString();
            SlaveInitMessage msg = new SlaveInitMessage(messageId, properties);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.properies);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            receiver.mvm.mvmClusterSuper.SetSlaveInitialized(receiver.clientNodeId.ToInt(),this.properies);
        }
    }
}
