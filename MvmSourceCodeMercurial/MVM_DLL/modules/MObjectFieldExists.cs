using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    [Module(@"
        <module_config>
            <name>object_field_exists</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType>
                    <xs:all>
                        <xs:element name='object_id' type='xs:string' datatype='object_id' mode='in' default='OBJECT.object_id' description='object id to test'/>
                        <xs:element name='field_name' type='xs:string' datatype='string' mode='in' description='name of the field to check for'/>
                        <xs:element name='output' type='xs:string' datatype='boolean' mode='out' values='0:field does not exist,1:field exists'/>
                    </xs:all>
                </xs:complexType>
            </xsd>
            <test>
                <object_field_exists>
                    <object_id>object(a=>1,b=>'foo')</object_id>
                    <field_name>'b'</field_name>
                    <output>1</output>
                </object_field_exists>
                <object_field_exists>
                    <object_id>object(a=>1)</object_id>
                    <field_name>'b'</field_name>
                    <output>0</output>
                </object_field_exists>
            </test>
            <doc>
                <category>Object Access</category>
                <description>Tests whether a given field name is defined on an object</description>
            </doc>
        </module_config>
    ")]
    class MObjectFieldExists : BaseModuleSetup, IModuleRun
    {
        // from xml
        private string objectIdSyntax;
        private string fieldNameSyntax;
        private string outputSyntax;

        // from setup
        private IReadString objectIdParsed;
        private IReadString fieldNameParsed;
        private IWriteString outputParsed;

        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MObjectFieldExists m = new MObjectFieldExists();
            m.SetupReadString(me, mc, "./object_id", out m.objectIdSyntax, out m.objectIdParsed);
            m.SetupReadString(me, mc, "./field_name", out m.fieldNameSyntax, out m.fieldNameParsed);
            m.SetupWriteString(me, mc, "./output", out m.outputSyntax, out m.outputParsed);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string objectId = this.objectIdParsed.Read(mc);
            string fieldName = this.fieldNameParsed.Read(mc);
            using (IObjectData obj = mc.objectCache.CheckOut(objectId))
            {
                if (obj.ContainsField(fieldName)) outputParsed.Write(mc, "1");
                else outputParsed.Write(mc, "0");
            }
        }
        }
    }
