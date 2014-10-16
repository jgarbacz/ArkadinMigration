using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MSetServerModeOn: IModuleSetup,IModuleRun
    {
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MSetServerModeOn m = new MSetServerModeOn();
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            mc.mvm.SetServerModeOn();
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("set_server_mode_on:");
        }
    }
}
