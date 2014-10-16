using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MFatal: IModuleSetup,IModuleRun
    {
        public string syntax;
        public IReadString parsedSyntax;
        public void Setup(XmlElement moduleElement, ModuleContext mc,List<IModuleRun> runModules)
        {
            MFatal m = new MFatal();
            m.syntax = moduleElement.InnerText;
            m.parsedSyntax = mc.ParseSyntax(m.syntax);
            runModules.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            String msg = this.parsedSyntax.Read(mc);
            mc.workMgr.mvm.Log(msg); ;
            throw new Exception(msg);

        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("fatal:" + this.syntax);
        }
    }
}
