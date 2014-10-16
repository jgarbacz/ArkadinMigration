using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MSetServerModeOff: IModuleSetup,IModuleRun
    {
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MSetServerModeOff m = new MSetServerModeOff();
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            mc.mvm.SetServerModeOff();
        }
    }
}
