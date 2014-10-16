using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using NLog;

namespace MVM
{
    public class SqlTableInfo : TableInfo
    {
        public SqlTableInfo(StaticDbLoginInfo dbinfo, string table)
            : base(dbinfo, table)
        {
            List<ColumnInfo> columnInfo = new List<ColumnInfo>();
            using (SqlConnection conn = DbUtilsSql.GetSqlConnection(dbinfo.server, dbinfo.db, dbinfo.user, dbinfo.pw))
            {
                string tblQueryString =
                    "select user_name(schema_id) as name, is_tracked_by_cdc from sys.tables where upper(name) = '" + table.ToUpper() + "'";
                SqlCommand command;
                try
                {
                    command = new SqlCommand(tblQueryString, conn);
                    command.CommandTimeout = 0;
                }
                catch (Exception e)
                {
                    throw new Exception("Error running query string=[" + tblQueryString + "] using connString=[" + conn.ConnectionString + "], msg=" + e.Message, e);
                }
                int counter = 0;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        this.owner = reader.GetString(0);
                        this.changeTrackingEnabled = reader.GetBoolean(1);
                        counter++;
                    }
                }
                if (counter != 1)
                {
                    throw new Exception("Error with table=[" + table + "]. Query string=[" + tblQueryString + "] using connString=[" + conn.ConnectionString + "]");
                }
                reader.Close();

