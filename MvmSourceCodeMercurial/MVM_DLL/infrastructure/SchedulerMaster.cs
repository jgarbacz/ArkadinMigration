using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading;
using System.Linq;
using System.IO;

namespace MVM
{

    public class SchedulerMaster
    {
        public readonly WorkMgr workMgr;
        public MvmEngine mvm
        {
            get
            {
                return this.workMgr.mvm;
            }
        }
        public SchedulerMaster(WorkMgr workMgr)
        {
            this.workMgr = workMgr;

            // load pop_the_scope
            this.ReadXmlConfigFromString("<proc name='pop_the_scope' work_type='finally' is_entry='false'><pop_scope/></proc>",this.workMgr.defaultWorker,"global", new ConfigLocator("embedded"));
            this.GetProcId("global","pop_the_scope");
        }

        public int popTheScopeProcId;

        public ProcInst GetProcInst(int procId,string objectId)
        {
            ProcInfo procInfo = GetProcInfo(procId);
            ProcInst procInst = new ProcInst(procInfo,objectId);
            return procInst;
        }

        // This class is used to make changes to existing procs. When you have
        // a handle to it, you simply manipulate nextProc and dispose this object
        // and the changes flow through to the schedule master.
        public class ProcChange : IDisposable
        {
            private readonly object inUse = new Object();
            private readonly SchedulerMaster schedulerMaster;
            public ProcDefinition currProc { get; private set; }
            public ProcDefinition nextProc;
            public int procId;

            // constructor
            public ProcChange(SchedulerMaster schedulerMaster, int procId)
            {
                this.schedulerMaster = schedulerMaster;
                this.procId = procId;
            }

            // sets it up and ensures that we have exclusive access to it until it is disposed
            public void Setup()
            {
                Monitor.Enter(this.inUse);
                {
                    this.currProc = this.schedulerMaster.GetProcDefinition(this.procId);
                    this.nextProc = this.currProc.GetNextVersion();
                }
            }
            // sync changes with schedule master procContext defs and unlock procContext so it can be changes again
            public void Dispose()
            {
                //Console.WriteLine("committing changeProc(" + initNamespaceProcName + ")");
                schedulerMaster.SetProcDef(this.nextProc);
                Monitor.Exit(this.inUse);
            }
        }

        // Maps a proc_id to its latest definition.
        // procDefs[procId]=ProcDefinition.
        private ReaderWriterLock procDefsLock = new ReaderWriterLock();
        private List<ProcDefinition> procDefs = new List<ProcDefinition>();

        // Maps a proc_id to its static procContext information.
        // procInfos[procId]=ProcDefinition.
        private ReaderWriterLock procInfosLock = new ReaderWriterLock();
        private List<ProcInfo> procInfos = new List<ProcInfo>();


        // Maps a full initNamespaceProcName to its procId
        // procDefMap["nameSpace.initNamespaceProcName"]=procId;
        private Dictionary<string, int> procDefMap = new Dictionary<string, int>();
        private ReaderWriterLock procDefsMapLock = new ReaderWriterLock();

        // Maps a local procContext name to the namespaces that have that procContext defined.
        // procNameSpaceMap["fullProcName"]["nameSpace"]="nameSpace.fullProcName";
        private Dictionary<string, Dictionary<string, string>> procNameSpaceMap = new Dictionary<string, Dictionary<string, string>>();
        private ReaderWriterLock procNameSpaceMapLock = new ReaderWriterLock();

        // Maps a namespace to it main directory. Typically for structural namespaces.
        private Dictionary<string, string> nameSpaceDirMap = new Dictionary<string, string>();
        private ReaderWriterLock nameSpaceDirMapMapLock = new ReaderWriterLock();


        // Populated by ProcessConfig and maps the full procContext names to their xml elements
        //xmlProcInfoMap["nameSpace.fullProcName"]=ProcInfo
        private SynchronizedDictionary<string, ProcInfo> xmlProcInfoMap = new SynchronizedDictionary<string, ProcInfo>();

        // Manages changes to a procContext. once we add to this dictionary we never RemoveSpecificItem stuff. we just 
        // reuse the ProcChange for future changes.
        private Dictionary<int, ProcChange> procChg = new Dictionary<int, ProcChange>();

        public void GetDocumentedProcs(out IEnumerable<ProcInfo> procs)
        {
            var result = from kv in this.xmlProcInfoMap
                         where kv.Value.procdoc != null
                         select kv.Value;
            procs = result;
        }

