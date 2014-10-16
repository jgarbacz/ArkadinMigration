using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace MVM
{

    /**
     * <file_delete>TEMP.file</file_delete>
     */
    class MFileDelete: IModuleSetup,IModuleRun
    {
        private string fileSyntax;
        private IReadString fileParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MFileDelete m = new MFileDelete();
            m.fileSyntax = me.InnerText;
            m.fileParsed = mc.ParseSyntax(m.fileSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string glob=this.fileParsed.Read(mc);
            foreach(string file in FileUtils2.GlobToList(glob)){
                mc.threadContext.CloseStreamWriter(file); // close it incase we opened it.
                File.Delete(file);
            }
        }

        public void Log(ModuleContext mc, ILogger log)
        {
             log.LogInfo("file_delete:" + this.fileSyntax);
        }
    }
}
