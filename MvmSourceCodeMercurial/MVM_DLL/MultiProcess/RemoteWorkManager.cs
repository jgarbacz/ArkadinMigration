using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NLog;
namespace MVM
{
    /// <summary>
    /// Keeps track of work that is running in other processes. This must be thread safe.
    /// </summary>
    public class RemoteWorkManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public readonly MvmEngine mvm;
        public RemoteWorkManager(MvmEngine mvm)
        {
            this.mvm = mvm;
        }

        public long nextBatchId=1;
        public long nextWorkId=1;
        private Dictionary<long, WorkInfo> workIdWorkMap = new Dictionary<long, WorkInfo>();
        private Dictionary<long, WorkBatch> batchIdBatchMap = new Dictionary<long, WorkBatch>();


        #region Tracking Batch create and complete counts
        
        // for debugging only
        private string IncompleteBatchIdsTxt
        {
            get
            {
                lock (this.batchIdBatchMap)
                {
                    string incompleteids = this.batchIdBatchMap.Where(r => r.Value.BatchComplete == false).Select(r => r.Key).JoinStrings(",");
                    return incompleteids;
                } 
            }
        }
        private object BatchesCreatedCompletedLock = new object();
        private long BatchesCreatedCount = 0;
        private long BatchesCompletedCount = 0;
        public void RegisterCreateBatch()
        {
            lock (this.BatchesCreatedCompletedLock)
            {
                this.BatchesCreatedCount++;
            }
        }
        public void RegisterCompleteBatch()
        {
            lock (this.BatchesCreatedCompletedLock)
            {
                this.BatchesCompletedCount++;
                // If we just completed all the outstanding work, then wakeup the workers incase
                // they are sleeping. This batch may not have an event that puts something in the 
                // worklist. In that case the workers waiting on the worklist would not wake up 
                // and would sleep forever instead of exiting.
                if (!this.HasWorkOutstanding)
                {
                    this.mvm.workMgr.WakeUp();
                }

            }
        }
        public bool HasWorkOutstanding
        {
            get
            {
                lock (this.BatchesCreatedCompletedLock)
                {
                    if (this.BatchesCreatedCount < this.BatchesCompletedCount)
                    {
                        throw new Exception("HasWorkOutstanding: completed>created??? create=" + this.BatchesCreatedCount + ",complete=" + this.BatchesCompletedCount);
                    }
                    //logger.Info("HasWorkOutstanding: create=" + this.BatchesCreatedCount + ",complete=" + this.BatchesCompletedCount);
                    return this.BatchesCreatedCount > this.BatchesCompletedCount;
                }
            }
        }
        #endregion


        public int RemoteWorkWorkCount
        {
            get
            {
                lock (workIdWorkMap)
                {
                    
                    return workIdWorkMap.Count;
                }
            }

        }

        public int RemoteWorkBatchCount
        {
            get
            {
                lock (batchIdBatchMap)
                {
                    return batchIdBatchMap.Count;
                }
            }
        }

        /// <summary>
        /// Clears out a batch
        /// </summary>
        /// <param name="batchId"></param>
        public void ClearBatch(long batchId)
        {
            lock (this.batchIdBatchMap)
            {
                WorkBatch workBatch = this.LookupBatch(batchId);
                this.batchIdBatchMap.Remove(batchId);
                foreach (WorkInfo workInfo in workBatch.GetWork())
                {
                    lock (workIdWorkMap)
                    {
                        workIdWorkMap.Remove(workInfo.workId);
                    }
                }
                // any batch that is cleared is completed.
                workBatch.ClearBatch();
            }
        }

        /// <summary>
        /// Clears work from the work manager and the batch it is in.
        /// </summary>
        /// <param name="workId"></param>
        public void ClearWork(WorkInfo workInfo)
        {
            workInfo.workBatch.RemoveWork(workInfo);
            lock (workIdWorkMap)
            {
                workIdWorkMap.Remove(workInfo.workId);
            }
        }

        
        /// <summary>
        /// creates a new work batch where you cannot add additional work items
        /// after the batch has been completed.
        /// </summary>
        /// <returns></returns>
        public long CreateBatch()
        {
            return this.CreateBatch(false);
        }