        public List<ProcInfo> GetMvmScriptUnitTests()
        {
            List<ProcInfo> results = new List<ProcInfo>();
            lock (this.xmlProcInfoMap.SyncRoot)
            {
                foreach (var procInfo in this.xmlProcInfoMap.Values)
                {
                    if (procInfo.procElem.GetAttribute("test").Equals("true"))
                    {
                        results.Add(procInfo);
                    }
                   
                }
            }
            return results;
        }

         /// <summary>
         /// Returns the next procId. This is a reserved index in this.procDefs and this.procInfos
         /// </summary>
         /// <returns></returns>
        public int GetNextProcId(ProcInfo procInfo){
            if (procInfo.procId != 0) throw new Exception("unexpected");
            int procId;
            this.procDefsLock.AcquireWriterLock(-1);
            try
            {
                    procId = this.procDefs.Count;
                    this.procDefs.Add(null);
            }
            finally
            {
                this.procDefsLock.ReleaseWriterLock();
            }
            this.procInfosLock.AcquireWriterLock(-1);
            try
            {
                this.procInfos.Add(procInfo);
            }
            finally
            {
                this.procInfosLock.ReleaseWriterLock();
            }
            return procId;
        }

        public ProcInfo GetProcInfo(int procId)
        {
            this.procInfosLock.AcquireReaderLock(-1);
            try
            {
                return this.procInfos[procId];
            }
            finally
            {
                this.procInfosLock.ReleaseReaderLock();
            }
        }

        public ProcInfo TryGetProcInfo(string currentNameSpace, string localName)
        {
            string nameSpace, procName;
            string error = this.TryResolveProcName(currentNameSpace, localName, out nameSpace, out procName);
            if (error != null)
            {
                return null;
            }
            return xmlProcInfoMap[procName];
        }

        // returns the procContext info for a procContext.
        public ProcInfo GetProcInfo(string currentNameSpace, string localName)
        {
            string nameSpace, procName;
            this.ResolveProcName(currentNameSpace, localName, out nameSpace, out procName);
            return xmlProcInfoMap[procName];
        }

        public void SetNameSpaceDir(string nameSpace, string dir)
        {
            this.nameSpaceDirMapMapLock.AcquireWriterLock(-1);
            try
            {
                this.nameSpaceDirMap[nameSpace] = dir;
            }
            finally
            {
                this.nameSpaceDirMapMapLock.ReleaseWriterLock();
            }
        }
        public string GetNameSpaceDir(string nameSpace)
        {
            string dir;
            this.nameSpaceDirMapMapLock.AcquireReaderLock(-1);
            try
            {
                dir = this.nameSpaceDirMap[nameSpace];
            }
            finally
            {
                this.nameSpaceDirMapMapLock.ReleaseReaderLock();
            }
            return dir;
        }
        public string GetGeneratedDir(string nameSpace, string label)
        {
            string genDirPath = this.workMgr.mvm.metraTechMvmGeneratedDir
                + Path.DirectorySeparatorChar
                + this.workMgr.mvm.MvmUniqueId
                + Path.DirectorySeparatorChar
                 + nameSpace
                + Path.DirectorySeparatorChar
                + label;
            DirectoryInfo genDirInfo = new DirectoryInfo(genDirPath);
            genDirInfo.CreateIfNotThere();
            return genDirInfo.FullName;
        }

        // get the procContext definition (threadContext safe);
        public ProcDefinition GetProcDefinition(int procId)
        {
            ProcDefinition procDef = null;
            this.procDefsLock.AcquireReaderLock(-1);
            try
            {
                procDef = this.procDefs[procId];
            }
            finally
            {
                this.procDefsLock.ReleaseReaderLock();
            }
            return procDef;
        }
        // get the procContext definition (threadContext safe);
        public ProcDefinition TryGetProcDefinition(int procId)
        {
            ProcDefinition procDef = null;
            this.procDefsLock.AcquireReaderLock(-1);
            try
            {
                procDef = procId < (this.procDefs.Count - 1) && procId >= 0 ? this.procDefs[procId] : null;
            }
            finally
            {
                this.procDefsLock.ReleaseReaderLock();
            }
            return procDef;
        }

