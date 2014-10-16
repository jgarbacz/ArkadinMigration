using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MVM
{
    public class MWriteCursorToExcel:IModuleSetup,IModuleRun
    {

        #region IModuleSetup Members

        string fileSyntax;
        string sheetSyntax;
        string cursorSyntax;

        IReadString fileParsed;
        IReadString sheetParsed;
        IReadString cursorParsed;

        public void Setup(System.Xml.XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MWriteCursorToExcel m = new MWriteCursorToExcel();
            m.fileSyntax = me.SelectNodeInnerText("file");
            m.cursorSyntax = me.SelectNodeInnerText("cursor");
            m.sheetSyntax = me.SelectNodeInnerText("sheet");
            m.fileParsed = mc.ParseSyntax(fileSyntax);
            m.cursorParsed = mc.ParseSyntax(cursorSyntax);
            m.sheetParsed = mc.ParseSyntax(sheetSyntax);
            run.Add(m);
        }

        #endregion

        #region IModuleRun Members

        public void Run(ModuleContext mc)
        {

            // open/create workbook
            // delete sheet if it exists
            // create sheet
            // get the cursor
            // write the header fields to the sheet
            // loop through the cursor and write the rows to the sheet 


        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogConfig("write_cursor_to_excel");
        }

        #endregion
    }
}
