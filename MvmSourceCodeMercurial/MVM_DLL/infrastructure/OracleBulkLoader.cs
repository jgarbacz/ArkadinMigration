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
    public class OracleBulkLoader:IBulkLoader
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public string TableName { get { return this.tableName; } }   
        public readonly string connString;
        public readonly string tableName;
        public readonly List<string> orderedFieldNames = new List<string>();
        public int commitSize;
        public readonly string server;
        public readonly string db;
        public readonly string user;
        public readonly string pw;

        public static Type dateTimeType = new System.DateTime().GetType();
        public List<int> dateCols = new List<int>();
        public static Type byteArrayType = new Byte[1].GetType();
        public List<int> byteArrayCols = new List<int>();
        public List<int> otherCols = new List<int>();

        // need an object array for each column we need to write to..
        public int numRows = 0;
        public OracleDbType[] columnTypes;
        public int columnCount;
        public object[][] columnData;
        public string[] columnNames;
        public string[] columnBinds;
        public string insertSql;
        public OracleCommand insertCmd;
        

        public OracleBulkLoader(string server, string db, string user, string pw, string tableName, int commitSize, params string[] fieldNames)
        {
            this.server = server;
            this.db = db;
            this.user = user;
            this.pw = pw;
            this.commitSize = commitSize;
            this.tableName = tableName;
            string columnNamesString = fieldNames.NotNullOrEmpty()?fieldNames.JoinStrings(","):"*";
            using (OracleConnection conn = DbUtilsOra.GetOraConnection(db,user,pw))
            {
                string queryString="SELECT " + columnNamesString + " FROM " + this.tableName + " where 1=2";
                try
                {
                    using (OracleCommand formatQuery = new OracleCommand(queryString, conn))
                    {
                        using (OracleDataReader formatReader = formatQuery.ExecuteReader())
                        {
                            this.columnCount = formatReader.FieldCount;
                            this.columnData = new object[this.columnCount][];
                            this.columnTypes = new OracleDbType[this.columnCount];
                            this.columnNames = new string[this.columnCount];
                            this.columnBinds = new string[this.columnCount];
                            for (int colNo = 0; colNo < this.columnCount; colNo++)
                            {
                                this.columnData[colNo] = new object[this.commitSize];
                                //var t = formatReader.GetProviderSpecificFieldType(colNo);
                                this.columnTypes[colNo] = OracleDbType.NVarchar2;
                                this.columnNames[colNo] = formatReader.GetName(colNo);
                                this.columnBinds[colNo] = ":" + formatReader.GetName(colNo);
                                this.orderedFieldNames.Add(formatReader.GetName(colNo).ToLower());
                                if (formatReader.GetFieldType(colNo).Equals(dateTimeType))
                                {
                                    dateCols.Add(colNo);
                                }
                                else if (formatReader.GetFieldType(colNo).Equals(byteArrayType))
                                {
                                    byteArrayCols.Add(colNo);
                                }
                                else
                                {
                                    otherCols.Add(colNo);
                                }
                            }
                            this.insertSql = "insert into " + this.tableName + " (" + this.columnNames.JoinStrings(",") + ") values (" + this.columnBinds.JoinStrings(",") + ")";
                            this.insertCmd = conn.CreateCommand();
                            this.insertCmd.BindByName = true;
                            this.insertCmd.CommandText = this.insertSql;
                            this.insertCmd.CommandType = CommandType.Text;
                            for (int colNo = 0; colNo < this.columnCount; colNo++)
                            {
                                this.insertCmd.Parameters.Add(this.columnBinds[colNo], this.columnTypes[colNo], this.columnData[colNo], ParameterDirection.Input);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Error in OracleBulkLoader contructor. Cannot run format query ["+queryString+"]:"+e.Message,e);
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
            //logger.Debug("OraBulkLoader " + this.tableName + " insertRow(" + row.Join(",") + ")");
            lock (this)
            {
                for (int i = 0; i < this.columnCount; i++)
                {
                    this.columnData[i][this.numRows] = row[i];
                }
                this.numRows++;
                if (this.numRows >= this.commitSize)
                    return this.Flush();
            }
            return 0;
        }

        public static byte[] StringToByteArray(String hex)
        {
            if (hex.StartsWith("0x")) hex = hex.Substring(2);
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        /// <summary>
        /// Returns number of rows committed
        /// </summary>
        /// <returns></returns>
        public int Flush()
        {
            int committing = this.numRows;
            if (committing > 0)
            {
                //logger.Debug("OraBulkLoader Flush " + this.tableName + " numRows=" + committing);
                lock (this)
                {
                    using (OracleConnection conn = DbUtilsOra.GetOraConnection(db, user, pw))
                    {
                        //logger.Debug("ABL_DF="+conn.GetSessionInfo().DateFormat);
                        this.insertCmd.Connection = conn;
                        this.insertCmd.ArrayBindCount = committing;
                        try
                        {
                            int insertCount = this.insertCmd.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            string msg = "Error in OraBulkLoader.Flush: [" + this.insertCmd.CommandText + "] using connString=[" + conn.ConnectionString + "] Error msg=[" + e.Message+"]";
                            logger.Fatal(msg);
                            logger.Fatal("Data table contents:");
                            logger.Fatal(this.columnNames.JoinStrings("|"));
                            for (int rowNum = 0; rowNum < this.numRows; rowNum++)
                            {
                                List<string> cols = new List<string>();
                                for (int i = 0; i < this.columnCount; i++) cols.Add(this.columnData[i][rowNum].ToString());
                                logger.Fatal(cols.JoinStrings("|"));
                                 
                            }
                            throw new Exception(msg, e);
                        }
                        this.numRows = 0;
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
            int aborting = this.numRows;
            return aborting;
        }
    }
}
