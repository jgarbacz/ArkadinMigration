using System;
using System.Collections.Generic;

using System.Text;
using NLog;
namespace MVM
{

    /*
     * 
     * <wait_for_batch_complete>TEMP.batch_id</wait_for_batch_complete>
     * 
     * suspends the current proc
     * adds a callback for it
     * adds a a batch complete trigger that fires the callback.
     * yields.
     * 
     */
    class MWaitForBatchComplete:IModuleSetup,IModuleRun
    {
        private string batchIdSyntax;
        private IReadString batchIdParsed;
        
        public void Setup(System.Xml.XmlElement me, ModuleContext mc, List<IModuleRun> runModules)
        {
            MWaitForBatchComplete m = new MWaitForBatchComplete();
            m.batchIdSyntax = me.InnerText;
            m.batchIdParsed = mc.ParseSyntax(m.batchIdSyntax);
            runModules.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string batchId = this.batchIdParsed.Read(mc);

            // Get a callback to the next module
            mc.procInst.nextModuleOrder = mc.procDefinition.GetNextModuleOrder(mc.moduleIdx);
            long callbackId = mc.workMgr.CreateCallback(mc.procInst);
           
            // Find the batch work and attach a callback event to it.
            CallbackBatchEvent callbackBatchEvent = new CallbackBatchEvent(callbackId);
            WorkBatch workBatch=mc.mvm.remoteWorkMgr.LookupBatch(long.Parse(batchId));
            workBatch.AddBatchCompleteEvent(callbackBatchEvent);

            mc.Yield();
        }
    }
   
}
