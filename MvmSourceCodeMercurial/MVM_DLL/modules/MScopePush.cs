using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MScopePush: IModuleSetup,IModuleRun
    {
        public void Setup(XmlElement moduleElement, ModuleContext mc,List<IModuleRun> runModules)
        {
            MScopePush m = new MScopePush();
            runModules.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            mc.tempContext.PushScope();
        }
    }
}
