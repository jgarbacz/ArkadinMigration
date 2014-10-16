using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using NLog;

namespace MVM
{
    public class ModuleContext
    {

        public void DumpMemory()
        {
            this.mvm.DumpMemory();
        }




        #region Instance members set once by the instanciating worker

        // TBD:this can be setu at an even higher level
        public static SyntaxMaster syntaxMaster = new SyntaxMaster();

        // globalContext<-threadContext<-procContext->tempContext;
        public ProcContext procContext;
        public ThreadContext threadContext;
        public GlobalContext globalContext;
        public TempContext tempContext;

        // this is the worker that is running the current newModule
        public Worker worker;

        #endregion

        #region Instance members set with each piece of procInst

        // current objectData 
        public IObjectData objectData;

        // current idx into the procInst newModule
        public int moduleIdx;

        // current newModule
        //public IModuleRun module;

        // definition for current procContext
        public ProcDefinition procDefinition;

        private int popTheScopeProcId
        {
            get
            {
                return this.schedulerMaster.popTheScopeProcId;
            }
        }

        public string NameSpace
        {
            get
            {
                return this.procDefinition.nameSpace;
            }
        }
        public string structuralNameSpace
        {
            get
            {
                return this.procDefinition.nameSpace;
            }
        }
        public IConfigLocator configLocator
        {
            get
            {
                return this.procDefinition.procInfo.location;
            }
        }
        
        public string LocalName
        {
            get
            {
                return this.procDefinition.localName;
            }
        }

        #endregion

        #region Breaking out

        public string breakFromProcName;

        #endregion

        #region Proc instances

        public void ProcInstDelete(long delProcInstId)
        {
            this.workMgr.DeleteCallback(this, delProcInstId);
        }

       
        #endregion


        #region Exception handling

        public MvmUserException exception;
        public MvmUserException caughtException;

        // this is how user config can throw an exception.
        public void ThrowException(string name, string message)
        {
            if (this.exception == null)
            {
                this.exception = MvmUserException.Create(name, message);
            }
            else
            {
                this.exception = MvmUserException.Create(name, message, exception);
            }
            this.moduleStatus = ModuleStatus.Exception;
        }

        public string GetCaughtExceptionName()
        {
            if (caughtException != null)
            {
                MvmUserException mvmUserException = caughtException as MvmUserException;
                if (mvmUserException!=null)
                {
                    return mvmUserException.exceptionName;
                }
                else
                {
                    return caughtException.GetType().ToString();
                }
            }
            return "";
        }

        public string GetCaughtExceptionMessage()
        {
            if (caughtException != null)
            {
                MvmUserException mvmUserException = caughtException as MvmUserException;
                if (mvmUserException != null)
                {
                    return mvmUserException.exceptionMessage;
                }
                else
                {
                    return caughtException.Message;
                }
            }
            return "";
        }

        public string GetCaughtExceptionStackTrace()
        {
            if (caughtException != null)
            {
                return caughtException.GetStackTraceRecursive();
            }
            return "";
        }

        #endregion

        #region Instance members set by the newModule to communicate to worker threadContext

        // this is set to indicate what should happen next

        public ModuleStatus moduleStatus = ModuleStatus.Continue;
        public int gotoModuleIdx;

        #endregion

        #region Static members

       
        public static long genSymCtr = 0;

        #endregion

        public readonly static Dictionary<string, string> internalObjectFields = new Dictionary<string, string>();
        static ModuleContext()
        {
            internalObjectFields["object_type"] = "object_type";
            internalObjectFields["eof"] = "eof";
            internalObjectFields["object_id"] = "object_id";

        }

        public bool LookupCursorViaOid(string csrOid, out ICursorBase csr)
        {
            if (csrOid == null)
            {
                csr = null;
                return false;
            }
            using (IObjectData currDataObject = this.objectCache.CheckOut(csrOid))
            {
                if (currDataObject.CursorInstId == null)
                {
                    csr = null;
                    return false;
                }
                csr = (ICursorBase)this.globalContext.GetNamedClassInst(currDataObject.CursorInstId);
                return true;
            }
        }

        public bool LookupCursorViaOid(string csrOid,out ICursor csr)
        {
            csr = null;
            ICursorBase cursorBase;
            if (!this.LookupCursorViaOid(csrOid, out cursorBase)) return false;
            csr = cursorBase as ICursor;
            return csr != null;
        }

        public ICursor LookupCursorViaOid(string csrOid)
        {
            using (IObjectData currDataObject = this.objectCache.CheckOut(csrOid))
            {
                if (currDataObject.CursorInstId == null)
                {
                    throw new Exception("Error, cannot lookup cursor for object with object_id=[" + currDataObject.objectId+ "] because it does not tie to a cursor_inst_id");
                }
                try
                {
                    ICursor cursor = (ICursor)this.globalContext.GetNamedClassInst(currDataObject.CursorInstId);
                    return cursor;
                }
                catch (Exception e)
                {
                    throw new Exception("Error cannot find cursor with cursor_inst_id=[" + currDataObject.CursorInstId + "] for object_id=[" + currDataObject.objectId+ "]",e);
                }
            }
        }
        public ICursor LookupCursorViaInstId(string csrInstId)
        {
            try
            {
                ICursor cursor = (ICursor)this.globalContext.GetNamedClassInst(csrInstId);
                return cursor;
            }
            catch (Exception e)
            {
                throw new Exception("Error cannot find cursor with cursor_inst_id=[" + csrInstId + "]",e);
            }
        }

