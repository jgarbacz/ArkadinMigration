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
  <startup_slave>
  <master>GLOBAL.master</master>
  <slave>TEMP.slave1</slave>
  <machine>localhost</machine>
  </startup_slave>
    */


    public class MStartupSlaves : IModuleSetup, IModuleRun
    {
        public string slaveCountSyntax;
        public IReadString slaveCountParsed;
        public string numStartedSyntax;
        public IWriteString numStartedParsed;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MStartupSlaves m = new MStartupSlaves();

            m.slaveCountSyntax = me.SelectNodeInnerText("./slave_count","*");
            m.slaveCountParsed = mc.ParseSyntax(m.slaveCountSyntax);

            m.numStartedSyntax = me.SelectNodeInnerText("./num_started");
            m.numStartedParsed = mc.ParseWritableSyntax(m.numStartedSyntax);

            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string slaveCount = this.slaveCountParsed.Read(mc);
            // * means startup all you can
            int slaveCountInt = int.MaxValue ;
            if (!slaveCount.Equals("*"))
            {
                slaveCountInt = slaveCount.ToInt();
            }

            // Make sure the cluster is setup.
            mc.mvm.mvmCluster.Setup();
           
            // Request N slaves, record how many were actually granted.
            List<int> newSlaveIds=mc.mvm.mvmCluster.RequestSlaves(slaveCountInt);
            this.numStartedParsed.Write(mc, newSlaveIds.Count.ToString());

            // need ability to loop thru your slaves.
        }
    }


}
