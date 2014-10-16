using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    /*
   

     */

    class MMtMsixdefSelect : IModuleSetup, IModuleRun
    {
        private string inputFileSyntax;
        private IReadString inputFileParsed;
        private List<IReadString> fieldNamesParsed = new List<IReadString>();
        private CursorSetupCommon cursorSetup;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MMtMsixdefSelect m = new MMtMsixdefSelect();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            m.inputFileSyntax = me.SelectNodeInnerText("./file");
            m.inputFileParsed = mc.ParseSyntax(m.inputFileSyntax);
            run.Add(m);
            m.cursorSetup.AddCursorSubProcs(me, mc, run);
        }

        public void Run(ModuleContext mc)
        {
            string fileName = this.inputFileParsed.Read(mc);
            var serviceDef = ServiceDef.GetServiceDef(fileName);
            List<string> orderedFieldNames = new List<string>() { 
                "paramtable_name",
                "column_name",
                "display_name",
                "is_condition",
                "column_operator", 
                "is_operator",
                "has_operator_name",
                "is_action",
                "column_type",
                "enum_namespace",
                "enum_type",
                "column_length", 
                "is_required",
                "default_value"
            };
            List<string[]> rows = new List<string[]>();
            foreach (FieldDef fieldDef in serviceDef.fieldDefs)
            {
                string[] row = new string[]{
                serviceDef.name.NullSafe(),
                fieldDef.name.NullSafe(),
                fieldDef.displayName.NullSafe(),
                fieldDef.isCondition.NullSafe(),
                fieldDef.columnOperator.NullSafe(), 
                fieldDef.isOperator.NullSafe(),
                fieldDef.hasOperatorName.NullSafe(),
                fieldDef.isAction.NullSafe(),
                fieldDef.type.NullSafe(),
                fieldDef.enumSpace.NullSafe(),
                fieldDef.enumType.NullSafe(),
                fieldDef.length.NullSafe(),
                fieldDef.required.NullSafe(),
                fieldDef.defaultValue.NullSafe(),
                };
                rows.Add(row);
            }
            var csr = new ListOfStringArrayFieldsCursor(mc, this.cursorSetup, orderedFieldNames, rows);
        }
    }
    }
