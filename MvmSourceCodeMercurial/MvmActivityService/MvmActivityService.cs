// Added by ICE do not RemoveSpecificItem
using System.ServiceModel;
using System.IO;
using System.Configuration;
using MetraTech.ActivityServices.Services.Common;
using MetraTech.ActivityServices.Common;
using MetraTech.Interop.RCD;


// My includes
using System.Threading;
using MetraTech;
using MVM;
using System;
using System.Reflection;
using System.Collections.Generic;
using MetraTech.DomainModel.MvmTypes;

namespace MetraTech.Custom.Services.Mvm
{

    /// <summary>
    /// Base class for both warning and error
    /// </summary>
    public class MvmValidationException : MASBasicException
    {
        public MvmValidationException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Gui should show the error
    /// </summary>
    public class MvmValidationError : MvmValidationException
    {
        public MvmValidationError(string message)
            : base(message)
        {
        }
    }
    /// <summary>
    /// Gui should warn, then allow user to resubmit with the force flag set.
    /// </summary>
    public class MvmValidationWarning : MvmValidationException
    {
        public MvmValidationWarning(string message)
            : base(message)
        {
        }
    }

    [ServiceContract]
    public interface IMvmActivityService
    {
        [OperationContract]
        [FaultContract(typeof(MASBasicFaultDetail))]
        void CallMvmEntity(ref List<MetraTech.DomainModel.MvmTypes.MvmBaseObject> mvmEntities);

        [OperationContract]
        [FaultContract(typeof(MASBasicFaultDetail))]
        void xx(MetraTech.DomainModel.MvmTypes.MvmBaseObject x);

        [OperationContract]
        [FaultContract(typeof(MASBasicFaultDetail))]
        void Reload();
    }

    public class MvmActivityService : CMASServiceBase, IMvmActivityService
    {            
        static MvmActivityService()
        {
            // THIS IS GENERATED IN BY ICE, DO NOT REMOVE IT!
            MTRcd rcd = new MTRcdClass();
            string configPath = Path.Combine(rcd.ExtensionDir, @"\Config\ActivityServices\MetraTech.Custom.Services.Mvm.MvmActivityService.config");
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = configPath;
            Configuration dllConfig = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            
            // Register starting stopping events
           CMASServiceBase.ServiceStarting += new ServiceStartingEventHandler(StartMvm);
           CMASServiceBase.ServiceStopped += new ServiceStoppedEventHandler(StopMvm);
        }

        private static Logger logger = new Logger("[MvmActivityService]");
        private static MVM.MvmEngine mvm;
        private static Thread mvmThread;
        private static int callEntityProcId;
        private static int wcfServiceCompleteProcId;
        private static WorkMgr workMgr;
        private static SchedulerMaster schedulerMaster;
        private static MVM.MemoryIndex dynamicEntities;
        private static Type[] prms = new Type[0];
        private static Object[] args = new Object[0];

        public static void StartMvm(){
            // Startup an mvm in its own threadContext
            logger.LogDebug("Starting the mvm");
            mvm = new MVM.MvmEngine();
            mvm.UseMtLogger();
            mvm.LoadConfig();
            workMgr = mvm.workMgr;
            schedulerMaster = mvm.workMgr.schedulerMaster;
            //var files = FileUtils2.GlobToList(@"D:\MetraTech\RMP\Extensions\Mvm\config\Mvm\MvmProcs\*.xml");
            //mvm.LoadXmlFileConfig(files);
            mvm.SetServerModeOn();
            mvmThread = new Thread(new ThreadStart(mvm.Start));
            mvmThread.Start();
            logger.LogDebug("Mvm is up, calling startup_entity_server");
            mvm.CallProcSynchronous("startup_entity_server");
            logger.LogDebug("Looking up proc ids");
            callEntityProcId = schedulerMaster.GetProcId("global","call_entity");
            wcfServiceCompleteProcId = schedulerMaster.GetProcId("global","wcf_service_complete");
            logger.LogDebug("startup_entity_server complete");   
            // need to get a reference to dynamic entities
            dynamicEntities = (MemoryIndex)workMgr.globalContext.GetNamedClassInst("DYNAMIC_ENTITIES");
            logger.LogInfo("GLOBAL.target_login=" + workMgr.globalContext["target_login"] + ", HASH=" + workMgr.globalContext.GetHashCode());
        }

        public static void StopMvm()
        {
            mvm.SetServerModeOff();
            // tbd, maybe wait for it to have no more procInst??
        }

        /// <summary>
        /// Stops the old mvm, creates a new one which loads the latest xml config.
        /// </summary>
        public void Reload()
        {
            logger.LogInfo("Got call to reload mvm");
            logger.LogInfo("Calling StopMvm");
            StopMvm();
            logger.LogInfo("Calling StartMvm");
            StartMvm();
        }

