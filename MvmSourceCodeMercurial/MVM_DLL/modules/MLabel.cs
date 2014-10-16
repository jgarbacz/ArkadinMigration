using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MVM
{
    class MLabel : IModuleSetup, IModuleRun, IModuleLabel
    {
        private string labelSyntax;
        private string label;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MLabel m = new MLabel();
            m.labelSyntax = me.InnerText;
            m.label = mc.SyntaxReadString(m.labelSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            //Console.WriteLine("AT LABEL:" + this.label);
        }

        public string GetLabel()
        {
            return this.label;
        }

    }
}
