using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using Oracle.DataAccess.Client;
using System.Data.SqlClient;
using System.Data;

/*
 
 OBJECT.database_type ='SQL'
 OBJECT.database_username='MT'
 OBJECT.database_name='MYDB'
 OBJECT.database_server ="1.2.3.4"
 OBJECT.database_password="MetraTEch1"
 
 TODO:
 <default_db_context>
 <db>OBJECT.db</db>
 <user>OBJECT(OBJECT.login).user</user>
 <pw>GLOBAL.pw</pw> 
 <num_rows>GLOBAL.pw</num_rows> 
 <exception>GLOBAL.pw</exception> 
 <exception_proc>GLOBAL.pw</exception_proc> 
 <default_db_context>

<db_insert>
<db>GLOBAL.db</db>
<user>GLOBAL.user</user>
<pw>GLOBAL.pw</pw> 
<name>'my_proc'</name>
<field name='myin'>'123'</param>
<field name='mydate' sql='true'>'SYSDATE'</param>
<object_id>TEMP.oid</object_id>
<num_rows>OBJECT.database_num_rows</num_rows>
<exception>OBJECT.database_exception</exception>
<exception_proc><exception_proc>
</db_insert>
*/

namespace MVM
{
    class MDbInsert : MDbCommon,IModuleSetup
    {
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDbInsert m = new MDbInsert();
            m.ParseDbCommon(me,mc,me.SelectNodeInnerText("./name"));
            if (m.type.EqualsIgnoreCase("oracle"))
            {
                new MDbInsertOra().Setup(me, mc, run);
            }
            else if (m.type.EqualsIgnoreCase("sql"))
            {
                new MDbInsertSql().Setup(me, mc, run);
            }
            else
            {
                throw new Exception("unexpected db type=[" + m.type + "]");
            }
        }
    }


    abstract class MDbInsertCommon : MDbCommon, IModuleRun
    {
        protected List<ColumnInfo> columnsInfo;
        protected TableInfo tableInfo;
        protected List<string> orderedParamNames = new List<string>();
        protected Dictionary<string, IReadString> readableParamsParsed = new Dictionary<string, IReadString>();
        protected string insertStmt;
        protected List<string> passedParamInputs = new List<string>();
        protected List<string> passedValues = new List<string>();
        protected void CommonSetupBefore(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            this.ParseDbCommon(me, mc, me.SelectNodeInnerText("./name"));
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
            this.tableInfo = mc.globalContext.schemaMaster.GetTableInfo(this.type, this.server, this.db, this.user, this.pw, this.name);
            this.columnsInfo = this.tableInfo.columnInfo;

           
            foreach (ColumnInfo p in this.columnsInfo)
            {
                this.orderedParamNames.Add(p.name);
                // if the value is sql
                if (sqlFields.ContainsKey(p.name))
                {
                    var sqlSyntax = sqlFields[p.name];
                    var sqlValue = mc.SyntaxReadString(sqlSyntax);
                    passedValues.Add(sqlValue);
                    passedParamInputs.Add("'" + p.name + "=['~" + sqlSyntax + "~']'");
                }
                // else the value is variable
                else
                {
                    if (this.type.EqualsIgnoreCase("oracle")) passedValues.Add(":" + p.name);
                    else passedValues.Add("@" + p.name);
                    this.orderedParamNames.Add(p.name);
                    string paramValueSyntax = overrider.GetSyntax(p.name);
                    IReadString paramValueParsed = mc.ParseSyntax(paramValueSyntax);
                    this.readableParamsParsed[p.name] = paramValueParsed;
                    passedParamInputs.Add("'" + p.name + "=['~" + paramValueSyntax + "~']'");
                }
            }

            // build the insert statement
            this.insertStmt = "insert into " + this.name + " (" + this.tableInfo.columnNames.Join(",") + ") values (" + passedValues.Join(",") + ")";

        }
        protected void CommonSetupAfter(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            string beginArgs = passedParamInputs.Count > 0 ? passedParamInputs.Join("~', '~") : "''";
            string beginMsg = "'call db_insert " + this.name + "('~" + beginArgs + "~')'";
            string endMsg = "'done db_insert " + this.name + "('~" + beginArgs + "~') num_rows=['~" + this.numRowsSyntaxList[0] + "~']'";
            run.Add(mc.GetLogModule(beginMsg, this.logLevel, true));
            run.Add(this);
            run.Add(mc.GetLogModule(endMsg, this.logLevel, true));
        }
        public abstract void Run(ModuleContext mc);
    }



    class MDbInsertSql : MDbInsertCommon
            {
        private SqlCommand sqlCommand;
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
                {
            MDbInsertSql m = new MDbInsertSql();
            m.CommonSetupBefore(me, mc, run);
            m.sqlCommand = new SqlCommand();
            m.sqlCommand.CommandText = m.insertStmt;
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
            List<string> orderedParamValues = new List<string>();
            using (SqlConnection conn = DbUtilsSql.GetSqlConnection(this.server, this.db, this.user, this.pw))
            {
                    try
                    {
                    this.sqlCommand.Connection = conn;
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
                    string msg = "Error db_insert: " + this.insertStmt + " (" + orderedParamValues.Join(",") + "). Ordered param names:" + this.orderedParamNames.Join(",") + ". Db error msg:" + e.Message;
                        this.ProcessSqlException(mc, msg, e);
                    }
                }
            }
    }

    class MDbInsertOra : MDbInsertCommon
            {
        private OracleCommand oracleCommand;
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDbInsertOra m = new MDbInsertOra();
            m.CommonSetupBefore(me, mc, run);
            m.oracleCommand = new OracleCommand();
            m.oracleCommand.BindByName = true;
            m.oracleCommand.CommandText = m.insertStmt;
            foreach (string colName in m.readableParamsParsed.Keys)
            {
                var parameter = new OracleParameter(":" + colName, OracleDbType.NVarchar2);
                m.oracleCommand.Parameters.Add(parameter);
            }
            m.CommonSetupAfter(me, mc, run);
        }
        public override void Run(ModuleContext mc)
        {
            List<string> orderedParamValues = new List<string>();
            using (OracleConnection conn = DbUtilsOra.GetOraConnection(db, user, pw))
            {
                try
                {
                    oracleCommand.Connection = conn;
                    foreach (string colName in readableParamsParsed.Keys)
                    {
                        string paramName = ":" + colName;
                        string paramValue = this.readableParamsParsed[colName].Read(mc);
                        oracleCommand.Parameters[paramName].Value=paramValue;
                        orderedParamValues.Add(paramValue);
    }
                    int numRows = oracleCommand.ExecuteNonQuery();
                    WriteNumRows(mc, numRows);
}
                catch (OracleException e)
                {
                    string msg = "Error db_insert: " + insertStmt + " (" + orderedParamValues.Join(",") + "). Ordered param names:" + orderedParamNames.Join(",") + ". Db error msg:" + e.Message;
                    this.ProcessOracleException(mc, msg, e);
                }
            }
        }
    }
}
