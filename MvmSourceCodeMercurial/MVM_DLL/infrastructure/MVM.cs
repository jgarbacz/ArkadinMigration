using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Data;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Linq;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace MVM
{
    public class MvmEngine
    {

        // tbd maybe move this to the cluster
        #region snapshot numbers

        #endregion

        public volatile bool shutdownAbortInProcess = false;

        public void CopySuperTargetLogin()
        {
            using (IObjectData targetLoginObj = this.objectCache.CreateAndGetObject("login_object"))
            {
                this.globalContext["target_login"] = targetLoginObj.objectId;
                string databaseType, databaseServer, databaseName, databaseUser, databasePassword;
                this.GetSuperDbInfo(out databaseType, out databaseServer, out databaseName, out databaseUser, out databasePassword);
                targetLoginObj["database_type"] = databaseType;
                targetLoginObj["database_server"] = databaseServer;
                targetLoginObj["database_name"] = databaseName;
                targetLoginObj["database_user"] = databaseUser;
                targetLoginObj["database_password"] = databasePassword;
                // bump the ref count so it does not ever get GCed. Better to put it in GLOBAL_OBJECTS
                // but we have an issue that GLOBAL_OBJECTS is not yet defined because we have not yet
                // processed the kids.
                var pinOidInMemForever = targetLoginObj.RefGet();
            }
        }

        public void GetSuperDbInfo(out string databaseType, out string databaseServer, out string databaseName, out string databaseUser, out string databasePassword)
        {
            //this.Log("HEY need to request db info from node super");

            // Need to wait for a response so do this through the work manager
            long batchId = this.remoteWorkMgr.CreateBatch();
            WorkBatch batch = this.remoteWorkMgr.LookupBatch(batchId);
            WorkInfo w = this.remoteWorkMgr.CreateWork(batchId);
            w.procName = "GetDbInfo";
            w.nodeId = this.mvmCluster.SuperNode.nodeId;
            w.priority = MessagePriority.Interupt;
            w.status = WorkStatus.WaitingToStart;

            // create and send the message.
            var msg = new DbInfoRequestMessage(w.workId);
            this.Log("sending DbInfoRequestMessage workId=" + msg.workId + " to node_id=" + w.nodeId);
            this.mvmCluster.SuperNode.SocketHandler.messageOutbox.Add(msg);

            // Wait for the batch to complete
            BlockingWaitBatchEvent blockingWait = new BlockingWaitBatchEvent();
            batch.AddBatchCompleteEvent(blockingWait);
            blockingWait.WaitForBatchComplete();

            // Inspect the results...
            string[] dbInfoArr = w.outputs as string[];
            databaseType = dbInfoArr[0];
            databaseServer = dbInfoArr[1];
            databaseName = dbInfoArr[2];
            databaseUser = dbInfoArr[3];
            databasePassword = dbInfoArr[4];
            //this.Log("got DbInfoResponse:" + dbInfoArr.JoinStrings(","));

            // clear the batch
            this.remoteWorkMgr.ClearBatch(batch.batchId);

        }

        public void DumpMemory()
        {
            this.Log("dump_memory(");
            Process proc = Process.GetCurrentProcess();

            int pid = proc.Id;

            this.Log("PrivateMemorySize64=" + proc.PrivateMemorySize64);
            this.Log("VirtualMemorySize64=" + proc.VirtualMemorySize64);
            this.Log("WorkingSet64=" + proc.WorkingSet64);
            this.Log("pid=" + pid);
            this.Log("RemoteWorkWorkCount=" + this.remoteWorkMgr.RemoteWorkWorkCount);
            this.Log("RemoteWorkBatchCount=" + this.remoteWorkMgr.RemoteWorkBatchCount);


            if (this.globalContext.bfs != null)
            {
                var cache = this.globalContext.bfs.bufferedFileCache;
                this.Log("bfs.cache NumItemsInCached=" + cache.NumItemsInCached);
                this.Log("bfs.cache NumItemsSavedState=" + cache.NumItemsSavedState);
                this.Log("bfs.cache NumLockedItems=" + cache.NumLockedItems);
                this.Log("bfs.cache NumUnLockedItems=" + cache.NumUnLockedItems);
            }
            else
            {
                this.Log("bfs not used.");
            }

            this.Log("NUM GLOBAL MONITORS:" + this.globalContext.monitors.Count);
            var globalNamedClassInstMap = this.globalContext.namedClassInstMap.UnsafeGetInnerDictionary();
            this.Log("NUM GLOBAL NAME CLASS INST MAP:" + globalNamedClassInstMap.Count);

            this.Log("TBD: UPDATE THIS FOR NESTED MEM  IDX !");
            foreach (var elem in globalNamedClassInstMap)
            {
                if (elem.Value.GetType().ToString().Equals("MVM.MemoryIndex"))
                {
                    MemoryIndex memIdx = (MemoryIndex)elem.Value;
                    int keyCount = memIdx.index.Count;
                    int valueCount = 0;
                    foreach (var row in memIdx.index.Values) valueCount += row.Count;
                    this.Log("GLOBAL_IDX_NAME=" + elem.Key + ", keys=" + keyCount + ", values=" + valueCount);
                }
                else if (elem.Value.GetType().ToString().Equals("MVM.MemoryIndexSync"))
                {
                    MemoryIndexSync memIdx = (MemoryIndexSync)elem.Value;
                    lock (memIdx.index)
                    {
                        int keyCount = memIdx.index.Count;
                        int valueCount = 0;
                        foreach (var row in memIdx.index.Values) valueCount += row.Count;
                        this.Log("GLOBAL_IDX_NAME=" + elem.Key + ", keys=" + keyCount + ", values=" + valueCount);
                    }
                }
                else
                {
                    this.Log("GLOBAL_NAME=" + elem.Key + ", type=" + elem.Value.GetType());
                }
            }

            this.Log("NUM TOTAL OBJECTS:" + this.objectCache.objects.Count);
            Dictionary<string, int> objTypeCount = new Dictionary<string, int>();
            foreach (var obj in this.objectCache.objects.Values)
            {
                if (!objTypeCount.ContainsKey(obj.objectType)) objTypeCount[obj.objectType] = 0;
                objTypeCount[obj.objectType] += 1;
            }

            foreach (string objectType in objTypeCount.Keys)
            {
                this.Log("NUM OBJECTS OF TYPE:" + objectType + "=" + objTypeCount[objectType]);
            }

            //this.Log("ALL OBJECTS:");
            //foreach (var obj in this.objectCache.objects.Values.OrderBy(obj => obj.objectType))
            //{
            //    this.Log("refctr="+obj.RefCount.ToString()+","+obj.fields.Select(f => f.Key + "=" + f.Value).JoinStrings(","));
            //}

            this.Log(")");
        }


        public void UpdateMvmCounterObject(string counter, string value)
        {
            using (IObjectData ctrObj = this.objectCache.CheckOut(this.globalContext["mvm_counter_object"]))
            {
                ctrObj[counter] = value;
            }
        }


        private StaticDbLoginInfo staticDbLoginInfo = null;
        public StaticDbLoginInfo GetDefaultDbLogin()
        {
            if (this.staticDbLoginInfo != null) return this.staticDbLoginInfo;
            string targetLoginOid = this.globalContext["target_login"];
            if (targetLoginOid.IsNullOrEmpty()) throw new Exception("Error GLOBAL.target_login not set");
            using (IObjectData targetLogin = this.objectCache.CheckOut(targetLoginOid))
            {
                this.staticDbLoginInfo = new StaticDbLoginInfo(targetLogin["database_server"], targetLogin["database_name"], targetLogin["database_user"], targetLogin["database_password"], targetLogin["database_type"]);
            }
            return this.staticDbLoginInfo;
        }

        # region Mvm engine constructor and api
        public readonly DateTime StartDateTime;

        /// <summary>
        /// Returns start datetime as YYYYMMDDHH24MISS
        /// </summary>
        public string StartTimeStamp
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(this.StartDateTime.Year.ToString());
                sb.Append(this.StartDateTime.Month.ToString().PadLeft(2, '0'));
                sb.Append(this.StartDateTime.Day.ToString().PadLeft(2, '0'));
                sb.Append(this.StartDateTime.Hour.ToString().PadLeft(2, '0'));
                sb.Append(this.StartDateTime.Minute.ToString().PadLeft(2, '0'));
                sb.Append(this.StartDateTime.Second.ToString().PadLeft(2, '0'));
                return sb.ToString();
            }
        }

        public int ThreadHash
        {
            get
            {
                return System.Threading.Thread.CurrentThread.GetHashCode();
            }
        }
        public string MvmUniqueId
        {
            get
            {
                return this.StartTimeStamp + "_" + MvmEngine.ProcessId.ToString() + '_' + this.ThreadHash.ToString();
            }
        }



        public readonly ObjectDataSerializer objectDataSerializer;

        public readonly ProcLoader procLoader;
        public readonly MvmDoc mvmDoc;
        // Constructor
        public MvmEngine()
        {
            StartDateTime = DateTime.Now;
            if (Thread.CurrentThread.Name == null)
            {
                Thread.CurrentThread.Name = "MVM";
            }
            this.procLoader = new ProcLoader(this);
            this.mvmDoc = new MvmDoc(this);
            LoadModules();
            this.workMgr = new WorkMgr(this);
            this.remoteWorkMgr = new RemoteWorkManager(this);
            this.objectDataSerializer = new ObjectDataSerializer(this);
        }

        public string metraTechDir = null;

        public string rmpDir
        {
            get
            {
                return MvmClusterCommon.rmpDir;
            }
        }

        public string rmpBinDir
        {
            get
            {
                return MvmClusterCommon.rmpBinDir;
            }
        }

        public string rmpLogDir
        {
            get
            {
                return MvmClusterCommon.rmpLogDir;
            }
        }

        // Returns the full path of the nlog config file
        public string nlogConfigFile(string basefile)
        {
            string file = this.globalContext["nlog_config"];
            if (file.NotNullOrEmpty() && File.Exists(file))
            {
                return file;
            }
            file = Path.Combine(rmpDir, "config", "logging", "NLog", basefile);
            if (File.Exists(file))
                return file;
            return Path.Combine(this.rmpBinDir, basefile);
        }

        public string mvmExecutableDir
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            }
        }
        public string documentationDir
        {
            get
            {
                DirectoryInfo mtdir = new DirectoryInfo(rmpDir).Parent;
                string[] dirs = { mtdir.FullName, "Source", "MetraTech", "Mvm", "doc" };
                return string.Join(Path.DirectorySeparatorChar.ToString(), dirs);
            }
        }
        public string extensionsDir
        {
            get
            {
                string extDir = rmpDir + Path.DirectorySeparatorChar + "extensions";
                string overrideExtDir = this.workMgr.globalContext["override_extensions_dir"];
                if (overrideExtDir.NotNullOrEmpty())
                {
                    extDir = overrideExtDir;
                }
                DirectoryInfo dir = new DirectoryInfo(extDir);
                dir.CreateIfNotThere();
                return dir.ToString();
            }
        }
        public string utilitiesGlob
        {
            get
            {
                return Path.Combine(rmpDir, "extensions", "*", "MvmConfig", "utilities");
            }
        }

        private string _metraTechMvmGeneratedDir = null;
        public string metraTechMvmGeneratedDir
        {
            get
            {
                if (_metraTechMvmGeneratedDir != null) 
                    return _metraTechMvmGeneratedDir;
                DirectoryInfo dir = new DirectoryInfo(rmpDir + Path.DirectorySeparatorChar + "MvmGenerated");
                dir.CreateIfNotThere();
                return this._metraTechMvmGeneratedDir=dir.ToString();
            }
        }

        private RmpSearcher rmpSearcherInst;
        public RmpSearcher rmpSearcher
        {
            get
            {
                if (this.rmpSearcherInst == null)
                {
                    this.rmpSearcherInst = new RmpSearcher(this.rmpDir);
                }
                return this.rmpSearcherInst;
            }
        }

        public GlobalContext globalContext
        {
            get
            {
                return this.workMgr.globalContext;
            }
        }
        public ObjectCache objectCache
        {
            get
            {
                return this.workMgr.objectCache;
            }
        }

        /// <summary>
        /// On development systems, metratechDir is c:/dev/XXX.
        /// On deployed systems, metratechDir is c:/metratech
        /// TBD THIS NEEDS TO BE FIXED
        /// </summary>
        /// <returns></returns>
        public string GetMetraTechDir()
        {
            throw new Exception("no need for this");

            //string assemblyFile = Assembly.GetExecutingAssembly().Location;
            //DirectoryInfo di = new FileInfo(assemblyFile).Directory;
            //while (di != null)
            //{
            //    // if the direcotry contains RMP, then we are on the metratech dir
            //    if (di.GetDirectories().Where(d=>d.Name.EqualsIgnoreCase("RMP")).Any()) 
            //        return di.FullName;
            //    // otherwise, go up one an try again.
            //    di = di.Parent;
            //}
            //throw new Exception("Error, cannot figure out where the metratech dir is located.");
        }

        // searches up in the file path for the extension the passed file is under
        public static string GetExtensionDir(string filePath)
        {
            DirectoryInfo di = new FileInfo(filePath).Directory;
            DirectoryInfo pdi = di.Parent;
            while (pdi != null)
            {
                if (pdi.Name.EqualsIgnoreCase("extensions")) return di.FullName;
                di = pdi;
                pdi = pdi.Parent;
            }
            return null;
        }

        // Map of filename to raw contents of that file (used when pushing code to remote listeners)
        public bool addXmlHashes = false;
        public Dictionary<string, XmlResourceInfo> XmlFileHashMap = new Dictionary<string, XmlResourceInfo>();
        public SHA1 FileHasher = new SHA1CryptoServiceProvider();
        public void AddFileHash(string file, string contents)
        {
            this.XmlFileHashMap[file] = new XmlResourceInfo(this.rmpDir, file, Encoding.UTF8.GetBytes(contents));
        }
        public void AddStringHash(string contents)
        {
            XmlResourceInfo r = new XmlResourceInfo(this.rmpDir, null, Encoding.UTF8.GetBytes(contents));
            this.XmlFileHashMap[r.remoteFilename] = r;
        }

        public void LoadRmpProcs()
        {
            SchedulerMaster sm = this.workMgr.schedulerMaster;
            List<ProcInfo> procs = new List<ProcInfo>();
            foreach (var structuralNameSpace in MvmClusterCommon.EnumerateRMPDirectories(extensionsDir))
            {
                int errors = 0;
                string[] xmlFiles = MvmClusterCommon.GetRMPFiles(structuralNameSpace[1]);
                Log("log_includes", "Including directory: " + structuralNameSpace[1]);
                sm.SetNameSpaceDir(structuralNameSpace[0], structuralNameSpace[1]);
                procs.AddRange(sm.ParseXmlConfigFromFiles(xmlFiles, workMgr.defaultWorker, structuralNameSpace[0], out errors));
            }
            Log("log_includes", "Registering procs...");
            sm.RegisterProcs(procs, workMgr.defaultWorker);
        }

        private bool haveLoggers = false;
        public static Logger nlogger = LogManager.GetCurrentClassLogger();
        public void UseNLogger(string nlogConfig)
        {
            if (nlogConfig.IsNullOrEmpty())
                nlogConfig = Path.Combine(this.rmpBinDir, "NLog.config");
            SetupNLog(nlogConfig);
            haveLoggers = true;
        }

        // Default log level for legacy callers is info
        public void Log(string msg)
        {
            Log(LogLevel.Info, msg);
        }
        public void Log(LogLevel lvl, string msg)
        {
            if (!this.haveLoggers)
            {
                Console.WriteLine(msg);
            }
            else
            {
                nlogger.Log(lvl, msg);
            }
        }


        public void LogFull(string msg, string classname)
        {
            Log("[" + classname + "]" + msg);
        }
        public void Log(string globalSwitch, string msg)
        {
            if (!globalSwitch.IsNullOrEmpty())
            {
                if (!this.workMgr.globalContext[globalSwitch].Equals("1")) return;
            }
            Log(msg);
        }
        public void Fatal(string msg)
        {
            Log(LogLevel.Fatal, msg);
        }
        public void Error(string msg)
        {
            Log(LogLevel.Error, msg);
        }
        public void Warn(string msg)
        {
            Log(LogLevel.Warn, msg);
        }
        public void Info(string msg)
        {
            Log(LogLevel.Info, msg);
        }
        public void Debug(string msg)
        {
            Log(LogLevel.Debug, msg);
        }
        public void Trace(string msg)
        {
            Log(LogLevel.Trace, msg);
        }

        public int NumWorkerThreads
        {
            get
            {
                return this.workMgr.numWorkers;
            }
            set
            {
                this.workMgr.numWorkers = value;
            }
        }
        private bool serverMode = false;
        public void SetServerModeOn()
        {
            if (this.serverMode == false)
            {
                Interlocked.Increment(ref this.workMgr.serviceCount);
                this.serverMode = true;
            }
        }
        public void SetServerModeOff()
        {
            if (this.serverMode == true)
            {
                Interlocked.Decrement(ref this.workMgr.serviceCount);
                this.serverMode = false;
            }
        }
        public void SetGlobalField(string fieldName, string fieldValue)
        {
            this.workMgr.globalContext[fieldName] = fieldValue;
        }
        public readonly RemoteWorkManager remoteWorkMgr;
        public readonly WorkMgr workMgr;
        public IDictionary<string, string> globalValues = new Dictionary<string, string>();
        public void LoadXmlFileConfig(List<string> xmlFileConfig, string structuralNameSpace)
        {
            workMgr.schedulerMaster.ReadXmlConfigFromFiles(xmlFileConfig.ToArray(), workMgr.GetDefaultWorker(), structuralNameSpace);
        }
        public void LoadXmlStringConfig(List<string> xmlStringConfig, string structuralNameSpace, IConfigLocator locator)
        {
            if (xmlStringConfig != null)
            {
                foreach (string xmlString in xmlStringConfig)
                {
                    workMgr.schedulerMaster.ReadXmlConfigFromString(xmlString, workMgr.GetDefaultWorker(), structuralNameSpace, locator);
                }
            }
        }


        public void ReapWorkerThreads()
        {
            this.workMgr.ReapWorkerThreads();
        }

        /// <summary>
        /// Start running.
        /// </summary>
        public void StartupWorkerThreads()
        {
            this.Log("starting worker threads");
            this.workMgr.StartupWorkerThreads();
            this.Log("done starting worker threads");
        }

        /// <summary>
        /// Calls a procContext with no arguments and returns when procContext is complete.
        /// </summary>
        /// <param name="initNamespaceProcName"></param>
        /// <returns></returns>
        public string CallProcSynchronous(string procName)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            return this.CallProcSynchronous(procName, parameters);
        }

        /// <summary>
        /// Calls a procContext and returns when the procContext has completed
        /// </summary>
        /// <param name="initNamespaceProcName"></param>
        public string CallProcSynchronous(string procName, Dictionary<string, string> parameters)
        {
            WorkMgr workMgr = this.workMgr;
            SchedulerMaster schedulerMaster = this.workMgr.schedulerMaster;
            int procId = schedulerMaster.GetProcId("global", procName);
            int workCompleteProcId = schedulerMaster.GetProcId("global", "wcf_service_complete");
            string startClusterObjectId = null;
            try
            {
                // create the starting procInst 
                startClusterObjectId = workMgr.objectCache.CreateAndGetObjectId("STARTING_OBJ");
                ProcInst startingWork = new ProcInst(schedulerMaster.GetProcInfo(procId), startClusterObjectId);
                startingWork.nextModuleOrder = schedulerMaster.GetScheduler().GetProcDefinition(startingWork.procId).GetFirstOrder();
                startingWork.procContext = new ProcContext();

                // set the procContext inputs
                var tempContext = startingWork.procContext.tempContext;
                foreach (var elem in parameters)
                {
                    tempContext[elem.Key] = elem.Value;
                }

                // set the name of the monitor on the cluster  
                // wcf_service_complete_monitor
                string monitorName = "wcf_service_complete_monitor_" + startClusterObjectId;
                using (IObjectData obj = workMgr.objectCache.CheckOut(startClusterObjectId))
                {
                    obj["wcf_service_complete_monitor"] = monitorName;
                }

                // create procNameSyntax procInst that is called when the service procContext is complete 
                ProcInst markCompleteWork = new ProcInst(schedulerMaster.GetProcInfo(workCompleteProcId), startClusterObjectId);
                markCompleteWork.procContext = startingWork.procContext;
                long markCompleteCallbackId = workMgr.CreateCallback(markCompleteWork);
                // make our procContext call the service complete procContext when done
                startingWork.callbackId = markCompleteCallbackId;

                // Lock the monitor.
                if (!workMgr.globalContext.EnterMonitor(monitorName, 10))
                {
                    throw new Exception("unexpected,cannot grab wcf_service_complete_monitor");
                }

                // Queue up the procInst we need to process
                workMgr.ProduceStackWork(startingWork);

                // When the procInst is complete, the procNameSyntax will unlock us and we will
                // be able get the monitor again.
                workMgr.globalContext.EnterMonitor(monitorName, 10);

                // RemoveSpecificItem the monitor now that we no longer need it.
                workMgr.globalContext.RemoveMonitor(monitorName);

                // now we are safe to grab the outputs
                foreach (var elem in tempContext.fields)
                {
                    parameters[elem.Key] = elem.Value;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                //if (startClusterObjectId != null)
                //{
                //    // no matter what happens try to RemoveSpecificItem the cluster and all the 
                //    // objects that were created.
                //    using (Cluster cluster = workMgr.clusterCache.CheckOut(startClusterObjectId))
                //    {
                //        foreach (string memberOid in cluster.objectIds.Keys)
                //        {
                //            workMgr.objectCache.RemoveObjectData(memberOid);
                //        }
                //    }
                //    workMgr.clusterCache.RemoveCluster(startClusterObjectId);
                //}
            }
            return "success";
        }

        #endregion

        #region Private internal stuff
        private void LoadCommandlineXmlFile(string param_file, string param_namespace)
        {
            if (!param_file.IsNullOrEmpty())
            {
                string fileName = param_file;
                string structuralNameSpace = param_namespace.Nvl("global");
                var pwd = Directory.GetCurrentDirectory();
                foreach (var pattern in fileName.Split(';'))
                {
                    this.LoadXmlFileConfig(FileUtils2.GlobToList(pattern), structuralNameSpace);
                }
            }
        }
        private void LoadCommandlineXml(string param_xml, string param_namespace, string param_xml_newline)
        {
            if (!param_xml.IsNullOrEmpty())
            {
                string xmlString = param_xml;
                string structuralNameSpace = !param_namespace.IsNullOrEmpty() ? param_namespace : "global";
                if (!param_xml_newline.IsNullOrEmpty())
                {
                    string origNewline = param_xml_newline;
                    string newline = "\r\n";
                    xmlString = xmlString.Replace(origNewline, newline);
                }
                this.LoadXmlStringConfig(new string[] { xmlString }.ToList(), structuralNameSpace, new ConfigLocator("commandline arguments"));
            }
        }
        private void LoadEmbeddedProcs()
        {
            // If user passed xml on the commandline tell mvm about it
            // if the executing assembly has embedded procs, load them all
            var entryAssembly = Assembly.GetEntryAssembly();
            foreach (var resourceName in entryAssembly.GetManifestResourceNames())
            {
                //Console.WriteLine("Checking:" + resourceName);
                if (resourceName.matches(@".*\.procs\..*\.xml"))
                {
                    Console.WriteLine("Loading:" + resourceName);
                    Stream strm = entryAssembly.GetManifestResourceStream(resourceName);
                    StreamReader reader = new StreamReader(strm);
                    string xmlString = reader.ReadToEnd();
                    this.workMgr.schedulerMaster.ReadXmlConfigFromString(xmlString, this.workMgr.GetDefaultWorker(), "global", new ConfigLocator("EMBEDDED:" + resourceName));
                }
            }
        }


        /**
         * Need to organize newModules by folder
         * Need direct access to a newModule
         * Modules need to know what folders they are in (so they can be in more than one)
         * moduleMetaData{module_name}=metadata
         * how do i create a folder view?
         * can use xml? 
         * 
         * how do deal w/ having one class implementing more than one newModule?
         */
        public class ModuleMetaData
        {
            XmlElement moduleAttrElem;
            public ModuleMetaData(XmlElement moduleAttrElem)
            {
                this.moduleAttrElem = moduleAttrElem;
            }
        }

        // Inspects the assembly and loads module data from Module() attribute metadata
        public void LoadModules()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                // Only check objects in namespace MVM, to avoid probing lazy-loaded external assemblies that might not even be present
                if (type.IsClass && type.Namespace != null && type.Namespace.Equals("MVM"))
                {
                    object[] attrs = new object[0];
                    try
                    {
                        // Ignore errors from GetCustomAttributes(), they generally mean an optional assembly isn't present
                        attrs = type.GetCustomAttributes(true);
                    }
                    catch
                    {
                    }
                    foreach (var attribute in attrs)
                    {
                        if (attribute is ModuleAttribute)
                        {
                            var defaultConstructor = type.GetConstructor(System.Type.EmptyTypes);
                            ((ModuleAttribute)attribute).Register(this, defaultConstructor, type);
                        }
                    }
                }
            }
            this.procLoader.LoadXSD();

            // No better place to put this at the moment
            if (ModuleAttribute.moduleAttributeLookup["MStartProfiler"].defaults.ContainsKey("sampling_period"))
            {
                MvmClusterProfiler.DefaultSamplingPeriod = ModuleAttribute.moduleAttributeLookup["MStartProfiler"].defaults["sampling_period"].ToInt();
            }
            if (ModuleAttribute.moduleAttributeLookup["MStartProfiler"].defaults.ContainsKey("reporting_count"))
            {
                MvmClusterProfiler.DefaultReportingCount = ModuleAttribute.moduleAttributeLookup["MStartProfiler"].defaults["reporting_count"].ToInt();
            }
        }



        #endregion

        #region Running from console

        // writes out the mercurial info we generate in the pre-build event.
        public static void PrintBuildInfo()
        {
            Assembly assbly = Assembly.GetExecutingAssembly();
            string resourceName = "MVM.version.txt";
            Stream strm = assbly.GetManifestResourceStream(resourceName);
            StreamReader reader = new StreamReader(strm);
            Console.Write("VERSION: " + reader.ReadToEnd());
        }

        public static void SetupNLog(string file)
        {
            // Consider using a "local" customization of the config file
            if (File.Exists(file + ".local"))
            {
                file = file + ".local";
            }
            try
            {
                NLog.Config.XmlLoggingConfiguration xml = new NLog.Config.XmlLoggingConfiguration(file);
                NLog.LogManager.Configuration = xml;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static readonly int ProcessId = Process.GetCurrentProcess().Id;
        public static readonly string MachineName = System.Environment.MachineName;

        public MvmClusterBase mvmCluster = null;
        public MvmClusterSuper mvmClusterSuper
        {
            get
            {
                return this.mvmCluster as MvmClusterSuper;
            }
        }

        public MvmCluster mvmClusterSlave
        {
            get
            {
                return this.mvmCluster as MvmCluster;
            }
        }

        public MvmClusterProfiler mvmClusterProfiler
        {
            get
            {
                return this.mvmCluster as MvmClusterProfiler;
            }
        }

        /// <summary>
        /// Super node is the node at the top of the mvm hierarchy. It the first process in the cluster to 
        /// start up.
        /// </summary>
        public bool IsSuperNode { get; private set; }

        public bool IsListening
        {
            get
            {
                return listener != null;
            }
        }

        public SlaveListener listener;

        #region mvm slave
        private Boolean IsSlave = false;
        public int nodeId = -1;
        private int superId = -1;

        private string _mvmObjectPrefix;
        public string mvmObjectPrefix{
            get{
                if(_mvmObjectPrefix!=null) return _mvmObjectPrefix;
                string mvmRunId=this.globalContext["mvm_run_id"];
                string tmpMvmObjectPrefix=mvmRunId+":"+this.nodeId+":";
                if(mvmRunId.NotNullOrEmpty()){
                    _mvmObjectPrefix=tmpMvmObjectPrefix;
                }
                return tmpMvmObjectPrefix;
            }
        }

        public static MvmEngine DefaultMvmEngine;

        public static readonly string ExecutableFullName = TryGetExecutableFullName();
        public static readonly string ExecutableName = TryGetExecutableName();
        public static readonly string ExecutableDir = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName;
        public static string TryGetExecutableFullName()
        {
            try
            {
                return System.Reflection.Assembly.GetEntryAssembly().Location;
            }
            catch
            {
                return Path.Combine(new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName, "mvm.exe");
            }
        }
        public static string TryGetExecutableName()
        {
            try
            {
                return new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location).Name;
            }
            catch
            {
                return "mvm.exe";
            }
        }

        public void LogIdentifyingInfo()
        {
            string info = "NODE INFO: " +
                "node_id=[" + this.nodeId + "] " +
                "node_port=[" + this.mvmCluster.Port.ToString() + "] " +
                "server=[" + MachineName + "] " +
                "pid=[" + ProcessId + "] " +
                "exe=[" + ExecutableFullName + "] " +
                "is64bit=[" + (IntPtr.Size == 8) + "] " +
                "master_id=[" + this.globalContext["master_id"] + "] " +
                "super_id=[" + this.mvmCluster.SuperNode.nodeIdStr + "] " +
                "super_machine=[" + this.mvmCluster.SuperNode.machine + "] " +
                "super_port=[" + this.mvmCluster.SuperNode.portStr + "] " +
                "mvm_run_id=[" + this.globalContext["mvm_run_id"] + "] " +
                "mvm_startup_date=[" + this.globalContext["mvm_startup_date"] + "] " +
                "nlog=[" + this.nlogConfigFile("NLog.slave.config") + "] "
                ;
            this.Warn(info);
        }

        /// <summary>
        /// Sets up the mvm to be a master.
        /// </summary>
        public void SetupAsSuper(bool startupProfilerNode)
        {
            MvmEngine.DefaultMvmEngine = this;
            this.nodeId = 0;
            this.IsSuperNode = true;
            System.Environment.SetEnvironmentVariable("node_id", this.nodeId.ToString());
            this.UseNLogger(this.nlogConfigFile("NLog.config"));
            this.mvmCluster = new MvmClusterSuper(this, this.nodeId, startupProfilerNode);
            this.workMgr.globalContext["node_id"] = this.nodeId.ToString();
            this.workMgr.globalContext["master_id"] = "";
            this.workMgr.globalContext["slave_id"] = "";
            this.workMgr.globalContext["super_id"] = "";

            // kill any other mvms that might be running on this machine
            // actually disable this, so that we can run while AMP et. al. are running
            //var currProcess = System.Diagnostics.Process.GetCurrentProcess();
            //var processName = ExecutableName.Replace(".exe", "");
            //var processList = System.Diagnostics.Process.GetProcessesByName(processName);
            //foreach (var p in processList)
            //{
            //    if (p.Id != currProcess.Id)
            //    {
            //        try
            //        {
            //            p.Kill();
            //            this.Log("Kill prexisting mvm process " + p.Id + " " + p.ProcessName + " worked");
            //        }
            //        catch (Exception e)
            //        {
            //            this.Log("Kill prexisting mvm process " + p.Id + " " + p.ProcessName + " failed:" + e.Message);
            //        }
            //    }
            //}
        }

        public void SetupAsSlave()
        {
            MvmEngine.DefaultMvmEngine = this;
            int slaveId = this.workMgr.globalContext["slave_id"].ToInt();
            this.nodeId = slaveId;
            System.Environment.SetEnvironmentVariable("node_id", this.nodeId.ToString());
            string nlog = Path.Combine(this.nlogConfigFile(this.workMgr.globalContext["nlog"]));
            this.UseNLogger(nlog);
            this.Log("slave logging is up");
            int superId = this.workMgr.globalContext["super_id"].ToInt();
            string superMachine = this.workMgr.globalContext["super_machine"];
            int superPort = this.workMgr.globalContext["super_port"].ToInt();
            string slaveMachine = this.workMgr.globalContext["slave_machine"];
            this.superId = superId;
            this.workMgr.globalContext["super_id"] = this.superId.ToString();
            this.workMgr.globalContext["node_id"] = this.nodeId.ToString();
            this.workMgr.globalContext["slave_id"] = "";
            //this.Log("before new MvmCluster");
            this.mvmCluster = new MvmCluster(this, this.nodeId);
            (this.mvmCluster as MvmCluster).SetupSlave(superId, superMachine, superPort, slaveId, slaveMachine);
            //this.Log("after mvmCluster.SetupSlave()"); 
            this.SetServerModeOn();
            //this.Log("after SetServerModeOn"); 
            this.IsSlave = true;
            this.CopySuperTargetLogin();
            //this.Log("slave setup complete");
        }

        public void BootstrapProfiler(Arguments args)
        {
            // TODO: this is ugly, can we get the filename from the NLog config?
            string logfile = this.rmpLogDir + Path.DirectorySeparatorChar + "MVM_LOG.0.txt";
            try
            {
                using (StreamReader sr = new StreamReader(logfile))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains("NODE INFO:")) {
                            Regex regex = new Regex(@"(?<=\s+(\w+)=\[)(.*?)(?=\])");
                            foreach (Match match in regex.Matches(line))
                            {
                                string variable = match.Groups[1].ToString();
                                if (variable.Equals("master_id"))
                                {
                                    args.Add("master_id", match.Groups[2].ToString());
                                }
                                else if (variable.Equals("node_id"))
                                {
                                    args.Add("super_id", match.Groups[2].ToString());
                                }
                                else if (variable.Equals("server"))
                                {
                                    args.Add("super_machine", match.Groups[2].ToString());
                                }
                                else if (variable.Equals("node_port"))
                                {
                                    args.Add("super_port", match.Groups[2].ToString());
                                }
                                else if (variable.Equals("mvm_run_id"))
                                {
                                    args.Add("mvm_run_id", match.Groups[2].ToString());
                                }
                                else if (variable.Equals("mvm_startup_date"))
                                {
                                    args.Add("mvm_startup_date", match.Groups[2].ToString());
                                }
                                else if (variable.Equals("nlog"))
                                {
                                    args.Add("nlog", match.Groups[2].ToString());
                                }
                            }
                            args.Add("profiler_machine", MachineName);
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Could not read supernode info from logfile [" + logfile + "]: " + e.Message);
            }
        }

        public void SetupAsProfiler(bool attach)
        {
            int nodeId = -1;
            if (!attach)
            {
                nodeId = this.workMgr.globalContext["profiler_id"].ToInt();
                this.nodeId = nodeId;
                this.workMgr.globalContext["node_id"] = nodeId.ToString();
            }
            MvmEngine.DefaultMvmEngine = this;
            System.Environment.SetEnvironmentVariable("node_id", "profiler");  // give nlog a distinct filename
            this.UseNLogger(this.nlogConfigFile("NLog.config"));
            this.Log("profiler logging is up");
            this.superId = this.workMgr.globalContext["super_id"].ToInt();
            string superMachine = this.workMgr.globalContext["super_machine"];
            int superPort = this.workMgr.globalContext["super_port"].ToInt();
            string profilerMachine = this.workMgr.globalContext["profiler_machine"];
            this.workMgr.globalContext["super_id"] = this.superId.ToString();
            this.mvmCluster = new MvmClusterProfiler(this, nodeId);
            (this.mvmCluster as MvmClusterProfiler).SetupProfiler(this, superId, superMachine, superPort, nodeId, profilerMachine, attach);
            this.SetServerModeOn();
            this.CopySuperTargetLogin();
        }

        public void FlushNLog()
        {
            //this.Log("flushing NLog");
            NLog.LogManager.Flush();
            
        }

        private string _logFileName = null;
        public string GetLogFileName()
        {
            // This is the only way I could figure out how to programatically find the log file name. I didn't want to assume
            // that this is up to date with the naming convention configured in nlog.config. This does rely on having a target 
            // called my_inner_file.
            if (_logFileName == null)
            {
                FileTarget tgt = (FileTarget)NLog.LogManager.Configuration.FindTargetByName("my_inner_file");
                // only rendered in a private method so use reflection to read it :(
                Dictionary<string, DateTime> dic = (Dictionary<string, DateTime>)typeof(FileTarget).InvokeMember("initializedFiles", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic,
         null, tgt, null);
                _logFileName = dic.Keys.First();
            }
            return _logFileName;
        }


        // Signals a shutdown abort from any node.  Must always call this method when aborting,
        // regardless of whether the error comes from a slave or the super.
        public void ShutdownAbort(string reason, params int[] skipNodeIds)
        {
            this.shutdownAbortInProcess = true;
            if (this.mvmClusterSuper != null)
            {
                this.mvmClusterSuper.ShutdownAbort(reason, skipNodeIds);
            }
            else
            {
                this.mvmCluster.SuperNode.ShutdownAbort(reason);
            }
            this.FlushNLog();
            this.ShutdownSlave();
        }

        private object _SlaveLogMessage = new object();
        private bool slaveLoggingOn = true;
        /// <summary>
        /// Mvm slave logger calls this to send log messages up to master. This has to be a static
        /// method because i do not know how else to call it from the target.
        /// </summary>
        /// <param name="message"></param>
        public void SlaveLogMessage(string message)
        {
            lock (_SlaveLogMessage)
            {
                if (!slaveLoggingOn) return;
                if (IsSlave)
                {
                    // Currently the profiler node can't log to its super, might need to add this eventually
                    if (this.mvmCluster.SuperNode != null)
                    {
                        LogMessage msg = new LogMessage(message);
                        this.mvmCluster.SuperNode.SocketHandler.messageOutbox.Add(msg, MessagePriority.Log);
                    }
                }
            }
        }
        public void DisableSlaveLogging()
        {
            lock (_SlaveLogMessage)
            {
                slaveLoggingOn = false;
            }
        }



        #endregion

        #endregion

        # region Mvm API
        private Arguments ProcessCommandlineArgs(string[] args)
        {
            // Set globals to the commandline args
            Arguments myArgs = new Arguments(args);

            // handle user passed params file which should be same format 
            // as command line but can have new lines.
            if (myArgs.Parameters.ContainsKey("params"))
            {
                string paramsFile = myArgs["params"];
                string[] paramsFileLines = File.ReadAllLines(paramsFile);
                foreach (var s in paramsFileLines)
                {
                    var ss = s.Trim();
                    if (ss.StartsWith("#")) continue;
                    var parts = ss.Split(new char[] { '=' }, 2);
                    if (parts.Length == 2)
                    {
                        var k = parts[0].Trim();
                        if (k.StartsWith("-")) k = k.Substring(1);
                        var v = parts[1].Trim();
                        myArgs.Add(k, v);
                    }
                }
                myArgs.Remove("params");
            }

            // print usage if requested
            if (myArgs.Parameters.ContainsKey("h") || myArgs.Parameters.ContainsKey("help"))
            {
                PrintUsage();
                Environment.Exit(0);
            }
            if (myArgs.Parameters.ContainsKey("v") || myArgs.Parameters.ContainsKey("version"))
            {
                PrintBuildInfo();
                Environment.Exit(0);
            }

            if (myArgs.Parameters.ContainsKey("f"))
            {
                string functionName = myArgs["f"].Equals("true") ? "" : myArgs["f"];
                Console.WriteLine(this.mvmDoc.GetFunctions(functionName));
                Environment.Exit(0);
            }

            if (myArgs.Parameters.ContainsKey("m"))
            {
                string moduleName = myArgs["m"].Equals("true") ? "" : myArgs["m"];
                Console.WriteLine(this.mvmDoc.GetModules(moduleName));
                Environment.Exit(0);
            }

            return myArgs;
        }

        // Some argument processing needs to happen after we've loaded procs, etc
        private void ProcessCommandlineArgs2(Arguments myArgs)
        {
            if (myArgs.Parameters.ContainsKey("p"))
            {
                string procName = myArgs["p"].Equals("true") ? "" : myArgs["p"];
                Console.WriteLine(this.mvmDoc.GetProcs(procName));
                Environment.Exit(0);
            }

            if (myArgs.Parameters.ContainsKey("doc"))
            {
                string docFile = myArgs["doc"].Equals("true") ? "" : myArgs["doc"];
                this.mvmDoc.GenerateXmlDoc(docFile);
                Environment.Exit(0);
            }
        }

        public static void PrintUsage()
        {
            Console.WriteLine("usage: " + MvmEngine.ExecutableName + " <arguments>");
            Console.WriteLine("-h\t\tPrints this usage message");
            Console.WriteLine("-f[=function]\tPrints function names, or documentation for a specific function");
            Console.WriteLine("-m[=module]\tPrints module names, or documentation for a specific module");
            Console.WriteLine("-p[=proc]\tPrints proc names, or documentation for a specific proc");
            //Console.WriteLine("-doc\t\tWrites MVM documentation files");
            Console.WriteLine("-s\t\tStandalone mode (skip loading configuration in RMP\\Extensions)");
            Console.WriteLine("-file=fname\tSemicolon-separated list of file glob pattern(s) to load");
            Console.WriteLine("-proc=pname\tProc to execute, default: 'main'");
            Console.WriteLine("-xml='text'\tXML text to load");
            Console.WriteLine("-params=file\tPath to parameters file");
            Console.WriteLine("-cluster=name\tName of mvm cluster to start (from NetMeter.mvm_clusters table)");
            Console.WriteLine("-profile\tStart profiler process to analyze performance");
            Console.WriteLine("-x=42\t\tAll other switches written to GLOBAL vars (e.g. GLOBAL.x = 42)");
            //Thread.Sleep(10000);
            return;
        }

        /// <summary>
        /// First checks for a corresponding utility to run; if not found, copies the arguments into GLOBAL.fields and sets the 
        /// appropriate mvm parameter if relevant.
        /// </summary>
        /// <param name="args"></param>
        private void SetGlobalParameters(Arguments args)
        {
            if (args.ParameterList.Count > 0)
            {
                string firstArg = args.ParameterList[0];
                foreach (var filename in FileUtils2.GlobToList(Path.Combine(utilitiesGlob, firstArg + ".xml")))
                {
                    this.isStandalone = true;
                    this.LoadXmlFileConfig(new List<string>() { filename }, "global");
                    args.Add("proc", firstArg);
                    break;  // just take the first one
                }
            }
            foreach (string k in args.Parameters.Keys)
            {
                var v = args.Parameters[k];
                this.SetGlobalParameter(k, v);
            }
        }
        public void SetGlobalParameter(string paramName, string paramValue)
        {
            this.SetGlobalField(paramName, paramValue);
            switch (paramName)
            {
                case "parallel":
                    {
                        this.NumWorkerThreads = paramValue.ToInt();
                        break;
                    }
            }
        }

        public bool isStandalone = false;
        public int exitCode = 0;

        /// <summary>
        /// Runs the mvm from the console
        /// </summary>
        /// <param name="args"></param>
        public static void RunFromConsole(string[] args)
        {
            bool inIde = false;
            MvmEngine mvm = null;
            try
            {
                mvm = new MvmEngine();
                Arguments myArgs = mvm.ProcessCommandlineArgs(args);
                inIde = myArgs.Parameters.ContainsKey("ide");
                bool isProfiler = false;
                bool attachToExistingMVM = false;
                bool startupProfilerNode = false;
                bool isSlave = myArgs.Parameters.ContainsKey("slave_id");
                string startingNamespace = myArgs["namespace"];
                string clusterName = myArgs["cluster"];

                if (myArgs.Parameters.ContainsKey("profile") && !isSlave)
                {
                    if (myArgs.Parameters.ContainsKey("proc"))
                    {
                        // This is a normal MVM startup, except we want to start profiling on all nodes
                        startupProfilerNode = true;
                    }
                    else if (myArgs.Parameters.ContainsKey("profiler_id"))
                    {
                        // We were spawned by the supernode, so we already have everything we need
                        isProfiler = true;
                    }
                    else
                    {
                        // We need to profile an existing MVM cluster, so scan the super's logfile for the
                        // info we need.  If we can't find a valid logfile, we will error out.
                        mvm.BootstrapProfiler(myArgs);
                        attachToExistingMVM = true;
                        isProfiler = true;
                    }
                }

                mvm.isStandalone = myArgs.Parameters.ContainsKey("s");
                mvm.SetGlobalParameters(myArgs);
                string startingProc = myArgs["proc"];

                // This is a kludge, but temporarily turn on tracking of the miscellaneous
                // xml code we want to synchronize with remote listeners.  Doing this for
                // all xml strings doesn't work as we don't want to synchronize many of them
                // and it's difficult to tell them all apart at the point where the parsing
                // happens.
                mvm.addXmlHashes = true;
                if (!mvm.isStandalone)
                {
                    mvm.LoadRmpProcs();
                    mvm.LoadEmbeddedProcs();
                }
                mvm.LoadCommandlineXml(myArgs["xml"], myArgs["namespace"], myArgs["xml_newline"]);
                mvm.LoadCommandlineXmlFile(myArgs["file"], myArgs["namespace"]);
                mvm.addXmlHashes = false;

                mvm.ProcessCommandlineArgs2(myArgs);

                // slave
                if (isSlave)
                {
                    mvm.SetupAsSlave();
                    mvm.StartupWorkerThreads();
                    mvm.InitializeGlobalNamespace();
                    mvm.SendSlaveInitMessage();
                    mvm.LogIdentifyingInfo();
                    mvm.WaitForShutdown();
                    mvm.Shutdown();
                }
                // profiler
                else if (isProfiler)
                {
                    mvm.SetupAsProfiler(attachToExistingMVM);
                    mvm.StartupWorkerThreads();
                    mvm.InitializeGlobalNamespace();
                    if (!attachToExistingMVM)
                    {
                        mvm.SendSlaveInitMessage();
                    }
                    mvm.LogIdentifyingInfo();
                    mvm.WaitForShutdown();
                    mvm.Shutdown();
                }
                // super cluster
                else if (clusterName.NotNullOrEmpty())
                {
                    mvm.SetupAsSuper(startupProfilerNode);
                    mvm.StartupWorkerThreads();
                    mvm.InitializeGlobalNamespace();
                    mvm.StartupCluster(clusterName);
                    mvm.LogIdentifyingInfo();
                    string masterId = mvm.globalContext["master_id"];
                    if (masterId.Equals("0"))
                        mvm.CallProc(startingProc, startingNamespace);
                    else
                        mvm.CallProcOnMaster(startingProc, startingNamespace);
                    mvm.ShutdownCluster();
                    mvm.Shutdown();
                }
                // super single
                else
                {
                    mvm.SetupAsSuper(startupProfilerNode);
                    mvm.StartupWorkerThreads();
                    mvm.InitializeGlobalNamespace();
                    mvm.StartupSingle();
                    mvm.LogIdentifyingInfo();
                    mvm.CallProc(startingProc, startingNamespace);
                    mvm.ShutdownCluster();
                    mvm.Shutdown();
                }
                //mvm.DumpMemory();
            }
            catch (Exception e)
            {
                if (mvm == null)
                    throw new Exception("mvm engine failed to start", e);
                mvm.PrintStackTrace(e);
                mvm.exitCode = 1;
            }
            if (inIde)
                PressAnyKeyToExit();
            System.Environment.Exit(mvm.exitCode);
        }

        private AutoResetEvent slaveShutDownEvent = new AutoResetEvent(false);
        public void WaitForShutdown()
        {
            //this.Log("waiting for shutdown event");
            this.slaveShutDownEvent.WaitOne();
            //this.Log("got slave shutdown event");
        }
        public void ShutdownSlave()
        {
            this.Log("setting slave shutdown event");
            this.slaveShutDownEvent.Set();
        }

        public void SendSlaveInitMessage()
        {
            //this.Log("Sending slave initialization message to super node");
            bool is64bit = IntPtr.Size == 8;
            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties["is64bit"] = is64bit ? "1" : "0";
            properties["pid"] = MvmEngine.ProcessId.ToString();
            this.mvmCluster.GetClusterNode(0).SocketHandler.messageOutbox.Add(new SlaveInitMessage(properties));
        }

        public void InitializeGlobalNamespace()
        {
            if (this.isStandalone)
            {
                return;
            }
            this.CallProc("initialize_core_procs", "global");
            this.Log("Done initializing global namespace");
        }

        public void CallProc(string param_proc, string param_namespace)
        {
            param_namespace = param_namespace.Nvl("global");
            if (!param_proc.IsNullOrEmpty())
            {
                this.Log("call proc [" + param_proc + "]");
                this.workMgr.WorkMgrResetWorkToComplete();
                string startProcName = param_proc;
                string startNameSpace = !param_namespace.IsNullOrEmpty() ? param_namespace : "global";
                int startProcId = this.workMgr.schedulerMaster.GetProcId(startNameSpace, startProcName);
                string startingObjectId = this.workMgr.objectCache.CreateAndGetObjectId("STARTING_OBJECT");
                ProcInst startingWork = ProcInst.CallProcForObjectId(
                    this.workMgr.schedulerMaster.GetScheduler(),
                    startProcId,
                    startingObjectId);
                this.workMgr.ProduceStackWork(startingWork);
                this.workMgr.WorkMgrWaitForWorkToComplete();
                this.Log("done proc [" + param_proc + "]");
            }
            if (this.workMgr.HaveException())
            {
                throw new Exception("Worker error!");
            }
        }
        public void CallProcOnMaster(string param_proc, string param_namespace)
        {
            // need to remote call a proc on
            string masterId = this.globalContext["master_id"];
            this.Log("Calling proc " + param_proc + " on master_id [" + masterId + "]");
            // this is a bit of a hack
            this.globalContext["call_global_starting_proc_on_master"] = param_proc;
            this.CallProc("call_global_starting_proc_on_master", "global");
        }


        public bool ClusterIsStarted = false;
        public void StartupCluster(string clusterName)
        {
            this.Log("starting cluster [" + clusterName + "]");
            this.mvmClusterSuper.ServerCredentialsGet();
            this.mvmClusterSuper.ReadMvmClusterTable(clusterName);
            this.mvmClusterSuper.SetupMvmClusterSuper();
            int numNodes = this.mvmClusterSuper.StartupNodes();
            this.Log("done starting cluster [" + clusterName + "] numNodes=[" + numNodes + "]");
            ClusterIsStarted = true;
        }

        public void StartupSingle()
        {
            this.Log("starting single");
            this.mvmClusterSuper.RegisterMachinePortRange(MachineName, MvmClusterCommon.DefaultPortStart, MvmClusterCommon.DefaultPortEnd);
            this.mvmClusterSuper.SetupMvmClusterSuper();
            int numNodes = this.mvmClusterSuper.StartupNodes();
            this.Log("done starting single");
        }

        // This locally keeps track of whether the profiler process has been started by this node
        public volatile bool TriedToStartProfiler = false;
        public void StartProfilerNode(int samplingPeriod, int reportingCount)
        {
            this.Log("trying to start profiler node");
            this.mvmCluster.SuperNode.SocketHandler.messageOutbox.Add(new ProfilerInitMessage(samplingPeriod, reportingCount));
            this.Log("done trying to start profiler node");
            TriedToStartProfiler = true;
        }
        public Dictionary<int, bool> StartedProfilerThread = new Dictionary<int, bool>();

        /// <summary>
        /// Starts up the GLOBAL.cluster or 'default' cluster if no cluster has
        /// yet been started.
        /// </summary>
        public void StartupCluster()
        {
            if (this.ClusterIsStarted || this.nodeId != 0)
                return;
            string theClusterName = this.globalContext["cluster"];
            if (theClusterName.IsNullOrEmpty())
            {
                theClusterName = "default";
            }
            this.StartupCluster(theClusterName);
        }

        public void ShutdownCluster()
        {
            this.Log("shutdown cluster");
            this.mvmClusterSuper.Shutdown();
            NLog.LogManager.Flush();
            this.Log("done shutdown cluster");
        }
        public void Shutdown()
        {
            this.Log("shutdown worker threads");
            this.workMgr.ShutdownWorkers();
            this.Log("reaping worker threads");
            this.workMgr.ReapWorkerThreads();
        }
        #endregion

        # region Stack trace handling and process exiting
        public static void PressAnyKeyToExit()
        {
            Console.WriteLine("MVM complete, press ENTER to exit...");
            try
            {
                var x = Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Console.ReadLine() threw exception: " + e.Message);
            }
        }

        public string GetStackTrace(Exception e)
        {
            StringBuilder sb = new StringBuilder();
            GetStackTraceRecursive(e, sb);
            return sb.ToString();
        }

        public void GetStackTraceRecursive(Exception e, StringBuilder sb)
        {
            if (e.InnerException != null)
            {
                GetStackTraceRecursive(e.InnerException, sb);
            }
            sb.AppendLine(e.Message);
            sb.AppendLine(e.StackTrace);
            sb.AppendLine("-".repeat(80));
        }

        public void PrintStackTrace(Exception e)
        {
            PrintStackTraceRecursive(e);
        }

        public void PrintStackTraceRecursive(Exception e)
        {
            if (e.InnerException != null)
            {
                PrintStackTraceRecursive(e.InnerException);
            }
            Error(e.Message);
            Error(e.StackTrace);
            Error("-".repeat(80));
        }
        #endregion
    }

    public struct XmlResourceInfo
    {
        public string filename;
        public string remoteFilename;  // can be different e.g. for files not under RMP
        public byte[] data;
        public XmlResourceInfo(string rmp, string file, byte[] data)
        {
            this.filename = file;
            this.data = data;
            if (file != null && file.StartsWith(rmp))
            {
                this.remoteFilename = MvmClusterCommon.PathRelativeToRMP(rmp, file);
            }
            else
            {
                this.remoteFilename = @"RMP\Extensions\MvmListener\MvmConfig\" + Path.GetRandomFileName().Replace(".", "") + ".xml";
            }
        }
    }
}
