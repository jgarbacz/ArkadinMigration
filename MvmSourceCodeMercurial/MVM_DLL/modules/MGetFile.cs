using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    /*
  <get_file>
    <dir>GLOBAL.input_dir</dir>
    <match>'ace_states.*'</match>
    <file>GLOBAL.ace_states_file</file>
  </get_file>
      */

    class MGetFile: IModuleSetup,IModuleRun
    {
        // from xml
        private string inputFileSyntax;
        private string inputDirSyntax;
        private string inputMatchSyntax;
        
        // from setup
        private IWriteString inputFileParsed;
        private IReadString inputDirParsed;
        private IReadString inputMatchParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MGetFile m = new MGetFile();
            // xml extraction
            m.inputDirSyntax = me.SelectNodeInnerText("./dir");
            m.inputFileSyntax = me.SelectNodeInnerText("./file");
            m.inputMatchSyntax = me.SelectNodeInnerText("./match");
            // parsing
            m.inputFileParsed = mc.ParseWritableSyntax(m.inputFileSyntax);
            m.inputDirParsed = mc.ParseSyntax(m.inputDirSyntax);
            m.inputMatchParsed = mc.ParseSyntax(m.inputMatchSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            // get our list of files
            List<string> files = new List<string>();
            string inputDir = this.inputDirParsed.Read(mc);
            string inputMatch = this.inputMatchParsed.Read(mc);
            DirectoryInfo root = new DirectoryInfo(inputDir);
            foreach (var elem in root.GetFiles(inputMatch, SearchOption.TopDirectoryOnly)) files.Add(elem.FullName);
            if(files.Count==0) throw new Exception("Error, cannot do get_file. dir=" +inputDir+",match="+inputMatch+", returns no files");
            if (files.Count > 1) throw new Exception("Error, cannot do get_file. dir=" + inputDir + ",match=" + inputMatch + ", returns more than one file. files="+files.Join(","));
            this.inputFileParsed.Write(mc,files[0]);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("get_file:");
        }
    }
}
