using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

namespace MVM
{
    class MLoadProcsFromGlob: IModuleSetup,IModuleRun
    {
        private List<string> globsSyntax;
        private List<IReadString> globsParsed;

        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MLoadProcsFromGlob m = new MLoadProcsFromGlob();

            m.globsSyntax=me.SelectNodesInnerText("./glob");
            m.globsParsed = mc.ParseSyntax(m.globsSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            // get our list of files
            List<string> files = new List<string>();
            foreach (IReadString globParsed in this.globsParsed)
            {
                string globValue=globParsed.Read(mc);
                files.AddRange(FileUtils2.GlobToList(globValue));
            }
            // load the config
            mc.ReadXmlConfigFromFiles(files.ToArray());
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("load_procs:");
        }
    }
}
