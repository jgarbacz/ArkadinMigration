using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MVM
{
    class MGotoLabel : IModuleSetup, IModuleRun
    {
        private string labelSyntax;
        private string label;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MGotoLabel m = new MGotoLabel();
            m.labelSyntax = me.InnerText;
            m.label = mc.SyntaxReadString(m.labelSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            //Console.WriteLine("GOTO LABEL:" + this.label);
            mc.moduleStatus = ModuleStatus.GotoModuleIdx;
            mc.gotoModuleIdx = mc.procDefinition.GetLabelModuleIdx(label);
        }
    }
}