        // Set the procContext definition (threadContext safe), only set by ChangeProc()
        // Returns the procId of the procContext.
        private int SetProcDef(ProcDefinition procDef)
        {
            this.procDefsLock.AcquireWriterLock(-1);
            try
            {
                this.procDefs[procDef.procId] = procDef;
            }
            finally
            {
                this.procDefsLock.ReleaseWriterLock();
            }
            return procDef.procId;
        }

        // If I want to change a procContext definition, I lock it down so no one else tries to merge changes
        // on it at the same time, make a copy of the persistedValue, apply my changes to the copy, update the
        // version, make it the current one and unlock it. They key is only 1 person is changing it at 
        // a time. It is ok if we let other ppl run while we're changing it.
        public SchedulerMaster.ProcChange ChangeProc(int procId)
        {
            //Console.WriteLine("tring to changeProc("+initNamespaceProcName+")");
            ProcChange pc;
            lock (procChg)
            {
                if (!procChg.ContainsKey(procId)) procChg[procId] = new ProcChange(this, procId);
                pc = procChg[procId];
            }
            pc.Setup();
            return pc;
        }

        // Will be called by workers to get the scheduler they should use.
        public Scheduler GetScheduler()
        {
            Scheduler s = new Scheduler(this);
            return s;
        }

        public void EnsureInitProcsExist(Worker worker, string nameSpace)
        {
            string initProcName = GetInitNamespaceProcName(nameSpace);
            string initTargetProcName = GetInitNamespaceTargetProcName(nameSpace);
            string initProcXmlString = "<proc name='" + initProcName + "'><run_once><call_proc>'" + initTargetProcName + "'</call_proc></run_once></proc>";
            string initTargetProcXmlString = "<proc name='" + initTargetProcName + "'><info switch='log_initialize'>'Initializing namespace [" + nameSpace + "]'</info></proc>";
            EnsureProcExists(worker, initProcName, nameSpace, initProcXmlString);
            EnsureProcExists(worker, initTargetProcName, nameSpace, initTargetProcXmlString);
        }

        // returns true if procContext already existed, false if it didn't and we made it
        private bool EnsureProcExists(Worker worker, string localName, string nameSpace, string procXmlString)
        {
            string procName = nameSpace + "." + localName;
            this.procDefsMapLock.AcquireWriterLock(-1);
            try
            {
                // if we have a value, return it
                if (this.procDefMap.ContainsKey(procName)) return true;
                // Error if we don't have an xml version
                if (xmlProcInfoMap.ContainsKey(procName)) return true;
                XmlElement procElem = MyXml.ParseXmlString(procXmlString).DocumentElement;
                ProcInfo procInfo = new ProcInfo(this,nameSpace, localName, procElem, nameSpace, new ConfigLocator("generated"));
                RegisterProc(procInfo, worker);
                return false;
            }
            finally
            {
                this.procDefsMapLock.ReleaseWriterLock();
            }
        }

        public int GetProcIdForExplicitName(string procName)
        {
            this.procDefsMapLock.AcquireWriterLock(-1);
            try
            {
                // if we have a value, return it
                if (this.procDefMap.ContainsKey(procName)) return this.procDefMap[procName];
                // Error if we don't have an xml version
                if (!xmlProcInfoMap.ContainsKey(procName)) throw new Exception("Cannot find proc with name [" + procName + "]");
                // Load the definition from the info
                ProcInfo procInfo = xmlProcInfoMap[procName];
                ProcDefinition procDef = LoadProcDefFromInfo(procInfo);
                int procId = this.SetProcDef(procDef);
                this.procDefMap[procName] = procId;
                return procId;
            }
            finally
            {
                this.procDefsMapLock.ReleaseWriterLock();
            }
        }

