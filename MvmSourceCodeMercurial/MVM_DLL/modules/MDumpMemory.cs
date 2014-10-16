using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

/*
 * Use this to find memory leaks, it dumps all the memory we know about.
<dump_memory/>
*/
namespace MVM
{
    public class MDumpMemory : IModuleSetup, IModuleRun
    {

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDumpMemory m = new MDumpMemory();
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            mc.DumpMemory();
        }

        
    }
}
