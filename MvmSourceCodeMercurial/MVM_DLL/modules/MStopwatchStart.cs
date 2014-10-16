using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    class MStopwatchStart:IModuleSetup,IModuleRun
    {
        private string swName;
        private string swGlobalName;
        public void Setup(System.Xml.XmlElement me, ModuleContext mc,List<IModuleRun> runModules)
        {
            MStopwatchStart m = new MStopwatchStart();
            m.swName = me.GetAttribute("name");
            m.swGlobalName = "SplitStopWatch:" + m.swName;
            mc.globalContext.namedClassInstMap.AddIfNull(m.swGlobalName, new SplitStopwatch());
            runModules.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            SplitStopwatch sw=mc.globalContext.GetNamedClassInst(this.swGlobalName) as SplitStopwatch;
            sw.Start();
        }
    }
}
