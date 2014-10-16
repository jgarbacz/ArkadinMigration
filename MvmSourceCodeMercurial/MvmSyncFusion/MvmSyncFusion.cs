using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Syncfusion.XlsIO;

namespace MVM
{
    public class MvmSyncFusion
    {
        


        public void x()
        {
            //Step 1 : Instantiate the spreadsheet creation engine.
            ExcelEngine excelEngine = new ExcelEngine();
            //Step 2 : Instantiate the excel application object.
            IApplication application = excelEngine.Excel;
            // create a workbook
            IWorkbook workbook = application.Workbooks.Create(new string[]{"first"});
            IWorksheet sheet = workbook.Worksheets[0];
            sheet.Range["A1:A5"].Text = "HelloWorld";
            workbook.SaveAs(@"c:\RobSample.xls");
            workbook.Close();
            excelEngine.Dispose();



            //
            //if (rdButtonXltm.Checked)
            //    workbook = application.Workbooks.Open(ResolveApplicationDataPath(@"MacroTemplate.xltm"), ExcelOpenType.Automatic);
            //else
            //    workbook = application.Workbooks.Create(3);
            //IWorksheet sheet = workbook.Worksheets[0];

            //if (rdButtonXltm.Checked)
            //{
            //    sheet.IsGridLinesVisible = false;
            //    sheet[1, 1].Text = "Essential XlsIO supports opening of XLTM (Excel 2007 Macro Enabled Template) file and save it in either XLTM or XLSM formats.";
            //    sheet[3, 1].Text = "You can run the macro by triggering the click event of the button placed in this worksheet.";
            //}
            //else
            //{
            //    sheet.Range["A1:A5"].Text = "HelloWorld";
            //    sheet.UsedRange.AutofitColumns();
            //}

            //if (this.CheckBox1.Checked)
            //{
            //    if (rdButtonXls.Checked)
            //    {
            //        //Save as .xls format
            //        workbook.SaveAs("Sample.xls", ExcelSaveType.SaveAsXLS, Response, ExcelDownloadType.Open);
            //    }
            //    else if (rdButtonXlsx.Checked)
            //    {
            //        //Save as .xlsx format
            //        workbook.Version = ExcelVersion.Excel2007;
            //        workbook.SaveAs("Sample.xlsx", ExcelSaveType.SaveAsXLS, Response, ExcelDownloadType.Open);
            //    }
            //    else if (rdButtonCsv.Checked)
            //    {
            //        //Save in .xlsx format
            //        workbook.SaveAs("Sample.csv", ",", Response, ExcelDownloadType.Open, ExcelHttpContentType.CSV);
            //    }
            //    else if (rdButtonXltx.Checked)
            //    {
            //        workbook.Version = ExcelVersion.Excel2007;
            //        workbook.SaveAs("Sample.xltx", ExcelSaveType.SaveAsTemplate, Response, ExcelDownloadType.Open, ExcelHttpContentType.Excel2007);
            //    }
            //    else if (rdButtonXltm.Checked)
            //    {
            //        workbook.Version = ExcelVersion.Excel2007;
            //        workbook.SaveAs("Sample.xltm", ExcelSaveType.SaveAsTemplate, Response, ExcelDownloadType.Open, ExcelHttpContentType.Excel2007);
            //    }
            //    else if (RadioButton1.Checked)
            //    {
            //        workbook.Version = ExcelVersion.Excel2010;
            //        workbook.SaveAs("Sample.xlsx", Response, ExcelDownloadType.Open, ExcelHttpContentType.Excel2010);
            //    }
            //}
            //else
            //{
            //    if (rdButtonXls.Checked)
            //    {
            //        //Save as .xls format
            //        workbook.SaveAs("Sample.xls", ExcelSaveType.SaveAsXLS, Response, ExcelDownloadType.PromptDialog);
            //    }
            //    //Save as .xlsx format
            //    else if (rdButtonXlsx.Checked)
            //    {
            //        workbook.Version = ExcelVersion.Excel2007;
            //        workbook.SaveAs("Sample.xlsx", ExcelSaveType.SaveAsXLS, Response, ExcelDownloadType.PromptDialog);
            //    }
            //    else if (rdButtonCsv.Checked)
            //    {
            //        //Save in .xlsx format
            //        workbook.SaveAs("Sample.csv", ",", Response, ExcelDownloadType.PromptDialog, ExcelHttpContentType.CSV);
            //    }
            //    else if (rdButtonXltx.Checked)
            //    {
            //        workbook.Version = ExcelVersion.Excel2007;
            //        workbook.SaveAs("Sample.xltx", ExcelSaveType.SaveAsTemplate, Response, ExcelDownloadType.PromptDialog, ExcelHttpContentType.Excel2007);
            //    }
            //    else if (rdButtonXltm.Checked)
            //    {
            //        workbook.Version = ExcelVersion.Excel2007;
            //        workbook.SaveAs("Sample.xltm", ExcelSaveType.SaveAsTemplate, Response, ExcelDownloadType.PromptDialog, ExcelHttpContentType.Excel2007);
            //    }
            //    else if (RadioButton1.Checked)
            //    {
            //        workbook.Version = ExcelVersion.Excel2010;
            //        workbook.SaveAs("Sample.xlsx", Response, ExcelDownloadType.PromptDialog, ExcelHttpContentType.Excel2010);
            //    }
            //}
            //

        }
    }
}
