using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using NGenerics.DataStructures.Trees;

namespace MVM
{

    /// <summary>
    /// Use this calls to bundle the objectData and the objectQueue we need to pull 
    /// the next item from. If objectQueue is null, the object is a future object
    /// so consume does not need to pull.
    /// </summary>
    public class ObjectTreeNode2 : IObjectTreeNode
    {
        public IObjectData ObjectData { get { return this.objectData; } }
        
        public readonly IObjectData objectData;
        public readonly ObjectQueueBufferedFile objectQueue;
        public readonly MvmEngine mvm;
        public ObjectTreeNode2(MvmEngine mvm,ObjectQueueBufferedFile objectQueue)
        {
            this.mvm = mvm;
            this.objectQueue = objectQueue;
            if (objectQueue.ReadObject(this.mvm.mvmCluster, out this.objectData))
            {
                return;
            }
            this.objectData = null;
        }
        public IObjectTreeNode BackFill()
        {
            ObjectTreeNode2 nextNode = new ObjectTreeNode2(this.mvm,this.objectQueue);
            if (nextNode.ObjectData != null) return nextNode;
            return null;
        }
    }


    /// <summary>
    /// This class allows multiple sorted object files to be merged together at once.
    /// </summary>
    public class ObjectQueueMerger : IEnumerator<IObjectData>, IEnumerable<IObjectData>
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        
        // Names of sorted files to merge
        public List<string> files=new List<string>();

        // The object file instances to merge
        public List<ObjectQueueBufferedFile> queues=new List<ObjectQueueBufferedFile>();

        /// <summary>
        /// The current object
        /// </summary>
        public IObjectData currentObjectData;

        // Keep links up the chain
        public MvmClusterBase mvmCluster;
        public MvmEngine mvm;
        public BufferedFileSystem bfs;

        // Keeps track of the files we are merging
        public RedBlackTree<string, LinkedList<IObjectTreeNode>> btree = new RedBlackTree<string, LinkedList<IObjectTreeNode>>();
        
        public List<LockingLruCache<string, IBufferedFile>.LockingLruCacheItem> lruReaderLocks = new List<LockingLruCache<string, IBufferedFile>.LockingLruCacheItem>();
        
        // Name of field we are sorting by
        public string sortField;
        
        public bool eof { get; protected set; }

        public bool HasStarted
        {
            get;
            private set;
        }

        public ObjectQueueMerger(MvmEngine mvm, string sortField)
        {
            this.mvm = mvm;
            this.sortField = sortField;
            this.HasStarted = false;
            this.eof = false;
            this.bfs = this.mvm.workMgr.globalContext.bfs;
        }

        public void AddSortedObjectFile(string file)
        {
            LockingLruCache<string, IBufferedFile>.LockingLruCacheItem lruLock = bfs.GetObjectQueue(file, true);
            var objectQueue = lruLock.value as ObjectQueueBufferedFile;
            objectQueue.Blocking = false;
            IObjectTreeNode treeNode = new ObjectTreeNode2(this.mvm, objectQueue);
            if (treeNode.ObjectData != null)
            {
                lruReaderLocks.Add(lruLock);
                this.btree.Enqueue(treeNode.ObjectData[this.sortField], treeNode);
            }
            else
            {
                lruLock.Dispose();
            }
        }



        /// <summary>
        /// Inserts btree
        /// </summary>
        public void InsertTree(IObjectTreeNode treeNode)
        {
            this.btree.Enqueue(treeNode.ObjectData[this.sortField], treeNode);
        }

        /// <summary>
        /// Removes a tree from the btree.
        /// </summary>
        public bool RemoveTree(IObjectTreeNode treeNode)
        {
            string treeSortKey = treeNode.ObjectData[this.sortField];
            var linkedList = this.btree[treeSortKey];
            if (linkedList == null)
                return false;
           return linkedList.Remove(treeNode);
        }


        /// <summary>
        /// Pulls the next node from the tree, backfilling if possible, and returns true if node is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool TryDequeue(out string key, out IObjectData obj)
        {
            IObjectTreeNode node;
            obj = null;
            key = null;
            if (this.btree.TryDequeue(out key, out node))
            {
                obj = node.ObjectData;
                IObjectTreeNode backfillNode = node.BackFill();
                if (backfillNode != null)
                {
                    this.btree.Enqueue(backfillNode.ObjectData[this.sortField], backfillNode);
                }
                return true;
            }
            return false;
        }

        private void ClearCursor()
        {
            foreach (var lruLock in this.lruReaderLocks)
            {
                var objectQueue = lruLock.value as ObjectQueueBufferedFile;
                objectQueue.FlushToFile(false);
                lruLock.Dispose();
            }
            lruReaderLocks.Clear();
        }

        public bool MoveNext()
        {
            this.HasStarted = true;

            //this.mvm.Log("OQME.MoveNext");
            if (this.eof)
            {
                //logger.Info("DBG:already at eof so return {0}", !this.eof);
                return !this.eof;
            }
            string key;
            if (TryDequeue(out key, out this.currentObjectData))
            {
                logger.Info("DBG:MoveNext: oid={1} : {0}", this.currentObjectData.objectId, this.currentObjectData.FieldKeyValuesPairs.Select(f => f.Key + "=" + f.Value).JoinStrings(","));
                this.eof = false;
                return !this.eof;
            }
            else
            {
                //logger.Info("at eof, clear cursor");
                this.ClearCursor();
                this.eof = true;
                //logger.Info("DBG:just set eof to true and return {0}", !this.eof);
                return !this.eof;
            }
            throw new Exception("never get here");
        }

        public IObjectData Current
        {
            get { return this.currentObjectData; }
        }
        public void Dispose()
        {
        }
        object System.Collections.IEnumerator.Current
        {
            get { return this.currentObjectData; }
        }
        public void Reset()
        {
            throw new NotImplementedException();
        }
        public IEnumerator<IObjectData> GetEnumerator()
        {
            return this;
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this;
        }
    }




}
