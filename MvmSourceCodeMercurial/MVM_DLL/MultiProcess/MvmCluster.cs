using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
 using System.Threading;
using System.Diagnostics;
namespace MVM
{
    public class MvmCluster:MvmClusterBase
    {
        public MvmCluster(MvmEngine mvm, int nodeId)
            : base(mvm, nodeId)
        {
        }

        // Called when this is a slave node
        public void SetupSlave(int superNodeId, string superMachine, int superPort, int nodeId, string machine)
        {
            // Connect to the super so he knows we're alive
            this.SuperNode = new MvmClusterNode(
                this,
                new Dictionary<string,string>{
                    {"is_profiler","0"},
                    {"node_id",superNodeId.ToString()},
                    {"server",superMachine},
                    {"port",superPort.ToString()}
                });
            this.AddKnownNode(this.SuperNode);
            var junk1 = this.SuperNode.SocketHandler;
            logger.Debug("Now connected with the super node");

            // Tell the super node we are ready... or ask for a new port if the one we have does not work...
            // get this passed in.
            this.Port = StartListener();
        }

        public int StartListener()
        {
            int port;
            for (; ; )
            {
                logger.Debug("Requesting a free port from the super node");

                port = this.mvm.mvmCluster.GetFreePort(this.SuperNode.nodeId, System.Environment.MachineName);
                logger.Debug("Super said to try port:" + port);
                if (port < 0)
                {
                    throw new Exception("Error, super node ran out of ports for machine: " + System.Environment.MachineName);
                }
                try
                {
                    this.MyListener = new SlaveListener(this.mvm, port);
                    logger.Debug("Now listening on my port {0}", port);
                    break;
                }
                catch (System.Net.Sockets.SocketException e)
                {
                    logger.Warn("Cannot listen on port [{0}], need to ask super for another. Exception message [{1}]", port, e.Message);
                }
            }
            return port;
        }

        public override int GetFreePort(int machineNodeId, string machineName)
        {
            logger.Debug("Requesting port with super for machine:" + machineName);

            // Need to wait for a response so do this through the work manager
            long batchId = mvm.remoteWorkMgr.CreateBatch();
            WorkBatch batch = this.mvm.remoteWorkMgr.LookupBatch(batchId);
            WorkInfo w = this.mvm.remoteWorkMgr.CreateWork(batchId);
            w.procName = "PortRequestMessage";
            w.nodeId = this.SuperNode.nodeId;
            w.priority = MessagePriority.Interupt;
            w.status = WorkStatus.WaitingToStart;

            // create and send the message.
            var msg = new PortRequestMessage(w.workId, machineName);
            logger.Debug("sending FormatIdRequestMessage workId={0}", msg.workId);
            this.SuperNode.SocketHandler.messageOutbox.Add(msg);

            // Wait for the batch to complete
            BlockingWaitBatchEvent blockingWait = new BlockingWaitBatchEvent();
            batch.AddBatchCompleteEvent(blockingWait);
            blockingWait.WaitForBatchComplete();

            // Inspect the results...
            Dictionary<string, string> outputs = w.outputs as Dictionary<string, string>;
            //Console.WriteLine(outputs.Count);
            string portStr = outputs["port"];
            int port = portStr.ToInt();
            logger.Debug("super returned portStr:" + portStr);

            // clear the batch
            this.mvm.remoteWorkMgr.ClearBatch(batch.batchId);

            // Return it.
            return port;
        }

