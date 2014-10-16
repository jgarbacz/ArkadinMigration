using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MDo: IModuleSetup,IModuleRun
    {
        private string valueSyntax;
        private IReadString valueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MDo m = new MDo();
            // xml extraction
            m.valueSyntax = me.InnerText;
            // parsing
            m.valueParsed = mc.ParseSyntax(m.valueSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            this.valueParsed.Read(mc);
        }
    }
}
