using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Microsoft.CSharp;
using System.Globalization;
namespace MVM
{

    public class CodeGen
    {
       

        public static void Test()
        {
            Console.WriteLine("test CodeGen");
            var dir = @"c:\";
            var codeFile = Path.Combine(dir, "test.cs");
            var assemblyFile = Path.Combine(dir, "test.dll");
            string code = @"
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{
 public class MVM
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""i am alive"");
        }
    }
}
";
            File.WriteAllText(codeFile, code);
            if (CompileDll(codeFile, new string[]{"System.dll"},assemblyFile))
            {
                Console.WriteLine("Successfully compiled:" + codeFile);
            }
            else
            {
                Console.WriteLine("CANNOT compile:" + codeFile);
            }
        }



        public static bool CompileExecutable(string sourceName,string[] references, string assemblyName)
        {
            FileInfo sourceFile = new FileInfo(sourceName);
            CodeDomProvider provider = null;
            bool compileOk = false;

            // Select the code provider based on the input file extension.
            if (sourceFile.Extension.ToUpper(CultureInfo.InvariantCulture) == ".CS")
            {
                provider = CodeDomProvider.CreateProvider("CSharp");
            }
            else if (sourceFile.Extension.ToUpper(CultureInfo.InvariantCulture) == ".VB")
            {
                provider = CodeDomProvider.CreateProvider("VisualBasic");
            }
            else
            {
                Console.WriteLine("Source file must have a .cs or .vb extension");
            }

            if (provider != null)
            {
                CompilerParameters cp = new CompilerParameters();
                cp.ReferencedAssemblies.AddRange(references);

                // Generate an executable instead of 
                // a class library.
                cp.GenerateExecutable = true;

                // Specify the assembly file name to generate.
                cp.OutputAssembly = assemblyName;

                // Save the assembly as a physical file.
                cp.GenerateInMemory = false;

                // Set whether to treat all warnings as errors.
                cp.TreatWarningsAsErrors = false;

                // Invoke compilation of the source file.
                CompilerResults cr = provider.CompileAssemblyFromFile(cp,
                    sourceName);

                if (cr.Errors.Count > 0)
                {
                    // Display compilation errors.
                    Console.WriteLine("Errors building {0} into {1}",
                        sourceName, cr.PathToAssembly);
                    foreach (CompilerError ce in cr.Errors)
                    {
                        Console.WriteLine("  {0}", ce.ToString());
                        Console.WriteLine();
                    }
                }
                else
                {
                    // Display a successful compilation message.
                    Console.WriteLine("Source {0} built into {1} successfully.",
                        sourceName, cr.PathToAssembly);
                }

                // Return the results of the compilation.
                if (cr.Errors.Count > 0)
                {
                    compileOk = false;
                }
                else
                {
                    compileOk = true;
                }
            }
            return compileOk;
        }
        public static bool CompileDll(String sourceName, string[] references, string assemblyName)
        {
            string compilerOutput;
            var status= CompileDll(sourceName, references, assemblyName, null, out compilerOutput);
            Console.WriteLine(compilerOutput);
            return status;
        }

        // return true if it works, else false;
        public static bool CompileDll(String sourceName, string[] references,string assemblyName, string compilerOptions, out string compilerOutput)
        {
            compilerOutput = "";
            StringBuilder errorOut = new StringBuilder();
            FileInfo sourceFile = new FileInfo(sourceName);
            CodeDomProvider provider = null;
            bool compileOk = false;

            // Select the code provider based on the input file extension.
            if (sourceFile.Extension.ToUpper(CultureInfo.InvariantCulture) == ".CS")
            {
                //provider = CodeDomProvider.CreateProvider("CSharp"); 
                provider = new Microsoft.CSharp.CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
            }
            else if (sourceFile.Extension.ToUpper(CultureInfo.InvariantCulture) == ".VB")
            {
                provider = CodeDomProvider.CreateProvider("VisualBasic");
            }
            else
            {
                compilerOutput="Source file must have a .cs or .vb extension";
                return false;
            }

            if (provider != null)
            {
                CompilerParameters cp = new CompilerParameters();
                cp.ReferencedAssemblies.AddRange(references);

                if (compilerOptions != null) cp.CompilerOptions = compilerOptions;
                
                // Generate a class library instead of 
                // a executable.
                cp.GenerateExecutable = false;

                // Specify the assembly file name to generate.
                cp.OutputAssembly = assemblyName;

                // Save the assembly as a physical file.
                cp.GenerateInMemory = false;

                // Set whether to treat all warnings as errors.
                cp.TreatWarningsAsErrors = false;

                // Invoke compilation of the source file.
                CompilerResults cr = provider.CompileAssemblyFromFile(cp,sourceName);

                if (cr.Errors.Count > 0)
                {
                    compilerOutput += "Errors building " + sourceName + " into " + cr.PathToAssembly + "".AppendLine();
                    foreach (CompilerError ce in cr.Errors)
                    {
                        compilerOutput += "\t" + ce.ToString() + "".AppendLine();
                    }
                }
                else
                {
                    compilerOutput += "Source " + sourceName + " built into " + cr.PathToAssembly + " successfully.".AppendLine();
                }

                // Return the results of the compilation.
                if (cr.Errors.Count > 0)
                {
                    compileOk = false;
                }
                else
                {
                    compileOk = true;
                }
            }
            return compileOk;
        }
       
    }
}
