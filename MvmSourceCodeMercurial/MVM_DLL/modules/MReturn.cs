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
     Returns from the nearest entry procContext.
  <return/>
     * 
     */

    class MReturn : IModuleSetup, IModuleRun
    {
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MReturn m = new MReturn();
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            ProcInst w = mc.workMgr.GetEntryWork(mc.procInst);
            if (w == null) throw new Exception("Error, cannot find entry proc to return to");
            mc.breakFromProcName = mc.scheduler.GetLocalProcName(w.procId);
            mc.moduleStatus = ModuleStatus.BreakFromProcName;
            // this indicates that this proc is done.
            //mc.procInst.nextModuleOrder = ModuleOrder.CreateDoneOrder();
        }
    }
    }
