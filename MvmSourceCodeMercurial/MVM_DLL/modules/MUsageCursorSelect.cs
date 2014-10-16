using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    /*
     <usage_hook_select>
      <hook>TEMP.hook_id</hook>
      <cursor>TEMP.usg_csr</cursor>
     </usage_hook_select>
     */
    class MUsageHookSelect: IModuleSetup,IModuleRun
    {
        // from xml
        private string hookIdSyntax;
        private string readOnlySyntax;

        // from setup
        private IReadString hookIdParsed;
        private IReadString readOnlyParsed;
        
        private CursorSetupCommon cursorSetup;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MUsageHookSelect m = new MUsageHookSelect();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            m.hookIdSyntax = me.SelectNodeInnerText("./hook_id");
            m.readOnlySyntax = me.SelectNodeInnerText("./read_only","0");
            m.hookIdParsed = mc.ParseSyntax(m.hookIdSyntax);
            m.readOnlyParsed = mc.ParseSyntax(m.readOnlySyntax);
            run.Add(m);
            m.cursorSetup.AddCursorSubProcs(me, mc, run);
        }
       
        public void Run(ModuleContext mc)
        {
            string hookId = this.hookIdParsed.Read(mc);
            string readOnly = this.readOnlyParsed.Read(mc);
            var csr = new UsageCursor(mc, this.cursorSetup, hookId, readOnly.Equals("1"));
        }

        public class UsageCursor : CursorCommonLinqEnabled, ICursor
        {
            private string hookId;
            private UsageHookObject usageHookObject;
           
            public UsageCursor(ModuleContext mc, CursorSetupCommon setup, string hookId, bool readOnly)
                : base(mc, setup)
            {
                this.hookId = hookId;
                this.usageHookObject = mc.globalContext.GetNamedClassInst(UsageHookObject.GetHookName(this.hookId)) as UsageHookObject;
                this.usageHookObject.readOnlyPass = readOnly;
                // usage hook needs link to the cursor because it needs to know
                // how to instanciate new objects.
                this.usageHookObject.ReadWriteStart();
            }

            public override IObjectData CursorNext()
            {
                    if (this.usageHookObject.ReadWriteNext())
                    {
                        // need to make sure this object goes in the object cache.
                        this.mvm.objectCache.AddOrMergeObject(this.usageHookObject.Current);
                        return this.usageHookObject.Current;
                    }
                return null;
            }

            public override void CursorClear()
            {
                this.usageHookObject.ReadWriteStop();
            }
        }
    }
}
