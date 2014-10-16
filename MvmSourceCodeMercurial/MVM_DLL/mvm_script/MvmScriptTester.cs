using System;
using Path = System.IO.Path;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using MVM;
using System.IO;

namespace MvmScript
{
    public class MvmScriptTester
    {
        public static void PrintStackTrace(Exception e)
        {
            PrintStackTraceRecursive(e);
        }

        public static void PrintStackTraceRecursive(Exception e)
        {
            if (e.InnerException != null)
            {
                PrintStackTraceRecursive(e.InnerException);
            }
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            Console.WriteLine("-".repeat(80));
        }
        public static void TestScript(string[] args)
        {
            string inputFile = "C:\\mvmscript.in.txt";
            Console.WriteLine("Parsing " + inputFile);
            string outputFile = inputFile.Replace(".in.txt", ".out.txt");
            SplitStopwatch sw = new SplitStopwatch();
            sw.Start();
            try
            {
                AstNode astNode = AstNode.ParseFileNoBuildAst(inputFile);
                sw.Stop();
                Console.WriteLine("Antlr Parse Time=" + sw.SplitMilliseconds + " ms");
                sw.Start();
                Console.WriteLine("PRETTY PRINTED1:");
                astNode.PrettyPrint();
                astNode = AstNode.BuildAst(astNode);
                Console.WriteLine("PRETTY PRINTED2:");
                astNode.PrettyPrint();
                Console.WriteLine("AST PRINTED3:");
                astNode.OuterAst.WriteToConsole();
                sw.Stop();
                Console.WriteLine("PostProcess Time=" + sw.SplitMilliseconds + " ms");
                Console.WriteLine("Total Time=" + sw.ElapsedMilliseconds + " ms");
                //Console.WriteLine(astNode.OuterAst);
                File.WriteAllText(outputFile, astNode.OuterAst);
            }
            catch (Exception e)
            {
                PrintStackTrace(e);
                string error = e.GetStackTraceRecursive();
                File.WriteAllText(outputFile, error);
            }
            finally
            {
                if (sw.IsRunning) sw.Stop();
            }
        }
        public static void TestMain(string[] args)
        {
            // read in xml write out mvm script
            string xmlTestDir = @"D:\MetraTech\SourceCode\MvmSourceCode\MVM_DLL\mvm_script\sample_xml";
            foreach (var inputFile in FileUtils2.Glob(xmlTestDir + @"\OFF.xml"))
            {
                string outputFile = inputFile.Replace(".xml", ".txt");
                string script = AstNode.ConvertXmlFileToMvmScript(inputFile);
                File.WriteAllText(outputFile, script);
            }

            //if (args.Length != 1) Console.Error.WriteLine("Usage: MvmScript <input_file>");
            string unitTestDir = @"D:\MetraTech\SourceCode\MvmSourceCode\MVM_DLL\mvm_script\unit_tests";
            foreach (var inputFile in FileUtils2.Glob(unitTestDir + @"\label.*.in.txt"))
            {
                Console.WriteLine("Parsing " + inputFile);
                string outputFile = inputFile.Replace(".in.txt", ".out.txt");
                SplitStopwatch sw = new SplitStopwatch();
                sw.Start();
                try
                {
                    
                    AstNode astNode = AstNode.ParseFileNoBuildAst(inputFile);
                    sw.Stop();
                    Console.WriteLine("Antlr Parse Time=" + sw.SplitMilliseconds + " ms");
                    sw.Start();
                    Console.WriteLine("PRETTY PRINTED1:");
                    astNode.PrettyPrint();
                    astNode = AstNode.BuildAst(astNode);
                    Console.WriteLine("PRETTY PRINTED2:");
                    astNode.PrettyPrint();
                    Console.WriteLine("AST PRINTED3:");
                    astNode.OuterAst.WriteToConsole();
                    sw.Stop();
                    Console.WriteLine("PostProcess Time=" + sw.SplitMilliseconds + " ms");
                    Console.WriteLine("Total Time=" + sw.ElapsedMilliseconds + " ms");
                    //Console.WriteLine(astNode.OuterAst);
                    File.WriteAllText(outputFile, astNode.OuterAst);
                }
                catch (Exception e)
                {
                    PrintStackTrace(e);
                    string error = e.GetStackTraceRecursive();
                    File.WriteAllText(outputFile, error);
                }
                finally
                {
                    if (sw.IsRunning) sw.Stop();
                }
            }

            // all done
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
