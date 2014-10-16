using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using System.IO;

namespace MVM
{
    public class UsageHookObject
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public HookState hookState = HookState.Idle;

        public enum HookState
        {
            Idle,
            ReadOnly,
            ReadWrite
        }

        /// <summary>
        /// Get the name to store the hook in GlobalContext. HookId is a counter and is not safe w/o prefix.
        /// </summary>
        /// <param name="hookId"></param>
        /// <returns></returns>
        public static string GetHookName(string hookId)
        {
            string hookName = "UsageHook_" + hookId;
            return hookName;
        }

        public readonly string hookId;
        public readonly string sortField;
        public readonly int idAccCount; //not used
        private readonly int maxOpenFiles;
        public readonly string tablesDir; // not used
        public readonly string joinDir; // not used
        public readonly string cursor1Dir;
        public readonly string cursor2Dir;
        public readonly string partitioningDir; // not used
        public readonly string streamingDir; //not used
        public readonly string releasedDir;
        public readonly string persistedDir; //not used
        public readonly string hookName;
        public readonly MvmEngine mvm;
        public readonly BufferedFileSystem bfs;
        private long objectCursorCtr = 0;
        private bool toggle = false;
        public bool readWriteInProgress = false;

        public MergeSortReaderWriter<Text, IObjectData> myReader;
        public MergeSortReaderWriter<Text, IObjectData> myWriter;

        private IEnumerator<KeyValuePair<Text, IObjectData>> _myReaderEnumerator = null;
        private IEnumerator<KeyValuePair<Text,IObjectData>> myReaderEnumerator{
            get
            {
                if (this._myReaderEnumerator == null)
                {
                    this._myReaderEnumerator = myReader.GetEnumerator();
                }
                return this._myReaderEnumerator;
            }
         
        }
        

        public bool readOnlyPass = false;
        private Dictionary<int, List<string>> fetchCache = new Dictionary<int, List<string>>();

        // Read write target object is really just current object
        public ObjectDataFormattedDelta readWriteTargetObject;


        public IObjectData Current
        {
            get
            {
                return this.myReaderEnumerator.Current.Value;
            }
        }


        public readonly int maxSortObjects;

        public string hookReadDir
        {
            get
            {
                return this.toggle ? this.cursor2Dir : this.cursor1Dir;
            }
        }
        public string hookWriteDir
        {
            get
            {
                return this.toggle ? this.cursor1Dir : this.cursor2Dir;
            }
        }
        // private LockingLruCache<string, IBufferedFile>.LockingLruCacheItem sortedWriteLruLock;
        //private ObjectQueueBufferedFile sortedWriteFileQueue;
        public string[] HookReadFiles
        {
            get
            {
                return this.bfs.Dir(this.hookReadDir).ToArray();
            }
        }


        # region fetch released usage

        /// <summary>
        /// Queues a request for usage from a node for an idAcc
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="idAcc"></param>
        public void FetchLocal(int nodeId, string idAcc)
        {
            if (!this.fetchCache.ContainsKey(nodeId)) this.fetchCache[nodeId] = new List<string>();
            this.fetchCache[nodeId].Add(idAcc);
        }

