using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;

namespace MVM
{
    class MReplace: IModuleSetup,IModuleRun
    {
        private string inputSyntax;
        private IReadString inputParsed;
        private string oldValueSyntax;
        private IReadString oldValueParsed;
        private string newValueSyntax;
        private IReadString newValueParsed;
        private string outputSyntax;
        private IWriteString outputParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MReplace m = new MReplace();
            // xml extraction
            m.inputSyntax = me.SelectNodeInnerText("input");
            m.oldValueSyntax = me.SelectNodeInnerText("old_value");
            m.newValueSyntax = me.SelectNodeInnerText("new_value");
            m.outputSyntax = me.SelectNodeInnerText("output");
            // parsing
            m.inputParsed = mc.ParseSyntax(m.inputSyntax);
            m.oldValueParsed = mc.ParseSyntax(m.oldValueSyntax);
            m.newValueParsed = mc.ParseSyntax(m.newValueSyntax);
            m.outputParsed = mc.ParseWritableSyntax(m.outputSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string input=this.inputParsed.Read(mc);
            string oldValue = this.oldValueParsed.Read(mc);
            string newValue = this.newValueParsed.Read(mc);
            string output = input.Replace(oldValue, newValue);
            this.outputParsed.Write(mc, output);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("replace:");
        }
    }
}
