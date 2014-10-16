using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NLog;
namespace MVM
{
    public struct ThreadWorker
    {
        public int workerNo;
        public Worker worker;
        public Thread thread;
    }

    public enum WorkerState { DoWork, TreadExit, TryConsumeWork, TryRequestRemoteQueuedWork, RegisterAsIdle, CheckForExit, WaitForWork }


    public class WorkMgr
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // workers cannot exit when serviceCount > 0
        public int serviceCount = 0;

        #region Members that are setup once

        // Number of worker we want to run with
        public int numWorkers = 1;

        // Master of the config
        public SchedulerMaster schedulerMaster;

        // The objectCache for the objects
        public ObjectCache objectCache;



        // This it this globalContext context for the machine
        public GlobalContext globalContext;

        /// <summary>
        /// List of workers that are in a wait state.
        /// If someone adds work and there is already work todo, wake up a waiting worker.
        /// </summary>
        public Stack<Worker> waitingWorkers = new Stack<Worker>();

        // Workers are all written here by workerno
        public ThreadWorker[] threadWorkers;

        // This is the default threadContext worker that is used when processing globalContext newModules
        // and startup blocks.
        public Worker defaultWorker;

        // link to parent mvm
        public readonly MvmEngine mvm;

        #endregion

        #region Instanciation in starting

        // Constructor
        public WorkMgr(MvmEngine mvm)
        {
            this.mvm = mvm;
            this.globalContext = new GlobalContext(this);
            this.objectCache = new ObjectCache(this);
            this.schedulerMaster = new SchedulerMaster(this);
            this.worklist = new WorkList();
            this.worklist.schedulerMaster = this.schedulerMaster;
            this.worklist.workMgr = this;
            this.defaultWorker = new Worker(this, -1);
        }


        public void Abort(Exception e)
        {
            this.workersExit = true;
            this.workerException = e;
            this.WorkerIndicateAllWorkersComplete();
        }


        private AutoResetEvent allThreadIdleEvent = new AutoResetEvent(false);
        public void WorkMgrResetWorkToComplete()
        {
            //this.mvm.Log("this.allThreadIdleEvent.Reset()");
            this.allThreadIdleEvent.Reset();
        }
        public void WorkMgrWaitForWorkToComplete()
        {
            //this.mvm.Log("this.allThreadIdleEvent.WaitOne()");
            this.allThreadIdleEvent.WaitOne();
            //this.mvm.Log("this.allThreadIdleEvent.WaitOne() - GOTIT");
        }
        public void WorkerIndicateAllWorkersComplete()
        {
            // end is if a thread is going to go idle it sees if the conditions are right to wake the mgr.
            //this.mvm.Log("this.allThreadIdleEvent.Set()");
            this.allThreadIdleEvent.Set();
            if (this.workerException != null)
            {
                // We used to re-throw workerException here, but it's really not needed.  We've already
                // printed the exception using log.Fatal() so it just duplicates the stack trace, plus
                // it pops up the windows coredump notification every time which is unnecessary.
            }
        }

        public void StartupWorkerThreads()
        {
            this.WorkMgrResetWorkToComplete();

            // setup our structures
            this.threadWorkers = new ThreadWorker[this.numWorkers];
            // creates the workers (but don't start them)
            for (int i = 0; i < this.numWorkers; i++)
            {
                ThreadWorker tw = new ThreadWorker();
                tw.workerNo = i;
                tw.worker = new Worker(this, tw.workerNo);
                ThreadStart job = new ThreadStart(tw.worker.Run);
                tw.thread = new Thread(job);
                string threadName = "W" + tw.workerNo;
                tw.thread.Name = threadName;
                threadWorkers[tw.workerNo] = tw;
            }
            // start the workers
            foreach (ThreadWorker tw in this.threadWorkers)
            {
                tw.thread.Start();
            }

            // wait until all the threads have started
            this.WorkMgrWaitForWorkToComplete();
            this.mvm.Log("all threads have started");
        }

        public void ReapWorkerThreads()
        {
            foreach (ThreadWorker tw in this.threadWorkers)
            {
                for (; ; )
                {
                    // do it this way so we can insert a break point in else
                    if (tw.thread.Join(5000))
                    {
                        tw.worker.Dispose();
                        logger.Info("disposing worker " + tw.worker.GetWorkerNoString() + ".");
                        //logger.Info("disposing worker " + tw.worker.GetWorkerNoString() + ".");
                        break;
                    }
                    else
                    {
                    }
                }
            }
        }

