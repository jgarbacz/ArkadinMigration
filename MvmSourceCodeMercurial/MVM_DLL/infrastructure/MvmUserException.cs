using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    /// <summary>
    /// This class is so mvm user can configure different exception types without us
    /// having to generate a real exception class for each type. 
    /// </summary>
    public class MvmUserException : System.ApplicationException
    {
        public string exceptionName;
        public string exceptionMessage;
        public string message;
        public static MvmUserException Create(string exceptionName, string exceptionMessage)
        {
            string message = exceptionName + ": " + exceptionMessage;
            MvmUserException e = new MvmUserException(message);
            e.exceptionName = exceptionName;
            e.exceptionMessage = exceptionMessage;
            return e;
        }
        public static MvmUserException Create(string exceptionName, string exceptionMessage, Exception innerException)
        {
            string message = exceptionName + ": " + exceptionMessage;
            MvmUserException e = new MvmUserException(message, innerException);
            e.exceptionName = exceptionName;
            e.exceptionMessage = exceptionMessage;
            return e;
        }

        private MvmUserException(string message)
            : base(message)
        {
        }

        private MvmUserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        private StringBuilder stackTrace = new StringBuilder();
        public void StackTraceAppend(string location)
        {
            stackTrace.AppendLine(location);
        }

        /// <summary>
        /// Override so we can modify the stack trace of the exception
        /// </summary>
        public override string StackTrace
        {
            get
            {
                return this.stackTrace.ToString();
            }
        }
    }
}
