using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MScopeSnapshot: IModuleSetup,IModuleRun
    {
        public void Setup(XmlElement moduleElement, ModuleContext mc,List<IModuleRun> runModules)
        {
            MScopeSnapshot m = new MScopeSnapshot();
            runModules.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            mc.procContext.snapshotScopeDepth=mc.tempContext.ScopeDepth;
        }
    }
}
