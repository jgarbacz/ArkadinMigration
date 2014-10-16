using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using NLog;

namespace MVM
{
    public abstract class MvmClusterBase
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public int NodeId;
        public int Port;
        public string NodeIdStr { get { return this.NodeId.ToString(); } }

        /// <summary>
        /// Points to the super node
        /// </summary>
        public MvmClusterNode SuperNode;

        /// <summary>
        /// Points to the master node
        /// </summary>
        public MvmClusterNode MasterNode;

        /// <summary>
        /// Hook to the mvm
        /// </summary>
        public MvmEngine mvm;

        /// <summary>
        /// These are the nodes we know about and can send data to.
        /// </summary>
        private Dictionary<int, MvmClusterNode> KnownNodes = new Dictionary<int, MvmClusterNode>();

        /// <summary>
        /// When we initiate a connection to ourself, the client side will end up in KnownNodes
        /// and the listener side will end up in SelfNode. 
        /// </summary>
        public MvmClusterNode SelfNode;

        /// <summary>
        /// Every node listens.
        /// </summary>
        public SlaveListener MyListener;

        /// <summary>
        /// Returns true if this node is the super node.
        /// </summary>
        public bool IsSuperNode
        {
            get
            {
                return (this is MvmClusterSuper);
            }
        }

        /// <summary>
        /// Returns true if this node is the profiler node.
        /// </summary>
        public bool IsProfilerNode
        {
            get
            {
                return (this is MvmClusterProfiler);
            }
        }

        /// <summary>
        /// Returns the profiler node
        /// </summary>
        public MvmClusterNode GetProfilerNode()
        {
            foreach (var node in this.GetKnownNodes())
            {
                if (node.isProfiler && (node.IsConnected || !this.IsSuperNode))
                {
                    return node;
                }
            }
            return null;
        }

        public ProfilerThread profilerThread = null;
        public volatile bool profilerActive = false;
        public volatile bool profilerNodeStarted = false;
        public object startProfilerEvent = new object();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mvm"></param>
        public MvmClusterBase(MvmEngine mvm, int nodeId)
        {
            this.mvm = mvm;
            this.NodeId = nodeId;
        }


        #region Known nodes

        private volatile int _maxNodeId;
        public int MaxNodeId
        {
            get
            {
                return _maxNodeId;
            }
            set
            {
                _maxNodeId = value;
            }
        }

        public void AddOrUpdateKnownNode(Dictionary<string,string> dic)
        {
            int passedNodeId = dic["node_id"].ToInt();
            if (passedNodeId == this.NodeId) return;
            MvmClusterNode node;
            if (this.TryGetKnownNode(passedNodeId, out node))
            {
                node.UpdateWithDictionary(dic);
            }
            else
            {
                AddKnownNode(new MvmClusterNode(this, dic));
            }
        }

        public void AddKnownNode(MvmClusterNode node){
            lock (this.KnownNodes)
            {
                // Self node does not go in the known nodes structure.
                if (this.NodeId == node.nodeId)
                {
                    if (this.SelfNode != null) 
                        throw new Exception("cannot end up with more then one connection with self!");
                    this.SelfNode = node;
                }
                else
                {
                    if (this.KnownNodes.ContainsKey(node.nodeId))
                        throw new Exception("Error duplicate known node registration of node_id=" + node.nodeId);
                    this.KnownNodes[node.nodeId] = node;
                }
            }
        }

        public bool TryGetKnownNode(int nodeId, out MvmClusterNode node)
        {
            lock (this.KnownNodes)
            {
                if (this.KnownNodes.TryGetValue(nodeId, out node)) return true;
                return false;
            }
        }

        public List<MvmClusterNode> GetKnownNodes()
        {
            lock (this.KnownNodes)
            {
                return this.KnownNodes.Values.ToList();
            }
        }

        /// <summary>
        /// A slave is any node that is not 0 or self or the profiler
        /// </summary>
        /// <returns></returns>
        public List<MvmClusterNode> GetSlaveNodes()
        {
            return this.GetKnownNodes().Where(n => (n.nodeId.NotIn(0, this.NodeId) && !n.isProfiler)).ToList();
        }

