using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
/*
<objects_equal>
<object_id>TEMP.obj1</object_id>
<object_id>TEMP.obj2</object_id>
<view_object_id>TEMP.view_oid</view_object_id>
<field name='fieldname'>
<exclude_view_object_id>TEMP.excluded_oid</exclude_view_object_id>
<exclude_field name='fieldname'/>
<result>TEMP.bool</result>
</objects_equal>      
*/

    class MObjectsEqual : IModuleSetup, IModuleRun
    {
        // from xml
        private string resultSyntax;

        // from setup
        private IWriteString resultParsed;

        private List<string> objectIdsSyntax;
        private List<IReadString> objectIdsParsed;

        private Dictionary<string, string> alwaysExclude = null;
        private List<string> excludeViewOidsSyntax;
        private List<IReadString> excludeViewOidsParsed;

        private Dictionary<string, string> alwaysInclude = null;
        private List<string> includeViewOidsSyntax;
        private List<IReadString> includeViewOidsParsed;


        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MObjectsEqual m = new MObjectsEqual();
            m.resultSyntax = me.SelectNodeInnerText("./result");
            m.resultParsed = mc.ParseWritableSyntax(m.resultSyntax);

            m.objectIdsSyntax = me.SelectNodesInnerText("./object_id");
            m.objectIdsParsed = mc.ParseSyntax(m.objectIdsSyntax);

            m.excludeViewOidsSyntax = me.SelectNodesInnerText("./exclude_view_object_id");
            m.excludeViewOidsParsed = mc.ParseSyntax(m.excludeViewOidsSyntax);

            m.includeViewOidsSyntax = me.SelectNodesInnerText("./view_object_id");
            m.includeViewOidsParsed = mc.ParseSyntax(m.includeViewOidsSyntax);

            m.alwaysExclude = new Dictionary<string, string>(ModuleContext.internalObjectFields);
            foreach (XmlElement exc in me.SelectNodes("exclude_field"))
            {
                string fieldName = exc.GetAttribute("name");
                m.alwaysExclude[fieldName]=null;
            }

            foreach (XmlElement inc in me.SelectNodes("field"))
            {
                string fieldName = inc.GetAttribute("name");
                if(m.alwaysInclude==null) m.alwaysInclude=new Dictionary<string,string>();
                m.alwaysInclude[fieldName]=null;
            }

            // runtime
            run.Add(m);
        }

        
        public void Run(ModuleContext mc)
        {
            // setup the exclusion dictionary
            Dictionary<string, string> exclude=null;
            if (excludeViewOidsParsed.Count == 0)
            {
                exclude = alwaysExclude;
            }
            else
            {
                exclude = new Dictionary<string, string>(alwaysExclude);
                foreach (var excludedViewOid in this.excludeViewOidsParsed)
                {
                    string oid = excludedViewOid.Read(mc);
                    using (IObjectData excludedViewObj = mc.objectCache.CheckOut(oid))
                    {
                        foreach (string fieldName in excludedViewObj.FieldNames)
                        {
                            exclude[fieldName] = null;
                        }
                    }
                }
            }

            // setup the inclusion dictionary
            Dictionary<string, string> include = null;
            if (includeViewOidsParsed.Count==0)
            {
                include = this.alwaysInclude;
            }
            else
            {
                if(alwaysInclude!=null)include = new Dictionary<string, string>(alwaysInclude);
                else include = new Dictionary<string, string>();
                foreach (var includedViewOid in this.includeViewOidsParsed)
                {
                    string oid = includedViewOid.Read(mc);
                    using (IObjectData includedViewObj = mc.objectCache.CheckOut(oid))
                    {
                        foreach (string fieldName in includedViewObj.FieldNames)
                        {
                            if(exclude.ContainsKey(fieldName)) include[fieldName] = null;
                        }
                    }
                }
            }

            // compare all of the passed object_ids
            string oid1 = objectIdsParsed[0].Read(mc);
            IObjectData obj1 = mc.objectCache.CheckOut(oid1);
            var dic1 = obj1.FieldKeyValuesPairs.AsDictionary();
            for (int i = 1; i < objectIdsParsed.Count; i++)
            {
                string oid2 = objectIdsParsed[i].Read(mc);
                IObjectData obj2 = mc.objectCache.CheckOut(oid2);
                var dic2 = obj2.FieldKeyValuesPairs.AsDictionary();
                if (!dic1.SameAs(dic2, include, exclude))
                {
                    resultParsed.Write(mc, "0");
                    return;
                }
                oid1 = oid2;
                obj1 = obj2;
                dic1 = dic2;
            }
            resultParsed.Write(mc, "1");
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("objects_equal:");
        }
    }
}