        public string TryResolveProcName(string currentNameSpace, string localName, out string nameSpace, out string procName)
        {
            nameSpace = null;
            procName = null;
            if (!this.procNameSpaceMap.ContainsKey(localName))
            {
                return "Cannot find proc with name [" + localName + "] in any namespace. Fyi current namespace is [" + currentNameSpace + "]";
                //this.procNameSpaceMap.Keys.ForEach(k=>Console.WriteLine("I HAVE "+k));
            }
            var nameSpaceMap = procNameSpaceMap[localName];
            if (nameSpaceMap.ContainsKey(currentNameSpace))
            {
                nameSpace = currentNameSpace;
                procName = nameSpaceMap[currentNameSpace];
            }
            else if (nameSpaceMap.ContainsKey("global"))
            {
                nameSpace = "global";
                procName = nameSpaceMap["global"];
            }
            else if (nameSpaceMap.Keys.Count == 1) // at some point we might want to turn this option off
            {
                nameSpace = nameSpaceMap.GetFirstKey();
                procName = nameSpaceMap.GetFirstValue();
            }
            else
            {
                return "Cannot resolve proc with local name=[" + localName + "] from namespace=[" + currentNameSpace + "]. [" + localName + "] is not defined in [" + currentNameSpace + "] or [global] namespace and there is more than one match in other namespaces: [" + nameSpaceMap.Keys.ToArray().Join(",") + "]. To fix this issue, define the [" + localName + "] in [" + currentNameSpace + "] or call the proc using the using explicite namespace.";
            }
            return null;
        }
        public void ResolveProcName(string currentNameSpace, string localName, out string nameSpace, out string procName)
        {
            string error = TryResolveProcName(currentNameSpace, localName, out nameSpace, out procName);
            if (error != null)
            {
                throw new Exception(error);
            }
        }

        // Returns the procId, resolving namespace as needed.
        public int GetProcId(string currentNameSpace, string localName)
        {
            string nameSpace, procName;
            this.procNameSpaceMapLock.AcquireReaderLock(-1);
            try
            {
                ResolveProcName(currentNameSpace, localName, out nameSpace, out procName);
                return this.GetProcIdForExplicitName(procName);
            }
            finally
            {
                this.procNameSpaceMapLock.ReleaseReaderLock();
            }
        }

        // Given a procId return the namespace for the procContext
        public string GetProcNameSpace(int procId)
        {
            this.procDefsLock.AcquireReaderLock(-1);
            try
            {
                ProcDefinition procDef = this.procDefs[procId];
                return procDef.nameSpace;
            }
            finally
            {
                this.procDefsLock.ReleaseReaderLock();
            }
        }

        public string TryGetProcName(int procId)
        {
            var def = TryGetProcDefinition(procId);
            if (def != null) return def.procName;
            return null;
        }

        public string GetProcName(int procId)
        {
            return GetProcDefinition(procId).procName;
        }

        public ProcType GetWorkType(int procId)
        {
            return GetProcDefinition(procId).procType;
        }

        // loads a procContext from an xml string
        private ProcDefinition LoadProcDefFromInfo(ProcInfo procInfo)
        {
            if (procInfo.procElem.LocalName.Equals("error"))
            {
                string errorMessage = procInfo.procElem.SelectNodeInnerText("error_message");
                string msg = "Error cannot call [" + procInfo.procName + "] in file [" + procInfo.location.GetLocation() + "] because the file failed to parse. Here is the parsing error: " + errorMessage;
                throw new Exception(msg);
            }

            ProcDefinition procDef = new ProcDefinition(procInfo);
            List<IModuleRun> modules = this.mvm.procLoader.ReadXmlModules(procInfo.procElem);
            procDef.AddModules(modules);
            return procDef;
        }

        public string GetInitFileProcName(string fileName)
        {
            return "initialize_file/" + fileName.Replace(".", "_");
        }
        public string GetInitFileTargetProcName(string fileName)
        {
            return "initialize_file_target/" + fileName.Replace(".", "_");
        }
        public string GetInitNamespaceProcName(string nameSpace)
        {
            return "initialize_namespace/" + nameSpace.Replace(".", "_");
        }
        public string GetInitNamespaceTargetProcName(string nameSpace)
        {
            return "initialize_namespace_target/" + nameSpace.Replace(".", "_");
        }
        public void AddInitializeProcCall(ProcInfo procInfo)
        {
            // only add init call if procContext is an entry point
            if (procInfo.isEntryPoint)
            {
                var initNamespaceProcName = GetInitNamespaceProcName(procInfo.nameSpace);
                var runOnce = procInfo.procElem.CreateElement("run_once");
                var callProc = procInfo.procElem.CreateTextElement("call_proc", initNamespaceProcName.q());
                runOnce.AppendChild(callProc);
                procInfo.procElem.PrependChild(runOnce);
            }
            else
            {
            }
        }



