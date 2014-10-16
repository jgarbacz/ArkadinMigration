using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Description;
namespace MVM
{
    class MShutDownWcfHost: IModuleSetup,IModuleRun
    {

        private string valueSyntax;
        private IReadString valueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MShutDownWcfHost m = new MShutDownWcfHost();
            // xml extraction
            m.valueSyntax = me.InnerText;
            m.valueParsed = mc.ParseSyntax(m.valueSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string hostName = valueParsed.Read(mc);
           ServiceHost selfHost=(ServiceHost) mc.globalContext.GetNamedClassInst(hostName);
           selfHost.Close();
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("shutdown_wcf_host:" + this.valueSyntax);
        }
    }
}
