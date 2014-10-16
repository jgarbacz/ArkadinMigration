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
    class MMvmClusterGet : MDbCommon, IModuleSetup
    {

        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            //// this module only has meaning if this node is the superNode.
            //if (!mc.mvm.IsSuperNode) return;

            //// the mvm cluster is a super
            //MvmClusterSuper superNode=mc.mvm.mvmCluster as MvmClusterSuper;

            //// This is the xml configured or db specified cluster nodes.
            //List<Dictionary<string, string>> clusterNodesConfig = new List<Dictionary<string, string>>();

            //this.ParseDbCommon(me, mc, "'mvm_cluster_get'");
            //string mvmClusterName = mc.ParseSyntax(me.SelectNodeInnerText("mvm_cluster_name", "'default'")).Read(mc);
            //string query = "select mvm_cluster_name,server,num_instances,port_start,port_end,bin,group_id from mvm_clusters where mvm_cluster_name="+mvmClusterName.q();
            //clusterNodesConfig = DbUtils.DbQueryToListOfDictionary(mc, this.dbInfo, query);

            //foreach (var r in clusterNodesConfig)
            //{
            //    string mvm_cluster_name = r.GetValueOrNull("mvm_cluster_name").Nvl(r["MVM_CLUSTER_NAME"]);
            //    string server = r.GetValueOrNull("server").Nvl(r["SERVER"]);
            //    if (server.Equals("localhost"))
            //        server = System.Environment.MachineName;
            //    string num_instances = r.GetValueOrNull("num_instances").Nvl(r["NUM_INSTANCES"]);
            //    string port_start = r.GetValueOrNull("port_start").Nvl(r["PORT_START"]);
            //    string port_end = r.GetValueOrNull("port_end").Nvl(r["PORT_END"]);
            //    string bin = r.GetValueOrNull("bin").Nvl(r["BIN"]);
            //    string group_id = r.GetValueOrNull("group_id").Nvl(r["GROUP_ID"]);

            //    // Define all the available ports
            //    int portStart = port_start.Nvl("50000").ToInt();
            //    int portEnd = port_end.Nvl("50099").ToInt();
            //    superNode.RegisterMachinePortRange(server, portStart, portEnd);

            //    // Define the instance count for the machine.
            //    int numInstances = num_instances.ToInt();
            //    for (int i = 1; i <= numInstances; i++)
            //        superNode.RegisterConfiguredNode(server, "", "", bin,group_id);
            //}
            }
        }


    

}
