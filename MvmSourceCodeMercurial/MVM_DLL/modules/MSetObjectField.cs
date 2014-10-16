using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
   
   
    class MSetObjectField : IModuleSetup, IModuleRun
    {
        private string objectIdSyntax;
        private IReadString objectIdParsed;
        private string fieldNameSyntax;
        private IReadString fieldNameParsed;
        private string valueSyntax;
        private IReadString valueParsed;
        private string fieldSyntax;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MSetObjectField m = new MSetObjectField();
            m.fieldNameSyntax = me.SelectNodeInnerText("./field_name");
            m.objectIdSyntax = me.SelectNodeInnerText("./object_id","OBJECT.object_id");
            m.fieldSyntax = me.SelectNodeInnerText("./field");
            if (m.fieldSyntax != null)
            {
                if (!mc.GetObjectFieldParts(m.fieldSyntax, out m.fieldNameSyntax, out m.objectIdSyntax))
                    throw new Exception("Error passed field=[" + m.fieldSyntax + "] is not valid object field syntax");
            }
            m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            m.fieldNameParsed = mc.ParseSyntax(m.fieldNameSyntax);
            m.valueSyntax = me.SelectNodeInnerText("./input");
            m.valueParsed = mc.ParseSyntax(m.valueSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string objectId = this.objectIdParsed.Read(mc);
            string fieldName = this.fieldNameParsed.Read(mc);
            string fieldValue = this.valueParsed.Read(mc);
            using (IObjectData obj = mc.objectCache.CheckOut(objectId))
            {
                obj[fieldName] = fieldValue;
            }
        }
        }
    }
