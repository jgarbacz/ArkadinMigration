using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{

    /*
     * <test_outbox_speed>
     * <round_trips>10</round_trips>
     * <node_id></node_id>
     * </test_outbox>
     */
    class MTestOutboxSpeed : IModuleSetup, IModuleRun
    {
        private string roundTripsSyntax;
        private IReadString roundTripsParsed;

        private string nodeIdSyntax;
        private IReadString nodeIdParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MTestOutboxSpeed m = new MTestOutboxSpeed();
            // xml extraction
            m.roundTripsSyntax = me.SelectNodeInnerText("./round_trips");
            m.nodeIdSyntax = me.SelectNodeInnerText("./node_id");
            // parsing
            m.roundTripsParsed = mc.ParseSyntax(m.roundTripsSyntax);
            m.nodeIdParsed = mc.ParseSyntax(m.nodeIdSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            int nodeId = this.nodeIdParsed.Read(mc).ToInt();
            int roundTrips = this.roundTripsParsed.Read(mc).ToInt() * 2;
            OutboxCountingMessage msg = new OutboxCountingMessage(0,roundTrips);
            Console.WriteLine("-------------- TestOutboxSpeed start:" + DateTime.Now);
            mc.mvm.mvmCluster.GetClusterNode(nodeId).SocketHandler.messageSender.SendImmediate(msg);
        }
    }
}
