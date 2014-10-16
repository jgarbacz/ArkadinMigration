using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MVM
{
    [Module(@"
        <module_config>
            <name>exit_code</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:element type='xs:string' datatype='integer' mode='in' description='desired exit code'/>
            </xsd>
            <doc>
                <category>Miscellaneous</category>
                <description>Sets the exit code that the MVM process will emit if no errors are encountered</description>
            </doc>
        </module_config>
    ")]
    class MExitCode : BaseModuleSetup, IModuleRun
    {
        private string valueSyntax;
        private IReadString valueParsed;
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MExitCode m = new MExitCode();
            m.SetupReadString(me, mc, out m.valueSyntax, out m.valueParsed);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            mc.mvm.exitCode = this.valueParsed.Read(mc).ToInt();
        }
    }
}
