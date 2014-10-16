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
 
<read_text_file>
<file>’C:\dir\file’<file>
<text>TEMP.txt</text>
</read_text_file>

      */

    class MReadTextFile : IModuleSetup, IModuleRun
    {
        // from xml
        private string inputSyntax;
        private string outputSyntax;

        // from setup
        private IReadString inputParsed;
        private IWriteString outputParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MReadTextFile m = new MReadTextFile();
            // xml extraction
            m.inputSyntax = me.SelectNodeInnerText("./file");
            m.outputSyntax = me.SelectNodeInnerText("./text");
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
                string output = File.ReadAllText(input);
                outputParsed.Write(mc, output);
            }
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("read_text_file:");
        }
    }
}
