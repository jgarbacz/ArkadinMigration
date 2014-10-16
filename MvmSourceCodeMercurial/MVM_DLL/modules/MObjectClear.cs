using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
/*
 
*/

    class MObjectClear : IModuleSetup, IModuleRun
    {
        private string inputSyntax;
        private IReadString inputParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MObjectClear m = new MObjectClear();
            
            m.inputSyntax = me.InnerText;
            m.inputParsed = mc.ParseSyntax(m.inputSyntax);
            run.Add(m);
        }

        
        public void Run(ModuleContext mc)
        {
            string oid = this.inputParsed.Read(mc);
            using (IObjectData obj = mc.objectCache.CheckOut(oid))
            {
                obj.Clear();
            }
        }
    }
}
