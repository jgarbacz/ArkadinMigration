using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace MVM
{

/*
 * 
 * <mvm_cluster_sockets>
 * 

  
  <startup_slave_listener>
  <master>GLOBAL.master<master>
  <port>53077</port>
  </startup_slave_listener>   
*/
    public class MStartupSlaveListener : IModuleSetup, IModuleRun
    {
        public string portSyntax;
        public IReadString portParsed;
        public string masterSyntax;
        public IWriteString masterParsed;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MStartupSlaveListener m = new MStartupSlaveListener();
            m.portSyntax = me.SelectNodeInnerText("./master_port");
            m.portParsed = mc.ParseSyntax(m.portSyntax);
            //m.masterSyntax = me.SelectNodeInnerText("./master_id");
            //m.masterParsed = mc.ParseWritableSyntax(m.masterSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            //string port = this.portParsed.Read(mc);
            //int portNo = port.ToInt();
            //SlaveListener slaveListener = new SlaveListener(mc, portNo);
            //mc.globalContext.SetNamedClassInst(mc.mvm.myNodeId, slaveListener);
            //this.masterParsed.Write(mc, slaveListener.listenerId);
        }
    }



   
    
}
