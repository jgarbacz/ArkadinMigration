using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MVM
{
    /*
    <stopwatch name='name' ms='TEMP.ms' total_ms='GLOBAL.total_ms'>
        : newModules...
    </stopwatch>
     */
    class MStopwatch: IModuleSetup
    {
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MStopwatch m = new MStopwatch();
            string stopwatchName = me.GetAttributeDefault("name", mc.GetGenSym("stopwatch"));
            string msTarget = me.GetAttribute("ms");
            string totalMsTarget = me.GetAttribute("total_ms");
            string totalCountTarget = me.GetAttribute("counter");
            string totalCountIncrTarget = me.GetAttributeDefaulted("counter_increment","1");
            string totalSkipFirstTarget = me.GetAttributeDefaulted("skip_first", "0");
            string stopwatchStart = "<stopwatch_start name=" + stopwatchName.q()+"/>";
            string stopwatchStop = "<stopwatch_stop name=" + stopwatchName.q() + " ms=" + msTarget.q() + " total_ms=" + totalMsTarget.q() + " counter=" + totalCountTarget.q() + " counter_increment=" + totalCountIncrTarget.q() + " skip_first=" + totalSkipFirstTarget.q() + " />";
            SchedulerMaster sm = mc.scheduler.schedulerMaster;
            run.Add(sm.GetModuleRun(stopwatchStart));
            foreach (XmlElement module in me.GetChildElems())
            {
                run.Add(sm.GetModuleRun(module));
            }
            run.Add(sm.GetModuleRun(stopwatchStop));
        }
    }
}
