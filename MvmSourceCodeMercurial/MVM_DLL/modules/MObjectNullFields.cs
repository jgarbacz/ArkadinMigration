using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

/*
  
<object_null_fields>
<object_id>TEMP.csr</object_id> // default to OBJECT.object_id
<exclude'do_no_clear_me1'</exclude>
<exclude'do_no_clear_me2'</exclude>
</object_null_fields>
 
 */

namespace MVM
{
    class MObjectNullFields: IModuleSetup,IModuleRun
    {
        public string objectIdSyntax;
        public IReadString objectIdParsed;
        public List<string> excludeListSyntax;
        public List<IReadString> excludeListParsed;

        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MObjectNullFields m = new MObjectNullFields();
            m.objectIdSyntax = me.SelectNodeInnerText("./object_id","OBJECT.object_id");
            m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            m.excludeListSyntax = me.SelectNodesInnerText("./exclude");
            m.excludeListParsed = mc.ParseSyntax(m.excludeListSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string objectId = objectIdParsed.Read(mc);
            Dictionary<string,bool> exclude=new Dictionary<string,bool>();
            foreach(var exclusion in this.excludeListParsed){
                exclude[exclusion.Read(mc)] = true;
            }
            using (var obj = mc.objectCache.CheckOut(objectId))
            {
                obj.NullFields(exclude);
            }
        }
    }
}
