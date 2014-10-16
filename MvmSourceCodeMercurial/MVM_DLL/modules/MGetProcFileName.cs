using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MGetProcFileName: IModuleSetup,IModuleRun
    {

        private string valueSyntax;
        private IWriteString valueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MGetProcFileName m = new MGetProcFileName();
            // xml extraction
            m.valueSyntax = me.InnerText;
            // parsing
            m.valueParsed = mc.ParseWritableSyntax(m.valueSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string output = mc.procDefinition.procInfo.location.GetLocation().Nvl("");
            this.valueParsed.Write(mc, output);
        }

       
    }
}
