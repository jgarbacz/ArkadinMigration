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
    * <copy_object_field>
    * <source_object_id>OBJECT.object_id</source_object_id>
    * <source_field_name>TEMP.fieldname</source_field_name>
    * <target_object_id>TEMP.object_id</target_object_id>
    * <target_field_name>TEMP.othername</target_field_name>
    * </copy_object_field>
    */

    class MCopyObjectField : IModuleSetup, IModuleRun
    {
        // from xml
        private string sourceObjectIdSyntax;
        private string sourceFieldNameSyntax;
        private string targetObjectIdSyntax;
        private string targetFieldNameSyntax;

        // from setup
        private IReadString sourceObjectIdParsed;
        private IReadString sourceFieldNameParsed;
        private IReadString targetObjectIdParsed;
        private IReadString targetFieldNameParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MCopyObjectField m = new MCopyObjectField();
            // xml extraction
            m.sourceObjectIdSyntax = me.SelectNodeInnerText("./source_object_id","OBJECT.object_id");
            m.sourceFieldNameSyntax = me.SelectNodeInnerText("./source_field_name");
            m.targetObjectIdSyntax = me.SelectNodeInnerText("./target_object_id", "OBJECT.object_id");
            m.targetFieldNameSyntax = me.SelectNodeInnerText("./target_field_name");
            // parsing
            m.sourceObjectIdParsed = mc.ParseSyntax(m.sourceObjectIdSyntax);
            m.sourceFieldNameParsed = mc.ParseSyntax(m.sourceFieldNameSyntax);
            m.targetObjectIdParsed = mc.ParseSyntax(m.targetObjectIdSyntax);
            m.targetFieldNameParsed = mc.ParseSyntax(m.targetFieldNameSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string sourceObjectId = this.sourceObjectIdParsed.Read(mc);
            string sourceFieldName = this.sourceFieldNameParsed.Read(mc);
            string targetObjectId = this.targetObjectIdParsed.Read(mc);
            string targetFieldName = this.targetFieldNameParsed.Read(mc);
            using (IObjectData target = mc.objectCache.CheckOut(targetObjectId))
            {
                using (IObjectData source = mc.objectCache.CheckOut(sourceObjectId))
                {
                    target[targetFieldName] = source[sourceFieldName];
                }
            }
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("copy_object_field:");
        }
    }
}
