using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
namespace MVM
{
        [TestFixture]
        public static class MvmScriptTests
        {
            private static MvmEngine _mvm;
            public static MvmEngine mvm
            {
                get
                {
                    if (_mvm == null)
                    {
                        Console.WriteLine("[[MVM]]] calling constructor");
                        _mvm = new MvmEngine();
                        Console.WriteLine("[[MVM]]] loading RMP procs");
                        _mvm.LoadRmpProcs(); 
                        Console.WriteLine("[[MVM]]] done load");
                    }
                    Console.WriteLine("[[MVM]]] returning mvm instance");
                    return _mvm;
                }
            }

            [TestFixtureSetUp]
            public static void SetUp()
            {
                Console.WriteLine("[[MVM]]]enter SetUp");
                mvm.SetupAsSuper(false);
                mvm.StartupWorkerThreads();
                mvm.InitializeGlobalNamespace();
                Console.WriteLine("[[MVM]]]exit SetUp");
            }

            [TestFixtureTearDown]
            public static void TearDown()
            {
                Console.WriteLine("[[MVM]]]enter TearDown");
                mvm.ShutdownCluster();
                mvm.Shutdown();
                Console.WriteLine("[[MVM]]]exit TearDown");
            }

            [Test]
            [TestCaseSource("FindMvmScriptTests")]
            public static void Run(string procName)
            {
                if (procName == null)
                {
                    Console.Write("[[MVM]]] no tests found");
                    Assert.Ignore("no tests found");
                    return;
                }
                var parts=procName.Split('.');
                var nameSpace = parts[0];
                var localProcName = parts[1];
                Console.Write("[[MVM]]] test " + localProcName);

                // nunit_test expects GLOBAL.unit_test_proc_name to be set
                mvm.globalContext["unit_test_proc_name"] = localProcName;
                mvm.CallProc("unit_test", "global");
                // pull the results out of GLOBAL
                string exception_name = mvm.globalContext["exception_name"];
                string exception_message = mvm.globalContext["exception_message"];
                string exception_trace = mvm.globalContext["exception_trace"];
                Console.Write("[[MVM]]] done " + localProcName);
                if (exception_name.NotNullOrEmpty())
                {
                    if (exception_name.Equals("nunit_pass"))
                    {
                        Assert.Pass(exception_message);
                    }
                    else if (exception_name.Equals("nunit_fail"))
                    {
                        Assert.Fail(exception_message);
                    }
                    else if (exception_name.Equals("nunit_inconclusive"))
                    {
                        Assert.Inconclusive(exception_message);
                    }
                    else if (exception_name.Equals("nunit_ignore"))
                    {
                        Assert.Ignore(exception_message);
                    }
                    else 
                    {
                        Assert.Fail("test through an unexpected exception: exception=["+exception_name+"] message=["+exception_message+" trace=["+exception_trace+"]");
                    }
                }
                Assert.Inconclusive("I think we need to diff an out file?");
            }

            public static List<string> FindMvmScriptTests
            {
                get
                {
                    Console.WriteLine("[[MVM]]] entering FindMvmScriptTests");
                    List<string> output = mvm.workMgr.schedulerMaster.GetMvmScriptUnitTests().Select(pi=>pi.procName).ToList();
                    if (output.Count == 0)
                    {
                        output.Add(null);
                    }
                    Console.WriteLine("[[MVM]]] exiting FindMvmScriptTests");
                    return output;
                }
            }
        }
}
