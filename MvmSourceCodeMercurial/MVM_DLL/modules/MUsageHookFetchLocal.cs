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
    public class MUsageHookFetchLocal : IModuleSetup, IModuleRun
    {
        private string usageHookIdSyntax;
        private IReadString usageHookIdParsed;
        private string nodeIdSyntax;
        private IReadString nodeIdParsed;
        private string idAccSyntax;
        private IReadString idAccParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MUsageHookFetchLocal m = new MUsageHookFetchLocal();
            m.usageHookIdSyntax = me.SelectNodeInnerText("./hook_id");
            m.usageHookIdParsed = mc.ParseSyntax(m.usageHookIdSyntax);
            m.nodeIdSyntax = me.SelectNodeInnerText("./node_id");
            m.nodeIdParsed = mc.ParseSyntax(m.nodeIdSyntax);
            m.idAccSyntax = me.SelectNodeInnerText("./id_acc");
            m.idAccParsed = mc.ParseSyntax(m.idAccSyntax);
            run.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            string hookId = this.usageHookIdParsed.Read(mc);
            string nodeId = this.nodeIdParsed.Read(mc);
            string idAcc = this.idAccParsed.Read(mc);
            string hookName=UsageHookObject.GetHookName(hookId);
            UsageHookObject usageHookObject = mc.globalContext.GetNamedClassInst(hookName) as UsageHookObject;
            usageHookObject.FetchLocal(nodeId.ToInt(),idAcc);
        }
    }
}