        // return a single worker.
        public Worker GetDefaultWorker()
        {
            return this.defaultWorker;
        }


        #endregion

        #region Manage callbacks

        // Counter for generating unique callback ids
        private long nextCallbackId = 0;

        // ProcInst that is waiting for someone to call it back
        public Dictionary<long, ProcInst> callbacks = new Dictionary<long, ProcInst>();

        // Returns a new unique procNameSyntax name
        private long GetNextCallbackId()
        {
            return Interlocked.Increment(ref this.nextCallbackId);
        }

        // Traverses passed procInst and procNameSyntax chain and returns the
        // first piece of procInst that has IsEntryPoint=1.
        public ProcInst GetEntryWork(ProcInst currentWork)
        {
            lock (this)
            {
                for (; ; )
                {
                    if (currentWork.procId != schedulerMaster.popTheScopeProcId && currentWork.procInfo.isEntryPoint) return currentWork;
                    if (currentWork.callbackId < 0) return null;
                    currentWork = this.callbacks[currentWork.callbackId];
                }
            }
        }

        // Traverses passed procInst and procNameSyntax chain and returns the
        // first piece of procInst that has isEntryPoint==true.
        public ProcInst GetEntryPointWork(ProcInst currentWork)
        {
            lock (this)
            {
                for (; ; )
                {
                    if (currentWork.procInfo.isEntryPoint) return currentWork;
                    if (currentWork.callbackId < 0) return null;
                    currentWork = this.callbacks[currentWork.callbackId];
                }
            }
        }
        // Traverses passed procInst and procNameSyntax chain and returns the
        // first piece of procInst that has isEntryPoint==true.
        public ProcInst GetEntryPointsParentWork(ProcInst currentWork)
        {
            lock (this)
            {
                ProcInst entryPoint = this.GetEntryPointWork(currentWork);
                if (entryPoint == null) return null;
                if (entryPoint.callbackId < 0) return null;
                ProcInst entryPointParent = this.callbacks[entryPoint.callbackId];
                return entryPointParent;
            }
        }

        /// <summary>
        ///  Stores a proc instance and return the procInstId
        ///  this is currently use 2 way:
        ///     1) for normal callbacks which get fired
        ///     2) for storing proc instances in vars which can be resumed (pulsed)
        ///  We should break this up into 2 concepts.
        /// </summary>
        /// <param name="callbackWork"></param>
        /// <returns></returns>
        public long CreateCallback(ProcInst callbackProcInst)
        {
            lock (this)
            {
                // callbacks hold an object ref
                this.objectCache.RefGet(callbackProcInst.objectId);
                long callbackId = GetNextCallbackId();
                this.callbacks[callbackId] = callbackProcInst;
                return callbackId;
            }
        }

        private ProcInst ReadCallback(long callbackId)
        {
            lock (this)
            {
                ProcInst cb = this.callbacks[callbackId];
                return cb;
            }
        }



        /// <summary>
        /// Returns true if the callback proc has completed
        /// </summary>
        /// <param name="callbackId"></param>
        /// <returns></returns>
        public bool CallbackIsComplete(long callbackId)
        {
            ProcInst procInst = this.ReadCallback(callbackId);
            return procInst.IsCompleted;
        }

        public ProcInfo GetCallbackProcInfo(long callbackId)
        {
            ProcInst procInst = this.ReadCallback(callbackId);
            return procInst.procInfo;
        }

        /// <summary>
        /// Returns the TempContext for the callback proc
        /// </summary>
        /// <param name="callbackId"></param>
        /// <returns></returns>
        public TempContext GetCallbacksTempContext(long callbackId)
        {
            ProcInst procInst = this.ReadCallback(callbackId);
            return procInst.procContext.tempContext;
        }

        /// <summary>
        ///  removes a proc inst
        /// </summary>
        /// <param name="callbackId"></param>
        private void RemoveCallbackId(long callbackId)
        {
            lock (this)
            {
                var procInst = this.callbacks[callbackId];
                procInst.objectData.RefRemove();
                this.callbacks.Remove(callbackId);
            }
        }

