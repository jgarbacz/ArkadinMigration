using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    class MStopwatchReset:IModuleSetup,IModuleRun
    {
        private string swName;
        private string swGlobalName;
        public void Setup(System.Xml.XmlElement me, ModuleContext mc,List<IModuleRun> runModules)
        {
            MStopwatchReset m = new MStopwatchReset();
            m.swName = me.GetAttribute("name");
            m.swGlobalName = "SplitStopWatch:" + m.swName;
            runModules.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            SplitStopwatch sw=mc.globalContext.GetNamedClassInst(this.swGlobalName) as SplitStopwatch;
            sw.Reset();
        }
    }
}
