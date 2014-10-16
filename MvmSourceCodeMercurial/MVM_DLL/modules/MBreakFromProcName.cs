using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MVM
{
    [Module(@"
        <module_config>
            <name>break_from_proc</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType/>
            </xsd>
            <doc>
                <category>Control Flow</category>
                <description>Immediately exits from the current proc</description>
            </doc>
        </module_config>
    ")]
    class MBreakFromProcName : BaseModuleSetup, IModuleRun
    {
        string breakToProcNameSyntax;
        IReadString breakToProcNameParsed;

        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MBreakFromProcName m = new MBreakFromProcName();
            m.breakToProcNameSyntax = me.InnerText;
            m.breakToProcNameParsed = mc.ParseSyntax(m.breakToProcNameSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string breakToProcName = this.breakToProcNameParsed.Read(mc);
            mc.breakFromProcName = breakToProcName;
            mc.moduleStatus = ModuleStatus.BreakFromProcName;
        }
    }
}
