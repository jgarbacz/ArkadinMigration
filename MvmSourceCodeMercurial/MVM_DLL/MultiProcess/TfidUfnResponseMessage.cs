using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
namespace MVM
{

    /// <summary>
    /// This message requests batchStart/machine for a node id. 
    /// </summary>
    public class TfidUfnResponseMessage : MvmMessage
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public override int Priority
        {
            get { return MessagePriority.TfidUfnResponseMessage; }
        }

        public long workId;

        // firstFeedbackId=3
        // feedbackIdFieldNames[0]='T_PV_CONFCON' means ufn=3='T_PV_CONFCON';
        public int firstFeedbackId;
        public string[] feedbackNames;

        // firstUfn=22
        // ufnFieldNames[0]='hi' means ufn=22='hi';
        public int firstUfn;
        public string[] ufnFieldNames;

        // [tfid1, #ufns, ufn1, ufn2...]...
        public int firstTfid;
        public int[] tfidUfnArrayMaps;

        public TfidUfnResponseMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public TfidUfnResponseMessage(long workId, int firstFeedbackId, string[] feedbackIdFieldNames, int firstUfn, string[] ufnFieldNames, int firstTfid, int[] tfidUfnArrayMaps)
            : base()
        {
            this.workId = workId;
            this.firstFeedbackId = firstFeedbackId;
            this.feedbackNames = feedbackIdFieldNames;   
            this.firstUfn=firstUfn;
            this.ufnFieldNames = ufnFieldNames;   
            this.firstTfid=firstTfid;
            this.tfidUfnArrayMaps = tfidUfnArrayMaps;
        }

        protected TfidUfnResponseMessage(long messageId, long workId, int firstFeedbackId, string[] feedbackNames, int firstUfn, string[] ufnFieldNames, int firstTfid, int[] tfidUfnArrayMaps)
            : base(messageId)
        {
            this.workId = workId;
            this.firstFeedbackId = firstFeedbackId;
            this.feedbackNames = feedbackNames;  
            this.firstUfn=firstUfn;
            this.ufnFieldNames = ufnFieldNames;   
            this.firstTfid=firstTfid;
            this.tfidUfnArrayMaps = tfidUfnArrayMaps;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return TfidUfnResponseMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 41;


        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            int firstFeedbackId = breader.ReadInt32();
            string[] feedbackNames = breader.ReadArrayOfString();
            int firstUfn=breader.ReadInt32();
            string[] ufnFieldNames = breader.ReadArrayOfString();
            int firstTfid = breader.ReadInt32(); 
            int[] tfidUfnArrayMaps=breader.ReadArrayOfInt32();
            TfidUfnResponseMessage msg = new TfidUfnResponseMessage(messageId, workId, firstFeedbackId,feedbackNames,firstUfn, ufnFieldNames,firstTfid,tfidUfnArrayMaps);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.workId);
            bwriter.Write(this.firstFeedbackId);
            bwriter.Write(this.feedbackNames);
            bwriter.Write(this.firstUfn);
            bwriter.Write(this.ufnFieldNames);
            bwriter.Write(this.firstTfid);
            bwriter.Write(this.tfidUfnArrayMaps);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            //logger.Info("TfidUfnResponse: work_id={0}",this.workId);
            receiver.mvmCluster.mvm.mvmClusterSlave.UnpackupNewUfnsAndTfids(firstFeedbackId, feedbackNames, firstUfn, ufnFieldNames, firstTfid, tfidUfnArrayMaps);

            // if this is a synchrounous response update the worker status so that guy resumes.
            if (this.workId >= 0)
            {
                //logger.Info("TfidUfnResponse, was an synchronous message");
                receiver.mvm.remoteWorkMgr.UpdateWorkStatus(this.workId, WorkStatus.Complete, this);
            }
            else
            {
                //logger.Info("TfidUfnResponse, was an async message");
            }
        }
    }
}
