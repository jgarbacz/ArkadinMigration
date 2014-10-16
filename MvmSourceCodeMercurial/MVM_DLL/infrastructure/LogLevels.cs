using System;
using System.Collections;
using NLog;

namespace MVM
{
    /// <summary>
    /// Summary description for LogLevels.
    /// </summary>
    public class LogLevels
    {
    public static Hashtable logLevelMap=new Hashtable();

        // Now we've standardized on NLog logging levels
        static LogLevels()
        {
            logLevelMap["FATAL"] = LogLevel.Fatal;
            logLevelMap["ERROR"] = LogLevel.Error;
            logLevelMap["WARNING"] = LogLevel.Warn;
            logLevelMap["WARN"] = LogLevel.Warn;
            logLevelMap["INFO"] = LogLevel.Info;
            logLevelMap["DEBUG"] = LogLevel.Debug;
            logLevelMap["TRACE"] = LogLevel.Trace;
            logLevelMap["FINE"] = LogLevel.Trace;
            logLevelMap["FINER"] = LogLevel.Trace;
            logLevelMap["FINEST"] = LogLevel.Trace;
            logLevelMap["ALL"] = LogLevel.Trace;
        }

        public static LogLevel GetLogLevel(string name)
        {
            return (LogLevel)logLevelMap[name.ToUpper()];
        }

        public static bool HasLogLevel(string name)
        {
            return logLevelMap.Contains(name.ToUpper());
        }
    }
}