        // This is how the listener tells us about people that connected to us.
        public void ListenerAddSocket(SocketHandler socketHandler)
        {
            // need to create or update a node for the socket
            MvmClusterNode mvmClusterNode;
            if (!this.TryGetKnownNode(socketHandler.clientNodeIdInt, out mvmClusterNode))
            {
                //this.mvm.Trace("LISTENER CREATING THE NODE " + socketHandler.clientNodeIdInt);
                mvmClusterNode = new MvmClusterNode(this, socketHandler);
                this.AddKnownNode(mvmClusterNode);
            }
            else
            {
                //this.mvm.Trace("LISTENER UPDATING THE NODE " + socketHandler.clientNodeIdInt);
                mvmClusterNode.ListenerSetSocketHandler(socketHandler);
            }
        }

        // This is called to start gathering profiler info on all nodes
        public void ProfileAllNodes()
        {
            foreach (var node in this.GetKnownNodes().Where(n => !n.isProfiler))
            {
                node.mvm.Log("Telling node " + node.nodeIdStr + " to start profiling");
                node.SocketHandler.messageOutbox.Add(new ProfilerInitMessage(MvmClusterProfiler.DefaultSamplingPeriod, MvmClusterProfiler.DefaultReportingCount));
                node.SocketHandler.messageOutbox.Add(new ProfilerStartMessage(-1, -1));
            }
            // If this is the supernode, it won't show up in the known nodes list
            if (this.IsSuperNode)
            {
                this.mvm.Log("Telling node " + this.SuperNode.nodeIdStr + " to start profiling");
                this.SuperNode.SocketHandler.messageOutbox.Add(new ProfilerInitMessage(MvmClusterProfiler.DefaultSamplingPeriod, MvmClusterProfiler.DefaultReportingCount));
                this.SuperNode.SocketHandler.messageOutbox.Add(new ProfilerStartMessage(-1, -1));
            }
        }

        # endregion

        public abstract int GetFreePort(int machineNodeId, string machineName);

        #region Persistent format ids PFIDs are stored int the database

        public int GetFormatId(List<string> sortedFields)
        {
            return this.GetFormatId(sortedFields.JoinStrings(","));
        }

        private SynchronizedDictionary<string, int> formatStringIdMap = new SynchronizedDictionary<string, int>();
        private SynchronizedDictionary<int, string[]> formatIdFieldsMap = new SynchronizedDictionary<int, string[]>();

        private object cachedFormatsLock = new object();
        private bool cachedFormats = false;
        /// <summary>
        /// Caches all the formats from the database
        /// </summary>
        public void CacheFormats()
        {
            if (this.cachedFormats) return;
            lock (this.cachedFormatsLock)
            {
                if (this.cachedFormats) return;
                this.cachedFormats = true;
                this.mvm.Log("Caching pre-existing formats from database");
                // assume GLOBAL.target_login is set
                string globalTargetLoginOid = this.mvm.globalContext["target_login"];
                StaticDbLoginInfo dbInfo = StaticDbLoginInfo.FromObjectId(this.mvm, globalTargetLoginOid);
                string query = "select * from agg_field_formats";
                var results = DbUtils.DbQueryToListOfDictionary(dbInfo.type, dbInfo.server, dbInfo.db, dbInfo.user, dbInfo.pw, query);
                foreach (var row in results)
                {
                    int formatId = row["FORMAT_ID"].ToInt();
                    string formatString = "";
                    foreach (var formatStringField in row.Keys.Where(f => f.StartsWith("FORMAT_STRING")).OrderBy(x => x.Substring(13).ToInt()))
                    {
                        formatString += row[formatStringField];
                    }
                    this.RegisterFormat(formatId, formatString);
                }
            }
        }


        /// <summary>
        /// Registers a format
        /// </summary>
        /// <param name="formatId"></param>
        /// <param name="formatString"></param>
        private void RegisterFormat(int formatId, string formatString)
        {
            lock (this.formatStringIdMap.SyncRoot)
            {
                if (!this.formatStringIdMap.ContainsKey(formatString)) this.formatStringIdMap[formatString] = formatId;
            }
            lock (this.formatIdFieldsMap.SyncRoot)
            {

                if (!formatIdFieldsMap.ContainsKey(formatId))
                {
                    string[] formatFields;
                    if (formatString.NotNullOrEmpty())
                        formatFields = formatString.Split(',');
                    else
                        formatFields = new string[] { };
                    //this.mvm.Log("registing formatId=" + formatId + ",formatFields.Length=" + formatFields.Length + ",formatString=[" + formatString + "],formatFields=[" + formatFields.JoinStrings(",") + "]");
                    this.formatIdFieldsMap[formatId] = formatFields;
                }
            }
        }

