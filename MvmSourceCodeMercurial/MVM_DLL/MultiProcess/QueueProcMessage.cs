using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
namespace MVM
{
    /// <summary>
    /// Message to queue a proc remotely
    /// </summary>
    public class QueueProcMessage : MvmMessage
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public override int Priority
        {
            get { return this.priority; }
        }

        public QueueProcMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }


        public QueueProcMessage(string procName, string procNameSpace, long workId, int priority, IObjectData objectData, Dictionary<string,string> inputParams, bool includeObjectFieldsInOutput)
            : base()
        {
            this.procName = procName;
            this.procNameSpace = procNameSpace;
            this.workId = workId;
            this.priority = priority;
            this.serializedObjectData = objectData.Serialize();
            this.serializedInputParams = inputParams.Serialize();
            this.includeObjectFieldsInOutput = includeObjectFieldsInOutput;
        }
        
        public QueueProcMessage(string procName, string procNameSpace, long workId, int priority, byte[] serializedObject, byte[] serializedParams, bool includeObjectFieldsInOutput)
            : base()
        {
            this.procName = procName;
            this.procNameSpace = procNameSpace;
            this.workId = workId;
            this.priority = priority;
            this.serializedObjectData = serializedObject;
            this.serializedInputParams = serializedParams;
            this.includeObjectFieldsInOutput=includeObjectFieldsInOutput;
            //logger.Info("QueueProcMessage:proc={0} ns={1} work_id={2} priority={3}", this.procName, this.procNameSpace, this.workId, this.priority);
        }

        private QueueProcMessage(long messageId, string procName, string procNameSpace, long workId, int priority, IObjectData objectData, IDictionary<string, string> inputParams, bool includeObjectFieldsInOutput)
            : base(messageId)
        {
            this.procName = procName;
            this.procNameSpace = procNameSpace;
            this.workId = workId;
            this.priority = priority;
            this.objectData = objectData;
            this.inputParams = inputParams;
            this.includeObjectFieldsInOutput = includeObjectFieldsInOutput;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return QueueProcMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 2;

        public string procName;
        public string procNameSpace;
        public int priority;
        
        public IObjectData objectData;
        public byte[] serializedObjectData;

        public IDictionary<string, string> inputParams;
        public byte[] serializedInputParams;

        bool includeObjectFieldsInOutput;

        public long workId;


        private class AsyncDeserializeObjectData
        {

        }



        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            //logger.Debug("DeserializeMessagePayload");
            string procName = breader.ReadString();
            //logger.Debug("received remote work:" + procName);
            string procNameSpace = breader.ReadString();
            //logger.Info("procNameSpace:" + procNameSpace);
            long workId = breader.ReadInt64();
            //logger.Info("workId:" + workId);
            int priority = breader.Read7BitEncodedInt();
            //logger.Info("priority:" + priority);

            IObjectData objectData = receiver.mvm.objectDataSerializer.Deserialize(breader);
            //logger.Info("objectData.objectId:" + objectData.objectId);
            //logger.Info("objectData:" + objectData.ToString());


            IDictionary<string, string> inputParams = new Dictionary<string, string>().Deserialize(breader);
            bool includeObjectFieldsInOutput=breader.ReadBoolean();
            QueueProcMessage msg = new QueueProcMessage(messageId, procName, procNameSpace, workId, priority, objectData, inputParams, includeObjectFieldsInOutput);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.procName);
            bwriter.Write(this.procNameSpace);
            bwriter.Write(this.workId);
            bwriter.Write7BitEncodedInt(this.priority);
            bwriter.Write(this.serializedObjectData);
            bwriter.Write(this.serializedInputParams);
            bwriter.Write(this.includeObjectFieldsInOutput);
        }

        /// <summary>
        /// Calls the proc
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            //logger.Debug("ProcessMessage");
            //logger.Info("workId:" + workId);
            //logger.Info("objectData.objectId:" + objectData.objectId);
            //logger.Info("objectData:" + objectData.ToString());

             // bump the ref so this stays around util we send it back
            this.objectData.RefGet();
            //logger.Info("add/overwrite object oid=" + this.objectData.objectId + ", refs=" + this.objectData.RefCount);
            receiver.mvm.objectCache.AddOrOverwriteObject(this.objectData);

            // create proc cleanup when done
            int cleanupProcId = receiver.mvm.workMgr.schedulerMaster.GetProcId("global", "send_remote_proc_results_to_caller");

            ProcInst cleanupWork = receiver.mvm.workMgr.schedulerMaster.GetProcInst(cleanupProcId, this.objectData.objectId);
            cleanupWork.isSync = 1;
            cleanupWork.procContext = new ProcContext();
            long cleanupCallbackId = receiver.mvm.workMgr.CreateCallback(cleanupWork);

            // setup the temp context with the info we need to send results
            TempContext tempContext = cleanupWork.procContext.tempContext;
            tempContext["remote_proc_name"] = procName;
            tempContext["remote_proc_namespace"] = procNameSpace;
            tempContext["caller_process_id"] = receiver.socketHandler.clientNodeId; 
            tempContext["work_id"] = this.workId.ToString();
            tempContext["include_object_fields_in_output"] = this.includeObjectFieldsInOutput ? "1" : "0";

            // setup the temp context with the input parameters
            foreach (var entry in this.inputParams) tempContext[entry.Key] = entry.Value;

            // create proc to do the work, explicitly pass the temp context
            int procId = receiver.mvm.workMgr.schedulerMaster.GetProcId(procNameSpace, this.procName);
            ProcInst newWork = receiver.mvm.workMgr.schedulerMaster.GetProcInst(procId, this.objectData.objectId);
            newWork.isSync = 1;
            newWork.procContext = new ProcContext(tempContext);
            
            // make sure the new work calls the cleanup when done
            newWork.callbackId = cleanupCallbackId;

            // launch the work
            //logger.Info("calling produce work work_id="+this.workId);
            receiver.mvm.workMgr.ProduceWork(newWork);
        }

    }
}
