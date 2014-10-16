using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    [Module(@"
        <module_config>
            <name>block</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:element type='proc_type'/>
            </xsd>
            <doc>
                <category>Control Flow</category>
                <description>A block is a container for a set of modules, similar to a proc</description>
            </doc>
        </module_config>
    ")]
    class MBlock : BaseModuleSetup
    {
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MBlock m = new MBlock();
            string childProcName = mc.GetChildProcName("block");
            mc.ReadXmlProcFromElem(childProcName, me);
            run.Add(mc.GetModuleRun("<call_proc_for_current_object_nested><name>'" + childProcName + "'</name></call_proc_for_current_object_nested>"));
        }
    }
}
