using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MThrow : IModuleSetup, IModuleRun
    {

        private string messageSyntax;
        private IReadString messageParsed;
        private string name;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MThrow m = new MThrow();
            m.name = me.GetAttributeDefaulted("name", "user_exception");
            m.messageSyntax = me.InnerText;
            m.messageParsed = mc.ParseSyntax(m.messageSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string message = this.messageParsed.Read(mc);
            mc.ThrowException(this.name, message);
        }
    }
}
