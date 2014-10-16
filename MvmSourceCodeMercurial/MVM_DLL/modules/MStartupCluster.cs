using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using NLog;
namespace MVM
{

    /*
     * 
     *  Starts up the cluster if it is not already started. Uses GLOBAL.cluster or the passed cluster name
     <startup_cluster>GLOBAL.cluster</startup_cluster>
     
    */
    public class MStartupCluster : IModuleSetup
    {
        public string clusterNameSyntax;
        public IReadString clusterNameParsed;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            if (mc.mvm.ClusterIsStarted || mc.mvm.nodeId!=0)
                return;

            string clusterName=null;
            this.clusterNameSyntax = me.InnerText;
            if (this.clusterNameSyntax.NotNullOrEmpty())
            {
                clusterName = mc.SyntaxReadString(this.clusterNameSyntax);
            }
           
            if (clusterName.IsNullOrEmpty())
                clusterName = mc.globalContext["cluster"];
            if(clusterName.IsNullOrEmpty())
                clusterName = "default";
            mc.mvm.StartupCluster(clusterName);
        }
    }
}
