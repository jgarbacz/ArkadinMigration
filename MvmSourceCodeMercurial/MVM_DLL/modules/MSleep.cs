using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MSleep: IModuleSetup,IModuleRun
    {

        private string valueSyntax;
        private IReadString valueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MSleep m = new MSleep();
            // xml extraction
            m.valueSyntax = me.InnerText;
            // parsing
            m.valueParsed = mc.ParseSyntax(m.valueSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string val=this.valueParsed.Read(mc);
            int ms = int.Parse(val);
            System.Threading.Thread.Sleep(ms);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("sleep:" + this.valueSyntax);
        }
    }
}
