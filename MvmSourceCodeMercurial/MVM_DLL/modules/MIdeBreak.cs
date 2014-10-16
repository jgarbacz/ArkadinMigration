using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MVM
{
    class MIdeBreak: IModuleSetup,IModuleRun
    {
        public string text;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MIdeBreak m = new MIdeBreak();
            m.text = me.InnerText;
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            //Console.WriteLine("depth="+mc.tempContext.ScopeDepth);
            var t=mc.tempContext;
            var d = t.ScopeDepth;
            string text = this.text;
            mc.moduleStatus = ModuleStatus.IdeBreakPoint;
        }

       
    }
}
