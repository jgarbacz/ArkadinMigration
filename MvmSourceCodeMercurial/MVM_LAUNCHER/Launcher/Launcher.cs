using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using MVM;

namespace MVM_LAUNCHER
{
    public partial class Launcher : ServiceBase
    {
        private LauncherListener listener;
        public FilesystemManager fmgr;
        public BasicLogger logger;

        public Launcher()
        {
            this.AutoLog = true;
            this.EventLog.Log = "Application";
            this.ServiceName = "MVM Listener";
            this.CanHandlePowerEvent = false;
            this.CanHandleSessionChangeEvent = false;
            this.CanPauseAndContinue = false;
            this.CanShutdown = true;
            this.CanStop = true;
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string directory = MvmClusterCommon.rmpDir + Path.DirectorySeparatorChar + "MvmListener";
            this.logger = new BasicLogger();
            this.logger.WriteLine("Listener start event");
            try
            {
                this.fmgr = new FilesystemManager(directory, this.logger);
                this.listener = new LauncherListener(this, this.logger, MvmClusterCommon.LauncherPort);
            }
            catch
            {
                this.logger.WriteLine("Listener initialization error, aborting!");
                this.Stop();
            }
        }

        protected override void OnStop()
        {
            this.logger.WriteLine("Listener stop event");
            this.logger.Close();
            this.listener.Stop();
            this.ExitCode = 0;
        }

        protected override void OnShutdown()
        {
            this.logger.WriteLine("Listener shutdown event");
            this.listener.Stop();
            this.ExitCode = 0;
        }
    }

    // This class accepts and services connections from other nodes in the cluster
    public class LauncherListener : BaseListener
    {
        public Launcher launcher;
        public BasicLogger traceFileWriter;

        public LauncherListener(Launcher launcher, BasicLogger logger, int portNo)
            : base(IPAddress.Any, portNo, "Listener")
        {
            this.launcher = launcher;
            this.traceFileWriter = logger;
        }

        protected override void HandleClient(TcpClient tcpClient)
        {
            traceFileWriter.WriteLine("Launching thread to handle new client");
            Thread thread = new Thread(new ParameterizedThreadStart(HandleCommand));
            thread.Name = "CommandHandler";
            thread.Start(tcpClient);
        }

        private void HandleCommand(object client)
        {
            TcpClient tcpClient = client as TcpClient;
            BinaryReader breader = new BinaryReader(tcpClient.GetStream());
            BinaryWriter bwriter = new BinaryWriter(tcpClient.GetStream());

            try
            {
                bool haveMoreCommands = true;
                while (haveMoreCommands)
                {
                    // Determine the type of command we have
                    ListenerCommandType commandType = (ListenerCommandType)breader.ReadInt32();

                    traceFileWriter.WriteLine("Received command: " + commandType.ToString());

                    switch (commandType)
                    {
                        case ListenerCommandType.StartNodes:
                            StartNodes(commandType, tcpClient, breader, bwriter);
                            break;
                        case ListenerCommandType.SynchronizeConfig:
                            SynchronizeConfig(commandType, tcpClient, breader, bwriter);
                            break;
                        case ListenerCommandType.LastCommand:
                            haveMoreCommands = false;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                traceFileWriter.WriteLine("Error processing commands: " + e.Message + ": " + e.StackTrace);
            }
            tcpClient.Close();
        }

        // Launch some cluster nodes
        private void StartNodes(ListenerCommandType commandType, TcpClient tcpClient, BinaryReader breader, BinaryWriter bwriter)
        {
            string superMachine = breader.ReadString();
            int mvmRunId = breader.ReadInt32();
            Sandbox sandbox = this.launcher.fmgr.GetSandbox(superMachine, mvmRunId);

            List<string[]> commands = breader.ReadListOfStringArray();
            traceFileWriter.WriteLine("Launching " + commands.Count.ToString() + " nodes for super machine " + superMachine);
            foreach (var command in commands)
            {
                // command array is [ executable, args ]
                // prefix each one with the dir -- should probably happen on the super side instead
                string exe = sandbox.rmpBinDir + Path.DirectorySeparatorChar + command[0];
                string cmd = MvmClusterCommon.GetCommand(exe, command[1]);
                traceFileWriter.WriteLine("Launching node with command: " + cmd);
                foreach (var output in MvmClusterCommon.RunCommand(exe, command[1], true))
                {
                    traceFileWriter.WriteLine("[shell output] " + output);
                }
            }
        }

        // Make sure this remote machine has the same configuration that the super does
        private void SynchronizeConfig(ListenerCommandType commandType, TcpClient tcpClient, BinaryReader breader, BinaryWriter bwriter)
        {
            string superMachine = breader.ReadString();
            int mvmRunId = breader.ReadInt32();
            Sandbox sandbox = this.launcher.fmgr.GetSandbox(superMachine, mvmRunId);
            traceFileWriter.WriteLine("Synchronizing configuration with super machine " + superMachine);

            Dictionary<string, byte[]> superFiles = breader.ReadDictionaryOfStringByteArray();
            List<string> neededFiles = new List<string>();
            foreach (var sf in superFiles)
            {
                if (!sandbox.ContainsFile(sf.Key, sf.Value))
                {
                    traceFileWriter.WriteLine("Sandbox does not have super file " + sf.Key);
                    neededFiles.Add(sf.Key);
                }
            }

            // Prune the sandbox of files it no longer needs
            foreach (var f in sandbox.EnumerateFiles())
            {
                if (!superFiles.ContainsKey(f))
                {
                    traceFileWriter.WriteLine("Sandbox must remove file " + f);
                    sandbox.RemoveFile(f);
                }
            }

            bwriter.Write(neededFiles);
            bwriter.Flush();

            Dictionary<string, byte[]> newFiles = breader.ReadDictionaryOfStringByteArray();
            foreach (var f in newFiles)
            {
                traceFileWriter.WriteLine("Sandbox receiving file " + f.Key);
                sandbox.AddOrReplaceFile(this, f.Key, f.Value, superFiles[f.Key]);
            }
        }

        protected override void OnStopListener()
        {
        }
    }

    public class BasicLogger
    {
        public TextWriter logWriter;
        public string directory;

        public BasicLogger()
        {
            this.directory = MvmClusterCommon.rmpLogDir;
            Directory.CreateDirectory(this.directory);
            this.logWriter = new StreamWriter(this.directory + Path.DirectorySeparatorChar + "MVM_LISTENER.txt");
        }

        public void WriteLine(string text)
        {
            this.logWriter.WriteLine("[" + DateTime.Now.ToString() + "] " + text);
            this.logWriter.Flush();
        }

        public void Close()
        {
            this.logWriter.Close();
        }
    }
}
