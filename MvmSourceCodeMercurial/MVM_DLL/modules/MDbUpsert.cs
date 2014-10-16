using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using Oracle.DataAccess.Client;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
/*
*/
namespace MVM
{
    class MDbUpsert : MDbCommon,IModuleSetup
    {
        // setup looks at the current db info, and does either oracle or sql
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDbUpsert m = new MDbUpsert();
            m.ParseDbCommon(me,mc,me.SelectNodeInnerText("./name"));
            if (m.type.EqualsIgnoreCase("oracle"))
            {
                new MDbUpsertOra().Setup(me, mc, run);
            }
            else if (m.type.EqualsIgnoreCase("sql"))
            {
                new MDbUpsertSql().Setup(me, mc, run);
            }
            else
            {
                throw new Exception("unexpected db type=[" + m.type + "]");
            }
        }

        abstract class MDbUpsertCommon : MDbCommon, IModuleRun
        {
            protected List<ColumnInfo> columnsInfo;
            protected TableInfo tableInfo;
            protected List<string> orderedParamNames = new List<string>();
            protected Dictionary<string, IReadString> readableParamsParsed = new Dictionary<string, IReadString>();
            protected string upsertStmt;
            protected List<string> updateKeyCols;
            protected List<string> passedParamInputs = new List<string>();
            protected Overrider overrider;
            protected Dictionary<string, string> sqlFields = new Dictionary<string, string>();
            protected void CommonSetupBefore(XmlElement me, ModuleContext mc, List<IModuleRun> run)
            {
                this.ParseDbCommon(me,mc,me.SelectNodeInnerText("./name"));
                this.overrider = new Overrider(me);

            // build up a list of tableName fields which should get sql values.
            foreach (var elem in me.SelectElements("./field"))
            {
                if (elem.GetAttribute("sql").EqualsIgnoreCase("true"))
                {
                    sqlFields[elem.GetAttribute("name")] = elem.InnerText;
                }
            }

            // get the ordered column names
                this.tableInfo = mc.globalContext.schemaMaster.GetTableInfo(this.type, this.server, this.db, this.user, this.pw, this.name);
                this.columnsInfo = this.tableInfo.columnInfo;

            // get the key fields
            List<string> keyColsSyntax = me.SelectNodesInnerText("./key_column_name");
                this.updateKeyCols = new List<string>();
            if (keyColsSyntax.Count > 0)
            {
                foreach (string kcs in keyColsSyntax)
                {
                        this.updateKeyCols.Add(mc.SyntaxReadString(kcs));
                }
            }
            else
            {
                    var pkCols = this.tableInfo.primaryKeyColumns;
                    this.updateKeyCols.AddRange(pkCols);
            }
            }

            protected void CommonSetupAfter(XmlElement me, ModuleContext mc, List<IModuleRun> run)
            {
                string beginArgs = passedParamInputs.Count > 0 ? passedParamInputs.Join("~', '~") : "''";
                string beginMsg = "'call db_upsert " + this.name + "('~" + beginArgs + "~') on (" + this.updateKeyCols.Join(",") + ")'";
                string endMsg = "'done db_upsert " + this.name + "('~" + beginArgs + "~') on (" + this.updateKeyCols.Join(",") + ") num_rows=['~" + this.numRowsSyntaxList[0] + "~']'";     
                //run.Add(mc.GetLogModule(beginMsg, m.logLevel));
                run.Add(this);
                //run.Add(mc.GetLogModule(endMsg, m.logLevel));
            }
            public abstract void Run(ModuleContext mc);
        }

