using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NLog;
namespace MVM
{
    public class SchemaMaster
    {
        public readonly GlobalContext globalContext;
        public SchemaMaster(GlobalContext globalContext){
            this.globalContext=globalContext;
        }
        public static Logger logger = LogManager.GetCurrentClassLogger();
        #region bulk update tables
        Dictionary<StringArray, string> bulkUpdateTables = new Dictionary<StringArray, string>();
        public string GetBulkUpdateTableName(string type, string server, string database, string user, string password, string table)
        {
            StringArray key = new StringArray(type, server, database, user, table);
            string updateTable;
            if (this.bulkUpdateTables.TryGetValue(key, out updateTable)) return updateTable;
            string pkColString = this.GetTableInfo(type,server,database,user,password,table).primaryKeyColumns.JoinStrings(",");
            Dictionary<string, string> procParams = new Dictionary<string, string>() { { "p_table", table }, { "p_prefix", "ampbu_" }, { "p_mvm_run_id", this.globalContext["mvm_run_id"] }, { "p_node_id", this.globalContext["node_id"] }, { "p_pk_col_string", pkColString } };
            DbProcInfo.CallProc(type, server, database, user, password, "mvm_create_blk_upd_table2", procParams);
            updateTable = procParams["p_blk_upd_table"];
            this.bulkUpdateTables[key] = updateTable;
            logger.Debug("Created bulk update table "+table+"="+updateTable);
            return updateTable;
        }
        #endregion

        #region bulk delete tables
        Dictionary<StringArray, string> bulkDeleteTables = new Dictionary<StringArray, string>();
        public string GetBulkDeleteTableName(string type, string server, string database, string user, string password, string table)
        {
            StringArray key = new StringArray(type, server, database, user, table);
            string deleteTable;
            if (this.bulkDeleteTables.TryGetValue(key, out deleteTable)) return deleteTable;
            Dictionary<string, string> procParams = new Dictionary<string, string>() { { "p_table", table }, { "p_prefix", "ampbd_" }, { "p_mvm_run_id", this.globalContext["mvm_run_id"] }, { "p_node_id", this.globalContext["node_id"] } };
            DbProcInfo.CallProc(type, server, database, user, password, "mvm_create_blk_del_table2", procParams);
            deleteTable = procParams["p_blk_del_table"];
            this.bulkDeleteTables[key] = deleteTable;
            logger.Debug("Created bulk delete table " + table + "=" + deleteTable);
            return deleteTable;
        }
        #endregion

        #region bulk insert tables
        Dictionary<StringArray, string> bulkInsertTables = new Dictionary<StringArray, string>();
        public string GetBulkInsertTableName(string type, string server, string database, string user, string password, string table)
        {
            StringArray key = new StringArray(type, server, database, user, table);
            string insertTable;
            if (this.bulkInsertTables.TryGetValue(key, out insertTable)) return insertTable;
            Dictionary<string, string> procParams = new Dictionary<string, string>() { { "p_table", table }, { "p_prefix", "ampbi_" }, { "p_mvm_run_id", this.globalContext["mvm_run_id"] }, { "p_node_id", this.globalContext["node_id"] } };
            DbProcInfo.CallProc(type, server, database, user, password, "mvm_create_blk_ins_table2", procParams);
            insertTable = procParams["p_blk_upd_table"];
            this.bulkInsertTables[key] = insertTable;
            logger.Debug("Created bulk insert table " + table + "=" + insertTable);

            return insertTable;
        }
        # endregion

        #region bulk upsert tables
        Dictionary<StringArray, string> bulkUpsertTables = new Dictionary<StringArray, string>();
        public string GetBulkUpsertTableName(string type, string server, string database, string user, string password, string table)
        {
            StringArray key = new StringArray(type, server, database, user, table);
            string upsertTable;
            if (this.bulkUpsertTables.TryGetValue(key, out upsertTable)) return upsertTable;
            Dictionary<string, string> procParams = new Dictionary<string, string>() { { "p_table", table }, { "p_prefix", "ampbups_" }, { "p_mvm_run_id", this.globalContext["mvm_run_id"] }, { "p_node_id", this.globalContext["node_id"] } };
            DbProcInfo.CallProc(type, server, database, user, password, "mvm_create_blk_ins_table2", procParams);
            upsertTable = procParams["p_blk_upd_table"];
            this.bulkUpsertTables[key] = upsertTable;
            logger.Debug("Created bulk upsert table " + table + "=" + upsertTable);

            return upsertTable;
        }
        # endregion


        #region bulk loaders
        Dictionary<StringArray, IBulkLoader> bulkLoaders = new Dictionary<StringArray, IBulkLoader>();
        public IBulkLoader GetBulkLoader(string type, string server, string database, string user, string password, string table, int commitSize)
        {
            lock (this.bulkLoaders)
            {
                StringArray key = new StringArray(type, server, database, user, table);
                if (this.bulkLoaders.ContainsKey(key)) return this.bulkLoaders[key];
                if (type.Equals("oracle"))
                {
                    OracleBulkLoader bcp = new OracleBulkLoader(server, database, user, password, table, commitSize);
                    this.bulkLoaders[key] = bcp;
                    return bcp;
                }
                else if (type.Equals("sql"))
                {
                    SqlBulkLoader bcp = new SqlBulkLoader(server, database, user, password, table, commitSize);
                    this.bulkLoaders[key] = bcp;
                    return bcp;
                }
                else
                {
                    throw new Exception("GetBulkLoader: Error, unknown db_type=[" + type + "]");
                }
            }
        }