        public override void GetRcnBatch(int reqBatchSize, out long batchStart, out int batchSize)
        {
            // Need to wait for a response so do this through the work manager
            long batchId = mvm.remoteWorkMgr.CreateBatch();
            WorkBatch batch = this.mvm.remoteWorkMgr.LookupBatch(batchId);
            WorkInfo w = this.mvm.remoteWorkMgr.CreateWork(batchId);
            w.procName = "RcnRequestMessage";
            w.nodeId = this.SuperNode.nodeId;
            w.priority = MessagePriority.Interupt;
            w.status = WorkStatus.WaitingToStart;

            // create and send the message.
            var msg = new RcnRequestMessage(w.workId, reqBatchSize);
            //logger.Trace("sending RcnRequestMessage workId={0} reqBatchSize={1}", msg.workId, reqBatchSize);
            this.SuperNode.SocketHandler.messageOutbox.Add(msg);

            // Wait for the batch to complete
            BlockingWaitBatchEvent blockingWait = new BlockingWaitBatchEvent();
            batch.AddBatchCompleteEvent(blockingWait);
            blockingWait.WaitForBatchComplete();

            // Inspect the results...
            var outputs = w.outputs as Tuple<long, int>;
            batchStart = outputs.Item1;
            batchSize = outputs.Item2;
            //logger.Trace("Super returned batchStart=" + batchStart + " batchSize=" + batchSize);
        }

        /// <summary>
        /// Returns the cluster node associated with the node id, asking the
        /// supernode if it doesn't already know about the nodeId.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public override MvmClusterNode GetClusterNode(int nodeId)
        {
            try
            {
                MvmClusterNode clusterNode;

                // If you already know about the node, return it,Otherwise, ask the supernode.
                if (this.TryGetKnownNode(nodeId, out clusterNode))
                    return clusterNode;

                //logger.Debug("Need to lookup machine/port with super for node_id:" + nodeId);

                // Need to wait for a response so do this through the work manager
                long batchId = mvm.remoteWorkMgr.CreateBatch();
                WorkBatch batch = this.mvm.remoteWorkMgr.LookupBatch(batchId);
                WorkInfo w = this.mvm.remoteWorkMgr.CreateWork(batchId);
                w.procName = "NodeIdInfoRequestMessage";
                w.nodeId = this.SuperNode.nodeId;
                w.priority = MessagePriority.Interupt;
                w.status = WorkStatus.WaitingToStart;

                // create and send the message.
                NodeIdInfoRequestMessage msg = new NodeIdInfoRequestMessage(w.workId, nodeId);
                //logger.Debug("sending msg to super.outbox");
                this.SuperNode.SocketHandler.messageOutbox.Add(msg);

                // Wait for the batch to complete
                BlockingWaitBatchEvent blockingWait = new BlockingWaitBatchEvent();
                batch.AddBatchCompleteEvent(blockingWait);
                blockingWait.WaitForBatchComplete();
                // Inspect the results...
                Dictionary<string, string> outputs = w.outputs as Dictionary<string, string>;
                //Console.WriteLine(outputs.Count);
                //logger.Trace("super said node_id:" + nodeId + " is machine=" +  outputs["machine"] + ", and port=" + outputs["port"]);
                string machine = outputs["machine"];
                string port = outputs["port"];
                //logger.Trace("super said node_id:" + nodeId + " is machine=" + machine + ", and port=" + port);


                // Create the active cluster node
                clusterNode = new MvmClusterNode(this, outputs);
                this.AddKnownNode(clusterNode);

                // Return it.
                return clusterNode;
            }
            catch (Exception e)
            {
                string msg = "Error fetching machine/port from super node for node_id=" + nodeId + ". Exception=" + e.GetStackTraceRecursive();
                logger.Fatal(msg);
                throw new Exception(msg,e);
            }
        }


        #region UFNs and TFIDs

