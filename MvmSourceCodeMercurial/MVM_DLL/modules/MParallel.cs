using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

//
// Parallel processes its child newModules
// <parallel>
//      <somemodule/>
//      <somemodule/>
// </parallel>
// 
// Wraps each newModule in a procContext
// Produces procInst for each procContext with the current object
//
namespace MVM
{
    class MParallel: IModuleSetup,IModuleRun
    {
        public List<int> childProcIds = new List<int>();
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            //Console.WriteLine("MPARALLEL_SETUP");
            MParallel m = new MParallel();
            int procCtr = 0;
            foreach (XmlElement elem in me.GetChildElems())
            {
                //string childProcName = mc.procDefinition.initNamespaceProcName + "/" + "parallel" + "[" + mc.moduleOrder + "]-" + procCtr++;
                string childProcName = mc.GetChildProcName("parallel" + procCtr++);
                string procString = "<proc name='" + childProcName + "'>" + elem.OuterXml + "</proc>";
                mc.ReadXmlConfigFromString(procString);
                int childProcId = mc.GetProcId(childProcName);
                m.childProcIds.Add(childProcId);
            }
            if(m.childProcIds.Count>0) run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string threadNo = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();

            //Console.WriteLine("MPARALLEL_RUN-1-"+threadNo);
            // get a procNameSyntax to here.
            // Advance to the next newModule
            mc.procInst.nextModuleOrder = mc.procDefinition.GetNextModuleOrder(mc.moduleIdx);
            //Console.WriteLine("MPARALLEL_RUN-2-" +threadNo+":"+ mc.procInst.nextModuleOrder.ToString());
            string callback = mc.workMgr.CreateCallback(mc.procInst).ToString();
            //Console.WriteLine("MPARALLEL_RUN-3-" + threadNo + ":" + procNameSyntax);
            mc.Yield();
            //Console.WriteLine("MPARALLEL_RUN-4-" + threadNo );
           
            // create the ctr procNameSyntax object
            string ctrOid=mc.Spawn("CTR_CALLBACK");
            //Console.WriteLine("MPARALLEL_RUN-5-" + threadNo + ":" + ctrOid);
            
            mc.WriteObjectField(ctrOid, "ctr", this.childProcIds.Count.ToString());
            //Console.WriteLine("MPARALLEL_RUN-6-" + threadNo + ":" + this.childProcIds.Count.ToString());
            mc.WriteObjectField(ctrOid, "callback", callback);
            //Console.WriteLine("MPARALLEL_RUN-7-" + threadNo + ":" + procNameSyntax);
            
            // get the id for the ctr_callback_proc
            int callbackProcId = mc.GetProcId("ctr_callback");
            //Console.WriteLine("MPARALLEL_RUN-8-" + threadNo + ":" + callbackProcId);
            
            // queue up the procInst'
            foreach (int procId in this.childProcIds)
            {
                //Console.WriteLine("MPARALLEL_RUN-9-" + threadNo + ":" + (++xx).ToString()+ "-"+ procId);
            
                mc.QueueProcForCurrentObjectNestedWithCb(procId, callbackProcId, ctrOid);
            }
            //Console.WriteLine("MPARALLEL_RUN-10-" + threadNo);
            
        }


        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("parallel:");
        }
    }
}
