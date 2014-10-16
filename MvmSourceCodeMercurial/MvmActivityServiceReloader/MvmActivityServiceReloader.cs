using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MetraTech.Custom.Services.Mvm.MvmActivityService.ClientProxies;

namespace Metratech.MvmActivityServiceReloader
{
    class MvmActivityServiceReloader
    {
        static void Main(string[] args)
        {
            MvmActivityService_Reload_Client client = new MvmActivityService_Reload_Client();
            client.UserName = "su";
            client.Password = "su123";
            Console.WriteLine("Calling mvm reload service");
            client.Invoke();
            Console.WriteLine("Done, press any key to exit");
            Console.ReadKey();
        }
    }
}
