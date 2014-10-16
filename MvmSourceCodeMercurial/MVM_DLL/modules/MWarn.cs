using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MWarn: IModuleSetup,IModuleRun
    {
        public string syntax;
        public IReadString parsedSyntax;
        public void Setup(XmlElement moduleElement, ModuleContext mc,List<IModuleRun> runModules)
        {
            MWarn m = new MWarn();
            m.syntax = moduleElement.InnerText;
            m.parsedSyntax = mc.ParseSyntax(m.syntax);
            runModules.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string msg="WARNING: "+this.parsedSyntax.Read(mc);
            mc.workMgr.mvm.Log(msg); ;
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("warn:" + this.syntax);
        }
    }
}
