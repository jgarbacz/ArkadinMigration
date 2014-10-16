//using System;
//using System.Collections.Generic;

//using System.Text;
//using System.Xml;
//using System.Diagnostics;
//using System.Data.SqlClient;
//using System.Data;
using NLog;

//namespace MVM
//{
//    class MLoggerDatabase : IModuleRun
//    {
//        private string msgSyntax;
//        private IReadString msgParsed;

//        private string logMessageFieldSyntax;
//        private IWriteString logMessageFieldParsed;

//        //private string logMessageDateFieldSyntax;
//        //private IModuleWritable logMessageDateFieldParsed;

//        private string logMessageDateColumn;
//        private string logLevelColumn;

//        // readableParamsParsed{columnname}=IModuleRecurse
//        private Dictionary<string, IReadString> readableParamsParsed = new Dictionary<string, IReadString>();

//        // log tableName ordered column names
//        private List<ColumnInfo> columnInfo = new List<ColumnInfo>();

//        // login info from first time we run this newModule
//        private IDbLoginInfo dbInfo;

//        // log tableName name
//        private string logTableName;

//        // the insert statement with :params in it
//        private string insertStmt;

//        // log level of the message
//        private int msgLevel;

//        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run, string loggerOid, string msgSyntax, int msgLevel)
//        {
//            // Evaluate the log level and return if msg isn't >= log level.
//            string logLevelStr = mc.ReadObjectField(loggerOid, "log_level");
//            if (logLevelStr.Equals("")) logLevelStr = mc.globalContext["log_level"];
//            if (logLevelStr.Equals("")) logLevelStr = "all";
//            int logLevel = mc.GetLogLevelInt(logLevelStr);
//            if (msgLevel < logLevel) return;

//            // Ok, we're going to log it, so setup the runtime log write
//            MLoggerDatabase m = new MLoggerDatabase();
//            m.msgLevel = msgLevel;
//            string logMessageField = mc.ReadObjectField(loggerOid, "log_message_field", "message");
//            m.logMessageFieldSyntax = "OBJECT." + logMessageField;
//            m.logMessageFieldParsed = mc.ParseWritableSyntax(m.logMessageFieldSyntax);
//            m.logMessageDateColumn = mc.ReadObjectField(loggerOid, "log_message_date_column", "message_date");
//            m.logLevelColumn = mc.ReadObjectField(loggerOid, "log_level_column", "log_level");
//            m.msgSyntax = msgSyntax;
//            m.msgParsed = mc.ParseSyntax(msgSyntax);

//            // Get the login info
//            m.dbInfo = StaticDbLoginInfo.FromObjectId(mc, loggerOid);

//            // Get the log tablename and tableName fields
//            m.logTableName = mc.ReadObjectField(loggerOid, "log_table_name", "MVM_LOG");
//            m.columnInfo = mc.globalContext.schemaMaster.GetTableInfo(mc, m.dbInfo, m.logTableName).columnInfo;

//            // Build the insert statement, making sure to use the sysdate command for the message date column
//            List<string> insertValues = new List<string>();
//            List<string> insertColumns = new List<string>();
//            foreach (ColumnInfo info in m.columnInfo)
//            {
//                string columnName = info.name;
//                insertColumns.Add(columnName);
//                if (columnName.Equals(m.logMessageDateColumn))
//                {
//                    if (m.dbInfo.GetType(mc).EqualsIgnoreCase("oracle"))
//                        insertValues.Add("SYSDATE");
//                    else
//                        insertValues.Add("getdate()");

//                }
//                else if (columnName.Equals(m.logLevelColumn))
//                {
//                    insertValues.Add(msgLevel.ToString());
//                }
//                else
//                {
//                    if (m.dbInfo.GetType(mc).EqualsIgnoreCase("oracle"))
//                        insertValues.Add(":" + columnName);
//                    else
//                        insertValues.Add("@" + columnName);
//                    string overrideColumnName = "override_" + columnName;
//                    string tempField = "TEMP." + columnName;
//                    string objectField = "OBJECT." + columnName;
//                    string globalField = "GLOBAL." + columnName;
//                    string defaultField = "(" + tempField + " ne ''?" + tempField + ":(" + objectField + " ne ''?" + objectField + ":" + globalField + "))";
//                    string columnSyntax = mc.ReadObjectField(loggerOid, overrideColumnName, defaultField);
//                    m.readableParamsParsed[columnName] = mc.ParseSyntax(columnSyntax);
//                }
//            }
//            m.insertStmt = "insert into " + m.logTableName + " (" + insertColumns.Join(",") + ") values (" + insertValues.Join(",") + ")";
//            run.Add(m);
//        }

