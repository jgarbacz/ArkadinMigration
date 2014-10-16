using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.XlsIO;

namespace MVM
{
    public class MXlCloseWorkbook:IModuleSetup,IModuleRun
    {

        #region IModuleSetup Members

        public string workbookSyntax;
        public IReadString workbookParsed;
     

        public void Setup(System.Xml.XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MXlCloseWorkbook m = new MXlCloseWorkbook();
            m.workbookSyntax = me.InnerText;
            m.workbookParsed = mc.ParseSyntax(m.workbookSyntax);
            run.Add(m);
        }

        #endregion

        #region IModuleRun Members

        public void Run(ModuleContext mc)
        {
            string filename = this.workbookParsed.Read(mc);
            MvmXlWorkbook.CloseWorkbook(mc, filename);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogConfig("xl_close_workbook");
        }

        #endregion
    }
}
