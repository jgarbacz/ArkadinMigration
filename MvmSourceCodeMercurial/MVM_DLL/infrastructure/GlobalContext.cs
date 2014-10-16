using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;

/*
 * TBD:
 * 
 * If we're going to really have caching you need to checkout and checkin objects. Otherwise, we
 * will not be able to swap objects to disk. Although this might be slow, a benefit of this is that
 * every object has a semaphore on it so you can do procInst in parallel without worrying about race 
 * conditions.
 * 
 */

namespace MVM
{
    public class GlobalContext:IDisposable
    {
        // this is the single schema master used by all workers to get schema info
        public SchemaMaster schemaMaster;

        // represents the globalContext object
        public IObjectData globalObjectData;
        public WorkMgr workMgr;
        public BufferedFileSystem bfs;
        public GlobalContext(WorkMgr workMgr)
        {
            this.schemaMaster = new SchemaMaster(this);
            this.workMgr = workMgr;
            globalObjectData = ObjectDataStringHash.GetGlobalObjectData(this.workMgr.objectCache);
        }

        #region Manage GLOBAL.field

        public string this[string fieldName]
        {
            get
            {
                return this.globalObjectData[fieldName];
            }
            set
            {
                this.globalObjectData[fieldName] = value;
            }
        }
        #endregion

        #region Manage named globalContext class instances

        public SyncDictionary<string, object> namedClassInstMap = new SyncDictionary<string, object>();
        public bool HasNamedClassInst(string name)
        {
            object classInst = namedClassInstMap.GetOrNull(name);
            return classInst != null;
        }
        public object GetNamedClassInst(string name)
        {
            object classInst = namedClassInstMap.GetOrNull(name);
            if (classInst == null) throw new Exception("Error cannot find object for global name=[" + name + "]");
            return classInst;
        }
        public bool GetNamedClassInst(string name,out object classInst)
        {
            classInst = namedClassInstMap.GetOrNull(name);
            return classInst!=null;
        }
        public object GetNamedClassInst(string name, object defaultValue)
        {
            namedClassInstMap.AddIfNull(name, defaultValue);
            return GetNamedClassInst(name);
        }
        public void RmNamedClassInst(string name)
        {
            namedClassInstMap.Remove(name);
        }
        public void SetNamedClassInst(string name, object classInst)
        {
            namedClassInstMap[name] = classInst;
        }

        #endregion

        #region Manages globalContext monitors

        // manages monitors
        public Dictionary<string, SimpleLock> monitors = new Dictionary<string, SimpleLock>();
        public SimpleLock GetOrCreateMonitor(string monitorName)
        {
            SimpleLock monitor;
            lock (this.monitors)
            {
                if (!this.monitors.ContainsKey(monitorName))
                {
                    monitor = new SimpleLock();
                    monitors[monitorName] = monitor;
                    //Console.WriteLine("Created new monitor:" + monitor.GetHashCode() + "--" + Thread.CurrentThread.Name + "--" + monitorName);
                }
                monitor = monitors[monitorName];
                return monitor;
            }
        }

        public bool RemoveMonitor(string monitorName)
        {
            lock (this.monitors)
            {
                return monitors.Remove(monitorName);
            }
        }

        // creates monitor if does not exists
        public bool TryEnterMonitor(string monitorName)
        {
            SimpleLock monitor = GetOrCreateMonitor(monitorName);
            bool gotMonitor = monitor.TryAcquireLock();
            //Console.WriteLine("TryEnterMonitor(" + monitor.GetHashCode() + "--" +Thread.CurrentThread.Name+"--"+ monitorName + ") returns " + gotMonitor);
            return gotMonitor;
        }

        public bool EnterMonitor(string monitorName, int sleepMs)
        {
            SimpleLock monitor = GetOrCreateMonitor(monitorName);
            for (; ; )
            {
                bool gotMonitor = monitor.TryAcquireLock();
                Console.WriteLine("EnterMonitor:TryAcquireLock: monitorHash=" + monitor.GetHashCode() + ", thread=" + Thread.CurrentThread.Name + ",monitor=" + monitorName + ", gotMonitor=" + gotMonitor);
                if (gotMonitor) return gotMonitor;
                Thread.Sleep(sleepMs);
                //Console.WriteLine("EnterMoniter(" + monitor.GetHashCode() + "--" +Thread.CurrentThread.Name+"--"+ monitorName + ") returns " + gotMonitor);
                //return gotMonitor;
            }
        }
        public void ExitMonitor(string monitorName)
        {
            try
            {
                lock (this.monitors)
                {
                    //Console.WriteLine("ExitMonitor(" + monitorName + ")");
                    SimpleLock monitor = monitors[monitorName];
                    Console.WriteLine("ExitMonitor(monitorHash=" + monitor.GetHashCode() + ", thread=" + Thread.CurrentThread.Name + ",monitor=" + monitorName);
                    monitor.ReleaseLock();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error cannot exit monitor ExitMonitor, monitorName=[" + monitorName + "]", e);
            }
        }

        #endregion

        #region Maybe object/cluster caches belong here

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (this.bfs != null) this.bfs.Dispose();
        }

        #endregion


        public void InsertGlobalObject(string object_id)
        {
            this.workMgr.mvm.Log("inserting global object_id="+object_id);
            MemoryIndexSync parentUsageObjects = GetGlobalObjects();
            parentUsageObjects.IndexInsert(null,new List<string>{object_id});
    }

        private MemoryIndexSync GetGlobalObjects()
        {
            MemoryIndexSync globalObjects = this.GetNamedClassInst("GLOBAL_OBJECTS") as MemoryIndexSync;
            if (globalObjects == null)
            {
                throw new Exception("Error looks like GLOBAL_OBJECTS is undefined or not type MemoryIndexSync");
}
            return globalObjects;
        }
    
    }
}