        // this is to procInst around the bug in CoreServicesClientProxyHook.exe
        public void xx(MetraTech.DomainModel.MvmTypes.MvmBaseObject x){
        }

        /// <summary>
        /// Entity processing, takes a list in and updates it with the output.
        /// </summary>
        /// <param name="mvmEntities"></param>
        public void CallMvmEntity(ref List<MetraTech.DomainModel.MvmTypes.MvmBaseObject> mvmEntities)
        {
            try
            {
                logger.LogInfo("GLOBAL.target_login=" + workMgr.globalContext["target_login"] + ", HASH=" + workMgr.globalContext.GetHashCode());

                // If no entities were passed just return
                if (mvmEntities == null || mvmEntities.Count == 0) return;

                // All entities should be of the same type
                Type entityType = mvmEntities[0].GetType();
                string entityName = entityType.Name;

                logger.LogDebug("Received service call for entity type: " + entityName + ", num entities=" + mvmEntities.Count);

                // For debugging log everything that was passed in
                this.LogMvmEntities(mvmEntities);


                string entityObjectType = entityName.ToUpper();
                string startClusterObjectId = null;
                try
                {
                    startClusterObjectId = workMgr.clusterCache.CreateCluster();

                    // create the starting procInst 
                    ProcInst startingWork = new ProcInst(schedulerMaster.GetProcInfo(callEntityProcId));
                    startingWork.nextModuleOrder = workMgr.schedulerMaster.GetScheduler().GetProcDefinition(startingWork.procId).GetFirstOrder();
                    startingWork.objectId = startClusterObjectId;
                    startingWork.procContext = new ProcContext();
                    // add the entity name to tempContext scope
                    startingWork.procContext.tempContext["entity_name"] = entityObjectType;

                    // Foreach passed entity create a mvm object with all the values on it.
                    logger.LogDebug("Foreach entity, create an mvm object and copy in values -- EXPECTING JUST 1");
                    int entityNo;
                    for (int i = 0; i < mvmEntities.Count; i++)
                    {
                        entityNo = i + 1;
                        var mvmEntity = mvmEntities[i];
                        logger.LogDebug("Entity #" + entityNo + ":");
                        IObjectData entityObjectData = workMgr.globalContext.cluster.CreateAndGetObject(entityObjectType);
                        // attach the object to the starting work
                        startingWork.objectId = entityObjectData.objectId;
                        foreach (var pi in mvmEntity.GetProperties())
                        {
                            if (!IsRealField(pi) || mvmEntity.GetValue(pi) == null) continue;
                            //if ((!IsRealField(pi) || mvmEntity.GetValue(pi) == null) && !IsDirtyFlag(pi)) continue;
                            string name = pi.Name.ToLower();
                            string value = GetMvmString(pi, mvmEntity.GetValue(pi));
                            logger.LogDebug("set OBJECT." + name + "=[" + value + "]");
                            entityObjectData[name] = value;
                        }
                    }
                    logger.LogDebug("Done-Copy entity properties to mvm entity object");

                    // set the name of the monitor on the cluster  
                    // wcf_service_complete_monitor
                    string monitorName = "wcf_service_complete_monitor_" + startClusterObjectId;
                    using (IObjectData obj = workMgr.objectCache.CheckOut(startClusterObjectId))
                    {
                        obj["wcf_service_complete_monitor"] = monitorName;
                    }

                    // create procNameSyntax procInst that is called when the service procContext is complete 
                    ProcInst markCompleteWork = new ProcInst(schedulerMaster.GetProcInfo(wcfServiceCompleteProcId));
                    markCompleteWork.objectId = startClusterObjectId;
                    markCompleteWork.procContext = startingWork.procContext;
                    string markCompleteCallbackId = workMgr.AddCallback(markCompleteWork);
                    // make our procContext call the service complete procContext when done
                    startingWork.callbackId = markCompleteCallbackId;

                    // Lock the monitor.
                    if (!workMgr.globalContext.EnterMonitor(monitorName, 10))
                    {
                        throw new Exception("Unexpected, cannot grab wcf_service_complete_monitor:" + monitorName);
                    }

                    // Queue up the procInst we need to process
                    logger.LogDebug("Calling entity");
                    workMgr.ProduceStackWork(startingWork);
                    logger.LogDebug("Waiting for results");

                    // When the procInst is complete, the procNameSyntax will unlock us and we will
                    // be able get the monitor again.
                    workMgr.globalContext.EnterMonitor(monitorName, 10);

                    // RemoveSpecificItem the monitor now that we no longer need it.
                    workMgr.globalContext.RemoveMonitor(monitorName);

                    // Start with a fresh output result set.
                    mvmEntities = new List<MetraTech.DomainModel.MvmTypes.MvmBaseObject>();

                    StringArray dynamicEntitiesKey = new StringArray(startingWork.objectId);
                    if (!dynamicEntities.index.ContainsKey(dynamicEntitiesKey))
                    {
                        string errMsg = "Error, looks like your config cleared DYNAMIC_ENTITIES for static_oid=[" + startingWork.objectId + "]";
                        logger.LogError(errMsg);
                        throw new Exception(errMsg);
                    }
                    

                    // Turn dynamic entites back into mvmEntities
                    logger.LogDebug("Copying results back into the entity");
                    entityNo = 1;
                    foreach (string[] valarray in dynamicEntities.index[dynamicEntitiesKey])
                    {
                        string dynamicOid = valarray[1];
                        logger.LogDebug("Entity #" + (entityNo++) + " (dynamic_oid=" + dynamicOid + "):");
                        using (IObjectData entityObjectData = workMgr.objectCache.CheckOut(dynamicOid))
                        {
                            // check for an entity error
                            string entityError = entityObjectData["entity_error"];
                            if(!entityError.Equals("")){
                                if (entityError.StartsWith("[warn]"))
                                {
                                    //string msg = entityError.Substring(6);
                                    throw new MvmValidationWarning(entityError);
                                }
                                else if (entityError.StartsWith("[error]"))
                                {
                                    //string msg = entityError.Substring(7);
                                    throw new MvmValidationError(entityError);
                                }
                                else
                                {
                                    throw new MvmValidationError("[error]"+entityError);
                                }
                            }


                            //logger.LogDebug("again... (dynamic_oid=" + entityObjectData.objectIdSyntax + "):");
                            // need to instanciate a new MetraTech.DomainModel.MvmTypes.MvmBaseObject (but specific)
                            MetraTech.DomainModel.MvmTypes.MvmBaseObject mvmEntity = entityType.GetConstructor(prms).Invoke(args) as MetraTech.DomainModel.MvmTypes.MvmBaseObject;
                            foreach (var pi in mvmEntity.GetProperties())
                            {
                                if (!IsRealField(pi) || IsDirtyFlag(pi)) continue;
                                string name = pi.Name.ToLower();
                                string value = entityObjectData[name];
                                logger.LogDebug("set mvmEntity." + pi.Name + "=[" + value + "], pi.PropertyType=" + pi.PropertyType.ToString());
                                object propertyValue = GetDomainProperty(pi, value);
                                mvmEntity.SetValue(pi, propertyValue);
                            }
                            mvmEntity.ResetDirtyFlag();
                            mvmEntities.Add(mvmEntity);
                        }
                    }
                    logger.LogDebug("Done copying results back into the entity");
                }
                catch (Exception e)
                {
                    string msg = "Exception in MvmActivityService: " + GetStackTraceRecursive(e);
                    logger.LogDebug(msg);
                    logger.LogException(msg, e);
                    throw (e);
                }
                finally
                {
                    if (startClusterObjectId != null)
                    {
                        logger.LogDebug("Cleaning up mvm objects");
                        // no matter what happens try to RemoveSpecificItem the cluster and all the 
                        // objects that were created.
                        using (Cluster cluster = workMgr.clusterCache.CheckOut(startClusterObjectId))
                        {
                            foreach (string memberOid in cluster.objectIds.Keys)
                            {
                                //logger.LogDebug("Try removing object " + memberOid);
                                if (workMgr.objectCache.objects.ContainsKey(memberOid))
                                {
                                    workMgr.objectCache.RemoveObjectData(memberOid);
                                    //logger.LogDebug("removed " + memberOid);
                                }
                                else
                                {
                                    //logger.LogDebug("object not in cache " + memberOid);
                                }
                            }
                        }
                        workMgr.clusterCache.RemoveCluster(startClusterObjectId);
                    }
                    logger.LogDebug("Clearing dynamic entities");
                    dynamicEntities.index.Clear();
                }
                logger.LogDebug("Successfully exiting service call");
            }
            catch (MvmValidationException e){
                throw(e);
            }
            catch (Exception e)
            {
                logger.LogException("Mvm threw exception ["+e.Message+"]",e);
                logger.LogError("Reloading mvm...");
                this.Reload();
            }
        }
        
