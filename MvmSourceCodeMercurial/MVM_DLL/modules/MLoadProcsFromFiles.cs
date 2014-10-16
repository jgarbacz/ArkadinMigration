using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

namespace MVM
{
    class MLoadProcsFromFiles : IModuleSetup, IModuleRun
    {
        private string inputFileSyntax;
        private string inputDirSyntax;
        private string inputMatchSyntax;
        private string errorCountSyntax;

        private IReadString inputFileParsed;
        private IReadString inputDirParsed;
        private IReadString inputMatchParsed;
        private IWriteString errorCountParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MLoadProcsFromFiles m = new MLoadProcsFromFiles();
            // xml extraction
            m.inputDirSyntax = me.SelectNodeInnerText("./input_dir");
            m.inputFileSyntax = me.SelectNodeInnerText("./input_file");
            m.inputMatchSyntax = me.SelectNodeInnerText("./input_match");
            m.errorCountSyntax = me.SelectNodeInnerText("./error_count");

            // parsing
            m.inputFileParsed = mc.ParseSyntax(m.inputFileSyntax);
            m.inputDirParsed = mc.ParseSyntax(m.inputDirSyntax);
            m.inputMatchParsed = mc.ParseSyntax(m.inputMatchSyntax);
            m.errorCountParsed = mc.ParseWritableSyntax(m.errorCountSyntax);

            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            // get our list of files
            List<string> files = new List<string>();
            string inputDir = this.inputDirParsed != null ? this.inputDirParsed.Read(mc) : null;
            if (this.inputFileParsed != null)
            {
                string fileName = this.inputFileParsed.Read(mc);
                if (inputDir == null)
                {
                    files.Add(fileName);
                }
                else
                {
                    DirectoryInfo root = new DirectoryInfo(inputDir);
                    foreach (var elem in root.GetFiles(fileName, SearchOption.TopDirectoryOnly))
                        files.Add(fileName);
                }
            }
            if (this.inputDirParsed != null && this.inputMatchParsed != null)
            {
                string inputMatch = this.inputMatchParsed.Read(mc);
                string inputRegex = FileUtils2.ConvertGlobToRegex(inputMatch);
                //Console.WriteLine("inputRegex=[" + inputRegex + "]");
                if (inputDir == null) throw new Exception("error, input_match expects input dir to be set");
                DirectoryInfo root = new DirectoryInfo(inputDir);
                //Console.WriteLine("Get files with match [" + inputMatch + "]");
                foreach (var elem in root.GetFiles("*", SearchOption.AllDirectories))
                {
                    string fullName = elem.FullName;
                    if (fullName.matches(inputRegex))
                    {
                        files.Add(elem.FullName);
                    }
                    else
                    {
                        //Console.WriteLine("skip:" + childElem.FullName);
                    }
                }
            }

            // Load the config and return the number of files with errors if requested
            int errors = mc.ReadXmlConfigFromFiles(files.ToArray());
            if (this.errorCountParsed != null)
            {
                this.errorCountParsed.Write(mc, errors.ToString());
            }
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("load_procs:");
        }
    }
}
