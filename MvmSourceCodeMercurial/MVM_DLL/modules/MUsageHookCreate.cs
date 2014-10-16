using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using NGenerics.DataStructures.Trees;
using NLog;
using System.Linq;
namespace MVM
{
    /*
    <usage_hook_create>
      <hook_id>TEMP.hook_id</hook_id>
      <sort_field>GLOBAL.usage_hook_sort_field</sort_field>
      <usage_tables_dir>GLOBAL.usage_tables_dir</usage_tables_dir>
      <usage_join_dir>GLOBAL.usage_join_dir</usage_join_dir>
      <usage_cursor_1_dir>GLOBAL.usage_cursor_1_dir</usage_cursor_1_dir>
      <usage_cursor_2_dir>GLOBAL.usage_cursor_2_dir</usage_cursor_2_dir>
      <usage_cursor_done_dir>GLOBAL.usage_cursor_done_dir</usage_cursor_done_dir>
    </usage_hook_create>
      */
    public class MUsageHookCreate : IModuleSetup, IModuleRun
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private string usageHookIdSyntax;
        private IReadString usageHookIdParsed;

        private string sortFieldSyntax;
        private IReadString sortFieldParsed;

        private string idAccCountSyntax;
        private IReadString idAccCountParsed;

        private string usageTablesDirSyntax;
        private IReadString usageTablesDirParsed;

        private string usageJoinDirSyntax;
        private IReadString usageJoinDirParsed;

        private string usageCursor1DirSyntax;
        private IReadString usageCursor1DirParsed;

        private string usageCursor2DirSyntax;
        private IReadString usageCursor2DirParsed;

        private string usagePartitioningDirSyntax;
        private IReadString usagePartitioningDirParsed;

        private string usageStreamingDirSyntax;
        private IReadString usageStreamingDirParsed;

        private string usageReleasedDirSyntax;
        private IReadString usageReleasedDirParsed;

        private string usagePersistedDirSyntax;
        private IReadString usagePersistedDirParsed;

        private string maxOpenFilesSyntax;
        private IReadString maxOpenFilesParsed;

