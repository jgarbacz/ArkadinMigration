using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading;

namespace MVM
{
    [Module(@"
        <module_config>
            <name>stop_profiler</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType>
                    <xs:sequence>
                        <xs:element name='node_id' type='xs:string' minOccurs='0' mode='in' datatype='integer' description='MVM node to stop profiling'/>
                    </xs:sequence>
                </xs:complexType>
            </xsd>
            <doc>
                <category>Profiler</category>
                <description>Stop profiling an MVM process</description>
            </doc>
        </module_config>
    ")]
    class MStopProfiler : BaseModuleSetup, IModuleRun
    {
        private string nodeIdSyntax;
        private IReadString nodeIdParsed;

        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MStopProfiler m = new MStopProfiler();
            m.SetupReadString(me, mc, "./node_id", out m.nodeIdSyntax, out m.nodeIdParsed);
            if (m.nodeIdParsed == null)
            {
                m.nodeIdParsed = mc.ParseSyntax(mc.mvm.nodeId.ToString());
            }
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            int nodeId = this.nodeIdParsed.Read(mc).ToInt();
            if (nodeId == mc.mvm.nodeId)
            {
                mc.mvmCluster.profilerActive = false;
            }
            else
            {
                mc.mvmCluster.GetClusterNode(nodeId).SocketHandler.messageOutbox.Add(new ProfilerStopMessage());
            }
        }
    }
}