        /// <summary>
        /// Gets or creates a format id for the passed format string
        /// This is hard coded for GLOBAL.target_login and agg_field_formats
        /// This is executed via messages so we don't want to rely on calling
        /// mvm config to do it. 
        /// </summary>
        /// <param name="formatString"></param>
        /// <returns></returns>
        public int GetFormatId(string formatString)
        {
            this.CacheFormats();
            int formatId;
            lock (this.formatStringIdMap.SyncRoot)
            {
                if (this.formatStringIdMap.TryGetValue(formatString, out formatId))
                {
                    return formatId;
                }
                else
                {
                    string procName = "mvm_get_format_id";
                    // assume GLOBAL.target_login is set
                    string globalTargetLoginOid = this.mvm.globalContext["target_login"];
                    StaticDbLoginInfo dbInfo = StaticDbLoginInfo.FromObjectId(this.mvm, globalTargetLoginOid);
                    Dictionary<string, string> procParams = new Dictionary<string, string>();

                    // break the format string into 4000 byte chunks.
                    double chunksDbl = formatString.Length / 4000;
                    int chunks = (int)Math.Ceiling(chunksDbl);
                    for (int i = 0; i <= chunks; i++)
                    {
                        string paramName = "p_format_string" + (i + 1);
                        int startIdx = 4000 * i;
                        int len = 4000;
                        if ((startIdx + len) > formatString.Length) len = formatString.Length - startIdx;
                        string paramValue = formatString.Substring(startIdx, len);
                        procParams[paramName] = paramValue;
                    }
                    DbProcInfo.CallProc(dbInfo.type, dbInfo.server, dbInfo.db, dbInfo.user, dbInfo.pw, procName, procParams);
                    formatId = procParams["p_format_id"].ToInt();
                    this.RegisterFormat(formatId, formatString);
                }
            }
            return formatId;
        }
        public string[] GetFormatFields(int formatId)
        {
            this.CacheFormats();
            string[] formatFields;
            if (this.formatIdFieldsMap.TryGetValue(formatId, out formatFields))
                return formatFields;

            string globalTargetLoginOid = this.mvm.globalContext["target_login"];
            StaticDbLoginInfo dbInfo = StaticDbLoginInfo.FromObjectId(this.mvm, globalTargetLoginOid);

            var rows = DbUtils.DbQueryToListOfDictionary(dbInfo.type, dbInfo.server, dbInfo.db, dbInfo.user, dbInfo.pw, "select format_string1,format_string2,format_string3,format_string4,format_string5 from agg_field_formats where format_id=" + formatId);
            if (rows.Count == 0) throw new Exception("Error, unknown formatId [" + formatId + "]");
            if (rows.Count > 1) throw new Exception("Error, more than one row in agg_field_formats with formatId [" + formatId + "]");
            var row = rows[0];
            var formatString =
                row["format_string1"]
            + row["format_string2"]
            + row["format_string3"]
            + row["format_string4"]
            + row["format_string5"];
            this.RegisterFormat(formatId, formatString);

            if (this.formatIdFieldsMap.TryGetValue(formatId, out formatFields))
                return formatFields;


            throw new Exception("Error, unexpected situation getting format string for format_id [" + formatId + "]");

        }

        #endregion



        #region UFNs and TFIDs

        // UfnEncoder["fieldName"]=ufn
        protected readonly ZeroBasedEncoder<string> UfnEncoder = new ZeroBasedEncoder<string>();

        // TfidEncoder and TfidUfnIdxMap are kept in sync
        // TfidEncoder[(field1,field2)]=int
        protected readonly ZeroBasedEncoder<StringArray> TfidEncoder = new ZeroBasedEncoder<StringArray>();

        // idx -1 means field not defined for the format.
        // TfidUfnIdxMap[tfid][ufn]=formatFieldsIdx
        protected readonly List<int[]> TfidUfnIdxMap = new List<int[]>();

