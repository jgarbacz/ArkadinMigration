using System;
using System.Collections.Generic;
using Oracle.DataAccess.Client;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;

namespace MVM
{
    [Module(@"
        <module_config>
            <name>db_select</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType>
                    <xs:sequence>
                        <xs:group ref='db_info_group'/>
                        <xs:element name='query' type='query_type' minOccurs='1' maxOccurs='unbounded' datatype='string' mode='in' description='query to run'/>
                        <xs:element name='timeout' type='xs:string' minOccurs='0' maxOccurs='1' datatype='integer' mode='in' description='query timeout in seconds'/>
                        <xs:element name='num_retries' type='xs:string' minOccurs='0' maxOccurs='1' datatype='integer' mode='in' description='number of times to retry query'/>
                        <xs:element name='retry_sleep' type='xs:string' minOccurs='0' maxOccurs='1' datatype='integer' mode='in' description='sleep up to this many ms between retries'/>
                        <xs:element name='from_table' type='xs:string' minOccurs='0' maxOccurs='unbounded' datatype='string' mode='in' description='Causes the query to return delta object that know they came from this table so they can be easily altered and persisted back.'/>                        
                        <xs:group ref='cursor_operation_group'/>
                    </xs:sequence>
                </xs:complexType>
            </xsd>
            <doc>
                <category>Database Access</category>
                <desc>Runs a database query and returns the results</desc>
                <description>Runs a select-style query and returns the set of output rows. For queries that do not return rows (e.g. DML or DDL), use db_execute.</description>
            </doc>
        </module_config>
    ")]
    class MDbSelect : MDbCommon, IModuleSetup
    {
        // setup looks at the current db info, and does either oracle or sql
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDbSelect m = new MDbSelect();
            m.ParseDbCommon(me, mc, "'db_select'");

            if (m.type.EqualsIgnoreCase("oracle"))
            {
                new MDbSelectOra().Setup(me, mc, run);
            }
            else if (m.type.EqualsIgnoreCase("sql"))
            {
                new MDbSelectSql().Setup(me, mc, run);
            }
            else
            {
                throw new Exception("unexpected db type=[" + m.type + "]");
            }
        }
    }

    abstract class MDbSelectCommon : MDbCommon, IModuleRun
    {
        protected string cursorSyntax;
        protected IWriteString cursorParsed;
        protected string querySyntax;
        protected string queryString;
        protected string timeoutSyntax;
        protected string numRetriesSyntax;
        protected string retrySleepSyntax;
        protected int timeoutInt;
        protected int numRetriesInt;
        protected int retrySleepInt;
        protected string bindQueryString;
        protected Dictionary<string, string> bindsSyntax;
        protected Dictionary<string, IReadString> bindsParsed = new Dictionary<string, IReadString>();
        protected CursorSetupCommon cursorSetup;
        protected List<string> fromTables;
        protected void CommonSetupBefore(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            if (me.HasChildElement("from_table"))
            {
                this.cursorSetup = new CursorSetupCommon(me, mc);
                this.fromTables = new List<string>();
                foreach (var xmlElement in me.SelectElements("./from_table"))
                {
                    string fromTable=mc.SyntaxReadString(xmlElement.InnerText);
                    this.fromTables.Add(fromTable);
                }
                this.cursorSetup.cursorTypeDefault = "erd";
                this.cursorSetup.cursorTypeFeedbackNameDefault = mc.GetGenSym(fromTables.JoinStrings(","));
            }
            else
            {
                this.cursorSetup = new CursorSetupCommon(me, mc);
            }
            this.ParseDbCommon(me, mc, "'db_select'");
            this.querySyntax = this.SelectTypedElem(me, "query").InnerText;
            this.cursorSyntax = me.SelectNodeInnerText("cursor", "TEMP.csr");
            this.cursorParsed = mc.ParseWritableSyntax(this.cursorSyntax);
            this.queryString = mc.SyntaxReadString(this.querySyntax);
            this.timeoutSyntax = me.SelectNodeInnerText("timeout", "0");
            this.timeoutInt = mc.SyntaxReadString(this.timeoutSyntax).ToInt();
            this.numRetriesSyntax = me.SelectNodeInnerText("num_retries", "0");
            this.numRetriesInt = mc.SyntaxReadString(this.numRetriesSyntax).ToInt();
            this.retrySleepSyntax = me.SelectNodeInnerText("retry_sleep", "0");
            this.retrySleepInt = mc.SyntaxReadString(this.retrySleepSyntax).ToInt();
        }
        protected void CommonSetupAfter(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            // log message are static since the query is static
            string beginMsg = mc.SyntaxReadString("'call " + mc.LocalName + ": '~" + this.querySyntax);
            string endMsg = mc.SyntaxReadString("'done " + mc.LocalName + ": '~" + this.querySyntax);

            // Add this runtime select newModule
            // add logging and main newModule
            if (logLevel == null || logLevel.Equals("")) logLevel = "debug";
            run.Add(mc.GetLogModule(beginMsg, logLevel, true));
            run.Add(this);
            this.cursorSetup.AddCursorSubProcs(me, mc, run);
            run.Add(mc.GetLogModule(endMsg, logLevel, true));
        }
        public abstract void Run(ModuleContext mc);
    }

    public enum OracleType { _string, _datetime, _int32, _int64, _decimal, _double, _binary };

    class MDbSelectOra : MDbSelectCommon, IModuleSetup, IModuleRun
    {
        private OracleCommand oracleCommand;
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDbSelectOra m = new MDbSelectOra();
            m.CommonSetupBefore(me, mc, run);

            // strip newlines
            m.queryString = m.queryString.Replace("\r\n", " ");
            m.queryString = m.queryString.Replace("\n", " ");

            // Parse out $${} notation for bind variables.
            m.bindQueryString = mc.TranslateDbBinds(m.queryString, out m.bindsSyntax, ":");
            m.oracleCommand = new OracleCommand();
            m.oracleCommand.BindByName = true;
            m.oracleCommand.CommandText = m.bindQueryString;
            if (m.timeoutInt > 0)
            {
                m.oracleCommand.CommandTimeout = m.timeoutInt;
            }
            foreach (var entry in m.bindsSyntax)
            {
                m.bindsParsed[entry.Key] = mc.ParseSyntax(entry.Value);
                OracleParameter param = new OracleParameter(entry.Key, OracleDbType.NVarchar2);
                m.oracleCommand.Parameters.Add(param);
            }
            m.CommonSetupAfter(me, mc, run);
        }

        public override void Run(ModuleContext mc)
        {
            this.RefreshDbInfo(mc);
            OracleConnection conn = DbUtilsOra.GetOraConnection(db, user, pw);
            try
            {
                this.oracleCommand.Connection = conn;
                // set bind parameters
                foreach (var entry in this.bindsParsed)
                {
                    string paramName = entry.Key;
                    string paramValue = entry.Value.Read(mc);
                    this.oracleCommand.Parameters[paramName].Value = paramValue;
                }
                OracleDataReader reader = null;
                for (int retry = 0; retry < this.numRetriesInt + 1; retry++)
                {
                    if (retry > 0 && this.retrySleepInt > 0)
                    {
                        System.Threading.Thread.Sleep(RandomUtils.RandomNumber(0, this.retrySleepInt));
                    }
                    try
                    {
                        reader = this.oracleCommand.ExecuteReader();
                        break;
                    }
                    catch (Exception e)
                    {
                        if (retry == this.numRetriesInt)
                        {
                            throw;
                        }
                        mc.mvm.Log("Error after attempt " + retry.ToString() + " running query string=[" + this.oracleCommand.CommandText + "] using connString=[" + conn.ConnectionString + "].".AppendLine() + "Error msg=" + e.Message);
                    }
                }
                reader.FetchSize = 2000000;
                List<string> fieldNames = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string fieldName = reader.GetName(i).ToLower();
                    fieldNames.Add(fieldName);
                }
                var csr = new OraDbCursor(mc, this.cursorSetup, fieldNames, reader, this.oracleCommand, conn,this.fromTables);
            }
            catch (Exception e)
            {
                throw new Exception("Error, running query string=[" + this.oracleCommand.CommandText + "] using connString=[" + conn.ConnectionString + "]".AppendLine() + "Error msg=" + e.Message, e);
            }
        }

