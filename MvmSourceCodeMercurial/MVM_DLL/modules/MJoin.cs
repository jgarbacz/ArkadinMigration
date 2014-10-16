using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    /*
        <join>
          <delim>TEMP.delim</delim>
          <input>1</input>
          <input>TEMP.two</input>
          <output>TEMP.one_delim_two</output>
        </join>
      */

    class MJoin : IModuleSetup, IModuleRun
    {
        // from xml
        private string delimSyntax;
        private string outputSyntax;
        private List<string> inputSyntax;

        // from setup
        private IReadString delimParsed;
        private IWriteString outputParsed;
        private List<IReadString> inputParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MJoin m = new MJoin();
            // xml extraction
            m.delimSyntax = me.SelectNodeInnerText("./delim");
            m.outputSyntax = me.SelectNodeInnerText("./output");
            m.inputSyntax = me.SelectNodesInnerText("./input");
            // parsing
            m.delimParsed = mc.ParseSyntax(m.delimSyntax);
            m.outputParsed = mc.ParseWritableSyntax(m.outputSyntax);
            m.inputParsed = mc.ParseSyntax(m.inputSyntax);
            // runtime
            run.Add(m);
        }
        volatile int startingLen = 100;
        public void Run(ModuleContext mc)
        {
            string delimValue = this.delimParsed.Read(mc);
            StringBuilder sb = new StringBuilder(startingLen);
            int i;
            for (i = 0; i < this.inputParsed.Count - 1; i++)
            {
                sb.Append(this.inputParsed[i].Read(mc));
                sb.Append(this.delimParsed.Read(mc));
            }
            if (i < this.inputParsed.Count)
            {
                sb.Append(this.inputParsed[i].Read(mc));
            }
            string output = sb.ToString();
            this.outputParsed.Write(mc, output);
            if (output.Length < startingLen) startingLen = output.Length;
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("join:");
        }
    }
}
