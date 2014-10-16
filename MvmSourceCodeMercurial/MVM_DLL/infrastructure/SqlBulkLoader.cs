using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using NLog;
namespace MVM
{
    public class SqlBulkLoader:IBulkLoader
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public string TableName { get { return this.tableName; } }   

        public readonly string connString;
        public readonly string tableName;
        public readonly DataTable dataTable;
        public readonly List<string> orderedFieldNames = new List<string>();
        public readonly List<int> binaries = new List<int> ();
        public int commitSize;
        public readonly string server;
        public readonly string db;
        public readonly string user;
        public readonly string pw;
        public readonly List<string> lowerTableColumnNames;
        public SqlBulkLoader(string server, string db, string user, string pw, string tableName, int commitSize, params string[] fieldNames)
        {
            this.server = server;
            this.db = db;
            this.user = user;
            this.pw = pw;
            this.commitSize = commitSize;
            this.tableName = tableName;
            this.lowerTableColumnNames=MvmEngine.DefaultMvmEngine.globalContext.schemaMaster.GetTableInfo("sql",server,db,user,pw,tableName).columnNames.Select(c => c.ToLower()).ToList();
            string columnNamesString = fieldNames.NotNullOrEmpty() ? fieldNames.JoinStrings(",") : "*";
            using (SqlConnection conn = DbUtilsSql.GetSqlConnection(server, db, user, pw))
            {
                using (SqlCommand formatQuery = new SqlCommand("SELECT " + columnNamesString + " FROM " + this.tableName + " where 1=2", conn))
                {
                    using (SqlDataReader formatReader = formatQuery.ExecuteReader())
                    {
                        this.dataTable = new DataTable();
                        this.dataTable.Load(formatReader);
                        this.dataTable.TableName = this.tableName;
                        int index = 0;
                        foreach ( DataColumn column in this.dataTable.Columns )
                        {
                          //logger.Debug("got " + column.ColumnName + " type " + column.DataType.ToString());
                          if ( column.DataType.ToString().Equals("System.Byte[]") )
                          {
                            this.binaries.Add ( index );
                          }
                          else
                          {
//                            column.DataType = typeof ( string );
                          }
                          column.AllowDBNull = true;
                          this.orderedFieldNames.Add ( column.ColumnName.ToLower () );
                          index++;
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Return table column names in their natural order
        /// </summary>
        /// <returns></returns>
        public List<string> GetOrderedFieldNames()
        {
            return this.orderedFieldNames;
        }
        /// <summary>
        /// Returns number of rows committed
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public int InsertRow(string[] row)
        {
            lock (this.dataTable)
            {
              object [] myrow = new object [ row.Length ];
              row.CopyTo ( myrow, 0 );
              foreach ( var index in binaries )
              {
                myrow [ index ] = OracleBulkLoader.StringToByteArray ( row [ index ] );
              }
              for ( int i = 0; i < myrow.Length; i++ )
              {
                if ( string.IsNullOrEmpty ( row [ i ] ) )
                {
                  myrow [ i ] =null ;
                }
              }

                this.dataTable.Rows.Add(myrow);
                if (this.dataTable.Rows.Count >= this.commitSize) 
                    return this.Flush();
            }
            return 0;
        }
        /// <summary>
        /// Returns number of rows committed
        /// </summary>
        /// <returns></returns>
        public int Flush()
        {
            int committing = this.dataTable.Rows.Count;
            if (committing > 0)
            {
                lock (this.dataTable)
                {
                    using (SqlConnection conn = DbUtilsSql.GetSqlConnection(server, db, user, pw))
                    {
                        try{
                        SqlBulkCopy bcp = new SqlBulkCopy(conn);
                        bcp.DestinationTableName = this.tableName;
                        int srcIdx=0;
                        foreach (DataColumn x in this.dataTable.Columns)
                        {
                            int tgtIdx = this.lowerTableColumnNames.IndexOf(x.ColumnName.ToLower());
                            bcp.ColumnMappings.Add(srcIdx++,tgtIdx);
                        }
                        bcp.WriteToServer(this.dataTable);
                        this.dataTable.Clear();
                        bcp.Close();
                        }
                        catch (Exception e)
                        {
                            string msg = "Error in SqlBulkLoader.Flush: table="+this.tableName+" using connString=[" + conn.ConnectionString + "] Error msg=" + e.Message;
                            logger.Fatal(msg);
                            List<string> dtc=new List<string>();
                            foreach(DataColumn x in this.dataTable.Columns){
                               dtc.Add( x.ColumnName);
                            }
                            logger.Fatal(dtc.JoinStrings("+"));
                            logger.Fatal(this.orderedFieldNames.JoinStrings("|"));
                            foreach (DataRow r in this.dataTable.Rows)
                            {
                                logger.Fatal(r.ItemArray.JoinStrings("|"));
                            }
                            throw new Exception(msg, e);
                        }
                    }
                }
            }
            return committing;
        }
        /// <summary>
        /// Discarded queued up rows and returns the number we aborted.
        /// </summary>
        /// <returns></returns>
        public int Abort()
        {
            int aborting = this.dataTable.Rows.Count;
            this.dataTable.Clear();
            return aborting;
        }
    }
}
