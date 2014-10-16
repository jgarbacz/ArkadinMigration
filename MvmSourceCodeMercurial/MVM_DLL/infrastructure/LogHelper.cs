using System;
using NLog;

namespace MVM
{
    /// <summary>
    /// Summary description for LogHelper.
    /// </summary>
  public abstract class LogHelper:ILogger
    {
        // messages with a logLevel >= viewableLogLevel will be logged
        public LogLevel viewablelogLevel;

        #region ILogger Members
    
        public abstract void writeLog(LogLevel logLevel, String msg);
        //public abstract void setModuleContext(ModuleContext moduleContext);
        //public abstract ILogger fork(ModuleContext moduleContext,ModuleContext forkedModuleContext);
    
        public void SetLogLevel(LogLevel viewableLogLevel)
        {
      this.viewablelogLevel=viewableLogLevel;
        }
        public void Log(LogLevel logLevel, String msg)
        {
      if(logLevel>=this.viewablelogLevel) this.writeLog(logLevel,msg);
        }
        public void LogFatal(String msg)
        {
            this.Log(LogLevel.Fatal, msg);
        }
        public void LogError(String msg)
        {
            this.Log(LogLevel.Error, msg);
        }
        public void LogWarn(String msg)
        {
            this.Log(LogLevel.Warn, msg);
        }
        public void LogInfo(String msg)
        {
            this.Log(LogLevel.Info, msg);
        }
        public void LogDebug(String msg)
        {
            this.Log(LogLevel.Debug, msg);
        }
        public void LogTrace(String msg)
        {
            this.Log(LogLevel.Trace, msg);
        }

        #endregion
    }
}
