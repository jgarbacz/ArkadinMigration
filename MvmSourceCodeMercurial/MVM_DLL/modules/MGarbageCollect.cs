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
  <garbage_collect/>
      */

    class MGarbageCollect : IModuleSetup, IModuleRun
    {

        //private string frequencySyntax;
        private int frequency;
        private int my_counter;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MGarbageCollect m = new MGarbageCollect();


            // xml extraction
            m.frequency = me.SelectNodeInnerText("./frequency").ToInt();

            // ...parsing and setup...

            my_counter = 0;
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            my_counter++;
            if (my_counter >= frequency)
            {
                //mc.mvm.Log("Garbage_Collect:");
                System.GC.Collect();
                my_counter = 0;
            }
            
        }

    }
}
