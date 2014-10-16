using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MCreateDirectory: IModuleSetup,IModuleRun
    {

        private string directorySyntax;
        private IReadString directoryParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MCreateDirectory m = new MCreateDirectory();
            m.directorySyntax = me.InnerText;
            m.directoryParsed = mc.ParseSyntax(m.directorySyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string dir=this.directoryParsed.Read(mc);
            System.IO.Directory.CreateDirectory(dir);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
             log.LogInfo("create_directory:" + this.directorySyntax);
        }
    }
}
