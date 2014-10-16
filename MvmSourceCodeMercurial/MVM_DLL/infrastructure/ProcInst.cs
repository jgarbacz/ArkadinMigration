using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    
    /// <summary>
    /// This is an runtime instance of a procContext call. Proc calls are first class object in the mvm.
    /// </summary>
    public class ProcInst :IDisposable
    {
        /// <summary>
        /// constructor a new instance of a proc
        /// </summary>
        /// <param name="procInfo"></param>
        /// <param name="objectId"></param>
        public ProcInst(ProcInfo procInfo, string objectId)
        {
            this.procInfo = procInfo;
            this.objectId = objectId;
            //this.mvm.Log("CONSTRUCTING: " + this.ToString());
        }

        /// <summary>
        /// Construct a new instance of a proc
        /// </summary>
        /// <param name="scheduler"></param>
        /// <param name="procId"></param>
        /// 
        /// <param name="objectId"></param>
        /// <returns></returns>
        public static ProcInst CallProcForObjectId(Scheduler scheduler, int procId, string objectId)
        {
            ProcInst w = new ProcInst(scheduler.GetProcInfo(procId), objectId);
            w.nextModuleOrder = scheduler.GetProcDefinition(procId).GetFirstOrder();
            w.procContext = new ProcContext();
            return w;
        }

        /// <summary>
        /// Returns true if the proc has started, false if it was instanciated but never started.
        /// </summary>
        public bool IsStarted
        {
            get
            {
                return this.nextModuleOrder != null;
            }
        }

        public MvmEngine mvm { get { return this.procInfo.mvm; } }
        public ObjectCache objectCache { get { return this.procInfo.mvm.objectCache; } }
        public IObjectData objectData { get { return this.objectCache.CheckOut(this.objectId); } }

        /// <summary>
        /// Returns true if the proc is complete.
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                // never started, cannot be completed
                if (!this.IsStarted) return false;
                // if it resumes another proc, then it cannot be completed.
                if (this.ResumeProcInst != null) return false;
                // if not on the final module, cannot be be completed.
                return this.nextModuleOrder.Equals(ModuleOrder.DoneOrder);
            }
        }

       
        /// <summary>
        /// The id for the proc.
        /// </summary>
        public int procId
        {
            get
            {
                return this.procInfo.procId;
            }
        }
        /// <summary>
        /// Full name of proc
        /// </summary>
        public string procName
        {
            get
            {
                return this.procInfo.procName;
            }
        }
        /// <summary>
        /// Short name of proc
        /// </summary>
        public string localName
        {
            get
            {
                return this.procInfo.localName;
            }
        }

        /// <summary>
        /// Static information for the proc from when it was instanciated.
        /// </summary>
        public readonly ProcInfo procInfo;

        /// <summary>
        /// Either standard block, catch block, or finally block
        /// </summary>
        public ProcType procType
        {
            get
            {
                return this.procInfo.procType;
            }
        }

        // Next newModule to fire for the procContext or ModuleOrder.DONE
        public ModuleOrder nextModuleOrder;

        // objectId to process the proc with
        public readonly string objectId;

        // Stores procContext scoped stuff (including tempContext scope)
        public ProcContext procContext;

        // Ctr indicating if the procInst under a synchronized block
        public int isSync = 0; 

        // Links this procInst to the procInst that should happen when this procInst is complete.
        public long callbackId=-1;

        // When a child procContext throws an exception it calls back the current procContext and
        // sets this field to the exception.
        public MvmUserException exception;

        // When a child procContext rolls up a break to label instruction does a procNameSyntax
        // to this piece of procInst and sets this value.
        public string breakFromProcName = null;

        // if this is set and this piece work us executed, the machine will
        // run execute the resumeProcInst instead of the current procInst. The
        // assumption is that the resumeProcInst is lower in the call same call
        // chain as the current proc inst so it will end up calling back the
        // current proc inst at some point.
        public ProcInst ResumeProcInst;


        private List<string> spawnedOids;
        public void RegisterSpawnedOid(string spawnedOid)
        {
            if (this.spawnedOids == null) this.spawnedOids = new List<string>();
            this.objectCache.RefGet(spawnedOid);
            spawnedOids.Add(spawnedOid);
        }
        public bool RemoveSpawnedOid(string spawnedOid)
        {
            if (this.spawnedOids == null) return false;
            if (this.spawnedOids.Remove(spawnedOid))
            {
                this.objectCache.RefRemove(spawnedOid);
                return true;
            }
            return false;

        }
        
        private bool isDisposed = false;
        public void Dispose()
        {
            if (!isDisposed)
            {
                //this.mvm.Log("DISPOSING: " + this.ToString());
                this.isDisposed = true;
                if (this.spawnedOids != null)
                {
                    foreach (var spawnedOid in this.spawnedOids)
                    {
                        this.objectCache.RefRemove(spawnedOid);
                    }
                }
            }
        }

        /// <summary>
        /// Destructor to catch when I am not disposing the proc instance.
        /// </summary>
        ~ProcInst()
        {
            if (!this.isDisposed)
            {
                //this.mvm.Trace("warning this proc was not explicitly disposed: " + this.ToString());
                this.Dispose();
            }
        }

        // for debugging
        override public string ToString()
        {
            return "work(proc=" + this.procName + ",obj=" + this.objectId + "/" + this.objectData.objectType + ",sync=" + this.isSync + ",ord=" + nextModuleOrder + ",cb=" + this.callbackId + ",workType=" + this.procType + ",started=" + this.IsStarted + ",complete=" + this.IsCompleted + ",hash="+this.GetHashCode()+ ")";
        }

        public string ToString(SchedulerMaster sm,WorkMgr workMgr)
        {
            string procName=sm.GetProcName(this.procId);
            string callback = "";
            if (this.callbackId >=0)
            {
                string callbackProcName = workMgr.GetCallbackProcInfo(this.callbackId).procName;
                callback = ", cb=" + callbackProcName;
            }
            return "work(procName=" + procName + ",sync=" + this.isSync + ",ord=" + nextModuleOrder + ",oid=" + this.objectId + callback + ",workType="+this.procType+")";
        }

        
    }
    }
