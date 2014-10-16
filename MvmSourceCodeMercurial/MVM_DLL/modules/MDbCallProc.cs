using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using Oracle.DataAccess.Client;
using System.Data.SqlClient;
using System.Data;

namespace MVM
{
    // TBD: This instanciates a new Command each time. It should prepare once like db_execute does.
    class MDbCallProc : MDbCommon, IModuleSetup
    {
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDbCallProc m = new MDbCallProc();
            m.ParseDbCommon(me, mc, "'db_call_proc'");
            if (m.type.EqualsIgnoreCase("oracle"))
            {
                new MDbCallProcOra().Setup(me, mc, run);
            }
            else if (m.type.EqualsIgnoreCase("sql"))
            {
                new MDbCallProcSql().Setup(me, mc, run);
            }
            else
            {
                throw new Exception("unexpected db type=[" + m.type + "]");
            }
        }
    }

    abstract class MDbCallProcCommon : MDbCommon, IModuleRun
    {
        protected Dictionary<string, string> paramsSyntax;
        protected List<ParamInfo> paramsInfo;
        protected List<string> orderedParamNames = new List<string>();
        protected Dictionary<string, IReadString> readableParamsParsed = new Dictionary<string, IReadString>();
        protected Dictionary<string, List<IWriteString>> writableParamsParsed = new Dictionary<string, List<IWriteString>>();
        protected List<string> passedParamInputs = new List<string>();
        protected List<string> passedParamOutputs = new List<string>();
        protected void CommonSetupBefore(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {

            this.ParseDbCommon(me, mc, me.SelectNodeInnerText("./name"));
            var paramNodes=me.SelectNodes("./param");
            if(paramNodes==null)
                this.paramsSyntax = new Dictionary<string, string>();
            else
                this.paramsSyntax = paramNodes.ToNameTextDictionary();
           
            // get the ordered param names
            this.paramsInfo = mc.globalContext.schemaMaster.GetProcInfo(this.type, this.server, this.db, this.user, this.pw, this.name).paramInfo;
            foreach (ParamInfo p in this.paramsInfo)
            {
                this.orderedParamNames.Add(p.nameLower);
                if (p.mode.Equals("IN"))
                {
                    string tempField = "TEMP." + p.nameLower;
                    string objectField = "OBJECT." + p.nameLower;
                    string nvlTempObjectField = "(" + tempField + " ne \"\" ? " + tempField + " : " + objectField + ")";
                    string paramValueSyntax = nvlTempObjectField;
                    if (this.paramsSyntax.ContainsKey(p.nameLower)) paramValueSyntax = this.paramsSyntax[p.nameLower];
                    IReadString paramValueParsed = mc.ParseSyntax(paramValueSyntax);
                    this.readableParamsParsed[p.nameLower] = paramValueParsed;
                    passedParamInputs.Add("'" + p.nameLower +/*":"+paramValueSyntax+*/"=['~" + paramValueSyntax + "~']'");
                    passedParamOutputs.Add("'" + p.nameLower + /*":" + paramValueSyntax + */"=['~" + paramValueSyntax + "~']'");
                }
                else if (p.mode.Equals("OUT"))
                {
                    this.writableParamsParsed[p.nameLower] = new List<IWriteString>();
                    // direct capture so trust the user to specify correctly
                    if (this.paramsSyntax.ContainsKey(p.nameLower))
                    {
                        string paramValueSyntax = this.paramsSyntax[p.nameLower];
                        IWriteString paramValueParsed = mc.ParseWritableSyntax(paramValueSyntax);
                        this.writableParamsParsed[p.nameLower].Add(paramValueParsed);
                        passedParamInputs.Add("'out " + p.nameLower + /*":" + paramValueSyntax +*/ "=['~" + paramValueSyntax + "~']'");
                        passedParamOutputs.Add("'out " + p.nameLower + /*":" + paramValueSyntax +*/ "=['~" + paramValueSyntax + "~']'");
                    }
                    // default capture, write to TEMP.procalias+ _ + paramname and then TEMP.paramname
                    else
                    {
                        string paramValueSyntax1 = "TEMP." + this.alias + "_" + p.nameLower;
                        IWriteString paramValueParsed1 = mc.ParseWritableSyntax(paramValueSyntax1);
                        this.writableParamsParsed[p.nameLower].Add(paramValueParsed1);
                        passedParamInputs.Add("'out " + p.nameLower + /*":" + paramValueSyntax1 +*/ "=['~" + paramValueSyntax1 + "~']'");
                        passedParamOutputs.Add("'out " + p.nameLower + /*":" + paramValueSyntax1 +*/ "=['~" + paramValueSyntax1 + "~']'");
                        string paramValueSyntax2 = "TEMP." + p.nameLower;
                        IWriteString paramValueParsed2 = mc.ParseWritableSyntax(paramValueSyntax2);
                        this.writableParamsParsed[p.nameLower].Add(paramValueParsed2);
                    }
                }
                else if (p.mode.Equals("IN/OUT"))
                {
                    this.writableParamsParsed[p.nameLower] = new List<IWriteString>();
                    // direct capture so trust the user to specify correctly
                    if (this.paramsSyntax.ContainsKey(p.nameLower))
                    {
                        string paramValueSyntax = this.paramsSyntax[p.nameLower];
                        IWriteString paramValueParsed = mc.ParseWritableSyntax(paramValueSyntax);
                        this.writableParamsParsed[p.nameLower].Add(paramValueParsed);
                        passedParamInputs.Add("'in/out " + p.nameLower + /*":" + paramValueSyntax +*/ "=['~" + paramValueSyntax + "~']'");
                        passedParamOutputs.Add("'in/out " + p.nameLower + /*":" + paramValueSyntax +*/ "=['~" + paramValueSyntax + "~']'");
                        // read
                        IReadString rParamValueParsed = mc.ParseSyntax(paramValueSyntax);
                        this.readableParamsParsed[p.nameLower] = rParamValueParsed;
                    }
                    // default capture, write to TEMP.procalias+_+ paramname and then TEMP.paramname
                    else
                    {
                        string paramValueSyntax1 = "TEMP." + this.alias + "_" + p.nameLower;
                        IWriteString paramValueParsed1 = mc.ParseWritableSyntax(paramValueSyntax1);
                        this.writableParamsParsed[p.nameLower].Add(paramValueParsed1);
                        passedParamOutputs.Add("'in/out " + p.nameLower + /* ":" + paramValueSyntax1 + */  "=['~" + paramValueSyntax1 + "~']'");
                        string paramValueSyntax2 = "TEMP." + p.nameLower;
                        IWriteString paramValueParsed2 = mc.ParseWritableSyntax(paramValueSyntax2);
                        this.writableParamsParsed[p.nameLower].Add(paramValueParsed2);
                        passedParamInputs.Add("'in/out " + p.nameLower + /* ":" + paramValueSyntax2 + */ "=['~" + paramValueSyntax2 + "~']'");
                        // read from TEMP.paramname
                        IReadString rParamValueParsed = mc.ParseSyntax(paramValueSyntax2);
                        this.readableParamsParsed[p.nameLower] = rParamValueParsed;
                    }
                }
                else
                {
                    throw new Exception("Unexpected parameter mode:" + p.mode);
                }
            }
        }
        protected void CommonSetupAfter(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            string beginArgs = passedParamInputs.Count > 0 ? passedParamInputs.Join("~', '~") : "''";
            string beginMsg = "'call stored proc " + this.name + "('~" + beginArgs + "~')'";
            string endMsg = "(TEMP.database_exception eq ''?'done':'error')~' stored proc " + this.name + "('~" + beginArgs + "~') '~(TEMP.database_exception eq ''?'num_rows=['~" + this.numRowsSyntaxList[0] + "~']':'exception=['~TEMP.database_exception~']')";

            //// add logging and main newModule
            //SchedulerMaster sm = mc.scheduler.schedulerMaster;
            //run.Add(sm.GetModuleRun("<do>TEMP.database_exception=''</do>"));
            //run.Add(mc.GetLogModule(beginMsg, this.logLevel));
            //run.Add(this);
            //run.Add(mc.GetLogModule(endMsg, this.logLevel));

            // Add this runtime select newModule
            // add logging and main newModule
            if (logLevel == null || logLevel.Equals("")) logLevel = "debug";
            SchedulerMaster sm = mc.scheduler.schedulerMaster;
            run.Add(sm.GetModuleRun("<do>TEMP.database_exception=''</do>"));
            run.Add(mc.GetLogModule(beginMsg, logLevel, false));
            run.Add(this);
            run.Add(mc.GetLogModule(endMsg, logLevel, false));

        }
        public abstract void Run(ModuleContext mc);
    }

    class MDbCallProcOra : MDbCallProcCommon, IModuleSetup, IModuleRun
    {
        //private OracleCommand oracleCommand;
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDbCallProcOra m = new MDbCallProcOra();
            m.CommonSetupBefore(me, mc, run);
            m.CommonSetupAfter(me, mc, run);
        }

        public override void Run(ModuleContext mc)
        {
            this.RefreshDbInfo(mc);
            // execute the stored procContext

            List<string> orderedParamValues = new List<string>();
            using (OracleConnection conn = DbUtilsOra.GetOraConnection(db, user, pw))
            {
                try
                {
                    using (OracleCommand stmt = new OracleCommand(name, conn))
                    {
                        stmt.CommandType = CommandType.StoredProcedure;
                        stmt.BindByName = true;
                        foreach (ParamInfo p in this.paramsInfo)
                        {
                            if (p.mode.Equals("IN"))
                            {
                                string paramValue = this.readableParamsParsed[p.nameLower].Read(mc);
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
                                string paramValue = this.readableParamsParsed[p.nameLower].Read(mc);
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
                                    throw new Exception("Uknown parameter mode ["+p.mode+"] for param ["+p.name+"]");
                            }
                        }
                        int numRows = stmt.ExecuteNonQuery();
                        this.WriteNumRows(mc, numRows);
                        foreach (var p in this.writableParamsParsed.Keys)
                        {
                            foreach (var writable in this.writableParamsParsed[p])
                            {
                                writable.Write(mc, stmt.Parameters[p].Value.ToString());
                            }
                        }
                    }
                }
                catch (OracleException e)
                {
                    string msg = "Error calling proc: " + name + " (" + orderedParamValues.Join(",") + "). Ordered param names:" + this.orderedParamNames.Join(",") + ". Db error msg:" + e.Message;
                    this.ProcessOracleException(mc, msg, e);
                }
            }

        }
    }

    class MDbCallProcSql : MDbCallProcCommon, IModuleSetup, IModuleRun
    {
       // private SqlCommand sqlCommand;
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDbCallProcSql m = new MDbCallProcSql();
            m.CommonSetupBefore(me, mc, run);
            m.CommonSetupAfter(me, mc, run);
        }

        public override void Run(ModuleContext mc)
        {
            this.RefreshDbInfo(mc);
            List<string> orderedParamValues = new List<string>();
            using (SqlConnection conn = DbUtilsSql.GetSqlConnection(server, db, user, pw))
            {
                using (SqlCommand stmt = new SqlCommand(name, conn))
                {
                    stmt.CommandType = CommandType.StoredProcedure;
                    stmt.CommandTimeout = 0;
                    foreach (ParamInfo p in this.paramsInfo)
                    {
                        if (p.mode.Equals("IN"))
                        {
                            string paramValue = this.readableParamsParsed[p.nameLower].Read(mc);
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
                            string paramValue = this.readableParamsParsed[p.nameLower].Read(mc);
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
                        this.WriteNumRows(mc, numRows);
                        foreach (var p in this.writableParamsParsed.Keys)
                        {
                            foreach (var writable in this.writableParamsParsed[p])
                            {
                                if (stmt.Parameters[p].DbType.Equals(DbType.DateTime))
                                {
                                    System.Data.SqlTypes.SqlDateTime sqlDt = (System.Data.SqlTypes.SqlDateTime)stmt.Parameters[p].Value;
                                        DateTime dt =DateTime.Parse(sqlDt.ToString());
                                    var outValue = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");//2009-10-31 00:00:00.000
                                    writable.Write(mc, outValue);
                                }
                                else
                                {
                                    writable.Write(mc, stmt.Parameters[p].Value.ToString());
                                }
                            }
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        string msg = "Error calling proc: " + name + " (" + orderedParamValues.Join(",") + ")." +
                            "Ordered param names:" + this.orderedParamNames.Join(",") +
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
                        this.ProcessSqlException(mc, msg, e);
                    }
                }
            }

        }
    }
}




