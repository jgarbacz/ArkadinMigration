using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{

    class MIf2 : IModuleSetup, IModuleRun
    {
        private List<IReadString> conditions = new List<IReadString>();
        private List<string> thenLabels = new List<string>();
        private string elseLabel = null;
        private string endifLabel;
        public void Setup(System.Xml.XmlElement moduleElement, ModuleContext mc, List<IModuleRun> runModules)
        {
            int thenElseCtr = 0;
            MIf2 m = new MIf2();
            runModules.Add(m); 
            m.endifLabel = mc.GetChildProcName("endif");
            for (XmlElement elem = moduleElement.FirstChildElem(); elem != null; elem = elem.NextSiblingElem())
            {
                if (elem.LocalName.Equals("condition"))
                {
                    var condition = elem.InnerText;
                    var parsedCondition = mc.ParseSyntax(condition);
                    m.conditions.Add(parsedCondition);
                    if (elem.NextSiblingElem().LocalName.In("then"))
                    {
                        elem = elem.NextSiblingElem();
                        string label = mc.GetChildProcName("if_then" + thenElseCtr++);
                        runModules.Add(mc.GetModuleRun("<label>'"+label+"'</label>"));
                        runModules.AddRange(mc.GetModuleRunList(elem.GetChildElems()));
                        runModules.Add(mc.GetModuleRun("<goto_label>'" + m.endifLabel + "'</goto_label>"));
                        m.thenLabels.Add(label);
                    }
                    else
                    {
                        m.thenLabels.Add(m.endifLabel);
                    }
                }
                else if (elem.LocalName.In("else"))
                {
                    m.elseLabel = mc.GetChildProcName("if_else" + thenElseCtr++);
                    runModules.Add(mc.GetModuleRun("<label>'" + m.elseLabel + "'</label>"));
                    runModules.AddRange(mc.GetModuleRunList(elem.GetChildElems()));
                }
                else
                {
                    throw new Exception("unexpected");
                }
            }
            runModules.Add(mc.GetModuleRun("<label>'" + m.endifLabel + "'</label>"));
        }

        public void Run(ModuleContext mc)
        {
            for (int i = 0; i < this.conditions.Count; i++)
            {
                string v = this.conditions[i].Read(mc);
                if (v.Equals("1"))
                {
                    string label = this.thenLabels[i];
                    mc.GotoLabel(label);
                    return;
                }
            }
            if (this.elseLabel!=null)
            {
                mc.GotoLabel(this.elseLabel);
                return;
            }
            mc.GotoLabel(this.endifLabel);
        }
    }

}
