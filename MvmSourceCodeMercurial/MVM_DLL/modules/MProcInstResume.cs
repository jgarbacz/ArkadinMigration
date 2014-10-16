using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    class MProcInstResume:IModuleSetup,IModuleRun
    {
        private string procInstIdSyntax;
        private IReadString procInstIdParsed;
        #region IModuleSetup Members

        public void Setup(System.Xml.XmlElement moduleElement, ModuleContext mc, List<IModuleRun> runModules)
        {
            MProcInstResume m = new MProcInstResume();
            m.procInstIdSyntax = moduleElement.InnerText;
            m.procInstIdParsed = mc.ParseSyntax(m.procInstIdSyntax);
            runModules.Add(m);
        }

        #endregion

        #region IModuleRun Members

        public void Run(ModuleContext mc)
        {
            string procInstIdSyntaxId = this.procInstIdParsed.Read(mc);
            mc.workMgr.PulseCallback(mc.procInst, procInstIdSyntaxId.ToLong());
            mc.procInst.nextModuleOrder = mc.procDefinition.GetNextModuleOrder(mc.moduleIdx);
            mc.moduleStatus = ModuleStatus.Yield;
        }

        #endregion
    }
}
