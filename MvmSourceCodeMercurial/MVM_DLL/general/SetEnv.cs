using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace MVM
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class SetEnv
    {
        // Import the kernel32 dll.
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        [return: MarshalAs(UnmanagedType.Bool)]

        // The declaration is similar to the SDK function
        public static extern bool SetEnvironmentVariable(string lpName, string lpValue);

        public SetEnv()
        {
        }

        public static bool SetEnvVar(string environmentVariable, string variableValue)
        {
            try
            {
                // Get the write permission to set the environment variable.
                EnvironmentPermission environmentPermission = new EnvironmentPermission(EnvironmentPermissionAccess.Write, environmentVariable);
                environmentPermission.Demand();
                return SetEnvironmentVariable(environmentVariable, variableValue);
            }
            catch (SecurityException e)
            {
                Console.WriteLine("Exception:" + e.Message);
            }
            return false;
        }
    }


    class MyClass
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Create a sample environment variable and set its value (for the current process).
            SetEnv.SetEnvVar("TESTENV", "TestValue");

            // Verify that environment variable is set correctly.
            Console.WriteLine("The value of TESTENV is: " + Environment.GetEnvironmentVariable("TESTENV"));
        }
    }
}