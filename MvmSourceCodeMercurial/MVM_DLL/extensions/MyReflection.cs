using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.IO;


namespace MVM
{
    public static class MyReflection
    {
        /**
         * Test method for this class
         */
        public static void Test()
        {
            Assembly aa = Assembly.GetExecutingAssembly();
              foreach (var type in aa.GetTypes()){
                  foreach (var m in type.GetMethods())
                  {
                      Console.WriteLine("method:"+m.Name);
                      foreach (var p in m.GetParameters())
                      {
                          Console.WriteLine("..param:" + p.Name);
                      }
                  }
              }
            
            return;
#if false
            if (1 == 1) return;
            Assembly assembly = GetAssembly("ade.exe");
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass)
                {
                    Console.WriteLine("CLASS=" + type.ToString());
                    foreach (var a in type.GetCustomAttributes(true))
                    {
                        Console.WriteLine("..ATTR=" + a.ToString());
                        if (a is ModuleAttribute)
                        {
                            ModuleAttribute moduleAttribute = (ModuleAttribute)a;
                            Console.WriteLine(moduleAttribute.xmlString);
                        }
                    }
                }
            }
#endif
        }


        /// <summary>
        /// Extracts an embedded file out of a given assembly.
        /// </summary>
        /// <param name="assemblyName">The namespace of you assembly.</param>
        /// <param name="fileName">The name of the file to extract.</param>
        /// <returns>A stream containing the file data.</returns>
        public static Stream GetEmbeddedFile(string assemblyName, string fileName)
        {
            try
            {
                System.Reflection.Assembly a = System.Reflection.Assembly.Load(assemblyName);
                Stream str = a.GetManifestResourceStream(assemblyName + "." + fileName);
                if (str == null)
                    throw new Exception("Could not locate embedded resource '" + fileName + "' in assembly '" + assemblyName + "'");
                return str;
            }
            catch (Exception e)
            {
                throw new Exception(assemblyName + ": " + e.Message);
            }
        }

        public static Stream GetEmbeddedFile(this System.Reflection.Assembly a, string fileName)
        {
            try
            {
                Stream str = a.GetManifestResourceStream(fileName);
                if (str == null)
                    throw new Exception("Could not locate embedded resource '" + fileName + "' in assembly '" + a.FullName + "'");
                return str;
            }
            catch (Exception e)
            {
                throw new Exception(a.FullName + ": " + e.Message);
            }
        }

        public static Stream GetEmbeddedFile(Type type, string fileName)
        {
            string assemblyName = type.Assembly.GetName().Name;
            return GetEmbeddedFile(assemblyName, fileName);
        }

        public static void PrintEmbeddedResources()
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            string[] names = myAssembly.GetManifestResourceNames();
            foreach (string name in names)
            {
                Console.WriteLine(name);
            }
        }


        /**
         * Returns the assembly name for the current process
         */
        public static string GetAssemblyName()
        {
            Process p = Process.GetCurrentProcess();
            string assemblyName = p.ProcessName + ".exe";
            return assemblyName;
        }

        /**
         * Returns the assembly for the current process
         */
        public static Assembly GetAssembly()
        {
            return GetAssembly(GetAssemblyName());
        }

        /**
         * Returns the assembly for the current process
         */
        public static Assembly GetAssembly(string assemblyName)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyName);
            return assembly;
        }

    }
}
