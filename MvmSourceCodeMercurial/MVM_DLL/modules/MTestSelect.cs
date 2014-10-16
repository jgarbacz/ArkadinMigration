using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;
using System.Linq;
using Antlr.Runtime.Tree;

namespace MVM
{
    [Module(@"
        <module_config>
            <name>test_select</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
               <xs:complexType>
                <xs:sequence>
                  <xs:sequence>
                    <xs:element name='cursor_value' type='xs:string' minOccurs='0' maxOccurs='1' mode='in' datatype='string' description='cursor field name' default='""value""'/>
                    <xs:group ref='cursor_operation_group'/>
                  </xs:sequence>
                </xs:sequence>
              </xs:complexType>
            </xsd>
            <doc>
                <category>Unit Testing</category>
                <description>Returns list of test proc names in the form of NAMESPACE.proc_name</description>
            </doc>
        </module_config>
    ")]
    class MTestSelect: BaseModuleSetup,IModuleRun
    {
        private CursorSetupCommon cursorSetup;
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MTestSelect m = new MTestSelect();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            run.Add(m);
            m.cursorSetup.AddCursorSubProcs(me, mc, run);
        }

        public void Run(ModuleContext mc)
        {
            

            var stringList = mc.mvm.workMgr.schedulerMaster.GetMvmScriptUnitTests().Select(pi => pi.procName).ToList();
            var csr = new SingleFieldListCursor(mc, this.cursorSetup, stringList);
        }

    }

 

   
}
