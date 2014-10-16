using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Oracle.DataAccess.Client;
using NLog;
namespace MVM
{
    public class OraTableInfo : TableInfo
    {
        public OraTableInfo(StaticDbLoginInfo dbinfo, string table)
            : base(dbinfo, table)
        {
            List<ColumnInfo> columnInfo = new List<ColumnInfo>();
            using (OracleConnection conn = DbUtilsOra.GetOraConnection(dbinfo.db, dbinfo.user, dbinfo.pw))
            {
                // make sure the tableName exists as a table or a view
                {
                    string fetchTableName;
                    string queryString =
                        @"select lower(a.table_name)
                        from all_tables a, user_synonyms b
                        where synonym_name = '" + table.ToUpper() + @"' and a.owner = b.table_owner and b.table_name = a.table_name
                        union
                        select lower(table_name)
                        from user_tables
                        where table_name = '" + table.ToUpper() + "'";
                    try
                    {
                        fetchTableName = System.Convert.ToString(new OracleCommand(queryString, conn).ExecuteScalar());
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Running query string=[" + queryString + "] using connString=[" + conn.ConnectionString + "], msg=" + e.Message, e);
                    }
                    if (fetchTableName.Equals(""))
                    {
                        string error1 = "[" + queryString + "] returned no rows.";

                        // see if it is a view
                        string queryString2 =
                            @"select lower(a.view_name)
                            from all_views a, user_synonyms b
                            where synonym_name = '" + table.ToUpper() + @"' and a.owner = b.table_owner and b.table_name = a.view_name
                            union
                            select lower(view_name)
                            from user_views
                            where view_name = '" + table.ToUpper() + "'";
                        try
                        {
                            fetchTableName = System.Convert.ToString(new OracleCommand(queryString2, conn).ExecuteScalar());
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Error running query string=[" + queryString2 + "] using connString=[" + conn.ConnectionString + "], msg=" + e.Message, e);
                        }
                        if (fetchTableName.Equals(""))
                        {
                            string error2 = "[" + queryString2 + "] returned no rows.";
                            throw new Exception("Cannot get table info for table [" + table + "] because [" + error1 + "] and [" + error2 + "]");
                        }
                    }
                }

                // ok, it does so get the tableName info
                {
                    // Can't have LONGs in a union so split it up
                    string[] queryStrings = {
                        @"select lower(a.column_name), a.column_id, a.data_type, a.nullable, case when a.data_type in ('NUMBER','BINARY_FLOAT','BINARY_DOUBLE') then 1 else 0 end is_numeric,
                        data_scale as numeric_scale, case when lower(data_type) like '%char%' then char_length else data_precision end as length, data_default as column_default
                        from all_tab_columns a, user_synonyms b
                        where synonym_name = '" + table.ToUpper() + @"' and a.owner = b.table_owner and b.table_name = a.table_name
                        order by column_id",
                        @"select lower(a.column_name), a.column_id, a.data_type, a.nullable, case when a.data_type in ('NUMBER','BINARY_FLOAT','BINARY_DOUBLE') then 1 else 0 end is_numeric,
                        data_scale as numeric_scale, case when lower(data_type) like '%char%' then char_length else data_precision end as length, data_default as column_default
                        from user_tab_columns a
                        where table_name = '" + table.ToUpper() + @"'
                        order by column_id"
                    };
                    Dictionary<string, bool> seen = new Dictionary<string, bool>();

                    foreach (var queryString in queryStrings)
                    {
                        OracleCommand command = new OracleCommand(queryString, conn);
                        try
                        {
                            OracleDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                string name = reader.GetString(0);
                                if (seen.ContainsKey(name))
                                {
                                    continue;
                                }
                                ColumnInfo info = new ColumnInfo();
                                info.name = name;
                                info.position = Convert.ToInt32(reader.GetValue(1));
                                info.type = reader.GetString(2);
                                info.nullable = reader.GetString(3).Equals('Y');
                                info.numeric = Convert.ToInt32(reader.GetValue(4)) == 1;
                                info.scale = reader.IsDBNull(5) ? -1 : Convert.ToInt32(reader.GetValue(5));
                                info.length = reader.IsDBNull(6) ? -1 : Convert.ToInt32(reader.GetValue(6));
                                info.defaultValue = reader.IsDBNull(7) ? "" : reader.GetString(7);
                                columnInfo.Add(info);
                                seen[name] = true;
                            }
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Error running query string=[" + queryString + "] using connString=[" + conn.ConnectionString + "], msg=" + e.Message, e);
                        }
                    }
                    columnInfo.OrderBy(ci => ci.position);
                }
            }
            this.SetColumnInfo(columnInfo);
            this.changeTrackingEnabled = false;
            this.owner = dbinfo.user;
        }

