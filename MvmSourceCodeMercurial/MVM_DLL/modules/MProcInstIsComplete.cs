using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    class MProcInstIsComplete:IModuleSetup,IModuleRun
    {
        private string procInstSyntax;
        private IReadString procInstParsed;
        private string isCompleteSyntax;
        private IWriteString isCompleteParsed;
        #region IModuleSetup Members

        public void Setup(System.Xml.XmlElement me, ModuleContext mc, List<IModuleRun> runModules)
        {
            MProcInstIsComplete m = new MProcInstIsComplete();
            m.procInstSyntax = me.SelectNodeInnerText("proc_inst_id");
            m.procInstParsed = mc.ParseSyntax(m.procInstSyntax);
            m.isCompleteSyntax = me.SelectNodeInnerText("is_complete");
            m.isCompleteParsed = mc.ParseWritableSyntax(m.isCompleteSyntax);
            runModules.Add(m);
        }

        #endregion

        #region IModuleRun Members

        public void Run(ModuleContext mc)
        {
            long procInstId = this.procInstParsed.Read(mc).ToLong();
            this.isCompleteParsed.Write(mc, mc.workMgr.CallbackIsComplete(procInstId) ? "1":"0");
        }
        #endregion
    }
}
