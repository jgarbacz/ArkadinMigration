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
  <round>
    <input>TEMP.money</input>
    <precision>2</precision>
    <output>TEMP.out</output>
  </round>
      */

    class MRound : IModuleSetup, IModuleRun
    {
        // from xml
        private string inputSyntax;
        private string precisionSyntax;
        private string outputSyntax;

        // from setup
        private IReadString inputParsed;
        private IReadString precisionParsed;
        private IWriteString outputParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MRound m = new MRound();
            // xml extraction
            m.inputSyntax = me.SelectNodeInnerText("./input");
            m.precisionSyntax = me.SelectNodeInnerText("./precision","0");
            m.outputSyntax = me.SelectNodeInnerText("./output");
            m.inputParsed = mc.ParseSyntax(m.inputSyntax);
            m.precisionParsed = mc.ParseSyntax(m.precisionSyntax);
            m.outputParsed = mc.ParseWritableSyntax(m.outputSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            decimal input,output;
            int precision;
            if (!decimal.TryParse(this.inputParsed.Read(mc), out input)) return;
            if(!int.TryParse(this.precisionParsed.Read(mc),out precision)) return;
            output = Math.Round(input, precision);
            outputParsed.Write(mc, decimal.Parse(output.ToString()).ToString("G29"));
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("round:");
        }
    }
}
