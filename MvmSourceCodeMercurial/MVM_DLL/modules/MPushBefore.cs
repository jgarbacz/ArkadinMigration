using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    class MPushBefore:IModuleSetup
    {
        #region IModuleSetup Members

        public void Setup(System.Xml.XmlElement moduleElement, ModuleContext mc, List<IModuleRun> runModules)
        {
            string procName=moduleElement.GetAttribute("name");
            int procId = mc.GetProcId(procName);
            string explicitOrder = moduleElement.GetAttributeDefaulted("order", int.MaxValue.ToString());
            List<IModuleRun> modules = mc.mvm.procLoader.ReadXmlModules(moduleElement);
            mc.scheduler.schedulerMaster.PushBefore(modules, procId, new StringDecimal(explicitOrder));
        }

        #endregion

       
    }
}
