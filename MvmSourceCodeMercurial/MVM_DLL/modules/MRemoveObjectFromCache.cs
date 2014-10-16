using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MRemoveObjectFromCache: IModuleSetup,IModuleRun
    {

        private string syntax;
        private IReadString parsedSyntax;
        public void Setup(XmlElement moduleElement, ModuleContext mc,List<IModuleRun> runModules)
        {
            MRemoveObjectFromCache m = new MRemoveObjectFromCache();
            m.syntax = moduleElement.InnerText;
            m.parsedSyntax = mc.ParseSyntax(m.syntax);
            runModules.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string oid=this.parsedSyntax.Read(mc);
            mc.objectCache.RemoveObjectData(oid);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("remove_object_from_cache:" + this.syntax);
        }
    }
}
