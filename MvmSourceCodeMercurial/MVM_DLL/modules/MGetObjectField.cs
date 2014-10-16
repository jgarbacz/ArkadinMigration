using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    class MGetObjectField : IModuleSetup, IModuleRun
    {
        private string fieldSyntax;

        private string objectIdSyntax;
        private IReadString objectIdParsed;

        private string fieldNameSyntax;
        private IReadString fieldNameParsed;

        private string outputSyntax;
        private IWriteString outputParsed;

        private string snapshotIdSyntax;
        private IReadString snapshotIdParsed;
        
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MGetObjectField m = new MGetObjectField();
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

            m.outputSyntax = me.SelectNodeInnerText("./output");
            m.outputParsed = mc.ParseWritableSyntax(m.outputSyntax);

            m.snapshotIdSyntax = me.SelectNodeInnerText("./snapshot_id");
            m.snapshotIdParsed = mc.ParseSyntax(m.snapshotIdSyntax);

            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {

            string objectId = this.objectIdParsed.Read(mc);
            string fieldName = this.fieldNameParsed.Read(mc);
            if (snapshotIdParsed != null)
            {
                string snapshotId = this.snapshotIdParsed.Read(mc);
                if (snapshotId.NotNullOrEmpty())
                {
                    // get the field for the passed snapshot
                    // exit
                    throw new Exception("Error, get_object_field for snapshot_id is not supported");
                }
            }
            try
            {
                // otherwise, no valid snapshot id passed just read the current value.
                using (IObjectData obj = mc.objectCache.CheckOut(objectId))
                {
                    string val = obj[fieldName];
                    outputParsed.Write(mc, val);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error, cannot read object field ["+fieldName+"] from object_id=["+objectId+"]",e);
            }
        }
        }
    }
