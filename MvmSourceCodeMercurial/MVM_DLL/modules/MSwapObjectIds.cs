using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

/*
  
<inherit_object>
<objectId1>TEMP.csr</objectId1>
<objectId2>TEMP.oid</objectId2> // default to OBJECT.object_id
</inherit_object>
 
 */
using System.Text.RegularExpressions;
namespace MVM
{

    [Module(@"
        <module_config>
            <name>swap_object_ids</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType>
                    <xs:sequence>
                        <xs:element name='object_id1' type='xs:string' datatype='object_id' mode='in' description='first object id'/>
                        <xs:element name='object_id2' type='xs:string' datatype='object_id' mode='in' description='second object id'/>
                    </xs:sequence>
                </xs:complexType>
            </xsd>
            <doc>
                <category>Object Access</category>
                <description>Swaps the object ids. User must be careful about who already has references to these objects.</description>
            </doc>
        </module_config>
    ")]
    class MSwapObjectIds: BaseModuleSetup,IModuleRun
    {
        public string objectId1Syntax;
        public string objectId2Syntax;
        public IReadString objectId1Parsed;
        public IReadString objectId2Parsed;
       
        public override void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            // xml extraction
            MSwapObjectIds m = new MSwapObjectIds();
            this.SetupReadString(me,mc,"object_id1",out m.objectId1Syntax,out m.objectId1Parsed);
            this.SetupReadString(me,mc,"object_id2",out m.objectId2Syntax,out m.objectId2Parsed);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string objectId1Oid = objectId1Parsed.Read(mc);
            string objectId2Oid = objectId2Parsed.Read(mc);
            IObjectData obj1 = mc.objectCache.RemoveObjectData(objectId1Oid);
            IObjectData obj2 = mc.objectCache.RemoveObjectData(objectId2Oid);
            obj1.objectId = objectId2Oid;
            obj2.objectId = objectId1Oid;
            mc.objectCache.InsertObjectData(obj1);
            mc.objectCache.InsertObjectData(obj2);
        }
    }
}
