using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

/*
    <print_text_to_file>
    <file>TEMP.src_file</file>
    <text>TEMP.src</text>
    </print_text_to_file> 
 */
namespace MVM
{
    class MPrintTextToFile : IModuleSetup, IModuleRun
    {
        private string fileNameSyntax;
        private IReadString fileNameParsed;
        private string textSyntax;
        private IReadString textParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MPrintTextToFile m = new MPrintTextToFile();
            m.fileNameSyntax = me.SelectNodeInnerText("./file");
            m.fileNameParsed = mc.ParseSyntax(m.fileNameSyntax);
            m.textSyntax = me.SelectNodeInnerText("./text");
            m.textParsed = mc.ParseSyntax(m.textSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string fileName = this.fileNameParsed.Read(mc);
            FileInfo fileInfo = new FileInfo(fileName);
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            string text = this.textParsed.Read(mc);
            text.WriteToFile(fileName);

            
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("write text to file:" );
        }
    }
}