        public bool LookupCursorViaInstId(string csrInstId, out ICursor cursor)
        {
            object classInst;
            this.globalContext.GetNamedClassInst(csrInstId, out classInst);
            cursor=(ICursor) classInst;
            return cursor != null;
        }

        public bool LookupCursorViaInstId(string csrInstId, out ICursorBase cursor)
        {
            object classInst;
            this.globalContext.GetNamedClassInst(csrInstId, out classInst);
            cursor = (ICursorBase)classInst;
            return cursor != null;
        }

         public bool LookupCursorViaInstId(string csrInstId, out ICursorOp cursor)
        {
            object classInst;
            this.globalContext.GetNamedClassInst(csrInstId, out classInst);
            cursor = (ICursorOp)classInst;
            return cursor != null;
        }




        #region Used by the newModules

        public int GetProcId(string nameSpace,string localName)
        {
            return this.schedulerMaster.GetProcId(nameSpace, localName);
        }

        public int GetProcId(string localName)
        {
            return this.schedulerMaster.GetProcId(this.NameSpace, localName);
        }

        //public int GetProcIdDefaulted(string localName,string defaultLocalName)
        //{
            
        //    return this.schedulerMaster.GetProcId(this.NameSpace, localName);
        //}

        public ProcInfo GetProcInfo(string localName)
        {
            return this.schedulerMaster.GetProcInfo(this.NameSpace, localName);
        }
       
        public static bool IsInternalObjectField(string fieldName)
        {
            return internalObjectFields.ContainsKey(fieldName);
        }
        
        public void ReadXmlProcFromElem(string procName, XmlElement procElem)
        {
            this.schedulerMaster.ReadXmlProcFromElem(this.NameSpace,procName, procElem, this.worker,this.structuralNameSpace,this.configLocator);
        }
 
        public void ReadXmlConfigFromString(string configString)
        {
            this.schedulerMaster.ReadXmlConfigFromString(configString, this.worker,this.NameSpace,this.configLocator);
        }
        
        public int ReadXmlConfigFromFiles(params string[] xmlFiles)
        {
            return this.schedulerMaster.ReadXmlConfigFromFiles(xmlFiles, this.worker, this.NameSpace);
        }

        /// <summary>
        /// Returns the current datetime in our internal dateformat
        /// </summary>
        /// <returns></returns>
        public string Now()
        {
            var now = System.DateTime.Now;
            StringBuilder sb = new StringBuilder(14);
            sb.Append(now.Year.ToString());
            sb.Append(now.Month.ToString().PadLeft(2, '0'));
            sb.Append(now.Day.ToString().PadLeft(2, '0'));
            sb.Append(now.Hour.ToString().PadLeft(2, '0'));
            sb.Append(now.Minute.ToString().PadLeft(2, '0'));
            sb.Append(now.Second.ToString().PadLeft(2, '0'));
            return sb.ToString();
        }

        public LogLevel GetLogLevel(string logLevelStr)
        {
            if (logLevelStr.Equals(""))
            {
                logLevelStr = "all";
            }
            if (LogLevels.HasLogLevel(logLevelStr))
            {
                return LogLevels.GetLogLevel(logLevelStr);
            }
            throw new Exception("Unexpected value for log_level [" + logLevelStr + "]");
        }

        public LogLevel GetCurrentLogLevel()
        {
            return this.GetLogLevel(this.globalContext["log_level"]);
        }

        public IModuleRun GetLogModule(string message, string level)
        {
            return GetLogModule(message, level, false);
        }
        public IModuleRun GetLogModule(string message, string level,bool isLiteral)
        {
            string logXml;
            string isLiteralAttribute = isLiteral ? " is_literal='1'" : "";
            string encodedMessage = System.Security.SecurityElement.Escape(message);
            if (LogLevels.HasLogLevel(level))
            {
                logXml = "<" + level + " switch='log_db'" + isLiteralAttribute + ">" + encodedMessage + "</" + level + ">";
            }
            else
            {
                logXml = "<log level='" + level + "' switch='log_db'" + isLiteralAttribute + ">" + encodedMessage + "</log>";
            }
            XmlElement logMe = this.mvm.procLoader.ParseInputXmlString(logXml);
            return this.mvm.procLoader.ReadXmlModule(logMe);
        }

        #region Loading xml newModules

        public List<IModuleRun> ReadXmlModules(string xmlString)
        {
            return this.mvm.procLoader.ReadXmlModules(xmlString);
        }
        
        public List<IModuleRun> ReadXmlModules(XmlElement elem)
        {
            return this.mvm.procLoader.ReadXmlModules(elem);
        }

        public IModuleRun ReadXmlModule(XmlElement moduleElement)
        {
            return this.mvm.procLoader.ReadXmlModule(moduleElement);
        }