        public void UnpackupNewUfnsAndTfids(int firstFeedbackId, string[] feedbackNames, int firstUfn, string[] ufnFieldNames, int firstTfid, int[] tfidUfnArrayMaps)
                {
            //logger.Info("UNPACK: firstFeedbackId=" + firstFeedbackId + "=[" + feedbackNames.JoinStrings(",") + "]");
            if (firstFeedbackId >= 0)
        {
                for (int i = 0; i < feedbackNames.Length; i++)
            {
                    int feedbackId = firstFeedbackId + i;
                    string feedbackName = feedbackNames[i];
                    int createdFeedbackId = this.GetCreateFeedbackId(feedbackName);
                    if (createdFeedbackId != feedbackId)
                    {
                        throw new Exception("unexpected. createdFeedbackId=[" + createdFeedbackId + "] != feedbackId=[" + feedbackId + "]");
            }
                    else
                    {
                        //logger.Info("ADDED FEEDBACK ID=" + createdFeedbackId + "=[" + feedbackName + "]");
        }
                }
            }
            //logger.Info("UNPACK: firstUfn=" + firstUfn + "=[" + ufnFieldNames.JoinStrings(",") + "]");

            if (firstUfn >= 0)
        {
                for (int i = 0; i < ufnFieldNames.Length; i++)
            {
                    int ufn = firstUfn + i;
                    string fieldName = ufnFieldNames[i];
                    int createdUfn = this.GetCreateUfn(fieldName);
                    if (createdUfn != ufn)
                    {
                        throw new Exception("unexpected. createdUfn=[" + createdUfn + "] != ufn=[" + ufn + "]");
            }
                    else
                    {
                        //logger.Info("ADDED ufn =" + createdUfn + "=[" + fieldName + "]");
        }
                }
            }
            //logger.Info("UNPACK: firstTfid=" + firstTfid + "=[" + tfidUfnArrayMaps.JoinStrings(",") + "]");

            if (firstTfid >= 0)
            {
                int j = 0;
                while (j < tfidUfnArrayMaps.Length)
        {
                    int tfid = tfidUfnArrayMaps[j++];
                    int numFields = tfidUfnArrayMaps[j++];
                    string[] fieldNames = new string[numFields];
                    for (int fldIdx = 0; fldIdx < numFields; fldIdx++)
            {
                        int ufn = tfidUfnArrayMaps[j++];
                        string fieldName = this.GetFieldName(ufn);
                        fieldNames[fldIdx] = fieldName;
                    }

                    int createdTfid = this.GetCreateTfid(fieldNames);
                    if (createdTfid != tfid)
                    {
                        throw new Exception("unexpected. createdTfid=[" + createdTfid + "] != tfid=[" + tfid + "]");
                }
                    else
                    {
                        //logger.Info("UNPACKED: tfid=" + tfid);
            }
        }
            }
            //logger.Info("DONE UNPACKING");

        }

        private void RemoteRequestTfidUfn(string getFeedbackIdForFeedbackName, string getUfnForFieldName, string[] getTfidForFieldNames)
        {
            //logger.Info("remote request!!");
            //this.mvm.FlushNLog();

            //logger.Info("remote request from stack:" + System.Environment.StackTrace);
            // Need to wait for a response so do this through the work manager
            long batchId = mvm.remoteWorkMgr.CreateBatch();
            WorkBatch batch = this.mvm.remoteWorkMgr.LookupBatch(batchId);
            WorkInfo w = this.mvm.remoteWorkMgr.CreateWork(batchId);
            w.procName = "TfidUfnRequestMessage";
            w.nodeId = this.SuperNode.nodeId;
            w.priority = MessagePriority.Interupt;
            w.status = WorkStatus.WaitingToStart;

            // create and send the message.
            var msg = new TfidUfnRequestMessage(w.workId, this.FeedbackEncoder.MaxIndex,this.UfnEncoder.MaxIndex, this.TfidEncoder.MaxIndex,getFeedbackIdForFeedbackName, getUfnForFieldName, getTfidForFieldNames);
            //logger.Info("sending RcnRequestMessage workId={0} reqBatchSize={1}", msg.workId, reqBatchSize);
            this.SuperNode.SocketHandler.messageOutbox.Add(msg);

            // Wait for the batch to complete
            BlockingWaitBatchEvent blockingWait = new BlockingWaitBatchEvent();
            batch.AddBatchCompleteEvent(blockingWait);
            blockingWait.WaitForBatchComplete();
            
            // clear it out
            this.mvm.remoteWorkMgr.ClearBatch(batchId);
        }

