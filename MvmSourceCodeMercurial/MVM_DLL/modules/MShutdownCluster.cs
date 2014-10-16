using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MVM
{
    /*
        
     */
    class MShutdownCluster : IModuleSetup,IModuleRun
    {
         public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
         {
             MShutdownCluster m = new MShutdownCluster();
             run.Add(m);
         }

         public void Run(ModuleContext mc)
         {
             mc.mvm.mvmCluster.Shutdown(-1);

         }
    }
}
