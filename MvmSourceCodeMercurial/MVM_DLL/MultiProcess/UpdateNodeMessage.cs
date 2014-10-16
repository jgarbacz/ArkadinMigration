using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
namespace MVM
{
    /// <summary>
    /// An unconnected node (e.g. the profiler) sends this to the supernode to update
    /// machine/port info so that other nodes can connect to it.
    /// </summary>
    public class UpdateNodeMessage : MvmMessage
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        Dictionary<string, string> properties = new Dictionary<string, string>();
        public override int Priority
        {
            get { return MessagePriority.InteruptProcCall; }
        }

        public UpdateNodeMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public UpdateNodeMessage(Dictionary<string, string> properties)
            : base()
        {
            this.properties = properties;
        }

        protected UpdateNodeMessage(long messageId, Dictionary<string, string> properties)
            : base(messageId)
        {
            this.properties = properties;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return UpdateNodeMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 46;

        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            Dictionary<string, string> properties = breader.ReadDictionaryOfStringString();
            UpdateNodeMessage msg = new UpdateNodeMessage(messageId, properties);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.properties);
        }

        /// <summary>
        /// Updates the node info, and push the changes to all other nodes
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            receiver.mvm.Log("Received message to update node info, so push updates to all nodes");
            receiver.mvmClusterSuper.AddOrUpdateKnownNode(this.properties);
            receiver.mvmClusterSuper.PushNodeInfo();
        }
    }
}