        private string maxSortObjectsSyntax;
        private IReadString maxSortObjectsParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MUsageHookCreate m = new MUsageHookCreate();
            m.usageHookIdSyntax = me.SelectNodeInnerText("./hook_id");
            m.usageHookIdParsed = mc.ParseSyntax(m.usageHookIdSyntax);
            m.sortFieldSyntax = me.SelectNodeInnerText("./sort_field");
            m.sortFieldParsed = mc.ParseSyntax(m.sortFieldSyntax);
            m.idAccCountSyntax = me.SelectNodeInnerText("./id_acc_count");
            m.idAccCountParsed = mc.ParseSyntax(m.idAccCountSyntax);
            m.maxOpenFilesSyntax = me.SelectNodeInnerText("./max_open_files");
            m.maxOpenFilesParsed = mc.ParseSyntax(m.maxOpenFilesSyntax);
            m.maxSortObjectsSyntax = me.SelectNodeInnerText("./max_sort_objects");
            m.maxSortObjectsParsed = mc.ParseSyntax(m.maxSortObjectsSyntax);
            m.usageTablesDirSyntax = me.SelectNodeInnerText("./usage_tables_dir");
            m.usageTablesDirParsed = mc.ParseSyntax(m.usageTablesDirSyntax);
            m.usageJoinDirSyntax = me.SelectNodeInnerText("./usage_join_dir");
            m.usageJoinDirParsed = mc.ParseSyntax(m.usageJoinDirSyntax);
            m.usageCursor1DirSyntax = me.SelectNodeInnerText("./usage_cursor_1_dir");
            m.usageCursor1DirParsed = mc.ParseSyntax(m.usageCursor1DirSyntax);
            m.usageCursor2DirSyntax = me.SelectNodeInnerText("./usage_cursor_2_dir");
            m.usageCursor2DirParsed = mc.ParseSyntax(m.usageCursor2DirSyntax);
            m.usagePartitioningDirSyntax = me.SelectNodeInnerText("./usage_partitioning_dir");
            m.usagePartitioningDirParsed = mc.ParseSyntax(m.usagePartitioningDirSyntax);
            m.usageStreamingDirSyntax = me.SelectNodeInnerText("./usage_streaming_dir");
            m.usageStreamingDirParsed = mc.ParseSyntax(m.usageStreamingDirSyntax);
            m.usageReleasedDirSyntax = me.SelectNodeInnerText("./usage_released_dir");
            m.usageReleasedDirParsed = mc.ParseSyntax(m.usageReleasedDirSyntax);
            m.usagePersistedDirSyntax = me.SelectNodeInnerText("./usage_persisted_dir");
            m.usagePersistedDirParsed = mc.ParseSyntax(m.usagePersistedDirSyntax);
            run.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            string hookId = this.usageHookIdParsed.Read(mc);
            string sortField = this.sortFieldParsed.Read(mc);
            int idAccCount = this.idAccCountParsed.Read(mc).ToInt();
            int maxOpenFiles = this.maxOpenFilesParsed.Read(mc).ToInt();
            int maxSortObjects = this.maxSortObjectsParsed.Read(mc).ToInt();
            string tableDir = this.usageTablesDirParsed.Read(mc);
            string joinDir = this.usageJoinDirParsed.Read(mc);
            string cursor1Dir = this.usageCursor1DirParsed.Read(mc);
            string cursor2Dir = this.usageCursor2DirParsed.Read(mc);
            string partitioningDir = this.usagePartitioningDirParsed.Read(mc);
            string streamingDir = this.usageStreamingDirParsed.Read(mc);
            string releasedDir = this.usageReleasedDirParsed.Read(mc);
            string persistedDir = this.usagePersistedDirParsed.Read(mc);
            UsageHookObject usageHookObject = new UsageHookObject(mc.mvm, hookId, sortField, idAccCount, maxOpenFiles, maxSortObjects, tableDir, joinDir, cursor1Dir, cursor2Dir, partitioningDir, streamingDir, releasedDir, persistedDir);
            mc.globalContext.SetNamedClassInst(usageHookObject.hookName, usageHookObject);
        }
    }



    /// <summary>
    /// This class provides a merge sorting enumerator across all the passed files. It holds read locks
    /// on all the files so care should be taken to reduce the file count prior to calling this. Files
    /// must already be sorted by the passed sort field. It releases readlocks after we hit eof on the
    /// file.
    /// </summary>
    //public class HookEnumerator : IEnumerator<IObjectData>, IEnumerable<IObjectData>
    //{
    //    public static Logger logger = LogManager.GetCurrentClassLogger();
    //    public readonly FutureTree futureTree;
    //    public bool FutureTreeInserted = false;

    //    /// <summary>
    //    /// Removes the future tree from the btree.
    //    /// </summary>
    //    public void RemoveFutureTree()
    //    {
    //        if (!FutureTreeInserted) throw new Exception("Cannot RemoveFutureTree if not inserted");
    //        this.FutureTreeInserted = false;
    //        string futureTreeSortKey = futureTree.ObjectData[this.sortField];
    //        var linkedList = this.btree[futureTreeSortKey];
    //        if (!linkedList.Remove(futureTree))
    //            throw new Exception("Error cannot find FutureTree to remove it");
    //    }

    //    /// <summary>
    //    /// Inserts the future tree into the btree.
    //    /// </summary>
    //    public void InsertFutureTree()
    //    {
    //        if (FutureTreeInserted) throw new Exception("Cannot InsertFutureTree if it is already inserted");
    //        this.FutureTreeInserted = true;
    //        this.btree.Enqueue(futureTree.ObjectData[this.sortField], this.futureTree);
    //    }

    //    public string[] files;
    //    public IObjectData targetObj;
    //    public ObjectQueueBufferedFile[] queues;
    //    public MvmClusterBase mvmCluster;
    //    public MvmEngine mvm;
    //    public RedBlackTree<string, LinkedList<IObjectTreeNode>> btree = new RedBlackTree<string, LinkedList<IObjectTreeNode>>();
    //    public BufferedFileSystem bfs;
    //    public List<LockingLruCache<string, IBufferedFile>.LockingLruCacheItem> lruReaderLocks = new List<LockingLruCache<string, IBufferedFile>.LockingLruCacheItem>();
    //    public string sortField;
    //    public string lastSortKey;



    //    public bool eof { get; private set; }

    //    //public bool ownsTargetObj;
    //    public readonly UsageHookObject usageHook;
    //    public HookEnumerator(UsageHookObject usageHook, string[] files, string sortField, bool trackDelta)
    //    {
    //        this.eof = false;
    //        this.usageHook = usageHook;
    //        this.mvm = usageHook.mvm;
    //        this.mvmCluster = this.mvm.mvmCluster;
    //        this.bfs = this.mvm.workMgr.globalContext.bfs;
    //        this.files = files;
    //        this.sortField = sortField;

    //        // preload the btree with first object from each file
    //        // holds locks on all the mergable files
    //        foreach (string file in this.files)
    //        {
    //            LockingLruCache<string, IBufferedFile>.LockingLruCacheItem lruLock = bfs.GetObjectQueue(file, true);
    //            var objectQueue = lruLock.value as ObjectQueueBufferedFile;
    //            objectQueue.Blocking = false;
    //            IObjectTreeNode treeNode = new ObjectTreeNode(objectQueue, this);
    //            if (treeNode.ObjectData != null)
    //            {
    //                lruReaderLocks.Add(lruLock);
    //                this.btree.Enqueue(treeNode.ObjectData[this.sortField], treeNode);
    //            }
    //            else
    //            {
    //                lruLock.Dispose();
    //            }
    //        }
    //        this.futureTree = new FutureTree(this);
    //    }

    //    /// <summary>
    //    /// Pulls the next node from the tree, backfilling if possible, and returns true if node is returned.
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="node"></param>
    //    /// <returns></returns>
    //    public bool TryDequeue(out string key, out IObjectData obj)
    //    {
    //        IObjectTreeNode node;
    //        obj = null;
    //        key = null;
    //        if (this.btree.TryDequeue(out key, out node))
    //        {
    //            // set the output object.
    //            obj = node.ObjectData;
    //            // try to backfill
    //            IObjectTreeNode backfillNode = node.BackFill();
    //            if (backfillNode != null)
    //            {
    //                this.btree.Enqueue(backfillNode.ObjectData[this.sortField], backfillNode);
    //            }

    //            // if the object is a parent object, update the current object with the values on the live parent.
    //            string isLiveParent = obj["is_live_parent"];
    //            string parentIdSess = obj["id_parent_sess"];
    //            string idSess = obj["id_sess"];

    //            // if this record is a child with a child and has a parent, make sure parent oid is set.
    //            if (!parentIdSess.Equals("") && !parentIdSess.Equals(idSess))
    //            {
    //                string parentOid = obj["parent_oid"];
    //                if (parentOid.Equals(""))
    //                {
    //                    string parentObjectId = this.LookupParentObjectId(parentIdSess);
    //                    if (parentObjectId == null)
    //                    {
    //                        // TBD WHAT TO DO ABOUT THIS?
    //                        //throw new Exception("Error, cannot file live parent object id_sess=[" + idSess + "] with id_parent_sess=[" + parentIdSess + "]");
    //                        logger.Warn("Warning, cannot find live parent object id_sess=[" + idSess + "] with id_parent_sess=[" + parentIdSess + "]");
    //                    }
    //                    else
    //                    {
    //                        obj["parent_oid"] = parentObjectId;
    //                        //logger.Info("Setting parent_oid for id_sess=[" + idSess + "] with id_parent_sess=[" + parentIdSess + "]");
    //                    }
    //                }
    //            }

    //            return true;
    //        }
    //        return false;
    //    }

    //    public string LookupParentObjectId(string idSess)
    //    {
    //        return this.usageHook.LookupParentObjectId(idSess);
    //    }

    //    public bool MoveNext()
                //{
    //        //this.mvm.Log("OQME.MoveNext");
    //        if (this.eof)
    //        {
    //            //logger.Info("DBG:already at eof so return {0}", !this.eof);
    //            return !this.eof;
                //}
    //        string key;
    //        if (TryDequeue(out key, out this.targetObj))
    //        {
    //            logger.Info("DBG:MoveNext: oid={1} : {0}", this.targetObj.objectId, this.targetObj.fields.Select(f => f.Key + "=" + f.Value).JoinStrings(","));
    //            //logger.Info("DBG:MoveNext: oid={0} : amount={1} id_sess={2}", objectData.objectId, objectData["amount"],objectData["id_sess"]);
    //            this.lastSortKey = this.targetObj[this.sortField];
    //            this.eof = false;
    //            return !this.eof;
    //        }
                //else
                //{
    //            //logger.Info("at eof, clear cursor");
    //            this.ClearCursor();
    //            this.eof = true;
    //            //logger.Info("DBG:just set eof to true and return {0}", !this.eof);
    //            return !this.eof;
                //}
    //        throw new Exception("never get here");
    //    }

    //    public IObjectData Current
    //    {
    //        get { return this.targetObj; }
    //    }

    //    private void ClearCursor()
    //    {

    //        // release all our reader locks
    //        foreach (var lruLock in this.lruReaderLocks)
    //        {
    //            var objectQueue = lruLock.value as ObjectQueueBufferedFile;
    //            objectQueue.FlushToFile(false);
    //            lruLock.Dispose();
    //        }
    //        lruReaderLocks.Clear();
    //    }

    //    public void Dispose()
    //    {
    //    }
    //    object System.Collections.IEnumerator.Current
    //    {
    //        get { return this.targetObj; }
    //    }
    //    public void Reset()
    //    {
    //        throw new NotImplementedException();
    //    }
    //    public IEnumerator<IObjectData> GetEnumerator()
    //    {
    //        return this;
    //    }
    //    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    //    {
    //        return this;
    //    }



    //    public void AddObject(ObjectDataDelta obj)
    //    {
    //        // need current sortValue
    //        string sortKey = obj[this.sortField];
    //        // future objects get added to the futureTree so they are picked up on this round
    //        if (this.lastSortKey == null || sortKey.IsGte(this.lastSortKey))
    //        {
    //            //logger.Info("add object to future tree:" + obj["dt_session"] + "," + obj["amount"]);
    //            this.futureTree.AddObject(obj);
    //        }
    //        // past objects are written to a file which we'll later sort.
    //        else
    //        {
    //            //logger.Info("add object to for next pass:" + obj["dt_session"] + "," + obj["amount"]);
    //            this.usageHook.AddNextPassObject(obj);
    //        }
    //    }
    //}


    //public class FutureTree : IObjectTreeNode
    //{
    //    public HookEnumerator looper;
    //    private IObjectData nextObjectData;
    //    public RedBlackTree<string, LinkedList<IObjectData>> futureBtree = new RedBlackTree<string, LinkedList<IObjectData>>();
    //    public FutureTree(HookEnumerator looper)
    //    {
    //        this.looper = looper;
    //    }
    //    public void AddObject(IObjectData obj)
    //    {
    //        string passedSortKey = obj[this.looper.sortField];

    //        // if no next object, this is the next object
    //        if (this.nextObjectData == null)
    //        {
    //            this.nextObjectData = obj;
    //            this.looper.InsertFutureTree();
    //            return;
    //        }
    //        // if this object can go after the next object, just add it to the futureTree
    //        if (passedSortKey.IsGte(this.nextObjectData[this.looper.sortField]))
    //        {
    //            this.futureBtree.Enqueue(obj[this.looper.sortField], obj);
    //            return;
    //        }

    //        // otherwise, this object needs to replace the next object, which involves
    //        // removing the FutureTree and re-adding it.
    //        this.looper.RemoveFutureTree();
    //        this.futureBtree.Enqueue(obj[this.looper.sortField], this.nextObjectData);
    //        this.nextObjectData = obj;
    //        this.looper.InsertFutureTree();
    //        // TBD:
    //        // if the new object takes us over the size limit, serialize to disk
    //        // add the file to looper.btree (if too many files open, it will take care of merging)
    //        // --the tricky bit is that the file has already had an object sucked off it so need to be sure
    //        // --to be sure we pickup in the correct spot. Can do this by just picking a node and keep calling
    //        // --back fill on it.
    //    }

    //    public IObjectTreeNode BackFill()
    //    {
    //        string key;
    //        if (this.futureBtree.TryDequeue(out key, out this.nextObjectData))
    //        {
    //            return this;
    //        }
    //        // if you backfill and there are not objects in the future tree then the 
    //        // future tree is not inserted.
    //        this.looper.FutureTreeInserted = false;
    //        return null;
    //    }


    //    public IObjectData ObjectData
    //    {
    //        get
    //        {
    //            return this.nextObjectData;
    //        }
    //    }
    //}

    ///// <summary>
    ///// Use this calls to bundle the objectData and the objectQueue we need to pull 
    ///// the next item from. If objectQueue is null, the object is a future object
    ///// so consume does not need to pull.
    ///// </summary>
    //public class ObjectTreeNode : IObjectTreeNode
    //{
    //    public IObjectData ObjectData { get { return this.objectData; } }
    //    public readonly IObjectData objectData;
    //    public readonly ObjectQueueBufferedFile objectQueue;
    //    public readonly HookEnumerator looper;
    //    public ObjectTreeNode(ObjectQueueBufferedFile objectQueue, HookEnumerator looper)
    //    {
    //        this.looper = looper;
    //        this.objectQueue = objectQueue;
    //        if (objectQueue.ReadObject(looper.mvm.mvmCluster, out this.objectData))
    //        {
    //            return;
    //        }
    //        this.objectData = null;
    //    }
    //    public IObjectTreeNode BackFill()
    //    {
    //        ObjectTreeNode nextNode = new ObjectTreeNode(this.objectQueue, this.looper);
    //        if (nextNode.ObjectData != null) return nextNode;
    //        return null;
    //    }
    //}

    /// <summary>
    /// 
    /// </summary>
    public interface IObjectTreeNode
    {
        /// <summary>
        /// The object for this node
        /// </summary>
        IObjectData ObjectData { get; }
        /// <summary>
        /// Tries to get the next object, returns null if none.
        /// </summary>
        /// <returns></returns>
        IObjectTreeNode BackFill();
    }

    public class PayloadFileLooper : IEnumerator<PayloadEntry>, IEnumerable<PayloadEntry>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        BinaryReader breader;
        public PayloadFileLooper(BinaryReader breader)
        {
            this.breader = breader;
        }
        PayloadEntry current = new PayloadEntry();
        #region IEnumerator<PayloadEntry> Members

        public PayloadEntry Current
        {
            get { return current; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {

        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get { return current; }
        }

        public bool MoveNext()
        {
            try
            {
                current.key = breader.ReadString();
            }
            catch (System.IO.EndOfStreamException e)
            {
                logger.Error("MoveNext() breader.ReadString() threw exception: " + e.Message);
                return false;
            }
            current.length = breader.ReadInt32();
            return true;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<PayloadEntry> Members

        public IEnumerator<PayloadEntry> GetEnumerator()
        {
            return this;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this;
        }

        #endregion
    }

    public class PayloadEntry
    {
        public string key;
        public int length;
    }

}
