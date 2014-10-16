using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MScopePop: IModuleSetup,IModuleRun
    {
        public void Setup(XmlElement moduleElement, ModuleContext mc,List<IModuleRun> runModules)
        {
            MScopePop m = new MScopePop();
            runModules.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            mc.tempContext.PopScope();
        }
    }
}
