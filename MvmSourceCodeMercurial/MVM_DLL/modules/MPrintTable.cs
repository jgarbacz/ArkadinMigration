using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

/*
<print_table>
<name>T_ACC_USAGE</name>       <-- format of the tableName must not vary
<output_dir></output_dir>
<login_object></login_object>
<type></type>
<server></server>
<db></db>
<user></user>
<pw></pw>
</print_table>
*/
namespace MVM
{
    // sql server date format is: 2009-10-31 00:00:00.000
    // for oracl i hard code YYYYMMDDHH24MISS
    class MPrintTable : IModuleSetup
    {
        public static void generatePrintRecord(XmlElement me, ModuleContext mc, List<IModuleRun> run, List<string> fieldNames, string file, string outfile, string fdel, string rdel, string dbtype)
        {
            Overrider overrider = new Overrider(me);
            StringBuilder m = new StringBuilder();
            m.AppendLine("<print_record>");
            m.AppendLine("<file>" + file + "</file>");
            if (!outfile.IsNullOrEmpty())
            {
                m.AppendLine("<output_filename>" + outfile + "</output_filename>");
            }
            m.AppendLine("<field_delim>" + fdel + "</field_delim>");
            m.AppendLine("<record_delim>" + rdel + "</record_delim>");
            if (dbtype != null && dbtype.Equals("sql"))
            {
                // this is because bcp does not support UTF8
                m.AppendLine("<encoding>'Windows-1252'</encoding>");
            }
            m.AppendLine("<data>");
            foreach (string f in fieldNames)
            {
                string printVal = overrider.GetSyntax(f);
                m.AppendLine("<field name=" + f.q() + ">" + printVal + "</field>");
            }
            m.AppendLine("</data>");
            m.AppendLine("</print_record>");

            // add the print_record runtime newModule to the machine
            SchedulerMaster sm = mc.scheduler.schedulerMaster;
            run.Add(sm.GetModuleRun(m.ToString()));
        }

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            DbInfo dbInfo = new DbInfo(me, mc);
            string db = dbInfo.GetDb(mc);
            string tableNameSyntax = me.SelectNodeInnerText("./name");
            string tableName = mc.SyntaxReadString(tableNameSyntax);
            string fieldDelimSyntax = me.SelectNodeInnerText("./field_delim", "GLOBAL.field_delim");
            string recordDelimSyntax = me.SelectNodeInnerText("./record_delim", "GLOBAL.record_delim");
            string outputDirSyntax = me.SelectNodeInnerText("./output_dir", "GLOBAL.output_dir");
            string outputFileSyntax = me.SelectNodeInnerText("./output_filename");
            string charsetSyntax = me.SelectNodeInnerText("./charset");

            //"C:\\_ROB\\mvm\\output."~GLOBAL.node_id~".txt"
            string outputDir = mc.ParseSyntax(outputDirSyntax).Read(mc).TrimEnd('/', '\\');
            string fSyntax = "'" + tableName + "." + db + @".'~GLOBAL.node_id~'.dat.txt'";
            string fileSyntax = outputDirSyntax + @"~'\\'~" + fSyntax;

            string ctrlSyntax = "'" + outputDir + @"'~'\" + tableName + "." + db + @".ctl.txt'";

            // Delim info
            string fdel = mc.ParseSyntax(fieldDelimSyntax).Read(mc);
            string rdel = mc.ParseSyntax(recordDelimSyntax).Read(mc);

            string charset = null;
            if (charsetSyntax.NotNullOrEmpty())
            {
                charset = mc.ParseSyntax(charsetSyntax).Read(mc);
            }

            // See if we want to skip any fields
            Dictionary<string, bool> skippedFields = new Dictionary<string, bool>();
            foreach (XmlElement elem in me.SelectElements("./skip"))
            {
                skippedFields[elem.GetAttribute("name")] = true;
            }

            // Get info for the tableName
            TableInfo tableInfo = mc.globalContext.schemaMaster.GetTableInfo(mc, dbInfo, tableName, skippedFields);
            List<string> fieldNames = tableInfo.columnNames;

            Dictionary<string, string> dateFields = new Dictionary<string, string>();
            foreach (var ci in tableInfo.columnInfo)
            {
                if (ci.type.ToLower().Contains("date"))
                {
                    dateFields.Add(ci.name, mc.mvm.globalContext["global_date_format"]);
                }
            }

            // Write out the control file
            string ctrlFileName = mc.ParseSyntax(ctrlSyntax).Read(mc);
            try
            {
                tableInfo.WriteLoaderCtrl(ctrlFileName, fdel, rdel, charset, dateFields);
            }
            catch
            {
                // Can get exceptions here from multiple slaves trying to write the ctrl file at the same time; ignore them
            }

            MPrintTable.generatePrintRecord(me, mc, run, fieldNames, fileSyntax, outputFileSyntax, fieldDelimSyntax, recordDelimSyntax, dbInfo.GetType(mc));
        }

        public void Log(ILogger log)
        {
            log.LogInfo("print_table:");
        }
    }
}
