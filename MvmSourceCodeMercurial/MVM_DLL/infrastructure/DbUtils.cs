using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
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
    public abstract class TableInfo
    {
        public static Dictionary<string, List<string>> updatableViewPkCols = new Dictionary<string, List<string>>();
        static TableInfo()
        {
            updatableViewPkCols["MVM_ADJUSTMENT_TRANSACTION"] = new List<string> { "adj_id_adj_trx" };
        }

        public static Logger logger = LogManager.GetCurrentClassLogger();
        public StaticDbLoginInfo dbLoginInfo;
        public string tableName;
        public string owner;
        public bool changeTrackingEnabled;
        public List<ColumnInfo> columnInfo;
        public List<string> columnNames = new List<string>();
        public List<string> sortedColumnNames = new List<string>();
        public Dictionary<string, ColumnInfo> columnInfoDictionary = new Dictionary<string, ColumnInfo>();
        private List<string> _primaryKeyColumns = null;

        public static TableInfo GetTableInfo(string type, string server, string db, string user, string pw, string tableName)
        {
            StaticDbLoginInfo dbLoginInfo = new StaticDbLoginInfo(server, db, user, pw, type);
            if (type.Equals("oracle"))
            {
                return new OraTableInfo(dbLoginInfo, tableName);
            }
            else if (type.Equals("sql"))
            {
                return new SqlTableInfo(dbLoginInfo, tableName);
            }
            else
            {
                throw new Exception("GetTableInfo: unknown db_type=[" + type + "]");
            }
        }

        public List<string> primaryKeyColumns
        {
            get
            {
                lock (this)
                {
                    if (_primaryKeyColumns != null) return this._primaryKeyColumns;
                    if (updatableViewPkCols.ContainsKey(tableName))
                    {
                        _primaryKeyColumns = updatableViewPkCols[tableName];
                        return this._primaryKeyColumns;
                    }
                    this._primaryKeyColumns = this.GetPrimaryKeyColumns();
                    return this._primaryKeyColumns;
                }
            }
        }

        public TableInfo(StaticDbLoginInfo dbLoginInfo, string tableName)
        {
            this.dbLoginInfo = dbLoginInfo;
            this.tableName = tableName;
        }

        public void SetColumnInfo(List<ColumnInfo> columnInfo)
        {
            this.columnInfo = columnInfo;
            foreach (var c in this.columnInfo)
            {
                this.columnNames.Add(c.name);
                this.columnInfoDictionary[c.name] = c;
            }
            this.sortedColumnNames = this.columnNames.OrderBy(x => x).ToList();
        }

        public void RemoveField(string fieldName)
        {
            ColumnInfo col = this.columnInfoDictionary[fieldName];
            this.columnInfo.Remove(col);
            this.columnNames.Remove(fieldName);
            this.sortedColumnNames.Remove(fieldName);
            this.columnInfoDictionary.Remove(fieldName);
            this._primaryKeyColumns = null;
        }

        public List<string> GetIncludedColumnList(StringDictionary includedCols, StringDictionary excludedCols)
        {
            List<string> cols = new List<string>();
            foreach (var col in this.columnInfo)
            {
                if ((includedCols.Count == 0 || includedCols.ContainsKey(col.name)) && !excludedCols.ContainsKey(col.name))
                {
                    cols.Add(col.name);
                }
            }
            return cols;
        }

        public abstract void WriteLoaderCtrl(string fileName, string fieldDelim, string recordDelim, string charset, Dictionary<string, string> dateFields);

        public abstract List<string> GetPrimaryKeyColumns();
    }

    public struct ColumnInfo
    {
        public String name;
        public String type;
        public int position;
        public int length;
        public int scale;
        public String defaultValue;
        public bool nullable;
        public bool numeric; // a lot of types are considered logically numeric so make this its own property.

        public ColumnInfo(String name, String type, int position, int length, int scale, string defaultValue, bool nullable, bool numeric)
        {
            this.name = name;
            this.type = type;
            this.position = position;
            this.length = length;
            this.scale = scale;
            this.defaultValue = defaultValue;
            this.nullable = nullable;
            this.numeric = numeric;
        }
    }

    public abstract class DbProcInfo
    {
        public string procName;
        public List<ParamInfo> paramInfo;
        public StaticDbLoginInfo dbLoginInfo;

        public static void CallProc(string type, string server, string db, string user, string pw, string procName, Dictionary<string, string> procParams)
        {
            DbProcInfo pi = GetProcInfo(type, server, db, user, pw, procName);
            pi.CallProc(procParams);
        }

        public static DbProcInfo GetProcInfo(string type, string server, string db, string user, string pw, string procName)
        {
            StaticDbLoginInfo dbLoginInfo = new StaticDbLoginInfo(server, db, user, pw, type);
            if (type.Equals("oracle"))
            {
                return new OraProcInfo(dbLoginInfo, procName);
            }
            else if (type.Equals("sql"))
            {
                return new SqlProcInfo(dbLoginInfo, procName);
            }
            else
            {
                throw new Exception("GetProcInfo: unknown db_type=[" + type + "]");
            }
        }

        public DbProcInfo(StaticDbLoginInfo dbLoginInfo, string procName)
        {
            this.procName = procName;
            this.dbLoginInfo = dbLoginInfo;
            this.paramInfo = new List<ParamInfo>();
        }

        public abstract int CallProc(Dictionary<string, string> procParams);
    }

    public struct ParamInfo
    {
        public String name;
        public String mode;
        public String type;
        public int position;
        public int size;
        public string nameLower
        {
            get { return name.ToLower(); }
        }

        public ParamInfo(String name, String mode, String type, int position, int size)
        {
            this.name = name;
            this.mode = mode;
            this.type = type;
            this.position = position;
            this.size = size;
        }
        public string paramName()
        {
            if (this.type.EqualsIgnoreCase("sql")) return "@" + this.name;
            return this.name;
        }
    }

    public class TrackedTableInfo
    {
        public string owner;
        public string table;
        public string instance;
        public string startTransId;
        public List<string> columns = new List<string>();
        public TrackedTableInfo(string owner, string table, string instance, string transId)
        {
            this.owner = owner;
            this.table = table;
            this.instance = instance;
            this.startTransId = transId;
        }
    }

    public class ChangeTrackingInfo
    {
    }

    public abstract class TrackedTable
    {
        public string tableName;
        public Dictionary<string, List<ChangeTrackingInfo>> trackingRules;
        public ManualResetEvent doneEvent;
        public StaticDbLoginInfo dbInfo;
        public TrackedTableInfo ttInfo;

        public static TrackedTable GetTrackedTable(
            MvmEngine mvm,
            string table,
            Dictionary<string, List<ChangeTrackingInfo>> rules,
            ManualResetEvent done,
            string min,
            string max,
            StaticDbLoginInfo db,
            TrackedTableInfo tti)
        {
            if (db.type.Equals("oracle"))
            {
                throw new Exception("OraTrackedTable not implemented yet");
                //return new OraTrackedTable(mvm, table, rules, done, min, max, db, instance);
            }
            else if (db.type.Equals("sql"))
            {
                return new SqlTrackedTable(mvm, table, rules, done, min, max, db, tti);
            }
            else
            {
                throw new Exception("GetProcInfo: unknown db_type=[" + db.type + "]");
            }
        }

        public TrackedTable(string table, Dictionary<string, List<ChangeTrackingInfo>> rules, ManualResetEvent done, StaticDbLoginInfo db, TrackedTableInfo tti)
        {
            this.tableName = table;
            this.trackingRules = rules;
            this.doneEvent = done;
            this.dbInfo = db;
            this.ttInfo = tti;
        }

        public void SetFields(ObjectDataFormattedDelta obj)
        {
        }

        public abstract IEnumerable<IObjectData> PollTable(Object table);
    }

    public abstract class DbQueryDispatcher
    {
        public static DbQueryDispatcher GetDbQueryDispatcher(string type)
        {
            if (type.Equals("oracle"))
            {
                return new OraQueryDispatcher();
            }
            else if (type.Equals("sql"))
            {
                return new SqlQueryDispatcher();
            }
            else
            {
                throw new Exception("Unknown db type=" + type);
            }
        }

        public abstract void QueryToFile(string server, string db, string user, string pw, string query, string order, string file, string ctrl, string fdel, string rdel);
        public abstract void DumpCtrl(string server, string db, string user, string pw, string table, string ctrl, string fdel, string rdel);
        public abstract IEnumerable<Dictionary<string, string>> DbQueryToEnumerableDictionary(string server, string db, string user, string pw, string queryString);
        public abstract List<Dictionary<string, string>> DbQueryToListOfDictionary(string server, string db, string user, string pw, string queryString);
        public abstract List<string> DbQueryToList(string server, string db, string user, string pw, string queryString);
        public abstract int DbExecute(string server, string db, string user, string pw, string stmtString);
        public abstract Dictionary<string, string> DbParameters(string server, string db, string user, string pw);
        public abstract List<TrackedTableInfo> GetTrackedTables(string server, string db, string user, string pw, string table);
    }

    public class DbUtils
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public static void QueryToFile(string type, string server, string db, string user, string pw, string query, string order, string file, string ctrl, string fdel, string rdel)
        {
            DbQueryDispatcher.GetDbQueryDispatcher(type).QueryToFile(server, db, user, pw, query, order, file, ctrl, fdel, rdel);
        }
        public static void DumpCtrl(ModuleContext mc, IDbLoginInfo dbInfo, string table, string ctrl, string fdel, string rdel)
        {
            DbQueryDispatcher.GetDbQueryDispatcher(dbInfo.GetType(mc)).DumpCtrl(dbInfo.GetServer(mc), dbInfo.GetDb(mc), dbInfo.GetUser(mc), dbInfo.GetPw(mc), table, ctrl, fdel, rdel);
        }
        public static IEnumerable<Dictionary<string, string>> DbQueryToEnumerableDictionary(string type, string server, string database, string user, string password, string queryString)
        {
            return DbQueryDispatcher.GetDbQueryDispatcher(type).DbQueryToEnumerableDictionary(server, database, user, password, queryString);
        }
        public static List<Dictionary<string, string>> DbQueryToListOfDictionary(string type, string server, string database, string user, string password, string queryString)
        {
            return DbQueryDispatcher.GetDbQueryDispatcher(type).DbQueryToListOfDictionary(server, database, user, password, queryString);
        }
        public static List<string> DbQueryToList(string type, string server, string database, string user, string password, string queryString)
        {
            return DbQueryDispatcher.GetDbQueryDispatcher(type).DbQueryToList(server, database, user, password, queryString);
        }
        public static List<string> DbQueryToList(ModuleContext mc, IDbLoginInfo dbInfo, string queryString)
        {
            return DbQueryToList(dbInfo.GetType(mc), dbInfo.GetServer(mc), dbInfo.GetDb(mc), dbInfo.GetUser(mc), dbInfo.GetPw(mc), queryString);
        }
        public static IEnumerable<Dictionary<string, string>> DbQueryToEnumerableDictionary(ModuleContext mc, IDbLoginInfo dbInfo, string queryString)
        {
            return DbQueryToEnumerableDictionary(dbInfo.GetType(mc), dbInfo.GetServer(mc), dbInfo.GetDb(mc), dbInfo.GetUser(mc), dbInfo.GetPw(mc), queryString);
        }
        public static List<Dictionary<string, string>> DbQueryToListOfDictionary(ModuleContext mc, IDbLoginInfo dbInfo, string queryString)
        {
            return DbQueryToListOfDictionary(dbInfo.GetType(mc), dbInfo.GetServer(mc), dbInfo.GetDb(mc), dbInfo.GetUser(mc), dbInfo.GetPw(mc), queryString);
        }
        public static int DbExecute(string type, string server, string db, string user, string pw, string stmtString)
        {
            return DbQueryDispatcher.GetDbQueryDispatcher(type).DbExecute(server, db, user, pw, stmtString);
        }
        public static int DbExecute(StaticDbLoginInfo staticDbLoginInfo, string stmtString)
        {
            return DbExecute(staticDbLoginInfo.type, staticDbLoginInfo.server, staticDbLoginInfo.db, staticDbLoginInfo.user, staticDbLoginInfo.pw, stmtString);
        }
        public static Dictionary<string, string> DbParameters(StaticDbLoginInfo staticDbLoginInfo)
        {
            return DbQueryDispatcher.GetDbQueryDispatcher(staticDbLoginInfo.type).DbParameters(staticDbLoginInfo.server, staticDbLoginInfo.db, staticDbLoginInfo.user, staticDbLoginInfo.pw);
        }
        public static List<TrackedTableInfo> GetTrackedTables(StaticDbLoginInfo staticDbLoginInfo)
        {
            return DbQueryDispatcher.GetDbQueryDispatcher(staticDbLoginInfo.type).GetTrackedTables(staticDbLoginInfo.server, staticDbLoginInfo.db, staticDbLoginInfo.user, staticDbLoginInfo.pw, null);
        }
        public static List<TrackedTableInfo> GetTrackedTables(StaticDbLoginInfo staticDbLoginInfo, string table)
        {
            return DbQueryDispatcher.GetDbQueryDispatcher(staticDbLoginInfo.type).GetTrackedTables(staticDbLoginInfo.server, staticDbLoginInfo.db, staticDbLoginInfo.user, staticDbLoginInfo.pw, table);
        }
    }

    public interface IDbLoginInfo
    {
        string GetServer(ModuleContext mc);
        string GetDb(ModuleContext mc);
        string GetUser(ModuleContext mc);
        string GetPw(ModuleContext mc);
        string GetType(ModuleContext mc);
        string GetExceptionProc(ModuleContext mc);
        string GetLogLevel(ModuleContext mc);
    }

    public class StaticDbLoginInfo : IDbLoginInfo
    {
        public string server;
        public string db;
        public string user;
        public string pw;
        public string type;
        public string exceptionProc;
        public string logLevel;
        public StaticDbLoginInfo()
        {
        }
        public StaticDbLoginInfo(string server, string db, string user, string pw, string type)
        {
            this.server = server;
            this.db = db;
            this.user = user;
            this.pw = pw;
            this.type = type;
        }
        public StaticDbLoginInfo(string server, string db, string user, string pw, string type, string exceptionProc)
        {
            this.server = server;
            this.db = db;
            this.user = user;
            this.pw = pw;
            this.type = type;
            this.exceptionProc = exceptionProc;
        }
        public StaticDbLoginInfo(string server, string db, string user, string pw, string type, string exceptionProc, string logLevel)
        {
            this.server = server;
            this.db = db;
            this.user = user;
            this.pw = pw;
            this.type = type;
            this.exceptionProc = exceptionProc;
            this.logLevel = logLevel;
        }
        public static StaticDbLoginInfo FromObjectId(ModuleContext mc, string oid)
        {
            StaticDbLoginInfo info = new StaticDbLoginInfo();
            info.server = mc.ReadObjectField(oid, "database_server");
            info.db = mc.ReadObjectField(oid, "database_name");
            info.user = mc.ReadObjectField(oid, "database_user");
            info.pw = mc.ReadObjectField(oid, "database_password");
            info.type = mc.ReadObjectField(oid, "database_type");
            info.exceptionProc = mc.ReadObjectField(oid, "database_exception_proc");
            info.logLevel = mc.ReadObjectField(oid, "database_log_level");
            return info;
        }
        public static StaticDbLoginInfo FromObjectId(MvmEngine mvm, string oid)
        {
            using (IObjectData objectData = mvm.objectCache.CheckOut(oid))
            {
                return FromObjectData(objectData);
            }
        }
        public static StaticDbLoginInfo FromObjectData(IObjectData objectData)
        {
            StaticDbLoginInfo info = new StaticDbLoginInfo();
            info.server = objectData["database_server"];
            info.db = objectData["database_name"];
            info.user = objectData["database_user"];
            info.pw = objectData["database_password"];
            info.type = objectData["database_type"];
            info.exceptionProc = objectData["database_exception_proc"];
            info.logLevel = objectData["database_log_level"];
            return info;
        }
        public string GetServer(ModuleContext mc)
        {
            return server;
        }
        public string GetDb(ModuleContext mc)
        {
            return db;
        }
        public string GetUser(ModuleContext mc)
        {
            return user;
        }
        public string GetPw(ModuleContext mc)
        {
            return pw;
        }
        public string GetType(ModuleContext mc)
        {
            return type;
        }
        public string GetExceptionProc(ModuleContext mc)
        {
            return exceptionProc;
        }
        public string GetLogLevel(ModuleContext mc)
        {
            return logLevel;
        }
    }

    public class DbInfo : IDbLoginInfo
    {
        private string serverSyntax;
        private string dbSyntax;
        private string userSyntax;
        private string pwSyntax;
        private string typeSyntax;
        private string exceptionProcSyntax;
        private string logLevelSyntax;
        private string loginObjectSyntax;
        private IReadString serverParsed;
        private IReadString dbParsed;
        private IReadString userParsed;
        private IReadString pwParsed;
        private IReadString typeParsed;
        private IReadString exceptionProcParsed;
        private IReadString logLevelParsed;
        private IReadString loginObjectParsed;

        // create db info from an object (should eventually be a login object but right now they are different)!
        public DbInfo(ModuleContext mc, string oid)
        {
            this.serverSyntax = mc.ReadObjectField(oid, "database_server");
            this.dbSyntax = mc.ReadObjectField(oid, "database_name");
            this.userSyntax = mc.ReadObjectField(oid, "database_user");
            this.pwSyntax = mc.ReadObjectField(oid, "database_password");
            this.typeSyntax = mc.ReadObjectField(oid, "database_type");
            this.exceptionProcSyntax = mc.ReadObjectField(oid, "database_exception_proc");
            this.logLevelSyntax = mc.ReadObjectField(oid, "database_log_level");
            this.loginObjectSyntax = mc.ReadObjectField(oid, "login_object");
            if (this.loginObjectSyntax != null && !this.loginObjectSyntax.Equals(""))
            {
                this.loginObjectParsed = mc.ParseSyntax(this.loginObjectSyntax);
                return;
            }
            if (this.serverSyntax == null || this.serverSyntax.Equals("")) this.serverSyntax = "(TEMP.database_server ne ''?TEMP.database_server:(OBJECT.database_server ne ''?OBJECT.database_server:GLOBAL.database_server))";
            if (this.dbSyntax == null || this.dbSyntax.Equals("")) this.dbSyntax = "(TEMP.database_name ne ''?TEMP.database_name:(OBJECT.database_name ne ''?OBJECT.database_name:GLOBAL.database_name))";
            if (this.userSyntax == null || this.userSyntax.Equals("")) this.userSyntax = "(TEMP.database_user ne ''?TEMP.database_user:(OBJECT.database_user ne ''?OBJECT.database_user:GLOBAL.database_user))";
            if (this.pwSyntax == null || this.pwSyntax.Equals("")) this.pwSyntax = "(TEMP.database_password ne ''?TEMP.database_password:(OBJECT.database_password ne ''?OBJECT.database_password:GLOBAL.database_password))";
            if (this.typeSyntax == null || this.typeSyntax.Equals("")) this.typeSyntax = "(TEMP.database_type ne ''?TEMP.database_type:(OBJECT.database_type ne ''?OBJECT.database_type:GLOBAL.database_type))";
            if (this.exceptionProcSyntax == null || this.exceptionProcSyntax.Equals("")) this.exceptionProcSyntax = "(TEMP.database_exception_proc ne ''?TEMP.database_exception_proc:(OBJECT.database_exception_proc ne ''?OBJECT.database_exception_proc:GLOBAL.database_exception_proc))";
            if (this.logLevelSyntax == null || this.logLevelSyntax.Equals("")) this.logLevelSyntax = "(TEMP.database_log_level ne ''?TEMP.database_log_level:(OBJECT.database_log_level ne ''?OBJECT.database_log_level:GLOBAL.database_log_level))";
            this.serverParsed = mc.ParseSyntax(this.serverSyntax);
            this.dbParsed = mc.ParseSyntax(this.dbSyntax);
            this.userParsed = mc.ParseSyntax(this.userSyntax);
            this.pwParsed = mc.ParseSyntax(this.pwSyntax);
            this.typeParsed = mc.ParseSyntax(this.typeSyntax);
            this.exceptionProcParsed = mc.ParseSyntax(this.exceptionProcSyntax);
            this.logLevelParsed = mc.ParseSyntax(this.logLevelSyntax);
        }
        public DbInfo(XmlElement me, ModuleContext mc)
        {
            this.serverSyntax = me.SelectNodeInnerText("./server");
            this.dbSyntax = me.SelectNodeInnerText("./db");
            this.userSyntax = me.SelectNodeInnerText("./user");
            this.pwSyntax = me.SelectNodeInnerText("./pw");
            this.typeSyntax = me.SelectNodeInnerText("./type");
            this.exceptionProcSyntax = me.SelectNodeInnerText("./exception_proc");
            this.logLevelSyntax = me.SelectNodeInnerText("./log_level");
            this.loginObjectSyntax = me.SelectNodeInnerText("./login_object", "GLOBAL.target_login");
            if (this.loginObjectSyntax != null)
            {
                this.loginObjectParsed = mc.ParseSyntax(this.loginObjectSyntax);
                return;
            }
            if (this.serverSyntax == null) this.serverSyntax = "(TEMP.database_server ne ''?TEMP.database_server:(OBJECT.database_server ne ''?OBJECT.database_server:GLOBAL.database_server))";
            if (this.dbSyntax == null) this.dbSyntax = "(TEMP.database_name ne ''?TEMP.database_name:(OBJECT.database_name ne ''?OBJECT.database_name:GLOBAL.database_name))";
            if (this.userSyntax == null) this.userSyntax = "(TEMP.database_user ne ''?TEMP.database_user:(OBJECT.database_user ne ''?OBJECT.database_user:GLOBAL.database_user))";
            if (this.pwSyntax == null) this.pwSyntax = "(TEMP.database_password ne ''?TEMP.database_password:(OBJECT.database_password ne ''?OBJECT.database_password:GLOBAL.database_password))";
            if (this.typeSyntax == null) this.typeSyntax = "(TEMP.database_type ne ''?TEMP.database_type:(OBJECT.database_type ne ''?OBJECT.database_type:GLOBAL.database_type))";
            if (this.exceptionProcSyntax == null) this.exceptionProcSyntax = "(TEMP.database_exception_proc ne ''?TEMP.database_exception_proc:(OBJECT.database_exception_proc ne ''?OBJECT.database_exception_proc:GLOBAL.database_exception_proc))";
            if (this.logLevelSyntax == null) this.logLevelSyntax = "(TEMP.database_log_level ne ''?TEMP.database_log_level:(OBJECT.database_log_level ne ''?OBJECT.database_log_level:GLOBAL.database_log_level))";
            this.serverParsed = mc.ParseSyntax(this.serverSyntax);
            this.dbParsed = mc.ParseSyntax(this.dbSyntax);
            this.userParsed = mc.ParseSyntax(this.userSyntax);
            this.pwParsed = mc.ParseSyntax(this.pwSyntax);
            this.typeParsed = mc.ParseSyntax(this.typeSyntax);
            this.exceptionProcParsed = mc.ParseSyntax(this.exceptionProcSyntax);
            this.logLevelParsed = mc.ParseSyntax(this.logLevelSyntax);
        }
        public string BackToXmlString()
        {
            StringBuilder sb = new StringBuilder();
            if (this.serverSyntax != null) sb.AppendLine("<server>" + this.serverSyntax + "</server>");
            if (this.dbSyntax != null) sb.AppendLine("<db>" + this.dbSyntax + "</db>");
            if (this.userSyntax != null) sb.AppendLine("<user>" + this.userSyntax + "</user>");
            if (this.pwSyntax != null) sb.AppendLine("<pw>" + this.pwSyntax + "</pw>");
            if (this.typeSyntax != null) sb.AppendLine("<type>" + this.typeSyntax + "</type>");
            if (this.exceptionProcSyntax != null) sb.AppendLine("<exception_proc>" + this.exceptionProcSyntax + "</exception_proc>");
            if (this.exceptionProcSyntax != null) sb.AppendLine("<log_level>" + this.exceptionProcSyntax + "</log_level>");
            if (this.loginObjectSyntax != null) sb.AppendLine("<login_object>" + this.loginObjectSyntax + "</login_object>");
            return sb.ToString();
        }
        public string GetServer(ModuleContext mc)
        {
            if (this.loginObjectParsed != null)
            {
                string oid = this.loginObjectParsed.Read(mc);
                return mc.ReadObjectField(oid, "database_server");
            }
            return this.serverParsed.Read(mc);
        }
        public string GetDb(ModuleContext mc)
        {
            if (this.loginObjectParsed != null)
            {
                string oid = this.loginObjectParsed.Read(mc);
                return mc.ReadObjectField(oid, "database_name");
            }
            return this.dbParsed.Read(mc);
        }
        public string GetUser(ModuleContext mc)
        {
            if (this.loginObjectParsed != null)
            {
                string oid = this.loginObjectParsed.Read(mc);
                return mc.ReadObjectField(oid, "database_user");
            }
            return this.userParsed.Read(mc);
        }
        public string GetPw(ModuleContext mc)
        {
            if (this.loginObjectParsed != null)
            {
                string oid = this.loginObjectParsed.Read(mc);
                return mc.ReadObjectField(oid, "database_password");
            }
            return this.pwParsed.Read(mc);
        }
        public string GetType(ModuleContext mc)
        {
            if (this.loginObjectParsed != null)
            {
                string oid = this.loginObjectParsed.Read(mc);
                return mc.ReadObjectField(oid, "database_type");
            }
            return this.typeParsed.Read(mc);
        }
        public string GetExceptionProc(ModuleContext mc)
        {
            if (this.loginObjectParsed != null)
            {
                string oid = this.loginObjectParsed.Read(mc);
                return mc.ReadObjectField(oid, "database_exception_proc");
            }
            return this.exceptionProcParsed.Read(mc);
        }
        public string GetLogLevel(ModuleContext mc)
        {
            if (this.loginObjectParsed != null)
            {
                string oid = this.loginObjectParsed.Read(mc);
                return mc.ReadObjectField(oid, "database_log_level");
            }
            return this.logLevelParsed.Read(mc);
        }
    }
}
