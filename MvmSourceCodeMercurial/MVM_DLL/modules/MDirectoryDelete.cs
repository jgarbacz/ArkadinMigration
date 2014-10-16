using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace MVM
{

    /**
     * <directory_delete>TEMP.directory</directory_delete>
     */
    class MDirectoryDelete: IModuleSetup,IModuleRun
    {
        private string directorySyntax;
        private IReadString directoryParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MDirectoryDelete m = new MDirectoryDelete();
            m.directorySyntax = me.InnerText;
            m.directoryParsed = mc.ParseSyntax(m.directorySyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string dir=this.directoryParsed.Read(mc);
            if(Directory.Exists(dir))
                Directory.Delete(dir, true);
        }
    }
}
