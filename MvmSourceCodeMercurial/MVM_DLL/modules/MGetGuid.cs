using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MGetGuid: IModuleSetup,IModuleRun
    {

        private string valueSyntax;
        private IWriteString valueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MGetGuid m = new MGetGuid();
            // xml extraction
            m.valueSyntax = me.InnerText;
            // parsing
            m.valueParsed = mc.ParseWritableSyntax(m.valueSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string guid = System.Guid.NewGuid().ToString().Replace("-","");
            this.valueParsed.Write(mc, guid);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("get_guid:" + this.valueSyntax);
        }
    }
}
