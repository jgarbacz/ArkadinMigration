using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

//
// THIS IS NOT A NORMAL MODULE!!!
//
// Serial processes its child newModules, typically used inside <parallel></parallel>
// <serial>
//      <somemodule/>
//      <somemodule/>
// </serial>
// 
// All its newModules in a procContext
// Produces procInst for each procContext with the current object
//
namespace MVM
{
    class InlineCallProcForCurrentObject: IModuleSetup,IModuleRun
    {
        int childProcId;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            InlineCallProcForCurrentObject m = new InlineCallProcForCurrentObject();
            //string childProcName = mc.procDefinition.initNamespaceProcName + "/" + "icpfco" + "[" + mc.moduleOrder + "]";
            string childProcName = mc.GetChildProcName("icpfco");
            mc.ReadXmlProcFromElem(childProcName, me);
            m.childProcId = mc.GetProcId(childProcName);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
                mc.CallProcForCurrentObject(this.childProcId);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("serial:");
        }
    }
}
