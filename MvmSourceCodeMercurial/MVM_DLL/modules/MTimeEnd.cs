using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MVM
{
    class MTimeEnd:IModuleSetup,IModuleRun
    {
        private string msSyntax;
        private IReadString msReadParsed;
        private IWriteString msWriteParsed;

        private string totalMsSyntax;
        private IReadString totalMsReadParsed;
        private IWriteString totalMsWriteParsed;

        #region IModuleSetup Members

        public void Setup(System.Xml.XmlElement me, ModuleContext mc,List<IModuleRun> runModules)
        {
            MTimeEnd m = new MTimeEnd();
            m.msSyntax = me.GetAttribute("ms");
            m.msReadParsed = mc.ParseSyntax(m.msSyntax);
            m.msWriteParsed = mc.ParseWritableSyntax(m.msSyntax);

            m.totalMsSyntax = me.GetAttribute("total_ms");
            if (!m.totalMsSyntax.Equals(""))
            {
                m.totalMsReadParsed = mc.ParseSyntax(m.totalMsSyntax);
                m.totalMsWriteParsed = mc.ParseWritableSyntax(m.totalMsSyntax);
            }
            runModules.Add(m);
        }

        #endregion

        #region IModuleRun Members

        public void Run(ModuleContext mc)
        {
            int endTickCount = Environment.TickCount;
            int startTickCount = int.Parse(this.msReadParsed.Read(mc));
            int duration = endTickCount - startTickCount;
            this.msWriteParsed.Write(mc, duration.ToString());
            if (totalMsReadParsed != null)
            {
                string prevTotalString = totalMsReadParsed.Read(mc);
                int prevTotal=0;
                int.TryParse(prevTotalString,out prevTotal);
                int newTotal = prevTotal + duration;
                this.totalMsWriteParsed.Write(mc, newTotal.ToString());
            }
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("time_end: " + this.msSyntax);
        }

        #endregion
    }


}
