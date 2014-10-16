using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MConsole: IModuleSetup,IModuleRun
    {
        private string valueSyntax;
        private IReadString valueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MConsole m = new MConsole();
            // xml extraction
            m.valueSyntax = me.InnerText;
            // parsing
            m.valueParsed = mc.ParseSyntax(m.valueSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            Console.WriteLine(this.valueParsed.Read(mc));
        }
    }
}