        private void LogMvmEntities(List<MvmBaseObject> mvmEntities)
        {
            // For debugging log everything that was passed in
            logger.LogDebug("Logging the passed entities:");
            for (int i = 0; i < mvmEntities.Count; i++)
            {
                int entityNo = i + 1;
                var mvmEntity = mvmEntities[i];
                logger.LogDebug("Entity #" + entityNo + ":");
                foreach (var p in mvmEntity.GetProperties())
                {
                    var name = p.Name;
                    var val = mvmEntity.GetValue(p);
                    logger.LogDebug(name + "=[" + (val != null ? val.ToString() : "null") + "], type=" + p.PropertyType);
                }
            }
            logger.LogDebug("Done - Logging the passed entities");
        }


        

        public string GetStackTraceRecursive(Exception e)
        {
            string output = "";
            if (e.InnerException != null)
            {
                output.AppendLine(GetStackTraceRecursive(e.InnerException));
            }
            output.AppendLine("msg:" + e.Message+" "+e.StackTrace);
            return output;
        }

        private static bool IsRealField(PropertyInfo pi)
        {
            bool output = true;
            var name = pi.Name;
            if (name.StartsWith("Is") && name.EndsWith("Dirty")) output = false;
            else if (name.EndsWith("DisplayName")) output = false;
            else if (name.EndsWith("ExtensionData")) output = false;
            return output;
        }