        class MDbUpsertOra : MDbUpsertCommon, IModuleSetup, IModuleRun
        {
            private OracleCommand oracleCommand;
            public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
            {
                MDbUpsertOra m = new MDbUpsertOra();
                m.CommonSetupBefore(me, mc, run);

                 // build up a hash of all the passed values by column name.
                 Dictionary<string, string> columnValues = new Dictionary<string, string>();
                 List<string> passedValues = new List<string>();
                 List<string> updateSets = new List<string>();
                 foreach (ColumnInfo p in m.columnsInfo)
                 {
                     m.orderedParamNames.Add(p.name);
                     // if the value is sql
                     if (sqlFields.ContainsKey(p.name))
                     {
                         var sqlSyntax = sqlFields[p.name];
                         var sqlValue = mc.SyntaxReadString(sqlSyntax);
                         columnValues[p.name] = (sqlValue);
                         passedValues.Add(sqlValue);
                         updateSets.Add(p.name + "=" + sqlValue);
                         passedParamInputs.Add("'" + p.name + "=['~" + sqlSyntax + "~']'");
                     }
                     // else the value is variable
                     else
                     {
                         string varName = m.type.EqualsIgnoreCase("oracle") ? ":" + p.name : "@" + p.name;
                         columnValues[p.name] = (varName);
                         passedValues.Add(varName);
                         if (!m.updateKeyCols.Contains(p.name)) updateSets.Add(p.name + "=" + varName);
                         string paramValueSyntax = m.overrider.GetSyntax(p.name);
                         IReadString paramValueParsed = mc.ParseSyntax(paramValueSyntax);
                         m.readableParamsParsed[p.name] = paramValueParsed;
                         passedParamInputs.Add("'" + p.name + "=['~" + paramValueSyntax + "~']'");
                     }
                 }
                 List<string> updateKeyJoins = new List<string>();
                 foreach (string col in m.updateKeyCols)
                 {
                     string passedValue = columnValues[col];
                     updateKeyJoins.Add(col + "=" + passedValue);
                 }
                 m.upsertStmt =
                         "merge into " + m.name + @" using dual on (" + updateKeyJoins.Join(" and ") + ") "
                     + "when not matched then insert (" + m.tableInfo.columnNames.Join(",") + ") values (" + passedValues.Join(",") + ") "
                     + "when matched then update set " + updateSets.Join(",");

                 m.oracleCommand = new OracleCommand();
                 m.oracleCommand.BindByName = true;
                 m.oracleCommand.CommandText = m.upsertStmt;
                 foreach (string colName in m.readableParamsParsed.Keys)
                 {
                     string paramName = ":" + colName;
                     OracleParameter param = new OracleParameter(paramName, OracleDbType.NVarchar2);
                     m.oracleCommand.Parameters.Add(param);
             }

                m.CommonSetupAfter(me, mc, run);
            }

            public override void Run(ModuleContext mc)
             {
                this.RefreshDbInfo(mc);
                using (OracleConnection conn = DbUtilsOra.GetOraConnection(db, user, pw))
                {
                    try
                    {
                        this.oracleCommand.Connection = conn;
                        foreach (string colName in this.readableParamsParsed.Keys)
                        {
                            string paramName = ":" + colName;
                            string paramValue = this.readableParamsParsed[colName].Read(mc);
                            this.oracleCommand.Parameters[paramName].Value = paramValue;
                        }
                        int numRows = oracleCommand.ExecuteNonQuery();
                        this.WriteNumRows(mc, numRows);
                    }
                    catch (OracleException e)
                    {
                        string msg = "Error db_upsert: [" + this.oracleCommand.CommandText + "] params=[" + this.readableParamsParsed.Select(x => x.Key + "=" + x.Value.Read(mc)).JoinStrings(",") + "] using connString=[" + conn.ConnectionString + "] Error msg=" + e.Message;
                        this.ProcessOracleException(mc, msg, e);
                    }
                }
            }
        }

