using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using MVM;

namespace MVM_LAUNCHER
{
    public struct CacheItem
    {
        public ulong fileId;  // unique file id returned by the OS (like an inode)
        public DateTime lastModified;
        public byte[] hash;
        public CacheItem(ulong id, DateTime date, byte[] hash)
        {
            this.fileId = id;
            this.lastModified = date;
            this.hash = hash;
        }
        public string GetFileName()
        {
            return MvmClusterCommon.ByteArrayToString(this.hash);
        }
    }

    public class FilesystemManager
    {
        public string rootDirectory;
        public string cacheDirectory;
        public string catalogFile;
        public SHA1 hasher;

        public BasicLogger logger;

        public Dictionary<string, CacheItem> HashIndex;
        public Dictionary<ulong, CacheItem> FileIdIndex;
        public Dictionary<string, Sandbox> SandboxIndex;

        public object cacheLock = new object();
        public object sandboxLock = new object();

        public Timer maintenanceTimer;
        public TimerCallback maintenanceCallback;

        // TODO: should check that we're using NTFS
        public FilesystemManager(string dir, BasicLogger logger)
        {
            this.rootDirectory = dir;
            this.cacheDirectory = this.rootDirectory + Path.DirectorySeparatorChar + "cache";
            this.hasher = new SHA1CryptoServiceProvider();
            this.logger = logger;

            try
            {
                if (!Directory.Exists(this.cacheDirectory))
                {
                    this.logger.WriteLine("FsManager creating cache directory: " + this.cacheDirectory);
                    Directory.CreateDirectory(this.cacheDirectory);
                }

                this.Refresh();
            }
            catch (Exception e)
            {
                this.logger.WriteLine("Could not instantiate FilesystemManager: " + e.Message + ": " + e.StackTrace);
                throw e;
            }

            // We wait 60 seconds before running maintenance the first time, then once every 24 hours after that
            this.maintenanceCallback = new TimerCallback(PerformMaintenance);
            this.maintenanceTimer = new Timer(this.maintenanceCallback, null, 60 * 1000, 24 * 60 * 60 * 1000);
        }

        public Sandbox GetSandbox(string machine, int mvmRunId)
        {
            Sandbox s;
            string id = machine + "_" + mvmRunId.ToString();
            lock (sandboxLock)
            {
                if (this.SandboxIndex.TryGetValue(id, out s))
                {
                    return s;
                }
                s = new Sandbox(this.rootDirectory, id, this);
                this.SandboxIndex[id] = s;
            }
            return s;
        }

        public void Refresh()
        {
            this.logger.WriteLine("FsManager refresh");
            this.HashIndex = new Dictionary<string, CacheItem>();
            this.FileIdIndex = new Dictionary<ulong, CacheItem>();

            // Don't need to recompute any hashes since we write each file then rename it to avoid
            // partial/corrupted writes
            lock (cacheLock)
            {
                foreach (var file in Directory.EnumerateFiles(this.cacheDirectory))
                {
                    string name = Path.GetFileNameWithoutExtension(file);
                    if (name.StartsWith("temp"))
                    {
                        // Delete any temp files from previous interrupted work
                        this.logger.WriteLine("FsManager deleting temp file " + file);
                        File.Delete(file);
                    }
                    else
                    {
                        byte[] hash = MvmClusterCommon.StringToByteArray(name);
                        ulong id = WinAPI.GetFileID(file);
                        CacheItem item = new CacheItem(id, DateTime.Now, hash);
                        this.HashIndex.Add(MvmClusterCommon.ByteArrayToString(hash), item);
                        this.FileIdIndex.Add(id, item);
                    }
                }
            }

            // TODO: do each sandbox in a separate thread
            this.SandboxIndex = new Dictionary<string, Sandbox>();
            lock (sandboxLock)
            {
                foreach (var sandboxDir in Directory.EnumerateDirectories(this.rootDirectory, Sandbox.SandboxDirectoryPrefix + "*"))
                {
                    Sandbox s = new Sandbox(sandboxDir, this);
                    this.SandboxIndex[s.id] = s;
                    this.logger.WriteLine("FsManager opening sandbox " + s.id + " in " + sandboxDir);
                }
            }
        }

        // Prune old files from the cache, and old sandboxes
        public void PerformMaintenance(object state)
        {
        }

        public bool ContainsHash(byte[] hash)
        {
            return this.HashIndex.ContainsKey(MvmClusterCommon.ByteArrayToString(hash));
        }

        public string FullFileName(byte[] hash)
        {
            return this.cacheDirectory + Path.DirectorySeparatorChar + this.HashIndex[MvmClusterCommon.ByteArrayToString(hash)].GetFileName();
        }

        public void AddFile(byte[] data, byte[] hash)
        {
            lock (cacheLock)
            {
                string prefix = this.cacheDirectory + Path.DirectorySeparatorChar;
                string filename = MvmClusterCommon.ByteArrayToString(hash);
                string tempfile = prefix + "temp_" + filename;
                using (FileStream fs = new FileStream(tempfile, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(data, 0, data.Length);
                }
                File.Move(tempfile, prefix + filename);

                ulong id = WinAPI.GetFileID(prefix + filename);
                CacheItem item = new CacheItem(id, DateTime.Now, hash);
                this.HashIndex.Add(MvmClusterCommon.ByteArrayToString(hash), item);
                this.FileIdIndex.Add(id, item);
                this.logger.WriteLine("FsManager added file " + filename);
            }
        }
    }
}
