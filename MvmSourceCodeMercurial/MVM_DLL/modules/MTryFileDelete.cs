using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace MVM
{

    /**
     * <try_file_delete>TEMP.file</file_delete>
     */
    // Got tired of waiting hours for a test to complete only to get a random IO error, created this guy
    class MTryFileDelete : IModuleSetup, IModuleRun
    {
        private string fileSyntax;
        private IReadString fileParsed;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MTryFileDelete m = new MTryFileDelete();
            m.fileSyntax = me.InnerText;
            m.fileParsed = mc.ParseSyntax(m.fileSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string glob = this.fileParsed.Read(mc);
            foreach (string file in FileUtils2.GlobToList(glob))
            {
                try
                {
                    mc.threadContext.CloseStreamWriter(file); // close it incase we opened it.
                    File.Delete(file);
                }
                catch (IOException ioex)
                {
                    string exmsg = String.Format("An IO exception occurred in MTryFileDelete but was smothered. The file {0} was not deleted. Exception details: {1}",
                        file, ioex.Message);
                    mc.mvm.Log(exmsg);                    
                }
                catch (Exception e)
                {
                    string emsg = String.Format("An unknown exception occurred in MTryFileDelete, but was handled. It is not known if the file {0} was deleted. Exception details: {1}", 
                        file, e.Message);
                    mc.mvm.Log(emsg);
                }
            }
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("try_file_delete:" + this.fileSyntax);
        }
    }
}