        // Loads a procContext into the system. The procContext childElem doesn't need
        // to be <procContext>, it can be any tag containing valid newModules.
        private void RegisterProc(ProcInfo procInfo, Worker worker)
        {
            RegisterProcs(new List<ProcInfo>() { procInfo }, worker);
        }
        public void RegisterProcs(List<ProcInfo> procList, Worker worker)
        {
            // Process globalContext newModules. Make sure we call the globalModule only once per node
            // Global newModule return success of failure. Failure really means that they have a 
            // dependancy that has not yet been fullfilled. We keep on trying as long as we 
            // make forward progress. If we cannot make any forward progress, we error and 
            // print out all the newModules that we cannot resolve. <startup> is going to be
            // a globalContext newModule but we need to first make it so we can call a procContext before
            // that procContext is defined. Right now that is not the case so we special handle
            // it.
            Dictionary<XmlElement, int> processedGlobals = new Dictionary<XmlElement, int>();
        GLOBALS:
            for (; ; )
            {
                Dictionary<XmlElement, string> erroredGlobals = new Dictionary<XmlElement, string>();
                foreach (ProcInfo procInfo in procList)
                {
                    foreach (string globalTag in this.mvm.procLoader.moduleGlobalMap.Keys)
                    {
                        var globalElems =
                             //from childElem in procInfo.procElem.SelectElements("//" + globalTag)
                             from elem in procInfo.procElem.SelectElements(globalTag)
                             where !processedGlobals.ContainsKey(elem)
                             select elem;
                        foreach (var globalElem in globalElems)
                        {
                            IModuleGlobal moduleGlobal = this.mvm.procLoader.moduleGlobalMap[globalTag];
                            string error = moduleGlobal.Global(procInfo, globalElem, this, worker);
                            if (error == null)
                            {
                                processedGlobals[globalElem] = 1;
                                goto GLOBALS; //Do this in case a globalContext owns stuff inside of it and wants to mess with it.
                            }
                            else
                            {
                                erroredGlobals[globalElem] = error;
                            }
                        }
                    }
                }
                if (erroredGlobals.Count() != 0)
                {
                    string msg = "Error, cannot resolve global modules:".AppendLine();
                    foreach (var entry in erroredGlobals)
                    {
                        msg.AppendLine("Cannot process module because: " + entry.Value);
                        msg.AppendLine(entry.Key.OuterXml);
                    }
                    throw new Exception(msg);
                }
                break;
            }

            // strip out the initialize blocks
            Dictionary<string, List<XmlElement>> initNameSpace = new Dictionary<string, List<XmlElement>>();
            foreach (ProcInfo procInfo in procList)
            {
                if (procInfo.localName.Equals("bound_dates_by_account_dates"))
                {
                }
                var initElems=procInfo.procElem.GetElementsByTagName("initialize").ToList();
                foreach (XmlElement initElem in initElems)
                {
                    XmlElement procElem = (XmlElement)initElem.ParentNode.RemoveChild(initElem);
                    string procNameSpace = procElem.GetAttribute("namespace");
                    if (procNameSpace.Equals(""))
                    {
                        procNameSpace = procInfo.nameSpace;
                        procElem.SetAttribute("namespace", procNameSpace);
                    }
                    if (!initNameSpace.ContainsKey(procNameSpace)) initNameSpace[procNameSpace] = new List<XmlElement>();
                    initNameSpace[procNameSpace].Add(procElem);
                }
            }

            // strip out the startup blocks startupOrphans[namespace]=xmlElement
            Dictionary<string, List<XmlElement>> startupOrphans = new Dictionary<string, List<XmlElement>>();
            foreach (ProcInfo procInfo in procList)
            {
                foreach (XmlElement startupElem in procInfo.procElem.SelectNodes("//startup"))
                {
                    XmlElement startupOrphan = (XmlElement)startupElem.ParentNode.RemoveChild(startupElem);
                    string startupNameSpace = startupOrphan.GetAttribute("namespace");
                    if (startupNameSpace.Equals(""))
                    {
                        startupNameSpace = procInfo.nameSpace;
                        startupOrphan.SetAttribute("namespace", startupNameSpace);
                    }
                    if (!startupOrphans.ContainsKey(startupNameSpace)) startupOrphans[startupNameSpace] = new List<XmlElement>();
                    startupOrphans[startupNameSpace].Add(startupOrphan);
                }
            }

            // register the procs
            foreach (ProcInfo procInfo in procList)
            {
                if (xmlProcInfoMap.ContainsKey(procInfo.procName))
                {
                    throw new Exception("Error, more than one version of proc:" + procInfo.procName);
                }
                xmlProcInfoMap[procInfo.procName] = procInfo;

                this.procNameSpaceMapLock.AcquireWriterLock(-1);
                try
                {
                    if (!this.procNameSpaceMap.ContainsKey(procInfo.localName))
                        this.procNameSpaceMap.Add(procInfo.localName, new Dictionary<string, string>());
                    this.procNameSpaceMap[procInfo.localName][procInfo.nameSpace] = procInfo.procName;
                }
                finally
                {
                    this.procNameSpaceMapLock.ReleaseWriterLock();
                }
            }

            // if the namespace procContext does not yet exist, create it.
            foreach (string nameSpace in procList.Select(x => x.nameSpace).Distinct())
            {
                this.EnsureInitProcsExist(worker, nameSpace);
            }
            // push the init newModules into the procContext info for the init procContext.
            List<string> nameSpaces = initNameSpace.Keys.ToList();
            if (nameSpaces.Count > 0)
            {
            }
            foreach (var nameSpace in nameSpaces)
            {
                string initTargetProc = GetInitNamespaceTargetProcName(nameSpace);
                int initTargetProcId = GetProcId(nameSpace, initTargetProc);
                ProcDefinition procDef = GetProcDefinition(initTargetProcId);
                foreach (XmlElement moduleElement in initNameSpace[nameSpace].OrderByDescending(x=>new StringDecimal(x.GetAttributeDefault("priority","0"))))
                {
                    List<IModuleRun> modules = this.mvm.procLoader.ReadXmlModules(moduleElement);
                    string xstring=moduleElement.OuterXml.Replace("'","\"");
                    xstring = xstring.Replace("<", "&lt;");
                    xstring = xstring.Replace(">", "&gt;");
                    //modules.Insert(0,this.GetModuleRun("<info switch='log_initialize'>'DOING INIT:" + xstring + "'</info>"));
                    this.PushAfter(modules, initTargetProcId, new StringDecimal(int.MaxValue));
                }
            }
            // add the run_once call init namespace stub into all the procs
            foreach (ProcInfo procInfo in procList)
            {
                this.AddInitializeProcCall(procInfo);
            }

           

            // run the startup blocks
            // TBD: this is a crappy implementation. It will puke if startup blocks nest (though they shouldn't nest)
            // TBD: it would be better to reuse the same startup object each time.
            // TBD: we don't support newOrder
            // TBD: this uses produce queue procInst to force that the startup blocks gets processed before moving
            //      on to the next newModule. This will break if already in a sync block (which you shouldn't be).
            // TBD: we should do this for all procs, not just startup!!!! same as <when procContext="startup"> 
            foreach (var nameSpace in startupOrphans.Keys)
            {
                string startupClusterObjectId = worker.workMgr.objectCache.CreateAndGetObjectId("STARTUP_OBJECT");
                string startupProcName = "_startup_proc_" + nameSpace + "_" + Interlocked.Increment(ref startupProcCtr);
                string startupProcBody = "";
                foreach (XmlElement elem in startupOrphans[nameSpace]) startupProcBody += elem.InnerXml;
                string startupProc = "<proc name='" + startupProcName + "'>" + startupProcBody + "</proc>";
                this.ReadXmlConfigFromString(startupProc, worker, nameSpace, new ConfigLocator("startup block for " + nameSpace));
                int startupProcId = this.GetProcId(nameSpace, startupProcName);
                ProcInst startupWork = ProcInst.CallProcForObjectId(this.GetScheduler(), startupProcId, startupClusterObjectId);
                worker.ProduceQueueWork(startupWork);
            }
        }
        public static int startupProcCtr = 0;

