using System;
using System.Collections.Generic;

using System.IO;
using System.Text;
using System.Xml;

namespace MVM
{
    class MGetProcText: IModuleSetup,IModuleRun
    {
        private string nameSyntax;
        private IReadString nameParsed;
        private string outputSyntax;
        private IWriteString outputParsed;
        private string successSyntax;
        private IWriteString successParsed;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MGetProcText m = new MGetProcText();
            // xml extraction
            m.nameSyntax = me.SelectNodeInnerText("./name");
            m.nameParsed = mc.ParseSyntax(m.nameSyntax);
            m.outputSyntax = me.SelectNodeInnerText("./output");
            m.outputParsed = mc.ParseWritableSyntax(m.outputSyntax);
            m.successSyntax = me.SelectNodeInnerText("./success");
            m.successParsed = mc.ParseWritableSyntax(m.successSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string p = this.nameParsed.Read(mc);
            ProcInfo pi = mc.schedulerMaster.TryGetProcInfo(mc.NameSpace, p);

            if (pi == null)
            {
                if (outputParsed != null)
                {
                    outputParsed.Write(mc, "");
                }
                if (successParsed != null)
                {
                    successParsed.Write(mc, "0");
                }
                return;
            }

            if (successParsed != null)
            {
                successParsed.Write(mc, "1");
            }

            if (outputParsed != null)
            {
                outputParsed.Write(mc, XmlConfigParser.LineInfoRegex.Replace(pi.procElem.PrettyString(), ""));
            }
        }
    }
}