        public override void WriteLoaderCtrl(string fileName, string fieldDelim, string recordDelim, string charset, Dictionary<string, string> dateFields)
        {
            DbUtilsOra.WriteOraSqlldrCtrl(fileName, this.tableName, fieldDelim, recordDelim, this.columnNames, charset, dateFields);
        }

        public override List<string> GetPrimaryKeyColumns()
        {
            string query =
                @"SELECT lower(cols.column_name)
                FROM user_constraints cons, user_cons_columns cols
                WHERE cols.table_name = upper('" + tableName + @"')
                AND cons.constraint_type = 'P'
                AND cons.constraint_name = cols.constraint_name
                AND cons.owner = cols.owner
                ORDER BY cols.position asc";
            List<string> pkCols = new OraQueryDispatcher().DbQueryToList(this.dbLoginInfo.server, this.dbLoginInfo.db, this.dbLoginInfo.user, this.dbLoginInfo.pw, query);
            return pkCols;
        }
    }

    public class OraProcInfo : DbProcInfo
    {
        public OraProcInfo(StaticDbLoginInfo dbinfo, string procName)
            : base(dbinfo, procName)
        {
            using (OracleConnection conn = DbUtilsOra.GetOraConnection(dbinfo.db, dbinfo.user, dbinfo.pw))
            {
                string queryString =
                    "select a.position, lower(a.argument_name),a.data_type,a.in_out,a.data_length from all_arguments a, user_synonyms b where " +
                    "synonym_name = '" + procName.ToUpper() + "' and a.owner = b.table_owner and b.table_name = a.object_name " +
                    "union " +
                    "select position, lower(argument_name), data_type, in_out, data_length from user_arguments where object_name = '" + procName.ToUpper() + "' " +
                    "order by position";

                OracleCommand command = new OracleCommand(queryString, conn);
                try
                {
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ParamInfo pi = new ParamInfo();
                        pi.position = Convert.ToInt32(reader.GetValue(0));
                        pi.name = reader.GetString(1);
                        pi.type = reader.GetString(2);
                        pi.mode = reader.GetString(3);
                        paramInfo.Add(pi);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Error running query string=[" + queryString + "] using connString=[" + conn.ConnectionString + "], msg=" + e.Message, e);
                }
            }
        }

        public override int CallProc(Dictionary<string, string> procParams)
        {
            List<string> orderedParamValues = new List<string>();
            using (OracleConnection conn = DbUtilsOra.GetOraConnection(dbLoginInfo.db, dbLoginInfo.user, dbLoginInfo.pw))
            {
                try
                {
                    using (OracleCommand stmt = new OracleCommand(this.procName, conn))
                    {
                        stmt.CommandType = CommandType.StoredProcedure;
                        stmt.BindByName = true;
                        foreach (ParamInfo p in paramInfo)
                        {
                            if (p.mode.Equals("IN"))
                            {
                                string paramValue = procParams.GetValueDefaulted(p.name, "");
                                var param = stmt.CreateParameter();
                                param.ParameterName = p.name;
                                param.OracleDbType = OracleDbType.NVarchar2;
                                param.Value = paramValue;
                                stmt.Parameters.Add(param);
                                orderedParamValues.Add(paramValue);
                            }
                            else if (p.mode.Equals("OUT"))
                            {
                                var param = stmt.CreateParameter();
                                param.ParameterName = p.name;
                                param.Direction = ParameterDirection.Output;
                                param.OracleDbType = OracleDbType.NVarchar2;
                                param.Size = 4000;
                                stmt.Parameters.Add(param);
                                orderedParamValues.Add("OUT_PARAM");
                            }
                            else if (p.mode.Equals("IN/OUT"))
                            {
                                var param = stmt.CreateParameter();
                                string paramValue = procParams.GetValueDefaulted(p.name, "");
                                param.ParameterName = p.name;
                                param.Direction = ParameterDirection.InputOutput;
                                param.OracleDbType = OracleDbType.NVarchar2;
                                param.Size = 4000;
                                param.Value = paramValue;
                                stmt.Parameters.Add(param);
                                orderedParamValues.Add(paramValue);
                            }
                            else
                            {
                                throw new Exception("Uknown parameter mode [" + p.mode + "] for param [" + p.name + "]");
                            }
                        }
                        int numRows = stmt.ExecuteNonQuery();
                        foreach (var p in paramInfo.Where(pi => pi.mode.Contains("OUT")))
                        {
                            procParams[p.name] = stmt.Parameters[p.name].Value.ToString();
                        }
                        return numRows;
                    }
                }
                catch (OracleException e)
                {
                    var orderedParamNames = paramInfo.Select(p => p.name);
                    string msg = "Error calling proc: " + this.procName + " (" + orderedParamValues.Join(",") + "). Ordered param names:" + orderedParamNames.JoinStrings(",") + ". Db error msg:" + e.Message;
                    throw new Exception(msg, e);
                }
            }
        }
    }

    public class OraQueryDispatcher : DbQueryDispatcher
    {
        /**
         * Calls datacopy to dump a query to a file
         */
        public override void QueryToFile(string server, string db, string user, string pw, string query, string order, string file, string ctrl, string fdel, string rdel)
        {
            if (order != null) query = query + " order by " + order;
            string cmd = "datacopy.exe";
            //string arguments = "-O -d \"oracle:" + user + "/" + pw + "@" + db + "\" -q \"" + query + "\" -D \"" + file + "\" -C \"" + ctrl + "\"";
            string[] args = new string[] { "-O", "-d", "oracle:" + user + "/" + pw + "@" + db, "-q", query, "-D", file, "-C", ctrl, "-f", fdel.InterpolateEscapesReverse(), "-r", rdel.InterpolateEscapesReverse() };
            string stdOut, stdErr;
            int exitCode;
            SystemCommand.RunCmd(cmd, args, out stdOut, out stdErr, out exitCode);
            if (exitCode != 0)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("Error called external cmd that returned non-zero value:" + exitCode);
                msg.AppendLine(cmd + " " + args.SurroundAll("\"", "\"").Join(" "));
                msg.AppendLine("Output:");
                msg.AppendLine(stdErr);
                throw new Exception(msg.ToString());
            }
        }

        public override IEnumerable<Dictionary<string, string>> DbQueryToEnumerableDictionary(string server, string db, string user, string pw, string queryString)
        {
            throw new Exception("Not implemented yet");
        }

        public override List<Dictionary<string, string>> DbQueryToListOfDictionary(string server, string db, string user, string pw, string queryString)
        {
            List<Dictionary<string, string>> results = new List<Dictionary<string, string>>();
            IDbLoginInfo dbLoginInfo = new StaticDbLoginInfo(db, db, user, pw, "oracle");
            //List<ColumnInfo> columnInfo = new List<ColumnInfo>();
            //logger.Info("connecting with String: {0}", connString);
            using (OracleConnection conn = DbUtilsOra.GetOraConnection(db, user, pw))
            {
                //logger.Info("connected String: {0}", conn.ConnectionString);
                OracleCommand command = new OracleCommand(queryString, conn);
                try
                {
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Dictionary<string, string> dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dict.Add(reader.GetName(i), reader[i].ToString());
                        }
                        results.Add(dict);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Error running query string=[" + queryString + "] using connString=[" + conn.ConnectionString + "], msg=" + e.Message, e);
                }
            }
            return results;
        }

        public override List<string> DbQueryToList(string server, string db, string user, string pw, string queryString)
        {
            List<string> results = new List<string>();
            using (OracleConnection conn = DbUtilsOra.GetOraConnection(db, user, pw))
            {
                OracleCommand command = new OracleCommand(queryString, conn);
                try
                {
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        results.Add(reader.GetString(0));
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Error running query string=[" + queryString + "] using connString=[" + conn.ConnectionString + "], msg=" + e.Message, e);
                }
            }
            return results;
        }

        public override int DbExecute(string server, string db, string user, string pw, string stmtString)
        {
            OracleConnection oracleConnection = null;
            OracleCommand oracleCommand = null;

            try
            {
                // strip newlines
                stmtString = stmtString.Replace("\r\n", " ");
                stmtString = stmtString.Replace("\n", " ");
                using (oracleConnection = DbUtilsOra.GetOraConnection(db, user, pw))
                {
                    oracleCommand = new OracleCommand();
                    oracleCommand.Connection = oracleConnection;
                    oracleCommand.CommandText = stmtString;
                    int numRows = oracleCommand.ExecuteNonQuery();
                    return numRows;
                }
            }
            catch (Exception e)
            {
                string msg = "Error in db_execute: [" + oracleCommand.CommandText + "] using connString=[" + oracleConnection.ConnectionString + "] Error msg=" + e.Message;
                throw new Exception(msg, e);
            }
        }

        /**
         * Make data copy create a sql loader control file
         */
        public override void DumpCtrl(string server, string db, string user, string pw, string table, string ctrl, string fdel, string rdel)
        {
            string query = "select * from " + table + " where 1=2";
            string cmd = "datacopy.exe";
            string[] args = new string[] { "-O", "-D", "-", "-d", "oracle:" + user + "/" + pw + "@" + db, "-q", query, "-C", ctrl, "-f", fdel, "-r", rdel, "-t", table };
            string stdOut, stdErr;
            int exitCode;
            SystemCommand.RunCmd(cmd, args, out stdOut, out stdErr, out exitCode);
            if (exitCode != 0)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("Error called external cmd that returned non-zero value:" + exitCode);
                msg.AppendLine(cmd + " " + args.SurroundAll("\"", "\"").Join(" "));
                msg.AppendLine("Output:");
                msg.AppendLine(stdErr);
                throw new Exception(msg.ToString());
            }
        }

        public override Dictionary<string, string> DbParameters(string server, string db, string user, string pw)
        {
            throw new Exception("Oracle DbParameters not implemented yet!");
        }

        public override List<TrackedTableInfo> GetTrackedTables(string server, string db, string user, string pw, string table)
        {
            throw new Exception("Oracle GetTrackedTables not implemented yet!");
        }
    }

    public class DbUtilsOra
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        #region Testing

        public static void Test()
        {
            //string db = "XE";
            //string user = "netmeter";
            //string pw = "netmeter";
            //OraCallStoredProc(db, user, pw, "rjp_proc");
            TestOracleStuff();
        }

        // Oracle test procContext
        //CREATE OR REPLACE procedure rjp_proc(
        //my_in_1 IN varchar2,
        //my_out_2 out varchar2,
        //my_in_3 in number,
        //my_out_4 out number, 
        //my_in_5 in DATE,
        //my_out_6 out DATE,
        //my_inout_7 in out varchar2
        //)
        //authid current_user 
        //as
        //my_sql varchar2(4000);
        //begin
        //my_out_2:= my_in_1;
        //my_out_4:= my_in_3;
        //my_out_6:= my_in_5;
        //select 'updated'|| my_inout_7 into my_inout_7 from dual;
        //end;

        public static void TestOracleStuff()
        {
            string db = "XE";
            string user = "netmeter";
            string pw = "netmeter";
            string proc = "rjp_proc";
            string connString = @"server = " + db + ";uid = " + user + ";password = " + pw + ";";
            OracleConnection conn = new OracleConnection(connString);
            try
            {
                conn.Open();
                new OracleCommand("alter session set nls_date_format = 'YYYYMMDDHH24MISS'", conn).ExecuteNonQuery();
                logger.Info("\tConnected String: {0}", conn.ConnectionString);
                using (OracleCommand stmt = new OracleCommand(proc, conn))
                {
                    logger.Info("running=" + proc);
                    stmt.CommandType = CommandType.StoredProcedure;
                    stmt.Parameters.Add("my_out_2", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                    stmt.Parameters.Add("my_in_1", OracleDbType.Varchar2).Value = 20;
                    stmt.Parameters.Add("my_in_3", OracleDbType.Varchar2).Value = 29;
                    stmt.Parameters.Add("my_out_4", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                    stmt.Parameters.Add("my_in_5", OracleDbType.Varchar2).Value = "20090101000000";
                    stmt.Parameters.Add("my_out_6", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                    stmt.Parameters.Add("my_inout_7", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.InputOutput;
                    stmt.Parameters["my_inout_7"].Value = "goingin";
                    int numRows = stmt.ExecuteNonQuery();
                    logger.Info("numrows=" + numRows);
                    logger.Info("my_out_2=" + stmt.Parameters["my_out_2"].Value);
                    logger.Info("my_out_4=" + stmt.Parameters["my_out_4"].Value);
                    logger.Info("my_out_6=" + stmt.Parameters["my_out_6"].Value);
                    logger.Info("my_inout_7=" + stmt.Parameters["my_inout_7"].Value);
                }
            }
            catch (OracleException e)
            {
                logger.Info("Error: " + e);
            }
            finally
            {
                conn.Close();
                logger.Info("Connection closed.");
            }
        }

        #endregion

        /**
         * Writes a sqlldr control file
         */
        public static void WriteOraSqlldrCtrl(string fileName, string tableName, string fieldDelim, string recordDelim, List<string> orderedFieldNames, string charset, Dictionary<string, string> dateFields)
        {
            TextWriter tw = new StreamWriter(fileName);
            string charsetString = "";
            if (charset.NotNullOrEmpty())
            {
                charsetString = "characterset " + charset;
            }
            string infile = " infile 'file' \"str '" + recordDelim + "'\"";
            if (recordDelim.Trim().Equals(""))
            {
                infile = "";
            }
            tw.WriteLine("load data " + charsetString + infile + " append into table " + tableName + " fields terminated by '" + fieldDelim + "' trailing nullcols (");
            for (int i = 0; i < orderedFieldNames.Count; i++)
            {
                string f = orderedFieldNames[i];
                tw.Write(f);
                if (dateFields.ContainsKey(f))
                {
                    tw.Write(" date \"" + dateFields[f] + "\"");
                }
                if (i < (orderedFieldNames.Count - 1)) tw.Write(",");
                tw.WriteLine();
            }
            tw.WriteLine(")");
            tw.Close();
        }

        /**
        * Writes a sqlldr control file
        */
        public static void WriteOraCtrl(string fileName, string fdel, string rdel, List<string> orderedFields)
        {
            WriteOraSqlldrCtrl(fileName, "TABLE", fdel, rdel, orderedFields, null, null);
        }

        static DbUtilsOra()
        {
            // Attempt to set oracle env vars. For some reason this doesn't work when mvm is lauched by ps_exec. 
            // If we get a conn with the wrong dateformat we'll alter the session so it is ok
            // if this doesn't work.
            // I tried a bunch of approaches to avoid doing the alter every time I get a pooling connection.
            //
            // 1) Connection string attribute 'Initialization String' does not appear to be supported, alteast in my odp.net
            // 2) OracleGlobalization.SetThreadInfo() does not appear to get inherited by connection/session so it does nothing for me.
            // 3) conn.SetSessionInfo(ogg) works but causes a db round trip so you want to avoid it.
            // 4) Call conn.GetSessionInfo() to see current settings before calling SetSessionInfo. This is where I ended
            //    up since GetSessionInfo does not appear to talk to the server. I sorta proved this by altering the session
            //    though an alter statement and the got the info using this method and saw that session info was now out of 
            //    sync with the server. Also, it is 50-100 times faster then SetSessionInfo. 
            // 5) when we feel like it we can work on getting the env vars right but I think this is fast enough.
            if (!SetEnv.SetEnvVar("NLS_DATE_FORMAT", "YYYYMMDDHH24MISS")) throw new Exception("Cannot set env var");
            if (!SetEnv.SetEnvVar("NLS_TIMESTAMP_FORMAT", "YYYYMMDDHH24MISS")) throw new Exception("Cannot set env var");
        }
        public static int OracleStatementCacheSize = 1000;

        public static OracleConnection GetOraConnection(string db, string user, string pw)
        {
            string connString = @"Pooling=true; Statement Cache Size=" + OracleStatementCacheSize + ";Data Source = " + db + ";User Id = " + user + ";Password = " + pw;//+ ";Initialization String=alter session set nls_date_format = 'YYYYMMDDHH24MISS'";
            OracleConnection conn = new OracleConnection(connString);
            try
            {
                conn.Open();
                //Stopwatch sw = new Stopwatch();
                //sw.Start();
                //for (int i = 0; i < 1000; i++)
                //{
                var ogg = conn.GetSessionInfo();
                bool setIt = false;
                if (!ogg.DateFormat.Equals("YYYYMMDDHH24MISS"))
                {
                    ogg.DateFormat = "YYYYMMDDHH24MISS";
                    setIt = true;
                }
                if (!ogg.TimeStampFormat.Equals("YYYYMMDDHH24MISS"))
                {
                    ogg.TimeStampFormat = "YYYYMMDDHH24MISS";
                    setIt = true;
                }
                if (setIt)
                {
                    //logger.Info("NEED TO ALTER SESSION FOR DATEFORMATS");
                    conn.SetSessionInfo(ogg);
                }
                //}
                //sw.Stop();
                //logger.Info("TIMEIS:" +sw.ElapsedMilliseconds);

                //new OracleCommand("alter session set nls_date_format = 'YYYYMMDDHH24MISS'", conn).ExecuteNonQuery();
                //new OracleCommand("alter session set nls_timestamp_format = 'YYYYMMDDHH24MISS'", conn).ExecuteNonQuery();
                return conn;
            }
            catch (Exception e)
            {
                throw new Exception("Error, cannot connect using connString=[" + connString + "], msg=" + e.Message, e);
            }
        }

        public static string ReadStringValue(OracleDataReader reader, string fieldName, int i, string nullValue)
        {
            string output = nullValue;
            var nativeFieldType = reader.GetFieldType(i);
            OracleType fieldType = MDbSelectOra.OracleGetTypeCase(nativeFieldType);
            try
            {
                if (reader.IsDBNull(i))
                {
                    output = nullValue;
                }
                else
                {
                    switch (fieldType)
                    {
                        case OracleType._string:
                            {
                                output = reader.GetString(i);
                                break;
                            }
                        case OracleType._datetime:
                            {
                                DateTime dt = (DateTime)reader.GetValue(i);
                                output = dt.ToString("yyyyMMddHHmmss");
                                break;
                            }
                        case OracleType._binary:
                            {
                                byte[] bytes = (byte[])reader.GetValue(i);
                                output = String.Join(String.Empty, Array.ConvertAll(bytes, x => x.ToString("X2"))); ;
                                break;
                            }
                        case OracleType._int32:
                            {
                                output = reader.GetInt32(i).ToString();
                                break;
                            }
                        case OracleType._int64:
                            {
                                output = reader.GetInt64(i).ToString();
                                break;
                            }
                        case OracleType._decimal:
                            {
                                output = reader.GetDecimal(i).StripFractionalTrailingZeros();
                                break;
                            }
                        case OracleType._double:
                            {
                                output = reader.GetDouble(i).StripFractionalTrailingZeros();
                                break;
                            }
                        default:
                            throw new Exception("Oracle ReadStringValue unhandled case");
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Cannot read oracle field " + fieldName, e);
            }
            return output;
        }
    }
}