        #region loadxmlprocs

        // Loads a procContext into the system. The procContext childElem doesn't need
        // to be <procContext>, it can be any tag containing valid newModules.
        public void ReadXmlProcFromElem(string nameSpace, string localName, XmlElement procElem, Worker worker, string structuralNameSpace, IConfigLocator location)
        {
            List<ProcInfo> procList = new List<ProcInfo>();
            procList.Add(new ProcInfo(this,nameSpace, localName, procElem, structuralNameSpace, location));
            this.RegisterProcs(procList, worker);
        }

        // Reads xml config from an xml string
        public void ReadXmlConfigFromString(string configString, Worker worker, string structuralNameSpace, IConfigLocator location)
        {
            List<ProcInfo> procList = new List<ProcInfo>();
            XmlElement docElem = this.mvm.procLoader.ParseInputXmlString(configString);
            foreach (XmlElement procElem in docElem.SelectNodes("//proc"))
            {
                string nameSpace = procElem.GetAttribute("namespace").Nvl(structuralNameSpace);
                string localName = procElem.GetAttribute("name");
                procList.Add(new ProcInfo(this,nameSpace, localName, procElem, structuralNameSpace, location));
            }
            this.RegisterProcs(procList, worker);
        }

        // Parses xml config from xml files.
        // Caller must still call registerProcs
        public List<ProcInfo> ParseXmlConfigFromFiles(string[] xmlFiles, Worker worker, string structuralNameSpace, out int errors)
        {
            errors = 0;
            List<ProcInfo> procList = new List<ProcInfo>();
            foreach (string xmlFile in xmlFiles)
            {
                this.mvm.Log("log_includes", "Load: " + xmlFile);
                XmlElement docElem = this.mvm.procLoader.ParseInputXmlFile(xmlFile);
                // if we got a parse error, register that here.
                if (docElem.LocalName.Equals("error"))
                {
                    this.mvm.Error("Warning: cannot include file [" + xmlFile + "]. Error=[" + docElem.InnerText + "]");
                    foreach (XmlElement procElem in docElem.SelectNodes("//proc"))
                    {
                        string nameSpace = procElem.GetAttribute("namespace").Nvl(structuralNameSpace);
                        string localName = procElem.GetAttribute("name");
                        // if the procContext has errors then they are written to
                        ProcInfo pi = new ProcInfo(this, nameSpace, localName, docElem, structuralNameSpace, new ConfigLocator(xmlFile));
                        mvm.Log("log_includes", "Warning: cannot include proc [" + pi.procName + "] in file [" + pi.location.GetLocation() + "]");
                        procList.Add(pi);
                    }
                    errors++;
                }
                else
                {
                    foreach (XmlElement procElem in docElem.SelectNodes("//proc"))
                    {
                        string nameSpace = procElem.GetAttribute("namespace").Nvl(structuralNameSpace);
                        string localName = procElem.GetAttribute("name");
                        // if the procContext has errors then they are written to
                        procList.Add(new ProcInfo(this, nameSpace, localName, procElem, structuralNameSpace, new ConfigLocator(xmlFile)));
                    }
                }

            }
            return procList;
        }

