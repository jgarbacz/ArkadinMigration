using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NLog;
using NLog.Targets;

namespace MVM
{
    /// <summary>
    /// An NLog target that logs to MTLogger
    /// </summary>
    [NLog.Targets.Target("MTLogger")]
    public sealed class MTLoggerTarget : NLog.Targets.TargetWithLayoutHeaderAndFooter
    {
        
        // reflection access to mtlogger
        private Type loggerType;
        private object loggerInstance;
        private void LogInfo(string msg)
        {
            loggerType.InvokeMember("LogInfo", BindingFlags.Default | BindingFlags.InvokeMethod, null, loggerInstance, new object[] { msg });
        }
        private void LogError(string msg)
        {
            loggerType.InvokeMember("LogError", BindingFlags.Default | BindingFlags.InvokeMethod, null, loggerInstance, new object[] { msg });
        }
        private void LogWarning(string msg)
        {
            loggerType.InvokeMember("LogWarning", BindingFlags.Default | BindingFlags.InvokeMethod, null, loggerInstance, new object[] { msg });
        }
        private void LogDebug(string msg)
        {
            loggerType.InvokeMember("LogDebug", BindingFlags.Default | BindingFlags.InvokeMethod, null, loggerInstance, new object[] { msg });
        }
        private void LogFatal(string msg)
        {
            loggerType.InvokeMember("LogFatal", BindingFlags.Default | BindingFlags.InvokeMethod, null, loggerInstance, new object[] { msg });
        }
        private void LogException(string msg, Exception e)
        {
            loggerType.InvokeMember("LogException", BindingFlags.Default | BindingFlags.InvokeMethod, null, loggerInstance, new object[] { msg, e });
        }

        /// <summary>
        /// Whether to log only the message, or to use the full Layout
        /// </summary>
        public bool MessageOnly { get; set; }

        /// <summary>
        /// The directory to log to with MTLogger
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// The tag to use with MTLogger which is required
        /// </summary>
        public string Tag { get; set; }

        // Path to assembly
        public string assemblyFile = "MetraTech.Common.dll";

        /// <summary>
        /// Constructor
        /// </summary>
        public MTLoggerTarget()
            : base()
        {
            this.Tag = "";
            this.MessageOnly = false;
        }

        /// <summary>
        /// ReadWriteStart the MTlogger (and write header if configured)
        /// </summary>
        protected override void InitializeTarget()
        {
            base.InitializeTarget();
            string dir= new System.IO.FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location).DirectoryName;
            string fullPath = System.IO.Path.Combine(dir, assemblyFile);
            Assembly assembly = Assembly.LoadFrom(fullPath);
            if (!string.IsNullOrEmpty(Directory) && !string.IsNullOrEmpty(Tag))
            {
                loggerType = assembly.GetType("MetraTech.Logger");
                loggerInstance = Activator.CreateInstance(loggerType, Directory, Tag);
            }
            else if (Tag != null)
            {
                loggerType = assembly.GetType("MetraTech.Logger");
                loggerInstance = Activator.CreateInstance(loggerType, Tag);
            }
            else
            {
                throw new Exception("Cannot initializeTarget MTLOGGER with null Tag");
            }
            if (Header != null)
            {
                this.LogInfo(Header.Render(NLog.LogEventInfo.CreateNullEvent()));
            }
        }

        /// <summary>
        /// FlushToFile this logger (and write footer if configured)
        /// </summary>
        protected override void CloseTarget()
        {
            if (Footer != null)
            {
                this.LogInfo(Footer.Render(NLog.LogEventInfo.CreateNullEvent()));
            }
            base.CloseTarget();
        }

        /// <summary>
        /// Write the log message to MTLogger.  LogEvents have 1-1 correspondence between NLog and MTLogger, other than Trace which is mapped to Debug in MTLogger
        /// </summary>
        /// <param name="logEvent">The log event to write</param>
        protected override void Write(NLog.LogEventInfo logEvent)
        {
            string msg;
            if (logEvent == null)
            {
                msg = "";
            }
            else if (MessageOnly)
            {
                msg = logEvent.FormattedMessage;
            }
            else
            {
                msg = Layout.Render(logEvent);
            }
            if (logEvent.Exception != null)
            {
                this.LogException(msg, logEvent.Exception);
            }
            if (logEvent.Level == NLog.LogLevel.Fatal)
            {
                this.LogFatal(msg);
            }
            else if (logEvent.Level == NLog.LogLevel.Error)
            {
                this.LogError(msg);
            }
            else if (logEvent.Level == NLog.LogLevel.Warn)
            {
                this.LogWarning(msg);
            }
            else if (logEvent.Level == NLog.LogLevel.Info)
            {
                this.LogInfo(msg);
            }
            else if (logEvent.Level == NLog.LogLevel.Debug)
            {
                this.LogDebug(msg);
            }
            else if (logEvent.Level == NLog.LogLevel.Trace)
            {
                this.LogDebug(msg);
            }
        }
    }
}
