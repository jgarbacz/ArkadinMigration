using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MRemoveNamedClass: IModuleSetup,IModuleRun
    {

        private string valueSyntax;
        private IReadString valueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MRemoveNamedClass m = new MRemoveNamedClass();
            // xml extraction
            m.valueSyntax = me.InnerText;
            // parsing
            m.valueParsed = mc.ParseSyntax(m.valueSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string name=this.valueParsed.Read(mc);
            mc.globalContext.RmNamedClassInst(name);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("do:" + this.valueSyntax);
        }
    }
}
