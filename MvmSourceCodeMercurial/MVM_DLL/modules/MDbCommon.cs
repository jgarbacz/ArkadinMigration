using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;

/*
Supports these tags
<login_object>GLOBAL.login</login_object>
<alias>'myalias'</alias>
<num_rows>OBJECT.database_num_rows</num_rows>
<exception>OBJECT.database_exception</exception>
*/

namespace MVM
{
    abstract class MDbCommon : BaseModuleSetup
    {
        // name or table proc etc
        public string nameSyntax;
        public IReadString nameParsed;
        public string name;

        // alias
        public string aliasSyntax;
        public IReadString aliasParsed;
        public string alias;

        // num rows
        public List<string> numRowsSyntaxList = new List<string>();
        public List<IWriteString> numRowsParsedList = new List<IWriteString>();

        // exceptions
        public List<string> exceptionSyntaxList = new List<string>();
        public List<IWriteString> exceptionParsedList = new List<IWriteString>();
        public List<string> firstExceptionSyntaxList = new List<string>();
        public List<IReadString> firstExceptionParsedList = new List<IReadString>();
        public int exceptionProcId = -1;
        public string exceptionSeveritySyntax;
        public IWriteString exceptionSeverityParsed;
        public string exceptionStateSyntax;
        public IWriteString exceptionStateParsed;

        // db info
        public DbInfo dbInfo;
        public string type;
        public string server;
        public string db;
        public string user;
        public string pw;
        public string logLevel;
        public string exceptionProc;

        public XmlElement SelectTypedElem(XmlElement me,string tagName){
            var myElem = me.SelectSingleElem(tagName + "[@type='" + this.type + "']");
            if (myElem != null) return myElem;
            var anyElem = me.SelectSingleElem(tagName+"[not(type)]");
            return anyElem;
        }

