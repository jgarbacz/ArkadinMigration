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
  <capitalize>
    <input>TEMP.in</input>
    <output>TEMP.out</output>
  </capitalize>
      */

    class MCapitalize : IModuleSetup, IModuleRun
    {
        // from xml
        private string inputSyntax;
        private string outputSyntax;

        // from setup
        private IReadString inputParsed;
        private IWriteString outputParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MCapitalize m = new MCapitalize();
            // xml extraction
            m.inputSyntax = me.SelectNodeInnerText("./input");
            m.outputSyntax = me.SelectNodeInnerText("./output");
            // parsing
            m.inputParsed = mc.ParseSyntax(m.inputSyntax);
            m.outputParsed = mc.ParseWritableSyntax(m.outputSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string input = this.inputParsed.Read(mc);
            if (input.Equals(""))
            {
                outputParsed.Write(mc, "");
            }
            else
            {
                string output = input.Substring(0, 1).ToUpper();
                if (input.Length > 1)
                {
                    string rest = input.Substring(1);
                    //Console.WriteLine("REST====" + rest);
                    output = output + rest;
                }

               // Console.WriteLine("OUTPUT====" + output);
                outputParsed.Write(mc, output);
            }
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("capitalize:");
        }
    }
}
