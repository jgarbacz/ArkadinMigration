using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using NGenerics.DataStructures.Trees;
using System.IO;
namespace MVM
{
   

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    /// <summary>
    /// 
    /// </summary>
    public interface IObjectMergeNode
    {
        /// <summary>
        /// The object for this node
        /// </summary>
        IObjectData ObjectData { get; }
        /// <summary>
        /// Tries to get the next object, returns null if none.
        /// </summary>
        /// <returns></returns>
        IObjectMergeNode BackFill();
        /// <summary>
        /// Number of objects in the node
        /// </summary>
        int Count { get; }
    }

    /// <summary>
    /// This class maintains a btree of objects and expects to be a member of 
    /// an SortedObjectMerger. It needs ObjectData to be set to the next object
    /// to be consumed. Backfill() consumes an object data.
    /// </summary>
    public class SortedObjectTree : IObjectMergeNode
    {
        public RedBlackTree<string, LinkedList<IObjectData>> btree = new RedBlackTree<string, LinkedList<IObjectData>>();
        private string currentKey;
        private IObjectData currentObj;
        private string sortKeyField=null;
        private ObjectMergeSortReadWrite merger;
        public SortedObjectTree(ObjectMergeSortReadWrite merger)
        {
            this.merger = merger;
        }
        public int Count { get; set; }
        public void AddObject(IObjectData newObj)
        {
            this.Count++;
            if (currentObj == null)
            {
                this.currentObj = newObj;
                this.merger.InsertMergeNode(this);
                return;

            }
            // obj if after current object, just add it to the btree
            // otherwise, add the currentObj and make this the current.
            string newKey = newObj[this.sortKeyField];
            if (newKey.IsGte(this.currentKey))
            {
                this.btree.Enqueue(newKey, newObj);
            }
            else
            {
                this.btree.Enqueue(this.currentKey, this.currentObj);
                this.currentKey = newKey;
                this.currentObj = newObj;
                this.merger.ReinsertMergeNode(this);
            }
        }

        public IObjectData ObjectData
        {
            get { return this.currentObj; }
        }

        public IObjectMergeNode BackFill()
        {
            this.Count--;
            if (this.btree.TryDequeue(out currentKey, out currentObj))
            {
                return this;
            }
            return null;
        }
    }

    /// <summary>
    /// This class maintains a queue of IObjects that are expected to be in the proper
    /// sort order.
    /// </summary>
    public class SortedObjectQueue : IObjectMergeNode
    {
        public Queue<IObjectData> queue = new Queue<IObjectData>();
        private IObjectData currentObj;
        public int Count { get; set; }
        public SortedObjectQueue()
        {
        }

        public void AddObject(IObjectData newObj)
        {
            this.Count++;
            if (currentObj == null)
            {
                this.currentObj = newObj;
                return;
            }
            this.queue.Enqueue(newObj);
        }

        public IObjectData ObjectData
        {
            get { return this.currentObj; }
        }

        public IObjectMergeNode BackFill()
        {
            if (this.queue.Count == 0)
            {
                return null;
            }
            this.Count--;
            this.currentObj = this.queue.Dequeue();
            return this;
        }
    }