        public static bool IsDirtyFlag(PropertyInfo pi)
        {
            bool output = false;
            var name = pi.Name;
            if (name.StartsWith("Is") && name.EndsWith("Dirty")) output = true;
            return output;
        }

        // Returns true if type is a nullable type
        public static bool IsNullableType(Type theType)
        {
            return (theType.IsGenericType && theType.
              GetGenericTypeDefinition().Equals
              (typeof(Nullable<>)));
        }

        /// <summary>
        /// Converts mvm string representation into domain model property
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object GetDomainProperty(PropertyInfo pi, string value)
        {
            
            object output=null;
            if (IsNullableType(pi.PropertyType)&&(value == null || value.Equals("")))
            {
                output=null;
            }
            else if (pi.PropertyType.Equals(typeof(System.String)))
            {
                output = value;
            }
            else if (pi.PropertyType.Equals(typeof(System.Nullable<System.DateTime>)))
            {
                DateTime val = DateTime.Parse(value);
                output = new System.Nullable<System.DateTime>(val);
            }
            else if (pi.PropertyType.Equals(typeof(System.Nullable<int>)))
            {
                int val = int.Parse(value);
                output = new System.Nullable<int>(val);
            }
            else if (pi.PropertyType.Equals(typeof(System.Nullable<decimal>)))
            {
                decimal val = decimal.Parse(value);
                output = new System.Nullable<decimal>(val);
            }
            else if (pi.PropertyType.Equals(typeof(System.Nullable<double>)))
            {
                double val = double.Parse(value);
                output = new System.Nullable<double>(val);
            }
            else if (pi.PropertyType.Equals(typeof(System.Nullable<float>)))
            {
                float val = float.Parse(value);
                output = new System.Nullable<float>(val);
            }
            else if (pi.PropertyType.Equals(typeof(System.Nullable<bool>)))
            {
                bool val = value.Equals("1");
                output = new System.Nullable<bool>(val);
            }
            else if (pi.PropertyType.Equals(typeof(System.Boolean)))
            {
                Boolean val = value.Equals("1");
                output = val;
            }
            else
            {
                throw new Exception("Unexpected parameter type: name=" + pi.Name + ", type=" + pi.PropertyType.ToString()); ;
            }
            return output;
        }

        /// <summary>
        /// Converts domain model property into mvm string representation
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetMvmString(PropertyInfo pi, object value)
        {
            //logger.LogDebug("GetMvmString for " + pi.Name + ",type=" + pi.PropertyType.ToString()+",dttype="+typeof(System.Nullable<System.DateTime>).ToString());
            string output = "";
            if (value == null)
            {
                output = "";
            }
            else if (pi.PropertyType.Equals(typeof(System.Nullable<System.DateTime>)))
            {
                var val = ((System.Nullable<System.DateTime>)value).Value;
                output = val.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            else if (pi.PropertyType.Equals(typeof(System.Nullable<int>)))
            {
                var val = ((System.Nullable<int>)value).Value;
                output = val.ToString();
            }
            else if (pi.PropertyType.Equals(typeof(System.Nullable<decimal>)))
            {
                var val = ((System.Nullable<decimal>)value).Value;
                output = val.ToString();
            }
            else if (pi.PropertyType.Equals(typeof(System.Nullable<double>)))
            {
                var val = ((System.Nullable<double>)value).Value;
                output = val.ToString();
            }
            else if (pi.PropertyType.Equals(typeof(System.Nullable<float>)))
            {
                var val = ((System.Nullable<float>)value).Value;
                output = val.ToString();
            }
            else if (pi.PropertyType.Equals(typeof(System.Nullable<bool>)))
            {
                bool val = ((System.Nullable<bool>)value).Value;
                output = val ? "1":"0";
            }
            else if (pi.PropertyType.Equals(typeof(System.Boolean)))
            {
                Boolean val = (System.Boolean)value;
                output = val ? "1" : "0";
            }
            else
            {
                output = value.ToString();
            }
            return output;
        }

    }
}