        // Reads xml config from xml files.
        // Reads in the xml config and maps initNamespaceProcName the xml element of the procContext
        // expecting file with root of 'procContext' or 'procContext/procs'
        public int ReadXmlConfigFromFiles(string[] xmlFiles, Worker worker, string structuralNameSpace)
        {
            int errors = 0;
            List<ProcInfo> procList = this.ParseXmlConfigFromFiles(xmlFiles, worker, structuralNameSpace, out errors);
            this.RegisterProcs(procList, worker);
            return errors;
        }
        #endregion

        // parses a single xml newModule and returns the IModuleRun for it
        public IModuleRun GetModuleRun(XmlElement moduleElem)
        {
            return this.mvm.procLoader.ReadXmlModule(moduleElem);
        }
        // parses a single xml newModule and returns the IModuleRun for it
        public IModuleRun GetModuleRun(string xmlModule)
        {
            XmlElement moduleElem = this.mvm.procLoader.ParseInputXmlString(xmlModule);
            return GetModuleRun(moduleElem);
        }

        // pushes newModules before a procContext
        public void PushBefore(List<IModuleRun> modules, int procId, StringDecimal explicitOrder)
        {
            // Force the procContext to be loaded incase it isn't
            ProcDefinition junk = this.GetProcDefinition(procId);

            // Do the push through our ProcChange technique
            using (ProcChange pc = this.ChangeProc(procId))
            {
                foreach (IModuleRun m in modules)
                {
                    pc.nextProc.PushBefore(m, explicitOrder);
                }
            }
        }

        // pushes newModules after a procContext
        public void PushAfter(List<IModuleRun> modules, int procId, StringDecimal explicitOrder)
        {
            // Force the procContext to be loaded incase it isn't
            ProcDefinition junk = this.GetProcDefinition(procId);

            // Do the push through our ProcChange technique
            using (ProcChange pc = this.ChangeProc(procId))
            {
                foreach (IModuleRun m in modules)
                {
                    pc.nextProc.PushAfter(m, explicitOrder);
                }
            }
        }

    }

}
