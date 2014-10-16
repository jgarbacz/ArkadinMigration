using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using NGenerics.DataStructures.Trees;
namespace MVM
{
    /*
    <usage_hook_add_object>
      <hook_id>TEMP.hook_id</hook_id>
    </usage_hook_create>
      */
    public class MUsageHookAddObject : IModuleSetup, IModuleRun
    {
        private string usageHookIdSyntax;
        private IReadString usageHookIdParsed;
        private string objectIdSyntax;
        private IReadString objectIdParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MUsageHookAddObject m = new MUsageHookAddObject();
            m.usageHookIdSyntax = me.SelectNodeInnerText("./hook_id");
            m.usageHookIdParsed = mc.ParseSyntax(m.usageHookIdSyntax);
            m.objectIdSyntax = me.SelectNodeInnerText("./object_id");
            m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            // set the sortkey
            run.Add(mc.GetModuleRun(
                @"<call_dynamic_proc_for_object>
                      <name>'generate_sortkey_'~OBJECT("+m.objectIdSyntax+@").id_view</name>
                      <object_id>"+m.objectIdSyntax+@"</object_id>
                </call_dynamic_proc_for_object>")
            );
            // then add it to the hook
            run.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            string hookId = this.usageHookIdParsed.Read(mc);
            string hookName=UsageHookObject.GetHookName(hookId);
            string objectId = this.objectIdParsed.Read(mc);
            UsageHookObject usageHookObject = mc.globalContext.GetNamedClassInst(hookName) as UsageHookObject;
            using (ObjectDataFormattedDelta obj = mc.objectCache.CheckOut(objectId) as ObjectDataFormattedDelta)
            {
                obj["usage_hook_add_object"] = "1";
                usageHookObject.AddObjectToHook(obj,false);
            }

        }
    }
}
