using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MGetProcName: IModuleSetup,IModuleRun
    {
        private string valueSyntax;
        private IWriteString valueParsed;
        private string localName;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MGetProcName m = new MGetProcName();
            m.valueSyntax = me.InnerText;
            m.valueParsed = mc.ParseWritableSyntax(m.valueSyntax);

            // the procname does not change from the point of view of a newModule so
            // we can pay the price to search the callstack 1 time.
            m.localName = mc.workMgr.GetEntryPointWork(mc.procInst).procInfo.localName;
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            this.valueParsed.Write(mc, this.localName);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("get_proc_name:" + this.valueSyntax);
        }
    }
}
