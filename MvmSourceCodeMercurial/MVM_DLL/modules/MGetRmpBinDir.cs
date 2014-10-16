using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MGetRmpBinDir: IModuleSetup,IModuleRun
    {

        private string valueSyntax;
        private IWriteString valueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MGetRmpBinDir m = new MGetRmpBinDir();
            // xml extraction
            m.valueSyntax = me.InnerText;
            // parsing
            m.valueParsed = mc.ParseWritableSyntax(m.valueSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string output = mc.mvm.rmpBinDir;
            this.valueParsed.Write(mc, output);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("get_rmp_bin_dir:" + this.valueSyntax);
        }
    }
}
