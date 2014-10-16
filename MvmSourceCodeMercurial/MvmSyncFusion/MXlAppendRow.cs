using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.XlsIO;

namespace MVM
{
    public class MXlAppendRow:IModuleSetup,IModuleRun
    {

        #region IModuleSetup Members

        public string workbookSyntax;
        public string worksheetSyntax;
        public string cursorSyntax;

        public IReadString workbookParsed;
        public IReadString worksheetParsed;
        public IReadString cursorParsed;

        public void Setup(System.Xml.XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MXlAppendRow m = new MXlAppendRow();
            m.workbookSyntax = me.SelectNodeInnerText("workbook");
            m.workbookParsed = mc.ParseSyntax(m.workbookSyntax);

            m.worksheetSyntax = me.SelectNodeInnerText("worksheet");
            m.worksheetParsed = mc.ParseSyntax(m.worksheetSyntax);

            m.cursorSyntax = me.SelectNodeInnerText("cursor");
            m.cursorParsed = mc.ParseSyntax(m.cursorSyntax);
            run.Add(m);
        }

        #endregion

        #region IModuleRun Members

        public void Run(ModuleContext mc)
        {
            string workbookFile = this.workbookParsed.Read(mc);
            string worksheet=this.worksheetParsed.Read(mc);
            string cursorOid=this.cursorParsed.Read(mc);

            MvmXlWorkbook mvmWorkbook = MvmXlWorkbook.GlobalGet(mc,workbookFile);
            MvmXlWorksheet mvmWorksheet=mvmWorkbook.GetWorksheet(worksheet);

            // write all the fields on the cursor onto the sheet row
            ICursor cursor = (ICursor)mc.globalContext.GetNamedClassInst(cursorOid);
            using (ObjectData currCsrObj = mc.objectCache.CheckOut(cursorOid))
            {
                foreach (string f in cursor.GetOrderedFieldNames())
                {
                    string value = currCsrObj[f];
                    mvmWorksheet.AppendField(value);
                }
            }

            // move onto the next row
            mvmWorksheet.NextRow();
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogConfig("xl_append_row");
        }

        #endregion
    }
}
