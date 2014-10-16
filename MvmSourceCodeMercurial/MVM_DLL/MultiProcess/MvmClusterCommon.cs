using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

// This is for code that needs to be shared between MVM and the listener

namespace MVM
{
    public enum ListenerCommandType { StartNodes, SynchronizeConfig, LastCommand };

    public class MvmClusterCommon
    {
        public static int DefaultPortStart = 50000;
        public static int DefaultPortEnd = 50099;

        public static int LauncherPort = 50101;

        public static string GetCommand(string executable, string args)
        {
            return executable + " " + args;
        }

        public static List<string> RunCommand(string executable, string args, bool showWindows)
        {
            List<string> output = new List<string>();
            if (showWindows)
            {
                using (CmdConsole cmd = new CmdConsole())
                {
                    string command = "start " + GetCommand(executable, args);
                    foreach (var ln in cmd.Execute(command))
                    {
                        if (ln.Trim().NotNullOrEmpty())
                        {
                            output.Add(ln);
                        }
                    }
                }
            }
            else
            {
                Process slaveProcess = new Process();
                slaveProcess.StartInfo.FileName = executable;
                slaveProcess.StartInfo.Arguments = args;
                slaveProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                slaveProcess.Start();
            }
            return output;
        }

        private static string _rmpDir;
        public static string rmpDir
        {
            get
            {
                if (_rmpDir != null)
                {
                    return _rmpDir;
                }

                _rmpDir = Environment.GetEnvironmentVariable("MTRMP");
                if (!_rmpDir.IsNullOrEmpty())
                {
                    _rmpDir = CheckDirectoryPath(_rmpDir);
                    return _rmpDir;
                }

                string assemblyFile = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                DirectoryInfo di = new FileInfo(assemblyFile).Directory;
                while (di != null)
                {
                    // If the directory contains RMP, then we are in the metratech dir
                    _rmpDir = di.GetDirectories().Where(d => d.Name.EqualsIgnoreCase("RMP")).Select(d => d.FullName).FirstOrDefault();
                    if (_rmpDir != null)
                    {
                        return _rmpDir;
                    }
                    // Otherwise, go up one an try again
                    di = di.Parent;
                }
                throw new Exception("Error, cannot locate RMP dir from location of mvm.exe, executing from location:" + Assembly.GetExecutingAssembly().Location);
            }
        }

        private static string _rmpBinDir;
        public static string rmpBinDir
        {
            get
            {
                if (_rmpBinDir != null)
                {
                    return _rmpBinDir;
                }
                _rmpBinDir = Environment.GetEnvironmentVariable("MTRMPBIN");
                if (!_rmpBinDir.IsNullOrEmpty())
                {
                    _rmpBinDir = CheckDirectoryPath(_rmpBinDir);
                    return _rmpBinDir;
                }

                // Assume the assembly is in RMP/bin
                string assemblyFile = Assembly.GetExecutingAssembly().Location;
                DirectoryInfo di = new FileInfo(assemblyFile).Directory;
                return di.FullName;
            }
        }

        public static string rmpLogBase = Path.DirectorySeparatorChar + "MvmLogs";

        public static string rmpLogDir
        {
            get
            {
                return rmpDir + rmpLogBase;
            }
        }

        public static string CheckDirectoryPath(string path)
        {
            if (path.EndsWith(":"))
            {
                path = path + "\\";
            }
            return path;
        }

        // Returns all top-level mvm config directories under RMP
        public static IEnumerable<string[]> EnumerateRMPDirectories(string extensionsDir)
        {
            foreach (string extensionDir in Directory.GetDirectories(extensionsDir))
            {
                string mvmConfigDir = Path.Combine(extensionDir, "MvmConfig");
                if (!Directory.Exists(mvmConfigDir))
                {
                    continue;
                }
                string extensionName = new DirectoryInfo(extensionDir).Name;
                foreach (string structuralNameSpaceDir in Directory.GetDirectories(mvmConfigDir))
                {
                    string structuralNameSpace = new DirectoryInfo(structuralNameSpaceDir).Name;
                    if (structuralNameSpace.EqualsIgnoreCase("generated"))
                    {
                        continue;
                    }
                    yield return new string[] { structuralNameSpace, structuralNameSpaceDir };
                }
            }
        }

        // Returns all mvm config files under a directory returned by EnumerateRMPDirectories
        public static string[] GetRMPFiles(string structuralNameSpaceDir)
        {
            return Directory.GetFiles(structuralNameSpaceDir, "*.xml", SearchOption.AllDirectories).Where(f => f.EndsWith(".xml")).ToArray();
        }

        // Returns all referenced assemblies that are under RMP
        public static void GetRMPAssemblies(Assembly ass, Dictionary<string, string> assemblyList)
        {
            AssemblyName fqn = ass.GetName();
            if (assemblyList.ContainsKey(fqn.FullName) || !ass.Location.Contains(MvmClusterCommon.rmpDir))
            {
                return;
            }
            assemblyList[fqn.FullName] = ass.Location;

            foreach (AssemblyName name in ass.GetReferencedAssemblies())
            {
                Assembly referenced = Assembly.Load(name);
                GetRMPAssemblies(referenced, assemblyList);
            }
        }