        /// <summary>
        /// Safely delete the proc instance by making it break its way up the callstack, allowing 
        /// finally blocks to fire as the should. This must be called from a module.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="delProcInstId"></param>
        public void DeleteCallback(ModuleContext mc, long delProcInstId)
        {
            //this.mvm.Log("DeleteCallback(" + delProcInstId + ")");
            ProcInst delProcInst = this.ReadCallback(delProcInstId);
            // if the proc is complete just remove the proc ref from callbacks.
            if (delProcInst.IsCompleted)
            {
                //this.mvm.Log("proc instance proc inst id [" + delProcInstId + "] [" + delProcInst.localName + "] is complete so deleted it");
                this.RemoveCallbackId(delProcInstId);
                return;
            }
            // if the proc never started then just remove proc ref from callbacks
            if (!delProcInst.IsStarted)
            {
                //this.mvm.Log("proc instance proc inst id [" + delProcInstId + "] [" + delProcInst.localName + "] never started so deleted it");
                this.RemoveCallbackId(delProcInstId);
                return;
            }
            // If we are here we are trying to delete a proc instance that has already started. It is possible
            // that this proc insance needs to resume other lower level procs. So if this proc is 'A', it is possible
            // that A->B which yielded so when A is pulsed it would resume B. We need to dig all the way down to the 
            // deepest proc instance and set it up to break all the way up to the current level. When we do this
            // any finally blocks in the call chain will get fired.
            ProcInst deepestProcInst = delProcInst;
            while (deepestProcInst.ResumeProcInst != null)
            {
                deepestProcInst = deepestProcInst.ResumeProcInst;
            }
            deepestProcInst.breakFromProcName = delProcInst.localName;
            {
                // now resume the deleted proc and make it callback to the current proc.
                this.FireCallback(mc.procInst, delProcInstId);

                //make sure the current proc picks up on the next module.
                mc.procInst.nextModuleOrder = mc.procDefinition.GetNextModuleOrder(mc.moduleIdx);
                mc.moduleStatus = ModuleStatus.Yield;
            }
        }

        /// <summary>
        ///  Pulses a proc instance and makes it callback to the passed proc instance.
        ///  Pulsing does not delete the callback id
        /// </summary>
        /// <param name="currentProcInst"></param>
        /// <param name="procInstIdSyntax"></param>
        public void PulseCallback(ProcInst currentProcInst, long pulseCallbackId)
        {
            lock (this)
            {
                ProcInst pulseProcInst = this.callbacks[pulseCallbackId];
                pulseProcInst.callbackId = CreateCallback(currentProcInst);
                this.ProduceWork(pulseProcInst);
            }
        }

        /// <summary>
        ///  Fires and removes the callback. Makes sure that when the callback is
        ///  complete that it then calls back to the passed proc inst.
        /// </summary>
        /// <param name="currentProcInst"></param>
        /// <param name="procInstIdSyntax"></param>
        public void FireCallback(ProcInst currentProcInst, long callbackId)
        {
            lock (this)
            {
                ProcInst cb = this.callbacks[callbackId];
                this.callbacks.Remove(callbackId);
                cb.callbackId = CreateCallback(currentProcInst);
                this.ProduceWork(cb);
                // intensionally remove the ref after we produce so refctr does not hit zero
                cb.objectData.RefRemove();
            }
        }

        /// <summary>
        ///  Fires and removes the callback.
        /// </summary>
        /// <param name="callbackId"></param>
        public void FireCallback(long callbackId)
        {
            lock (this)
            {
                ProcInst cb = this.callbacks[callbackId];
                this.callbacks.Remove(callbackId);
                this.ProduceWork(cb);
                // intensionally remove the ref after we produce so refctr does not hit zero
                cb.objectData.RefRemove();
            }
        }


        public void RunCallback(long callbackId, MvmUserException exception, string breakToLabel)
        {
            lock (this)
            {
                // get the procInst
                ProcInst cb = this.callbacks[callbackId];
                // if there is already a break in there, leave it
                if (cb.breakFromProcName == null)
                    cb.breakFromProcName = breakToLabel;

                // if we are passed an exception
                if (exception != null)
                {
                    // if there is no exception set to the passed, otherwise chain them.
                    if (cb.exception == null)
                    {
                        cb.exception = exception;
                    }
                    else
                    {
                        // Need to chain 2 exception into 1. Unfortunately this is not possible since you cannot
                        // update the InnerException property in dotnet. To get around this we include the stacktrace
                        // of the outer exception in the outter exceptions error message. 
                        //
                        // resumeProcInst.exception will be the first one, inner
                        // exception will be the second one, outer, liker finally
                        var innerException = cb.exception;
                        var outerException = exception;
                        string outerExceptionName = outerException.exceptionName;
                        string outerExceptionMessage = outerException.exceptionMessage.AppendLine() + outerException.StackTrace;
                        var mergedException = MvmUserException.Create(outerExceptionName, outerExceptionMessage, innerException);
                        cb.exception = mergedException;
                    }
                }
                this.FireCallback(callbackId);
            }
        }