        public IModuleRun ReadXmlModule(string xmlString)
        {
            return this.mvm.procLoader.ReadXmlModule(xmlString);
        }
#endregion


        #region Affect newModule moduleStatus

        /// <summary>
        /// Make current proc go off the stack, then back on, and continue on the current.
        /// </summary>
        public void YieldAndCallback()
        {
            this.procInst.nextModuleOrder = this.moduleOrder;
            this.worker.ProduceWork(this.procInst);
            this.moduleStatus = ModuleStatus.Yield;
        }

        /// <summary>
        /// Make current proc go off the stack, then back on, and continue on the next module.
        /// </summary>
        public void YieldAndContinue()
        {
            this.procInst.nextModuleOrder = this.procDefinition.GetNextModuleOrder(this.moduleIdx);
            this.worker.ProduceWork(this.procInst);
            this.moduleStatus = ModuleStatus.Yield;
        }

        /// <summary>
        /// Make current proc go off the stack. If someone calls it back, it continues on the same module.
        /// </summary>
        public void YieldAndRepeat()
        {
            this.procInst.nextModuleOrder = this.moduleOrder;
            this.moduleStatus = ModuleStatus.Yield;
        }

        /// <summary>
        /// Make current proc go off the stack.
        /// </summary>
        public void Yield()
        {
            this.moduleStatus = ModuleStatus.Yield;
        }

        /// <summary>
        /// Make the program ptr goto the passed label
        /// </summary>
        /// <param name="label"></param>
        public void GotoLabel(string label)
        {
            this.moduleStatus = ModuleStatus.GotoModuleIdx;
            this.gotoModuleIdx = this.procDefinition.GetLabelModuleIdx(label);
        }

        #endregion


        #region Generating procs and newModules

        /// <summary>
        /// Generate a unique yet somewhat informative child proc name.
        /// </summary>
        /// <param name="desciption"></param>
        /// <returns></returns>
        public string GetChildProcName(string desciption)
        {
            return this.procDefinition.localName + "/" + desciption + "[" + this.moduleOrder + "]";
        }

        /// <summary>
        /// parses a single xml newModule and returns the IModuleRun for it
        /// </summary>
        /// <param name="moduleElem"></param>
        /// <returns></returns>
        public IModuleRun GetModuleRun(XmlElement moduleElem)
        {
            return this.schedulerMaster.GetModuleRun(moduleElem);
        }

        /// <summary>
        ///  Parses a single xml newModule and returns the IModuleRun for it
        /// </summary>
        /// <param name="xmlModule"></param>
        /// <returns></returns>
        public IModuleRun GetModuleRun(string xmlModule)
        {
            return this.schedulerMaster.GetModuleRun(xmlModule);
        }
        /// <summary>
        /// Given a list of newModule elements it returns the list of IModuleRun.
        /// </summary>
        /// <param name="moduleElems"></param>
        /// <returns></returns>
        public List<IModuleRun> GetModuleRunList(IEnumerable<XmlElement> moduleElems)
        {
            List<IModuleRun> run = new List<IModuleRun>();
            foreach (XmlElement moduleElem in moduleElems)
            {
                run.Add(this.schedulerMaster.GetModuleRun(moduleElem));
            }
            return run;
        }


        #endregion

        #region Calling/queuing procs


        /// <summary>
        /// Returns a ProcInst object for to call the passed procId;
        /// </summary>
        /// <param name="procId"></param>
        /// <returns></returns>
        public ProcInst GetProcToProcId(int procId,string objectId){
            ProcInfo procInfo = scheduler.GetProcInfo(procId);
            ProcInst work = new ProcInst(procInfo,objectId);
            return work;
        }
        


        // calls the procContext for the current object id
        public ProcInst CallProcForCurrentObject(int procId)
        {
            return this.CallProcForObject(procId, this.objectData.objectId);
        }

        // calls the procContext for the passed object id, supports passing in params.
        public ProcInst CallProcForObject(int procId, string objectId,TempContext tempContext)
        {
            // Advance to the next newModule
            this.procInst.nextModuleOrder = this.procDefinition.GetNextModuleOrder(this.moduleIdx);

            // Setup the procInst for the procContext
            ProcInst newWork = this.GetProcToProcId(procId, objectId);
            newWork.isSync = this.procInst.isSync;
            //newWork.objectId = objectId;
            newWork.procContext = new ProcContext(tempContext);

            // Setup the procNameSyntax chain
            newWork.callbackId = this.workMgr.CreateCallback(this.procInst);
            // Put the new procInst in the machine
            this.worker.ProduceWork(newWork);
            // Make the procContext yield b/c the cur procInst is not finished
            this.Yield();
            return newWork;
        }

        // Calls the proc for the passed object id, 
        // returns the top level ProcInst so procNameSyntax chain is correct.
        public ProcInst CallProcForObject(int procId, string objectId)
        {
            // Advance to the next newModule
            this.procInst.nextModuleOrder = this.procDefinition.GetNextModuleOrder(this.moduleIdx);

            // Setup the procInst for the procContext
            ProcInst newWork = this.GetProcToProcId(procId,objectId);
            newWork.isSync = this.procInst.isSync;
            //newWork.objectId = objectId;
            newWork.procContext = new ProcContext();

            // Setup the procNameSyntax chain
            newWork.callbackId = this.workMgr.CreateCallback(this.procInst);
            // Put the new procInst in the machine
            this.worker.ProduceWork(newWork);
            // Make the procContext yield b/c the cur procInst is not finished
            this.Yield();
            // return ref to the new work...
            return newWork;
        }

