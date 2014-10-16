using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
namespace MVM
{
    /// <summary>
    /// Requests new Tfids and Ufns
    /// </summary>
    public class TfidUfnRequestMessage : MvmMessage
    {
        public static object TfidUfnRequestMessageLock = new object();

        private static Logger logger = LogManager.GetCurrentClassLogger();
        public override int Priority
        {
            get { return MessagePriority.TfidUfnRequestMessage; }
        }

        public TfidUfnRequestMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public TfidUfnRequestMessage(long workId,int maxFeedbackId, int maxUfn, int maxTfid, string getFeedbackIdForFeedbackName,string getUfnForFieldName, string[] getTfidForFieldNames)
            : base()
        {
            this.workId = workId;
            this.lastFeedbackId = maxFeedbackId;
            this.lastUfn = maxUfn;
            this.lastTfid = maxTfid;
            this.getFeedbackIdForFeedbackName = getFeedbackIdForFeedbackName;
            this.getUfnForFieldName = getUfnForFieldName;
            this.getTfidForFieldNames = getTfidForFieldNames;
            //logger.Info("TfidUfnRequestMessage: work_id={3},feedback={0},ufn={1},tfidflds=[{2}]", this.getFeedbackIdForFeedbackName, this.getUfnForFieldName, this.getTfidForFieldNames.JoinStrings(","), this.workId);
           
        }

        protected TfidUfnRequestMessage(long messageId, long workId, int maxFeedbackId, int maxUfn, int maxTfid, string getFeedbackIdForFeedbackName, string getUfnForFieldName, string[] getTfidForFieldNames)
            : base(messageId)
        {
            this.workId = workId;
            this.lastFeedbackId = maxFeedbackId;
            this.lastUfn = maxUfn;
            this.lastTfid = maxTfid;
            this.getFeedbackIdForFeedbackName = getFeedbackIdForFeedbackName;
            this.getUfnForFieldName = getUfnForFieldName;
            this.getTfidForFieldNames = getTfidForFieldNames;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return TfidUfnRequestMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 40;

        // for remote work manager to get synchronous call
        public long workId;
        // last feedbackId on this node
        public int lastFeedbackId;
        // last ufn on this node
        public int lastUfn;
        // last tfid on this node
        public int lastTfid;


        // If not null these initiate calls on the super so that super is sure to
        // include these in its response.
        public string getFeedbackIdForFeedbackName = null;
        public string getUfnForFieldName=null;
        public string[] getTfidForFieldNames=null;

        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            int maxFeedbackId = breader.ReadInt32();
            int maxUfn = breader.ReadInt32();
            int maxTfid = breader.ReadInt32();
            string getFeedbackIdForFeedbackName = breader.ReadString();
            string getUfnForFieldName = breader.ReadString();
            string[] getTfidForFieldNames = breader.ReadArrayOfString();
            TfidUfnRequestMessage msg = new TfidUfnRequestMessage(messageId, workId, maxFeedbackId, maxUfn, maxTfid, getFeedbackIdForFeedbackName,getUfnForFieldName, getTfidForFieldNames);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.workId);
            bwriter.Write(this.lastFeedbackId);
            bwriter.Write(this.lastUfn);
            bwriter.Write(this.lastTfid);
            bwriter.Write(this.getFeedbackIdForFeedbackName.Nvl(""));
            bwriter.Write(this.getUfnForFieldName.Nvl(""));
            bwriter.Write(this.getTfidForFieldNames);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            lock (TfidUfnRequestMessageLock)
            {
                //logger.Info("TfidUfnRequestMessage: work_id={3},feedback={0},ufn={1},tfidflds={2}", this.getFeedbackIdForFeedbackName, this.getUfnForFieldName, this.getTfidForFieldNames.JoinStrings(","), this.workId);
                // initiate any requests
                if (this.getFeedbackIdForFeedbackName.NotNullOrEmpty())
                {
                    receiver.mvmClusterSuper.GetFeedbackId(this.getFeedbackIdForFeedbackName);
                }
                if (this.getUfnForFieldName.NotNullOrEmpty())
                {
                    receiver.mvmClusterSuper.GetUfn(this.getUfnForFieldName);
                }
                if (this.getTfidForFieldNames != null && this.getTfidForFieldNames.Length > 0)
                {
                    receiver.mvmClusterSuper.GetTfid(this.getTfidForFieldNames);
                }
                // send any updates to the the client
                TfidUfnResponseMessage msg = receiver.mvmClusterSuper.SendNewUfnsAndTfidsToNode(this.workId, receiver.clientNodeId.ToInt());
                // this needs to be send immediate to guarantee order
                receiver.messageSender.SendImmediate(msg);
            }
        }
    }
}
