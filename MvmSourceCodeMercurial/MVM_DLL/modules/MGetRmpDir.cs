using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    [Module(@"
        <module_config>
            <name>get_rmp_dir</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:element type='xs:string' datatype='string' mode='out' description='path of RMP directory'/>
            </xsd>
            <test example_only='1'>
                <get_rmp_dir>'D:\MetraTech\RMP'</get_rmp_dir>
            </test>
            <doc>
                <category>MetraNet</category>
                <description>Returns the full path of the RMP directory</description>
            </doc>
        </module_config>
    ")]
    class MGetRmpDir : BaseModuleSetup, IModuleRun
    {

        private string valueSyntax;
        private IWriteString valueParsed;
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MGetRmpDir m = new MGetRmpDir();
            // xml extraction
            m.valueSyntax = me.InnerText;
            // parsing
            m.valueParsed = mc.ParseWritableSyntax(m.valueSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string output = mc.mvm.rmpDir;
            this.valueParsed.Write(mc, output);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("get_rmp_dir:" + this.valueSyntax);
        }
    }
}
