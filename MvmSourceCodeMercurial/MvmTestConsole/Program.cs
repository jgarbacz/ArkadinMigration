using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.SqlClient;
using System.Diagnostics;
using MVM;
using System.Data;
using System.Management;
using System;
using System.Management;
using System.IO;
using Oracle.DataAccess;
using Oracle.DataAccess.Client;
namespace MvmTestConsole
{



    public class App
    {
        [MTAThread]
        public static void Go(string[] args)
        {
            TMain();

            // Beware! the account used to connect must have remote WMI privileges on the remote server.

            RunProcess M = new RunProcess("metratech\rparks", "nmNM0710", "RparksI7w7");
            M.Run();
        }

        public static void TMain()
        {
            // Build an options object for the remote connection
            // if you plan to connect to the remote
            // computer with a different user name
            // and password than the one you are currently using.
            // This example uses the default values. 

            ConnectionOptions options =
                new ConnectionOptions();
            options.Username = "rparks";
            options.Password = "nmNM0710";


            // Make a connection to a remote computer.
            // Replace the "FullComputerName" section of the
            // string "\\\\FullComputerName\\root\\cimv2" with
            // the full computer name or IP address of the
            // remote computer.
            ManagementScope scope =
                new ManagementScope(
                "\\\\rparksI7w7.metratech.com\\root\\cimv2", options);
            //"\\\\FullComputerName\\root\\cimv2", options);
            scope.Connect();

            //Query system for Operating System information
            ObjectQuery query = new ObjectQuery(
                "SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher(scope, query);

            ManagementObjectCollection queryCollection = searcher.Get();
            foreach (ManagementObject m in queryCollection)
            {
                // Display the remote computer information
                Console.WriteLine("Computer Name : {0}",
                    m["csname"]);
                Console.WriteLine("Windows Directory : {0}",
                    m["WindowsDirectory"]);
                Console.WriteLine("Operating System: {0}",
                    m["Caption"]);
                Console.WriteLine("Version: {0}", m["Version"]);
                Console.WriteLine("Manufacturer : {0}",
                    m["Manufacturer"]);
            }
        }


        sealed class RunProcess
        {
            private ConnectionOptions co;
            private ManagementScope scope;

            public RunProcess(string ConnectionUser, string ConnectionPassword, string Machine)
            {
                co = new ConnectionOptions();
                co.Username = ConnectionUser;
                co.Password = ConnectionPassword;
                co.Impersonation = ImpersonationLevel.Impersonate;
                scope = new ManagementScope(@"\\" + Machine + @"\root\cimv2", co);
                scope.Connect();
            }
            public void Run()
            {
                string logFileName = "system";
                UInt32 recordNumber = 0;
                // default blocksize = 1, larger value may increase network throughput
                EnumerationOptions opt = new EnumerationOptions();
                opt.BlockSize = 1000;
                // Get only Logon/LogOff category from security log
                //          SelectQuery query = new SelectQuery("select RecordNumber, CategoryString, TimeGenerated, User, Type from Win32_NtLogEvent where RecordNumber > " + recordNumber + " and Logfile ='" + logFileName + "' " + "and category = 2");
                //          SelectQuery query = new SelectQuery("select RecordNumber, CategoryString, TimeGenerated, User, Type from Win32_NtLogEvent where Logfile ='" + logFileName + "' " + "and category = 2");
                SelectQuery query = new SelectQuery("select RecordNumber, CategoryString, TimeGenerated, User, Type from Win32_NtLogEvent where RecordNumber > 100 and Logfile ='" + logFileName + "' ");
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query, opt))
                {
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        string logInfo = String.Format("{0} - {1} - {2} - (3)", mo["RecordNumber"], mo["Type"], mo["CategoryString"], mo["User"]);
                        //               string logInfo = String.Format("{0} - {1} - {2}", mo["Type"], mo["CategoryString"], mo["User"]);
                        Console.WriteLine(logInfo);
                    }
                }
            }
        }


        public class TestOraOdp
        {
            public static void Go()
            {
                Stopwatch sw = new Stopwatch();

                Dictionary<string, string> obj = new Dictionary<string, string>();
                string db = "dev-rac1";
                string user = "perf4_na";
                string pw = "perf4_na";
                OracleConnection conn = DbUtils.GetOraConnection(db, user, pw);
                sw.Start();
                OracleCommand oracleCommand = new OracleCommand(@"select /*+ FIRST_ROWS ORDERED USE_NL(b c) PUSH_PRED BOOBOO */
          c.*
          from adeuh_A66508BF7FFB1D0BE040C80A a,
          T_ACC_USAGE  b,
          t_pv_InterCallConnection c
          where
          b.id_usage_interval=949420062
          and a.id_acc=b.id_payee
          and b.id_view = 7690
          and b.id_sess=c.id_sess
          and b.id_usage_interval = c.id_usage_interval
          order by b.id_sess");
                try
                {
                    oracleCommand.Connection = conn;
                    oracleCommand.FetchSize = 1024*1024*10;
                    OracleDataReader reader = oracleCommand.ExecuteReader();
                    List<string> fieldNames = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string fieldName = reader.GetName(i).ToLower();
                        fieldNames.Add(fieldName);
                    }
                    while (reader.Read())
                    {
                        for (int i = 0; i < fieldNames.Count; i++)
                        {
                            string x = reader.GetValue(i).ToString();
                            obj[fieldNames[i]]=x;
                        }
                        //Console.WriteLine("gotrow");
                    }
                    sw.Stop();
                    Console.WriteLine("TIME="+sw.ElapsedMilliseconds);
                }
                catch (Exception e)
                {
                    throw new Exception("Error, running query string=[" + oracleCommand.CommandText + "] using connString=[" + conn.ConnectionString + "]".AppendLine() + "Error msg=" + e.Message, e);
                }
                Console.ReadKey();
            }
        }

        class Program
        {

            static void Main(string[] args)
            {
                Console.WriteLine("RUNNING MY TEST CONSOLE");
                TestOraOdp.Go();
                if (1 == 1) return;
                
                App.Go(args);
                //AntlrXPath.Test();
                //MyXml.SpeedTest();
                //XmlConfigParser.Test();
                //TestBcp();
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();


                //SplitStopwatch.Test();
                //dbtest();



                //List<CursorDistinctRow> mylist = new List<CursorDistinctRow> { 
                //    new CursorDistinctRow("1"),
                //    new CursorDistinctRow("2"),
                //    new CursorDistinctRow("2")
                //};
                //Console.WriteLine("0==2" + mylist[0].Equals(mylist[2]).ToString());
                //Console.WriteLine("1==2" + mylist[1].Equals(mylist[2]).ToString());
                //foreach (var x in mylist) Console.WriteLine(x.vals[0]);
                //Console.WriteLine("distinct");
                //foreach (var x in mylist.Distinct()) Console.WriteLine(x.vals[0]);

                //string x1 = "a-b5";
                //string x2 = "ab-4";
                //string x3 = "ab-6";
                //Console.WriteLine("x1="+x1);
                //Console.WriteLine("x2="+x2);
                //Console.WriteLine("x3="+x3);
                //Console.WriteLine("x1 compared to x2 better be the same as x1 compared to x3!");
                //Console.WriteLine("x1.CompareTo(x2)=" + x1.CompareTo(x2));
                //Console.WriteLine("x1.CompareTo(x3)=" + x1.CompareTo(x3));
                //Console.WriteLine("But they are not!!!!");

                //Console.WriteLine("Now this is going to break my sort...");
                //string[] arr=new string[]{x1,x2,x3};

                //Console.WriteLine("string[] unsorted");
                //for (int i = 0; i < arr.Length; i++) Console.WriteLine(arr[i]);
                //Array.Sort(arr);
                //Console.WriteLine("string[] sorted");
                //for (int i = 0; i < arr.Length; i++) Console.WriteLine(arr[i]);


                ////if(1==1) return;


                //string aa = "122271-11704600";
                //string bb = "1222711-1668827";
                //string cc = "1222711-6500000";



                //Console.WriteLine("a->b=" + aa.CompareTo(bb));
                //Console.WriteLine("a->c=" + aa.CompareTo(cc));

                //Console.WriteLine("b->c=" + bb.CompareTo(cc));
                //Console.WriteLine("b->a=" + bb.CompareTo(aa));
                //Console.WriteLine("c->a=" + cc.CompareTo(aa));
                //Console.WriteLine("c->b=" + cc.CompareTo(bb));

                //List<string> slist = new List<string> { aa, bb, cc };
                //Console.WriteLine("string[] unsorted a b c");
                //slist.ForEach(x => Console.WriteLine(x));
                //slist.Sort();
                //Console.WriteLine("string[] sorted");
                //slist.ForEach(x => Console.WriteLine(x));

                //StringArray a = new StringArray(aa);
                //StringArray b = new StringArray(bb);
                //StringArray c = new StringArray(cc);

                //Console.WriteLine("a->b=" + a.CompareTo(b));
                //Console.WriteLine("a->c=" + a.CompareTo(c));
                //Console.WriteLine("b->c=" + b.CompareTo(c));
                //Console.WriteLine("b->a=" + b.CompareTo(a));
                //Console.WriteLine("c->a=" + c.CompareTo(a));
                //Console.WriteLine("c->b=" + c.CompareTo(b));

                //List<StringArray> list = new List<StringArray>();
                //list.Add(a);
                //list.Add(b);
                //list.Add(c);

                //Console.WriteLine("unsorted a b c");
                //list.ForEach(x => Console.WriteLine(x.array[0]));
                //list.Sort();
                //Console.WriteLine("sorted");
                //list.ForEach(x => Console.WriteLine(x.array[0]));

                //RmpSearcher.Test();

                //List<string[]> output= GrepForProcs(@"C:\Users\rparks\Code\junk.xml","mystuff");
                //Console.WriteLine("DONE RUNNING MY TEST CONSOLE");


                //var serverInfo = new MTSERVERACCESSLib.MTServerAccessDataSet();
                //serverInfo.ReadWriteStart();
                //var info = serverInfo.FindAndReturnObject("Netmeter");
                //var user = info.UserName;

                //string databaseType,  serverName, databaseName, userName,  password;
                //MtReflection.GetMtDbInfo("Netmeter", out databaseType, out serverName, out databaseName, out userName, out password);

                //Console.WriteLine(password);
                //// how can i stay a level of direction away...





                //string assemblyFile = @"MetraTech.DomainModel.MvmTypes.dll";
                //string codeFile = @".\mvm\MetraTech.DomainModel.MvmTypes.cs";
                //Console.WriteLine("Try compiling "+codeFile+" into "+assemblyFile);
                //string[] references = new string[] { 
                //    "MetraTech.DomainModel.BaseTypes.dll",
                //    "MetraTech.DomainModel.Common.dll",
                //    "MetraTech.DomainModel.Enums.Generated.dll",
                //    "System.dll",
                //    "System.Core.dll",
                //    "System.Data.dll",
                //    "System.Data.DataSetExtensions.dll",
                //    "System.Runtime.Serialization.dll",
                //    "System.ServiceModel.dll",
                //    "System.Web.Extensions.dll",
                //    "System.Xml.dll",
                //    "System.Xml.Linq.dll"
                //};

                //string compilerOptions = @"/keyfile:D:\MetraTech\RMP\bin\MetraTech.snk";
                //string output;
                //if (CodeGen.CompileDll(codeFile, references, assemblyFile, compilerOptions, out output))
                //{
                //    Console.WriteLine("Successfully compiled:" + codeFile);
                //}
                //else
                //{
                //    Console.WriteLine("CANNOT compile:" + codeFile+":".AppendLine()+output);
                //}

            }

            static void TestBcp()
            {
                using (SqlConnection conn = DbUtils.GetSqlConnection(null, "Netmeter", null, null))
                {
                    SqlCommand formatQuery = new SqlCommand("SELECT * FROM BulkCopyDemoMatchingColumns where 1=2", conn);
                    SqlDataReader formatReader = formatQuery.ExecuteReader();
                    DataTable table = new DataTable();
                    table.Load(formatReader);
                    table.TableName = "robtable";
                    foreach (Constraint c in table.Constraints)
                    {
                        Console.WriteLine("cons=" + c.ConstraintName);
                    }



                    // ordered columns names
                    foreach (DataColumn column in table.Columns)
                    {
                        Console.WriteLine(column.ColumnName + " " + column.DataType);
                    }
                    //ProductID System.Int32
                    //Name System.String
                    //ProductNumber System.String
                    DataRow row = table.NewRow();
                    row[0] = 1;
                    row[1] = 2;
                    row[2] = 50.55;
                    row[3] = "2010-08-24 13:12:07.183";
                    table.Rows.Add(row);

                    SqlBulkCopy bcp = new SqlBulkCopy(conn);
                    bcp.DestinationTableName = "BulkCopyDemoMatchingColumns";
                    bcp.WriteToServer(table);

                    bcp.Close();
                    Console.WriteLine("copied rows");

                    // need an targetObj TableBulkInsert per tableName that 
                    //      stores the tableName format so i can pull the proper fields.
                    //      stores a DataTable b/c this is the only way i know to get a datarow.
                    // Dictionary<string,TableBulkInsert>
                }
            }



            static void dbtest()
            {
                string server = "pcg-poc-pmna-b1";
                string db = "netmeter";
                string user = "sa";
                string pw = "MetraTech1";
                string queryString = @"select a.*, c.orig_dt_start, c.orig_dt_end, c.id_sched orig_id_sched, c.orig_begintype, c.orig_endtype 
from t_pt_intltransportrate a
                  left outer join rapid_rate_consolidations b on a.id_audit = b.id_audit and b.current_id_sched = 2629709
                  left outer join rapid_rate_date_changes c on b.id_sched = c.id_sched and c.change_tt_end >= case '' when '' then dbo.MTMaxDate() else '' end 
                                                and case '' when '' then dbo.MTMaxDate() else '' end > c.change_tt_start
                  where a.id_sched = 2629709
                  and tt_end = dbo.MTMaxDate()
                  order by a.n_order desc

";
                Dictionary<string, string> rowHash = new Dictionary<string, string>();
                for (int x = 0; x < 26; x++)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    SqlConnection conn = DbUtils.GetSqlConnection(server, db, user, pw);
                    SqlCommand command;
                    SqlDataReader reader;
                    try
                    {
                        command = new SqlCommand(queryString, conn);
                        command.CommandTimeout = 0;
                        reader = command.ExecuteReader();
                        // get the name of the query results
                        List<string> fieldNames = new List<string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string fieldName = reader.GetName(i).ToLower();
                            fieldNames.Add(fieldName);
                        }
                        // loop thru results
                        while (reader.Read())
                        {
                            for (int i = 0; i < fieldNames.Count; i++)
                            {
                                string paramValue = null;
                                var fieldType = reader.GetFieldType(i);
                                try
                                {
                                    if (reader.IsDBNull(i))
                                    {
                                        paramValue = "";
                                    }
                                    else if (fieldType.FullName.Equals("System.DateTime") && !reader.IsDBNull(i))
                                    {
                                        DateTime dt = (DateTime)reader.GetValue(i);
                                        paramValue = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");//2009-10-31 00:00:00.000
                                    }
                                    else
                                    {
                                        paramValue = System.Convert.ToString(reader.GetValue(i));
                                    }
                                }
                                catch (Exception e)
                                {
                                    throw new Exception("cannot read field " + fieldNames[i], e);
                                }
                                rowHash[fieldNames[i]] = paramValue;
                                //Console.WriteLine(fieldNames[i] + "=" + paramValue);
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error, running query string=[" + queryString + "] using connString=[" + conn.ConnectionString + "].".AppendLine() + "Sql error msg=" + e.Message, e);
                    }
                    command.Dispose();
                    reader.Close();
                    conn.Close();
                    sw.Stop();
                    Console.WriteLine("done ms=" + sw.ElapsedMilliseconds.ToString());
                }
            }





        }
    }
}