        class MDbUpsertSql : MDbUpsertCommon, IModuleSetup, IModuleRun
        {
            private SqlCommand sqlCommand;
            public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
            {
                MDbUpsertSql m = new MDbUpsertSql();
                m.CommonSetupBefore(me, mc, run);

                 // build up a hash of all the passed values by column name.
                 Dictionary<string, string> columnValues = new Dictionary<string, string>();
                 List<string> passedValues = new List<string>();
                 List<string> updateSets = new List<string>();
                 List<string> sourceSelects = new List<string>();
                 foreach (ColumnInfo p in m.columnsInfo)
                 {
                     m.orderedParamNames.Add(p.name);
                     // if the value is sql
                     if (sqlFields.ContainsKey(p.name))
                     {
                         var sqlSyntax = sqlFields[p.name];
                         var sqlValue = mc.SyntaxReadString(sqlSyntax);
                         columnValues[p.name] = (sqlValue);
                         passedValues.Add(sqlValue);
                         updateSets.Add("Target." + p.name + "=" + "Source." + p.name);
                         sourceSelects.Add(sqlValue + " as " + p.name);
                         passedParamInputs.Add("'" + p.name + "=['~" + sqlSyntax + "~']'");
                     }
                     // else the value is variable
                     else
                     {
                         string varName = m.type.EqualsIgnoreCase("oracle") ? ":" + p.name : "@" + p.name;
                         columnValues[p.name] = (varName);
                         passedValues.Add(varName);
                         if (!m.updateKeyCols.Contains(p.name)) updateSets.Add("Target." + p.name + "=" + "Source." + p.name);
                         sourceSelects.Add(varName+" as "+p.name);
                         string paramValueSyntax = m.overrider.GetSyntax(p.name);
                         IReadString paramValueParsed = mc.ParseSyntax(paramValueSyntax);
                         m.readableParamsParsed[p.name] = paramValueParsed;
                         passedParamInputs.Add("'" + p.name + "=['~" + paramValueSyntax + "~']'");
                     }
                 }
                 List<string> updateKeyJoins = new List<string>();
                 foreach (string col in m.updateKeyCols)
                 {
                     updateKeyJoins.Add("Source." + col + "=" + "Target." + col);
                 }
                 m.upsertStmt =
                         "merge into " + m.name + @" as Target using (select " + sourceSelects.Join(",") + ") as Source on (" + updateKeyJoins.Join(" and ") + ") "
                     + " when matched then update set " + updateSets.Join(",")
                     + " when not matched then insert (" + m.tableInfo.columnNames.Join(",") + ") values (" + m.tableInfo.columnNames.Select(c=>"Source."+c).JoinStrings(",") + ");"
                     ;

                 m.sqlCommand = new SqlCommand();
                 m.sqlCommand.CommandText = m.upsertStmt;
                 m.sqlCommand.CommandTimeout = 0;
                 foreach (string colName in m.readableParamsParsed.Keys)
                 {
                     string paramName = "@" + colName;
                     SqlParameter param = new SqlParameter(paramName, SqlDbType.NVarChar);
                     m.sqlCommand.Parameters.Add(param);
        }
                m.CommonSetupAfter(me, mc, run);
            }

            public override void Run(ModuleContext mc)
        {
            this.RefreshDbInfo(mc);
            List<string> orderedParamValues = new List<string>();
            using (SqlConnection conn = DbUtilsSql.GetSqlConnection(server, db, user, pw))
                {
                    try
                    {
                        this.sqlCommand.Connection = conn;
                        // set bind parameters
                            foreach (string colName in this.readableParamsParsed.Keys)
                            {
                            string paramName = "@" + colName;
                                string paramValue = this.readableParamsParsed[colName].Read(mc);
                                // treat '' as null for sqlserver
                                if (paramValue.Equals(""))
                                {
                                this.sqlCommand.Parameters[paramName].Value = System.Data.SqlTypes.SqlString.Null;
                                    paramValue = "SQL_NULL";
                                }
                                else
                                {
                                this.sqlCommand.Parameters[paramName].Value = paramValue;
                                }
                                orderedParamValues.Add(paramValue);
                            }
                        int numRows = sqlCommand.ExecuteNonQuery();
                            this.WriteNumRows(mc, numRows);
                        }
                    catch (SqlException e)
                    {
                        string msg = "Error db_upsert: " + upsertStmt + " (" + orderedParamValues.Join(",") + "). Ordered param names:" + this.orderedParamNames.Join(",") + ". Db error msg:" + e.Message;
                        this.ProcessSqlException(mc, msg, e);
                    }
                }
            }
            }
        }
    }
