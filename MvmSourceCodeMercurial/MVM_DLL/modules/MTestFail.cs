using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;
using NUnit.Framework;
namespace MVM
{

    [Module(@"
        <module_config>
            <name>test_fail</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:element type='xs:string' datatype='string' mode='in' description='Error message'/>
            </xsd>
            <doc>
                <category>Unit Testing</category>
                <description>Indicates the current unit test has failed</description>
            </doc>
        </module_config>
    ")]
    class MTestFail: BaseModuleSetup,IModuleRun
    {
        private string valueSyntax;
        private IReadString valueParsed;
        public override void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MTestFail m = new MTestFail();
            m.SetupReadString(me, mc, out m.valueSyntax, out m.valueParsed);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string val=this.valueParsed.Read(mc);
            mc.mvm.Log("[TEST_FAIL]" + val);
            mc.ThrowException("nunit_fail", val);
        }
    }
}
