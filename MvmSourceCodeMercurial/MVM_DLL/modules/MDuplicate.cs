using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MVM
{
    /*
    <duplicate quantity='50'>
        : newModules...
    </duplicate>
     */
    class MDuplicate: IModuleSetup
    {
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDuplicate m = new MDuplicate();
            string quantityStr = me.GetAttributeDefaulted("quantity","100");
            SchedulerMaster sm = mc.scheduler.schedulerMaster;
            for (int i = 0; i < quantityStr.ToInt(); i++)
            {
                foreach (XmlElement module in me.GetChildElems())
                {
                    run.Add(sm.GetModuleRun(module));
                }
            }
        }
    }
}
