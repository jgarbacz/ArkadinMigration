using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.IO;
namespace MVM
{
    // Source to get this thing working:
    // http://social.msdn.microsoft.com/Forums/en-US/csharplanguage/thread/4ff8d862-f228-434c-8101-63fe57a21982/
    // http://arxiv.org/ftp/cs/papers/0502/0502012.pdf
    // http://msdn.microsoft.com/en-us/library/microsoft.win32.safehandles.safefilehandle.aspx
    // http://msdn.microsoft.com/en-us/library/aa288474%28v=vs.71%29.aspx (havent worked on this one yet)

    using System.IO;
    public class Junk
    {
        public const uint FILE_FLAG_NO_BUFFERING = 0x20000000;

        [DllImport("kernel32", SetLastError = true)]
        static extern SafeFileHandle CreateFile(
            string FileName, // file name
            uint DesiredAccess, // access mode
            uint ShareMode, // share mode
            IntPtr SecurityAttributes, // Security Attr
            uint CreationDisposition, // how to create
            uint FlagsAndAttributes, // file attributes
            IntPtr hTemplate // template file
        );

        public static void TestJunk()
        {
            string FileName= @"d:\testjunk.txt";
            SafeFileHandle handle = CreateFile(FileName,
            (uint)FileAccess.Read,
            (uint)FileShare.None,
            IntPtr.Zero,
            (uint)FileMode.Open,
            FILE_FLAG_NO_BUFFERING,
            IntPtr.Zero);

            FileStream stream = new FileStream(handle,
            FileAccess.Read,
            4096,
            false
            );

            StreamReader sw = new StreamReader(stream);
            string s = sw.ReadToEnd();
            Console.WriteLine("GOT: [" + s + "]");
            sw.Close();
        }
    }


    public class UnbufferedFileLoader
    {
         public static FileStream GetUnbufferedFileStream(string path, FileAccess fileAccess, FileShare fileShare, FileMode fileMode)
         {
             FileStream fs = new UnmanagedFileLoader().GetUnbufferFileStream(path,fileAccess,fileShare,fileMode);
             return fs;
         }
    }

    public class UnmanagedFileLoader
    {
        public const short FILE_ATTRIBUTE_NORMAL = 0x80;
        public const short INVALID_HANDLE_VALUE = -1;
        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;
        public const uint CREATE_NEW = 1;
        public const uint CREATE_ALWAYS = 2;
        public const uint OPEN_EXISTING = 3;
        public const uint FILE_FLAG_NO_BUFFERING = 0x20000000;
        public const uint FILE_FLAG_WRITE_THROUGH = 0x80000000;
        public const uint FILE_SHARE_READ = 0x00000001; 

        // Use interop to call the CreateFile function.
        // For more information about CreateFile,
        // see the unmanaged MSDN reference library.
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern SafeFileHandle CreateFile(
            string lpFileName, 
            uint dwDesiredAccess,
            uint dwShareMode, 
            IntPtr lpSecurityAttributes, 
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes, 
            IntPtr hTemplateFile);

        public FileStream GetUnbufferFileStream(string Path,FileAccess fileAccess, FileShare fileShare,FileMode fileMode)
        {
            if (Path == null || Path.Length == 0)
            {
                throw new ArgumentNullException("Path");
            }

            // Try to open the file. -- ORIGINAL
            //handleValue = CreateFile(Path, GENERIC_WRITE, 0, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);

            // Rob's version
            SafeFileHandle Handle = CreateFile(
                Path,
                //(uint)fileAccess,
                GENERIC_WRITE,
                //(uint)fileShare,
                FILE_SHARE_READ,
                IntPtr.Zero,
                //(uint)fileMode,
                CREATE_ALWAYS,
                //FILE_FLAG_NO_BUFFERING,
                (FILE_FLAG_NO_BUFFERING | FILE_FLAG_WRITE_THROUGH), 
                IntPtr.Zero);

            //SafeFileHandle Handle = CreateFile(Path, GENERIC_WRITE, FILE_SHARE_READ, IntPtr.Zero, CREATE_ALWAYS, useBuffer ? 0 : (FILE_FLAG_NO_BUFFERING | FILE_FLAG_WRITE_THROUGH), IntPtr.Zero); 

            // If the handle is invalid,
            // get the last Win32 error 
            // and throw a Win32Exception.
            if (Handle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            FileStream fs = new FileStream(Handle, fileAccess, 4096, false);
            return fs;
        }
    }
}

