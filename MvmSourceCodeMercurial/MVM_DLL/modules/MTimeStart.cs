using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    class MTimeStart:IModuleSetup,IModuleRun
    {
        private string msSyntax;
        private IWriteString msParsed;
        
        #region IModuleSetup Members

        public void Setup(System.Xml.XmlElement me, ModuleContext mc,List<IModuleRun> runModules)
        {
            MTimeStart m = new MTimeStart();
            m.msSyntax = me.GetAttribute("ms");
            m.msParsed = mc.ParseWritableSyntax(m.msSyntax);
            runModules.Add(m);
        }

        #endregion

        #region IModuleRun Members

        public void Run(ModuleContext mc)
        {
            int tickCount = Environment.TickCount;
            this.msParsed.Write(mc, tickCount.ToString());
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("time_start: " + this.msSyntax);
        }

        #endregion
    }
}
