//using System;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Threading;
//using Microsoft.Win32.SafeHandles;

//namespace BackupTool
//{
//    static class Program
//    {
//        public static long UtcOffset = (DateTime.Now.Ticks - DateTime.UtcNow.Ticks);//for getting DateTime.Now 50x times faster
//        //static void Main()
//        //{
//        //    AppDomain.CurrentDomain.UnhandledException += HandleExceptions;
//        //    Console.ForegroundColor = ConsoleColor.Green;

//        //    var config = ToolConfig.GetConfiguration();
//        //    var s = new DirectoryInfo(config.Source);
//        //    if (!s.Exists) return;
//        //    uint t1, t2, t3;
//        //    GetDiskFreeSpaceW(s.Root.ToString(), out t1, out SectorSize, out t2, out t3);
//        //    if (new DirectoryInfo(config.Destination).Exists)
//        //    {
//        //        var dirs = Directory.GetDirectories(config.Destination);
//        //        if (dirs.Length > config.MaxDaysForHistory)
//        //        {
//        //            var DirsToDelete = (from dir in dirs let di = new DirectoryInfo(dir) orderby di.CreationTimeUtc ascending select di).
//        //                      Take(dirs.Length - config.MaxDaysForHistory);
//        //            foreach (var dir in DirsToDelete)
//        //            {
//        //                dir.Delete(true);
//        //            }
//        //        }
//        //    }
//        //    CopyDir(config.Source, config.Destination);
//        //}

//        public static string OrginalSrc;
//        public static string OrginalDst;


//        //public static void CopyDir(string src, string dst)
//        //{
//        //    OrginalSrc = src;
//        //    if (!new DirectoryInfo(OrginalSrc).Exists) return;

//        //    var dt = Now.ToString().Split(' ');

//        //    OrginalDst = Path.Combine(dst, dt[0].ReplaceWithUnderScores());

//        //    var di = new DirectoryInfo(OrginalDst);
//        //    if (!di.Exists) di.Create();

//        //    OrginalDst = Path.Combine(OrginalDst, dt[1].ReplaceWithUnderScores());

//        //    new Thread(() => InternalCopyDir(OrginalSrc, OrginalDst)) { Name = OrginalDst }.Start();
//        //}

//        //private static void InternalCopyDir(string src, string dst)
//        //{
//        //    new DirectoryInfo(dst).Create();

//        //    foreach (string dir in Directory.EnumerateDirectories(src))
//        //    {
//        //        InternalCopyDir(dir, OrginalDst + dir.Substring(OrginalSrc.Length));
//        //    }

//        //    foreach (string file in Directory.EnumerateFiles(src))
//        //    {
//        //        CopyFile(file);
//        //    }
//        //}

//        //copy files without using the filesystem cache to write directly to disk without any buffers in between,
//        //by using ReadFile & WriteFile , which is faster then CopyFile
//        //for more info:
//        // http://blogs.technet.com/b/askperf/archive/2007/05/08/slow-large-file-copy-issues.aspx
//        // http://msdn.microsoft.com/en-us/library/cc644950(v=VS.85).aspx
//        // http://msdn.microsoft.com/en-us/library/aa363858(VS.85).aspx#caching_behavior
//        // http://research.microsoft.com/apps/pubs/default.aspx?id=64538
//        public static void CopyFile(string src)
//        {
//            int size = 0xFFFF;

//            byte[] buffer;
//            using (var r = new FileStream(src, FileMode.Open, FileAccess.Read))
//            {
//                buffer = new byte[r.Length];
//                r.Read(buffer, 0, buffer.Length);
//            }
//            Console.WriteLine("Copying: " + src);

