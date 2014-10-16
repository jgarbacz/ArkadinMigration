using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using Oracle.DataAccess.Client;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MVM
{
    class MDbExecute : MDbCommon, IModuleSetup
    {
        // setup looks at the current db info, and does either oracle or sql
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDbExecute m = new MDbExecute();
            m.ParseDbCommon(me, mc, "'db_execute'");
            if (m.type.EqualsIgnoreCase("oracle"))
            {
                new MDbExecuteOra().Setup(me, mc, run);
            }
            else if (m.type.EqualsIgnoreCase("sql"))
            {
                new MDbExecuteSql().Setup(me, mc, run);
            }
            else
            {
                throw new Exception("unexpected db type=[" + m.type + "]");
            }
        }

       abstract class MDbExecuteCommon : MDbCommon, IModuleRun
        {
            protected string stmtSyntax;
            protected string stmtString;
            protected string bindStmtString;
            protected Dictionary<string, string> bindsSyntax;
            protected Dictionary<string, IReadString> bindsParsed = new Dictionary<string, IReadString>();
            protected Dictionary<string, IWriteString> outBindsParsed = new Dictionary<string, IWriteString>();

            protected void CommonSetupBefore(XmlElement me, ModuleContext mc, List<IModuleRun> run)
            {
                this.ParseDbCommon(me, mc, "'db_execute'");
                this.stmtSyntax = this.SelectTypedElem(me, "statement").InnerText;
                this.stmtString = mc.SyntaxReadString(this.stmtSyntax);
            }
            protected void CommonSetupAfter(XmlElement me, ModuleContext mc, List<IModuleRun> run)
            {
                // log message are static 
                string beginMsg = mc.SyntaxReadString("'call db_execute ['~" + this.stmtSyntax + "~']'");
                string endMsg = mc.SyntaxReadString("'done db_execute ['~" + this.stmtSyntax + "~'] num_rows=['~" + this.numRowsSyntaxList[0] + "~']'");
                run.Add(mc.GetLogModule(beginMsg, this.logLevel, true));
                run.Add(this);
                run.Add(mc.GetLogModule(endMsg, this.logLevel, true));
            }
            public abstract void Run(ModuleContext mc);
        }

        class MDbExecuteOra : MDbExecuteCommon, IModuleSetup, IModuleRun
        {
            private OracleCommand oracleCommand;
            public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
            {
                MDbExecuteOra m = new MDbExecuteOra();
                m.CommonSetupBefore(me, mc, run);
               
                // strip newlines
                m.stmtString = m.stmtString.Replace("\r\n", " ");
                m.stmtString = m.stmtString.Replace("\n", " ");

                // Parse out $${} notation for bind variables.
                m.bindStmtString = mc.TranslateDbBinds(m.stmtString, out m.bindsSyntax, ":");
                m.oracleCommand = new OracleCommand();
                m.oracleCommand.BindByName = true;
                m.oracleCommand.CommandText = m.bindStmtString;
                foreach (var entry in m.bindsSyntax)
                {
                    string val = entry.Value;
                    ParameterDirection dir = ParameterDirection.Input;
                    int output = 0;
                    if (val.StartsWith("OUT_INT:"))
                    {
                        output = 1;
                    }
                    else if (val.StartsWith("OUT_STR:"))
                    {
                        output = 2;
                    }
                    if (output > 0)
                    {
                        // This means we want to bind it as output
                        val = entry.Value.Substring(8);
                        dir = ParameterDirection.Output;
                        m.outBindsParsed[entry.Key] = mc.ParseWritableSyntax(val);
                    }
                    else
                    {
                        m.bindsParsed[entry.Key] = mc.ParseSyntax(val);
                    }
                    OracleParameter param = new OracleParameter(entry.Key, OracleDbType.NVarchar2);
                    param.Direction = dir;
                    if (output == 1)
                    {
                        param.DbType = DbType.Int32;
                    }
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
                        // set in bind parameters
                        foreach (var entry in this.bindsParsed)
                        {
                            string paramName = entry.Key;
                            string paramValue = entry.Value.Read(mc);
                            this.oracleCommand.Parameters[paramName].Value = paramValue;
                        }
                        // execute the statement
                        int numRows = this.oracleCommand.ExecuteNonQuery();
                        // read out bind vars
                        foreach (var entry in this.outBindsParsed)
                        {
                            string paramName = entry.Key;
                            entry.Value.Write(mc, this.oracleCommand.Parameters[paramName].Value.ToString());
                        }
                        this.WriteNumRows(mc, numRows);
                    }
                    catch (OracleException e)
                    {
                        string msg = "Error db_execute: [" + this.oracleCommand.CommandText + "] params=[" + this.bindsParsed.Select(x => x.Key + "=" + x.Value.Read(mc)).JoinStrings(",") + "] using connString=[" + conn.ConnectionString + "] Error msg=" + e.Message;
                        this.ProcessOracleException(mc, msg, e);
                    }
                }
            }
        }

        class MDbExecuteSql : MDbExecuteCommon, IModuleSetup, IModuleRun
        {
            private SqlCommand sqlCommand;
            public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
            {
                MDbExecuteSql m = new MDbExecuteSql();
                m.CommonSetupBefore(me, mc, run);

                // Parse out $${} notation for bind variables.
                m.bindStmtString = mc.TranslateDbBinds(m.stmtString, out m.bindsSyntax, "@");
                m.sqlCommand = new SqlCommand();
                m.sqlCommand.CommandText = m.bindStmtString;
                m.sqlCommand.CommandTimeout = 0;
                foreach (var entry in m.bindsSyntax)
                {
                    string val = entry.Value;
                    ParameterDirection dir = ParameterDirection.Input;
                    int output = 0;
                    if (val.StartsWith("OUT_INT:"))
                    {
                        output = 1;
                    }
                    else if (val.StartsWith("OUT_STR:"))
                    {
                        output = 2;
                    }
                    if (output > 0)
                    {
                        // This means we want to bind it as output
                        val = entry.Value.Substring(8);
                        dir = ParameterDirection.Output;
                        m.outBindsParsed[entry.Key] = mc.ParseWritableSyntax(val);
                    }
                    else
                    {
                        m.bindsParsed[entry.Key] = mc.ParseSyntax(val);
                    }
                    SqlParameter param = new SqlParameter(entry.Key,SqlDbType.NVarChar);
                    param.Direction = dir;
                    if (output == 1)
                    {
                        param.DbType = DbType.Int32;
                    }
                    m.sqlCommand.Parameters.Add(param);
                }

                m.CommonSetupAfter(me, mc, run);
        }

            public override void Run(ModuleContext mc)
        {
            this.RefreshDbInfo(mc);
            using (SqlConnection conn = DbUtilsSql.GetSqlConnection(server, db, user, pw))
                {
                    this.sqlCommand.Connection = conn;
                    // set in bind parameters
                    foreach (var entry in this.bindsParsed)
                    {
                        string paramName = entry.Key;
                        string paramValue = entry.Value.Read(mc);
                        this.sqlCommand.Parameters[paramName].Value = paramValue;
                    }
                    try
                    {
                            int numRows = this.sqlCommand.ExecuteNonQuery();
                        // read out bind vars
                            foreach (var entry in this.outBindsParsed)
                            {
                                string paramName = entry.Key;
                            entry.Value.Write(mc, this.sqlCommand.Parameters[paramName].Value.ToString());
                            }
                            this.WriteNumRows(mc, numRows);
                    }
                    catch (SqlException e)
                    {
                        string msg = "Error db_execute: " + this.sqlCommand.CommandText + ". Db error msg:" + e.Message;
                        this.ProcessSqlException(mc, msg, e);
                    }
                }
            }
            }
        }
    }
