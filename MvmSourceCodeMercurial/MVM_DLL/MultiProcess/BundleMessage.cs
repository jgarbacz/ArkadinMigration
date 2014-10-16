using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
namespace MVM
{
     
    /// <summary>
    /// Hold data to be written into an index
    /// </summary>
    public class BundleMessage : MvmMessage
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public List<MvmMessage> bundledMessages;

        public override int Priority
        {
            get { return MessagePriority.BundleMessage; }
        }

        public BundleMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }


        private int _estimatedBytes;
        public override int EstimatedMessageBytes
        {
            get
            {
                return this._estimatedBytes;
            }
        }

        /// <summary>
        /// Constructor for a new message
        /// </summary>
        /// <param name="indexName"></param>
        public BundleMessage(List<MvmMessage> bundledMessages)
            : base()
        {
            this.bundledMessages = bundledMessages;
            foreach (var msg in bundledMessages)
            {
                this._estimatedBytes += msg.EstimatedMessageBytes;
        }
        }

        /// <summary>
        /// Constructor for deserialized message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="requestMessageId"></param>
        /// <param name="indexName"></param>
        protected BundleMessage(long messageId, List<MvmMessage> bundledMessages)
            : base(messageId)
        {
            this.bundledMessages = bundledMessages;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return BundleMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 28;


        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            int msgCount = breader.ReadInt32();
            //MessageReceiver.logger.Debug("Deserialize bundle of {0} messages", msgCount);
                
            List<MvmMessage> bundledMessages = new List<MvmMessage>(msgCount);
            for (int i = 1; i <= msgCount; i++)
            {
                byte messageType = MvmMessage.GetMessageType(breader);
                //MessageReceiver.logger.Debug("Got bundled message of type {0} = {1}", messageType, MessageReceiver.messageHanders.ContainsKey(messageType) ? MessageReceiver.messageHanders[messageType].GetType().ToString() : "INVALIDTYPE-" + messageType);
                MvmMessage messageHandler;
                if (!MessageReceiver.messageHanders.TryGetValue(messageType, out messageHandler))
                {
                    string err = "Error deserializing bundled message #" + i + ". Invalid messageType=" + messageType;
                    MessageReceiver.logger.Fatal(err);
                    throw new Exception(err);
                }
                MvmMessage message = messageHandler.Deserialize(receiver, breader);
                bundledMessages.Add(message);
            }
            BundleMessage bundleMessage = new BundleMessage(messageId, bundledMessages);
            return bundleMessage;
        }

        // msg count [int32]
        // mvm messages..
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.bundledMessages.Count);
            foreach (MvmMessage msg in bundledMessages) 
                msg.Serialize(bwriter);
        }

        public override void ProcessMessage(MessageReceiver receiver)
        {
            //logger.Debug("Process bundle, count={0}", this.bundledMessages.Count.ToString());
            foreach (var mvmMessage in this.bundledMessages)
            {
                mvmMessage.ProcessMessage(receiver);
                if (receiver.Stop)
                {
                    MessageReceiver.logger.Debug("MessageReciever thread {0} breaking for exit (while working on message bundle).", receiver.messageReceiverThread.Name);
                    return;
                }
            }
            //logger.Debug("bundle complete");
        }

    }
}
