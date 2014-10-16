using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using NGenerics.DataStructures.Trees;
namespace MVM
{
    /*
  
    
      */

    public class MUsageHookSaveLocal : IModuleSetup, IModuleRun
    {
        private string usageHookIdSyntax;
        private IReadString usageHookIdParsed;
        private string objectIdSyntax;
        private IReadString objectIdParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MUsageHookSaveLocal m = new MUsageHookSaveLocal();
            m.usageHookIdSyntax = me.SelectNodeInnerText("./hook_id");
            m.usageHookIdParsed = mc.ParseSyntax(m.usageHookIdSyntax);
            m.objectIdSyntax = me.SelectNodeInnerText("./object_id");
            m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            run.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            string hookId = this.usageHookIdParsed.Read(mc);
            string objectId = this.objectIdParsed.Read(mc);
            string hookName=UsageHookObject.GetHookName(hookId);
            UsageHookObject usageHookObject = mc.globalContext.GetNamedClassInst(hookName) as UsageHookObject;
            usageHookObject.SaveLocal(objectId);
        }
    }
}
