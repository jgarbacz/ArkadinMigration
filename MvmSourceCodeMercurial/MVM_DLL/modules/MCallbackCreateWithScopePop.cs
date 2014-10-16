using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    class MCallbackCreateWithScopePop:IModuleSetup,IModuleRun
    {
        private string callback;
        private IWriteString parsedCallback;
        #region IModuleSetup Members

        public void Setup(System.Xml.XmlElement moduleElement, ModuleContext mc, List<IModuleRun> runModules)
        {
            MCallbackCreateWithScopePop m = new MCallbackCreateWithScopePop();
            m.callback = moduleElement.InnerText;
            m.parsedCallback = mc.ParseWritableSyntax(m.callback);
            runModules.Add(m);
        }

        #endregion

        #region IModuleRun Members

        public void Run(ModuleContext mc)
        {
            long callbackId = mc.AddCallbackWithScopePop();
            this.parsedCallback.Write(mc,callbackId.ToString());
        }

        public void Log(ModuleContext mc,ILogger log)
        {
            log.LogInfo("callback create with pop");
        }

        #endregion
    }
}
