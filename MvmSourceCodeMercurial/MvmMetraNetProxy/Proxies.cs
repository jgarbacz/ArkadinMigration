using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace Metratech.Mvm.MtProxy
{
    public class MtProxy
    {
        public string defaultRmpBin=@"D:\MetraTech\RMP\bin";

        public MtProxy()
        {
            
            // Try to load missing assemblies from RMP/bin.
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(currentDomain_AssemblyResolve);
            // The assembly resolver isn't working so error if not launching rmp/bin/*.exe 
            if (!AppDomain.CurrentDomain.BaseDirectory.ToLower().EndsWith(@"rmp\bin"))
            {
                throw new Exception("Error, cannot use metranet functionality unless the top level assembly exists in rmp/bin");
            }
        }

        /// <summary>
        /// This is intended to catch missing references and load from rmp bin. I could not get this
        /// to work on MTServerAccess.interop.dll
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Assembly currentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //This handler is called only when the common language runtime tries to bind to the assembly and fails.
	
            //Retrieve the list of referenced assemblies in an array of AssemblyName.
            Assembly MyAssembly, objExecutingAssemblies;
            string strTempAssmbPath = "";
	
            objExecutingAssemblies = Assembly.GetExecutingAssembly();
            
            AssemblyName[] arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies();
	
            // this is the name of the assembly the is failing
            string failedAssemblyName=args.Name.Substring(0, args.Name.IndexOf(","));
            //Loop through the array of referenced assembly names.
            foreach (AssemblyName strAssmbName in arrReferencedAssmbNames)
            {
                //Check for the assembly names that have raised the "AssemblyResolve" event.
                string currentAssemblyName=strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(","));
                if (currentAssemblyName.Equals(failedAssemblyName))
                {
                    //Build the path of the assembly from where it has to be loaded.
                    //The following line is probably the only line of code in this method you may need to modify:
                    //<strong>strTempAssmbPath = txtAssemblyDir.Text;</strong>
                    strTempAssmbPath = defaultRmpBin;

                    // THIS INCORRECTLY ASSUMES THAT THE ASSEMBLY_NAME IS SAME AS ASSEMBLY_FILE.dll WHICH IT 
                    // DOES NOT HAVE TO BE!
                    strTempAssmbPath = System.IO.Path.Combine(strTempAssmbPath, "MTServerAccess.interop.dll");
                    //strTempAssmbPath=System.IO.Path.Combine(strTempAssmbPath, failedAssemblyName + ".dll");
                    break;
                }
            }
            //Load the assembly from the specified path.
            MyAssembly = Assembly.LoadFrom(strTempAssmbPath);
            //Return the loaded assembly.
            return MyAssembly;
        }

        public void GetMtDbInfo(string serverType, out string databaseType, out string serverName, out string databaseName, out string userName, out string password)
        {
            var serverInfo = new global::MTSERVERACCESSLib.MTServerAccessDataSet();
            serverInfo.Initialize();
            var info = serverInfo.FindAndReturnObject(serverType);
            databaseType = info.DatabaseType;
            serverName = info.ServerName;
            databaseName = info.DatabaseName;
            userName = info.UserName;
            password = info.Password;
        }

        
    }
}