        // Feedback name tie objects together so that we instanciate the widest format
        // that we need.
        // FeedbackEncoder["feedbackName"]=feedbackId
        protected readonly ZeroBasedEncoder<string> FeedbackEncoder = new ZeroBasedEncoder<string>();

        // feedback tracking is local to a node even though feedback names span nodes.
        // feedbackIdFormatMap[feedbackId]=formatId that should be used on instanciation.
        protected readonly List<int> feedbackIdFormatMap = new List<int>();

        /// <summary>
        /// This is how ending formatId feedback so next time we instanciate an object with the 
        /// feedbackId or expand an existing object it uses the wider formatId. Every nodes 
        /// manages its own feedback so it is possible that feebackNames have different formats
        /// which is a performance win.
        /// </summary>
        /// <param name="feedbackId"></param>
        /// <param name="tfid"></param>
        public void FeedbackFormat(int feedbackId, int tfid)
        {
            //logger.Info("FEEDBACK_ID [" + feedbackId + "/"+this.GetFeedbackName(feedbackId)+"] gets TFID [" + tfid + "]");
            lock (feedbackIdFormatMap)
            {
                int existingTfid;
                if (feedbackId >= this.feedbackIdFormatMap.Count)
                {
                    // no existing format so set it and return
                    this.feedbackIdFormatMap.Set(feedbackId, tfid, -1);
                    return;
                }
                else
                {
                    existingTfid = this.feedbackIdFormatMap[feedbackId];
                    if (existingTfid == tfid)
                    {
                        // same formats so return
                        return;
                    }
                    if (existingTfid == -1)
                    {
                        // no existing format so set it and return
                        this.feedbackIdFormatMap[feedbackId]= tfid;
                        return;
                    }
                }
                // if we get here we need to expand the format id
                // Append new fields from the passed format to the existing one. This means
                // that if you ever compiled and remember field positions for a feedback id
                // those positions are guaranteed to remain valid. This is very important!!
                List<string> newTfidFields=this.GetTfidFields(existingTfid).ToList();
                foreach (string field in this.GetTfidFields(tfid))
                {
                    if (!newTfidFields.Contains(field))
                    {
                        newTfidFields.Add(field); 
                    }
                }
                int nextTfid=this.GetTfid(newTfidFields.ToArray());
                feedbackIdFormatMap[feedbackId] = nextTfid;
            }
        }

        /// <summary>
        /// Returns the feedback Tfid or -1 if not found.
        /// </summary>
        /// <param name="feedbackId"></param>
        /// <returns></returns>
        public int GetFeedbackFormat(int feedbackId)
        {
            lock (feedbackIdFormatMap)
            {
                return this.feedbackIdFormatMap.GetValueDefaulted(feedbackId, -1);

            }
        }


        /// <summary>
        /// Given a feedbackName return its feedbackId
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public abstract int GetFeedbackId(string feedbackName);

        /// <summary>
        /// Get or create a feedback id for the feedback name
        /// </summary>
        /// <param name="feedbackName"></param>
        /// <returns></returns>
        public int GetCreateFeedbackId(string feedbackName)
        {
            return FeedbackEncoder.GetCreateIdx(feedbackName);
        }

        /// <summary>
        /// Given a feedbackId, return the feedbackName
        /// </summary>
        /// <param name="ufn"></param>
        /// <returns></returns>
        public abstract string GetFeedbackName(int feedbackId);

        /// <summary>
        /// Given a fieldName return its UFN
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public abstract int GetUfn(string fieldName);

        public int GetCreateUfn(string fieldName)
        {
            //Debug.Assert(fieldName == null, "not expecting fieldName to be null");
            return UfnEncoder.GetCreateIdx(fieldName);
        }

        /// <summary>
        /// Given a UFN retun its fieldName
        /// </summary>
        /// <param name="ufn"></param>
        /// <returns></returns>
        public abstract string GetFieldName(int ufn);

        /// <summary>
        /// Returns the field index for a tfid/ufn. -1 means field not found.
        /// </summary>
        /// <param name="tfid"></param>
        /// <param name="ufn"></param>
        /// <returns></returns>
        public abstract int GetFormatFieldIdx(int tfid, int ufn);

        /// <summary>
        /// Returns a array that maps UFN->fieldIndex
        /// </summary>
        /// <param name="tfid"></param>
        /// <returns></returns>
        public abstract int[] GetUfnIdxMap(int tfid);

