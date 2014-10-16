using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
namespace MVM
{
    /// <summary>
    /// This class provides reflection access to common MT dlls. This allows me to use them
    /// when the product is there. 
    /// </summary>
    public class MtReflection
    {
        private static string rmpBin=new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
        public static string RmpBin{
            get{
                return rmpBin;
            }
            set{
                rmpBin=value;
            }
        }

        /// <summary>
        /// Reads MetraNet database information using API. "Netmeter" is an example of serverType.
        /// </summary>
        /// <param name="serverType"></param>
        /// <param name="databaseType"></param>
        /// <param name="serverName"></param>
        /// <param name="databaseName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public static void GetMtDbInfo(string serverType, string rmpBin, out string databaseType, out string serverName, out string databaseName, out string userName, out string password)
        {
            if (rmpBin.IsNullOrEmpty())
            {
                rmpBin = RmpBin;
            }
            string assemblyFile = System.IO.Path.Combine(rmpBin,"MTServerAccess.interop.dll");
            Assembly assembly = Reflector.LoadAssemblyFrom(assemblyFile);
            //foreach (var m in assembly.GetExportedTypes()) Console.WriteLine(m.FullName);
            string typeName="MetraTech.Interop.MTServerAccess.MTServerAccessDataSetClass";
            Type type = assembly.GetType(typeName);
            if (type == null)  throw new Exception("Cannot find type=["+typeName+"] in assembly=["+assemblyFile+"]");
            object mtServerAccessDataSet = Reflector.CreateInstance(type);
            Reflector.CallMethod(mtServerAccessDataSet, "Initialize");
            object mtServerAccessData = Reflector.CallMethod(mtServerAccessDataSet, "FindAndReturnObject", serverType);
            databaseType = (string)Reflector.GetProperty(mtServerAccessData, "DatabaseType");
            serverName = (string)Reflector.GetProperty(mtServerAccessData, "ServerName");
            databaseName = (string)Reflector.GetProperty(mtServerAccessData, "DatabaseName");
            userName = (string)Reflector.GetProperty(mtServerAccessData, "UserName");
            password = (string)Reflector.GetProperty(mtServerAccessData, "Password");
        }
    }
}
