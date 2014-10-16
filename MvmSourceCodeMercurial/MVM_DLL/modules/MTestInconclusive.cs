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
            <name>test_inconclusive</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:element type='xs:string' datatype='string' mode='in' description='Reason'/>
            </xsd>
            <doc>
                <category>Unit Testing</category>
                <description>Indicates the current unit test is inconclusive</description>
            </doc>
        </module_config>
    ")]
    class MTestInconclusive: BaseModuleSetup,IModuleRun
    {
        private string valueSyntax;
        private IReadString valueParsed;
        public override void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MTestInconclusive m = new MTestInconclusive();
            m.SetupReadString(me, mc, out m.valueSyntax, out m.valueParsed);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string val=this.valueParsed.Read(mc);
            mc.mvm.Log("[TEST_INCONCLUSIVE]" + val);
            mc.ThrowException("nunit_inconclusive", val);
        }
    }
}