        /// <summary>
        /// Fetches queued up idAcc requests for usage and returns when all the usage has been added to the hook.
        /// </summary>
        public void FetchFlush()
        {
            // Pull out the local usage if any
            List<string> localUsage = null;
            fetchCache.TryGetRemoveValue(mvm.nodeId, out localUsage);

            // determine if we need to remote fetch and local fetch
            bool needRemoteFetch = fetchCache.Count > 0;
            bool needLocalFetch = localUsage != null && localUsage.Count > 0;

            WorkBatch batch = null;
            if (needRemoteFetch)
            {
                //logger.Info("need to fetch usage from remote nodes: {0}", this.fetchCache.Keys.JoinStrings(","));
                // Need to wait for a response so do this through the work manager. We
                // create a single batch with separate work for each remote node.
                long batchId = mvm.remoteWorkMgr.CreateBatch();
                batch = this.mvm.remoteWorkMgr.LookupBatch(batchId);

                // Send message to all the remote nodes requesting released usage for the passed id_accs
                // do this in a single batch per node...
                foreach (var entry in this.fetchCache)
                {
                    int nodeId = entry.Key;
                    List<string> idAccs = entry.Value;

                    WorkInfo w = this.mvm.remoteWorkMgr.CreateWork(batchId);
                    w.procName = "UsageRequestMessage";
                    w.nodeId = nodeId;
                    w.priority = MessagePriority.Interupt;
                    w.status = WorkStatus.WaitingToStart;

                    // create and send the message.
                    var msg = new UsageRequestMessage(w.workId, idAccs, this.hookId);
                    //logger.Info("sending UsageRequestMessage workId={0} to nodeId={1}", msg.workId, nodeId);

                    // send the message...
                    var clusterNode = this.mvm.mvmCluster.GetClusterNode(nodeId);
                    //logger.Info("got clusterNode");

                    var socketHandler = clusterNode.SocketHandler;
                    //logger.Info("got socketHandler");

                    socketHandler.messageOutbox.Add(msg);
                    //logger.Info("send msg to outbox");
                }
            }

            // now deal with fetching my local released usage
            if (needLocalFetch)
            {
                //logger.Info("Fetch local released usage on my node");
                this.ServiceLocalUsageRequest(localUsage);
            }

            if (needRemoteFetch)
            {
                //logger.Info("waiting for all remote fetches to complete");
                // now wait for the batch to complete so i know i have all the remote usage.
                BlockingWaitBatchEvent blockingWait = new BlockingWaitBatchEvent();
                batch.AddBatchCompleteEvent(blockingWait);
                blockingWait.WaitForBatchComplete();
                //logger.Info("Usage fetch complete:");
            }
        }


        private static Dictionary<string, Dictionary<string, bool>> GroupIdAccsByFile(List<string> idAccs)
        {
            Dictionary<string, Dictionary<string, bool>> fileIdAccs = new Dictionary<string, Dictionary<string, bool>>();
            lock (LocalIdPayeeFileNameMap)
            {
                foreach (var idAcc in idAccs)
                {
                    string file = null;
                    if (LocalIdPayeeFileNameMap.TryGetValue(idAcc, out file))
                    {
                        if (!fileIdAccs.ContainsKey(file)) fileIdAccs[file] = new Dictionary<string, bool>();
                        fileIdAccs[file][idAcc] = true;
                    }
                }
            }
            return fileIdAccs;
        }


