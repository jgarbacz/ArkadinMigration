using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MGetExceptionMessage: IModuleSetup,IModuleRun
    {
        private string valueSyntax;
        private IWriteString valueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MGetExceptionMessage m = new MGetExceptionMessage();
            m.valueSyntax = me.InnerText;
            m.valueParsed = mc.ParseWritableSyntax(m.valueSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            this.valueParsed.Write(mc, mc.GetCaughtExceptionMessage());
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("get_exception_name:" + this.valueSyntax);
        }
    }
}
