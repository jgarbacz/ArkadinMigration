using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.XlsIO;

namespace MVM
{
    public class MXlOpenWorkbook:IModuleSetup,IModuleRun
    {

        #region IModuleSetup Members

        public string workbookSyntax;
        public string sheetSyntax;
        public string cursorSyntax;

        public IReadString workbookParsed;
        public IReadString sheetParsed;
        public IReadString cursorParsed;

        public void Setup(System.Xml.XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MXlOpenWorkbook m = new MXlOpenWorkbook();
            m.workbookSyntax = me.InnerText;
            m.workbookParsed = mc.ParseSyntax(m.workbookSyntax);
            run.Add(m);
        }

        #endregion

        #region IModuleRun Members

        public void Run(ModuleContext mc)
        {
            string filename = this.workbookParsed.Read(mc);
            MvmXlWorkbook mvmWorkbook = MvmXlWorkbook.OpenWorkbook(mc,filename);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogConfig("xl_open_workbook");
        }

        #endregion
    }
}
