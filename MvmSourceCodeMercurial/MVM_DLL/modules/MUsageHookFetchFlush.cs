using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using NGenerics.DataStructures.Trees;
namespace MVM
{
    /*
    <usage_hook_release>
      <hook_id>TEMP.hook_id</hook_id>
    </usage_hook_create>
    
      */

    public class MUsageHookFetchFlush : IModuleSetup, IModuleRun
    {
        private string usageHookIdSyntax;
        private IReadString usageHookIdParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MUsageHookFetchFlush m = new MUsageHookFetchFlush();
            m.usageHookIdSyntax = me.SelectNodeInnerText("./hook_id");
            m.usageHookIdParsed = mc.ParseSyntax(m.usageHookIdSyntax);
            run.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            string hookId = this.usageHookIdParsed.Read(mc);
            string hookName=UsageHookObject.GetHookName(hookId);
            UsageHookObject usageHookObject = mc.globalContext.GetNamedClassInst(hookName) as UsageHookObject;
            usageHookObject.FetchFlush();
        }
    }
}
