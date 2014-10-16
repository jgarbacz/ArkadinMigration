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
  <compile_dll>
    <source_glob>*.cs</source_glob>
    <assembly>my.dll</assembly>
    <reference>'System'</reference>
    <reference>'System.Threading'</reference>
    <compiler_options>''</compiler_options>
    <success>TEMP.out</success>
    <compiler_output>TEMP.errors</compiler_output>
  </compile_dll>
      */

    class MCompileDll : IModuleSetup, IModuleRun
    {
        private List<string> sourceGlobsSyntax;
        private List<IReadString> sourceGlobsParsed;
        private string assemblySyntax;
        private IReadString assemblyParsed;
        private List<string> referencesSyntax;
        private List<IReadString> referencesParsed;
        private string compilerOptionsSyntax;
        private IReadString compilerOptionsParsed;
        private string successSyntax;
        private IWriteString successParsed;
        private string errorInfoSyntax;
        private IWriteString errorInfoParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MCompileDll m = new MCompileDll();
            m.sourceGlobsSyntax = me.SelectNodesInnerText("./source_glob");
            m.sourceGlobsParsed = mc.ParseSyntax(m.sourceGlobsSyntax);
            m.assemblySyntax = me.SelectNodeInnerText("./assembly");
            m.assemblyParsed = mc.ParseSyntax(m.assemblySyntax);
            m.referencesSyntax = me.SelectNodesInnerText("./reference");
            m.referencesParsed = mc.ParseSyntax(m.referencesSyntax);
            m.compilerOptionsSyntax = me.SelectNodeInnerText("./compiler_options");
            m.compilerOptionsParsed = mc.ParseSyntax(m.compilerOptionsSyntax);
            m.successSyntax = me.SelectNodeInnerText("./success");
            m.successParsed = mc.ParseWritableSyntax(m.successSyntax);
            m.errorInfoSyntax = me.SelectNodeInnerText("./compiler_output");
            m.errorInfoParsed = mc.ParseWritableSyntax(m.errorInfoSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string assembly = this.assemblyParsed.Read(mc);
            string compilerOptions = this.compilerOptionsParsed.Read(mc);
            List<string> sourceFiles = new List<string>();
            foreach (var fg in this.sourceGlobsParsed)
            {
                string glob = fg.Read(mc);
                foreach (string f in FileUtils2.Glob(glob))
                {
                    sourceFiles.Add(f);
                }
            }
            List<string> references = new List<string>();
            foreach (var r in this.referencesParsed)
            {
                string reference = r.Read(mc);
                references.Add(reference);
            }

            string compilerOutput;
            if (CodeGen.CompileDll(sourceFiles[0], references.ToArray(), assembly, compilerOptions, out compilerOutput))
            {
                this.successParsed.Write(mc, "1");
            }
            else
            {
                this.successParsed.Write(mc, "0");
            }
            this.errorInfoParsed.Write(mc, compilerOutput);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("compile_dll:");
        }
    }
}
