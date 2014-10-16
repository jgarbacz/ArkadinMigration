using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MGetProcNameSpace: IModuleSetup,IModuleRun
    {
        private string valueSyntax;
        private IWriteString valueParsed;
        private string nameSpace;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MGetProcNameSpace m = new MGetProcNameSpace();
            m.valueSyntax = me.InnerText;
            m.valueParsed = mc.ParseWritableSyntax(m.valueSyntax);

            // the procname does not change from the point of view of a newModule so
            // we can pay the price to search the callstack 1 time.
            m.nameSpace = mc.workMgr.GetEntryPointWork(mc.procInst).procInfo.nameSpace;
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            this.valueParsed.Write(mc, this.nameSpace);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("get_proc_namespace:" + this.valueSyntax);
        }
    }
}