        /// <summary>
        /// If enableAddAfterComplete=true, creates a new work batch where you CAN add additional work items
        /// after the batch has been completed. In this case the batch will not
        /// appear completed until it is explicitely cleared. This is used by
        /// map reduce.
        /// </summary>
        /// <returns></returns>
        public long CreateBatch(bool enableAddAfterComplete)
        {
            WorkBatch workBatch = new WorkBatch(this, enableAddAfterComplete);
            lock (this.batchIdBatchMap)
            {
                this.batchIdBatchMap[workBatch.batchId] = workBatch;
            }
            //this.mvm.Log("create batch_id="+workBatch.batchId+" from " + System.Environment.StackTrace);
            this.RegisterCreateBatch();
            return workBatch.batchId;
        }

       

        /// <summary>
        /// Creates remote work in a batch
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public WorkInfo CreateWork(long batchId)
        {
            WorkBatch workBatch = this.LookupBatch(batchId);
            WorkInfo work=workBatch.CreateWork();
            lock (this.workIdWorkMap)
            {
                this.workIdWorkMap[work.workId] = work;
            }
            return work;
        }

        public WorkBatch LookupBatch(long batchId)
        {
            lock (this.batchIdBatchMap)
            {
                WorkBatch workBatch;
                if (!batchIdBatchMap.TryGetValue(batchId, out workBatch))
                    throw new Exception("unknown batchId:" + batchId);
                return workBatch;
            }
        }

        public WorkInfo LookupWork(long workId)
        {
            lock (this.workIdWorkMap)
            {
                WorkInfo workInfo;
                if (!workIdWorkMap.TryGetValue(workId, out workInfo))
                    throw new Exception("unknown workId:" + workId);
                return workInfo;
            }
        }

