using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace MVM
{
    /**
     * Creates a zero length file if one doesn't exist
     * <file_touch>TEMP.file</file_touch>
     */
    class MFileTouch: IModuleSetup,IModuleRun
    {
        private string fileSyntax;
        private IReadString fileParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MFileTouch m = new MFileTouch();
            m.fileSyntax = me.InnerText;
            m.fileParsed = mc.ParseSyntax(m.fileSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string file=this.fileParsed.Read(mc);
            if (!File.Exists(file))
            {
                FileStream fs = File.Create(file);
                fs.Close();
            }
            else
            {
                mc.threadContext.CloseStreamWriter(file); /* close it in case we have it open */
                FileStream fs=File.OpenWrite(file);
                fs.Close();
            }
        }

        public void Log(ModuleContext mc, ILogger log)
        {
             log.LogInfo("file_touch:" + this.fileSyntax);
        }
    }
}
