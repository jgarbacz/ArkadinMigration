using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using NLog;

namespace MVM
{
    /// <summary>
    /// Treat a file as a queue of objects.
    /// </summary>
    public class ObjectQueueBufferedFile : BinaryQueueBufferedFile
    {
        new public static Logger logger = LogManager.GetCurrentClassLogger();
        public MvmEngine mvm { get { return this.bufferedFileSystem.mvm; } }

        /// <summary>
        /// Instanciates the file
        /// </summary>
        /// <param name="bufferedFileSystem"></param>
        /// <param name="fileName"></param>
        public ObjectQueueBufferedFile(BufferedFileSystem bfs, string fileName,bool isPersistent,object state):base(bfs,fileName,isPersistent, state)
        {
            //logger.Debug("Construct " + fileName + ", persist=" + isPersistent);
        }
       
        /// <summary>
        /// This structure tracks the formats we've persisted to this file.
        /// </summary>
        private Dictionary<int, bool> existingFormatIds = new Dictionary<int, bool>();

        /// <summary>
        /// Persists the object to the file
        /// </summary>
        /// 
        /// <param name="targetObj"></param>
        public void WriteObject(IObjectData obj)
        {
            obj.Serialize(this.BinaryWriter);
            this.Commit();
        }

        /// <summary>
        /// Reads data into passed object.
        /// </summary>
        public bool ReadObject(ModuleContext mc, out IObjectData obj)
        {
            return ReadObject(mc.mvmCluster, out obj);
        }

        /// <summary>
        /// Reads the next object from the object file.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="targetObj"></param>
        public bool ReadObject(MvmClusterBase mvmCluster, out IObjectData obj)
        {
            return ObjectDataBase.Deserialize(this.BinaryReader, mvmCluster, out obj);
        }

        public bool ReadObjectIntoObjectCache(MvmClusterBase mvmCluster, out IObjectData obj)
        {
            return this.mvm.objectCache.DeserializeObject(this.BinaryReader, out obj);
        }


        public SplitStopwatch SortObjectFileReadTimer = new SplitStopwatch();
        public static readonly bool SortObjectFileReadTimerOn = true;
        public SplitStopwatch SortObjectFileMemTimer = new SplitStopwatch();
        public static readonly bool SortObjectFileMemTimerOn = true;
        public SplitStopwatch SortObjectFileWriteTimer = new SplitStopwatch();
        public static readonly bool SortObjectFileWriteTimerOn = true;



       /// <summary>
       /// Creates sorted files named, outputPrefix+batchNo+outputSuffix
       /// </summary>
       /// <param name="mc"></param>
       /// <param name="batchSize"></param>
       /// <param name="sortField"></param>
       /// <param name="outputPrefix"></param>
       /// <param name="outputSuffix"></param>
        //public void SortObjectFile(int batchSize, string sortField, string outputPrefix,string outputSuffix)
        //{
        //    //logger.Info("sort input file {0}, batch size={1}" , this.fileName,batchSize);
        //    //this.mvm.DumpMemory();

