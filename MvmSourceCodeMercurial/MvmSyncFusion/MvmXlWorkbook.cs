using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.XlsIO;
using System.IO;
namespace MVM
{
    public class MvmXlWorkbook
    {
        public const string WorkbookPrefix = "[XlWorkbook]";

        public ExcelEngine excelEngine;
        public IApplication application;
        public IWorkbook workbook;
        public string fileName;
        public string globalName;

        public Dictionary<string, MvmXlWorksheet> sheets = new Dictionary<string, MvmXlWorksheet>();

        private MvmXlWorkbook()
        {
        }
        
        public void GlobalSet(ModuleContext mc)
        {
            mc.globalContext.SetNamedClassInst(this.globalName, this);
        }

        public void GlobalRemove(ModuleContext mc)
        {
            mc.globalContext.RmNamedClassInst(this.globalName);
        }

        public static MvmXlWorkbook GlobalGet(ModuleContext mc, string workbookFile)
        {
            string globalName = WorkbookPrefix + workbookFile;
            return (MvmXlWorkbook) mc.globalContext.GetNamedClassInst(globalName);
        }

        /// <summary>
        /// Opens new or existing workbook
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static MvmXlWorkbook OpenWorkbook(ModuleContext mc, string fileName)
        {
            MvmXlWorkbook mvmWorkbook = new MvmXlWorkbook();
            mvmWorkbook.fileName = fileName;
            mvmWorkbook.globalName = WorkbookPrefix + fileName;
            mvmWorkbook.excelEngine = new ExcelEngine();
            mvmWorkbook.application = mvmWorkbook.excelEngine.Excel;
            FileInfo fileInfo = new FileInfo(mvmWorkbook.fileName);
            if (fileInfo.Exists)
            {
                mvmWorkbook.workbook = mvmWorkbook.application.Workbooks.Open(mvmWorkbook.fileName);
            }else{
                mvmWorkbook.workbook = mvmWorkbook.application.Workbooks.Create(1);
            }
            mvmWorkbook.GlobalSet(mc);
            return mvmWorkbook;
        }

        /// <summary>
        /// Closes and saves a previously opened workbook
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="fileName"></param>
        public static void CloseWorkbook(ModuleContext mc, string fileName)
        {
            MvmXlWorkbook mvmWorkbook = MvmXlWorkbook.GlobalGet(mc,fileName);
            mvmWorkbook.workbook.SaveAs(mvmWorkbook.fileName);
            mvmWorkbook.workbook.Close();
            mvmWorkbook.excelEngine.Dispose();
            mvmWorkbook.GlobalRemove(mc);
        }

        /// <summary>
        /// Deletes the worksheet returns true if the sheet was their, else false;
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public bool DeleteWorksheet(string sheetName)
        {
            if (this.sheets.ContainsKey(sheetName)) {
                this.sheets.Remove(sheetName);
            }
            IWorksheet worksheet = this.workbook.Worksheets[sheetName];
            if (worksheet == null) return false;
            // cannot remove the last worksheet so if this is the last one add Sheet1 then delete.
            // if trying to delete Sheet1 and only one sheet, then just clear it.
            if (workbook.Worksheets.Count == 1)
            {
                if (worksheet.Name.Equals("Sheet1"))
                {
                    worksheet.Clear();
                    return true;
                }
                workbook.Worksheets.Create("Sheet1");
            }
            this.workbook.Worksheets.Remove(worksheet);
            return true;
        }

        public MvmXlWorksheet GetWorksheet(string sheetName)
        {
            if (this.sheets.ContainsKey(sheetName)) return this.sheets[sheetName];
            IWorksheet worksheet = this.workbook.Worksheets[sheetName];
            if (worksheet == null)
            {
                // if only 1 worksheet and it is named "Sheet1" and it is empty take it over
                if (this.workbook.Worksheets.Count == 1)
                {
                    IWorksheet sheet1 = this.workbook.Worksheets["Sheet1"];
                    if (sheet1!=null&&sheet1.Cells.Length == 0)
                    {
                        sheet1.Name = sheetName;
                        worksheet = sheet1;
                    }
                }
                // if still no sheet create a new one
                if(worksheet==null) worksheet=workbook.Worksheets.Create(sheetName);
            }
            MvmXlWorksheet mvmWorksheet = new MvmXlWorksheet(worksheet);
            this.sheets[sheetName] = mvmWorksheet;
            return mvmWorksheet;
        }
    }
}
