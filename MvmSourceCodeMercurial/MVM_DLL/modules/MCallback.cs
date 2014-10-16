using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    class MCallback:IModuleSetup,IModuleRun
    {
        private string callback;
        private IReadString parsedCallback;
        #region IModuleSetup Members

        public void Setup(System.Xml.XmlElement moduleElement, ModuleContext mc, List<IModuleRun> runModules)
        {
            MCallback m = new MCallback();
            m.callback = moduleElement.InnerText;
            m.parsedCallback = mc.ParseSyntax(m.callback);
            runModules.Add(m);
        }

        #endregion

        #region IModuleRun Members

        public void Run(ModuleContext mc)
        {
            long callbackId = this.parsedCallback.Read(mc).ToLong();
            if (mc.breakFromProcName != null) throw new Exception("Not expecting to have nonnull breakToLabel=[" + mc.breakFromProcName + "] when we reach module <callback>");
            if (mc.exception != null) throw new Exception("Not expecting to have nonnull exception=[" + mc.exception.message + "] when we reach module <callback>");
            mc.workMgr.FireCallback(callbackId);
        }

        #endregion
    }
}