        public override int GetFeedbackId(string feedbackName)
        {
            int feedbackId;
            if (this.FeedbackEncoder.TryGetIdx(feedbackName, out feedbackId))
                return feedbackId;
            this.RemoteRequestTfidUfn(feedbackName, null,null);
            if (this.FeedbackEncoder.TryGetIdx(feedbackName, out feedbackId))
                return feedbackId;
            throw new Exception("unexpected remote GetFeedbackId(" + feedbackName + ") failed");
        }

        public override string GetFeedbackName(int feedbackId)
            {
            string feedbackName;
            if (this.FeedbackEncoder.TryGetItem(feedbackId, out feedbackName))
                return feedbackName;
            this.RemoteRequestTfidUfn(null, null,null);
            if (this.FeedbackEncoder.TryGetItem(feedbackId, out feedbackName))
                return feedbackName;
            throw new Exception("unexpected remote GetFeedbackName(" + feedbackId + ") failed");
            }

        public override int GetUfn(string fieldName)
        {
            int ufn;
            if (this.UfnEncoder.TryGetIdx(fieldName, out ufn))
                return ufn;
            this.RemoteRequestTfidUfn(null,fieldName, null);
            if (this.UfnEncoder.TryGetIdx(fieldName, out ufn))
                return ufn;
            throw new Exception("unexpected remote GetUfn(" + fieldName + ") failed");
        }

        public override string GetFieldName(int ufn)
        {
            string fieldName;
            if (this.UfnEncoder.TryGetItem(ufn, out fieldName))
            {
                return fieldName;
            }
            this.RemoteRequestTfidUfn(null, null,null);
            if (this.UfnEncoder.TryGetItem(ufn, out fieldName))
            {
                return fieldName;
            }
            throw new Exception("unexpected remote GetFieldName(" + ufn + ") failed");
        }

        public override int GetFormatFieldIdx(int tfid, int ufn)
            {
            lock (TfidUfnIdxMap)
                {
                if (tfid < TfidUfnIdxMap.Count)
                {
                    int[] ufnIdxMap = TfidUfnIdxMap[tfid];
                    if (ufn >= ufnIdxMap.Length) return -1;
                    return ufnIdxMap[ufn];
                }
            }
                    this.RemoteRequestTfidUfn(null,null, null);
            lock (TfidUfnIdxMap)
            {
                int[] ufnIdxMap = TfidUfnIdxMap[tfid];
                if (ufn >= ufnIdxMap.Length) return -1;
                return ufnIdxMap[ufn];
            }
        }

        public override int[] GetUfnIdxMap(int tfid)
        {
            lock (TfidUfnIdxMap)
        {
                if (tfid < TfidUfnIdxMap.Count)
            {
                    return TfidUfnIdxMap[tfid];
                }
            }
                    this.RemoteRequestTfidUfn(null,null, null);
            lock (TfidUfnIdxMap)
            {
                return TfidUfnIdxMap[tfid];
                }
            }
           
        public override int GetTfid(string[] fieldNames)
            {
            StringArray fieldNamesObj = new StringArray(fieldNames);
            int tfid;
            if (this.TfidEncoder.TryGetIdx(fieldNamesObj, out tfid))
                return tfid;
            this.RemoteRequestTfidUfn(null,null, fieldNames);
            if (this.TfidEncoder.TryGetIdx(fieldNamesObj, out tfid))
                return tfid;
            throw new Exception("unexpected remote GetTfid(" + fieldNames.JoinStrings(",") + ") failed");
            }

        public override string[] GetTfidFields(int tfid)
        {
            StringArray fieldNamesObj;
            if (this.TfidEncoder.TryGetItem(tfid, out fieldNamesObj))
                return fieldNamesObj.array;
            this.RemoteRequestTfidUfn(null,null, null);
            if (this.TfidEncoder.TryGetItem(tfid, out fieldNamesObj))
                return fieldNamesObj.array;
            throw new Exception("unexpected remote GetTfidFields(" + tfid + ") failed");
        }

        #endregion
    }

}
