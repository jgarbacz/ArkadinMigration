using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    /*
    <run_once>
        : newModules...
    </run_once>
     */

    // this newModule is simply a block {} that runs once and never again. this newModule must
    // finish before other newModules can get past it. 
    //  
    //
    public class MRunOnce: IModuleSetup,IModuleRun
    {
        public int runProcId;
        public string childProcName;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MRunOnce m = new MRunOnce();
            m.childProcName = mc.procDefinition.localName + "/" + "run_once" + "[" + mc.moduleOrder + "]";
            mc.ReadXmlProcFromElem(m.childProcName, me);
            m.runProcId = mc.GetProcId(m.childProcName);
            // consider adding sync blocks to ensure the run once finishes before other threads
            // get past it... 
            run.Add(m);
        }
        
        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("run_once:" );
        }

        #region IModuleRun Members

        public bool runOnce = false;
        public void Run(ModuleContext mc)
        {
            lock (this)
            {
                if (!runOnce)
                {
                    // CHANGED THIS NOT TO BUMP SCOPE SINCE WE DO NOT HAVE A GOOD WAY OF
                    // SNAPING/UNSNAPING THE SCOPE ON RUN ONCE. IF WE USE THE SAME SCOPE WE DO NOT NEED
                    // TO WORRY ABOUT UNSNAPPING IT. ALSO, YOU COULD ARGUE THAT YOU WANT THE SAME SCOPE.
                    //mc.CallProcForCurrentObjectNested(runProcId);
                    mc.CallProcForCurrentObjectSameScope(runProcId);
                    mc.RemoveCurrentModule();
                    this.runOnce = true;
                }
            }
            return;
        }

        #endregion
    }
}