        // Check to make sure we haven't dropped fields
        public void ValidateTfids(int tfid, int nextTfid, string msg)
        {
            if (tfid == nextTfid) return;
            var fromFields = this.GetTfidFields(tfid);
            var toFields = this.GetTfidFields(nextTfid);
            Dictionary<string, bool> toFieldsDic = new Dictionary<string, bool>();
            foreach (var f in toFields)
            {
                toFieldsDic[f] = true;
            }
            foreach (var f in fromFields)
            {
                if (!toFieldsDic.ContainsKey(f))
                {
                    logger.Fatal("FOUND BUG, dropped field [" + f + "] tfid=" + tfid + " nextTfit=" + nextTfid + " msg=" + msg);
                }
            }
        }
        
        /// <summary>
        /// Returns the field index for a tfid/ufn, sets nextTfid to a new tfid should it be needed.
        /// </summary>
        /// <param name="tfid"></param>
        /// <param name="ufn"></param>
        /// <returns></returns>
        public int GetFormatFieldIdx(int feedbackId, int tfid, int ufn, out int nextTfid)
        {
            int formatFieldIdx;
            lock (TfidUfnIdxMap)
            {
                int[] ufnIdxMap = GetUfnIdxMap(tfid);
                if (ufn < ufnIdxMap.Length)
                {
                    formatFieldIdx = ufnIdxMap[ufn];
                    if (formatFieldIdx >= 0)
                    {
                        // field exists in format
                        nextTfid = tfid;
                        //this.ValidateTfids(tfid, nextTfid,"1");
                        return formatFieldIdx;
                    }
                }
            }

            // If we are here, we need to change the TFID to accommodate the new
            // format. If there is a feedback id, we want to start with that and
            // expand that if necessary.
            if (feedbackId >= 0)
            {
                int feedbackTfid = this.GetFeedbackFormat(feedbackId);
                if (feedbackTfid >= 0 && feedbackTfid != tfid)
                {
                    var feedbackFields = this.GetTfidFields(feedbackTfid);
                    var currentFields = this.GetTfidFields(tfid).ToList();
                    var currentDic = new Dictionary<string, int>();
                    int idx = 0;
                    foreach (var f in currentFields)
                    {
                        currentDic[f] = idx++;
                    }
                    foreach (var f in feedbackFields)
                    {
                        if (!currentDic.ContainsKey(f))
                        {
                            currentFields.Add(f);
                            currentDic[f] = idx++;
                        }
                    }
                    string ufnFieldName = this.GetFieldName(ufn);
                    if (!currentDic.ContainsKey(ufnFieldName))
                    {
                        currentFields.Add(ufnFieldName);
                        currentDic[ufnFieldName] = idx++;
                    }
                    nextTfid = this.GetTfid(currentFields.ToArray());
                    //this.ValidateTfids(tfid, nextTfid, "2");
                    this.FeedbackFormat(feedbackId, nextTfid);
                    return currentDic[ufnFieldName];
                }
            }

            //logger.Info("NEED NEW TFID...");
            // if we are here the field does not exist in the format so we need
            // to get a new tfid has it.
            nextTfid = this.GetNextTfid(tfid, ufn);
            lock (TfidUfnIdxMap)
            {
                formatFieldIdx = TfidUfnIdxMap[nextTfid][ufn];
            }
            // feedback the format for next time
            if (feedbackId >= 0)
            {
                this.FeedbackFormat(feedbackId, nextTfid);
            }
            //this.ValidateTfids(tfid, nextTfid, "3");
            return formatFieldIdx;

        }

        
        // need a fast way to say tfid.Append(ufn) => nextTfid
        // TfidAppendUfnToNextTfidMap[tfid][appendUfn]=nextTfid
        //private readonly List<int[]> TfidAppendUfnToNextTfidMap=new List<int[]>();
        // can treat this structure as a cache because we can rebuild it.
        private int GetNextTfid(int startTfid, int appendUfn)
        {
            // TBD: Add a caching layer here. this is more expensive then it needs to be.
            string[] currFields = this.GetTfidFields(startTfid);
            string[] nextfields = new string[currFields.Length + 1];
            string appendField = GetFieldName(appendUfn);
            Array.Copy(currFields, nextfields, currFields.Length);
            nextfields[nextfields.Length - 1] = appendField;
            int nextTfid = GetTfid(nextfields);
            //logger.Info("START TFID ["+startTfid+"] CREATED TFID [" + nextTfid + "] =" + nextfields.JoinStrings(","));
            return nextTfid;
        }

