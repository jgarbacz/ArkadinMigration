using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    [Module(@"
        <module_config>
            <name>object_field_value_select</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType>
                    <xs:sequence>
                        <xs:element name='object_id' type='xs:string' minOccurs='0' datatype='object_id' mode='in' default='OBJECT.object_id' description='object with field/value pairs'/>
                        <xs:element name='view_object_id' type='xs:string' minOccurs='0' datatype='object_id' mode='in' default='MODULE.object_id' description='select only fields present on this object'/>
                        <xs:element name='show_internal_fields' type='xs:string' minOccurs='0' datatype='boolean' mode='in' default='0' values='1:display internal fields,0:hide internal fields'/>
                        <xs:element name='cursor_field' type='xs:string' minOccurs='0' datatype='string' mode='in' default='""field""' description='output cursor field holding field names'/>
                        <xs:element name='cursor_value' type='xs:string' minOccurs='0' datatype='string' mode='in' default='""value""' description='output cursor field holding field values'/>
                        <xs:group ref='cursor_operation_group'/>
                    </xs:sequence>
                </xs:complexType>
            </xsd>
            <doc>
                <category>Object Access</category>
                <description>Returns a cursor that iterates through an object's field/value pairs</description>
            </doc>
        </module_config>
    ")]
    class MObjectFieldValueSelect : BaseModuleSetup, IModuleRun
    {
        // from xml
        private string objectSyntax;
        private string viewObjectSyntax;
        private string cursorValueSyntax;
        private string cursorFieldSyntax;
        private string showInternalFieldsSyntax;

        // from setup
        private IReadString objectParsed;
        private IReadString viewObjectParsed;
        private IReadString cursorFieldParsed;
        private IReadString cursorValueParsed;
        private IReadString showInternalFieldsParsed;

        private CursorSetupCommon cursorSetup;
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MObjectFieldValueSelect m = new MObjectFieldValueSelect();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            // xml extraction
            m.cursorFieldSyntax = m.SelectSingleNode(me, "./cursor_field");
            m.cursorValueSyntax = m.SelectSingleNode(me, "./cursor_value");
            m.objectSyntax = m.SelectSingleNode(me, "./object_id");
            m.viewObjectSyntax = me.SelectNodeInnerText("./view_object_id", m.objectSyntax);
            m.showInternalFieldsSyntax = m.SelectSingleNode(me, "./show_internal_fields");
            // parsing
            m.objectParsed = mc.ParseSyntax(m.objectSyntax);
            m.viewObjectParsed = mc.ParseSyntax(m.viewObjectSyntax);
            m.cursorValueParsed = mc.ParseSyntax(m.cursorValueSyntax);
            m.cursorFieldParsed = mc.ParseSyntax(m.cursorFieldSyntax);
            m.showInternalFieldsParsed = mc.ParseSyntax(m.showInternalFieldsSyntax);
            
            run.Add(m);
            m.cursorSetup.AddCursorSubProcs(me, mc, run);
        }

        public void Run(ModuleContext mc)
        {
            // setup the cursor
            string cursorField = this.cursorFieldParsed.Read(mc);
            string cursorValue = this.cursorValueParsed.Read(mc);
            string objectId = this.objectParsed.Read(mc);
            
            string viewObjectId = this.viewObjectParsed.Read(mc);
            string showInternalFields = this.showInternalFieldsParsed.Read(mc);
            bool skipInternalFields = !showInternalFields.Equals("1");

            List<string> orderedFieldNames = new List<string>();
            orderedFieldNames.Add(cursorField);
            orderedFieldNames.Add(cursorValue);

            List<List<string>> keyValuePairs = new List<List<string>>();
            using (IObjectData viewObj = mc.objectCache.CheckOut(viewObjectId))
            {
                using (IObjectData obj = mc.objectCache.CheckOut(objectId))
                {
                    foreach (var kv in viewObj.FieldKeyValuesPairs)
                    {
                        string field = kv.Key;
                        if (skipInternalFields&&ModuleContext.IsInternalObjectField(field)) continue;
                        List<string> values = new List<string>(2);
                        values.Add(field);
                        values.Add(kv.Value);
                        keyValuePairs.Add(values);
                    }
                }
            }
            var csr = new MultiFieldListCursor(mc, this.cursorSetup, orderedFieldNames, keyValuePairs);
        }
    }
    }
