﻿using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    [Module(@"
        <module_config>
            <name>atomic_increment</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:element type='atomic_incr_decr_type'/>
            </xsd>
            <test>
                <atomic_increment>
                    <object_id>object(a=>1,b=>2)</object_id>
                    <field_name>'b'</field_name>
                    <output>3</output>
                </atomic_increment>
            </test>
            <doc>
                <category>Object Access</category>
                <description>Increments a field on an object as an atomic operation</description>
            </doc>
        </module_config>
    ")]
    class MAtomicIncrement : BaseModuleSetup, IModuleRun
    {
        private string OidSyntax;
        private IReadString OidParsed;

        private string fieldNameSyntax;
        private IReadString fieldNameParsed;

        private string outputSyntax;
        private IWriteString outputParsed;

        public override  void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MAtomicIncrement m = new MAtomicIncrement();
            // xml extraction
            m.OidSyntax = m.SelectSingleNode(me, "./object_id");
            m.fieldNameSyntax = m.SelectSingleNode(me, "./field_name");
            m.outputSyntax = m.SelectSingleNode(me, "./output");

            m.OidParsed = mc.ParseSyntax(m.OidSyntax);
            m.fieldNameParsed = mc.ParseSyntax(m.fieldNameSyntax);
            m.outputParsed = mc.ParseWritableSyntax(m.outputSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string oid=this.OidParsed.Read(mc);
            string fieldName = this.fieldNameParsed.Read(mc);
            string output=mc.IncrementObjectField(oid, fieldName);
            this.outputParsed.Write(mc, output);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("atomic incr:");
        }
    }
}