        // this manages TFID transitions plans
        // TfidTransitionPlans{fromTfid,tfid}=plan
        public Dictionary<Tuple<int,int>,int[]> TfidTransitionPlans = new Dictionary<Tuple<int,int>,int[]>();
        public int[] GetTfidTransitionPlan(int fromTfid, int toTfid)
        {
            lock (TfidTransitionPlans)
            {
                Debug.Assert(fromTfid != toTfid);
                Debug.Assert(fromTfid >= 0);
                Debug.Assert(toTfid >= 0);

                // see if we have a cached plan
                var planKey = new Tuple<int, int>(fromTfid, toTfid);
                int[] plan;
                if (this.TfidTransitionPlans.TryGetValue(planKey, out plan))
                {
                    return plan;
                }

                var fromFields = this.GetTfidFields(fromTfid);
                var toFields = this.GetTfidFields(toTfid);
                plan = fromFields.BuildArrayCopyPlan(toFields);
                TfidTransitionPlans[planKey] = plan;
                return plan;
            }
        }

        /// <summary>
        /// Given an ordered array of field names this returns the Tfid, creating one if needed.
        /// </summary>
        /// <param name="fieldNames"></param>
        /// <returns></returns>
        public abstract int GetTfid(string[] fieldNames);


        /// <summary>
        /// Given an ordered array of field names this returns the Tfid, creating one if needed.
        /// </summary>
        /// <param name="fieldNames"></param>
        /// <returns></returns>
        public int GetCreateTfid(string[] fieldNames)
        {
            StringArray fields = new StringArray(fieldNames);
            bool isNew;
            int tfid = TfidEncoder.GetCreateIdx(fields, out isNew);
            if (isNew)
            {
                lock (TfidUfnIdxMap)
                {
                    // don't know how big this is going to get so use a list then
                    // cut down to array.
                    List<int> ufnIdxMapList = new List<int>(fieldNames.Length);
                    for (int fldIdx = 0; fldIdx < fieldNames.Length; fldIdx++)
                    {
                        string fieldName = fieldNames[fldIdx];
                        int ufn = GetUfn(fieldName);
                        ufnIdxMapList.Set(ufn, fldIdx,-1);
                    }
                    TfidUfnIdxMap.Set(tfid, ufnIdxMapList.ToArray());
                    //logger.Info("CREATE TFID: "+tfid+":"+fieldNames.JoinStrings(","));
                }
            }
            return tfid;
        }

        /// <summary>
        /// Given a tfid, return the field names.
        /// </summary>
        /// <param name="tfid"></param>
        /// <returns></returns>
        public abstract string[] GetTfidFields(int tfid);


        // Given a tfid, return the ufns // TUNE THIS so it doesn't derive
        public int[] GetTfidUfns(int tfid)
        {
            string[] tfidFields = this.GetTfidFields(tfid);
            int[] ufns = new int[tfidFields.Length];
            for (int i = 0; i < tfidFields.Length; i++)
            {
                string fieldName = tfidFields[i];
                ufns[i] = GetUfn(fieldName);
            }
            return ufns;
        }


        
        # endregion




        #region Relative Change Numbers (RCN) and snapshots

        /// <summary>
        /// Gets a new RCN batch from the super node.
        /// </summary>
        /// <param name="reqBatchSize"></param>
        /// <param name="batchStart"></param>
        /// <param name="batchSize"></param>
        public abstract void GetRcnBatch(int reqBatchSize, out long batchStart, out int batchSize);


        /// <summary>
        /// This is the Relative Change Number (RCN) lock. 
        /// </summary>
        private object rcnLock = new object();
        private long rcnNext = 1;
        private long rcnLast = -1;
        private int RCN_BATCH_SIZE = 1000;

