using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml;
using System.IO;
using System.Linq;


using Antlr.Runtime.Tree;

namespace MVM
{
    /*
        <rename_object_fields>
        <object_id>'t_pt_blah'</object_id>
        <pattern>'(pt_)(*.)$</pattern>
        <replacement>$2<replacement>
        <rename_object_fields>
      */

    class MRenameObjectFields : IModuleSetup, IModuleRun
    {
        // from xml
        private string sourceObjectIdSyntax;
        private string patternSyntax;
        private string replacementSyntax;
       
        // from setup
        private IReadString sourceObjectIdParsed;
        private IReadString patternParsed;
        private IReadString replacementParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MRenameObjectFields m = new MRenameObjectFields();
            // xml extraction
            m.sourceObjectIdSyntax = me.SelectNodeInnerText("./object_id", "OBJECT.object_id");
            m.patternSyntax = me.SelectNodeInnerText("./pattern");
            m.replacementSyntax = me.SelectNodeInnerText("./replacement");
            // parsing
            m.sourceObjectIdParsed = mc.ParseSyntax(m.sourceObjectIdSyntax);
            m.patternParsed = mc.ParseSyntax(m.patternSyntax);
            m.replacementParsed = mc.ParseSyntax(m.replacementSyntax);
           // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string sourceObjectId = this.sourceObjectIdParsed.Read(mc);
            string pattern = this.patternParsed.Read(mc);
            string replacement = this.replacementParsed.Read(mc);
            Regex regex = new Regex(pattern);
            using (IObjectData obj = mc.objectCache.CheckOut(sourceObjectId))
            {
                foreach (string field in obj.FieldNames)
                {
                    if (field.Equals("object_id") || field.Equals("object_type") || field.Equals("eof")) continue;
                    string newField = regex.Replace(field, replacement);
                    if (field.Equals(newField)) continue;
                    obj[newField] = obj[field];
                    obj.RemoveObjectField(field);
                }
            }
        }

        }
    }
