using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;
using System.Linq;
using System.Net.Sockets;

/*
<set_mvm_cluster>
	<login_object>GLOBAL.target_login</login_object>
	<mvm_cluster_name>GLOBAL.mvm_cluster_name</mvm_cluster_name>
	<node>
		<node_id></node_id>
		<machine></machine>
		<user></user>
		<password></password>		
		<port_start></port_start>
		<port_end></port_end>
	</node>
    <node>
        ...
    </node>
</set_mvm_cluster>
*/
namespace MVM
{
    class MMvmClusterSet : MDbCommon, IModuleSetup
    {
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            // this module only has meaning if this node is the superNode.
            if (!mc.mvm.IsSuperNode) return;

            // the mvm cluster is a super
            MvmClusterSuper superNode=mc.mvm.mvmCluster as MvmClusterSuper;

            // it is useful perhaps to startup more then 1 cluster in a given executions.
            string clusterName = mc.ParseSyntax(me.SelectNodeInnerText("mvm_cluster_name", "'default'")).Read(mc);

            List<Dictionary<string, string>> listDic = new List<Dictionary<string, string>>();
            this.ParseDbCommon(me, mc, "'mvm_cluster_set'");
            me.SelectElements("./node")
                .SelectIndexValuePairs()
                .ForEach(n =>
                {
                    var row = new Dictionary<string, string>();
                    row["order"] = n.index.ToString();
                    var syntaxDic = n.value.GetChildElementDictionary();
                    foreach (var e in syntaxDic)
                    {
                        string v = mc.ParseSyntax(e.Value).Read(mc);
                        row[e.Key] = v;
                    }
                    listDic.Add(row);
                });
            DbUtils.DbExecute(
                    this.GetStaticDbLoginInfo(mc),
                    "delete from mvm_clusters where mvm_cluster_name=" + clusterName.q()
                    );
            foreach (var r in listDic)
            {
                string server = r["server"];
                string bin = r.GetValueDefaulted("bin", "");
                string portStart = r.GetValueDefaulted("port_start", "");
                string portEnd = r.GetValueDefaulted("port_end", "");
                string groupId = r.GetValueDefaulted("group_id", "");
                string numInstances = r.GetValueDefaulted("num_instances", "1");
                DbUtils.DbExecute(
                    this.GetStaticDbLoginInfo(mc), 
                    "insert into mvm_clusters (mvm_cluster_name,server,num_instances,group_id,port_start,port_end,bin) "
                    + " values (" + clusterName.q() + "," + server.q() + "," + numInstances.q() + "," + groupId.q() + "," + portStart.q() + "," + portEnd.q() + "," + bin.q() + ")"
                    );
            }
        }
    }


    

}