        public IBulkLoader GetBulkLoader(ModuleContext mc, IDbLoginInfo dbInfo, string table, int commitSize)
        {
            return GetBulkLoader(dbInfo.GetType(mc), dbInfo.GetServer(mc), dbInfo.GetDb(mc), dbInfo.GetUser(mc), dbInfo.GetPw(mc), table, commitSize);
        }


        public IBulkLoader GetBulkLoaderFormatted(string type, string server, string database, string user, string password, string table, int commitSize,int formatId,string[] formatFields)
        {
            lock (this.bulkLoaders)
            {
                StringArray key = new StringArray(type, server, database, user, table,formatId.ToString());
                if (this.bulkLoaders.ContainsKey(key)) return this.bulkLoaders[key];
                if (type.Equals("oracle"))
                {
                    OracleBulkLoader bcp = new OracleBulkLoader(server, database, user, password, table, commitSize, formatFields);
                    this.bulkLoaders[key] = bcp;
                    return bcp;
                }
                else if (type.Equals("sql"))
                {
//                    throw new Exception("not done");
                    SqlBulkLoader bcp = new SqlBulkLoader(server, database, user, password, table, commitSize);
                    this.bulkLoaders[key] = bcp;
                    return bcp;
                }
                else
                {
                    throw new Exception("GetBulkLoader: Error, unknown db_type=[" + type + "]");
                }
            }
        }

        public IBulkLoader GetBulkLoaderFormatted(ModuleContext mc, IDbLoginInfo dbInfo, string table, int commitSize,int formatId, string[] formatFields)
        {
            return GetBulkLoaderFormatted(dbInfo.GetType(mc), dbInfo.GetServer(mc), dbInfo.GetDb(mc), dbInfo.GetUser(mc), dbInfo.GetPw(mc), table, commitSize, formatId, formatFields);
        }

        public List<IBulkLoader> GetAllBulkLoaders()
        {
            lock (this.bulkLoaders)
            {
                return this.bulkLoaders.Values.ToList();
            }
        }
        #endregion

        #region proc info
        Dictionary<StringArray, DbProcInfo> procInfo = new Dictionary<StringArray, DbProcInfo>();
        public DbProcInfo GetProcInfo(string type, string server, string database, string user, string password, string proc)
        {
            lock (procInfo)
            {
                StringArray key = new StringArray(type, server, database, user, proc);
                if (this.procInfo.ContainsKey(key))
                    return this.procInfo[key];
                DbProcInfo info = DbProcInfo.GetProcInfo(type, server, database, user, password, proc);
                this.procInfo[key] = info;
                return info;
            }
        }

        public DbProcInfo GetProcInfo(ModuleContext mc, IDbLoginInfo dbInfo, string proc)
        {
            return GetProcInfo(dbInfo.GetType(mc), dbInfo.GetServer(mc), dbInfo.GetDb(mc), dbInfo.GetUser(mc), dbInfo.GetPw(mc), proc);
        }

        #endregion

        #region table info

        Dictionary<StringArray, TableInfo> tableInfo = new Dictionary<StringArray, TableInfo>();
        public TableInfo GetTableInfo(string type, string server, string database, string user, string password, string table, Dictionary<string, bool> skippedFields = null, bool refresh = false)
        {
            if (skippedFields == null)
            {
                skippedFields = new Dictionary<string, bool>();
            }
            lock (tableInfo)
            {
                StringArray key = new StringArray(type, server, database, user, table.ToUpper(), string.Join("|", skippedFields.Keys));
                if (this.tableInfo.ContainsKey(key) && !refresh)
                    return this.tableInfo[key];
                TableInfo info = TableInfo.GetTableInfo(type, server, database, user, password, table);
                foreach (var field in skippedFields)
                {
                    info.RemoveField(field.Key);
                }
                this.tableInfo[key] = info;
                return info;
            }
        }

        public TableInfo GetTableInfo(ModuleContext mc, IDbLoginInfo dbInfo, string table, Dictionary<string, bool> skippedFields = null, bool refresh = false)
        {
            return GetTableInfo(dbInfo.GetType(mc), dbInfo.GetServer(mc), dbInfo.GetDb(mc), dbInfo.GetUser(mc), dbInfo.GetPw(mc), table, skippedFields);
        }

        public TableInfo GetTableInfo(StaticDbLoginInfo dbInfo, string table, Dictionary<string, bool> skippedFields = null, bool refresh = false)
        {
            return GetTableInfo(dbInfo.type, dbInfo.server, dbInfo.db, dbInfo.user, dbInfo.pw, table);
        }

        #endregion

    }
}
