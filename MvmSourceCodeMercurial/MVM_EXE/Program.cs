using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVM;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MVM_EXE
{
    public class Program
    {
        static void MyHandler(ConsoleCtrl.ConsoleEvent consoleEvent)
        {
            Console.WriteLine("Event: {0} received at {1}", consoleEvent, DateTime.Now.ToString("yyyyMMddHHmmss.fff"));
            if (MVM.MvmEngine.DefaultMvmEngine != null)
            {
                Console.WriteLine("Trying to kill slave processes");
                MVM.MvmEngine.DefaultMvmEngine.ShutdownAbort("Looks like you did CTRL-C");
            }
            Environment.Exit(3);
        }

        static void Main(string[] args)
        {
            //TestStuff.Main();
            //MyArray.TestArrayCopyPlans();
            //Console.WriteLine("press to exit");
            //ArrayPref.Main1();
            //Console.ReadKey();
            //return;

            ConsoleCtrl cc = new ConsoleCtrl();
            cc.ControlEvent += new ConsoleCtrl.ControlEventHandler(MyHandler);
            MVM.MvmEngine.RunFromConsole(args);
        }
    }

    public static class ArrayPref
    {
        const string Format = "{0,7:0.000} ";
        public static void Main1()
        {
            TwoFer();
            //Jagged();
            //Multi();
            //Single();
        }

        static void TwoFer()
        {
            const int numTfids = 500;
            const int numUfns = 300;


            var timer = new Stopwatch();
            timer.Start();
            var jagged = new int[numTfids][];
            for (var i = 0; i < numTfids; i++)
            {
                jagged[i] = new int[numTfids];
                for (var j = 0; j < numUfns; j++)
                {
                    jagged[i][j] = 99;

                }
            }
            timer.Stop();
            Console.WriteLine(Format,
                (double)timer.ElapsedTicks / TimeSpan.TicksPerMillisecond);


            timer = new Stopwatch();
            timer.Start();
            for (var passes = 0; passes < 10; passes++)
            {
                for (int i = 0; i < 10000000; i++)
                {
                    int tfid = RandomUtils.RandomNumber(i%50, 100);
                    int x = jagged[tfid][tfid];
                }
                timer.Stop();
                Console.WriteLine(Format,
                    (double)timer.ElapsedTicks / TimeSpan.TicksPerMillisecond);
            }
        }

        static void Jagged()
        {
            const int dim = 100;
            for (var passes = 0; passes < 10; passes++)
            {
                var timer = new Stopwatch();
                timer.Start();
                var jagged = new int[dim][][];
                for (var i = 0; i < dim; i++)
                {
                    jagged[i] = new int[dim][];
                    for (var j = 0; j < dim; j++)
                    {
                        jagged[i][j] = new int[dim];
                        for (var k = 0; k < dim; k++)
                        {
                            jagged[i][j][k] = i * j * k;
                        }
                    }
                }
                timer.Stop();
                Console.Write(Format,
                    (double)timer.ElapsedTicks / TimeSpan.TicksPerMillisecond);



            }
            Console.WriteLine();
        }
        static void Multi()
        {
            const int dim = 100;
            for (var passes = 0; passes < 10; passes++)
            {
                var timer = new Stopwatch();
                timer.Start();
                var multi = new int[dim, dim, dim];
                for (var i = 0; i < dim; i++)
                {
                    for (var j = 0; j < dim; j++)
                    {
                        for (var k = 0; k < dim; k++)
                        {
                            multi[i, j, k] = i * j * k;
                        }
                    }
                }
                timer.Stop();
                Console.Write(Format,
                    (double)timer.ElapsedTicks / TimeSpan.TicksPerMillisecond);
            }
            Console.WriteLine();
        }
        static void Single()
        {
            const int dim = 100;
            for (var passes = 0; passes < 10; passes++)
            {
                var timer = new Stopwatch();
                timer.Start();
                var single = new int[dim * dim * dim];
                for (var i = 0; i < dim; i++)
                {
                    for (var j = 0; j < dim; j++)
                    {
                        for (var k = 0; k < dim; k++)
                        {
                            single[i * dim * dim + j * dim + k] = i * j * k;
                        }
                    }
                }
                timer.Stop();
                Console.Write(Format,
                    (double)timer.ElapsedTicks / TimeSpan.TicksPerMillisecond);
            }
            Console.WriteLine();
        }
    }















    // Catches CTRL-C and window closing so we can kill slave processes
    //
    // Copied from:
    // http://www.java2s.com/Tutorial/CSharp/0520__Windows/HookinguptoaWindowsCallback.htm
    class ConsoleCtrl
    {
        public enum ConsoleEvent
        {
            CTRL_C = 0,        // From wincom.h
            CTRL_BREAK = 1,
            CTRL_CLOSE = 2,
            CTRL_LOGOFF = 5,
            CTRL_SHUTDOWN = 6
        }

        public delegate void ControlEventHandler(ConsoleEvent consoleEvent);

        public event ControlEventHandler ControlEvent;

        // save delegate so the GC doesn�t collect it.
        ControlEventHandler eventHandler;

        public ConsoleCtrl()
        {
            // save this to a private var so the GC doesn't collect it
            eventHandler = new ControlEventHandler(Handler);
            SetConsoleCtrlHandler(eventHandler, true);
        }

        private void Handler(ConsoleEvent consoleEvent)
        {
            if (ControlEvent != null)
                ControlEvent(consoleEvent);
        }

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleCtrlHandler(ControlEventHandler e, bool add);
    }
}
