using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    /*
    <run>
        : newModules...
    </run>
     */

    // this newModule is simply a block {}
    class MRun: IModuleSetup,IModuleRun
    {
        private int runProcId;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MRun m = new MRun();
            //string childProcName = mc.procDefinition.initNamespaceProcName + "/" + "run" + "[" + mc.moduleOrder + "]";
            string childProcName = mc.GetChildProcName("run");
            mc.ReadXmlProcFromElem(childProcName, me);
            m.runProcId = mc.GetProcId(childProcName);
            run.Add(new MScopeSnapshot());
            run.Add(m);
            run.Add(new MScopeUnsnapshot());
        }

        public void Run(ModuleContext mc)
        {
            mc.CallProcForCurrentObjectNested(this.runProcId); // added snap/unsnap in setup
            return;
        }
    }
}
