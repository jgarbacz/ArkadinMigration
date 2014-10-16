using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    class MSyncStart:IModuleSetup,IModuleRun
    {
        public string monitorNameSyntax;
        public IReadString monitorNameParsed;

        #region IModuleSetup Members

        public void Setup(System.Xml.XmlElement me, ModuleContext mc,List<IModuleRun> runModules)
        {
            MSyncStart m = new MSyncStart();
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
            bool gotSync=mc.globalContext.TryEnterMonitor(monitorName);
            if (gotSync)
            {
                //FIXED
                mc.procInst.isSync++;
                //Console.WriteLine("sync_start (in): oid="+mc.objectData.objectIdSyntax+",workerNo=" + mc.worker.GetWorkerNoString() + ", " + monitorName);
                return;
            }
            //Console.WriteLine("sync_start (SLEEP/wait): oid=" + mc.objectData.objectIdSyntax + ",workerNo=" + mc.worker.GetWorkerNoString() + ", " + monitorName);
            System.Threading.Thread.Sleep(1);
            mc.YieldAndCallback();
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("sync_start: " + this.monitorNameSyntax);
        }

        #endregion
    }
}
