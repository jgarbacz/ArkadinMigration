using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MDeleteObject: IModuleSetup,IModuleRun
    {

        private string syntax;
        private IReadString parsedSyntax;
        public void Setup(XmlElement moduleElement, ModuleContext mc,List<IModuleRun> runModules)
        {
            MDeleteObject m = new MDeleteObject();
            m.syntax = moduleElement.InnerText;
            m.parsedSyntax = mc.ParseSyntax(m.syntax);
            runModules.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string oid=this.parsedSyntax.Read(mc);
            mc.objectCache.DeleteObject(oid);
        }

      
    }
}