        // calls the procContext for the passed object id
        public void CallProcForObjectNoCallback(int procId, string objectId)
        {
            // Advance to the next newModule
            this.procInst.nextModuleOrder = this.procDefinition.GetNextModuleOrder(this.moduleIdx);
            // Setup the procInst for the procContext
            ProcInst newWork = this.GetProcToProcId(procId, objectId);
            newWork.isSync = this.procInst.isSync;
            //newWork.objectId = objectId;
            newWork.procContext = new ProcContext();
            // Put the new procInst in the machine
            this.worker.ProduceWork(newWork);
            // Make the procContext yield b/c the cur procInst is not finished
            this.Yield();
        }

        // queues the procContext for the current object id with procNameSyntax
        public void QueueProcForCurrentObjectWithCb(int procId, int cbProcId, string cbObjectId, string cbClusterObjectId)
        {
            this.QueueProcForObjectWithCb(procId, this.objectData.objectId, cbProcId, cbObjectId);
        }

        // queues the procContext for the current object id
        public void QueueProcForCurrentObject(int procId)
        {
            this.QueueProcForObject(procId, this.objectData.objectId);
        }

        // queues the procContext for the passed object id in the current cluster with procNameSyntax
        public void QueueProcForObjectWithCb(int procId, string objectId, int cbProcId, string cbObjectId)
        {
            this.QueueProcForObjectWithCb(procId, objectId, cbProcId, cbObjectId);
        }

        // queues the procContext for the passed object id in the current cluster
        public void QueueProcForObject(int procId, string objectId)
        {
            this.QueueProcForObject(procId, objectId);
        }




        // queues the procContext for the passed object id, in the passed cluster
        public void QueueProcForObjectNestedWithCb(int procId, string objectId, int cbProcId, string cbObjectId)
        {
            // Advance to the next newModule
            this.procInst.nextModuleOrder = this.procDefinition.GetNextModuleOrder(this.moduleIdx);

            // Setup the procInst for the procContext
            ProcInst newWork = this.GetProcToProcId(procId, objectId);
            newWork.isSync = this.procInst.isSync;
            //newWork.objectId = objectId;
            TempContext forkedTempContext = this.procInst.procContext.tempContext.Fork();
            newWork.procContext = new ProcContext(forkedTempContext);
            // Setup the procNameSyntax chain
            ProcInst cbWork = this.GetProcToProcId(cbProcId, cbObjectId);
            cbWork.isSync = this.procInst.isSync;
            //cbWork.objectId = cbObjectId;
            cbWork.procContext = new ProcContext();
            newWork.callbackId = this.workMgr.CreateCallback(cbWork);
            // produce the new procInst
            this.workMgr.ProduceWork(newWork);
        }

        // queues the procContext for the current object id, in the passed cluster
        public void QueueProcForCurrentObjectNestedWithCb(int procId, int cbProcId, string cbObjectId)
        {
            this.QueueProcForObjectNestedWithCb(procId, this.objectData.objectId, cbProcId, cbObjectId);
        }

        
        /// <summary>
        /// Calls the proc for the current object id with nested scope. Caller of this 
        /// better have inlined snap/unsnap scope modules... If they didn't then the
        /// scope grows our of control.
        /// </summary>
        /// <param name="procId"></param>
        public void CallProcForCurrentObjectNested(int procId)
        {
            this.CallProcForObjectNested(procId, this.objectData.objectId);
        }

        /// <summary>
        /// Calls the proc for the passed object id with nested scope.Caller of this 
        /// better have inlined snap/unsnap scope modules... If they didn't then the
        /// scope grows our of control.
        /// </summary>
        /// <param name="procId"></param>
        /// <param name="objectId"></param>
        public void CallProcForObjectNested(int procId, string objectId)
        {
            this.tempContext.PushScope();
            this.procInst.nextModuleOrder = this.procDefinition.GetNextModuleOrder(this.moduleIdx);
            ProcInst newWork = this.GetProcToProcId(procId, objectId);
            newWork.isSync = this.procInst.isSync;
            //newWork.objectId = objectId;
            // FIXED BUG: we were using the same proc context, but we cannot do this because we store
            // the temp scope snapshot on the proc context and the kid will erase the parents snapshot.
            //newWork.procContext = this.procContext; 
            // NEW WAY: get a new proc context but use the parents temp context.
            newWork.procContext = new ProcContext(this.tempContext);
            newWork.callbackId = this.workMgr.CreateCallback(this.procInst);
            this.worker.ProduceWork(newWork);
            this.Yield();
        }


        /// <summary>
        /// Calls the proc for the current object id with nested scope(no push)
        /// </summary>
        /// <param name="procId"></param>
        public void CallProcForCurrentObjectSameScope(int procId)
        {
            this.CallProcForObjectSameScope(procId, this.objectData.objectId);
        }

