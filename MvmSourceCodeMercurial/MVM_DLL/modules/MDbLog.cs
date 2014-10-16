//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Xml;
//using System.Diagnostics;
//using System.Data;

//// Setup:
//// -create a tnsname.ora file
//// -point TNS_ADMIN env var to directory where tnsnames.ora is located.
//// -point the path into the instant client dir
///*

// OBJECT.database_type ='SQL'
// OBJECT.database_username='MT'
// OBJECT.database_name='MYDB'
// OBJECT.database_server ="1.2.3.4"
// OBJECT.database_password="MetraTEch1"
 
// <default_db_context>
// <db>OBJECT.db</db>
// <user>OBJECT(OBJECT.login).user</user>
// <pw>GLOBAL.pw</pw> 
// <num_rows>GLOBAL.pw</num_rows> 
// <exception>GLOBAL.pw</exception> 
// <exception_proc>GLOBAL.pw</exception_proc> 
// <default_db_context>
 
//<db_call_proc>
//<db>GLOBAL.db</db>
//<user>GLOBAL.user</user>
//<pw>GLOBAL.pw</pw> 
//<procContext>'my_proc'</procContext>
//<param name='myin'>'123'</param>
//<param name='myout'>TEMP.out</param>
//<num_rows>OBJECT.database_num_rows</num_rows>
//<exception>OBJECT.database_exception</exception>
//<exception_proc><exception_proc>
//</db_call_proc>
//*/

//namespace MVM
//{
//    // log tableName name
//    // db info
//    // what to write to each column
//    //
//    //
//    // OBJECT.log_type='db'
//    // OBJECT.log_table_name='LOG_TABLE'
//    // OBJECT.log_message_column='msg'
//    // OBJECT.log_message_field='log_message'
//    // OBJECT.override_id_acc='OBJECT.id_acc'
//    // OBJECT.override_log_message='OBJECT.external_id~","~OBJECT.external_id_type~","~OBJECT.log_message'
//    class MDbLog: IModuleRun
//    {
//        private string procSyntax;
//        private Dictionary<string, string> paramsSyntax;
//        private IReadString procParsed;
//        private DbInfo dbInfo;
//        private string proc;
//        private List<ParamInfo> paramsInfo;
//        private List<string> orderedParamNames = new List<string>();
//        private Dictionary<string, IReadString> readableParamsParsed=new Dictionary<string,IReadString>();
//        private Dictionary<string, IWriteString> writableParamsParse=new Dictionary<string,IWriteString>();
//        private string numRowsSyntax;
//        private string exceptionSyntax;
//        private string exceptionProcSyntax;
//        private IWriteString numRowsParsed;
//        private IWriteString exceptionParsed;
//        private IReadString exceptionProcParsed;
//        private int exceptionProcId=-1;

//        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run, string loggerOid)
//        {
//            MDbLog m = new MDbLog();
//            m.dbInfo = new DbInfo(me, mc);
//            m.procSyntax = me.SelectNodeInnerText("./name");
//            m.paramsSyntax = me.SelectNodes("./param").ToNameTextDictionary();
//            m.procParsed = mc.ParseSyntax(m.procSyntax);
//            m.numRowsSyntax = me.SelectNodeInnerText("./num_rows","OBJECT.database_num_rows");
//            m.exceptionSyntax = me.SelectNodeInnerText("./exception", "OBJECT.database_exception");
//            m.exceptionProcSyntax = me.SelectNodeInnerText("./exception_proc", "OBJECT.database_exception_proc");

//            // procContext name prototype cannot change dynamically since we set this up once!
//            m.proc = m.procParsed.Read(mc);
//            string type = m.dbInfo.GetType(mc);
//            string server = m.dbInfo.GetServer(mc);
//            string db = m.dbInfo.GetDb(mc);
//            string user = m.dbInfo.GetUser(mc);
//            string pw = m.dbInfo.GetPw(mc);
//            m.numRowsParsed = mc.ParseWritableSyntax(m.numRowsSyntax);
//            m.exceptionParsed = mc.ParseWritableSyntax(m.exceptionSyntax);
//            m.exceptionProcParsed = mc.ParseSyntax(m.exceptionProcSyntax);
//            string exceptionProc = m.exceptionProcParsed.Read(mc);
//            if (!exceptionProc.Equals(""))
//            {
//                m.exceptionProcId = mc.GetProcId(exceptionProc);
//            }

