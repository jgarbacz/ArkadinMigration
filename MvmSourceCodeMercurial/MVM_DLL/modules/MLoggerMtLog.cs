//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Xml;
//using System.Diagnostics;
//using Oracle.DataAccess.Client;
//using System.Data;
//using System.Reflection;
//using NLog;
//namespace MVM
//{

   
//    public class MLoggerMtLog: IModuleRun
//    {
//        public static Type loggerType;
//        public static object loggerInstance;

//        public MLoggerMtLog()
//        {
//            string assemblyFile= @"\\MetraTech.Common.dll";
//            string fullPath = MetraTech.Basic.Config.SystemConfig.GetBinDir() + assemblyFile;
//            Assembly assembly = Assembly.LoadFrom(fullPath);
//            loggerType = assembly.GetType("MetraTech.Logger");
//            loggerInstance = Activator.CreateInstance(loggerType,"[mvm]");
//        }

//        public MLoggerMtLog(ModuleContext mc)
//        {
//            string assemblyFile = @"\\MetraTech.Common.dll";
//            string fullPath = MetraTech.Basic.Config.SystemConfig.GetBinDir() + assemblyFile;
//            Assembly assembly = Assembly.LoadFrom(fullPath);
//            loggerType = assembly.GetType("MetraTech.Logger");
//            loggerInstance = Activator.CreateInstance(loggerType, "[AMP:" + mc.LocalName + "]");
//        }

//        public static void LogInfo(string s){
//            object[] arguments = new object[] { s };
//            loggerType.InvokeMember("LogInfo",
//                                        BindingFlags.Default | BindingFlags.InvokeMethod,
//                                        null,
//                                        loggerInstance,
//                                        arguments);
//        }


//        public static void LogDebug(string s)
//        {
//          object[] arguments = new object[] { s };
//          loggerType.InvokeMember("LogDebug",
//                                      BindingFlags.Default | BindingFlags.InvokeMethod,
//                                      null,
//                                      loggerInstance,
//                                      arguments);
//        }

//        private string msgSyntax;
//        private IReadString msgParsed;

//        private string logMessageFieldSyntax;
//        private IWriteString logMessageFieldParsed;
        
//        private string logMessageDateFieldSyntax;
//        private IWriteString logMessageDateFieldParsed;

//        private string fullMsgSyntax;
//        private IReadString fullMsgParsed;

//        LogLevel msgLevel;

//        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run, string loggerOid, string msgSyntax, LogLevel msgLevel)
//        {
//            // Evaluate the log level and return if msg isn't >= log level.
//            string logLevelStr = mc.ReadObjectField(loggerOid, "log_level");
//            if (logLevelStr.Equals("")) logLevelStr = mc.globalContext["log_level"];
//            if (logLevelStr.Equals("")) logLevelStr = "all";
//            LogLevel logLevel = mc.GetLogLevel(logLevelStr);
//            if (msgLevel < logLevel) return;
       
//            // Ok, we're going to log it, so setup the runtime log write

//            // LOGGER.log_message_field="log_message" <-- so we write the message to OBJECT.log_message
//            // LOGGER.log_message_date_field="log_message_date" <-- if this is set we'll write the time to this field
//            // LOGGER.log_column_names="acct_ext_id,serv_ext_id,mymsg"
//            // LOGGER.override_mymsg="'MYMSG='~OBJECT.log_message"
//            // LOGGER.override_acct_ext_id="'ACCT_EXT_ID='~OBJECT.acct_ext_id"
//            // LOGGER.override_serv_ext_id="'SERV_EXT_ID='~OBJECT.serv_ext_id"
//            // LOGGER.log_column_delimiter=","
//            MLoggerMtLog m = new MLoggerMtLog(mc);
//            m.msgLevel = msgLevel;
//            string logMessageField = mc.ReadObjectField(loggerOid, "log_message_field", "log_message");
//            m.logMessageFieldSyntax = "OBJECT." + logMessageField;
//            m.logMessageFieldParsed = mc.ParseWritableSyntax(m.logMessageFieldSyntax);

//            string logMessageDateField = mc.ReadObjectField(loggerOid, "log_message_date_field", "log_message_date");
//            m.logMessageDateFieldSyntax = "OBJECT." + logMessageDateField;
//            m.logMessageDateFieldParsed = mc.ParseWritableSyntax(m.logMessageDateFieldSyntax);

//            m.msgSyntax = msgSyntax;
//            m.msgParsed = mc.ParseSyntax(msgSyntax);

//            string columnDelimSyntax = mc.ReadObjectField(loggerOid, "log_column_delimiter", "','");
//            var logColumns = mc.ReadObjectField(loggerOid, "log_column_names", "log_message").Split(',');
//            List<string> columnsSyntax=new List<string>();
//            foreach (string column in logColumns)
//            {
//                string columnSyntax = mc.ReadObjectField(loggerOid, "override_" + column, "OBJECT." + column);
//                columnsSyntax.Add(columnSyntax);
//            }
//            m.fullMsgSyntax = columnsSyntax.Join("~" + columnDelimSyntax + "~");
//            m.fullMsgParsed = mc.ParseSyntax(m.fullMsgSyntax);
//            run.Add(m);
//        }

//        public void Run(ModuleContext mc)
//        {
//            // Evaluate the log message the log message
//            string logMessage=msgParsed.Read(mc);
//            string logMessageDate = mc.Now();
//            // lock to the object so we don't hit a race condition on setting the message field and
//            // writing out the full message.
//            string fullMsg;
//            lock(mc.objectData){
//                // write log message into the log message field
//                this.logMessageFieldParsed.Write(mc,logMessage);
//                this.logMessageDateFieldParsed.Write(mc, logMessageDate);
//                fullMsg = this.fullMsgParsed.Read(mc);
//            }

//            if (this.msgLevel == LogLevel.Fatal)
//            {
//                LogInfo("*** FATAL ERROR ***");
//                LogInfo(fullMsg);
//                throw new Exception(fullMsg);
//            }
//            else if (this.msgLevel == LogLevels.DEBUG)
//            {
//              LogDebug(fullMsg);
//            }
//            else
//            {
//              LogInfo(fullMsg);
//            }
//        }
           
//        public void Log(ModuleContext mc, ILogger log)
//        {
//            log.LogInfo("logger_mtlog:" + this.msgSyntax);
//        }
//    }
//}