        public void ParseDbCommon(XmlElement me, ModuleContext mc, string nameSyntax)
        {
            this.dbInfo = new DbInfo(me, mc);
            this.nameSyntax = nameSyntax;
            if (this.nameSyntax == null || this.nameSyntax.Equals(""))
            {
                this.nameSyntax = me.InnerText;
                this.aliasSyntax = this.nameSyntax;
            }
            else
            {
                this.aliasSyntax = me.SelectNodeInnerText("./alias", this.nameSyntax);
            }
            this.nameParsed = mc.ParseSyntax(this.nameSyntax);
            this.aliasParsed = mc.ParseSyntax(this.aliasSyntax);
            this.name = this.nameParsed.Read(mc);
            this.alias = this.aliasParsed.Read(mc);

            // setup where we write exception state and syntax
            this.exceptionSeveritySyntax = "TEMP.database_exception_severity";
            this.exceptionSeverityParsed = mc.ParseWritableSyntax(this.exceptionSeveritySyntax);
            this.exceptionStateSyntax = "TEMP.database_exception_state";
            this.exceptionStateParsed = mc.ParseWritableSyntax(this.exceptionStateSyntax);

            // setup where we write num rows
            string numRowsSyntax = me.SelectNodeInnerText("./num_rows");
            if (numRowsSyntax != null)
            {
                this.numRowsSyntaxList.Add(numRowsSyntax);
                this.numRowsParsedList.Add(mc.ParseWritableSyntax(numRowsSyntax));
            }
            else
            {
                numRowsSyntax = "TEMP.database_num_rows";
                this.numRowsSyntaxList.Add(numRowsSyntax);
                this.numRowsParsedList.Add(mc.ParseWritableSyntax(numRowsSyntax));
                numRowsSyntax = "OBJECT." + this.alias + "_num_rows";
                this.numRowsSyntaxList.Add(numRowsSyntax);
                this.numRowsParsedList.Add(mc.ParseWritableSyntax(numRowsSyntax));
                numRowsSyntax = "OBJECT.database_num_rows";
                this.numRowsSyntaxList.Add(numRowsSyntax);
                this.numRowsParsedList.Add(mc.ParseWritableSyntax(numRowsSyntax));
            }

            // setup where we write exceptions
            string exceptionSyntax = me.SelectNodeInnerText("./exception");
            if (exceptionSyntax != null)
            {
                this.exceptionSyntaxList.Add(exceptionSyntax);
                this.exceptionParsedList.Add(mc.ParseWritableSyntax(exceptionSyntax));
            }
            exceptionSyntax = "OBJECT." + this.alias + "_exception";
            this.exceptionSyntaxList.Add(exceptionSyntax);
            this.exceptionParsedList.Add(mc.ParseWritableSyntax(exceptionSyntax));
            exceptionSyntax = "OBJECT.database_exception";
            this.exceptionSyntaxList.Add(exceptionSyntax);
            this.exceptionParsedList.Add(mc.ParseWritableSyntax(exceptionSyntax));
            exceptionSyntax = "TEMP.database_exception";
            this.exceptionSyntaxList.Add(exceptionSyntax);
            this.exceptionParsedList.Add(mc.ParseWritableSyntax(exceptionSyntax));

            string firstExceptionSyntax1 = "OBJECT.first_" + this.alias + "_exception=OBJECT.first_" + this.alias + "_exception ne '' ? OBJECT.first_" + this.alias + "_exception ne '':" + this.exceptionSyntaxList[0];
            this.firstExceptionSyntaxList.Add(firstExceptionSyntax1);
            this.firstExceptionParsedList.Add(mc.ParseSyntax(firstExceptionSyntax1));
            string firstExceptionSyntax2 = "OBJECT.first_database_exception=OBJECT.first_database_exception ne '' ? OBJECT.first_database_exception ne '':" + this.exceptionSyntaxList[0];
            this.firstExceptionSyntaxList.Add(firstExceptionSyntax2);
            this.firstExceptionParsedList.Add(mc.ParseSyntax(firstExceptionSyntax2));

            // Refresh the db info
            this.RefreshDbInfo(mc);
            if (!exceptionProc.Equals(""))
            {
                this.exceptionProcId = mc.GetProcId(exceptionProc);
            }
        }

      
        public void RefreshDbInfo(ModuleContext mc)
        {
            type = this.dbInfo.GetType(mc);
            server = this.dbInfo.GetServer(mc);
            db = this.dbInfo.GetDb(mc);
            user = this.dbInfo.GetUser(mc);
            pw = this.dbInfo.GetPw(mc);
            logLevel = this.dbInfo.GetLogLevel(mc);
            if (logLevel == null || logLevel.Equals("")) logLevel = "info";
            exceptionProc = this.dbInfo.GetExceptionProc(mc);
        }

        public StaticDbLoginInfo GetStaticDbLoginInfo(ModuleContext mc)
        {
            this.RefreshDbInfo(mc);
            return new StaticDbLoginInfo(server, db, user, pw, type, exceptionProc,logLevel);
        }

        public void WriteNumRows(ModuleContext mc, int numRows)
        {
            foreach (var target in this.numRowsParsedList)
            {
                target.Write(mc, numRows.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="msg"></param>
        /// <param name="e"></param>
        public void ProcessException(ModuleContext mc, string msg, Exception e)
        {
            this.exceptionParsedList.ForEach(t => t.Write(mc, msg));
            this.firstExceptionParsedList.ForEach(s => s.Read(mc));
            if (this.exceptionProcId >= 0)
                mc.CallProcForCurrentObjectSameScope(this.exceptionProcId);
            else
                throw new Exception(msg, e);
        }

        public void ProcessOracleException(ModuleContext mc, string msg, Oracle.DataAccess.Client.OracleException e)
        {
            this.ProcessException(mc, msg, e);
        }

        public void ProcessSqlException(ModuleContext mc, string msg, SqlException e)
        {
            this.exceptionSeverityParsed.Write(mc, e.Class.ToString());
            this.exceptionStateParsed.Write(mc, e.State.ToString());
            this.ProcessException(mc, msg, e);
        }

    }


}