        /// <summary>
        /// Calls the proc for the passed object id with same scope (no push)
        /// </summary>
        /// <param name="procId"></param>
        /// <param name="objectId"></param>
        public void CallProcForObjectSameScope(int procId, string objectId)
        {
            this.procInst.nextModuleOrder = this.procDefinition.GetNextModuleOrder(this.moduleIdx);
            ProcInst newWork = this.GetProcToProcId(procId, objectId);
            newWork.isSync = this.procInst.isSync;
            //newWork.objectId = objectId;
            newWork.procContext = this.procContext;
            newWork.callbackId = this.workMgr.CreateCallback(this.procInst);
            this.worker.ProduceWork(newWork);
            this.Yield();
        }

        /// <summary>
        /// NOTE: you better make sure the scope gets popped!
        /// </summary>
        /// <param name="procId"></param>
        /// <param name="objectId"></param>
        public void CallProcForObjectNestedNoCallbackOnStack(int procId, string objectId)
        {
            // Push the scope
            this.tempContext.PushScope();

            // Advance to the next newModule
            this.procInst.nextModuleOrder = this.procDefinition.GetNextModuleOrder(this.moduleIdx);

            // Setup the procInst for the procContext to call
            ProcInst newWork = this.GetProcToProcId(procId, objectId);
            newWork.isSync = 0;
            //newWork.objectId = objectId;
            newWork.procContext = this.procContext;

            // Put the new procInst in the machine
            this.worker.ProduceStackWork(newWork);

            // Make the procContext yield b/c the cur procInst is not finished
            this.Yield();
        }

       
        // NOTE: you better make sure the scope gets popped!
        public void CallProcForObjectNestedNoCallback(int procId, string objectId)
        {
            // Push the scope
            this.tempContext.PushScope();

            // Advance to the next newModule
            this.procInst.nextModuleOrder = this.procDefinition.GetNextModuleOrder(this.moduleIdx);

            // Setup the procInst for the procContext to call
            ProcInst newWork = this.GetProcToProcId(procId, objectId);
            newWork.isSync = this.procInst.isSync;
            //newWork.objectId = objectId;
            newWork.procContext = this.procContext;

            // Put the new procInst in the machine
            this.worker.ProduceWork(newWork);

            // Make the procContext yield b/c the cur procInst is not finished
            this.Yield();
        }

        // calls the procContext for the passed object id and return to the calling newModule
        public void CallProcForCurrentObjectAndReturn(int procId)
        {
            this.CallProcForObjectAndReturn(procId, this.objectData.objectId);
        }

        // calls the procContext for the passed object id and return to the calling newModule
        public void CallProcForObjectAndReturn(int procId, string objectId)
        {
            // Keep this procContext on the current newModule
            this.procInst.nextModuleOrder = this.procDefinition.GetCurrModuleOrder(this.moduleIdx);

            // Setup the procInst for the procContext
            ProcInst newWork = this.GetProcToProcId(procId, objectId);
            newWork.isSync = this.procInst.isSync;
            //newWork.objectId = objectId;
            newWork.procContext = new ProcContext();

            // Setup the procNameSyntax chain
            newWork.callbackId = this.workMgr.CreateCallback(this.procInst);

            // Put the new procInst in the machine
            this.worker.ProduceWork(newWork);

            // Make the procContext yield b/c the cur procInst is not finished
            this.Yield();
        }

        // call procContext
        public void CallProcForCurrentObjectAndReturnNested(int procId)
        {
            this.CallProcForObjectAndReturnNested(procId, this.objectData.objectId);
        }

        // calls the procContext for the passed object id and return to the calling newModule
        public void CallProcForObjectAndReturnNested(int procId, string objectId)
        {
            // Keep this procContext on the current newModule
            this.procInst.nextModuleOrder = this.procDefinition.GetCurrModuleOrder(this.moduleIdx);

            // Setup the procInst for the procContext
            ProcInst newWork = this.GetProcToProcId(procId, objectId);
            newWork.isSync = this.procInst.isSync;
            //newWork.objectId = objectId;
            newWork.procContext = this.procContext;

            // Setup the procNameSyntax chain
            newWork.callbackId = this.workMgr.CreateCallback(this.procInst);

            // Put the new procInst in the machine
            this.worker.ProduceWork(newWork);

            // Make the procContext yield b/c the cur procInst is not finished
            this.Yield();
        }

        public void QueueProcForCurrentObjectNestedOnStack(int procId)
        {
            this.QueueProcForObjectNestedOnStack(procId, this.objectData.objectId);
        }
        // Forks the tempContext scope of the calling procContext and queues a call up for a new procContext.
        public void QueueProcForObjectNestedOnStack(int procId, string objectId)
        {
            // Advance to the next newModule in current procContext
            this.procInst.nextModuleOrder = this.procDefinition.GetNextModuleOrder(this.moduleIdx);
            // Setup the procInst for the procContext to queue
            ProcInst newWork = this.GetProcToProcId(procId, objectId);
            newWork.isSync = 0;
            //newWork.objectId = objectId;
            TempContext newTempContext = this.procInst.procContext.tempContext.Fork();
            newWork.procContext = new ProcContext(newTempContext);
            // produce the new procInst
            this.workMgr.ProduceStackWork(newWork);
        }