                string queryString =
                    @"SELECT so.NAME AS table_name, lower(substring(sc.NAME,1,LEN(sc.NAME))) AS column_name, sc.colorder as position, st.NAME AS datatype, sc.isnullable,(case when st.name in ('int','bigint','bit','decimal', 'float', 'numeric','real', 'money','smallint','smallmoney','tinyint') then 1 else 0 end) as is_numeric,
                    isc.numeric_scale, case when isc.data_type like '%char%' then isc.character_maximum_length else isc.numeric_precision end as length, isc.column_default
                    FROM syscolumns sc
                    right outer join sysobjects so on so.id=sc.id
                    join systypes st on st.xtype=sc.xtype and st.xusertype=sc.xusertype
                    join INFORMATION_SCHEMA.COLUMNS isc
                        on isc.table_name = so.name
                        and substring(isc.column_name,1,LEN(isc.column_name)) = substring(sc.NAME,1,LEN(sc.NAME))
                        and isc.table_schema = '" + this.owner + @"'
                        and isc.table_catalog = '" + dbinfo.db + @"'
                    WHERE upper(so.name) = '" + table.ToUpper() + @"'
                    ORDER BY table_name, position";
                try
                {
                    command = new SqlCommand(queryString, conn);
                    command.CommandTimeout = 0;
                }
                catch (Exception e)
                {
                    throw new Exception("Error running query string=[" + queryString + "] using connString=[" + conn.ConnectionString + "], msg=" + e.Message, e);
                }
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ColumnInfo info = new ColumnInfo();
                    if (!reader.IsDBNull(1))
                    {
                        info.name = reader.GetString(1);
                        info.position = Convert.ToInt32(reader.GetValue(2));
                        info.type = reader.GetString(3);
                        info.nullable = reader.GetInt32(4) == 1 ? true : false;
                        info.numeric = reader.GetInt32(5) == 1 ? true : false;
                        info.scale = reader.IsDBNull(6) ? -1 : reader.GetInt32(6);
                        info.length = reader.IsDBNull(7) ? -1 : reader.GetInt32(7);
                        info.defaultValue = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    }
                    columnInfo.Add(info);
                }
                reader.Close();
                if (columnInfo.Count == 0 || columnInfo[0].name == null)
                {
                    throw new Exception("Error, table=[" + table + "] doesn't exist. Query string=[" + queryString + "] using connString=[" + conn.ConnectionString + "]");
                }
            }
            this.SetColumnInfo(columnInfo);
        }

        public override void WriteLoaderCtrl(string fileName, string fieldDelim, string recordDelim, string charset, Dictionary<string, string> dateFields)
        {
            DbUtilsSql.WriteSqlBcpFormat(fileName, this.tableName, fieldDelim, recordDelim, this.columnNames);
        }

        public override List<string> GetPrimaryKeyColumns()
        {
            bool isPartitioned = DbUtilsSql.IsTablePartitioned(this.dbLoginInfo.server, this.dbLoginInfo.db, this.dbLoginInfo.user, this.dbLoginInfo.pw, tableName);
            string query;
            if (!isPartitioned)
            {
                query =
                    @"select c.COLUMN_NAME
                    from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk,
                    INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
                    where lower(pk.TABLE_NAME) = lower('" + tableName + @"')
                    and	CONSTRAINT_TYPE = 'PRIMARY KEY'
                    and	c.TABLE_NAME = pk.TABLE_NAME
                    and	c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME
                    order by c.ordinal_position asc";
            }
            else
            {
                query =
                    @"select c.COLUMN_NAME
                    from n_default.INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk,
                    n_default.INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
                    where lower(pk.TABLE_NAME) = lower('" + tableName + @"')
                    and	CONSTRAINT_TYPE = 'PRIMARY KEY'
                    and	c.TABLE_NAME = pk.TABLE_NAME
                    and	c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME
                    order by c.ordinal_position asc";
            }
            List<string> pkCols = new SqlQueryDispatcher().DbQueryToList(this.dbLoginInfo.server, this.dbLoginInfo.db, this.dbLoginInfo.user, this.dbLoginInfo.pw, query);
            return pkCols;
        }
    }

    public class SqlProcInfo : DbProcInfo
    {
        public SqlProcInfo(StaticDbLoginInfo dbinfo, string procName)
            : base(dbinfo, procName)
        {
            using (SqlConnection conn = DbUtilsSql.GetSqlConnection(dbinfo.server, dbinfo.db, dbinfo.user, dbinfo.pw))
            {
                string paramInfoQuery =
                    @"select so.name proc_name, sc.name arg_name, sc.colid arg_pos, sc.length arg_length, sc.isoutparam arg_is_out, st.name type
                    from syscolumns sc
                    right outer join sysobjects so on so.id=sc.id
                    right outer join systypes st on st.xtype=sc.xtype and st.xusertype=sc.xusertype AND st.xusertype!=256
                    where so.xtype = 'P' and so.name = 'SP_RJP_PROC2'
                    union
                    select so.name proc_name, sc.name arg_name, sc.colid arg_pos,sc.length arg_length, sc.isoutparam arg_is_out, st.name type
                    from master..syscolumns sc
                    right outer join systypes st on st.xtype=sc.xtype AND st.xusertype!=256
                    right outer join master..sysobjects so on so.id=sc.id
                    where so.xtype = 'P' and so.name = 'SP_RJP_PROC2' and so.category=2 and substring(so.name,0,4)='sp_'
                    and (select count(*) from syscolumns sc right outer join sysobjects so on so.id=sc.id where so.xtype = 'P' and so.name = 'SP_RJP_PROC2')=0
                    order by arg_pos;";

                paramInfoQuery = paramInfoQuery.Replace("SP_RJP_PROC2", procName);
                SqlCommand sqlcmd = new SqlCommand(paramInfoQuery, conn);
                SqlDataReader reader = sqlcmd.ExecuteReader();
                string returnedProcName = null;
                while (reader.Read())
                {
                    returnedProcName = reader.GetString(0);
                    string name = reader.GetString(1).Substring(1);
                    int position = System.Convert.ToInt32(reader.GetValue(2));
                    int size = System.Convert.ToInt32(reader.GetValue(3));
                    string mode = ((int)reader.GetValue(4)) == 1 ? "IN/OUT" : "IN";
                    string type = reader.GetString(5);
                    ParamInfo info = new ParamInfo(name, mode, type, position, size);
                    paramInfo.Add(info);
                }
                reader.Close();
                if (returnedProcName == null)
                {
                    throw new Exception("Stored proc [" + procName + "] cannot be accessed from [" + conn.ConnectionString + "]");
                }
            }
        }

        public override int CallProc(Dictionary<string, string> procParams)
        {
            List<string> orderedParamValues = new List<string>();
            using (SqlConnection conn = DbUtilsSql.GetSqlConnection(dbLoginInfo.server, dbLoginInfo.db, dbLoginInfo.user, dbLoginInfo.pw))
            {
                using (SqlCommand stmt = new SqlCommand(this.procName, conn))
                {
                    stmt.CommandType = CommandType.StoredProcedure;
                    stmt.CommandTimeout = 0;
                    foreach (ParamInfo p in paramInfo)
                    {
                        if (p.mode.Equals("IN"))
                        {
                            string paramValue = procParams.GetValueDefaulted(p.name, "");

                            SqlParameter param = stmt.Parameters.Add(p.paramName(), SqlDbType.NVarChar);
                            param.Direction = ParameterDirection.Input;
                            // treat '' as null for sql server
                            if (paramValue.Equals(""))
                            {
                                param.Value = System.Data.SqlTypes.SqlString.Null;
                                paramValue = "SQL_NULL";
                            }
                            else
                            {
                                param.Value = paramValue;
                            }
                            orderedParamValues.Add(paramValue);
                        }
                        else if (p.mode.Equals("IN/OUT"))
                        {
                            SqlParameter param;
                            // instanciate the param setting correct size if needed
                            if (p.type.Equals("datetime"))
                            {
                                param = stmt.Parameters.Add(p.paramName(), SqlDbType.DateTime, p.size);
                            }
                            else if (p.type.Equals("int"))
                            {
                                param = stmt.Parameters.Add(p.paramName(), SqlDbType.Int, p.size);
                            }
                            else
                            {
                                param = stmt.Parameters.Add(p.paramName(), SqlDbType.NVarChar, p.size);
                            }
                            // read the param value that was passed, and set it to sql null if ''
                            string paramValue = procParams.GetValueDefaulted(p.name, "");
                            if (paramValue.Equals(""))
                            {
                                param.Value = System.Data.SqlTypes.SqlString.Null;
                                paramValue = "SQL_NULL";
                            }
                            else
                            {
                                param.Value = paramValue;
                            }
                            param.Direction = ParameterDirection.InputOutput;
                            orderedParamValues.Add(paramValue);
                        }
                        else
                        {
                            throw new Exception("Unexpected stored proc param mode [" + p.mode + "]");
                        }
                    }
                    try
                    {
                        int numRows = stmt.ExecuteNonQuery();
                        foreach (var p in paramInfo.Where(pi => pi.mode.Contains("OUT")))
                        {
                            if (stmt.Parameters[p.name].DbType.Equals(DbType.DateTime))
                            {
                                System.Data.SqlTypes.SqlDateTime sqlDt = (System.Data.SqlTypes.SqlDateTime)stmt.Parameters[p.name].Value;
                                DateTime dt = DateTime.Parse(sqlDt.ToString());
                                var outValue = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");//2009-10-31 00:00:00.000
                                procParams[p.name] = outValue;
                            }
                            else
                            {
                                procParams[p.name] = System.Convert.ToString(stmt.Parameters[p.name].Value);
                            }
                        }
                        return numRows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        var orderedParamNames = paramInfo.Select(p => p.name);
                        string msg = "Error calling proc: " + this.procName + " (" + orderedParamValues.Join(",") + ")." +
                            "Ordered param names:" + orderedParamNames.JoinStrings(",") +
                            "Sql Message=[" + e.Message + "]\n" +
                            "Sql State=[" + e.State + "]\n" +
                            "Sql Severity=[" + e.Class + "]\n" +
                            "Sql LineNumber number=[" + e.LineNumber + "]\n" +
                            "Sql Procedure=[" + e.Procedure + "]\n" +
                            "Sql Source=[" + e.Source + "]\n" +
                            "Sql StackTrace=[" + e.StackTrace + "]\n" +
                            "Sql Server=[" + e.Server + "]\n" +
                            "Sql Database=[" + conn.Database + "]\n" +
                            "Sql Number=[" + e.Number + "]\n" +
                            "Sql HelpLink=[" + e.HelpLink + "]\n";
                        throw new Exception(msg, e);
                    }
                }
            }
        }
    }

    public class SqlQueryDispatcher : DbQueryDispatcher
    {
        /**
         * Runs query and dumps result to a file using BCP
         * 
         * Need to do some hoop jumping to get the format the way we want.
         * 
         * SELECT ISNULL(CONVERT(VARCHAR(23), my_datetime, 121),'') my_datetime FROM rjp
         * SELECT (CASE when my_integer is NULL THEN '' ELSE CAST(my_integer AS VARCHAR(8000))END) my_integer  FROM rjp
         * SELECT (CASE when my_decimal is NULL THEN '' ELSE CAST(my_decimal AS VARCHAR(8000))END) my_decimal  FROM rjp
         * 
         */
        public override void QueryToFile(string server, string db, string user, string pw, string query, string order, string file, string ctrl, string fdel, string rdel)
        {
            string origFdel = fdel;
            string origRdel = rdel;
            fdel = fdel.InterpolateEscapesReverse();
            // BCP BUG: it turns \n into \r\n. so if i get \r\n, replace it with \n to do the right thing
            rdel = rdel.Replace("\r\n", "\n");
            rdel = rdel.InterpolateEscapesReverse();
            // get the db connection
            // hardcoded this connection to master
            string connString = "Server=" + server + ";Trusted_Connection=no;Database=" + db + ";Uid=" + user + ";Pwd=" + pw;
            SqlConnection conn;
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();
            }
            catch (Exception e)
            {
                throw new Exception("Error, Cannot connect to db server using [" + connString + "]", e);
            }

            // create view as query
            string guid = System.Guid.NewGuid().ToString().Replace("-", "");
            string viewName = "MVM1_" + guid;
            string createView = "create view " + viewName + " as " + query;
            try
            {
                new SqlCommand(createView, conn).ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("Error, cannot create view do query to file sql=[" + createView + "] connection=[" + connString + "]", e);
            }

            // select format of view
            List<string> orderedFieldNames = new List<string>();
            List<string> convertedFields = new List<string>();
            string inspectView = "select column_name,data_type from INFORMATION_SCHEMA.columns where table_name='" + viewName + "' order by ordinal_position";
            try
            {
                using (SqlDataReader r = new SqlCommand(inspectView, conn).ExecuteReader())
                {
                    while (r.Read())
                    {
                        string columnName = Convert.ToString(r["column_name"]);
                        string dataType = Convert.ToString(r["data_type"]);
                        orderedFieldNames.Add(columnName.ToLower());
                        string convertedColumnName = columnName.ToLower();

                        // This makes mmddyyhh24miss
                        //if (dataType.Equals("datetime"))convertedColumnName = "replace(convert(char(10)," + columnName + ",111),'/','')+replace(convert(char(10)," + columnName + ",108),':','') " + convertedColumnName;

                        // NONE OF THIS ENDED UP BEING NECESSARY
                        //if (dataType.StartsWith("date")) convertedColumnName = "ISNULL(CONVERT(VARCHAR(23), " + columnName + ", 121),'') " + convertedColumnName;
                        //else if (dataType.EndsWith("varchar")) convertedColumnName = "isnull(" + columnName + ",'') " + convertedColumnName;
                        //else if (dataType.StartsWith("int")) convertedColumnName = "(CASE when " + columnName + " is NULL THEN '' ELSE CAST(" + columnName + " AS VARCHAR(8000))END) " + convertedColumnName;
                        //else if (dataType.Equals("decimal")) convertedColumnName = "(CASE when " + columnName + " is NULL THEN '' ELSE CAST(" + columnName + " AS VARCHAR(8000))END) " + convertedColumnName;
                        //else throw new Exception("bcp out for datatype="+dataType+", not setup");
                        convertedFields.Add(convertedColumnName);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error, cannot inspect view to do query to file sql=[" + inspectView + "] connection=[" + connString + "]", e);
            }

            // create the to view
            string convertedViewName = "MVM2_" + guid;
            string convertedCreateView = "create view " + convertedViewName + " as select " + convertedFields.Join(",") + " from " + viewName;
            try
            {
                new SqlCommand(convertedCreateView, conn).ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("Error, cannot create view do query to file sql=[" + createView + "] connection=[" + connString + "]", e);
            }

            // we're done with the connection
            conn.Close();

            // print out the ora ctrl
            DbUtilsOra.WriteOraCtrl(ctrl, origFdel, origRdel, orderedFieldNames);

            // first write to a tempContext file, then strip ascii nulls and write to main file
            //string bcpOutFile = Path.GetTempFileName();
            string bcpOutFile = file;

            // run bcp
            if (order != null)
            {
                string fullConvertedViewName = db + ".." + convertedViewName;
                string q = "select * from " + fullConvertedViewName;
                if (order != null) q += " order by " + order;
                string cmd = "bcp.exe";
                string[] args = new string[] { q, "queryout", bcpOutFile, "-S", server, "-U", user, "-P", pw, "-c", "-t", fdel, "-r", rdel };
                string stdOut, stdErr;
                int exitCode;
                SystemCommand.RunCmd(cmd, args, out stdOut, out stdErr, out exitCode);
                if (exitCode != 0)
                {
                    StringBuilder msg = new StringBuilder();
                    msg.AppendLine("Error called external cmd that returned non-zero value:" + exitCode);
                    msg.AppendLine(cmd + " " + args.SurroundAll("\"", "\"").Join(" "));
                    msg.AppendLine("std err:");
                    msg.AppendLine(stdErr);
                    msg.AppendLine("std out:");
                    msg.AppendLine(stdOut);
                    throw new Exception(msg.ToString());
                }
            }
            else
            {
                // run bcp
                string fullConvertedViewName = db + ".." + convertedViewName;
                string cmd = "bcp.exe";
                string[] args = new string[] { fullConvertedViewName, "out", bcpOutFile, "-S", server, "-U", user, "-P", pw, "-c", "-t", fdel, "-r", rdel };
                string stdOut, stdErr;
                int exitCode;
                SystemCommand.RunCmd(cmd, args, out stdOut, out stdErr, out exitCode);
                if (exitCode != 0)
                {
                    StringBuilder msg = new StringBuilder();
                    msg.AppendLine("Error called external cmd that returned non-zero value:" + exitCode);
                    msg.AppendLine(cmd + " " + args.SurroundAll("\"", "\"").Join(" "));
                    msg.AppendLine("std err:");
                    msg.AppendLine(stdErr);
                    msg.AppendLine("std out:");
                    msg.AppendLine(stdOut);
                    throw new Exception(msg.ToString());
                }
            }

            // read and write the file out without ascii nulls
            //RecordReader rr = new RecordReader(bcpOutFile, rdel);
            //string line;
            //while (line=rr.ReadLine)
            //{
            //}
        }

        public override IEnumerable<Dictionary<string, string>> DbQueryToEnumerableDictionary(string server, string db, string user, string pw, string queryString)
        {
            IDbLoginInfo dbLoginInfo = new StaticDbLoginInfo(server, db, user, pw, "sql");
            List<ColumnInfo> columnInfo = new List<ColumnInfo>();
            using (SqlConnection conn = DbUtilsSql.GetSqlConnection(server, db, user, pw))
            {
                SqlCommand command;
                SqlDataReader reader;
                try
                {
                    command = new SqlCommand(queryString, conn);
                    command.CommandTimeout = 0;
                    reader = command.ExecuteReader();
                }
                catch (Exception e)
                {
                    throw new Exception("Error running query string=[" + queryString + "] using connString=[" + conn.ConnectionString + "], msg=" + e.Message, e);
                }
                while (reader.Read())
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string fieldName = reader.GetName(i).ToUpper();
                        dict.Add(fieldName, DbUtilsSql.ReadStringValue(reader, fieldName, i, null));
                    }
                    yield return dict;
                }
                reader.Close();
            }
        }

        public override List<Dictionary<string, string>> DbQueryToListOfDictionary(string server, string db, string user, string pw, string queryString)
        {
            List<Dictionary<string, string>> results = new List<Dictionary<string, string>>();
            IDbLoginInfo dbLoginInfo = new StaticDbLoginInfo(server, db, user, pw, "sql");
            List<ColumnInfo> columnInfo = new List<ColumnInfo>();
            using (SqlConnection conn = DbUtilsSql.GetSqlConnection(server, db, user, pw))
            {
                SqlCommand command;
                try
                {
                    command = new SqlCommand(queryString, conn);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Dictionary<string, string> dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string fieldName = reader.GetName(i).ToUpper();
                            dict.Add(fieldName, DbUtilsSql.ReadStringValue(reader, fieldName, i, null));
                        }
                        results.Add(dict);
                    }
                    reader.Close();
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
            IDbLoginInfo dbLoginInfo = new StaticDbLoginInfo(server, db, user, pw, "sql");
            List<ColumnInfo> columnInfo = new List<ColumnInfo>();
            //logger.Debug("connecting with String: {0}", connString);
            using (SqlConnection conn = DbUtilsSql.GetSqlConnection(server, db, user, pw))
            {
                SqlCommand command;
                try
                {
                    command = new SqlCommand(queryString, conn);
                    command.CommandTimeout = 0;
                }
                catch (Exception e)
                {
                    throw new Exception("Error running query string=[" + queryString + "] using connString=[" + conn.ConnectionString + "], msg=" + e.Message, e);
                }
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(reader.GetString(0));
                }
                reader.Close();
            }
            return results;
        }

        public override int DbExecute(string server, string db, string user, string pw, string stmtString)
        {
            using (SqlConnection conn = DbUtilsSql.GetSqlConnection(server, db, user, pw))
            {
                using (SqlCommand stmt = new SqlCommand(stmtString, conn))
                {
                    stmt.CommandType = CommandType.Text;
                    stmt.CommandTimeout = 0;
                    try
                    {
                        int numRows = stmt.ExecuteNonQuery();
                        return numRows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        string msg = "Error calling statement: " + stmtString +
                            "Sql Message=[" + e.Message + "]\n" +
                            "Sql State=[" + e.State + "]\n" +
                            "Sql Severity=[" + e.Class + "]\n" +
                            "Sql LineNumber number=[" + e.LineNumber + "]\n" +
                            "Sql Procedure=[" + e.Procedure + "]\n" +
                            "Sql Source=[" + e.Source + "]\n" +
                            "Sql StackTrace=[" + e.StackTrace + "]\n" +
                            "Sql Server=[" + e.Server + "]\n" +
                            "Sql Database=[" + conn.Database + "]\n" +
                            "Sql Number=[" + e.Number + "]\n" +
                            "Sql HelpLink=[" + e.HelpLink + "]\n";
                        throw new Exception(msg, e);
                    }
                }
            }
        }

        public override void DumpCtrl(string server, string db, string user, string pw, string table, string ctrl, string fdel, string rdel)
        {
            throw new Exception("DumpCtrl not implemented for Sql Server");
        }

        public override Dictionary<string, string> DbParameters(string server, string db, string user, string pw)
        {
            string query =
                @"select is_cdc_enabled from sys.databases
                where lower(name) = '" + db + "'";
            List<Dictionary<string, string>> results = this.DbQueryToListOfDictionary(server, db, user, pw, query);
            if (results.Count != 1)
            {
                throw new Exception("Error fetching database parameters!");
            }
            return results[0];
        }

        private Regex splitRegex = new Regex(@"\]\s*,\s*\[");
        public override List<TrackedTableInfo> GetTrackedTables(string server, string db, string user, string pw, string table)
        {
            List<TrackedTableInfo> results = new List<TrackedTableInfo>();
            string query = "sys.sp_cdc_help_change_data_capture";
            var rows = DbQueryToListOfDictionary(server, db, user, pw, query);
            foreach (var row in rows)
            {
                if (table == null || row["SOURCE_TABLE"].EqualsIgnoreCase(table))
                {
                    TrackedTableInfo tblInfo = new TrackedTableInfo(row["SOURCE_SCHEMA"], row["SOURCE_TABLE"], row["CAPTURE_INSTANCE"], row["START_LSN"]);
                    tblInfo.columns = splitRegex.Split(row["CAPTURED_COLUMN_LIST"].Trim().TrimStart('[').TrimEnd(']')).ToList();
                    results.Add(tblInfo);
                }
            }
            return results;
        }
    }

    public class SqlTrackedTable : TrackedTable
    {
        public MvmEngine mvm;
        public string instance;
        public string minTransId;
        public string maxTransId;
        public SqlTrackedTable(
            MvmEngine mvm,
            string table,
            Dictionary<string, List<ChangeTrackingInfo>> rules,
            ManualResetEvent done,
            string min,
            string max,
            StaticDbLoginInfo db,
            TrackedTableInfo tti)
            : base(table, rules, done, db, tti)
        {
            this.mvm = mvm;
            this.minTransId = min;
            this.maxTransId = max;
            this.instance = tti.instance;
        }

        public override IEnumerable<IObjectData> PollTable(Object table)
        {
            string funcName = "cdc.fn_cdc_get_all_changes_" + instance;
            string query =
                "select * from " + funcName +
                "(convert(binary, '" + minTransId + "', 2), convert(binary, '" + maxTransId + "', 2), N'all update old') " +
                "order by __$start_lsn, __$seqval, __$operation";

            // Values for __$operation: 1=delete, 2=insert, 3=pre-update, 4=post-update
            Dictionary<string, string> LastPreUpdateRow = null;
            foreach (var row in new SqlQueryDispatcher().DbQueryToEnumerableDictionary(dbInfo.server, dbInfo.db, dbInfo.user, dbInfo.pw, query))
            {
                bool skip = false;
                string operation = "";
                ObjectDataFormattedDelta obj = null;
                switch (row["__$OPERATION"])
                {
                    case "1":
                        operation = "D";
                        obj = mvm.objectCache.CreateAndGetObjectDataFormattedDelta("CDC", instance);
                        obj.deltaState = ObjectDataFormattedDelta.DeltaState.SettingPersisted;
                        SetObjectFields(obj, row, null);
                        break;
                    case "2":
                        operation = "I";
                        obj = mvm.objectCache.CreateAndGetObjectDataFormattedDelta("CDC", instance);
                        obj.deltaState = ObjectDataFormattedDelta.DeltaState.SettingNew;
                        SetObjectFields(obj, row, null);
                        break;
                    case "3":
                        skip = true;
                        LastPreUpdateRow = row;
                        break;
                    case "4":
                        operation = "U";
                        string fieldBitmask = row["__$UPDATE_MASK"];
                        Dictionary<string, bool> updateFields = GetUpdateFields(fieldBitmask, this.ttInfo);
                        obj = mvm.objectCache.CreateAndGetObjectDataFormattedDelta("CDC", instance);
                        obj.deltaState = ObjectDataFormattedDelta.DeltaState.SettingPersisted;
                        SetObjectFields(obj, row, updateFields);
                        obj.deltaState = ObjectDataFormattedDelta.DeltaState.SettingNew;
                        SetObjectFields(obj, LastPreUpdateRow, updateFields);
                        LastPreUpdateRow = null;
                        break;
                    default:
                        throw new Exception("Unexpected Sql CDC operation: " + row["__$OPERATION"]);
                }
                if (skip)
                {
                    continue;
                }

                obj["__operation"] = operation;
                yield return obj;

                // phil fixme remove all this
                // Now that the delta object is built, call the routing proc(s) on it
                //foreach (var rp in trackingRules)
                //{
                //    string[] routingProc = rp.Key.Split('.');
                //    if (routingProc.Length != 2)
                //    {
                //        throw new Exception("Routing proc must be qualified with a namespace");
                //    }
                //    int procId = mvm.workMgr.schedulerMaster.GetProcId(routingProc[0], routingProc[1]);
                //    //ProcInst.CallProcForObjectId(mvm.workMgr.schedulerMaster.GetScheduler(), procId, obj.objectId);
                //    //mvm.CallProc(routingProc[1], routingProc[0]);

                //    ModuleContext mc = new ModuleContext();
                //    mc.CallProcForObject(procId, obj.objectId);

                //    // FIXME: send this object to those nodes, right now just doing current node

                //    // Loop through the associated integration procs
                //    foreach (var cti in rp.Value)
                //    {
                //        obj[cti.operationField] = operation;
                //        obj[cti.tableField] = this.tableName;
                //        Dictionary<string, bool> typesDone = new Dictionary<string, bool>();
                //        foreach (var oip in cti.overrideIntegrationProcs)
                //        {
                //            string[] integrationProc = oip.Value.Split('.');
                //            if (oip.Key.Equals(operation))
                //            {
                //                IIndex i = (IIndex)mvm.globalContext.GetNamedClassInst("TEMP_INDEX1");
                //                List<string> fv = new List<string>();
                //                fv.Add(operation);
                //                fv.Add(row["A"]);
                //                fv.Add(row["B"]);
                //                fv.Add(row["C"]);
                //                i.IndexInsert(new ModuleContext(), fv);
                //                typesDone[operation] = true;
                //            }
                //        }
                //        foreach (var ip in cti.integrationProcs)
                //        {
                //            if (!typesDone.ContainsKey(operation))
                //            {
                //                string[] integrationProc = ip.Split('.');
                //                IIndex i = (IIndex)mvm.globalContext.GetNamedClassInst("TEMP_INDEX1");
                //                List<string> fv = new List<string>();
                //                fv.Add(operation);
                //                fv.Add(row["A"]);
                //                fv.Add(row["B"]);
                //                fv.Add(row["C"]);
                //                i.IndexInsert(new ModuleContext(), fv);
                //            }
                //        }
                //    }
                //}
            }

            this.doneEvent.Set();
        }

        public void SetObjectFields(ObjectDataFormattedDelta obj, Dictionary<string, string> row, Dictionary<string, bool> fields)
        {
            foreach (var field in row)
            {
                if ((!field.Key.StartsWith("__$")) && (fields == null || fields.ContainsKey(field.Key)))
                {
                    obj[field.Key.ToLower()] = field.Value;
                }
            }
        }

        public Dictionary<string, bool> GetUpdateFields(string bitmask, TrackedTableInfo tti)
        {
            Dictionary<string, bool> fields = new Dictionary<string, bool>();
            byte[] mask = MvmClusterCommon.StringToByteArray(bitmask);

            for (int i = 0; i < tti.columns.Count; i++)
            {
                if ((mask[i/8] & (1 << (i/8))) > 0)
                {
                    fields.Add(tti.columns[i], true);
                }
            }

            return fields;
        }
    }

    public class DbUtilsSql
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public static bool IsTablePartitioned(string server, string db, string user, string pw, string tableName)
        {
            Dictionary<string, string> procParams = new Dictionary<string, string>();
            procParams["p_table"] = tableName;
            DbProcInfo.CallProc("sql", server, db, user, pw, "mvm_is_partitioned", procParams);
            if (procParams["p_is_partitioned"].Equals("1")) return true;
            return false;
        }

        public static void SqlDumpCtrl(string db, string user, string pw, string table, string ctrl, string fdel)
        {
            throw new Exception("not implemented");
        }

        /**
         * Generates a bcp format file
         */
        public static void WriteSqlBcpFormat(string fileName, string tableName, string fieldDelim, string recordDelim, List<string> orderedFieldNames)
        {
            string fd = fieldDelim.InterpolateEscapesReverse();
            string rd = recordDelim.InterpolateEscapesReverse();

            TextWriter tw = new StreamWriter(fileName);
            tw.WriteLine("9.0");
            tw.WriteLine(orderedFieldNames.Count);
            for (int i = 0; i < orderedFieldNames.Count; i++)
            {
                int fieldNo = i + 1;
                string fieldName = orderedFieldNames[i];
                if (i < (orderedFieldNames.Count - 1))
                {
                    //1       SQLCHAR       0       0      ","      1     id_acc                   ""
                    tw.WriteLine(fieldNo + "\tSQLCHAR\t0\t0\t" + fd.qq() + "\t" + fieldNo + "\t" + fieldName + "\t" + "".qq());
                }
                else
                {
                    //1       SQLCHAR       0       0      "\r\n"      1     id_acc                   ""
                    tw.WriteLine(fieldNo + "\tSQLCHAR\t0\t0\t" + rd
                        .qq() + "\t" + fieldNo + "\t" + fieldName + "\t" + "".qq());
                }
            }
            tw.Close();
        }

        /// <summary>
        /// Gets a sql server connection. If pw is null it assumes trusted connection.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="db"></param>
        /// <param name="user"></param>
        /// <param name="pw"></param>
        /// <returns></returns>
        public static SqlConnection GetSqlConnection(string server, string db, string user, string pw)
        {
            string connString;
            if (pw == null || pw.Equals(""))
            {
                connString = @"Pooling=true;Trusted_Connection=yes;Database=" + db;
            }
            else
            {
                connString = @"Pooling=true;Server=" + server + ";Trusted_Connection=no;Database=" + db + ";Uid=" + user + ";Pwd=" + pw + ";";
            }
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                conn.Open();
                return conn;
            }
            catch (Exception e)
            {
                throw new Exception("Error, cannot connect using connString=[" + connString + "], msg=" + e.Message, e);
            }
        }

        public static string ReadStringValue(SqlDataReader reader, string fieldName, int i, string nullValue)
        {
            string output = nullValue;
            var fieldType = reader.GetFieldType(i);
            try
            {
                if (reader.IsDBNull(i))
                {
                    output = nullValue;
                }
                else if (fieldType == typeof(System.DateTime))
                {
                    DateTime dt = (DateTime)reader.GetValue(i);
                    output = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                else if (fieldType == typeof(System.Byte[]))
                {
                    byte[] bytes = (byte[])reader.GetValue(i);
                    output = String.Join(String.Empty, Array.ConvertAll(bytes, x => x.ToString("X2")));
                }
                else if (fieldType == typeof(System.Decimal))
                {
                    Decimal decimalValue = (Decimal)reader.GetValue(i);
                    output = decimalValue.StripFractionalTrailingZeros();
                }
                else if (fieldType == typeof(System.Double))
                {
                    Double doubleValue = (Double)reader.GetValue(i);
                    output = doubleValue.StripFractionalTrailingZeros();
                }
                else
                {
                    output = System.Convert.ToString(reader.GetValue(i));
                }
            }
            catch (Exception e)
            {
                throw new Exception("Cannot read sql field " + fieldName, e);
            }
            return output;
        }
    }
}
