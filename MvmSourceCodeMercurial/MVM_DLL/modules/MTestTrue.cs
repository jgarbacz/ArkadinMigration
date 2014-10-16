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
            <name>test_true</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:element type='xs:string' datatype='bool' mode='in' description='Condition that must be true'/>
            </xsd>
            <doc>
                <category>Unit Testing</category>
                <description>Must be true for unit test to pass</description>
            </doc>
        </module_config>
    ")]
    class MTestTrue: BaseModuleSetup,IModuleRun
    {
        private string valueSyntax;
        private IReadString valueParsed;

         
        public override void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MTestTrue m = new MTestTrue();
            m.SetupReadString(me, mc, out m.valueSyntax, out m.valueParsed);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string val=this.valueParsed.Read(mc);
            string msg= "[TEST_TRUE] "+this.valueSyntax +"=[" + val+"]";
            if (val.Equals("1"))
            {
                mc.mvm.Log(msg+" passed");
            }
            else
            {
                mc.mvm.Log(msg + " FAILED");
                mc.ThrowException("nunit_fail", msg + " FAILED");
            }
        }
    }
}
