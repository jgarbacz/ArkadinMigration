using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MGetDate: IModuleSetup,IModuleRun
    {

        private string valueSyntax;
        private IWriteString valueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MGetDate m = new MGetDate();
            // xml extraction
            m.valueSyntax = me.InnerText;
            // parsing
            m.valueParsed = mc.ParseWritableSyntax(m.valueSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string d = mc.Now();
            this.valueParsed.Write(mc, d);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("get_date:" + this.valueSyntax);
        }
    }
}
