using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    class MProcInstDelete:IModuleSetup,IModuleRun
    {
        private string procInstIdSyntax;
        private IReadString procInstIdParsed;
        #region IModuleSetup Members

        public void Setup(System.Xml.XmlElement moduleElement, ModuleContext mc, List<IModuleRun> runModules)
        {
            MProcInstDelete m = new MProcInstDelete();
            m.procInstIdSyntax = moduleElement.InnerText;
            m.procInstIdParsed = mc.ParseSyntax(m.procInstIdSyntax);
            runModules.Add(m);
        }

        #endregion

        #region IModuleRun Members

        public void Run(ModuleContext mc)
        {
            string procInstId = this.procInstIdParsed.Read(mc);
            //mc.mvm.Log("MProcInstDelete("+procInstId+")");
            mc.ProcInstDelete(procInstId.ToLong());
        }

        #endregion
    }
}
