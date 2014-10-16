using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using NLog;
namespace MVM
{
    public class OracleBulkUpdater:BulkUpdater
    {
        public OracleBulkUpdater(SchemaMaster schemaMaster, string server, string db, string user, string pw, string bulkTable, string targetTable, int bulkTableCommitSize, int formatId, string[] formatFields)
        :base(schemaMaster,"oracle",server,db,user,pw,bulkTable,targetTable,bulkTableCommitSize,formatId,formatFields){
            this.bcp = new OracleBulkLoader(server, db, user, pw, bulkTable, bulkTableCommitSize, this.orderedBulkTableFields.ToArray());
            this.bulkCommand = "update (select " + selects + " from " + targetTable + " old," + bulkTable + " new where " + wheres + " and new.format_id=" + formatId + ") set " + sets + ";";
        }
    }

    public class SqlBulkUpdater : BulkUpdater
    {
        public SqlBulkUpdater(SchemaMaster schemaMaster, string server, string db, string user, string pw, string bulkTable, string targetTable, int bulkTableCommitSize, int formatId, string[] formatFields)
            : base(schemaMaster, "sql", server, db, user, pw, bulkTable, targetTable, bulkTableCommitSize, formatId, formatFields)
        {
            this.bcp = new SqlBulkLoader(server, db, user, pw, bulkTable, bulkTableCommitSize, this.orderedBulkTableFields.ToArray());
            this.bulkCommand = ";WITH CTE AS (select " + selects + " from " + targetTable + " old," + bulkTable + " new where " + wheres + " and new.format_id=" + formatId + ") UPDATE CTE set " + sets + ";";
        }
    }

    public class BulkUpdater
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public readonly string type;
        public readonly string server;
        public readonly string db;
        public readonly string user;
        public readonly string pw;
        public readonly SchemaMaster schemaMaster;
        public readonly string bulkTable;
        public readonly string targetTable;
        public readonly int bulkTableCommitSize;
        public readonly int formatId;
        public readonly TableInfo tableInfo;
        public IBulkLoader bcp { get; protected set; }
        public string bulkCommand { get; protected set; }
        public string wheres;
        public string sets;
        public string selects;
        public List<string> orderedOrigPkFields;
        public readonly List<string> orderedPrimaryKeyFields;
        public readonly string[] orderedUpdateFields;
        public List<string> orderedBulkTableFields;

        public BulkUpdater(SchemaMaster schemaMaster, string type,string server, string db, string user, string pw, string bulkTable, string targetTable, int bulkTableCommitSize, int formatId, string[] updateFields)
        {
            this.type=type;
            this.schemaMaster = schemaMaster;
            this.server = server;
            this.db = db;
            this.user = user;
            this.pw = pw;
            this.bulkTableCommitSize = bulkTableCommitSize;
            this.targetTable = targetTable;
            this.bulkTable = bulkTable;
            this.formatId = formatId;

            // these are the fields to be updated
            this.orderedUpdateFields = updateFields;
         
            this.tableInfo = this.schemaMaster.GetTableInfo(type, server, db, user, pw, targetTable);
            this.orderedPrimaryKeyFields = tableInfo.primaryKeyColumns;
            if (orderedPrimaryKeyFields.Count == 0) throw new Exception("Cannot bulk update [" + targetTable + "] because it has no primary key");

            // bulk update tables have formatId, orig pk fields, followed by update
            // these are the ordered columns in the bulk table
            this.orderedBulkTableFields = new List<string> { "format_id" };
            this.orderedOrigPkFields = new List<string>();
            this.orderedPrimaryKeyFields.SelectIndexValuePairs().ForEach(c => orderedOrigPkFields.Add("pk_" + c.index));
            orderedBulkTableFields.AddRange(orderedOrigPkFields);
            orderedBulkTableFields.AddRange(updateFields);
          
            
            this.wheres = orderedPrimaryKeyFields.SelectIndexValuePairs().Select(c => "old." + c.value + "=new.pk_" + c.index).JoinStrings(" and ");
            this.sets = updateFields.SelectIndexValuePairs().Select(p => "old_" + p.index + "=new_" + p.index).JoinStrings(",");
            this.selects = updateFields.SelectIndexValuePairs().Select(p => "old." + p.value + " old_" + p.index + "," + "new." + p.value + " new_" + p.index).JoinStrings(",");
        }

        /// <summary>
        /// Number of columns int he bulk table
        /// </summary>
        public int RowSize { get { return this.orderedBulkTableFields.Count; } }

        /// <summary>
        /// Inserts a bulk update row
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public int InsertRow(string[] row)
        {
            return this.bcp.InsertRow(row);
        }

        /// <summary>
        /// Flushes the underlying bulk update rows to the bulk update table
        /// </summary>
        /// <returns></returns>
        public int Flush()
        {
            return this.bcp.Flush();
        }

        /// <summary>
        /// Trash an uncommited bulk update rows
        /// </summary>
        /// <returns></returns>
        public int Abort()
        {
            return this.bcp.Abort();
        }

        /// <summary>
        /// Name of the table to be updated
        /// </summary>
        public string TargetTableName { get { return this.targetTable; } }

        /// <summary>
        /// Name of the temp bulk update table
        /// </summary>
        public string BulkTableName { get { return this.bulkTable; } }

    }
}