        public string GetCallbacksString()
        {
            lock (this)
            {
                StringBuilder result = new StringBuilder("CALLBACKS:");
                foreach (long callbackId in this.callbacks.Keys)
                {
                    result.AppendLine("CB:" + callbackId + "=" + this.callbacks[callbackId]);
                }
                return result.ToString();
            }
        }

        public void PrintCallbacks()
        {
            this.mvm.Log(this.GetCallbacksString());
        }


        #endregion


        #region Proc References

        // Counter for generating unique callback ids
        private long nextProcRefId = 0;

        // symbolic ref
        public Dictionary<string, ProcInst> procRefIdMap = new Dictionary<string, ProcInst>();

        private string GetNextProcRefId()
        {
            lock (this)
            {
                return (this.nextProcRefId++).ToString();
            }
        }

        public string CreateRefToProcInst(ProcInst procInst)
        {
            lock (this)
            {
                // callbacks hold an object ref
                this.objectCache.RefGet(procInst.objectId);
                string procRefId = GetNextProcRefId();
                this.procRefIdMap[procRefId] = procInst;
                return procRefId;
            }
        }




        #endregion


        #region Manage producer consumer

        /// <summary>
        /// This stores work to be done.
        /// </summary>
        private WorkList worklist = new WorkList();


        /// <summary>
        /// Called by Worker threads to get work. This will block until either there is work todo or
        /// all workers are idle and it is time to exit. 
        /// </summary>
        /// <param name="work"></param>
        /// <param name="workerState"></param>
        public void ConsumeWork(Worker worker, out ProcInst work)
        {
            worker.workerState = WorkerState.TryConsumeWork;

            // this is the critical section that only 1 worker can be in so that only
            // one worker thread can get a piece of work.
            lock (this.worklist)
            {
                for (; ; )
                {
                NEXT_STATE:
                    //this.mvm.Log("Enter state: "+ worker.workerState);
                    switch (worker.workerState)
                    {
                        case WorkerState.TryConsumeWork:
                            {
                                work = this.worklist.ConsumeWork();
                                if (work != null)
                                {
                                    worker.workerState = WorkerState.DoWork;
                                    return;
                                }
                                worker.workerState = WorkerState.TryRequestRemoteQueuedWork;
                                goto NEXT_STATE;
                            }
                        case WorkerState.TryRequestRemoteQueuedWork:
                            {
                                if (this.mvm.mvmCluster.RequestMaxWaitingWork())
                                {
                                    //logger.Info("worker {0} is requested remote waiting work.", worker.workerNo);
                                }
                                else
                                {
                                    //logger.Info("worker {0} did not find waiting work to request.", worker.workerNo);
                                }
                                worker.workerState = WorkerState.RegisterAsIdle;
                                goto NEXT_STATE;
                            }
                        case WorkerState.RegisterAsIdle:
                            {
                                Interlocked.Increment(ref numIdleWorkers);
                                worker.workerState = WorkerState.CheckForExit;
                                goto NEXT_STATE;
                            }
                        case WorkerState.CheckForExit:
                            {
                                // if we are the last thread to go idle, then we need to alert the work
                                // manager so they can return control and decide what to do.
                                // ROB DO NOT DO THIS IF THERE IS REMOTE WORK OUTSTANDING!!!!
                                if (this.numIdleWorkers == this.numWorkers)
                                {
                                    if (this.mvm.remoteWorkMgr.HasWorkOutstanding == false || this.workersExit)
                                    {
                                        //logger.Trace("worker {0}: is last worker to go idle, alert the work mgr", worker.GetWorkerNoString());
                                        this.WorkerIndicateAllWorkersComplete();
                                    }
                                    else
                                    {
                                        //logger.Trace("worker {0}: is last worker to go idle, but there is still outstanding work!");
                                    }
                                }
                                worker.workerState = WorkerState.WaitForWork;
                                goto NEXT_STATE;
                            }
                        case WorkerState.WaitForWork:
                            {
                                //logger.Trace("worker {0} waiting on the worklist", worker.workerNo);
                                worker.sleepStopWatch.Start();
                                //while (!Monitor.Wait(this.worklist, 3000)) // use it like this when debugging.
                                while (!Monitor.Wait(this.worklist))
                                {
                                    worker.sleepStopWatch.Stop();
                                    //logger.Trace("still waiting, check for exit");
                                    worker.workerState = WorkerState.CheckForExit;
                                    goto NEXT_STATE;
                                }
                                worker.sleepStopWatch.Stop();

                                // no longer idle
                                Interlocked.Decrement(ref numIdleWorkers);
                                //logger.Trace("worker {0} woke up after {1} ms", worker.workerNo, worker.sleepStopWatch.SplitMilliseconds);

                                if (this.workersExit)
                                    worker.workerState = WorkerState.TreadExit;
                                else
                                    worker.workerState = WorkerState.TryConsumeWork;
                                goto NEXT_STATE;
                            }
                        case WorkerState.TreadExit:
                            {
                                work = null;
                                //logger.Trace("worker {0}: is going exit. Its total sleep time was {1} ms", worker.workerNo, worker.sleepStopWatch.ElapsedMilliseconds);
                                Monitor.PulseAll(this.worklist);
                                return;
                            }
                    }
                }
            }
        }


