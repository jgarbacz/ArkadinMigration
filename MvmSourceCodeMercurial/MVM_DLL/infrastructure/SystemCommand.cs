using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace MVM
{
    class SystemCommand
    {
          public static void RunCmd(string cmd, IList<string> args, out string stdOut, out string stdErr, out int exitCode)
        {
            List<string> escArgs = new List<string>();
            foreach (string arg in args)
            {
                string escArg = arg.Replace("\"", "\"\"");
                escArgs.Add("\"" + escArg + "\"");
            }
            string argString = escArgs.Join(" ");
            Console.WriteLine(cmd + " " + argString);
            RunCmd(cmd, argString, out stdOut, out stdErr, out exitCode);
        }


        public static void RunCmd(string cmd, string argString, out string stdOut, out string stdErr, out int exitCode)
        {

            Console.WriteLine(cmd + " " + argString);
            Process p = new Process();
            StreamReader sr;
            StreamReader err;
            ProcessStartInfo psi = new ProcessStartInfo(cmd);
            psi.Arguments = argString;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.CreateNoWindow = true;
            p.StartInfo = psi;
            p.Start();
            sr = p.StandardOutput;
            err = p.StandardError;
            stdOut = sr.ReadToEnd();
            stdErr = err.ReadToEnd();
            p.WaitForExit();
            exitCode = p.ExitCode;
        }
    }
    
}
