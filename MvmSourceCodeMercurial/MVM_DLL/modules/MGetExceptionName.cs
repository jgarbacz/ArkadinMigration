using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MGetExceptionName: IModuleSetup,IModuleRun
    {
        private string valueSyntax;
        private IWriteString valueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MGetExceptionName m = new MGetExceptionName();
            m.valueSyntax = me.InnerText;
            m.valueParsed = mc.ParseWritableSyntax(m.valueSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            this.valueParsed.Write(mc, mc.GetCaughtExceptionName());
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("get_exception_name:" + this.valueSyntax);
        }
    }
}
