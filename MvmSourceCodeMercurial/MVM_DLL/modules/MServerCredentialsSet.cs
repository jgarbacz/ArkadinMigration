using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;
using System.Linq;
using System.Net.Sockets;
/*
<server_credentials_set>
<login_object>GLOBAL.target_login</login_object>
<machine></machine>
<user></user>
<password></password>
</server_credentials_set>
*/
namespace MVM
{
    class MServerCredentialsSet : MDbCommon, IModuleSetup
    {
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            // this module only has meaning if this node is the superNode.
            if (!mc.mvm.IsSuperNode) return;

            // the mvm cluster is a super
            MvmClusterSuper superNode = mc.mvm.mvmCluster as MvmClusterSuper;

            // This is the xml configured or db specified cluster nodes.
            List<Dictionary<string, string>> clusterNodesConfig = new List<Dictionary<string, string>>();
            this.ParseDbCommon(me, mc, "'server_credentials_set'");
            var syntaxDic = me.GetChildElementDictionary();
            var row = new Dictionary<string, string>();
            foreach (var e in syntaxDic)
            {
                string v = mc.ParseSyntax(e.Value).Read(mc);
                row[e.Key] = v;
            }
            clusterNodesConfig.Add(row);

            foreach (var r in clusterNodesConfig)
            {
                string server = MvmClusterNode.ResolveLocalName(r["server"]);
                string username = r["username"];
                string password = r["password"];

                // encrypt the pw
                string ePassword = TestCrypto.EncryptToHexString(password);

                DbUtils.DbExecute(
                    this.GetStaticDbLoginInfo(mc),
                     "delete from mvm_server_credentials where server=" + server.q() + "and username=" + username.q()
                );
                DbUtils.DbExecute(
                   this.GetStaticDbLoginInfo(mc),
                   "insert into mvm_server_credentials (server,username,password) "
                   + " values (" + server.q() + "," + username.q() + "," + ePassword.q() + ")"
                );
            }
        }
    }
}