        /// <summary>
        /// returns a number where all modification after this point will have a greater number than this number. The first snapshot is '1'.
        /// </summary>
        /// <returns></returns>
        public long GetSnapshot()
        {
            long snapshotRcn;
            lock (rcnLock)
            {
                for(int i=0;i<=1;i++){
                    if (rcnNext <= rcnLast)
                    {
                        snapshotRcn = rcnNext;
                        rcnNext++;
                        return snapshotRcn;
                    }
                    else
                    {
                        this.GetNewRcnBatch();
                    }
                }
                throw new Exception("Failed to get RCN batch");
            }
        }


        /// <summary>
        /// Fetches a new rcn batch even if the current batch has RCNs left.
        /// </summary>
        public void GetNewRcnBatch()
        {
            lock (rcnLock)
            {
                // fetch a new batch.
                long batchStart;
                int batchSize;
                this.GetRcnBatch(RCN_BATCH_SIZE, out batchStart, out batchSize);
                this.rcnNext = batchStart;
                this.rcnLast = batchStart + batchSize - 1;
            }
        }

        /// <summary>
        /// This is the current relative change number (RCN). This number is used for delta tracking objects. 
        /// It starts at 1 and counts up anytime someone gets a snapshot. A snap shot is simply an RCN.
        /// </summary>
        public long CurrentRcn
        {
            get
            {
                lock (rcnLock)
                {
                    // current the next rcn is invalid, need a new batch
                    if (this.rcnNext > this.rcnLast) this.GetNewRcnBatch();
                    return this.rcnNext;
                }
            }
        }

        /// <summary>
        /// returns the current rcn
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        /// <category>Delta Tracking</category>
        [MvmExport("current_rcn")]
        public static string current_rcn(ModuleContext mc)
        {
            return mc.mvmCluster.CurrentRcn.ToString();
        }

        /// <summary>
        /// returns the current rcn
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        /// <category>Delta Tracking</category>
        [MvmExport("get_new_rcn_batch")]
        public static void get_new_rcn_batch(ModuleContext mc)
        {
            mc.mvmCluster.GetNewRcnBatch();
        }

        /// <summary>
        /// Returns a new snapshot.
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        /// <category>Delta Tracking</category>
        [MvmExport("get_snapshot")]
        public static string get_snapshot(ModuleContext mc)
        {
            return mc.mvmCluster.GetSnapshot().ToString();
        }

        #endregion


        /// <summary>
        /// Returns the cluster node associated with the node id, asking the
        /// supernode if it doesn't already know about the nodeId.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public abstract MvmClusterNode GetClusterNode(int nodeId);

        #region Connection status to nodes

        // get permission to connect.
        protected AutoResetEvent newConnection = new AutoResetEvent(false);
        private Dictionary<int,ConnStatus> nodeIdConnStatus=new Dictionary<int,ConnStatus>();
        public bool GetPermissionToConnect(int nodeId, bool isOutgoing)
        {
            if (nodeId == this.NodeId) return true;
            lock (nodeIdConnStatus)
            {
                // After initial setup, each node will have a connection to the supernode; any given node
                // will also connect on demand to whatever other nodes it needs to talk to.  The main
                // purpose of this permissions check is to prevent duplicate connections between nodes
                // when they happen to try to connect to each other at the same time.
                //
                // We also allow outgoing connections to the supernode to bypass this check, as they can
                // only happen in the case of an unrelated process (e.g. a standalone profiler trying to
                // get started) that is trying to handshake with an existing supernode.
                // (Incoming connections from the supernode should be an error, so don't just allow all
                // supernode connections.)
                if ((!nodeIdConnStatus.ContainsKey(nodeId)) ||
                    (nodeIdConnStatus[nodeId] == ConnStatus.Disconnected) ||
                    (isOutgoing == true && nodeId == 0))
                {
                    nodeIdConnStatus[nodeId] = ConnStatus.Connecting;
                    return true;
                }
                return false;
            }
        }

        // cancel a connection after we've got permission to connect.
        public void CancelConnection(int nodeId)
        {
            if (nodeId == this.NodeId) return;
            lock (nodeIdConnStatus)
            {
                if (!nodeIdConnStatus.ContainsKey(nodeId))
                    throw new Exception("Cannot cancel a connection that does not any status");
                if (nodeIdConnStatus[nodeId] != ConnStatus.Connecting)
                    throw new Exception("Cannot cancel a connection that does not have status of Connecting. Current status is " + nodeIdConnStatus[nodeId].ToString());
                nodeIdConnStatus[nodeId] = ConnStatus.Disconnected;
            }
        }

