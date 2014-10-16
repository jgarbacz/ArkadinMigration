using System;
using NLog;

namespace MVM
{
    public class ConsoleLogger : LogHelper
    {
        public ModuleContext moduleContext;
        public string prefix = null;

        override public void writeLog(LogLevel logLevel, string msg)
        {
            if(prefix!=null)Console.Write(prefix);
            Console.WriteLine(msg);
            if (logLevel == LogLevel.Fatal) throw new Exception(msg);
        }
    }
}