        public void QueueProcForCurrentObjectNested(int procId)
        {
            this.QueueProcForObjectNested(procId, this.objectData.objectId);
        }

        // Forks the tempContext scope of the calling procContext and queues a call up for a new procContext.
        public void QueueProcForObjectNested(int procId, string objectId)
        {
            // Advance to the next newModule in current procContext
            this.procInst.nextModuleOrder = this.procDefinition.GetNextModuleOrder(this.moduleIdx);
            // Setup the procInst for the procContext to queue
            ProcInst newWork = this.GetProcToProcId(procId, objectId);
            newWork.isSync = this.procInst.isSync;
            //newWork.objectId = objectId;
            TempContext newTempContext = this.procInst.procContext.tempContext.Fork();
            newWork.procContext = new ProcContext(newTempContext);
            // produce the new procInst
            this.workMgr.ProduceWork(newWork);
        }



        // Registers a procNameSyntax and returns the generated name for it
        public long AddCallback()
        {
            return this.workMgr.CreateCallback(this.procInst);
        }

        // Registers a procNameSyntax and returns the generated name for it
        public long AddCallbackWithScopePop()
        {
            // Setup procInst that deals with popping
            ProcInst popWork = this.GetProcToProcId(this.popTheScopeProcId, this.objectData.objectId);
            popWork.isSync = this.procInst.isSync;
            //popWork.objectId = this.objectData.objectId;
            popWork.procContext = this.procContext;

            // Setup the procNameSyntax chain
            long popCallbackId = this.workMgr.CreateCallback(popWork);
            popWork.callbackId = this.workMgr.CreateCallback(this.procInst);
            return popCallbackId;
        }


        #endregion

        # region Altering a procContext
        public void RemoveCurrentModule()
        {
            // Make the changes at the schedule master level, knowing that the calling
            // newModule will yield and that and the threadContext's scheduler will get the changes
            // before we resume.
            using (SchedulerMaster.ProcChange pc = this.schedulerMaster.ChangeProc(this.procId))
            {
                pc.nextProc.RemoveModule(this.moduleIdx);

            }
        }

        // this replaces the current newModule with the passed newModules
        public void ReplaceCurModuleWith(params IModuleRun[] newModules)
        {
            // Make the changes at the schedule master level, knowing that the calling
            // newModule will yield and that and the threadContext's scheduler will get the changes
            // before we resume.
            using (SchedulerMaster.ProcChange pc = this.schedulerMaster.ChangeProc(this.procId))
            {
                pc.nextProc.ReplaceModule(this.moduleIdx, newModules);

            }
        }
        // This appends the current executing newModule with the passed newModules
        // There is prob a more straight forward way to do this but i am leveraging that
        // ReplaceCurModuleWith() is well tested.
        //public void AppendCurModuleWith(params IModuleRun[] newModules)
        //{
        //   IModuleRun [] appendModules=new IModuleRun[newModules.Length+1];
        //   appendModules[0] = this.module;
        //   newModules.CopyTo(appendModules, 1);
        //    using (SchedulerMaster.ProcChange pc = this.schedulerMaster.ChangeProc(this.procId))
        //    {
        //        pc.nextProc.ReplaceModule(this.moduleIdx, appendModules);

        //    }
        //}

        #endregion

        # region Parsing

        public bool IsLiteralString(string syntax)
        {
            return syntax.IsQuoted();
        }

        public string GetLiteralString(string syntax)
        {
            return syntax.StripQuotes();
        }

        // parses the syntax and returns the parsed syntax
        public List<IReadString> ParseSyntax(List<string> syntaxList)
        {
            if (syntaxList == null) return null;
            List<IReadString> output = new List<IReadString>();
            foreach (string syntax in syntaxList)
            {
                IReadString v = syntaxMaster.SetupRead(syntax);
                output.Add(v);
            }
            return output;
        }

        // parses and runs the syntax and returns result as a string.
        public string SyntaxReadString(string syntax)
        {
            IReadString v = this.ParseSyntax(syntax);
            if (v == null) return null;
            return v.Read(this);
        }

        // parses the syntax and returns the parsed syntax
        public IReadString ParseSyntax(string syntax)
        {
            if (syntax.IsNullOrEmpty()) return null;
            IReadString v = syntaxMaster.SetupRead(syntax);
            return v;
        }

        // parses the writable syntax and returns the parsed syntax
        public IWriteString ParseWritableSyntax(string syntax)
        {
            try
            {
                if (syntax.IsNullOrEmpty()) return null;
                IWriteString v = syntaxMaster.SetupWritable(syntax);
                return v;
            }
            catch (Exception e)
            {
                throw new Exception("Cannot parse writeable syntax [" + syntax + "]", e);
            }
        }

        // parses the writable syntax and returns the parsed syntax
        public List<IWriteString> ParseWritableSyntax(List<string> syntax)
        {
            List<IWriteString> outputList = new List<IWriteString>();
            foreach (string s in syntax)
            {
                outputList.Add(this.ParseWritableSyntax(s));
            }
            return outputList;
        }