        private volatile bool workersExit = false;
        private volatile Exception workerException = null;
        public void ShutdownWorkers()
        {
            if (this.numIdleWorkers != this.numWorkers)
            {
                //mvm.Log("WARNING SHUTDOWN WAS CALLED WHEN THERE WERE STILL WORKERS WORKING!");
            }
            this.workersExit = true;
            this.WakeUp();
        }

        public void WakeUp()
        {
            lock (this.worklist)
            {
                Monitor.PulseAll(this.worklist);
            }
        }

        public bool HaveException()
        {
            return this.workerException != null;
        }
        public Exception GetException()
        {
            return this.workerException;
        }

        public void WorkerThreadsAbort(Exception e)
        {
            foreach (var tw in this.threadWorkers)
            {
                tw.worker.workerState = WorkerState.TreadExit;
            }
            this.Abort(e);
        }

        /// <summary>
        /// Number of workers that are idle
        /// </summary>
        private int numIdleWorkers = 0;

        /// <summary>
        /// Called from produce methods to wake up the correct number of idle workers.
        /// </summary>
        private void PulseIdleWorkers()
        {
            lock (this.worklist)
            {
                int numPulses = Math.Min(this.worklist.Count, this.numIdleWorkers);
                for (int i = 0; i < numPulses; i++) Monitor.Pulse(this.worklist);
            }
        }

        /// <summary>
        ///  Adds work to the work queue
        /// </summary>
        /// <param name="w"></param>
        public void ProduceQueueWork(ProcInst w)
        {
            lock (this.worklist)
            {
                // increment the ref counter to the proc's object
                this.objectCache.RefGet(w.objectId);
                this.worklist.ProduceQueueWork(w);
                this.PulseIdleWorkers();
            }
        }

        /// <summary>
        /// Adds work to the work stack
        /// </summary>
        /// <param name="w"></param>
        public void ProduceStackWork(ProcInst w)
        {
            lock (this.worklist)
            {
                // increment the ref counter to the proc's object
                this.objectCache.RefGet(w.objectId);
                this.worklist.ProduceStackWork(w);
                this.PulseIdleWorkers();
            }
        }

        /// <summary>
        /// If work is synchronous, add it to the queue, else the stack.
        /// </summary>
        /// <param name="w"></param>
        public void ProduceWork(ProcInst w)
        {
            if (w.isSync > 0)
                this.ProduceQueueWork(w);
            else
                this.ProduceStackWork(w);
        }

        /// <summary>
        /// Produce multiple pieces of work at once.
        /// </summary>
        /// <param name="work"></param>
        public void ProduceWork(List<ProcInst> work)
        {
            foreach (ProcInst w in work) this.ProduceWork(w);
        }


        #endregion
    }
}
