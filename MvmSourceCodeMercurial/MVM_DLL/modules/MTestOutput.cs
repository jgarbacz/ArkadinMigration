using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;
namespace MVM
{

     [Module(@"
        <module_config>
            <name>test_output</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:element type='xs:string' datatype='string' mode='in' description='test output'/>
            </xsd>
            <doc>
                <category>Unit Testing</category>
                <description>Writes output for the test. Correct tests should always output the same thing each time it is run. Do not include things that could change like timestamps etc.</description>
            </doc>
        </module_config>
    ")]
    class MTestOutput: BaseModuleSetup,IModuleRun
    {
        private string valueSyntax;
        private IReadString valueParsed;
        public override void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MTestOutput m = new MTestOutput();
            m.SetupReadString(me, mc, out m.valueSyntax, out m.valueParsed);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string val=this.valueParsed.Read(mc);
            mc.mvm.Log("[TEST_OUTPUT]" + val);

            string unitTestProcName=mc.globalContext["unit_test_proc_name"];
            if(unitTestProcName.NotNullOrEmpty()){
                string fileName =MTestOutputDiff.GetProcTestOutputFile(mc,unitTestProcName);
                StreamWriter w = mc.threadContext.GetStreamWriter(fileName);
                w.WriteLine(val);
            }

        }
    }
}
