using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Data;
using NLog;

namespace MVM
{
    class MLoggerConsole: IModuleRun
    {
        private string msgSyntax;
        private IReadString msgParsed;
        
        private string logMessageFieldSyntax;
        private IWriteString logMessageFieldParsed;

        private string logMessageDateFieldSyntax;
        private IWriteString logMessageDateFieldParsed;

        private string fullMsgSyntax;
        private IReadString fullMsgParsed;

        LogLevel msgLevel;
       

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run, string loggerOid, string msgSyntax, LogLevel msgLevel)
        {

           
            // Evaluate the log level and return if msg isn't >= log level.
            string logLevelStr = mc.ReadObjectField(loggerOid, "log_level");
            if (logLevelStr.Equals("")) logLevelStr = mc.globalContext["log_level"];
            if (logLevelStr.Equals("")) logLevelStr = "all";
            LogLevel logLevel = mc.GetLogLevel(logLevelStr);
            if (msgLevel < logLevel) return;

            // Ok, we're going to log it, so setup the runtime log write

            // LOGGER.log_message_field="log_message" <-- so we write the message to OBJECT.log_message
            // LOGGER.log_message_date_field="log_message_date" <-- if this is set we'll write the time to this field
            // LOGGER.log_column_names="acct_ext_id,serv_ext_id,mymsg"
            // LOGGER.override_mymsg="'MYMSG='~OBJECT.log_message"
            // LOGGER.override_acct_ext_id="'ACCT_EXT_ID='~OBJECT.acct_ext_id"
            // LOGGER.override_serv_ext_id="'SERV_EXT_ID='~OBJECT.serv_ext_id"
            // LOGGER.log_column_delimiter=","
            MLoggerConsole m = new MLoggerConsole();
            m.msgLevel = msgLevel;
            string logMessageField = mc.ReadObjectField(loggerOid, "log_message_field", "log_message");
            m.logMessageFieldSyntax = "OBJECT." + logMessageField;
            m.logMessageFieldParsed = mc.ParseWritableSyntax(m.logMessageFieldSyntax);

            string logMessageDateField = mc.ReadObjectField(loggerOid, "log_message_date_field", "log_message_date");
            m.logMessageDateFieldSyntax = "OBJECT." + logMessageDateField;
            m.logMessageDateFieldParsed = mc.ParseWritableSyntax(m.logMessageDateFieldSyntax);

            m.msgSyntax = msgSyntax;
            m.msgParsed = mc.ParseSyntax(msgSyntax);

            string columnDelimSyntax = mc.ReadObjectField(loggerOid, "log_column_delimiter", "','");
            var logColumns = mc.ReadObjectField(loggerOid, "log_column_names", "log_message").Split(',');
            List<string> columnsSyntax=new List<string>();
            foreach (string column in logColumns)
            {
                string columnSyntax = mc.ReadObjectField(loggerOid, "override_" + column, "OBJECT." + column);
                columnsSyntax.Add(columnSyntax);
            }
            m.fullMsgSyntax = columnsSyntax.Join("~" + columnDelimSyntax + "~");
            m.fullMsgParsed = mc.ParseSyntax(m.fullMsgSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            // Evaluate the log message the log message
            string logMessage=msgParsed.Read(mc);
            string logMessageDate = mc.Now();
            // lock to the object so we don't hit a race condition on setting the message field and
            // writing out the full message.
            string fullMsg;
            lock(mc.objectData){
                // write log message into the log message field
                this.logMessageFieldParsed.Write(mc,logMessage);
                this.logMessageDateFieldParsed.Write(mc, logMessageDate);
                fullMsg = this.fullMsgParsed.Read(mc);
            }
           
            if (this.msgLevel == LogLevel.Fatal)
            {
                Console.WriteLine("*** FATAL ERROR ***");
                Console.WriteLine(fullMsg);
                throw new Exception(fullMsg);
            }
            else
            {
                Console.WriteLine(fullMsg);
            }
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("logger_console:" + this.msgSyntax);
        }
    }
}
