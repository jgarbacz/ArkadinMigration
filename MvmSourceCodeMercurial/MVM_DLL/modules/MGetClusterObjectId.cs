using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MGetClusterObjectId: IModuleSetup,IModuleRun
    {

        private string valueSyntax;
        private IWriteString valueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MGetClusterObjectId m = new MGetClusterObjectId();
            // xml extraction
            m.valueSyntax = me.InnerText;
            // parsing
            m.valueParsed = mc.ParseWritableSyntax(m.valueSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            throw new Exception("fixme5");
            //this.valueParsed.Write(mc, mc.clusterObjectId);
        }

      
    }
    }
