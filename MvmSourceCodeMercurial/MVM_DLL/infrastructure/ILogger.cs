using System;
using NLog;

namespace MVM
{
    /// <summary>
    /// Summary description for ILogger.
    /// </summary>
  public interface ILogger 
    {
        void SetLogLevel(LogLevel viewableLogLevel);
        void Log(LogLevel logLevel, string msg);
        void LogFatal(string msg);
        void LogError(string msg);
        void LogWarn(string msg);
        void LogInfo(string msg);
        void LogDebug(string msg);
        void LogTrace(string msg);
    }
}