//            // get the ordered param names!
//            m.paramsInfo = DbUtilsOra.OraPrepareStoredProc(db, user, pw, m.proc);
//            foreach (ParamInfo p in m.paramsInfo)
//            {
//                m.orderedParamNames.Add(p.name);
//                if (p.mode.Equals("IN"))
//                {
//                    string paramValueSyntax="OBJECT."+p.name;
//                    if(m.paramsSyntax.ContainsKey(p.name)) paramValueSyntax=m.paramsSyntax[p.name];
//                    IReadString paramValueParsed = mc.ParseSyntax(paramValueSyntax);
//                    m.readableParamsParsed[p.name] = paramValueParsed;
//                }
//                else if (p.mode.Equals("OUT"))
//                {
//                    string paramValueSyntax = "OBJECT." + p.name;
//                    if (m.paramsSyntax.ContainsKey(p.name)) paramValueSyntax = m.paramsSyntax[p.name];
//                    IWriteString paramValueParsed = mc.ParseWritableSyntax(paramValueSyntax);
//                    m.writableParamsParse[p.name] = paramValueParsed;
//                }
//                else if (p.mode.Equals("IN/OUT"))
//                {
//                    string paramValueSyntax = "OBJECT." + p.name;
//                    if (m.paramsSyntax.ContainsKey(p.name)) paramValueSyntax = m.paramsSyntax[p.name];
//                    IReadString rParamValueParsed = mc.ParseSyntax(paramValueSyntax);
//                    m.readableParamsParsed[p.name] = rParamValueParsed;
//                    IWriteString wParamValueParsed = mc.ParseWritableSyntax(paramValueSyntax);
//                    m.writableParamsParse[p.name] = wParamValueParsed;
//                }
//                else
//                {
//                    throw new Exception("Unexpected parameter mode:"+p.mode);
//                }
//            }
//            run.Add(m);
//        }

//        public void Run(ModuleContext mc)
//        {
//            string type = this.dbInfo.GetType(mc);
//            string server = this.dbInfo.GetServer(mc);
//            string db = this.dbInfo.GetDb(mc);
//            string user = this.dbInfo.GetUser(mc);
//            string pw = this.dbInfo.GetPw(mc);
//            List<string> orderedParamValues = new List<string>();
//            // execute the stored procContext
//            string connString = @"Pooling=true;server = " + db + ";uid = " + user + ";password = " + pw + ";";
//            using (OracleConnection conn = DbUtilsOra.GetOraConnection(db, user, pw))
//            {
//                try
//                {
//                    using (OracleCommand stmt = new OracleCommand(proc, conn))
//                    {
//                        stmt.CommandType = CommandType.StoredProcedure;
//                        stmt.BindByName = true;
//                        foreach (ParamInfo pi in this.paramsInfo)
//                        {
//                            if (pi.mode.Equals("IN"))
//                            {
//                                string paramValue = this.readableParamsParsed[pi.name].Read(mc);
//                                var param = stmt.CreateParameter();
//                                param.ParameterName = pi.name;
//                                param.OracleDbType = OracleDbType.NVarchar2;
//                                param.Value = paramValue;
//                                stmt.Parameters.Add(param);
//                                orderedParamValues.Add(paramValue);
//                            }
//                            else if (pi.mode.Equals("OUT"))
//                            {
//                                var param = stmt.CreateParameter();
//                                param.ParameterName = pi.name;
//                                param.Direction = ParameterDirection.Output;
//                                param.OracleDbType = OracleDbType.NVarchar2;
//                                param.Size = 4000;
//                                stmt.Parameters.Add(param);
//                                orderedParamValues.Add("OUT_PARAM");
//                            }
//                            else if (pi.mode.Equals("IN/OUT"))
//                            {
//                                var param = stmt.CreateParameter();
//                                string paramValue = this.readableParamsParsed[pi.name].Read(mc);
//                                param.ParameterName = pi.name;
//                                param.Direction = ParameterDirection.InputOutput;
//                                param.OracleDbType = OracleDbType.NVarchar2;
//                                param.Size = 4000;
//                                param.Value = paramValue;
//                                stmt.Parameters.Add(param);
//                                orderedParamValues.Add(paramValue);
//                            }
//                        }
            
//                        int numRows = stmt.ExecuteNonQuery();
//                        this.numRowsParsed.Write(mc, numRows.ToString());
//                        foreach (var p in this.writableParamsParse.Keys)
//                        {
//                            this.writableParamsParse[p].Write(mc, stmt.Parameters[p].Value.ToString());
//                        }
//                    }
//                }
//                catch (OracleException e)
//                {
//                    string msg = "Error calling proc: " + proc + " (" + orderedParamValues.Join(",") + "). Ordered param names:" + this.orderedParamNames.Join(",") + ". Db error msg:" + e.Message;
//                    this.exceptionParsed.Write(mc, msg);
//                    if (this.exceptionProcId >= 0)
//                    {
//                        mc.CallProcForCurrentObject(this.exceptionProcId);
//                    }
//                    else
//                    {
//                        throw new Exception(msg, e);
//                    }
//                }
//            }
//        }

//    }
//}