//        public void Run(ModuleContext mc)
//        {
//            // Evaluate the log message the log message
//            string logMessage = msgParsed.Read(mc);
//            string logMessageDate = mc.Now();
//            // lock to the object so we don't hit a race condition
//            Dictionary<string, string> readableParamValues = new Dictionary<string, string>();
//            lock (mc.objectData)
//            {
//                this.logMessageFieldParsed.Write(mc, logMessage);
//                foreach (string columnName in this.readableParamsParsed.Keys)
//                {
//                    readableParamValues[columnName] = this.readableParamsParsed[columnName].Read(mc);
//                }
//            }

//            // do the insert
//            string type = this.dbInfo.GetType(mc);
//            string server = this.dbInfo.GetServer(mc);
//            string db = this.dbInfo.GetDb(mc);
//            string user = this.dbInfo.GetUser(mc);
//            string pw = this.dbInfo.GetPw(mc);

//            if (type.EqualsIgnoreCase("oracle"))
//            {
//                using (OracleConnection conn = DbUtilsOra.GetOraConnection(db, user, pw))
//                {
                    
//                    List<string> bindVars = new List<string>();
//                    try
//                    {
//                        using (OracleCommand stmt = new OracleCommand(this.insertStmt, conn))
//                        {
//                            stmt.BindByName = true;
//                            foreach (string columnName in this.readableParamsParsed.Keys)
//                            {
//                                string paramName = ":" + columnName;
//                                string paramValue = readableParamValues[columnName];
//                                if (paramValue.Length > 4000) paramValue = paramValue.Substring(0, 4000);
//                                bindVars.Add(paramName + '=' + paramValue);
//                                var param = stmt.CreateParameter();
//                                param.ParameterName = paramName;
//                                param.OracleDbType = OracleDbType.NVarchar2;
//                                param.Value = paramValue;
//                                stmt.Parameters.Add(param);
//                            }
//                            int numRows = stmt.ExecuteNonQuery();
//                        }
//                    }
//                    catch (Exception e)
//                    {
//                        throw new Exception("Error, running insert=[" + insertStmt + "] bind_vars=[" + bindVars.Join(",") + "] using connString=[" + conn.ConnectionString + "], msg=" + e.Message, e);
//                    }
//                }
//            }

//            if (type.EqualsIgnoreCase("sql"))
//            {
//                using (SqlConnection conn = DbUtils.GetSqlConnection(server, db, user, pw))
//                {
//                    List<string> bindVars = new List<string>();
//                    try
//                    {
//                        using (SqlCommand stmt = new SqlCommand(this.insertStmt, conn))
//                        {
//                            foreach (string columnName in this.readableParamsParsed.Keys)
//                            {
//                                string paramName = "@" + columnName;
//                                string paramValue = readableParamValues[columnName];
//                                if (paramValue.Length > 4000) paramValue = paramValue.Substring(0, 4000);
//                                bindVars.Add(paramName + '=' + paramValue);
//                                var param = stmt.CreateParameter();
//                                param.ParameterName = paramName;
//                                param.SqlDbType = SqlDbType.VarChar;
//                                param.Value = paramValue;
//                                stmt.Parameters.Add(param);
//                            }
//                            int numRows = stmt.ExecuteNonQuery();
//                        }
//                    }
//                    catch (Exception e)
//                    {
//                        throw new Exception("Error, running insert=[" + insertStmt + "] bind_vars=[" + bindVars.Join(",") + "] using connString=[" + conn.ConnectionString + "], msg=" + e.Message, e);
//                    }
//                }
//            }
//            // force fatal to throw exception.
//            if (this.msgLevel.Equals(LogLevels.FATAL))
//            {
//                throw new Exception("mvm user defined fatal error:" + logMessage);
//            }
//        }

//        public void Log(ModuleContext mc, ILogger log)
//        {
//            log.LogInfo("logger_console:" + this.msgSyntax);
//        }
//    }
//}
