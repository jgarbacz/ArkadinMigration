using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{

    /// <summary>
    /// The concept of the Scheduler is that it is only accessed by one threadContext at a time
    /// whereas the SchedulerMaster is for the entire MvmEngine and must be threadContext safe.
    /// The scheduler can be used as a caching layer between Workers and the ScheduleMaster.
    /// </summary>
    public class Scheduler
    {
        // points to the master to get procContext definitions
        public readonly SchedulerMaster schedulerMaster;

        // Constructor
        public Scheduler(SchedulerMaster schedulerMaster)
        {
            this.schedulerMaster = schedulerMaster;
        }

        // Returns the latest definition for the passed procId.
        public ProcDefinition GetProcDefinition(int procId)
        {
            // Return latest procContext definition the master has. We might want to rethink this.
            return this.schedulerMaster.GetProcDefinition(procId);
        }

        private Dictionary<int, string> procIdNameMap = new Dictionary<int, string>();
        /// <summary>
        /// Returns the initNamespaceProcName for the passed procId
        /// </summary>
        /// <param name="procId"></param>
        /// <returns></returns>
        public string GetProcName(int procId)
        {
            string procName;
            if (procIdNameMap.TryGetValue(procId,out procName)) return procName;
            procName=GetProcDefinition(procId).procName;
            procIdNameMap[procId] = procName;
            return procName;
        }

        private Dictionary<int, string> procIdLocalNameMap = new Dictionary<int, string>();
        /// <summary>
        /// Returns the fullProcName for the passed procId
        /// </summary>
        /// <param name="procId"></param>
        /// <returns></returns>
        public string GetLocalProcName(int procId)
        {
            string procLocalName;
            if (procIdLocalNameMap.TryGetValue(procId, out procLocalName)) return procLocalName;
            procLocalName = GetProcDefinition(procId).localName;
            procIdLocalNameMap[procId] = procLocalName;
            return procLocalName;
        }

        private Dictionary<int, ProcType> procIdWorkTypeMap = new Dictionary<int, ProcType>();
        /// <summary>
        /// Returns the ProcType for the passed procId
        /// </summary>
        /// <param name="procId"></param>
        /// <returns></returns>
        public ProcType GetWorkType(int procId)
        {
            ProcType workType;
            if (procIdWorkTypeMap.TryGetValue(procId, out workType)) return workType;
            workType = GetProcDefinition(procId).procType;
            procIdWorkTypeMap[procId] = workType;
            return workType;
        }

        private Dictionary<int, ProcInfo> procIdInfoMap = new Dictionary<int, ProcInfo>();
        /// <summary>
        /// Returns the ProcInfo for the passed procId
        /// </summary>
        /// <param name="procId"></param>
        /// <returns></returns>
        public ProcInfo GetProcInfo(int procId)
        {
            ProcInfo procInfo;
            if (procIdInfoMap.TryGetValue(procId, out procInfo)) return procInfo;
            procInfo = schedulerMaster.GetProcInfo(procId);
            procIdInfoMap[procId] = procInfo;
            return procInfo;
        }

        //public ProcInst GetProcInst(int procId)
        //{
        //    ProcInfo procInfo = GetProcInfo(procId);
        //    ProcInst procInst = new ProcInst(procInfo);
        //    return procInst;
        //}

    }
}
