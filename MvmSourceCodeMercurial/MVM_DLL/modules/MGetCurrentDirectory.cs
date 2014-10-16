using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MGetCurrentDirectory: IModuleSetup,IModuleRun
    {

        private string valueSyntax;
        private IWriteString valueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MGetCurrentDirectory m = new MGetCurrentDirectory();
            // xml extraction
            m.valueSyntax = me.InnerText;
            // parsing
            m.valueParsed = mc.ParseWritableSyntax(m.valueSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string d = System.IO.Directory.GetCurrentDirectory();
            this.valueParsed.Write(mc, d);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("get_working_directory:" + this.valueSyntax);
        }
    }
}
