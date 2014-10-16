using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Microsoft.CSharp;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;

namespace MVM
{

    /*
# expose specific procs as services
<expose_wcf_services>
<procContext>'blah'</procContext>
<procContext>'blow'</procContext>
<generate_host>true/false</generate_host>
<self_host>true/false</self_host>
<binding>'http'</binding>
</expose_wcf_services>

# expose all matching procs services
<expose_wcf_services>
<generate_host>true/false</generate_host>
<self_host>true/false</self_host>
<proc_regex>'.*_entity_get'</proc_regex>
<binding>'pipe'</binding>
</expose_wcf_services>

# expose all procs with a certain attribute
<expose_wcf_services>
<proc_attribute>'is_service'</proc_attribute>
<self_host>true/false</self_host>
<binding>'http'</binding>
<generate_host>true/false</generate_host>
</expose_wcf_services>
      */

    public interface IStartServiceHost
    {
        ServiceHost StartServiceHost(ModuleContext mc);
    }

    public class MHostWcfServices : IModuleSetup, IModuleRun
    {
        // from xml
        private List<string> procSyntax;
        private List<IReadString> procParsed;
        private string uriSyntax;
        private IReadString uriParsed;
        private string endPointSyntax;
        private IReadString endPointParsed;
        private string genDirSyntax;
        private IReadString genDirParsed;
        private string hostNameSyntax;
        private IReadString hostNameParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MHostWcfServices m = new MHostWcfServices();
            // xml extraction
            m.procSyntax = me.SelectNodesInnerText("./proc_name");
            m.procParsed = mc.ParseSyntax(m.procSyntax);
            m.hostNameSyntax = me.SelectNodeInnerText("./host_name");
            m.hostNameParsed = mc.ParseSyntax(m.hostNameSyntax);
            m.uriSyntax = me.SelectNodeInnerText("./base_address");
            m.uriParsed = mc.ParseSyntax(m.uriSyntax);
            m.endPointSyntax = me.SelectNodeInnerText("./end_point");
            m.endPointParsed = mc.ParseSyntax(m.endPointSyntax);
            m.genDirSyntax = me.SelectNodeInnerText("./generate_dir");
            m.genDirParsed = mc.ParseSyntax(m.genDirSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string uri = this.uriParsed.Read(mc);
            string genDir = this.genDirParsed.Read(mc);
            string hostName = this.hostNameParsed.Read(mc);
            string endPoint = this.endPointParsed.Read(mc);

            List<string> procs = new List<string>();
            foreach (var parsed in this.procParsed)
            {
                string proc = parsed.Read(mc);
                procs.Add(proc);
            }
            // pull out dupes
            procs = procs.GetDistinct();

            // build up setting the inputs
            Dictionary<string, List<string>> setInputs = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> setOutputs = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> defaultSetOutputs = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> procMethodParams = new Dictionary<string, List<string>>();
            Dictionary<string, ProcInfo> procInfo = new Dictionary<string, ProcInfo>();
            foreach (string proc in procs)
            {
                procInfo[proc] = mc.GetProcInfo(proc);
                setInputs[proc] = new List<string>();
                setOutputs[proc] = new List<string>();
                defaultSetOutputs[proc] = new List<string>();
                procMethodParams[proc] = new List<string>();
                foreach (XmlElement pelem in procInfo[proc].paramElems)
                {
                    string name = pelem.GetAttribute("name");
                    string mode = pelem.GetAttributeDefaulted("mode", "in");
                    if (mode.Equals("in"))
                    {
                        procMethodParams[proc].Add("string " + name);
                        setInputs[proc].Add("tempContext[" + name.qq() + "]=" + name + ";");
                    }
                    else if (mode.Equals("in out"))
                    {
                        procMethodParams[proc].Add("ref string " + name);
                        setInputs[proc].Add("tempContext[" + name.qq() + "]=" + name + ";");
                        setOutputs[proc].Add(name + "=tempContext[" + name.qq() + "];");
                    }
                    else if (mode.Equals("out"))
                    {
                        procMethodParams[proc].Add("out string " + name);
                        defaultSetOutputs[proc].Add(name + @"=null;");
                        setOutputs[proc].Add(name + "=tempContext[" + name.qq() + "];");
                    }
                    else
                    {
                        throw new Exception("invalid mode:" + mode);
                    }
                }
            }





            string wcfHostClass = "WcfHost_" + hostName;
            string wcfServiceInterface = "IWcfSvc_" + hostName;
            string wcfServiceClass = "WcfSvc_" + hostName;

            // generate a method for the procContext with the same name as the procContext and 
            // string args having same direction as the procContext.
            StringBuilder src = new StringBuilder();
            src.Append(
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
namespace MVM
{
    public class " + wcfHostClass + @":IStartServiceHost
    {
        public ServiceHost StartServiceHost(ModuleContext mc)
        {
            Uri baseAddress = new Uri(""" + uri + @""");
            ServiceHost selfHost = new ServiceHost(new " + wcfServiceClass + @"(mc), baseAddress);
            try
            {
                selfHost.AddServiceEndpoint(typeof(" + wcfServiceInterface + @"),new WSHttpBinding(),""" + endPoint + @""");
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                selfHost.Description.Behaviors.Add(smb);
                selfHost.Open();
                Console.WriteLine(""The service is ready:""+baseAddress);
                return selfHost;
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine(""An exception occurred: {0}"", ce.Message);
                selfHost.Abort();
            }
            return null;
        }
    }
    [ServiceContract(Namespace = ""MVM"")]
    public interface " + wcfServiceInterface + @"
    {
");
            foreach (string proc in procs)
            {
                src.Append(@"
        [OperationContract]
        string " + proc + @"(" + procMethodParams[proc].Join(",") + @");
            ");
            }
            src.Append(@"
    }
    [System.ServiceModel.ServiceBehavior(InstanceContextMode=System.ServiceModel.InstanceContextMode.Single)]
    public class " + wcfServiceClass + @":" + wcfServiceInterface + @"
    {
        public WorkMgr workMgr;
");
            foreach (string proc in procs)
            {
                src.Append(@"
         public int " + proc + @"_ProcId;
            ");
            }
            src.Append(@"
        public int wcfServiceCompleteProcId;
        public " + wcfServiceClass + @"(ModuleContext mc){
  ");
            foreach (string proc in procs)
            {
                src.Append(@"
         this." + proc + @"_ProcId= mc.GetProcId(" + proc.qq() + @");
            ");
            }
            src.Append(@"

            this.wcfServiceCompleteProcId= mc.GetProcId(""wcf_service_complete"");
            this.workMgr=mc.workMgr;
        }
");
            foreach (string proc in procs)
            {
                src.Append(@"
        public string " + proc + @"(" + procMethodParams[proc].Join(",") + @"){
            string startClusterObjectId=null;
            try{

                // create the starting work 
                startClusterObjectId = workMgr.clusterCache.CreateCluster();
                Work startingWork = new Work();
                startingWork.clusterObjectId = startClusterObjectId;
                startingWork.procId = " + proc + @"_ProcId;
                startingWork.nextModuleOrder = workMgr.schedulerMaster.GetScheduler().GetProcDefinition(startingWork.procId).GetFirstOrder();
                startingWork.objectId = startClusterObjectId;
                startingWork.procContext = new ProcContext();

                // set the proc inputs 
                var tempContext=startingWork.procContext.tempContext;
                " + setInputs[proc].Join("".AppendLine()) + @"

                // set the name of the monitor on the cluster  
                // wcf_service_complete_monitor
                string monitorName=""wcf_service_complete_monitor_"" + startClusterObjectId;
                using (ObjectData obj = workMgr.objectCache.CheckOut(startClusterObjectId))
                {
                    obj[""wcf_service_complete_monitor""] = monitorName;
                }             

                // create callback work that is called when the service proc is complete 
                Work markCompleteWork = new Work();
                markCompleteWork.procId = this.wcfServiceCompleteProcId;
                markCompleteWork.objectId = startClusterObjectId;
                markCompleteWork.clusterObjectId = startClusterObjectId;
                markCompleteWork.procContext = startingWork.procContext;
                string markCompleteCallbackId = workMgr.AddCallback(markCompleteWork);
                // make our proc call the service complete proc when done
                startingWork.callbackId = markCompleteCallbackId;        

                // Lock the monitor.
                if(!workMgr.globalContext.EnterMonitor(monitorName,10)){
                    throw new Exception(""unexpected,cannot grab wcf_service_complete_monitor"");
                }

                // Queue up the work we need to process
                workMgr.ProduceStackWork(startingWork);

                // When the work is complete, the callback will unlock us and we will
                // be able get the monitor again.
                workMgr.globalContext.EnterMonitor(monitorName,10);

                // remove the monitor now that we no longer need it.
                workMgr.globalContext.RemoveMonitor(monitorName);

                // now we are safe to grab the outputs
                " + setOutputs[proc].Join("".AppendLine()) + @"
            } catch (Exception e)
            {
            " + defaultSetOutputs[proc].Join("".AppendLine()) + @"
                return e.Message;
            } finally {
                if(startClusterObjectId!=null){
                    // no matter what happens try to remove the cluster and all the 
                    // objects that were created.
                    using (Cluster cluster = workMgr.clusterCache.CheckOut(startClusterObjectId))
                    {
                        foreach (string memberOid in cluster.objectIds.Keys)
                        {
                            workMgr.objectCache.RemoveObjectData(memberOid);
                        }
                    }
                    workMgr.clusterCache.RemoveCluster(startClusterObjectId);
                }
            }
            return ""success"";
        }");
            }
            src.Append(@"

    }
}
");
            // write out the dll and compile it
            var codeFile = Path.Combine(genDir, "expose_wcf_service.cs");
            var assemblyFile = Path.Combine(genDir, "expose_wcf_service.dll");
            src.ToString().WriteToFile(codeFile);
            string[] references = new string[]{
                "System.dll",
                "System.Core.dll",
                "System.Data.dll",
                "System.Data.DataSetExtensions.dll",
                "System.ServiceModel.dll",
                "ade_lib.dll"
            };
            if (!CodeGen.CompileDll(codeFile, references, assemblyFile))
            {
                throw new Exception("Error cannot compile codeFile:" + codeFile);
            }

            // load the dll
            Assembly assembly = Assembly.LoadFrom(assemblyFile);
            string hostTypeName = "MVM." + wcfHostClass;

            // create and start the service host
            IStartServiceHost serviceHostCreater = (IStartServiceHost)assembly.CreateInstance(hostTypeName);
            ServiceHost serviceHost = serviceHostCreater.StartServiceHost(mc);

            // store the host in globalContext so someone can shut it down by name.
            try
            {
                mc.globalContext.SetNamedClassInst(hostName, serviceHost);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot define wcf service host [" + 
                    hostName + 
                    "] because this name is already in use: " +
                    e.Message);
            }

            // register the service with procInst manager so we will not exit 
            // until process is killed, service shuts itself down, or we run <shutdown_service>
            Interlocked.Increment(ref mc.workMgr.serviceCount);
        }




        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("expose_wcf:");
        }

        public static void GenWcf()
        {
        }


    }
}
