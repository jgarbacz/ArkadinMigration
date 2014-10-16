using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Reflection;

namespace MVM_LAUNCHER
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            bool isServiceInstalled = DoesServiceExist("NCWatchService", Environment.MachineName);

            if (args.Length > 0)
            {
                if (args[0].Trim().ToLower() == "/i" || args[0].Trim().ToLower() == "-i")
                {
                    // Install the service
                    if (!isServiceInstalled)
                    {
                        System.Configuration.Install.ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                    }
                }
                else if (args[0].Trim().ToLower() == "/u" || args[0].Trim().ToLower() == "-u")
                {
                    // Uninstall the service
                    if (!isServiceInstalled)
                    {
                        System.Configuration.Install.ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                    }
                }
                else
                {
                    Console.WriteLine("Only /i (install) and /u (uninstall) arguments are allowed.");
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new Launcher() };
                ServiceBase.Run(ServicesToRun);
            }
        }

        static bool DoesServiceExist(string serviceName, string machineName)
        {
            ServiceController[] services = ServiceController.GetServices(machineName);
            var service = services.FirstOrDefault(s => s.ServiceName == serviceName);
            return service != null;
        }
    }
}
