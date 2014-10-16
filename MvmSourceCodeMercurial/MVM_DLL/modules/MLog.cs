using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using NLog;

namespace MVM
{
    class MLog: IModuleSetup
    {
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> runModules)
        {

            // grab the msg syntax
            string msgSyntax=me.InnerText;
            
            // if is_literal is set, the syntax is the literal
            string msgLiteral = null;
            if (me.GetAttribute("is_literal").Equals("1"))
                msgLiteral = msgSyntax;

            // look for the globalContext logger
            string loggerOid = mc.globalContext["logger"];

            // get the log type from the logger, else use nlog
            string logType = mc.ReadObjectField(loggerOid, "log_type");
            if (string.IsNullOrEmpty(logType)) logType = "nlog";

            // see if there is a log switch. switch is higher level then log_level.
            // we only proceed if the switch is on
            string globalSwitch = me.GetAttribute("switch");
            if (!globalSwitch.IsNullOrEmpty())
            {
                if (!mc.globalContext[globalSwitch].Equals("1"))
                    return;
            }

            // Get the msg log level
            string msgLevelStr;
            if(me.LocalName.Equals("log"))
                msgLevelStr = me.GetAttributeDefaulted("level", "error");
            else if (me.LocalName.Equals("print"))
                msgLevelStr = me.GetAttributeDefaulted("level", "error");
            else
                msgLevelStr=me.LocalName;
            LogLevel msgLevel = mc.GetLogLevel(msgLevelStr);

          

            // Evaluate the log level and return if msg isn't >= log level.
            string logLevelStr = mc.ReadObjectField(loggerOid, "log_level");
            if (logLevelStr.Equals("")) logLevelStr = mc.globalContext["log_level"];
            if (logLevelStr.Equals("")) logLevelStr = "all";
            LogLevel logLevel = mc.GetLogLevel(logLevelStr);
            if (msgLevel < logLevel) return;

            // Setup using the appropriate logging type
            if (logType.Equals("nlog"))
                new MLoggerNLog().Setup(me, mc, runModules, loggerOid, msgSyntax,msgLiteral, msgLevel);
            else if (logType.Equals("console"))
                new MLoggerNLog().Setup(me, mc, runModules, loggerOid, msgSyntax, msgLiteral,msgLevel);
            else
                throw new Exception("Unknown logger log_type [" + logType + "]. Expecting one of: console, database, mtlog, nlog");
        }
        }
    }
