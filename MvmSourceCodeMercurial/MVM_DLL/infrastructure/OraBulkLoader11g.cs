//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;
//using Oracle.DataAccess.Client;

//namespace MVM
//{
//    public class OracleBulkLoader11g:IBulkLoader
//    {
//        public readonly string connString;
//        public readonly string tableName;
//        public readonly DataTable dataTable;
//        public readonly List<string> orderedFieldNames = new List<string>();
//        public int commitSize;
//        public readonly string server;
//        public readonly string db;
//        public readonly string user;
//        public readonly string pw;

//        public static Type dateTimeType = new System.DateTime().GetType();
//        public List<int> dateCols = new List<int>();
//        public static Type byteArrayType = new Byte[1].GetType();
//        public List<int> byteArrayCols = new List<int>();
//        public List<int> otherCols = new List<int>();
//        public OracleBulkLoader11g(string server, string db, string user, string pw, string tableName, int commitSize)
//        {
//            this.server = server;
//            this.db = db;
//            this.user = user;
//            this.pw = pw;
//            this.commitSize = commitSize;
//            this.tableName = tableName;
//            using (OracleConnection conn = OracleBulkLoader11g.GetOraConnection(db,user,pw))
//            {
//                using (OracleCommand formatQuery = new OracleCommand("SELECT * FROM " + this.tableName + " where 1=2", conn))
//                {
//                    using (OracleDataReader formatReader = formatQuery.ExecuteReader())
//                    {
//                        this.dataTable = new DataTable();
//                        this.dataTable.Load(formatReader);
//                        this.dataTable.TableName = this.tableName;
//                        int colIdx = 0;
//                        foreach (DataColumn column in this.dataTable.Columns)
//                        {
//                            this.orderedFieldNames.Add(column.ColumnName.ToLower());
//                            if(column.DataType.Equals(dateTimeType)){
//                                dateCols.Add(colIdx);
//                            }
//                            else if (column.DataType.Equals(byteArrayType))
//                            {
//                                byteArrayCols.Add(colIdx);
//                            }
//                            else
//                            {
//                                otherCols.Add(colIdx);
//                            }
//                            colIdx++;
//                        }
//                    }
//                }
//            }
//        }
//        /// <summary>
//        /// Return table column names in their natural order
//        /// </summary>
//        /// <returns></returns>
//        public List<string> GetOrderedFieldNames()
//        {
//            return this.orderedFieldNames;
//        }
//        /// <summary>
//        /// Returns number of rows committed
//        /// </summary>
//        /// <param name="row"></param>
//        /// <returns></returns>
//        public int InsertRow(string[] row)
//        {
//            Console.WriteLine("OraBulkLoader insertRow(" + row.Join(","));

//            lock (this.dataTable)
//            {
//                DataRow dataRow = this.dataTable.NewRow();

//                // convert data columns
//                foreach (int colIdx in this.dateCols)
//                {
//                    var c = this.dataTable.Columns[colIdx]; 
//                    string from = row[colIdx];
//                    object to;
//                    if (from.IsNullOrEmpty())
//                    {
//                        Console.WriteLine("col:" + c.ColumnName + ",type=" + c.DataType.FullName+",idx="+colIdx + " is null ");
//                        to = DBNull.Value;
//                    }
//                    else
//                    {
//                        // ("YYYYMMDDHH24MISS") ->("YYYY-MM-DD HH24:MI:SS")
//                        to = from.Substring(0, 4) + "-" + from.Substring(4, 2) + "-" + from.Substring(6, 2) + " " + from.Substring(8, 2) + ":" + from.Substring(10, 2) + ":" + from.Substring(12, 2);
//                        Console.WriteLine("Converting date col:" + c.ColumnName + ",type=" + c.DataType.FullName + " from=" + from + ", colIdx=" + colIdx + ",ordinal=" + c.Ordinal);
//                    }
//                    dataRow[colIdx] = to;
//                }
//                // convert byte array columns to true byte arrays
//                foreach (int colIdx in this.byteArrayCols)
//                {
//                    var c = this.dataTable.Columns[colIdx];
//                    string from = row[colIdx];
//                    object to;
//                    if (from.IsNullOrEmpty())
//                    {
//                        Console.WriteLine("col:" + c.ColumnName + ",type=" + c.DataType.FullName+",idx="+colIdx + " is null ");
//                        to = DBNull.Value;
//                    }
//                    else
//                    {
//                        to = StringToByteArray(from);
//                        Console.WriteLine("Converting byte[] col:" + c.ColumnName + ",type=" + c.DataType.FullName + " fromHex=" + from + ", colIdx=" + colIdx + ",ordinal=" + c.Ordinal);
//                    }
//                    dataRow[colIdx] = to;
//                }

