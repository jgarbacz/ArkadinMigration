using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MWhile:IModuleSetup,IModuleRun
    {
        private string condition;
        private IReadString parsedCondition;
        private int childProcId=-1;
        public void Setup(System.Xml.XmlElement moduleElement, ModuleContext mc, List<IModuleRun> runModules)
        {
            MWhile m = new MWhile();
            m.condition = moduleElement.SelectNodeInnerText("./condition");
            m.parsedCondition = mc.ParseSyntax(m.condition);
            string childProcName = mc.GetChildProcName("while");
            XmlElement loopElem=moduleElement.SelectSingleElem("./loop");
            mc.ReadXmlProcFromElem(childProcName,loopElem);
            m.childProcId = mc.GetProcId(childProcName);
            runModules.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            // check the condition
            string v = this.parsedCondition.Read(mc);
            if (v.Equals("1"))
            {
                // call the child procContext adding the procNameSyntax to this procContext
                mc.CallProcForCurrentObjectAndReturnNested(this.childProcId);
            }
        }
    }
    class MWhile2 : IModuleSetup, IModuleRun,IModuleLabel
    {
        private string condition;
        private IReadString parsedCondition;
        private string whileConditionLabel;
        private string whileEndLabel;
        private bool nest;
        public void Setup(System.Xml.XmlElement moduleElement, ModuleContext mc, List<IModuleRun> runModules)
        {
            MWhile2 m = new MWhile2();
            m.nest = moduleElement.GetAttributeDefault("nest", "true").Equals("true");
            m.condition = moduleElement.SelectNodeInnerText("./condition");
            m.parsedCondition = mc.ParseSyntax(m.condition);
            m.whileConditionLabel = mc.GetChildProcName("while_loop_condition");
            m.whileEndLabel = mc.GetChildProcName("while_loop_end");
            runModules.Add(m); // has the while condition label
            XmlElement loopElem = moduleElement.SelectSingleElem("./loop");
            if (loopElem != null)
            {
                if (m.nest)runModules.Add(mc.GetModuleRun("<push_scope/>"));
                runModules.AddRange(mc.GetModuleRunList(loopElem.GetChildElems()));
                if (m.nest) runModules.Add(mc.GetModuleRun("<pop_scope/>"));
            }
            runModules.Add(mc.GetModuleRun("<goto_label>'" + m.whileConditionLabel + "'</goto_label>"));
            runModules.Add(mc.GetModuleRun("<label>'" + m.whileEndLabel + "'</label>"));
        }
        public void Run(ModuleContext mc)
        {
            string v = this.parsedCondition.Read(mc);
            if (!v.Equals("1"))
            {
                mc.GotoLabel(this.whileEndLabel);
            }
        }
        public string GetLabel()
        {
            return this.whileConditionLabel;
        }
      
    }
}
