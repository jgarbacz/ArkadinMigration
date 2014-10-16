using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    class MProcInstGetParams:IModuleSetup,IModuleRun
    {
        private string procInstSyntax;
        private IReadString procInstParsed;
        private Dictionary<string, string> paramsSyntax=new Dictionary<string,string>();
        private Dictionary<string,IWriteString> paramsParsed=new Dictionary<string,IWriteString>();
        #region IModuleSetup Members

        public void Setup(System.Xml.XmlElement me, ModuleContext mc, List<IModuleRun> runModules)
        {
            MProcInstGetParams m = new MProcInstGetParams();
            m.procInstSyntax = me.SelectNodeInnerText("proc_inst_id");
            m.procInstParsed = mc.ParseSyntax(m.procInstSyntax);
            foreach (var elem in me.SelectElements("./param"))
            {
                string name = elem.GetAttribute("name");
                string value = elem.InnerText;
                m.paramsSyntax[name] = value;
                m.paramsParsed[name] = mc.ParseWritableSyntax(value);
            }
            runModules.Add(m);
        }

        #endregion

        #region IModuleRun Members

        public void Run(ModuleContext mc)
        {
            long procInstId = this.procInstParsed.Read(mc).ToLong();
            TempContext procTempContext = mc.workMgr.GetCallbacksTempContext(procInstId);
            foreach (var p in this.paramsParsed)
            {
                string currentValue = procTempContext[p.Key];
                p.Value.Write(mc,currentValue);
            }
        }

        #endregion
    }
}