        public void RegisterConnection(int nodeId)
        {
            if (nodeId == this.NodeId) return;
            lock (nodeIdConnStatus)
            {
                if (!nodeIdConnStatus.ContainsKey(nodeId))
                    throw new Exception("Cannot register a connection if we never got permission to connect");
                if (nodeIdConnStatus[nodeId] != ConnStatus.Connecting)
                    throw new Exception("Cannot register a connection that does not have status of Connecting. Current status is " + nodeIdConnStatus[nodeId].ToString());
                nodeIdConnStatus[nodeId] = ConnStatus.Connected;
                this.newConnection.Set();
            }
        }

        // Remap a connection onto a newly assigned nodeId
        public void RemapConnection(int oldNodeId, int newNodeId)
        {
            if (oldNodeId == newNodeId) return;
            lock (nodeIdConnStatus)
            {
                if (!nodeIdConnStatus.ContainsKey(oldNodeId))
                    throw new Exception("Cannot remap a connection that does not exist");
                nodeIdConnStatus[newNodeId] = nodeIdConnStatus[oldNodeId];
                nodeIdConnStatus.Remove(oldNodeId);
            }
        }

        public ConnStatus GetConnStatus(int nodeId){
            lock (nodeIdConnStatus)
            {
                if (!nodeIdConnStatus.ContainsKey(nodeId))
                    return ConnStatus.NotConnected;
                return nodeIdConnStatus[nodeId];
            }
        }

        # endregion


        public void DisconnectNonSuperNodes()
        {
            var nonSuperNodes = this.GetKnownNodes().Where(n => !n.IsSuper);
            foreach (var node in nonSuperNodes)
            {
                node.Disconnect();
            }
        }

        /// <summary>
        /// Stop receiver threads, but leaves the socket open.
        /// </summary>
        public void StopReceiverThreads()
        {
            this.mvm.DisableSlaveLogging();
            lock (this.KnownNodes)
            {
                foreach (MvmClusterNode node in this.KnownNodes.Values)
                {
                    logger.Debug("Shutdown receiver thread for node_id:" + node.nodeId);
                    node.SocketHandler.messageReceiver.StopReceiving();
                }
            }
        }

        private AsyncShutdown shutdownObj;
        private Thread shutdownThread;
        private string shutdownThreadName;

        /// <summary>
        /// This is an async command b/c i cannot tie up the receiver...
        /// </summary>
        /// <returns></returns>
        public void Shutdown(long workId)
        {
            this.shutdownObj = new AsyncShutdown(this, workId);
            this.shutdownThread= new Thread(this.shutdownObj.Shutdown);
            this.shutdownThreadName = "shutdown";
            this.shutdownThread.Name = this.shutdownThreadName;
            this.shutdownThread.Start();
        }

        #region non interupt work

        public bool RequestMaxWaitingWork()
        {
            MvmClusterNode maxNode = null;
            int? maxPriority = null;

            foreach (var node in this.GetKnownNodes())
            {
                if (node.IsConnected)
                {
                    //this.mvm.Log("CHECK SOCKET HANDLER FOR NODE_ID=" + node.nodeId + " TO SEE IF THEY NOTIFIED US OF WORK");
                    SocketHandler socketHandler = node.SocketHandler;
                    int? priority = socketHandler.MaxWaitingPriority;
                    if (!priority.HasValue) continue;
                    //this.mvm.Log("SOCKET HANDLER FOR NODE_ID=" + node.nodeId + " DID NOTIFY US OF PENDING WORK");
                    if (maxNode == null || priority > maxPriority)
                    {
                        maxNode = node;
                        maxPriority = priority;
                    }
                }
            }
           
            // if we have any work send a message to get it
            if (maxNode != null && maxPriority < MessagePriority.Interupt)
            {
                //this.mvm.Log("REQUEST MAX WAIT WORK FROM NODE_ID=" + maxNode.nodeId);
                maxNode.SocketHandler.messageReceiver.RequestAnyWork();
                return true;
            }

            return false;
        }

        #endregion
    }

    public enum ConnStatus
    {
        Connecting,
        Connected,
        Disconnected,
        NotConnected,
        Uknown
    }
}
