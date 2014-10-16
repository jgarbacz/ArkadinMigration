using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace MVM
{

    /**
     * Forces a file owned by this thead to be closed. Doesn't complain if file wasn't opened.
     * <file_close>TEMP.file</file_close>
     */
    class MFileClose: IModuleSetup,IModuleRun
    {
        private string fileSyntax;
        private IReadString fileParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MFileClose m = new MFileClose();
            m.fileSyntax = me.InnerText;
            m.fileParsed = mc.ParseSyntax(m.fileSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string file=this.fileParsed.Read(mc);
            mc.threadContext.CloseStreamWriter(file);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
             log.LogInfo("file_close:" + this.fileSyntax);
        }
    }
}
