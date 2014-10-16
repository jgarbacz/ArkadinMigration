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
    class MServerCredentialsGet : MDbCommon, IModuleSetup
    {

        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            // this module only has meaning if this node is the superNode.
            if (!mc.mvm.IsSuperNode) return;

            // the mvm cluster is a super
            MvmClusterSuper superNode=mc.mvm.mvmCluster as MvmClusterSuper;

            // This is the xml configured or db specified cluster nodes.
            List<Dictionary<string, string>> clusterNodesConfig = new List<Dictionary<string, string>>();

            this.ParseDbCommon(me, mc, "'server_credentials_get'");
            string query = "select server,username,password from mvm_server_credentials";
            clusterNodesConfig = DbUtils.DbQueryToListOfDictionary(mc, this.dbInfo, query);

            foreach (var r in clusterNodesConfig)
            {
                string server = r.GetValueOrNull("server").Nvl(r["SERVER"]);
                string username = r.GetValueOrNull("username").Nvl(r["USERNAME"]);
                string epassword = r.GetValueOrNull("password").Nvl(r["PASSWORD"]);
                string password = TestCrypto.DecryptHexString(epassword);
                superNode.RegisterServerCredentials(server, username, password);
            }
        }
        
    }


    

}
