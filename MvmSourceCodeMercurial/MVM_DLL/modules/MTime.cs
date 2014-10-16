using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MVM
{
    /*
    <time ms='TEMP.ms'>
        : newModules...
    </time>
     */
    class MTime: IModuleSetup
    {
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            //Console.WriteLine("SETUP !");
            MTime m = new MTime();
            //string childProcName = mc.procDefinition.initNamespaceProcName + "/" + "time" + "[" + mc.moduleOrder + "]";
            //string childProcName = mc.GetChildProcName("time");
            //mc.ReadXmlProcFromElem(childProcName, me);
            string timeTarget = me.GetAttributeDefault("ms","TEMP."+mc.GetGenSym("time"));
            string totalTimeTarget = me.GetAttributeDefault("total_ms", "");
            string timeStart = "<time_start ms=" + timeTarget.q() + " total_ms="+totalTimeTarget.q()+"/>";
            //string callProc = "<call_proc_for_current_object_nested><name>" + childProcName.q() + "</name></call_proc_for_current_object_nested>";
            string timeEnd = "<time_end ms=" + timeTarget.q() + " total_ms=" + totalTimeTarget.q() + "/>";
            SchedulerMaster sm = mc.scheduler.schedulerMaster;
            run.Add(sm.GetModuleRun(timeStart));
            foreach (XmlElement module in me.GetChildElems())
            {
                run.Add(sm.GetModuleRun(module));
            }
            run.Add(sm.GetModuleRun(timeEnd));
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("time:" );
        }
    }
}
