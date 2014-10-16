using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
namespace MVM
{
    /*
  <file_copy>
    <source>TEMP.f1</source>
    <target>TEMP.f2</target>
  </file_copy>
      */

    class MFileCopy : IModuleSetup, IModuleRun
    {
        // from xml
        private List<string> sourceSyntax;
        private string targetSyntax;

        // from setup
        private List<IReadString> sourceParsed;
        private IReadString targetParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MFileCopy m = new MFileCopy();
            // xml extraction
            m.sourceSyntax = me.SelectNodesInnerText("./source");
            m.targetSyntax = me.SelectNodeInnerText("./target");
            // parsing
            m.sourceParsed = mc.ParseSyntax(m.sourceSyntax);
            m.targetParsed = mc.ParseSyntax(m.targetSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            //string source = this.sourceParsed.Run(mc);
            string targetValue = this.targetParsed.Read(mc);
            //Console.WriteLine("CHECK Directory.exists("+targetValue+")");
            // target is a directory, this is a unix mv
            if (Directory.Exists(targetValue))
            {
                //Console.WriteLine("CHECK Directory.exists(" + targetValue + ")==TRUE");
                foreach (IReadString src in this.sourceParsed)
                {
                    string sourceFileName = src.Read(mc);
                    mc.threadContext.CloseStreamWriter(sourceFileName); /* close it incase we opened it.*/
                    string destFileName = Path.Combine(targetValue, Path.GetFileName(sourceFileName));
                    //Console.WriteLine("File.Copy(" + sourceFileName + "," + destFileName + ")");
                    File.Copy(sourceFileName, destFileName,true);
                }
                return;
            }
            else
            {
                //Console.WriteLine("CHECK Directory.exists(" + targetValue + ")==FALSE");
            }
            {
                // Otherwise treat target as a filename so do rename
                if (this.sourceParsed.Count > 1) throw new Exception("Error, cannot file_copy multiple sources when target=[" + targetValue + "] is a directory");
                string sourceFileName = this.sourceParsed[0].Read(mc);
                mc.threadContext.CloseStreamWriter(sourceFileName); /* close it incase we opened it.*/
                //Console.WriteLine("File.Copy(" + sourceFileName + "," + targetValue + ")");
                File.Copy(sourceFileName, targetValue,true);
            }
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("file_copy:");
        }
    }
}
