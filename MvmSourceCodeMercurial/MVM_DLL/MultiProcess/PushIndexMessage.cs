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
    public class PushIndexMessage : MvmMessage
    {
        
        public string indexName;
        public string[] orderedFieldValues;
        public bool lastRow = false;

        public override int Priority
        {
            get { return MessagePriority.PushIndex; }
        }

        public PushIndexMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        /// <summary>
        /// Constructor for a new message
        /// </summary>
        /// <param name="indexName"></param>
        public PushIndexMessage(string indexName, string[] orderedFieldValues)
            : base()
        {
            this.indexName = indexName;
            this.orderedFieldValues = orderedFieldValues;
        }

        /// <summary>
        /// Constructor for deserialized message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="requestMessageId"></param>
        /// <param name="indexName"></param>
        protected PushIndexMessage(long messageId, string indexName, string[] orderedFieldValues)
            : base(messageId)
        {
            this.indexName = indexName;
            this.orderedFieldValues = orderedFieldValues;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return PushIndexMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 7;


        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            string indexName = breader.ReadString();
            string[] orderedFieldValues = breader.ReadArrayOfString();
            PushIndexMessage msg = new PushIndexMessage(messageId, indexName, orderedFieldValues);
            return msg;
        }

        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
                bwriter.Write(this.indexName);
                bwriter.Write(this.orderedFieldValues);
        }

        // This message needs to write its data to the target index
        public override void ProcessMessage(MessageReceiver receiver)
        {
                //receiver.mvm.Log("INSERTING INTO:" + this.indexName + "-" + orderedFieldValues.Join(","));
                IIndex index = (IIndex)receiver.globalContext.GetNamedClassInst(this.indexName);
                index.IndexInsert(null, orderedFieldValues.ToList());
        }

    }
}
