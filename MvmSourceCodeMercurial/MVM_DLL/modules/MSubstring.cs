using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    /*
  <substring>
    <input>TEMP.in</input>
    <offset>0</offset>
    <length>4</length>
    <output>TEMP.out</output>
  </substring>
      */

    class MSubstring: IModuleSetup,IModuleRun
    {
        // from xml
        private string inputSyntax;
        private string offsetSyntax;
        private string lengthSyntax;
        private string outputSyntax;
        
        // from setup
        private IReadString inputParsed;
        private IReadString offsetParsed;
        private IReadString lengthParsed;
        private IWriteString outputParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MSubstring m = new MSubstring();
            // xml extraction
            m.inputSyntax = me.SelectNodeInnerText("./input");
            m.offsetSyntax = me.SelectNodeInnerText("./offset","0");
            m.lengthSyntax = me.SelectNodeInnerText("./length");
            m.outputSyntax = me.SelectNodeInnerText("./output");
            // parsing
            m.inputParsed = mc.ParseSyntax(m.inputSyntax);
            m.offsetParsed = mc.ParseSyntax(m.offsetSyntax);
            m.lengthParsed = m.lengthSyntax==null?null:mc.ParseSyntax(m.lengthSyntax);
            m.outputParsed = mc.ParseWritableSyntax(m.outputSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            //try
            //{
                string input = this.inputParsed.Read(mc);
                string output;
                int offset = Convert.ToInt32(this.offsetParsed.Read(mc));
                if (offset > (input.Length - 1))
                {
                    outputParsed.Write(mc, "");
                    return;
                }
                if (this.lengthParsed == null)
                {
                    output = input.Substring(offset);
                    outputParsed.Write(mc, output);
                    return;
                }
                int length = Convert.ToInt32(this.lengthParsed.Read(mc));
                if ((offset + length) >= input.Length)
                {
                    output = input.Substring(offset);
                    outputParsed.Write(mc, output);
                    return;
                }
                output = input.Substring(offset, length);
                outputParsed.Write(mc, output);
            //}
            //catch (Exception e)
            //{
            //    var x = 1;
            //}
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("substring:");
        }
    }
}
