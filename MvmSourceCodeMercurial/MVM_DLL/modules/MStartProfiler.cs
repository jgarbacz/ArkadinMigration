using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading;

namespace MVM
{
    [Module(@"
        <module_config>
            <name>start_profiler</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType>
                    <xs:sequence>
                        <xs:element name='node_id' type='xs:string' minOccurs='0' mode='in' datatype='integer' description='MVM node to profile'/>
                        <xs:element name='sampling_period' type='xs:string' minOccurs='0' mode='in' datatype='integer' default='100' description='number of milliseconds to sleep between samples'/>
                        <xs:element name='reporting_count' type='xs:string' minOccurs='0' mode='in' datatype='integer' default='10' description='number of samples to gather between sending results'/>
                    </xs:sequence>
                </xs:complexType>
            </xsd>
            <doc>
                <category>Profiler</category>
                <description>Start profiling the performance of an MVM process</description>
            </doc>
        </module_config>
    ")]
    class MStartProfiler : BaseModuleSetup, IModuleRun
    {
        private string nodeIdSyntax;
        private IReadString nodeIdParsed;

        private string samplingPeriodSyntax;
        private IReadString samplingPeriodParsed;
        private bool haveSamplingPeriod;

        private string reportingCountSyntax;
        private IReadString reportingCountParsed;
        private bool haveReportingCount;

        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MStartProfiler m = new MStartProfiler();
            m.SetupReadString(me, mc, "./node_id", out m.nodeIdSyntax, out m.nodeIdParsed);
            if (m.nodeIdParsed == null)
            {
                m.nodeIdParsed = mc.ParseSyntax(mc.mvm.nodeId.ToString());
            }
            m.SetupReadString(me, mc, "./sampling_period", out m.samplingPeriodSyntax, out m.samplingPeriodParsed);
            m.haveSamplingPeriod = me.SelectSingleElem("./sampling_period") != null;
            m.SetupReadString(me, mc, "./reporting_count", out m.reportingCountSyntax, out m.reportingCountParsed);
            m.haveReportingCount = me.SelectSingleElem("./reporting_count") != null;
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            int nodeId = this.nodeIdParsed.Read(mc).ToInt();
            int samplingPeriod = this.samplingPeriodParsed.Read(mc).ToInt();
            int reportingCount = this.reportingCountParsed.Read(mc).ToInt();
            if (mc.mvmCluster.GetProfilerNode() == null && !mc.mvm.TriedToStartProfiler)
            {
                mc.mvm.StartProfilerNode(samplingPeriod, reportingCount);
            }
            if (!mc.mvm.StartedProfilerThread.ContainsKey(nodeId))
            {
                mc.mvmCluster.GetClusterNode(nodeId).SocketHandler.messageOutbox.Add(new ProfilerInitMessage(samplingPeriod, reportingCount));
                mc.mvm.StartedProfilerThread[nodeId] = true;
            }
            if (nodeId == mc.mvm.nodeId)
            {
                mc.mvmCluster.profilerActive = true;
            }
            else
            {
                ProfilerStartMessage msg = new ProfilerStartMessage(haveSamplingPeriod ? samplingPeriod : -1, haveReportingCount ? reportingCount : -1);
                mc.mvmCluster.GetClusterNode(nodeId).SocketHandler.messageOutbox.Add(msg);
            }
        }
    }
}
