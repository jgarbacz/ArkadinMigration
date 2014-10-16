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

    class MCursorFieldNameSelect : IModuleSetup, IModuleRun
    {
        //private string inputCursorSyntax;
        //private IReadString inputCursorParsed;
        //private CursorSetupCommon cursorSetup;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            throw new Exception("cursor field name select no longer supported, use object_field_select instead");
            //MCursorFieldNameSelect m = new MCursorFieldNameSelect();
            //m.cursorSetup = new CursorSetupCommon(me, mc);
            //m.inputCursorSyntax = me.SelectNodeInnerText("./input_cursor");
            //m.inputCursorParsed = mc.ParseSyntax(m.inputCursorSyntax);
            //run.Add(m);
            //m.cursorSetup.AddCursorSubProcs(me, mc, run);
        }

        public void Run(ModuleContext mc)
        {
            //// get the name of the cursor value
            //string cursorValue = this.cursorSetup.GetCursorValue(mc);

            //// get the input cursor
            //string inputCursorOid = this.inputCursorParsed.Read(mc);
            //ICursor inputCursor = mc.LookupCursorViaOid(inputCursorOid);
            //List<string> stringList = inputCursor.GetOrderedFieldNames();

            //// set the output cursor
            //var csr = new SingleFieldListCursor(mc, cursorSetup, stringList);
        }

    }
    }
