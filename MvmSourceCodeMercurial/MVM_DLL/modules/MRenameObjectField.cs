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
    * <rename_object_field>
    * <object_id>OBJECT.object_id</object_id>
    * <source_field_name>TEMP.fieldname</source_field_name>
    * <target_field_name>TEMP.othername</target_field_name>
    * </rename_object_field>
    */
    class MRenameObjectField : IModuleSetup, IModuleRun
    {
        // from xml
        private string sourceObjectIdSyntax;
        private string sourceFieldNameSyntax;
        private string targetFieldNameSyntax;

        // from setup
        private IReadString sourceObjectIdParsed;
        private IReadString sourceFieldNameParsed;
        private IReadString targetFieldNameParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MRenameObjectField m = new MRenameObjectField();
            // xml extraction
            m.sourceObjectIdSyntax = me.SelectNodeInnerText("./object_id","OBJECT.object_id");
            m.sourceFieldNameSyntax = me.SelectNodeInnerText("./source_field_name");
            m.targetFieldNameSyntax = me.SelectNodeInnerText("./target_field_name");
            // parsing
            m.sourceObjectIdParsed = mc.ParseSyntax(m.sourceObjectIdSyntax);
            m.sourceFieldNameParsed = mc.ParseSyntax(m.sourceFieldNameSyntax);
            m.targetFieldNameParsed = mc.ParseSyntax(m.targetFieldNameSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string sourceObjectId = this.sourceObjectIdParsed.Read(mc);
            string sourceFieldName = this.sourceFieldNameParsed.Read(mc);
            string targetFieldName = this.targetFieldNameParsed.Read(mc);
            if(sourceFieldName.Equals(targetFieldName)) return;
            using (IObjectData source = mc.objectCache.CheckOut(sourceObjectId))
            {
                source[targetFieldName] = source[sourceFieldName];
                source.RemoveObjectField(sourceFieldName);
            }
        }
        }
    }
