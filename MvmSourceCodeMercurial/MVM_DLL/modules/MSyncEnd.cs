using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    class MSyncEnd:IModuleSetup,IModuleRun
    {
        public string monitorNameSyntax;
        public IReadString monitorNameParsed;
        #region IModuleSetup Members

        public void Setup(System.Xml.XmlElement me, ModuleContext mc,List<IModuleRun> runModules)
        {
            MSyncEnd m = new MSyncEnd();
            if (me.HasAttribute("name"))
            {
                m.monitorNameSyntax = me.GetAttribute("name").q();
            }
            else if (me.HasAttribute("dynamic_name"))
            {
                m.monitorNameSyntax = me.GetAttribute("dynamic_name");
            }
            else
            {
                throw new Exception("Expecting name or dynamic_name");
            }
            m.monitorNameParsed = mc.ParseSyntax(m.monitorNameSyntax);
            runModules.Add(m);
        }

        #endregion

        #region IModuleRun Members

        public void Run(ModuleContext mc)
        {

            string monitorName = this.monitorNameParsed.Read(mc);
            //Console.WriteLine("sync_end: oid=" + mc.objectData.objectIdSyntax + ",workerNo=" + mc.worker.GetWorkerNoString() + ", " + monitorName);
            try
            {
                mc.globalContext.ExitMonitor(monitorName);
            }
            catch (Exception e)
            {
                throw new Exception("Error cannot exit monitor with monitorName=[" + monitorName + "], syntax=[" + this.monitorNameSyntax + "], cur oid=["+mc.objectData.objectId+"], cur ot=["+mc.objectData.objectType+"]",e);
            }
            mc.procInst.isSync--;
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("sync_end: " + monitorNameSyntax);
        }

        #endregion
    }
    
}
