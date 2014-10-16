using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MGetWorkerNo: IModuleSetup,IModuleRun
    {

        private string valueSyntax;
        private IWriteString valueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MGetWorkerNo m = new MGetWorkerNo();
            // xml extraction
            m.valueSyntax = me.InnerText;
            // parsing
            m.valueParsed = mc.ParseWritableSyntax(m.valueSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {

            this.valueParsed.Write(mc, mc.worker.GetWorkerNoString());
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("get_worker_no:" + this.valueSyntax);
        }
    }
}
