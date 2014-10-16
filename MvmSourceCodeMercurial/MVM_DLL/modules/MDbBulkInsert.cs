using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;
namespace MVM
{
    class MDbBulkInsert : MDbCommon, IModuleRun
    {
        private List<ColumnInfo> columnsInfo;
        private TableInfo tableInfo;
        IBulkLoader bulkLoader;
        private List<string> orderedParamNames = new List<string>();
        private Dictionary<string, IReadString> readableParamsParsed = new Dictionary<string, IReadString>();
       
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDbBulkInsert m = new MDbBulkInsert();
            m.ParseDbCommon(me, mc, me.SelectNodeInnerText("./name"));
            Overrider overrider = new Overrider(me);

            // build up a list of tableName fields which should get sql values.
            Dictionary<string, string> sqlFields = new Dictionary<string, string>();
            foreach (var elem in me.SelectElements("./field"))
            {
                if (elem.GetAttribute("sql").EqualsIgnoreCase("true"))
                {
                    sqlFields[elem.GetAttribute("name")] = elem.InnerText;
                }
            }

            // get the ordered column names
            m.tableInfo = mc.globalContext.schemaMaster.GetTableInfo(m.type, m.server, m.db, m.user, m.pw, m.name);
            string commitSizeSyntax = me.SelectNodeInnerText("commit_size", int.MaxValue.ToString());
            string commitSizeStr = mc.ParseSyntax(commitSizeSyntax).Read(mc).Nvl(int.MaxValue.ToString());
            int commitSize = int.Parse(commitSizeStr);
            m.bulkLoader = mc.globalContext.schemaMaster.GetBulkLoader(m.type, m.server, m.db, m.user, m.pw, m.name, commitSize);
            m.columnsInfo = m.tableInfo.columnInfo;

            List<string> passedParamInputs = new List<string>();
            List<string> passedValues = new List<string>();
            foreach (ColumnInfo p in m.columnsInfo)
            {
                m.orderedParamNames.Add(p.name);
                // if the value is sql
                if (sqlFields.ContainsKey(p.name))
                {
                    throw new Exception("marking a field with sql='true' is not supported for db_bulk_insert");
#if false
                    var mySqlSyntax = sqlFields[p.name];
                    var sqlValue = mc.SyntaxReadString(mySqlSyntax);
                    passedValues.Add(sqlValue);
                    passedParamInputs.Add("'" + p.name + "=['~" + mySqlSyntax + "~']'");
#endif
                }
                // else the value is variable
                else
                {
                    string paramValueSyntax = overrider.GetSyntax(p.name);
                    IReadString paramValueParsed = mc.ParseSyntax(paramValueSyntax);
                    m.readableParamsParsed[p.name] = paramValueParsed;
                    passedParamInputs.Add("'" + p.name + "=['~" + paramValueSyntax + "~']'");
                }
            }

            string beginArgs = passedParamInputs.Count > 0 ? passedParamInputs.Join("~', '~") : "''";
            string beginMsg = "'call bulk_db_insert " + m.name + "('~" + beginArgs + "~')'";
            string endMsg = "'done bulk_db_insert " + m.name + "('~" + beginArgs + "~')'";

            // add logging and main module
            //run.Add(mc.GetLogModule(beginMsg, m.logLevel));
            run.Add(m);
            //run.Add(mc.GetLogModule(endMsg, m.logLevel));
        }

        public void Run(ModuleContext mc)
        {
            if (this.bulkLoader != null)
            {
                string[] row = new string[this.readableParamsParsed.Count];
                int colNo = 0;
                foreach (string colName in this.readableParamsParsed.Keys)
                {
                    string paramValue = this.readableParamsParsed[colName].Read(mc);
                    if (paramValue.Equals("")) paramValue = null;
                    row[colNo++] = paramValue;
                }
                try
                {
                   int numRows=this.bulkLoader.InsertRow(row);
                   this.WriteNumRows(mc, numRows);
                }
                catch (Exception e)
                {
                    string msg = "db_bulk_insert errored.";
                    this.ProcessException(mc, msg, e);
                }
                return;
            }
            throw new Exception("db_bulk_insert no why is bulk loader null?");
        }
    }
}