        /// <summary>
        /// This scans incompete usage for the passed idAccs writes the locally
        /// </summary>
        /// <param name="requesterSocketHandler"></param>
        /// <param name="workId"></param>
        /// <param name="hookId"></param>
        /// <param name="idAccs"></param>
        public void ServiceLocalUsageRequest(List<string> idAccs)
        {
            // Group the idAccs by the file they are in
            // fileIdAccs{file}{id_acc}=true
            Dictionary<string, Dictionary<string, bool>> fileIdAccs = GroupIdAccsByFile(idAccs);
            foreach (var entry in fileIdAccs)
            {
                string fileName = entry.Key;
                //logger.Info("Grepping local released usage file:" + fileName);
                Dictionary<string, bool> idAccsMap = entry.Value;
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024 * 1024))
                {
                    BinaryReader br = new BinaryReader(fs);
                    foreach (PayloadEntry pe in new PayloadFileLooper(br))
                    {
                        string idPayee = pe.key;
                        int payloadLength = pe.length;
                        if (idAccsMap.ContainsKey(idPayee))
                        {
                            if (!this.DeserializeObjectToHook(br))
                            {
                                throw new Exception("Not,expecting object but hit eof when deserializing local usage");
                            }
                        }
                        else
                        {
                            //logger.Info("Skipping deserializing usage for idPayee:" + idPayee);
                            br.ReadBytes(payloadLength); // just skip ahead w/o deserializing.
                        }
                    }
                }
                // remove the entry from the hash since the usage is no longer in there
                foreach (var idAcc in idAccsMap.Keys)
                {
                    //logger.Info("Remove idAcc to file mapping for idAcc={0}", idAcc);
                    RemoveIdPayeeToFile(idAcc);
                }
            }
        }

        private object deserializeObjectToHookLock = new object();

        /// <summary>
        /// Derserializes an object from the binary reader and and writes it to the usage hook. In the case where
        /// we are fetching usage over the wire, multiple receiver thread could be calling this at the same time.
        /// When this happens we must ensure exclusive access to this so use a deserializeObjectHookLock.
        /// </summary>
        /// <param name="br"></param> 
        /// <returns></returns>
        private bool DeserializeObjectToHook(BinaryReader br)
        {

            //logger.Info("deserializing usage event");
            IObjectData tmpObject;
            if (!ObjectDataBase.Deserialize(br, this.mvm.mvmCluster, out tmpObject))
            {
                //logger.Info("hit end of file");
                return false;
            }
            lock (deserializeObjectToHookLock)
            {
                this.mvm.objectCache.AddOrMergeObject(tmpObject);
                this.AddObjectToHook(tmpObject, false); // TBD Need to get smarter here so we can say inorder=true.
                // if the we didn't store the object in any structs, delete it.
                if (tmpObject.RefCount == 0)
                {
                    tmpObject.Delete();
                }
            }
            return true;
        }

        private void AddDeletedUsage(IObjectData delUsage)
        {
            string idView = delUsage["id_view"];
            string delOid = delUsage.objectId;
            mvm.Log("BEFORE ADD DESERIALIZED deleted usage with oid=" + delUsage.objectId + ", refs=" + delUsage.RefCount);
            MemoryIndexSync memIdx = mvm.globalContext.GetNamedClassInst("USAGE_TO_DELETE") as MemoryIndexSync;
            memIdx.IndexInsert(null, new List<string> { idView, delOid });
            mvm.Log("AFTER ADD DESERIALIZED deleted usage with oid=" + delUsage.objectId + ", refs=" + delUsage.RefCount);
        }


        /// <summary>
        /// Greps released files for passed idAccs and send UsageResponseMessages over the wire to the requester.
        /// </summary>
        /// <param name="requesterSocketHandler"></param>
        /// <param name="workId"></param>
        /// <param name="hookId"></param>
        /// <param name="idAccs"></param>
        public static void ServiceUsageRequest(SocketHandler requesterSocketHandler, long workId, string hookId, List<string> idAccs)
        {
            var mvm = requesterSocketHandler.mvm;
            // hash the idAccs and group them by the files they are in...
            // for each file, scan it and pull out the usage for the idAccs writing the usage to a buffer
            // anytime buffer is > X byte cut a response message.
            // empty buffer and repeat
            // when we're totally done set workId to positive number and sent remaining...
            Dictionary<string, Dictionary<string, bool>> fileIdAccs = GroupIdAccsByFile(idAccs);

            int messageBufferStart = 1024;
            int messageBufferMax = 1 * 1024 * 1024;
            MemoryStream messageBuffer = new MemoryStream(messageBufferStart);

            // open up the local fetch file to write to
            foreach (var entry in fileIdAccs)
            {
                string fileName = entry.Key;
                //logger.Info("Grepping local released usage file:" + fileName);
                Dictionary<string, bool> idAccsMap = entry.Value;
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024 * 1024))
                {
                    BinaryReader br = new BinaryReader(fs);
                    foreach (PayloadEntry pe in new PayloadFileLooper(br))
                    {
                        string idPayee = pe.key;
                        int payloadLength = pe.length;
                        byte[] payload = br.ReadBytes(payloadLength);
                        //logger.Info("got record for idPayee={0}, payload len={1},payload real len={2)", idPayee, payloadLength, payload.Length);
                        if (idAccsMap.ContainsKey(idPayee))
                        {
                            //logger.Info("Saving usage for idPayee:" + idPayee);
                            messageBuffer.Write(payload, 0, payload.Length);

                            // if over the limit, send what we got.
                            if (messageBuffer.Length > messageBufferMax)
                            {
                                byte[] usageBuffer = messageBuffer.ToArray();
                                long incompleteWorkId = 0 - workId;
                                UsageResponseMessage response = new UsageResponseMessage(incompleteWorkId, hookId, usageBuffer);
                                //logger.Info("Sending partial usage response with workId=[{0}] usageBuffer.len={1} ", response.workId, usageBuffer.Length);
                                requesterSocketHandler.messageOutbox.Add(response);
                                // since we called ToArray on the message buffer, we reuse it. Just need to rewind it.
                                messageBuffer.SetLength(0);
                                messageBuffer.Position = 0;
                            }
                        }
                        else
                        {
                            //logger.Info("Skipping writing usage for idPayee:" + idPayee);
                        }
                    }
                }

                // remove the entry from the hash since the usage is no longer in there
                foreach (var idAcc in idAccsMap.Keys)
                {
                    //logger.Info("Remove idAcc to file mapping for idAcc={0}", idAcc);
                    RemoveIdPayeeToFile(idAcc);
                }
            }
            {
                // Send final response using the postive work id.
                byte[] usageBuffer = messageBuffer.ToArray();
                UsageResponseMessage response = new UsageResponseMessage(workId, hookId, usageBuffer);
                //logger.Info("Sending final usage response with workId={0} usageBuffer.len={1} ", response.workId, usageBuffer.Length);
                requesterSocketHandler.messageOutbox.Add(response);
            }
        }


        /// <summary>
        /// Write usage that was fetched over the wire to the usage hook.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="usageBuffer"></param>
        public void WriteFetchedUsage(int nodeId, byte[] usageBuffer)
        {
            //logger.Info("WriteFetchUsage from nodeId={0}, usageBuffer.length={1}", nodeId, usageBuffer.Length);
            MemoryStream memStream = new MemoryStream(usageBuffer);
            BinaryReader breader = new BinaryReader(memStream);
            //int objNo = 0;
            while (this.DeserializeObjectToHook(breader))
            {
                //logger.Info("read obj number =" + (++objNo).ToString() + ", now curPos=" + memStream.Position);
            }
        }


        private static Dictionary<string, string> LocalIdPayeeFileNameMap = new Dictionary<string, string>();
        private static void MapIdPayeeToFile(string idPayee, string fileName)
        {
            lock (LocalIdPayeeFileNameMap)
            {
                string existingFileName;
                if (LocalIdPayeeFileNameMap.TryGetValue(idPayee, out existingFileName))
                {
                    if (existingFileName.Equals(fileName)) return;
                    throw new Exception("Error, usage for an id payee should only be in one local file: existing file=[" + existingFileName + "] new file [" + fileName + "]");
                }
                LocalIdPayeeFileNameMap[idPayee] = fileName;
            }
        }
        private static bool RemoveIdPayeeToFile(string idPayee)
        {
            lock (LocalIdPayeeFileNameMap)
            {
                return LocalIdPayeeFileNameMap.Remove(idPayee);
            }
        }


        // the usage hook has a local file associated to it.
        // this file is opened on demand, and closed when the hook is closed.
        // Only instanciate the fileStream when we need it. Make sure this is synchronous.
        private string releaseFileName { get { return Path.Combine(this.releasedDir, this.hookId + ".txt"); } }
        private BinaryWriter _releaseBinaryWriterValue;
        private BinaryWriter releaseBinaryWriter
        {
            get
            {
                if (this._releaseBinaryWriterValue == null)
                {
                    this._releaseBinaryWriterValue = new BinaryWriter(this.releasefileStream);
                }
                return this._releaseBinaryWriterValue;
            }
        }
        private FileStream _releasefileStreamValue;
        private FileStream releasefileStream
        {
            get
            {
                if (this._releasefileStreamValue == null)
                {
                    //logger.Info("Opening filestream to {0}", this.releaseFileName);
                    FileInfo fileInfo = new FileInfo(this.releaseFileName);
                    fileInfo.Directory.CreateIfNotThere();
                    this._releasefileStreamValue = new FileStream(
                        this.releaseFileName,
                        FileMode.OpenOrCreate,
                        FileAccess.ReadWrite,
                        FileShare.ReadWrite,
                        1024 * 1024
                        );
                }
                return this._releasefileStreamValue;
            }
        }

        // close file stream, returning true if it was open
        private bool CloseReleaseFileStream()
        {
            if (this._releasefileStreamValue != null)
            {
                //logger.Info("Closing filestream to {0}", this.releaseFileName);
                this._releaseBinaryWriterValue.Flush();
                this._releasefileStreamValue.Close();
                this._releasefileStreamValue = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Saves the usage to the released directory
        /// not be possible for someone to read from a file while someone is writing to it since the 
        /// scheduler will not release the id acc until the hook is complete.
        /// </summary>
        /// <param name="objectId"></param>
        public void SaveLocal(string objectId)
        {
            using (ObjectDataFormattedDelta obj = (ObjectDataFormattedDelta)mvm.objectCache.CheckOut(objectId))
            {
                string idPayee = obj["id_payee"];
                bool isLiveParent = obj["is_live_parent"].Equals("1");
                MapIdPayeeToFile(idPayee, this.releaseFileName);
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                //logger.Info("Save local usage id_sess={0} with oid={1}", obj["id_sess"], obj.objectId);
                obj.Serialize(bw);
                int serializedLength = (int)ms.Length;
                byte[] serializedBuffer = ms.GetBuffer();
                // write [id_payee][payload.length][payload]
                // types [string][int][byte[]]
                releaseBinaryWriter.Write(idPayee);
                releaseBinaryWriter.Write(serializedLength);
                releaseBinaryWriter.Write(serializedBuffer, 0, serializedLength);
            }
        }
        #endregion



        public static MemoryIndexSync GetParentUsageObjectFields(MvmEngine mvm)
        {
            MemoryIndexSync parentUsageObjectFields = mvm.globalContext.GetNamedClassInst("PARENT_USAGE_OBJECT_FIELDS") as MemoryIndexSync;
            if (parentUsageObjectFields == null)
            {
                throw new Exception("Error looks like PARENT_USAGE_OBJECT_FIELDS is undefined or not type MemoryIndexSync");
            }
            return parentUsageObjectFields;
        }

        private static MemoryIndexSync GetParentUsageObjects(MvmEngine mvm)
        {
            MemoryIndexSync parentUsageObjects = mvm.globalContext.GetNamedClassInst("PARENT_USAGE_OBJECTS") as MemoryIndexSync;
            if (parentUsageObjects == null)
            {
                throw new Exception("Error looks like PARENT_USAGE_OBJECTS is undefined or not type MemoryIndexSync");
            }
            return parentUsageObjects;
        }

        /// <summary>
        /// Given an id_view get the updateable fields for a live parent
        /// </summary>
        /// <param name="mvm"></param>
        /// <param name="idView"></param>
        /// <returns></returns>
        public static List<string> GetParentObjectFields(MvmEngine mvm, string idView)
        {
            List<string> fieldNames = new List<string>();
            MemoryIndexSync parentUsageObjectFields = GetParentUsageObjectFields(mvm);
            StringArray idViewKey = new StringArray(idView);
            List<string[]> rows = null;
            if (parentUsageObjectFields.index.TryGetValue(idViewKey, out rows))
            {
                foreach (var row in rows)
                {
                    string fieldName = row[1];
                    fieldNames.Add(fieldName);
                }
            }
            return fieldNames;
        }


        /// <summary>
        /// Given an id_sess, get the live parent oid
        /// </summary>
        /// <param name="idSess"></param>
        /// <returns></returns>
        public string LookupParentObjectId(string idSess)
        {
            MemoryIndexSync parentUsageObjects = GetParentUsageObjects(mvm);
            StringArray idSessKey = new StringArray(idSess);
            List<string[]> rows = null;
            if (parentUsageObjects.index.TryGetValue(idSessKey, out rows))
            {
                var row = rows[0];
                string parentObjectId = row[1];
                return parentObjectId;
            }
            return null;
        }

        /// <summary>
        /// Create a new live parent object, copies the appropriate updateable fields from the passed object, 
        /// and inserts the live parent into PARENT_USAGE_OBJECTS.
        /// </summary>
        /// <param name="mvm"></param>
        /// <param name="parentObject"></param>
        public static void RegisterLiveParentUsageObject(MvmEngine mvm, IObjectData cursorObject)
        {
            //mvm.Log("REGISTER LIVE PARENT: " + cursorObject);
            string idSess = cursorObject["id_sess"];
            string idView = cursorObject["id_view"];

            // spawn a small vanilla object since we do not need any delta tracking on the slim parent

            // the passed object is the real live parent object id. we need to make our live
            // parent use the passed object id... so we need to spawn a live parent and swap
            // object ids with the passed object.
            ObjectDataStringHash liveParent = mvm.objectCache.CreateAndGetObject("LIVE_PARENT");
            foreach (var fieldName in GetParentObjectFields(mvm, idView))
            {
                liveParent[fieldName] = cursorObject[fieldName];
                //logger.Info("cs Set LIVE_PARENT." + fieldName + "=[" + liveParent[fieldName] + "], for oid=" + liveParent.objectId);
            }
            // now delete the passed object, because we need to steal is object id
            string liveParentOid = cursorObject.objectId;
            int liveParentRefCount = cursorObject.RefCount;
            cursorObject.Delete();

            // now change the spawned objects oid to be the live parent oid
            string spawnedOid = liveParent.objectId;
            liveParent.objectId = liveParentOid;
            liveParent.RefCount = liveParentRefCount;
            liveParent["object_id"] = liveParent.objectId;
            // remove the spawned object id..
            mvm.objectCache.RemoveObjectData(spawnedOid);
            // reinsert the new object id
            mvm.objectCache.InsertObjectData(liveParent);

            //logger.Info("cs PARENT_USAGE_OBJECT insert id_sess=[" + idSess + "] id_view=[" + liveParent["id_view"] + "] oid=" + liveParent.objectId + ",type=" + liveParent.GetType().FullName);
            MemoryIndexSync parentUsageObjects = GetParentUsageObjects(mvm);
            parentUsageObjects.IndexInsert(null, new List<string> { idSess, liveParent.objectId });
        }

        /// <summary>
        /// Clears all object from PARENT_USAGE_OBJECTS
        /// </summary>
        public void ClearParentUsageObjects()
        {
            //logger.Info("Clearing parent_usage_objects:");
            MemoryIndexSync parentUsageObjects = GetParentUsageObjects(mvm);
            parentUsageObjects.IndexClear(null);
        }

        /// <summary>
        /// Create a usage hook
        /// </summary>
        public UsageHookObject(MvmEngine mvm, string hookId, string sortField, int idAccCount, int maxOpenFiles, int maxSortObjects, string tablesDir, string joinDir, string cursor1Dir, string cursor2Dir, string partitioningDir, string streamingDir, string releasedDir, string persistedDir)
        {
            this.mvm = mvm;
            this.bfs = this.mvm.workMgr.globalContext.bfs;
            this.hookId = hookId;
            this.sortField = sortField;
            this.idAccCount = idAccCount;
            this.maxOpenFiles = maxOpenFiles;
            this.maxSortObjects = maxSortObjects;
            this.tablesDir = tablesDir;
            this.joinDir = joinDir;
            this.cursor1Dir = cursor1Dir;
            this.cursor2Dir = cursor2Dir;
            this.partitioningDir = partitioningDir;
            this.streamingDir = streamingDir;
            this.releasedDir = releasedDir;
            this.persistedDir = persistedDir;
            this.hookName = GetHookName(hookId);
            this.myReader = new MergeSortReaderWriter<Text, IObjectData>(this.maxOpenFiles, this.maxSortObjects, this.cursor1Dir, "", MergeableComparer<Text>.Default, Serializabler<Text>.Default, this.mvm.objectDataSerializer);
            this.myWriter = new MergeSortReaderWriter<Text, IObjectData>(this.maxOpenFiles, this.maxSortObjects, this.cursor2Dir, "", MergeableComparer<Text>.Default, Serializabler<Text>.Default, this.mvm.objectDataSerializer);
        }

        /// <summary>
        /// Init the usage hook and return true if there is any usage
        /// </summary>
        /// <returns></returns>
        public bool ReadWriteStart()
        {
            this.hookState = HookState.ReadWrite;
            this.objectCursorCtr = 0;
            return true;
        }

        /// <summary>
        /// Cleanup the readwrite cursor
        /// </summary>
        public void ReadWriteStop()
        {
            this.hookState = HookState.Idle;
            this.objectCursorCtr = 0;
            // if the reader enumerator was instanciated, dispose of it
            if (this._myReaderEnumerator != null)
            {
                this._myReaderEnumerator.Dispose();
                this._myReaderEnumerator = null;
            }
            // flip the reader and the writer
            var temp = this.myReader;
            this.myReader = myWriter;
            this.myWriter = temp;
        }


        private KeyValuePair<Text, IObjectData>? previousRow;

        /// <summary>
        /// Write out current row and read in next row
        /// </summary>
        /// <returns></returns>
        public bool ReadWriteNext()
        {
            // write out the previous row...
            if (previousRow != null)
            {
                // now write out the previous object as in order
                this.myWriter.Add(previousRow.Value, SortOrder.Forward);
                // Mark previous object as null since it is now processed.
                previousRow = null;
            }
            else
            {
            }

            // Move on to the next object
            if (this.myReaderEnumerator.MoveNext())
            {
                this.objectCursorCtr++;
                this.previousRow = this.myReaderEnumerator.Current;

                // if child has a parent, make sure parent oid is set.
                IObjectData obj = this.Current;
                string parentIdSess = obj["id_parent_sess"];
                string idSess = obj["id_sess"];
                if (!parentIdSess.Equals("") && !parentIdSess.Equals(idSess))
                {
                    string parentOid = obj["parent_oid"];
                    if (parentOid.Equals(""))
                    {
                        string parentObjectId = this.LookupParentObjectId(parentIdSess);
                        if (parentObjectId == null)
                        {
                            logger.Warn("Warning, cannot find live parent object id_sess=[" + idSess + "] with id_parent_sess=[" + parentIdSess + "]");
                        }
                        else
                        {
                            obj["parent_oid"] = parentObjectId;
                            //logger.Info("Setting parent_oid for id_sess=[" + idSess + "] with id_parent_sess=[" + parentIdSess + "]");
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Adds an object to the usage hook
        /// </summary>
        /// <param name="targetObj"></param>
        public void AddObjectToHook(IObjectData obj, bool inOrder)
        {
            //logger.Info("[add_object_to_hook] hook_id=[" + this.hookId + "] oid=[" + obj.objectId + "] id_sess=[" + obj["id_sess"] + "] deleted=[" + obj["deleted"] + "] amount=[" + obj["amount"] + "]");
            // if the object is 'deleted' then we insert it into the 'USAGE_TO_DELETE'
            if (obj["deleted"].Equals("1"))
            {
                //logger.Info("[add_object_to_hook] Register deleted obj oid=[" + obj.objectId + "] id_sess=[" + obj["id_sess"] + "]");
                AddDeletedUsage(obj);
                return;
            }
            // Add the object to the hook
            switch (this.hookState)
            {
                case HookState.Idle:
                    {
                        //logger.Info("[add_object_to_hook] hook idle, add to this pass obj oid=[" + obj.objectId + "] id_sess=[" + obj["id_sess"] + "]");
                        this.AddThisPassObject(obj, inOrder);
                        break;
                    }
                case HookState.ReadWrite:
                    {
                        string passedSortKey = obj[this.sortField];
                        bool objectIsFuture = passedSortKey.IsGte(this.Current[this.sortField]) ? true : false;
                        if (objectIsFuture)
                        {
                            //logger.Info("[add_object_to_hook] hook on, future obj, add to this pass oid=[" + obj.objectId + "] sortkey=[" + obj["sortkey"] + "]");
                            this.AddThisPassObject(obj, inOrder);
                        }
                        else
                        {
                            //logger.Info("[add_object_to_hook] hook on, backed dated obj, add to next pass oid=[" + obj.objectId + "] sortkey=[" + obj["sortkey"] + "]");
                            this.AddNextPassObject(obj, inOrder);
                        }
                        break;
                    }
                default: throw new Exception("unexpected usage hook state:" + this.hookState);
            }

            // if the object is live, then we need to register it
            if (obj["is_live_parent"].Equals("1"))
            {
                RegisterLiveParentUsageObject(mvm, obj);
                //logger.Info("Registered live parent oid=[" + obj.objectId + "] id_sess=[" + obj["id_sess"] + "] refs=[" + obj.RefCount + "]");
            }
        }


        public void AddThisPassObject(IObjectData obj, bool inOrder)
        {
            SortOrder sortOrder = inOrder ? SortOrder.Forward : SortOrder.None;
            Text sortKey = new Text(obj[this.sortField]);
            this.myReader.Add(new KeyValuePair<Text, IObjectData>(sortKey, obj), sortOrder);
        }

        public void AddNextPassObject(IObjectData obj, bool inOrder)
        {
            SortOrder sortOrder = inOrder ? SortOrder.Forward : SortOrder.None;
            Text sortKey = new Text(obj[this.sortField]);
            this.myWriter.Add(new KeyValuePair<Text, IObjectData>(sortKey, obj), sortOrder);

        }

        /// <summary>
        /// Writes out the usage to the done dir by id_acc....
        /// Deletes current cursor files...
        /// </summary>
        public void Release()
        {
            // make sure the release usage file handle is closed
            this.CloseReleaseFileStream();

            // delete all the objects in the live objects.
            ClearParentUsageObjects();
        }


        /// <summary>
        /// Hash the key and numMods it by passed numMods.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="numMods"></param>
        /// <returns></returns>
        public static int HashMod(int key, int numMods)
        {
            uint h = HashIt((uint)key);
            int m = (int)(h % numMods);
            return m;
        }

        /// <summary>
        /// Robert Jenkins' 32 bit integer hash function
        /// http://www.concentric.net/~ttwang/tech/inthash.htm
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static uint HashIt(uint a)
        {
            a = (a + 0x7ed55d16) + (a << 12);
            a = (a ^ 0xc761c23c) ^ (a >> 19);
            a = (a + 0x165667b1) + (a << 5);
            a = (a + 0xd3a2646c) ^ (a << 9);
            a = (a + 0xfd7046c5) + (a << 3);
            a = (a ^ 0xb55a4f09) ^ (a >> 16);
            return a;
        }
    }
}