        // Used to pull out ${insides}
        public static readonly Regex translateRegex = new Regex(@"^(.*?)(\${)(.*?)(})(.*?)$", RegexOptions.Compiled);

        // Translates a string with interpolated ${params}. It is careful
        // to only do 1 layer of translation.
        public string Translate(string input)
        {
            //Console.WriteLine("translating:"+input);
            System.Text.StringBuilder output = new System.Text.StringBuilder();
            string remainder = input;
            for (; ; )
            {
                Match m = translateRegex.Match(remainder);
                if (m.Success)
                {
                    string paramName = m.Groups[3].ToString();
                    string paramValue = this.objectData[paramName];
                    output.Append(m.Groups[1].ToString());
                    output.Append(paramValue);
                    remainder = m.Groups[5].ToString();
                }
                else
                {
                    output.Append(remainder);
                    break;
                }
            }
            return output.ToString();
        }


        // Used to pull out ${insides}
        public static readonly Regex dbBindRegex = new Regex(@"^(.*?)(\$\${)(.*?)(})(.*?)$", RegexOptions.Singleline);

        // Translates a string with interpolated ${params}. It is careful
        // to only do 1 layer of translation.
        // pull out the $${x} and replace with :b1, :b2 etc.
        public string TranslateDbBinds(string input, out Dictionary<string,string> binds, string bindVarPrefix)
        {

            // Console.WriteLine("translating db binds:"+input);
            StringBuilder output = new StringBuilder();
            binds = new Dictionary<string,string>();
            string remainder = input;
            for (; ; )
            {
                Match m = dbBindRegex.Match(remainder);
                if (m.Success)
                {
                    string bindSyntax = m.Groups[3].ToString();
                    string bindName = "b" + binds.Count;
                    string bindVar = bindVarPrefix + bindName;
                    binds[bindName] = bindSyntax;
                    output.Append(m.Groups[1].ToString());
                    output.Append(bindVar);
                    remainder = m.Groups[5].ToString();
                }
                else
                {
                    output.Append(remainder);
                    break;
                }
            }
            return output.ToString();
        }
        #endregion

        # region Data Access

        // reads an object field through a reference, default if eq ''
        public string ReadObjectField(string objectId, string fieldName, string defaultValue)
        {
            using (IObjectData obj = objectCache.CheckOut(objectId))
            {
                return obj[fieldName].Equals("") ? defaultValue : obj[fieldName];
            }
        }

        // reads an object field through a reference
        public string ReadObjectField(string objectId, string fieldName)
        {
            using (IObjectData obj = objectCache.CheckOut(objectId))
            {
                return obj[fieldName];
            }
        }

        // reads an object field through a reference
        public string ReadObjectField(string objectId, int ufn)
        {
            using (IObjectData obj = objectCache.CheckOut(objectId))
            {
                return obj[ufn];
            }
        }

        // writes an object field through a reference
        public string  WriteObjectField(string objectId, string fieldName, string fieldValue)
        {
            using (IObjectData obj = objectCache.CheckOut(objectId))
            {
                obj[fieldName] = fieldValue;
                return fieldValue;
            }
        }

        // writes an object field through a reference
        public string WriteObjectField(string objectId, int ufn, string fieldValue)
        {
            using (IObjectData obj = objectCache.CheckOut(objectId))
            {
                obj[ufn] = fieldValue;
                return fieldValue;
            }
        }

        // increments an object field through a reference
        public string IncrementObjectField(string objectId, string fieldName)
        {
            using (IObjectData obj = objectCache.CheckOut(objectId))
            {
                return obj.Increment(fieldName);
            }
        }

        // increments an object field through a reference
        public string DecrementObjectField(string objectId, string fieldName)
        {
            using (IObjectData obj = objectCache.CheckOut(objectId))
            {
                return obj.Decrement(fieldName);
            }
        }

        // returns a certain number of modules starting at a certain point
        // also see the logic in ProcDefinition.ReplaceModule()
        public List<ModuleOrder> GetModuleOrders(ModuleOrder currModuleOrder, int howMany)
        {
            ProcDefinition currDef = this.procDefinition;
            ProcDefinition latestDef = this.schedulerMaster.GetProcDefinition(this.procId);
            List<ModuleOrder> latestModuleOrders = latestDef.moduleOrders;
            List<IModuleRun> latestModules = latestDef.moduleList;
            int firstModuleIndex = latestModuleOrders.BinarySearchForInsertIdx(currModuleOrder);
            return latestModuleOrders.GetRange(firstModuleIndex, howMany);
        }

        // Get or set a field on the current object.
        public string this[string fieldName]
        {
            get
            {
                return this.objectData[fieldName];
            }
            set
            {
                this.objectData[fieldName] = value;
            }
        }

        // copies all non-null fields from source onto the target. (excluding object type and id)
        public void InheritObject(string sourceOid, string targetOid)
        {
            //this.mvm.Info("INHERIT_OBJECT: " + sourceOid + "->" + targetOid);
                using (IObjectData tgt = this.objectCache.CheckOut(targetOid))
                {
                tgt.InheritFieldValues(sourceOid);
                    }
                }

