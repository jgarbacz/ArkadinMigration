using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MGetMachineName: IModuleSetup,IModuleRun
    {

        private string valueSyntax;
        private IWriteString valueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MGetMachineName m = new MGetMachineName();
            // xml extraction
            m.valueSyntax = me.InnerText;
            // parsing
            m.valueParsed = mc.ParseWritableSyntax(m.valueSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string machineName = System.Environment.MachineName;
            this.valueParsed.Write(mc, machineName);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("get_machine_name:" + this.valueSyntax);
        }
    }
}
