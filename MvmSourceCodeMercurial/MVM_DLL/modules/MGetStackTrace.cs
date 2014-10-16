using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MGetStackTrace: IModuleSetup,IModuleRun
    {
        private string valueSyntax;
        private IWriteString valueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MGetStackTrace m = new MGetStackTrace();
            m.valueSyntax = me.InnerText;
            m.valueParsed = mc.ParseWritableSyntax(m.valueSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string value=mc.worker.MyStackTrace;
            this.valueParsed.Write(mc, value);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("get_stack_trace:" + this.valueSyntax);
        }
    }
}
