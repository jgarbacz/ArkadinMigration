using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace MVM
{
    // this is how newModules communicate up to the machine
    public enum ModuleStatus
    {
        Continue,
        Yield,
        BreakFromModules,
        Exception,
        BreakFromProcName,
        IdeBreakPoint,
        GotoModuleIdx
    }

    // Each instance is a threadContext that pulls and perform procInst from the WorkMgr
    public class Worker : IDisposable
    {
        public ModuleOrder currentModuleOrder = null;
        public Dictionary<string, XmlElement> ModuleOrderElementMap = new Dictionary<string, XmlElement>();

        #region Members setup by constructor

        // Links up to the Mvm
        public MvmEngine mvm;

        // Links to the procInst manager
        public WorkMgr workMgr;

        // Master of the config
        public SchedulerMaster schedulerMaster;

        // Local version of the SchedulerMaster
        public Scheduler scheduler;

        // The objectCache for the objects
        private ObjectCache objectCache;


        // This it this globalContext context for the machine
        private GlobalContext globalContext;

        // This is the threadContext context the scheduler is working in
        private ThreadContext threadContext;

        // The workerNo from 0 assigned to this worker
        public readonly int workerNo;
        public readonly string workerNoString;

        #endregion

        #region These properties are just so I can make sense of things though the debugger
        public ProcInst MyProcCall
        {
            get
            {
                return this.procInst;
            }
        }
        public ProcInfo MyProcInfo
        {
            get
            {
                if (this.procInst.procId >= 0)
                {
                    return this.scheduler.GetProcInfo(this.procInst.procId);
                }
                return null;
            }
        }
        public string MyProcName
        {
            get
            {
                if (this.MyProcInfo != null)
                {
                    return this.MyProcInfo.procName;
                }
                return null;
            }
        }
        public List<StackFrame> StackTrace
        {
            get
            {
                List<StackFrame> stack = new List<StackFrame>();
                lock (this.workMgr)
                {
                    ProcInst tempWork = this.procInst;
                    if (tempWork != null && tempWork.procId >= 0)
                    {
                        XmlElement elem = null;
                        string moduleOrder = this.currentModuleOrder == null ? "" : this.currentModuleOrder.ToString();
                        this.ModuleOrderElementMap.TryGetValue(tempWork.procId + ":" + moduleOrder, out elem);
                        if (elem == null)
                        {
                            stack.Insert(0, new StackFrame(this.mvm.nodeId, tempWork.procInfo.localName, tempWork.procInfo.nameSpace, "", "", -1));
                        }
                        else
                        {
                            string[] info = elem.GetAttributeDefaulted("_lineinfo", "unknown|-1").Split('|');
                            stack.Insert(0, new StackFrame(this.mvm.nodeId, tempWork.procInfo.localName, tempWork.procInfo.nameSpace, elem.LocalName, info[0], info[1].ToInt()));
                        }
                        do
                        {
                            if (tempWork.callbackId < 0)
                            {
                                // We're at the top level and have nowhere to call back to
                                break;
                            }
                            else if (!this.workMgr.callbacks.ContainsKey(tempWork.callbackId))
                            {
                                stack.Insert(0, new StackFrame(this.mvm.nodeId, "Unknown proc " + tempWork.callbackId.ToString(), "", "", "", -1));
                                break;
                            }
                            tempWork = this.workMgr.callbacks[tempWork.callbackId];
                            ProcInfo tempProcInfo = this.scheduler.GetProcInfo(tempWork.procId);
                            stack.Insert(0, new StackFrame(this.mvm.nodeId, tempProcInfo.localName, tempProcInfo.nameSpace, "", "", -1));
                        } while (tempWork.callbackId >= 0);
                    }
                    return stack;
                }
            }
        }
        public string MyStackTrace
        {
            get
            {
                StringBuilder output = new StringBuilder();
                lock (this.workMgr)
                {
                    ProcInst tempWork = this.procInst;
                    output.AppendLine(tempWork.procName);
                    do
                    {
                        if (!this.workMgr.callbacks.ContainsKey(tempWork.callbackId))
                        {
                            output.AppendLine("Broken call chain. No callbackId=" + tempWork.callbackId);
                            break;
                        }
                        tempWork = this.workMgr.callbacks[tempWork.callbackId];
                        ProcInfo tempProcInfo = this.scheduler.GetProcInfo(tempWork.procId);
                        output.AppendLine(tempProcInfo.procName);
                    } while (tempWork.callbackId >= 0);
                    return output.ToString();
                }
            }
        }
        public ProcInst CallbackWork
        {
            get
            {
                if (this.procInst.callbackId >=0)
                {
                    return this.workMgr.callbacks[this.procInst.callbackId];
                }
                return null;
            }
        }
        public ProcInfo CallbackProcInfo
        {
            get
            {
                if (this.CallbackWork != null)
                {
                    return this.scheduler.GetProcInfo(this.CallbackWork.procId);
                }
                return null;
            }
        }
        public string CallbackProcName
        {
            get
            {
                if (this.CallbackProcInfo != null)
                {
                    return this.CallbackProcInfo.procName;
                }
                return null;
            }
        }
        #endregion

        #region Members keeping state

        // Current procInst we're working on
        public ProcInst procInst;

        #endregion

        #region Interactions with WorkMgr

        public SplitStopwatch sleepStopWatch = new SplitStopwatch();

        // Creates a new worker with the passed starting object
        public Worker(WorkMgr workMgr, int workerNo)
        {
            this.workerNo = workerNo;
            this.workerNoString = this.workerNo.ToString();
            this.workMgr = workMgr;
            this.mvm = this.workMgr.mvm;
            this.schedulerMaster = workMgr.schedulerMaster;
            this.scheduler = this.schedulerMaster.GetScheduler();
            this.objectCache = this.workMgr.objectCache;
            this.globalContext = this.workMgr.globalContext;
            this.threadContext = new ThreadContext(this.globalContext, this.workerNo);
        }

      
        public WorkerState workerState=WorkerState.TryConsumeWork;

        /// <summary>
        /// Worker thread main loop
        /// </summary>
        public void Run()
        {
            this.mvm.Log("Worker thread [" + Thread.CurrentThread.Name + "] is alive");
            try
            {
                for (; ; )
                {
                    // Blocking call that consumes work or says it is time to exit
                    this.workMgr.ConsumeWork(this, out this.procInst);
                    if (this.workerState == WorkerState.TreadExit)
                    {
                        this.mvm.Log("Worker thread [" + Thread.CurrentThread.Name + "] exiting");
                        return;
                    }
                    this.DoWork();
                    // now that we are done working on this proc inst decrement the ref to 
                    // the proc's object. it may very well have already been incremented if this
                    // proc instance was put in a callback or back on the call stack. 
                    // 
                    // If it wasn't decrementing we will
                    // cause the object to be cleaned up if there are not other refs to the object.
                    // it is important to do this after we are done processing the object. The
                    // ref counter was originally incremented when the proc instance was added to
                    // the worklist.
                    // this.mvm.Log("REF IS BEING REMOVED FOR OID=" + this.procInst.objectId + " for proc=" + this.procInst.procName+",ctr="+this.procInst.objectData.RefCount);
                    this.objectCache.RefRemove(this.procInst.objectId);

                    // if the current proc is complete we want to call the Dispose() hook so it can release
                    // any resources it tied up.
                    if (this.procInst.IsCompleted) this.procInst.Dispose();
                }
            }
            catch (Exception e)
            {
                string messageText = "Worker thread " + Thread.CurrentThread.Name + " errored.".AppendLine();
                messageText = messageText.AppendLine("-".repeat(80));
                messageText = messageText.AppendLine(mvm.GetStackTrace(e));
                mvm.Fatal(messageText);
                mvm.Fatal("Worker '" + Thread.CurrentThread.Name + "' had a fatal exception, initiate SHUTDOWN ABORT!");
                mvm.FlushNLog();
                mvm.ShutdownAbort(messageText);
                // unlock top level thread which is waiting so that it exits MVM. We do not want to call
                // application.exit() as this would take down activity services or any other 
                // host so instead just get it so top thread exits.
                this.workMgr.WorkerThreadsAbort(e);
                return;
            }
        }

        /// <summary>
        /// Executes the current proc instance
        /// </summary>
        private void DoWork()
        {
            // Attach the procInst to this threadContext
            this.procInst.procContext.SetThreadContext(this.threadContext);

            // If this procInst has resume procInst then do so.
            if (this.procInst.ResumeProcInst != null)
            {
                var origProcInst = this.procInst;
                this.procInst = origProcInst.ResumeProcInst;
                origProcInst.ResumeProcInst = null;
                //Console.WriteLine("origProcInst=" + origProcInst.procName+" resuming ->"+this.procInst.procName);
            }
            // Setup the newModule context for the procInst
            ModuleContext mc = new ModuleContext(this.procInst, this);

            // Exception processing takes precedence over breaking.
            if (this.procInst.exception != null)
            {
                switch (this.procInst.procType)
                {
                    case ProcType.Standard:
                        mc.exception = procInst.exception;
                        goto RUN_CALLBACK;
                    case ProcType.Catch:
                        mc.caughtException = procInst.exception;
                        break;
                    case ProcType.Finally:
                        mc.exception = procInst.exception;
                        break;
                }
            }
            else if (this.procInst.breakFromProcName != null)
            {
                switch (this.procInst.procType)
                {
                    case ProcType.Standard:
                        mc.breakFromProcName = procInst.breakFromProcName;
                        goto RUN_CALLBACK;
                    case ProcType.Catch:
                        mc.breakFromProcName = procInst.breakFromProcName;
                        goto RUN_CALLBACK;
                    case ProcType.Finally:
                        mc.breakFromProcName = procInst.breakFromProcName;
                        break;
                }
            }
           // IModuleRun nothing = (IModuleRun)new MNothing();
        //RUN_MODULES:
            {
                if (procInst.nextModuleOrder == null)
                    procInst.nextModuleOrder = mc.procDefinition.GetFirstOrder();
                else if (procInst.nextModuleOrder.Equals(ModuleOrder.DoneOrder))
                    goto RUN_CALLBACK;
                using (mc.objectData = this.GetObjectData(procInst.objectId))
                {
                    int myCount = mc.procDefinition.moduleList.Count;
                    var myModuleList = mc.procDefinition.moduleList;
                    for (mc.moduleIdx = mc.procDefinition.GetIdxForOrder(this.procInst.nextModuleOrder); mc.moduleIdx < myCount; mc.moduleIdx++)
                    {
                        try
                        {
                            this.currentModuleOrder = mc.procDefinition.moduleOrders[mc.moduleIdx];
                            myModuleList[mc.moduleIdx].Run(mc);
                            this.currentModuleOrder = null;
                        }
                        catch (Exception e)
                        {
                            this.currentModuleOrder = null;
                            mc.moduleStatus = ModuleStatus.Exception;
                            mc.exception = e as MvmUserException;
                            if (mc.exception == null)
                            {
                                MvmUserException mvmUserException;
                                if (e.InnerException != null)
                                    mvmUserException = MvmUserException.Create(e.GetType().ToString(), e.Message, e);
                                else
                                    mvmUserException = MvmUserException.Create(e.GetType().ToString(), e.Message);
                                mvmUserException.StackTraceAppend(e.StackTrace);
                                mc.exception = mvmUserException;
                            }
                            // update next module order for this proc instance so that we can correctly report the line number of the error
                            this.procInst.nextModuleOrder = mc.procDefinition.GetNextModuleOrder(mc.moduleIdx);
                        }
                        switch (mc.moduleStatus)
                        {
                            case ModuleStatus.Continue: continue;
                            case ModuleStatus.GotoModuleIdx:
                                mc.moduleIdx = mc.gotoModuleIdx - 1;
                                mc.moduleStatus = ModuleStatus.Continue;
                                continue;
                            case ModuleStatus.Yield: goto RUN_YIELD;
                            case ModuleStatus.BreakFromModules: goto RUN_CALLBACK;
                            case ModuleStatus.BreakFromProcName: goto RUN_CALLBACK;
                            case ModuleStatus.Exception: goto RUN_CALLBACK;
                            case ModuleStatus.IdeBreakPoint:
                                continue; // put a breakpoint here to pause on <ide_break/>
                            default: throw new Exception("Unexpected ModuleStatus: " + mc.moduleStatus);
                        }
                    }
                    // mark that the work is complete
                    if (mc.moduleIdx == myCount)
                    {
                        this.procInst.nextModuleOrder = ModuleOrder.DoneOrder;
                    }
                }
            }
        // No more newModules OR current newModule has moduleStatus=BreakFromModules
        RUN_CALLBACK:
            {
                if (mc.exception != null)
                {
                    // If no callback we cannot roll up the exception so throw a real one.
                    if (this.procInst.callbackId < 0)
                    {
                        XmlElement errModuleElem = null;
                        ModuleOrder currModuleOrder = mc.procDefinition.GetPreviousModuleOrder(this.procInst.nextModuleOrder);
                        if (currModuleOrder != null &&
                            this.ModuleOrderElementMap.TryGetValue(mc.procInst.procId + ":" + currModuleOrder.ToString(), out errModuleElem))
                        {
                            string[] info = errModuleElem.GetAttributeDefaulted("_lineinfo", "unknown|-1").Split('|');
                            mc.exception.StackTraceAppend("   at mvm proc: " + mc.procName + " in " + info[0] + ", line " + info[1]);
                        }
                        else
                        {
                            mc.exception.StackTraceAppend("   at mvm proc: " + mc.procName + " on an unknown line");
                        }

                        throw new Exception("Uncaught mvm exception", mc.exception);
                    }
                    // Otherwise, append the current location to the exception stacktrace
                    if (mc.procInst.procId != schedulerMaster.popTheScopeProcId)
                    {
                        XmlElement errModuleElem = null;
                        ModuleOrder currModuleOrder = mc.procDefinition.GetPreviousModuleOrder(this.procInst.nextModuleOrder);
                        if (currModuleOrder != null &&
                            this.ModuleOrderElementMap.TryGetValue(mc.procInst.procId + ":" + currModuleOrder.ToString(), out errModuleElem))
                        {
                            string[] info = errModuleElem.GetAttributeDefaulted("_lineinfo", "unknown|-1").Split('|');
                            mc.exception.StackTraceAppend("   at mvm proc: " + mc.procName + " in " + info[0] + ", line " + info[1]);
                        }
                        else
                        {
                            mc.exception.StackTraceAppend("   at mvm proc: " + mc.procName + " on an unknown line");
                        }
                    }
                }
                if (this.procInst.callbackId >=0)
                {
                    if (mc.breakFromProcName != null)
                    {
                        if (!mc.breakFromProcName.Equals(mc.LocalName))
                        {
                            try
                            {
                                this.workMgr.RunCallback(this.procInst.callbackId, mc.exception, mc.breakFromProcName);
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                            return;
                        }
                        else
                        {
                            // On the break proc, so we will not procNameSyntax with the break label
                        }
                    }
                    try
                    {
                        this.workMgr.RunCallback(this.procInst.callbackId, mc.exception, null);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                return;
            }
        RUN_YIELD:
            {
                return;
            }
        }

        #endregion

        #region Interactions with ModuleContext

        // Looks up and returns an existing object
        public IObjectData GetObjectData(string objectId)
        {
            return this.objectCache.CheckOut(objectId);
        }

        public void ProduceWork(ProcInst work)
        {
            //Console.WriteLine("[produce:" + procInst + "]");
            this.workMgr.ProduceWork(work);
        }

        public void ProduceQueueWork(ProcInst work)
        {
            //Console.WriteLine("[produce:" + procInst + "]");
            this.workMgr.ProduceQueueWork(work);
        }

        public void ProduceStackWork(ProcInst work)
        {
            //Console.WriteLine("[produce:" + procInst + "]");
            this.workMgr.ProduceStackWork(work);
        }

        public string GetWorkerNoString()
        {
            return this.workerNoString;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.threadContext.Dispose();
        }

        #endregion
    }
}
