using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Metratech.Mvm.MtProxy;
namespace Metratech.Mvm.MtProxy.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            string databaseType, serverName, databaseName, userName, password;
            MtProxy mtProxy = new MtProxy();
            mtProxy.GetMtDbInfo("Netmeter", out databaseType, out serverName, out databaseName, out userName, out password);
            Console.WriteLine(databaseType);
        }
    }
}
