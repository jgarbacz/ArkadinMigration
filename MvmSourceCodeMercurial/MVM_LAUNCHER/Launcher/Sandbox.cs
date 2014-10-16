using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using MVM;

namespace MVM_LAUNCHER
{
    public class Sandbox
    {
        public static string SandboxDirectoryPrefix = "sandbox_";

        public string id;  // identifies the sandbox -- currently just the super machine + run id
        public string directory;
        public FilesystemManager fsManager;
        public Dictionary<string, CacheItem> FileCache;

        public object sandboxLock = new object();

        public Sandbox(string rootDir, string id, FilesystemManager fmgr)
        {
            string dirName = SandboxDirectoryPrefix + id.Replace(@"\", "").Replace("/", "") + "_" + DateTime.Now.ToString("yyyyMMDDHHmmss");
            string directory = rootDir + Path.DirectorySeparatorChar + dirName;
            this.directory = directory;
            this.fsManager = fmgr;
            if (!Directory.Exists(this.rmpDir))
            {
                this.fsManager.logger.WriteLine("Sandbox creating directory " + this.rmpDir);
                Directory.CreateDirectory(this.rmpDir);
            }
            using (StreamWriter outfile = new StreamWriter(this.nameFile))
            {
                outfile.WriteLine(id);
            }

            this.Refresh();
        }

        public Sandbox(string dir, FilesystemManager fmgr)
        {
            this.directory = dir;
            this.fsManager = fmgr;
            this.Refresh();
        }

        public void Refresh()
        {
            lock (sandboxLock)
            {
                SHA1 FileHasher = new SHA1CryptoServiceProvider();
                this.FileCache = new Dictionary<string, CacheItem>();
                foreach (var line in File.ReadLines(this.nameFile))
                {
                    this.id = line;
                }
                foreach (var file in this.EnumerateFiles())
                {
                    CacheItem item;
                    ulong id = WinAPI.GetFileID(this.FullFilename(file));
                    this.fsManager.logger.WriteLine("Sandbox found file: " + file);
                    if (!this.fsManager.FileIdIndex.TryGetValue(id, out item))
                    {
                        byte[] data = File.ReadAllBytes(this.FullFilename(file));
                        byte[] hash = FileHasher.ComputeHash(data);
                        this.fsManager.AddFile(data, hash);
                        this.fsManager.logger.WriteLine("Sandbox added file to cache: " + file);
                    }
                    this.FileCache.Add(file, item);
                }
            }
        }

        public string nameFile
        {
            get
            {
                return this.directory + Path.DirectorySeparatorChar + "name.txt";
            }
        }

        public string rmpDir
        {
            get
            {
                return this.directory + Path.DirectorySeparatorChar + "RMP";
            }
        }

        public string rmpBinDir
        {
            get
            {
                return this.rmpDir + Path.DirectorySeparatorChar + "bin";
            }
        }

        public string FullFilename(string relativeFilename)
        {
            return this.directory + Path.DirectorySeparatorChar + relativeFilename;
        }

        public bool ContainsFile(string filename, byte[] hash)
        {
            string hash1 = MvmClusterCommon.ByteArrayToString(this.FileCache[filename].hash);
            string hash2 = MvmClusterCommon.ByteArrayToString(hash);
            return this.FileCache.ContainsKey(filename) && hash1.Equals(hash2);
        }

        // TODO: maintenance thread to prune cache, sandboxes
        public void AddOrReplaceFile(LauncherListener l, string filename, byte[] data, byte[] hash)
        {
            lock (sandboxLock)
            {
                try
                {
                    string fullPath = this.FullFilename(filename);

                    // Make the sandbox directory if it doesn't exist
                    Directory.CreateDirectory((new FileInfo(fullPath)).Directory.FullName);

                    this.fsManager.logger.WriteLine("Sandbox adding file " + filename);

                    // Delete the file if it already exists (maintenance thread will remove from cache when needed)
                    File.Delete(fullPath);

                    if (!this.fsManager.ContainsHash(hash))
                    {
                        this.fsManager.AddFile(data, hash);
                    }

                    // Make a hard link to the file
                    WinAPI.CreateHardLink(fullPath, this.fsManager.FullFileName(hash), IntPtr.Zero);
                }
                catch (Exception e)
                {
                    this.fsManager.logger.WriteLine("Sandbox add/replace error: " + e.Message + ": " + e.StackTrace);
                }
            }
        }

        public void RemoveFile(string relativeFile)
        {
            lock (sandboxLock)
            {
                this.FileCache.Remove(relativeFile);
                string fullFile = this.FullFilename(relativeFile);
                FileInfo fi = new FileInfo(fullFile);
                DirectoryInfo di = fi.Directory;
                File.Delete(fullFile);
                while (di.GetFiles().Length == 0 && di.GetDirectories().Length == 0)
                {
                    Directory.Delete(di.FullName, false);
                    di = di.Parent;
                }
            }
        }

        public IEnumerable<string> EnumerateFiles()
        {
            lock (sandboxLock)
            {
                // Return all files under RMP, filtering out the logs directory
                foreach (var file in Directory.EnumerateFiles(this.directory, "RMP" + Path.DirectorySeparatorChar + "*.*", SearchOption.AllDirectories)
                    .Where(f => !f.Contains(MvmClusterCommon.rmpLogBase + @"\")))
                {
                    yield return MvmClusterCommon.PathRelativeToRMP(this.rmpDir, file);
                }
            }
        }
    }
}