        public static Dictionary<Type, OracleType> OracleTypeTypeCaseMap = new Dictionary<Type, OracleType>();
        static MDbSelectOra()
        {
            OracleTypeTypeCaseMap[typeof(string)] = OracleType._string;
            OracleTypeTypeCaseMap[typeof(DateTime)] = OracleType._datetime;
            OracleTypeTypeCaseMap[typeof(byte[])] = OracleType._binary;
            OracleTypeTypeCaseMap[typeof(Int32)] = OracleType._int32;
            OracleTypeTypeCaseMap[typeof(Int64)] = OracleType._int64;
            OracleTypeTypeCaseMap[typeof(decimal)] = OracleType._decimal;
            OracleTypeTypeCaseMap[typeof(double)] = OracleType._double;
        }
        public static OracleType OracleGetTypeCase(Type t)
        {
            OracleType typeInt;
            if (OracleTypeTypeCaseMap.TryGetValue(t, out typeInt)) return typeInt;
            //return 0;
            throw new Exception("Error, unhandled type in Oracle DbSelect [" + t.FullName + "]");
        }
    }

    class MDbSelectSql : MDbSelectCommon, IModuleSetup, IModuleRun
    {
        private SqlCommand sqlCommand;
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDbSelectSql m = new MDbSelectSql();
            m.CommonSetupBefore(me, mc, run);

            // Parse out $${} notation for bind variables.
            m.bindQueryString = mc.TranslateDbBinds(m.queryString, out m.bindsSyntax, "@");
            m.sqlCommand = new SqlCommand();
            m.sqlCommand.CommandText = m.bindQueryString;
            m.sqlCommand.CommandTimeout = m.timeoutInt;
            foreach (var entry in m.bindsSyntax)
            {
                m.bindsParsed[entry.Key] = mc.ParseSyntax(entry.Value);
                SqlParameter param = new SqlParameter(entry.Key, SqlDbType.NVarChar);
                m.sqlCommand.Parameters.Add(param);
            }

            m.CommonSetupAfter(me, mc, run);
        }

        public override void Run(ModuleContext mc)
        {
            this.RefreshDbInfo(mc);
            SqlConnection conn = DbUtilsSql.GetSqlConnection(server, db, user, pw);
            try
            {
                this.sqlCommand.Connection = conn;
                // set bind parameters
                foreach (var entry in this.bindsParsed)
                {
                    string paramName = entry.Key;
                    string paramValue = entry.Value.Read(mc);
                    this.sqlCommand.Parameters[paramName].Value = paramValue;
                }
                SqlDataReader reader = null;
                for (int retry = 0; retry < this.numRetriesInt + 1; retry++)
                {
                    if (retry > 0 && this.retrySleepInt > 0)
                    {
                        System.Threading.Thread.Sleep(RandomUtils.RandomNumber(0, this.retrySleepInt));
                    }
                    try
                    {
                        reader = this.sqlCommand.ExecuteReader();
                        break;
                    }
                    catch (Exception e)
                    {
                        if (retry == this.numRetriesInt)
                        {
                            throw;
                        }
                        mc.mvm.Log("Error after attempt " + retry.ToString() + " running query string=[" + queryString + "] using connString=[" + conn.ConnectionString + "].".AppendLine() + "Sql error msg=" + e.Message);
                    }
                }
                List<string> fieldNames = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string fieldName = reader.GetName(i).ToLower();
                    fieldNames.Add(fieldName);
                }
                var csr = new SqlDbCursor(mc, this.cursorSetup, fieldNames, reader, this.sqlCommand, conn,this.fromTables);
            }
            catch (Exception e)
            {
                throw new Exception("Error, running query string=[" + queryString + "] using connString=[" + conn.ConnectionString + "].".AppendLine() + "Sql error msg=" + e.Message, e);
            }
        }
    }
}