        // Takes C:\MetraTech\RMP\xxx\yyy\f.xml and returns RMP\xxx\yyy\f.xml
        public static string PathRelativeToRMP(string rmp, string path)
        {
            if (!path.StartsWith(rmp))
            {
                throw new Exception("Unexpected: path " + path + " does not start with " + rmp);
            }
            DirectoryInfo topDir = new DirectoryInfo(rmp);
            return path.Substr(topDir.Parent.FullName.Length).TrimStart('\\').TrimStart('/');
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
    }

    public abstract class BaseListener
    {
        public static int TcpBufferSize = 10 * 1000 * 1024;

        protected readonly Thread thread;
        private readonly TcpListener listener;
        public int port;

        private readonly object listenerLock;
        private bool stopped = true;

        public BaseListener(IPAddress listenIP, int portNo, string threadName)
        {
            port = portNo;
            listenerLock = new object();
            listener = new TcpListener(listenIP, port);
            try
            {
                listener.Start();
            }
            catch (Exception e)
            {
                throw new Exception("Cannot listen on port " + this.port + ": " + e.Message);
            }
            thread = new Thread(new ThreadStart(StartListener));
            thread.Name = threadName;
            Start();
        }

        public bool IsStopped
        {
            get
            {
                lock (listenerLock)
                {
                    return stopped;
                }
            }
        }

        public bool IsStarted
        {
            get
            {
                lock (listenerLock)
                {
                    return !stopped;
                }
            }
        }

        public void Start()
        {
            lock (listenerLock)
            {
                if (stopped)
                {
                    stopped = false;
                    thread.Start();
                }
            }
        }

        public void Stop()
        {
            lock (listenerLock)
            {
                if (stopped)
                {
                    return;
                }
                stopped = true;
                listener.Stop();
            }
            thread.Join();
        }

        protected void StartListener()
        {
            try
            {
                while (IsStarted)
                {
                    Socket socket = listener.AcceptSocket();
                    if (IsStopped)
                    {
                        break;
                    }
                    TcpClient client = SetupClient(socket);
                    HandleClient(client);
                }
            }
            catch (SocketException se)
            {
                // Calling listener.Stop() while listening throws a socket exception that we want to ignore
                string msg = se.Message;
            }
            finally
            {
                OnStopListener();
                try
                {
                    Stop();
                }
                catch
                {
                }
            }
        }

        private TcpClient SetupClient(Socket socket)
        {
            TcpClient client = new TcpClient();
            client.Client = socket;
            client.SendBufferSize = TcpBufferSize;
            client.ReceiveBufferSize = TcpBufferSize;
            client.NoDelay = true;
            return client;
        }

        protected abstract void HandleClient(TcpClient tcpClient);
        protected abstract void OnStopListener();
    }

    public class WinAPI
    {
        public const short FILE_ATTRIBUTE_NORMAL = 0x80;
        public const short INVALID_HANDLE_VALUE = -1;
        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;
        public const uint CREATE_NEW = 1;
        public const uint CREATE_ALWAYS = 2;
        public const uint OPEN_EXISTING = 3;
        public const uint FILE_SHARE_READ = 0x00000001;
        public const uint FILE_SHARE_WRITE = 0x00000002;
        public const uint FILE_SHARE_DELETE = 0x00000004;

        //public struct IO_STATUS_BLOCK
        //{
        //    uint status;
        //    ulong information;
        //}

        public struct _FILE_INTERNAL_INFORMATION
        {
            public ulong IndexNumber;
        }

        public enum FILE_INFORMATION_CLASS
        {
            FileDirectoryInformation = 1,
            FileFullDirectoryInformation,
            FileBothDirectoryInformation,
            FileBasicInformation,
            FileStandardInformation,
            FileInternalInformation
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BY_HANDLE_FILE_INFORMATION
        {
            public uint FileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME CreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWriteTime;
            public uint VolumeSerialNumber;
            public uint FileSizeHigh;
            public uint FileSizeLow;
            public uint NumberOfLinks;
            public uint FileIndexHigh;
            public uint FileIndexLow;
        }

        //[DllImport("ntdll.dll", SetLastError = true)]
        //public static extern IntPtr NtQueryInformationFile(IntPtr fileHandle, ref IO_STATUS_BLOCK IoStatusBlock, IntPtr pInfoBlock, uint length, FILE_INFORMATION_CLASS fileInformation);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetFileInformationByHandle(IntPtr hFile, out BY_HANDLE_FILE_INFORMATION lpFileInformation);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool DeleteFile(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        public static ulong GetFileID(string filename)
        {
            BY_HANDLE_FILE_INFORMATION objectFileInfo = new BY_HANDLE_FILE_INFORMATION();
            IntPtr handle = CreateFile(filename, GENERIC_READ, FILE_SHARE_READ, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
            if (handle.ToInt64() == INVALID_HANDLE_VALUE)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
            GetFileInformationByHandle(handle, out objectFileInfo);
            CloseHandle(handle);
            return ((ulong)objectFileInfo.FileIndexHigh << 32) + (ulong)objectFileInfo.FileIndexLow;
        }
    }
}
