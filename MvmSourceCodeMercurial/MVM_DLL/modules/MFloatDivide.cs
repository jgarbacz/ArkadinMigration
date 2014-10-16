using System;
using System.Collections.Generic;
////using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    /*
  <float_divide>
    <input>TEMP.in1</input>
    <input>TEMP.in2</input>
    <output>TEMP.out</output>
  </float_divide>
      */

    class MFloatDivide: IModuleSetup, IModuleRun
    {
        // from xml
        private List<string> inputsSyntax;
        private string outputSyntax;

        // from setup
        private List<IReadString> inputsParsed=new List<IReadString>();
        private IWriteString outputParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MFloatDivide m = new MFloatDivide();
            // xml extraction
            m.inputsSyntax = me.SelectNodesInnerText("./input");
            m.outputSyntax = me.SelectNodeInnerText("./output");
            // parsing
            foreach (string input in m.inputsSyntax)
            {
                m.inputsParsed.Add(mc.ParseSyntax(input));
            }
            m.outputParsed = mc.ParseWritableSyntax(m.outputSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            double n = double.Parse(this.inputsParsed[0].Read(mc));
            for (int i = 1; i < this.inputsParsed.Count; i++)
            {
                double d = double.Parse(this.inputsParsed[i].Read(mc));
                n = n / d;
            }
            outputParsed.Write(mc, n.ToString());
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("float_divide:");
        }
    }
}