        public void InheritObject(string sourceOid, string targetOid,List<Regex> exclusions)
        {
            using (IObjectData src = this.objectCache.CheckOut(sourceOid))
            {
                using (IObjectData tgt = this.objectCache.CheckOut(targetOid))
                {
                    string tgtObjectType = tgt.objectType;
                
                    foreach (string f in src.FieldNames)
                    {
                        bool include = true;
                        foreach (Regex exclusion in exclusions)
                        {
                            if (exclusion.IsMatch(f))
                            {
                                include = false;
                                break;
                            }
                        }
                        if(include)tgt[f] = src[f];
                   
                    }
                    tgt["object_type"] = tgtObjectType;
                    tgt["object_id"] = tgt.objectId;
                }
            }
        }

        #endregion

        #region Creating objects

        /// <summary>
        /// Instanciates a new delta object
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="deltaTrackingOn"></param>
        /// <returns></returns>
        public string SpawnObjectDataFormattedDelta(string objectType, string feedbackName)
        {
            return this.objectCache.CreateAndGetObjectDataFormattedDelta(objectType, feedbackName).objectId;
        }

        /// <summary>
        /// Instanciates a new delta object
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="deltaTrackingOn"></param>
        /// <returns></returns>
        public string SpawnObjectDataFormatted(string objectType,string feedbackName)
        {
            return this.objectCache.CreateAndGetObjectDataFormatted(objectType,feedbackName).objectId;
        }

        ///// <summary>
        ///// Instanciates a new delta object
        ///// </summary>
        ///// <param name="objectType"></param>
        ///// 
        ///// <returns></returns>
        //public string SpawnObjectDataDelta(string objectType)
        //{
        //    return this.objectCache.CreateAndGetObjectDataDelta(objectType).objectId;
        //}

        /// <summary>
        /// Instanciate a new basic object
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="clusterObjectId"></param>
        /// <returns></returns>
        public string Spawn(string objectType)
        {
            return this.objectCache.CreateAndGetObject(objectType).objectId;
        }

        #endregion

        #endregion

        #region Setup by the worker

        // Constuctor
        public ModuleContext()
        {
        }
       
        // Constructor
        public ModuleContext(ProcInst procCall, Worker worker)
        {
            this.worker = worker;
            this.procInst = procCall;
            this.procDefinition = this.scheduler.GetProcDefinition(this.procInst.procId);
            this.procContext = this.procInst.procContext;
            this.tempContext = this.procContext.tempContext;
            this.threadContext = this.procContext.threadContext;
            this.globalContext = this.procContext.threadContext.globalContext;
           
        }
        #endregion






        public bool GetObjectFieldParts(string fieldAccessSyntax, out string fieldNameSyntax, out string objectIdSyntax)
        {
            Regex curObjRexex = new Regex(@"^\s*OBJECT\.([a-zA-Z0-9_]+)\s*$");
            Match curObjMatch = curObjRexex.Match(fieldAccessSyntax);
            if (curObjMatch.Success)
            {
                objectIdSyntax = "OBJECT.object_id";
                fieldNameSyntax = curObjMatch.Groups[1].Value.q();
                return true;
            }
            Regex refObjRexex = new Regex(@"^\s*OBJECT\((.+)\)\.([a-zA-Z0-9_]+)\s*$");
            Match refObjMatch = refObjRexex.Match(fieldAccessSyntax);
            if (refObjMatch.Success)
            {
                objectIdSyntax = refObjMatch.Groups[1].Value;
                fieldNameSyntax = refObjMatch.Groups[2].Value.q();
                return true;
            }
            objectIdSyntax = null;
            fieldNameSyntax = null;
            return false;
        }



        // used for macroing, it returns a symbol that should be system wide unique (unless someone names 
        // something GeNsYm#__*)
        public string GetGenSym(string hint)
        {
            return "GeNsYm" + (ModuleContext.genSymCtr++) + "__" + hint;
        }

        // creates and returns syntax for a new windows tempContext file
        public string GetTempFileSyntax()
        {
            string tempFileName = System.IO.Path.GetTempFileName();
            string tempFileNameSyntax = tempFileName.EscEsc().q();
            return tempFileNameSyntax;
        }

        #region Read only properties

        public ObjectCache objectCache
        {
            get
            {
                return this.worker.workMgr.objectCache;
            }
        }
       
        public int procId
        {
            get
            {
                return this.procDefinition.procId;
            }
        }

        public string procName
        {
            get
            {
                return this.procDefinition.procName;
            }
        }
        public Scheduler scheduler
        {
            get
            {
                return this.worker.scheduler;
            }
        }
        public SchedulerMaster schedulerMaster
        {
            get
            {
                return this.scheduler.schedulerMaster;
            }
        }



        public readonly ProcInst procInst;
  
        public WorkMgr workMgr
        {
            get
            {
                return this.worker.workMgr;
            }
        }
        public MvmEngine mvm
        {
            get
            {
                return this.worker.workMgr.mvm;
            }
        }
        public MvmClusterBase mvmCluster
        {
            get
            {
                return this.mvm.mvmCluster;
            }
        }
        public ModuleOrder moduleOrder
        {
            get
            {
                return this.procDefinition.moduleOrders[moduleIdx];
            }
        }

       
        #endregion

        
    }
}
