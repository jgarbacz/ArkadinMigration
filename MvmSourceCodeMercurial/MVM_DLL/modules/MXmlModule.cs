using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Threading;

namespace MVM
{
    public class MXmlModule : IModuleRun, IModuleLabel
    {
        private XmlElement moduleElem;
        private IModuleSetup moduleSetup;

        // Constructor
        public MXmlModule(XmlElement moduleElem, IModuleSetup moduleSetup)
        {
            this.moduleElem = moduleElem;
            this.moduleSetup = moduleSetup;
        }

        // Override this to do the procInst we want to do
        public void Run(ModuleContext mc)
        {
            if (Monitor.TryEnter(this))
            {
                try
                {
                    List<IModuleRun> runModules = new List<IModuleRun>();

                    if (mc.globalContext["trace_compile"].Equals("1"))
                    {
                        mc.mvm.Log("[trace]" + moduleElem.OuterXml);
                    }

                    int currentModule = mc.moduleIdx;
                    ModuleOrder currentModuleOrder = mc.moduleOrder;

                    // save location info in case this bombs on compiling this xml module
                    {
                        var moduleLookupKey = mc.procId + ":" + currentModuleOrder.ToString();
                        mc.worker.ModuleOrderElementMap[moduleLookupKey] = this.moduleElem;
                    }

                    // run the module
                    moduleSetup.Setup(this.moduleElem, mc, runModules);
                    mc.ReplaceCurModuleWith(runModules.ToArray());

                    // map the generated modules back to the setup module element
                    if (runModules.Count > 0)
                    {
                        foreach (var runModuleOrder in mc.GetModuleOrders(currentModuleOrder, runModules.Count))
                        {
                            var moduleLookupKey = mc.procId + ":" + runModuleOrder.ToString();
                            mc.worker.ModuleOrderElementMap[moduleLookupKey] = this.moduleElem;

                        }
                    }
                }
                catch (Exception e)
                {
                    Regex regex = XmlConfigParser.LineInfoRegex;
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("cannot perform xml module:");
                    string moduleString = this.moduleElem.PrettyString();
                    using (StringReader reader = new StringReader(moduleString))
                    {
                        int maxLines = 5;
                        int count = 0;
                        string line = reader.ReadLine();
                        while (count < maxLines && line != null)
                        {
                            sb.AppendLine(regex.Replace(line, ""));
                            line = reader.ReadLine();
                            count++;
                        }
                        if (line != null && count == maxLines)
                        {
                            sb.AppendLine("[additional lines omitted]");
                        }
                    }
                    throw MvmUserException.Create("ModuleException", sb.ToString(), e);
                }
            }
            mc.YieldAndCallback();
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("setup:" + this.moduleElem.LocalName);
            //log.LogInfo("setup:" + this.moduleElem.OuterXml); 
        }

        #region IModuleLabel Members

        public string GetLabel()
        {
            IModuleLabel moduleLabel = this.moduleSetup as IModuleLabel;
            if (moduleLabel != null)
            {
                string label = moduleLabel.GetLabel();
                if (label != null)
                    return label;
                if (this.moduleElem.LocalName.Equals("label"))
                {
                    label = this.moduleElem.InnerText.StripQuotes();
                    return label;
                }
                label = this.moduleElem.GetAttribute("label");
                if (!label.Equals(""))
                    return label;
                return null;
            }
            return null;
        }

        #endregion
    }
}
