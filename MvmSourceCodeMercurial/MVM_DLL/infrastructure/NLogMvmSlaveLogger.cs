using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using NLog.Targets;

namespace MVM
{
    [Target("MvmSlaveLogger")]
    public sealed class NLogMvmSlaveLoggerTarget : TargetWithLayout
    {
        public NLogMvmSlaveLoggerTarget()
        {
        }
        protected override void Write(LogEventInfo logEvent)
        {
            string logMessage = this.Layout.Render(logEvent);
            MvmEngine.DefaultMvmEngine.SlaveLogMessage(logMessage);
        }
    }






}
