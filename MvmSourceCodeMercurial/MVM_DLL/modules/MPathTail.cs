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
  <path_tail>
    <input>TEMP.money</input>
    <output>TEMP.out</output>
  </path_tail>
      */

    class MPathTail : IModuleSetup, IModuleRun
    {
        // from xml
        private string inputSyntax;
        private string outputSyntax;

        // from setup
        private IReadString inputParsed;
        private IWriteString outputParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MPathTail m = new MPathTail();
            // xml extraction
            m.inputSyntax = me.SelectNodeInnerText("./input");
            m.outputSyntax = me.SelectNodeInnerText("./output");
            m.inputParsed = mc.ParseSyntax(m.inputSyntax);
            m.outputParsed = mc.ParseWritableSyntax(m.outputSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string input=this.inputParsed.Read(mc);
            string output = input.PathTail();
            outputParsed.Write(mc, output);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("path_tail:");
        }
    }
}
