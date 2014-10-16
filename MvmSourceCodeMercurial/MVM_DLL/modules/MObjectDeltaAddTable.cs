using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{

    [Module(@"
        <module_config>
            <name>object_delta_add_table</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType>
                    <xs:sequence>
                        <xs:element name='object_id' type='xs:string' datatype='object_id' mode='in' description='object to add the table to'/>
                        <xs:element name='table_name' type='xs:string' datatype='string' mode='in' description='table this object should persist to'/>
                        <xs:element name='table_status' type='xs:string' maxOccurs='unbounded' datatype='string' mode='in' default='""no_row""' description='cursor of objects as you would like them to appear' values='has_row:this object already exists in this table,no_row:this object does not yet exist in this table,unknown:we do not know if this object exists in this table'/>
                    </xs:sequence>
                </xs:complexType>
            </xsd>
            <doc>
                <category>Delta Objects</category>
                <description>Makes the object persist to the table, doing insert, upsert, update based on table_status</description>
            </doc>
        </module_config>
    ")]
    class MObjectDeltaAddTable: BaseModuleSetup,IModuleRun
    {
        private string objectIdSyntax;
        private IReadString objectIdParsed;
        private string tableNameSyntax;
        private IReadString tableNameParsed;
        private string tableStatusSyntax;
        private IReadString tableStatusParsed;
        public override void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MObjectDeltaAddTable m = new MObjectDeltaAddTable();
            this.SetupReadString(me, mc, "object_id", out m.objectIdSyntax, out m.objectIdParsed);
            this.SetupReadString(me, mc, "table_name", out m.tableNameSyntax, out m.tableNameParsed);
            this.SetupReadString(me, mc, "table_status", out m.tableStatusSyntax, out m.tableStatusParsed);
            run.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            string objectId=this.objectIdParsed.Read(mc);
            string tableName = this.tableNameParsed.Read(mc);
            string tableStatus = this.tableStatusParsed.Read(mc);
            using (IObjectData obj = mc.objectCache.CheckOut(objectId))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj == null) throw new Exception("not a delta tracked object");
                if (deltaObj != null)
                {
                    if (tableStatus.Equals("no_row"))
                    {
                        deltaObj.AddPersistTable(tableName, ObjectDataFormattedDelta.STATUS_NO_ROW);
                    }
                    else if (tableStatus.Equals("has_row"))
                    {
                        deltaObj.AddPersistTable(tableName, ObjectDataFormattedDelta.STATUS_HAS_ROW);
                    }
                    else if (tableStatus.Equals("unknown"))
                    {
                        deltaObj.AddPersistTable(tableName, ObjectDataFormattedDelta.STATUS_UNKNOWN);
                    }
                    else
                    {
                        throw new Exception("error, unknown table_status=[" + tableStatus + "], expecting on of (has_row,no_row,unknown)");
                    }
                }
            }
        }
    }
}