        public void UpdateWorkStatus(long workId, WorkStatus workStatus,object outputs)
        {
            WorkInfo workInfo=LookupWork(workId);
            workInfo.RecordResults(workStatus, outputs);
        }
    }


   /// <summary>
   /// Work belongs to a batch
   /// </summary>
    public class WorkBatch
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public readonly long batchId;
        public readonly RemoteWorkManager remoteWorkManager;
        private Dictionary<long, WorkInfo> batchWork = new Dictionary<long, WorkInfo>();
        private Queue<IBatchEvent> batchCompleteEvents = new Queue<IBatchEvent>();
        private Queue<IBatchEvent> unreducedWorkEvents = new Queue<IBatchEvent>();
        
        private long createdCount = 0;
        private long completedCount = 0;
        private long reducedCount = 0;

        private readonly bool enableAddAfterComplete;
        
        /// <summary>
        /// Keeps track of assigned work in progress. As soon as work is assigned
        /// to a nodeId, it is considered work in progresss. Once the work is
        /// returns its results it is not longer considered work in progress.
        /// <code>NodeWipCtr[nodeId]=#</code>.
        /// </summary>
        private List<int> NodeWorkInProgressCtr = new List<int>();

        public bool NoWorkInProgress(int nodeId)
        {
            lock (this)
            {
                if (nodeId > this.NodeWorkInProgressCtr.MaxIndex()) return true;
                return this.NodeWorkInProgressCtr[nodeId] <= 0;
            }
        }

        public void SetWorkNodeId(WorkInfo work,int nodeId)
        {
            lock (this)
            {
                // Currently do not support reassigning a work to a node.
                if (work.nodeId >= 0)
                {
                    throw new Exception("Error cannot reassign node id for work");
                }
                if (nodeId < 0)
                {
                    throw new Exception("Error cannot assign negative node id for work");
                }


                // back door set the node id on the workInfo
                work._setNodeId(nodeId);

                // register the work.
                this.NodeWorkInProgressCtr.SetMinCount(work.nodeId+1, 0);
                this.NodeWorkInProgressCtr[work.nodeId]+=1;
            }
        }

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="remoteWorkManager"></param>
        public WorkBatch(RemoteWorkManager remoteWorkManager,bool enableAddAfterComplete)
        {
            this.enableAddAfterComplete = enableAddAfterComplete;
            this.completedCount = 0;
            this.remoteWorkManager = remoteWorkManager;
            this.batchId = Interlocked.Increment(ref this.remoteWorkManager.nextBatchId);
        }

        /// <summary>
        /// Care should be taken that nobody messes with the batch after this is called. 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WorkInfo> GetWork()
        {
            lock (this)
            {
                return this.batchWork.Values;
            }
        }

        /// <summary>
        /// Care should be taken that nobody messes with the batch after this is called.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<WorkInfo> GetWorkEnumerator()
        {
            lock (this)
            {
                return batchWork.Values.GetEnumerator();
            }
        }

        /// <summary>
        /// Returns true all the work that has been created has also been completed.
        /// </summary>
        public bool BatchComplete
        {
            get
            {
                lock (this)
                {
                    return this.createdCount == this.completedCount;
                }
            }
        }

        /// <summary>
        /// Returns true if there is work that has been completed but has not been reduced.
        /// </summary>
        public bool HasUnreducedWork
        {
            get
            {
                lock (this)
                {
                    return this.completedCount > this.reducedCount;
                }
            }
        }

        /// <summary>
        /// Tells the batch that this work has completed. Fires events as needed.
        /// </summary>
        /// <param name="work"></param>
        public void RegisterWorkComplete(WorkInfo work){
            //logger.Trace("RegisterWorkComplete workId={0}, batch_id={1}", work.workId,work.batchId);
            lock (this)
            {
                // Record that the work is complete
                this.completedCount++;
                this.NodeWorkInProgressCtr[work.nodeId]--;
                work.status = WorkStatus.Complete;

                // Fire unreduced work events
                if (this.HasUnreducedWork)
                {
                    IBatchEvent batchEvent;
                    while (this.unreducedWorkEvents.Count > 0)
                    {
                        batchEvent = unreducedWorkEvents.Dequeue();
                        batchEvent.Fire(this);
                    }
                }
                // Fire batch compete events
                if (this.BatchComplete)
                {
                    IBatchEvent batchEvent;
                    while (batchCompleteEvents.Count>0)
                    {
                        batchEvent = batchCompleteEvents.Dequeue();
                        batchEvent.Fire(this);
                    }
                    // register that the batch is complete 
                    // It is very important that this happens after all events have been fired.
                    // If this isn't the case then it is possible that the WorkMgr returns control
                    // up to API caller level while there is still an event to that adds more work
                    // to the worklist.
                    this.registerBatchComplete();
                }
            }
        }

        // There are two ways a batch can be complete. Either, all work in the batch been completed
        // or the batch has been cleared. We need to keep the RemoteWorkMgr up to date on
        // batch complete counts and we cannot double report. So in here be sure to only report 
        // to remoteWorkMgr 1x per batch.
        private bool reportedBatchComplete = false;
        private void registerBatchComplete()
        {
            lock (this)
            {
                // if more work could be added in this batch return.
                if (this.enableAddAfterComplete)
                    return;
                if (this.reportedBatchComplete) 
                    return;
                this.remoteWorkManager.RegisterCompleteBatch();
                this.reportedBatchComplete = true;
            }
        }
        public void ClearBatch()
        {
            lock (this)
            {
                if (this.reportedBatchComplete)
                    return;
                this.remoteWorkManager.RegisterCompleteBatch();
                this.reportedBatchComplete = true;
            }
        }


        /// <summary>
        /// Creates new work in this batch.
        /// </summary>
        /// <returns></returns>
        public WorkInfo CreateWork(){
            lock (this)
            {
                this.createdCount++;
                WorkInfo work = new WorkInfo(this);
                this.batchWork[work.workId] = work;
                return work;
            }
        }

        /// <summary>
        /// Removes work from the batch.
        /// </summary>
        /// <param name="workInfo"></param>
        public void RemoveWork(WorkInfo workInfo)
        {
            lock (this)
            {
                this.batchWork.Remove(workInfo.workId);
            }
        }

        /// <summary>
        /// Adds a batch complete event that fires every time the batch
        /// is in a complete state meaning all work sent out has returned.
        /// </summary>
        /// <param name="batchEvent"></param>
        public void AddBatchCompleteEvent(IBatchEvent batchEvent)
        {
            lock (this)
            {
                // if the batch is complete and we've already started, fire the completion event now.
                if (this.BatchComplete && this.createdCount > 0)
                {
                    batchEvent.Fire(this);
                    return;
                }
                // otherwise queuue it and it will be fired when the batch is complete.
                batchCompleteEvents.Enqueue(batchEvent);
            }
        }

        /// <summary>
        /// Adds an event that fires when there is unreduced work
        /// </summary>
        /// <param name="batchEvent"></param>
        public void AddUnreducedWorkEvent(IBatchEvent batchEvent)
        {
            lock (this)
            {
                
                if (this.HasUnreducedWork)
                {
                    //logger.Trace("AddUnreducedWorkEvent.Fire()");
                    batchEvent.Fire(this);
                    return;
                }
                //logger.Trace("AddUnreducedWorkEvent.Enqueue()");
                unreducedWorkEvents.Enqueue(batchEvent);
            }
        }


        /// <summary>
        /// Sees if there is completed work to reduce. If there is it, takes
        /// the work out of the batch and returns it.
        /// </summary>
        /// <param name="workInfo"></param>
        /// <returns></returns>
        public bool TryGetWorkToReduce(out WorkInfo workInfo){
            lock (this)
            {
                this.reducedCount++;
                workInfo=this.batchWork.Values.Where(w => w.status == WorkStatus.Complete).FirstOrDefault();   
            }
            // intensionally do this outside lock so lock order is always: remoteWorkManager, batchWork.
            if (workInfo != null)
            {
                this.remoteWorkManager.ClearWork(workInfo);
                return true;
            }
            return false;
        }
    }



  /// <summary>
  /// Interface for events related to batches such as UnreducedWorkEvent and BatchCompleteEvent.
  /// </summary>
    public interface IBatchEvent
    {
        void Fire(WorkBatch workBatch);
    }

    /// <summary>
    /// Use this class create a batch event that allows caller to 
    /// safely wait for batch complete. You must be careful calling
    /// this as it ties up a thread until it gets a response.
    /// </summary>
    public class BlockingWaitBatchEvent : IBatchEvent
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        private AutoResetEvent autoResetEvent=new AutoResetEvent(false);
        public void WaitForBatchComplete()
        {
            this.autoResetEvent.WaitOne();
        }
        public void Fire(WorkBatch workBatch)
        {
            autoResetEvent.Set();
        }
    }
   

    public class WorkInfo
    {
        public readonly long nextWorkId = 1;
        public readonly WorkBatch workBatch;
        public string procName;
        public int priority;


        private int _nodeId=-1;
        public void _setNodeId(int nodeId)
        {
            this._nodeId = nodeId;
        }
        public int nodeId
        {
            get
            {
                return this._nodeId;
            }
            set
            {
                this.workBatch.SetWorkNodeId(this,value);
            }
        }
        public long workId;
        public long batchId{
            get{
                return this.workBatch.batchId;
            }
        }

        public WorkStatus status;
        public object outputs;

        public WorkInfo(WorkBatch workBatch)
        {
            this.workBatch = workBatch;
            this.workId = Interlocked.Increment(ref this.workBatch.remoteWorkManager.nextBatchId);
        }

        public void RecordResults(WorkStatus workStatus, object outputs)
        {
            this.status = workStatus;
            this.outputs = outputs;
            this.workBatch.RegisterWorkComplete(this);
        }
    }

    public enum WorkStatus
    {
        Produced,           // exists but not in the output yet so no way it could be running.
        WaitingToStart,     // still in our outbox,  or could be running... (currently do no distinguish)
        Complete,           // finished, updated when we get the status update.
        Reduced
    }

    public class CallbackBatchEvent : IBatchEvent
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private long callbackId;
        public CallbackBatchEvent(long callbackId)
        {
            this.callbackId = callbackId;
        }
        public void Fire(WorkBatch workBatch)
        {
            workBatch.remoteWorkManager.mvm.workMgr.FireCallback(this.callbackId);
            //logger.Debug("Fire callback to {0}", this.callbackId);

        }
    }
}
