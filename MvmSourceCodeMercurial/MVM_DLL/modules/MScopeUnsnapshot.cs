using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MScopeUnsnapshot: IModuleSetup,IModuleRun
    {
        public void Setup(XmlElement moduleElement, ModuleContext mc,List<IModuleRun> runModules)
        {
            MScopeUnsnapshot m = new MScopeUnsnapshot();
            runModules.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            mc.tempContext.ScopeDepth=mc.procContext.snapshotScopeDepth;
        }
    }
}
