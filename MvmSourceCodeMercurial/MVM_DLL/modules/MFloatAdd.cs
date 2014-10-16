using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    /*
  <float_add>
    <input>TEMP.in1</input>
    <input>TEMP.in2</input>
    <output>TEMP.out</output>
  </float_add>
      */

    class MFloatAdd : IModuleSetup, IModuleRun
    {
        // from xml
        private List<string> inputsSyntax;
        private string outputSyntax;

        // from setup
        private List<IReadString> inputsParsed=new List<IReadString>();
        private IWriteString outputParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MFloatAdd m = new MFloatAdd();
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
            double sum = 0.0;
            foreach (IReadString input in this.inputsParsed)
            {
                string s = input.Read(mc);
                if (s.Equals("")) s = "0";
                double d = Double.Parse(s);
                sum += d;
            }
            outputParsed.Write(mc, sum.ToString());
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("float_add:");
        }
    }
}
