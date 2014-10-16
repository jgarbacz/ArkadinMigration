using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;
using System.Linq;
using Antlr.Runtime.Tree;

namespace MVM
{
    /*
   
   */

    class MSlaveNodeIdSelect: IModuleSetup,IModuleRun
    {
        private CursorSetupCommon cursorSetup;
        private bool slavesOnly = true;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            mc.mvm.StartupCluster();
            MSlaveNodeIdSelect m = new MSlaveNodeIdSelect();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            m.cursorSetup.cursorValueDefault = "node_id";
            m.slavesOnly = me.LocalName.StartsWith("slave");
            run.Add(m);
            m.cursorSetup.AddCursorSubProcs(me, mc, run);
        }

        public void Run(ModuleContext mc)
        {

            List<IDictionary<string, string>> listDic = new List<IDictionary<string, string>>();
            if (this.slavesOnly)
            {
                foreach (var node in mc.mvmCluster.GetKnownNodes().Where(n => (n.nodeId.NotIn(0, mc.mvmCluster.NodeId) && !n.isProfiler)))
                {
                    listDic.Add(node.ToDictionary());
                }
            }
            else
            {
                foreach (var node in mc.mvmCluster.GetKnownNodes().Where(n => !n.isProfiler))
                {
                    listDic.Add(node.ToDictionary());
                }
            }
            ListOfDictionaryCursor csr = new ListOfDictionaryCursor(mc, this.cursorSetup, listDic);
        }
    }

 

   
}
