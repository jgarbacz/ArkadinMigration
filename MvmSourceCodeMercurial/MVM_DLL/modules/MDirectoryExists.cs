﻿using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    /*
  <directory_exists>
    <input>TEMP.in</input>
    <output>TEMP.out</output>
  </directory_exists>
      */

    class MDirectoryExists : IModuleSetup, IModuleRun
    {
        // from xml
        private string inputSyntax;
        private string outputSyntax;

        // from setup
        private IReadString inputParsed;
        private IWriteString outputParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDirectoryExists m = new MDirectoryExists();
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
            string output = Directory.Exists(input) ? "1":"0";
            outputParsed.Write(mc, output);

        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("directory_exists:");
        }
    }
}
