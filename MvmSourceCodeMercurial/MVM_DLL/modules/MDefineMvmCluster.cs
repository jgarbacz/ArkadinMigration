using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;
using System.Linq;
using System.Net.Sockets;
/*

*/
namespace MVM
{
    class MDefineMvmCluster : MDbCommon, IModuleSetup
    {

        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
        //    // this module only has meaning if this node is the superNode.
        //    if (!mc.mvm.IsSuperNode) return;

        //    // the mvm cluster is a super
        //    MvmClusterSuper superNode=mc.mvm.mvmCluster as MvmClusterSuper;

        //    // This is the xml configured or db specified cluster nodes.
        //    List<Dictionary<string, string>> clusterNodesConfig = new List<Dictionary<string, string>>();

        //    // If a query is defined use that to get the list of nodes
        //    XmlElement queryElem = me.SelectSingleElem("./query");
        //    if (queryElem != null)
        //    {
        //        this.ParseDbCommon(me, mc, null);
        //        string query = mc.SyntaxReadString(queryElem.InnerText);
        //        clusterNodesConfig = DbUtils.DbQueryToListOfDictionary(mc, this.dbInfo, query);
        //    }

        //    // Otherwise, we expect they are explicitly defined in the xml
        //    else
        //    {
        //        me.SelectElements("./node")
        //            .SelectIndexValuePairs()
        //            .ForEach(n =>
        //            {
        //                var row = new Dictionary<string, string>();
        //                row["order"] = n.index.ToString();
        //                row.AddAll(n.value.GetAttributeDictionary());
        //                clusterNodesConfig.Add(row);
        //            });
        //    }

        //    foreach (var r in clusterNodesConfig)
        //    {
        //        string machine = r["machine"];
        //        if(machine.Equals("localhost"))
        //            machine = System.Environment.MachineName;
        //        string user = r["user"];
        //        string password = r["password"];
        //        string bin = r["bin"].Nvl(mc.mvm.rmpBinDir);
        //        int portStart = r["port_start"].ToInt();
        //        int portEnd = r["port_end"].Nvl(r["port_start"]).ToInt();
        //        int numIntances = r["num_instances"].ToInt();
        //        string groupId = r["group_id"];
                
        //        // Define all the available ports
        //        superNode.RegisterMachinePortRange(machine, portStart, portEnd);

        //        // Define the instance count for the machine.
        //        for (int i = 1; i <= numIntances; i++)
        //            superNode.RegisterConfiguredNode(machine, user, password, bin, groupId);
        //    }
            }


}
}
