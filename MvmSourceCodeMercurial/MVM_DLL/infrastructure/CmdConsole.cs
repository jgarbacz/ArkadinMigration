using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace MVM
{

    /**
     * This class encapsulates an cmd.exe console. It is an easy way to issue
     * and capture the output of multiple commands. The key application for this
     * is when you need state (env vars, current dir) perserved between command calls
     * and when you need to see the results before desideding what command to enter 
     * next.
     * 
     * TODO: This class set redirects std output to std error. In newOrder to service both std out/err I need to 
     * spawn a threadContext for each stream that will read the stream into it own buffer, so i can look at that buffer
     * from the main threadContext without blocking. The C# API doesn't give me a non-block access.
     * 
     */
    public class CmdConsole : IDisposable
    {
        private const string ECHO_STRING = "\"_THIS_IS_A_DUMMY_STRING_#^or83939asdfnxnvif7y-978yasdfuhadfpeu8y[e7y\"";
        private static readonly string EchoStdOut = "echo " + ECHO_STRING;
        private static readonly string EchoStdErr = EchoStdOut + " >&2";

        private StreamReader stdOutReader;
        private StreamReader stdErrReader;
        private StreamWriter stdInWriter;

        /**
         * Test method for the class
         */
        public static void Test()
        {
            using (CmdConsole cmd = new CmdConsole())
            {
                Console.WriteLine("Test using: cmd.Execute(command, out stdOut, out stdErr)");
                string stdOut;
                string stdErr;
                string[] commands = new string[]{
                    "dir",
                    "echo hi rob",
                    "echo OFF",
                    "echo this is with echo off1",
                    "echo this is with echo off2",
                    "echo ON",
                    "echo this is with echo back on"
                };
                foreach (string command in commands)
                {
                    cmd.Execute(command, out stdOut, out stdErr);
                    Console.WriteLine("COMMAND: [" + command + "]");
                    Console.WriteLine("STDOUT: [" + stdOut + "]");
                }
                Console.WriteLine("Test using: cmd.Execute(command) to get an iterator");
                foreach (string line in cmd.Execute("dir"))
                {
                    Console.WriteLine("LINE=["+line+"]");
                }
            }
        }



        /**
         * Creates a new instance of the command console
         */
        public CmdConsole()
        {
            string cmdExe = string.Format(System.Globalization.CultureInfo.InvariantCulture, @"{0}\cmd.exe", new object[] { Environment.SystemDirectory });
            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo(cmdExe);
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.RedirectStandardInput = true;
            //psi.WindowStyle = ProcessWindowStyle.Minimized;
            //psi.CreateNoWindow = false;
            p.StartInfo = psi;
            p.Start();
            stdOutReader = p.StandardOutput;
            stdErrReader = p.StandardError;
            stdInWriter = p.StandardInput;
            stdInWriter.AutoFlush = true;
            this.SkipCmdPreamble();
        }

        // Skip any default stuff that is printed when you open cmd.
        private void SkipCmdPreamble()
        {
            //Console.WriteLine("___BEGIN_SKIP_PREAMBLE__");
            stdInWriter.WriteLine(EchoStdOut);
            string outputLine;
            while (GetOutputLine(this.stdOutReader, out outputLine)) ;
            //Console.WriteLine("___END_SKIP_PREAMBLE__");
        }


        // Issues a command to execute and provides an Iterator for the command's stdout
        // If you are not redirecting std error, it gets lost.
        public IEnumerable<string> Execute(string command)
        {
            stdInWriter.WriteLine(command);
            stdInWriter.WriteLine(EchoStdOut);
            string commandLine = stdOutReader.ReadLine();
            //Console.WriteLine("READ_ISSUE_COMMAND--" + commandLine);
            string outputLine;
            while (GetOutputLine(this.stdOutReader, out outputLine))
            {
                 yield return outputLine; 
            }
        }


        // Issues a command and returns stdOut/Err
        public void Execute(string command, out string stdOut, out string stdErr)
        {
            StringBuilder stdOutSb = new StringBuilder();
            StringBuilder stdErrSb = new StringBuilder();
            //Console.WriteLine("ISSUE_COMMAND:" + command);    
            stdInWriter.WriteLine(command);
            stdInWriter.WriteLine(EchoStdOut);
            string commandLine = stdOutReader.ReadLine();
            //Console.WriteLine("READ_ISSUE_COMMAND--" + commandLine);
            string outputLine;
            while (GetOutputLine(this.stdOutReader, out outputLine))
            {
                stdOutSb.AppendLine(outputLine); //maybe dont keep the final newline?
            }
            stdOut = stdOutSb.ToString();
            if (stdOut.Length > 0) stdOut = stdOut.StripTrailingLineSeparator();
            stdErr = stdErrSb.ToString();
            if (stdOut.Length > 0) stdErr = stdErr.StripTrailingLineSeparator();
        }

        // assumes an echo has been issued. Stops when it eats the echo result.
        // sets output line and returns false when no more output.
        private static bool GetOutputLine(StreamReader reader, out string outputLine)
        {
            outputLine = reader.ReadLine();
            if (outputLine.Contains(EchoStdOut))
            {
                //Console.WriteLine("READ_ECHO_ISSUE--" + outputLine);
                if (AtPrompt(outputLine))
                {
                    outputLine = reader.ReadLine();
                    //Console.WriteLine("READ_ECHO_RESULT_ON1--" + outputLine);
                    outputLine = reader.ReadLine();
                    //Console.WriteLine("READ_ECHO_RESULT_ON2--" + outputLine);
                }
                else
                {
                    outputLine = reader.ReadLine();
                    //Console.WriteLine("READ_ECHO_RESULT_OFF--" + outputLine);
                }
                return false;
            }
            else
            {
                //Console.WriteLine("READ_STD_OUT-" + outputLine);
                return true;
            }
        }

        // Returns true if line is at a prompt.
        private static bool AtPrompt(string line)
        {
            bool atPrompt = line.matches(@"^[A-Z]:\\.*>") ? true : false;
            return atPrompt;
        }

        // Closes the CmdConsole, freeing up resources.
        public void Close()
        {
            if (this.stdInWriter != null)
            {
                this.stdInWriter.Close();
                this.stdInWriter = null;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Close();
        }

        #endregion

       
    }
}
