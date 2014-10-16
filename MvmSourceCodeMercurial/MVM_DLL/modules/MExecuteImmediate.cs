using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    /*
  right now it works on <proc> and <procs> 
  TBD, make it work on newModules meaning make it inline them.
  Assuming we want the test to be dynamic we can have the
   newModules generated into a proc, then hash the text can
   call the appropriate proc_id.
     */
    class MExecuteImmediate : IModuleSetup, IModuleRun
    {
        public string configSyntax;
        public IReadString configParsed;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MExecuteImmediate m = new MExecuteImmediate();
            m.configSyntax = me.InnerText;
            m.configParsed = mc.ParseSyntax(m.configSyntax);

            //string childProcName = mc.GetChildProcName("block");
            
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string config = this.configParsed.Read(mc);
            // if the config is a proc or procs then we want to
            // add them to the system.
            mc.ReadXmlConfigFromString(config);

            //mc.ReadXmlProcFromElem(childProcName, me);
            //m.runProcId = mc.GetProcId(childProcName);
            //mc.CallProcForCurrentObjectNested(this.runProcId);
            return;
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("execute_immediate:");
        }
    }
}
