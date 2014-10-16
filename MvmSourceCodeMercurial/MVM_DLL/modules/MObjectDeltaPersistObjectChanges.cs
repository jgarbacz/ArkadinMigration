using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MVM
{
     [Module(@"
        <module_config>
            <name>object_delta_persist_changes</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType>
                    <xs:sequence>
                        <xs:element name='persisted_object_id' type='xs:string' datatype='object_id' mode='in' description='object as it appears in the database'/>
                        <xs:element name='current_object_id' type='xs:string' datatype='object_id' mode='in' description='object in its current form'/>
                    </xs:sequence>
                </xs:complexType>
            </xsd>
            <doc>
                <category>Delta Objects</category>
                <description>Makes the persisted_object_id in the database looks like the current_object_id</description>
            </doc>
        </module_config>
    ")]
    class MObjectDeltaPersistObjectChanges: BaseModuleSetup,IModuleRun
    {
        private string persistedObjectIdSyntax;
        private IReadString persistedObjectIdParsed;
        private string currentObjectIdSyntax;
        private IReadString currentObjectIdParsed;
        public override void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MObjectDeltaPersistObjectChanges m = new MObjectDeltaPersistObjectChanges();
            this.SetupReadString(me, mc, "persisted_object_id", out m.persistedObjectIdSyntax, out m.persistedObjectIdParsed);
            this.SetupReadString(me, mc, "current_object_id", out m.currentObjectIdSyntax, out m.currentObjectIdParsed);
            run.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            string persistedOid = this.persistedObjectIdParsed.Read(mc);
            string currentOid = this.currentObjectIdParsed.Read(mc);
            using (ObjectDataFormattedDelta currentObj = mc.objectCache.CheckOut(currentOid) as ObjectDataFormattedDelta)
            {
                using (ObjectDataFormattedDelta persistedObj = mc.objectCache.CheckOut(persistedOid) as ObjectDataFormattedDelta)
                {
                    persistedObj.PersistObjectChanges(persistedObj, currentObj);
                }
            }
        }
    }
}
