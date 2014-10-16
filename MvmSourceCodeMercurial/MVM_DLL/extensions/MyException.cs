using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    static class MyException
    {
        public static string GetStackTraceRecursive(this Exception e)
        {
            StringBuilder output = new StringBuilder();
            if (e.InnerException != null)
            {
                output.AppendLine(GetStackTraceRecursive(e.InnerException));
            }
            if (e.StackTrace != null) output.AppendLine(e.Message.AppendLine()+ e.StackTrace);
            else output.AppendLine(e.Message);
            return output.ToString();
        }

    }
}
