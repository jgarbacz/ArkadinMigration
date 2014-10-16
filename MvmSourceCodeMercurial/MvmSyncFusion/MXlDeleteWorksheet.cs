using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.XlsIO;

namespace MVM
{
    public class MXlDeleteWorksheet:IModuleSetup,IModuleRun
    {

        #region IModuleSetup Members

        public string workbookSyntax;
        public string worksheetSyntax;

        public IReadString workbookParsed;
        public IReadString worksheetParsed;

        public void Setup(System.Xml.XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MXlDeleteWorksheet m = new MXlDeleteWorksheet();
            m.workbookSyntax = me.SelectNodeInnerText("workbook");
            m.workbookParsed = mc.ParseSyntax(m.workbookSyntax);

            m.worksheetSyntax = me.SelectNodeInnerText("worksheet");
            m.worksheetParsed = mc.ParseSyntax(m.worksheetSyntax);

            run.Add(m);
        }

        #endregion

        #region IModuleRun Members

        public void Run(ModuleContext mc)
        {
            string workbookFile = this.workbookParsed.Read(mc);
            string worksheet=this.worksheetParsed.Read(mc);
            MvmXlWorkbook mvmWorkbook = MvmXlWorkbook.GlobalGet(mc,workbookFile);
            bool success=mvmWorkbook.DeleteWorksheet(worksheet);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogConfig("xl_delete_worksheet");
        }

        #endregion
    }
}
