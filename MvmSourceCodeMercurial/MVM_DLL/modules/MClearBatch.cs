using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{

    /*
     * 
     * <clear_batch>TEMP.batch_id</clear_batch>
     * 
     * clears the batch from the remote manager
     * 
     */
    class MClearBatch:IModuleSetup,IModuleRun
    {
        private string batchIdSyntax;
        private IReadString batchIdParsed;
        
        public void Setup(System.Xml.XmlElement me, ModuleContext mc, List<IModuleRun> runModules)
        {
            MClearBatch m = new MClearBatch();
            m.batchIdSyntax = me.InnerText;
            m.batchIdParsed = mc.ParseSyntax(m.batchIdSyntax);
            runModules.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string batchId = this.batchIdParsed.Read(mc);
            mc.mvm.remoteWorkMgr.ClearBatch(long.Parse(batchId));
        }
    }
}
