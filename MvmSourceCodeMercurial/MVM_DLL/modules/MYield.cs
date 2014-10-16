using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    class MYield:IModuleSetup,IModuleRun
    {
      
        #region IModuleSetup Members

        public void Setup(System.Xml.XmlElement moduleElement, ModuleContext mc, List<IModuleRun> runModules)
        {
            MYield m = new MYield();
           
            runModules.Add(m);
        }

        #endregion

        #region IModuleRun Members

        public void Run(ModuleContext mc)
        {
            ProcInst entryProcInst=mc.workMgr.GetEntryPointWork(mc.procInst); // can cache this on the current procInfo!
          // if the entry proc is not the same as the current proc, we need to tell the entry proc
          // to resume the current proc. Otherwise we do not need to do this.
            if (!entryProcInst.Equals(mc.procInst))
            {
            entryProcInst.ResumeProcInst=mc.procInst;
            }
            mc.workMgr.FireCallback(entryProcInst.callbackId);
            mc.procInst.nextModuleOrder = mc.procDefinition.GetNextModuleOrder(mc.moduleIdx);
            mc.moduleStatus = ModuleStatus.Yield;
        }
        #endregion
    }
}