        //    var mvm = this.bufferedFileSystem.mvm;
        //    int batchNo = 0;
        //    List<IObjectData> sortBatch = new List<IObjectData>();
        //    var bqueue = this;
        //    bqueue.Blocking = false;
        //    bool eof = false;
        //    while (!eof)
        //    {
        //        //logger.Info("reading upto {0} objects into memory ", batchSize);
        //        //mvm.DumpMemory();
        //        Cluster tempCluster = mvm.workMgr.clusterCache.CreateAndGetCluster();
        //        if (SortObjectFileReadTimerOn) this.SortObjectFileReadTimer.Start();
        //        for (int i = 0; i < batchSize; i++)
        //        {
        //            IObjectData csrObj = null;
        //            if (bqueue.ReadObject(mvm.mvmCluster, out csrObj))
        //            {
        //                sortBatch.Add(csrObj);
        //            }
        //            else
        //            {
        //                eof = true;
        //                break;
        //            }
        //        }
        //        if (SortObjectFileReadTimerOn)
        //        {
        //            this.SortObjectFileReadTimer.Stop();
        //            this.mvm.UpdateMvmCounterObject("sort_object_file_read_timer", this.SortObjectFileReadTimer.ElapsedMilliseconds.ToString());
        //        }
        //        if (sortBatch.Count > 0)
        //        {
        //            // sort the batch
        //            if (SortObjectFileMemTimerOn) this.SortObjectFileMemTimer.Start();
        //            var sortedIter = sortBatch.OrderBy(o => o[sortField]).ToArray();
        //            if (SortObjectFileMemTimerOn)
        //            {
        //                this.SortObjectFileMemTimer.Stop();
        //                this.mvm.UpdateMvmCounterObject("sort_object_file_mem_timer", this.SortObjectFileMemTimer.ElapsedMilliseconds.ToString());
        //            }
        //            // write out the sorted records
        //            if (SortObjectFileWriteTimerOn) this.SortObjectFileWriteTimer.Start();
        //            string outputFile = outputPrefix + (batchNo++).ToString() + outputSuffix;
        //            //logger.Info("Write out sorted batch file {0}", outputFile);
        //            using (var lockedItem = mvm.globalContext.bfs.GetObjectQueue(outputFile, false))
        //            {
        //                var bqueueOut = (lockedItem.value as ObjectQueueBufferedFile);
        //                foreach (IObjectData obj in sortedIter)
        //                {
        //                    bqueueOut.WriteObject(obj);
        //                }
        //            }
        //            if (SortObjectFileWriteTimerOn)
        //            {
        //                this.SortObjectFileWriteTimer.Stop();
        //                this.mvm.UpdateMvmCounterObject("sort_object_file_write_timer", this.SortObjectFileWriteTimer.ElapsedMilliseconds.ToString());
        //            }
        //            sortBatch.Clear();
        //            //logger.Info("cleared ObjectList for next round");
        //            //mvm.DumpMemory();
        //        }
        //        // remove the cluster and all its objects
        //        tempCluster.PurgeCluster();
        //        //logger.Info("purged temp cluster"); 
        //        //mvm.DumpMemory();

                
        //    }
        //    sortBatch = null;
        //    //logger.Info("done sort input file {0}, batch size={1}", this.fileName, batchSize);
        //    //this.mvm.DumpMemory();

        //    //logger.Info("COLLECTED GC done sort input file {0}, batch size={1}", this.fileName, batchSize);
        //    //System.GC.Collect();
        //}

        

        public ObjectQueueEnumerator GetObjectDataDeltaEnumerator(MvmEngine mvm, bool trackDelta){
            return new ObjectQueueEnumerator(mvm, this, trackDelta);
        }

        public class ObjectQueueEnumerator : IEnumerator<IObjectData>, IEnumerable<IObjectData>
        {
            public IObjectData targetObj;
            public ObjectQueueBufferedFile queue;
            public MvmClusterBase mvmCluster;
            public MvmEngine mvmEngine;
            public ObjectQueueEnumerator(MvmEngine mvmEngine, ObjectQueueBufferedFile queue, bool trackDelta)
            {
                this.mvmEngine = mvmEngine;
                this.mvmCluster = this.mvmEngine.mvmCluster;
                this.queue = queue;
            }
            public bool MoveNext()
            {
                // tbd:what do i do w/ previous target??
                return queue.ReadObject(mvmCluster, out targetObj);
            }
            public IObjectData Current
            {
                get { return this.targetObj; }
            }
            public void Dispose()
            {
                targetObj.Delete();
            }
            object System.Collections.IEnumerator.Current
            {
                get { return this.targetObj; }
            }
            public void Reset()
            {
                throw new NotImplementedException();
            }

            #region IEnumerable<IObjectData> Members

            public IEnumerator<IObjectData> GetEnumerator()
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
    }
}
