using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.XlsIO;

namespace MVM
{
    public class MXlAppendField:IModuleSetup,IModuleRun
    {

        #region IModuleSetup Members

        public string workbookSyntax;
        public string worksheetSyntax;
        public string fieldSyntax;

        public IReadString workbookParsed;
        public IReadString worksheetParsed;
        public IReadString fieldParsed;

        public void Setup(System.Xml.XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MXlAppendField m = new MXlAppendField();
            m.workbookSyntax = me.SelectNodeInnerText("workbook");
            m.workbookParsed = mc.ParseSyntax(m.workbookSyntax);

            m.worksheetSyntax = me.SelectNodeInnerText("worksheet");
            m.worksheetParsed = mc.ParseSyntax(m.worksheetSyntax);

            m.fieldSyntax = me.SelectNodeInnerText("field");
            m.fieldParsed = mc.ParseSyntax(m.fieldSyntax);
            run.Add(m);
        }

        #endregion

        #region IModuleRun Members

        public void Run(ModuleContext mc)
        {
            string workbookFile = this.workbookParsed.Read(mc);
            string worksheet=this.worksheetParsed.Read(mc);
            string field=this.fieldParsed.Read(mc);

            MvmXlWorkbook mvmWorkbook = MvmXlWorkbook.GlobalGet(mc,workbookFile);
            MvmXlWorksheet mvmWorksheet=mvmWorkbook.GetWorksheet(worksheet);
            mvmWorksheet.AppendField(field);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogConfig("xl_append_field");
        }

        #endregion
    }
}
