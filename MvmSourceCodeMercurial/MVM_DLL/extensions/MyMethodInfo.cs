using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace MVM
{
    public static class MyMethodInfo
    {
        public static bool IsVoid(this MethodInfo methodInfo){
            return methodInfo.ReturnType.Equals(typeof(void));
        }
    }
}