    /// <summary>
    /// Writes objects to a file. Objects are expected to be in sorted order.
    /// </summary>
    public class SortedObjectFile : IObjectMergeNode
    {
        private BinaryReader breader;
        private IObjectData currentObj;
        public int Count { get; set; }
        public readonly MvmEngine mvm;
        public readonly string fileName;
        public SortedObjectFile(MvmEngine mvm, string fileName, IEnumerable<IObjectData> objects)
        {
            this.mvm = mvm;
            this.fileName = fileName;
            // open the file, write all the objects to it, and close it.
            using (FileStream fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None))
            {
                using (BinaryWriter bwriter = new BinaryWriter(fs))
                {
                    foreach (IObjectData obj in objects)
                    {
                        if (this.Count++ == 0)
                        {
                            this.currentObj = obj;
                        }
                        else
                        {
                            obj.Serialize(bwriter);
                        }
                    }
                }
            }
            // open a reader into the file
            this.breader = new BinaryReader(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None));
        }

        public IObjectData ObjectData
        {
            get { return this.currentObj; }
        }

        public IObjectMergeNode BackFill()
        {
            if (this.Count == 0) return null;
            if (ObjectDataBase.Deserialize(this.breader, this.mvm.mvmCluster, out this.currentObj))
            {
                return this;
            }
            else
            {
                throw new Exception("unexpected EOF on " + this.fileName);
            }
        }
    }

    public class ObjectMergeSortReader : IEnumerator<IObjectData>, IEnumerable<IObjectData>
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The current object
        /// </summary>
        public IObjectData currentObjectData;

        // Keep links up the chain
        public MvmClusterBase mvmCluster;
        public MvmEngine mvm;

        // Keeps track of the files we are merging
        public RedBlackTree<string, LinkedList<IObjectMergeNode>> btree = new RedBlackTree<string, LinkedList<IObjectMergeNode>>();

        // Name of field we are sorting by
        public string sortField;

        public bool eof { get; protected set; }

        public bool HasStarted
        {
            get;
            private set;
        }


        public ObjectMergeSortReader(MvmEngine mvm, string sortField)
        {
            this.mvm = mvm;
            this.mvmCluster = mvm.mvmCluster;
            this.sortField = sortField;
            this.HasStarted = false;
            this.eof = false;
        }


        #region Called by IObjectMergeNodes

        /// <summary>
        /// Inserts an ObjectMergeNode
        /// </summary>
        public void InsertMergeNode(IObjectMergeNode mergeNode)
        {
            this.btree.Enqueue(mergeNode.ObjectData[this.sortField], mergeNode);
        }

        /// <summary>
        /// Removes an ObjectMergeNode
        /// </summary>
        public bool RemoveMergeNode(IObjectMergeNode mergeNode)
        {
            string treeSortKey = mergeNode.ObjectData[this.sortField];
            var linkedList = this.btree[treeSortKey];
            if (linkedList == null)
                return false;
            return linkedList.Remove(mergeNode);
        }

        /// <summary>
        /// Remove and insert the ObjectMergeNode.
        /// </summary>
        /// <param name="mergeNode"></param>
        /// <returns></returns>
        public void ReinsertMergeNode(IObjectMergeNode mergeNode)
        {
            if (!this.RemoveMergeNode(mergeNode))
                throw new Exception("Unexpected that remove tree returned false");
            this.ReinsertMergeNode(mergeNode);
        }

        #endregion


        /// <summary>
        /// Pulls the next node from the tree, backfilling if possible, and returns true if node is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool TryDequeue(out string key, out IObjectData obj)
        {
            IObjectMergeNode node;
            obj = null;
            key = null;
            if (this.btree.TryDequeue(out key, out node))
            {
                obj = node.ObjectData;
                IObjectMergeNode backfillNode = node.BackFill();
                if (backfillNode != null)
                {
                    this.btree.Enqueue(backfillNode.ObjectData[this.sortField], backfillNode);
                }
                return true;
            }
            return false;
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
                //this.ClearCursor();
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

    /// <summary>
    /// This class provides sorted access to objects. Objects may be
    /// added even while one is looping through this. If too many objects
    /// are added it automatically spills to disk.
    /// 
    /// *maybe* rename to sorted object queue.
    /// </summary>
    public class ObjectMergeSortReadWrite : ObjectMergeSortReader
    {
        public SortedObjectTree sortedTree;
        public SortedObjectQueue sortedQueue;
        public List<SortedObjectFile> sortedFiles = new List<SortedObjectFile>();
        public string tempDir;
        private int tempCtr = 0;
        public int maxOpenFiles;
        public int maxLiveObjects;
        public int LiveObjectCount
        {
            get
            {
                return this.sortedTree.Count + this.sortedQueue.Count;
            }
        }

        public ObjectMergeSortReadWrite(MvmEngine mvm, string sortField, int maxOpenFiles, int maxLiveObjects, string tempDir)
            : base(mvm, sortField)
        {
            this.maxLiveObjects = maxLiveObjects;
            this.maxOpenFiles = maxOpenFiles;
            this.tempDir = tempDir;
            this.sortedQueue = new SortedObjectQueue();
            this.sortedTree = new SortedObjectTree(this);
        }

        /// <summary>
        /// Writes an object to the system. If inOrder then calls to this
        /// must be pass sorted objects.
        /// </summary>
        /// <param name="newObj"></param>
        /// <param name="inOrder"></param>
        public void AddObject(IObjectData newObj, bool inOrder)
        {
            // if objects are being added in order use the queue
            // otherwise use the btree which sorts them in memory for us.
            if (inOrder)
                this.sortedQueue.AddObject(newObj);
            else
                this.sortedTree.AddObject(newObj);

            // if we exceeded the max number of objects we
            // can keep in memory, then we need to serialize them all 
            // to a sorted file. If this file would take us over the max files,
            // then we also need to merge the file with fewest number of objects
            // remaining.
            if (this.LiveObjectCount > this.maxLiveObjects)
            {
                ObjectMergeSortReader mergeReader = new ObjectMergeSortReader(this.mvm, this.sortField);
                // remove the live queue and tree
                this.RemoveMergeNode(this.sortedQueue);
                this.RemoveMergeNode(this.sortedTree);
                mergeReader.InsertMergeNode(this.sortedQueue);
                mergeReader.InsertMergeNode(this.sortedTree);
                if (this.sortedFiles.Count >= this.maxOpenFiles)
                {
                    int minIndex = -1;
                    int minCount = int.MaxValue;
                    for (int i = 0; i < this.sortedFiles.Count; i++)
                    {
                        SortedObjectFile f = this.sortedFiles[i];
                        if (f.Count < minCount)
                        {
                            minCount = f.Count;
                            minIndex = i;
                        }
                    }
                    SortedObjectFile minF = this.sortedFiles[minIndex];
                    this.sortedFiles.RemoveAt(minIndex);
                    this.RemoveMergeNode(minF);
                    mergeReader.InsertMergeNode(this.sortedQueue);
                }
                string sortedFileName = Path.Combine(this.tempDir, this.tempCtr++.ToString());
                SortedObjectFile mergeFile = new SortedObjectFile(this.mvm, sortedFileName, mergeReader);
            }
        }
    }

}