//            try
//            {
//                SafeFileHandle hDst = CreateFile(OrginalDst + src.Substring(OrginalSrc.Length),
//                                 (uint)FileAccess.Write,
//                                 (uint)FileShare.Write,
//                                 IntPtr.Zero,
//                                 (uint)FileMode.Create,
//                                 (buffer.Length % SectorSize == 0) ? AlignedFlags : NonAlignedFlags,
//                                 IntPtr.Zero);
//                using (var w = new FileStream(hDst, FileAccess.Write, Math.Max(Math.Min(size, buffer.Length), 4096), false))
//                {
//                    int len = buffer.Length;
//                    w.SetLength(len);
//                    for (int i = 0; i < len; i += size)
//                    {
//                        int chunkSize = (len - i);
//                        if (chunkSize < size) size = chunkSize;
//                        w.Write(buffer, i, size);
//                        w.Flush();
//                    }
//                    w.Flush();
//                }
//            }
//            catch
//            {
//                SafeFileHandle hDst = CreateFile(OrginalDst + src.Substring(OrginalSrc.Length),
//                         (uint)FileAccess.Write,
//                         (uint)FileShare.Write,
//                         IntPtr.Zero,
//                         (uint)FileMode.Create,
//                         NonAlignedFlags,
//                         IntPtr.Zero);
//                using (var w = new FileStream(hDst, FileAccess.Write, Math.Max(Math.Min(size, buffer.Length), 4096), false))
//                {
//                    int len = buffer.Length;
//                    w.SetLength(len);
//                    for (int i = 0; i < len; i += size)
//                    {
//                        int chunkSize = (len - i);
//                        if (chunkSize < size) size = chunkSize;
//                        w.Write(buffer, i, size);
//                        w.Flush();
//                    }
//                    w.Flush();
//                }
//            }
//        }


//        private static uint SectorSize;
//        private const uint AlignedFlags = FILE_FLAG_NO_BUFFERING | FILE_FLAG_SEQUENTIAL_SCAN | FILE_FLAG_WRITE_THROUGH;
//        private const uint NonAlignedFlags = FILE_FLAG_SEQUENTIAL_SCAN | FILE_FLAG_WRITE_THROUGH;

//        private const uint FILE_FLAG_NO_BUFFERING = 0x20000000;
//        private const uint FILE_FLAG_SEQUENTIAL_SCAN = 0x08000000;
//        private const uint FILE_FLAG_WRITE_THROUGH = 0x80000000;


//        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
//        public static extern SafeFileHandle CreateFile(
//          [In] [MarshalAs(UnmanagedType.LPWStr)] string lpFileName,
//          uint dwDesiredAccess, uint dwShareMode,
//          [In] IntPtr lpSecurityAttributes,
//          uint dwCreationDisposition,
//          uint dwFlagsAndAttributes,
//          [In] IntPtr hTemplateFile);

//        [DllImport("kernel32.dll", EntryPoint = "GetDiskFreeSpaceW")]
//        [return: MarshalAs(UnmanagedType.Bool)]
//        public static extern bool GetDiskFreeSpaceW([In] [MarshalAs(UnmanagedType.LPWStr)] string lpRootPathName,
//                             [Out] out uint lpSectorsPerCluster, [Out] out uint lpBytesPerSector,
//                            [Out]out uint lpNumberOfFreeClusters, [Out] out uint lpTotalNumberOfClusters);

//        public static string ReplaceWithUnderScores(this string str)
//        {
//            var s = str.ToCharArray();
//            for (int i = 0; i < s.Length; i++)
//            {
//                if (((s[i] == '/') || (s[i] == ':')) || (s[i] == ' ')) s[i] = '_';
//            }
//            return new string(s, 0, s.Length);
//        }

//        public static DateTime Now
//        {
//            get { return DateTime.UtcNow.AddTicks(UtcOffset); }
//        }

//        private static void HandleExceptions(object sender, UnhandledExceptionEventArgs e)
//        {
//            string error = (e.ExceptionObject as Exception).GetBaseException().ToString();
//            Console.WriteLine("\n\n\n");

//            Console.ForegroundColor = ConsoleColor.Red;
//            Console.WriteLine(error);

//            Console.ReadKey(true);
//            Environment.Exit(0);
//        }
//    }
//}

