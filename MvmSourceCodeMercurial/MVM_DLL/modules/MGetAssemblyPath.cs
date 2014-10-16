using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MGetAssemblyUri: IModuleSetup,IModuleRun
    {

        private string valueSyntax;
        private IWriteString valueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MGetAssemblyUri m = new MGetAssemblyUri();
            // xml extraction
            m.valueSyntax = me.InnerText;
            // parsing
            m.valueParsed = mc.ParseWritableSyntax(m.valueSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string assemblyPath = System.Reflection.Assembly.GetEntryAssembly().EscapedCodeBase;
            this.valueParsed.Write(mc, assemblyPath);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("get_assembly_Path:" + this.valueSyntax);
        }
    }
}