//                // add the other columns
//                foreach (int colIdx in this.otherCols)
//                {
//                    var c = this.dataTable.Columns[colIdx];
//                    string from = row[colIdx];
//                    object to;
//                    if (from.IsNullOrEmpty())
//                    {
//                        Console.WriteLine("col:" + c.ColumnName + ",type=" + c.DataType.FullName+",idx="+colIdx + " is null ");
//                        to = DBNull.Value;
//                    }
//                    else
//                    {
//                        Console.WriteLine("Other col:" + c.ColumnName + ",type=" + c.DataType.FullName + "string=" + row[colIdx] + ", colIdx=" + colIdx + ",ordinal=" + c.Ordinal);
//                        to = from;
//                    }
//                    dataRow[colIdx] = to;
//                }

//                this.dataTable.Rows.Add(dataRow);
//                if (this.dataTable.Rows.Count >= this.commitSize)
//                    return this.Flush();
//            }
//            return 0;
//        }

//        public static byte[] StringToByteArray(String hex)
//        {
//            if (hex.StartsWith("0x")) hex = hex.Substring(2);
//            int NumberChars = hex.Length;
//            byte[] bytes = new byte[NumberChars / 2];
//            for (int i = 0; i < NumberChars; i += 2)
//                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
//            return bytes;
//        }

//        /// <summary>
//        /// Returns number of rows committed
//        /// </summary>
//        /// <returns></returns>
//        public int Flush()
//        {
//            int committing = this.dataTable.Rows.Count;
//            if (committing > 0)
//            {
//                lock (this.dataTable)
//                {
//                    using (OracleConnection conn = OracleBulkLoader11g.GetOraConnection(db, user, pw))
//                    {
//                        OracleBulkCopy bcp = new OracleBulkCopy(conn);
//                        bcp.DestinationTableName = this.tableName;
//                        bcp.WriteToServer(this.dataTable);
//                        this.dataTable.Clear();
//                        bcp.Close();
//                    }
//                }
//            }
//            return committing;
//        }
//        /// <summary>
//        /// Discarded queued up rows and returns the number we aborted.
//        /// </summary>
//        /// <returns></returns>
//        public int Abort()
//        {
//            int aborting = this.dataTable.Rows.Count;
//            this.dataTable.Clear();
//            return aborting;
//        }

//        public static OracleConnection GetOraConnection(string db, string user, string pw)
//        {
//            string connString = @"Pooling=true;Data Source = " + db + ";User Id = " + user + ";Password = " + pw + ";";
//            OracleConnection conn = new OracleConnection(connString);
//            try
//            {
//                conn.Open();
//                new OracleCommand("alter session set nls_date_format = 'YYYYMMDDHH24MISS'", conn).ExecuteNonQuery();
//                new OracleCommand("alter session set nls_timestamp_format = 'YYYYMMDDHH24MISS'", conn).ExecuteNonQuery();
//                return conn;
//            }
//            catch (Exception e)
//            {
//                throw new Exception("Error, cannot connect using connString=[" + connString + "], msg=" + e.Message, e);
//            }
//        }
//    }
//}
