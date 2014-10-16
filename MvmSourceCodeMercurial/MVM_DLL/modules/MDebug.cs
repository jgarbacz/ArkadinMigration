using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MDebug: IModuleSetup,IModuleRun
    {
        private string syntax;
        private IReadString parsedSyntax;
        public void Setup(XmlElement moduleElement, ModuleContext mc,List<IModuleRun> runModules)
        {
            if (mc.globalContext["debug"].Equals("1"))
            {
                MDebug m = new MDebug();
                m.syntax = moduleElement.InnerText;
                m.parsedSyntax = mc.ParseSyntax(m.syntax);
                runModules.Add(m);
            }
        }

        public void Run(ModuleContext mc)
        {
            //Console.WriteLine(this.parsedSyntax.Run(mc));
            Console.WriteLine(mc.worker.GetWorkerNoString() + "\t|" + this.parsedSyntax.Read(mc));
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("print:" + this.syntax);
        }
    }
}
