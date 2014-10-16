using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

/*
<print_table_for_ctrl>
<name>'TABLE_NAME'</name>
<ctrl_file>'/blah/my.ctrl'</ctrl_file>       
<output_dir></output_dir>
<field name="x">'overridevalue'</field>
</print_table_for_ctrl>
*/
namespace MVM
{
    class MPrintTableForCtrl : IModuleSetup
    {
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            string ctrlSyntax = me.SelectNodeInnerText("./ctrl_file");
            string outputDirSyntax = me.SelectNodeInnerText("./output_dir", "GLOBAL.output_dir");
            string outputFileSyntax = me.SelectNodeInnerText("./output_filename");
            // read the ctrl file
            string ctrlFileName = mc.ParseSyntax(ctrlSyntax).Read(mc);
            OracleDumpCtrlFile ctrl = new OracleDumpCtrlFile(ctrlFileName);
            string fieldDelimSyntax = ctrl.fieldDelim.InterpolateEscapesReverse().q();
            string recordDelimSyntax = ctrl.recordDelim.InterpolateEscapesReverse().q();
            string tableName = ctrl.tableName;
            // name the output file
            //"C:\_ROB\mvm\output."~TEMP.worker_no~".txt"
            string outputDir = mc.ParseSyntax(outputDirSyntax).Read(mc).TrimEnd('/', '\\');
            string fileSyntax = "'" + outputDir + @"'~'\" + tableName + @".'~THREAD.worker_no~'.dat.txt'";
            var fieldNames = ctrl.GetOrderedFieldNames();
            MPrintTable.generatePrintRecord(me, mc, run, fieldNames, fileSyntax, outputFileSyntax, fieldDelimSyntax, recordDelimSyntax, null);
        }

        public void Log(ILogger log)
        {
            log.LogInfo("print_table_for_ctrl:");
        }
    }
}
