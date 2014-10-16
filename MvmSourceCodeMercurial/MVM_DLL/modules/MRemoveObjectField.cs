using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Antlr.Runtime.Tree;

namespace MVM
{
   
    class MRemoveObjectField : IModuleSetup, IModuleRun
    {
        // from xml
        private string objectIdSyntax;
        private string fieldNameSyntax;
        private string outputSyntax;

        // from setup
        private IReadString objectIdParsed;
        private IReadString fieldNameParsed;
        private IWriteString outputParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MRemoveObjectField m = new MRemoveObjectField();
            // xml extraction
            m.objectIdSyntax = me.SelectNodeInnerText("./object_id","OBJECT.object_id");
            m.fieldNameSyntax = me.SelectNodeInnerText("./field_name");
            m.outputSyntax = me.SelectNodeInnerText("./output");
            // parsing
            m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            m.fieldNameParsed = mc.ParseSyntax(m.fieldNameSyntax);
            m.outputParsed = mc.ParseWritableSyntax(m.outputSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string objectId = this.objectIdParsed.Read(mc);
            string fieldName = this.fieldNameParsed.Read(mc);
            using (IObjectData obj = mc.objectCache.CheckOut(objectId))
            {
                bool result=obj.RemoveObjectField(fieldName);
                if (outputParsed != null) outputParsed.Write(mc, result ? "1" : "0");
            }
        }

        }
    }
