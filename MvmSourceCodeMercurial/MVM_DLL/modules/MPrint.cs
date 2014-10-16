using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MPrint : IModuleSetup, IModuleRun
    {
        public string syntax;
        public IReadString parsedSyntax;
        public void Setup(XmlElement moduleElement, ModuleContext mc, List<IModuleRun> runModules)
        {
            MPrint m = new MPrint();
            m.syntax = moduleElement.InnerText;
            m.parsedSyntax = mc.ParseSyntax(m.syntax);
            runModules.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            mc.workMgr.mvm.LogFull(this.parsedSyntax.Read(mc), mc.LocalName);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("print:" + this.syntax);
        }
    }
}
