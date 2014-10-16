using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MGetFullProcName: IModuleSetup,IModuleRun
    {
        private string valueSyntax;
        private IWriteString valueParsed;
        private string fullProcName;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MGetFullProcName m = new MGetFullProcName();
            m.valueSyntax = me.InnerText;
            m.valueParsed = mc.ParseWritableSyntax(m.valueSyntax);

            // the procname does not change from the point of view of a newModule so
            // we can pay the price to search the callstack 1 time.
            m.fullProcName = mc.workMgr.GetEntryPointWork(mc.procInst).procInfo.procName;
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            this.valueParsed.Write(mc, this.fullProcName);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("get_full_proc_name:" + this.valueSyntax);
        }
    }
}
