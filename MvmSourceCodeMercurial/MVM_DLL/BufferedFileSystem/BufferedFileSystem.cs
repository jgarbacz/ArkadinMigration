using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using NLog;
namespace MVM
{
    /// <summary>
    /// Creates a buffered file system on top of the current file system. Provides an LRU
    /// for files so that most recently used files stay in memory. Allows users to specify
    /// max files open, max buffer size.
    /// </summary>
    public class BufferedFileSystem : IDisposable
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public static void Test()
        {
            //TestProducerConsumer();
            //TestCaching();
            //TestConsumerCatchup();
            Junk.TestJunk();
        }

        public static void TestProducerConsumer()
        {
            int maxFiles = 2;
            int writeStartBytes = 40;
            int writeIncrBytes = 2;
            int writeMaxBytes = 80;
            int readBytes = 80;
            BufferedFileSystem bfs = new BufferedFileSystem(maxFiles, writeStartBytes, writeIncrBytes, writeMaxBytes, readBytes);

            string file1 = @"d:\t1_persist.txt";
            Thread producerThread1 = new Thread(new ThreadStart(new TestProducer(bfs, file1, true).Run));
            producerThread1.Name = "producer1";
            Thread consumerThread1 = new Thread(new ThreadStart(new TestConsumer(bfs, file1, true).Run));
            consumerThread1.Name = "consumer1";

            string file2 = @"d:\t2_nopersist.txt";
            Thread producerThread2 = new Thread(new ThreadStart(new TestProducer(bfs, file2, false).Run));
            producerThread2.Name = "producer2";
            Thread consumerThread2 = new Thread(new ThreadStart(new TestConsumer(bfs, file2, false).Run));
            consumerThread2.Name = "consumer2";

            producerThread1.Start();
            consumerThread1.Start();

            producerThread2.Start();
            consumerThread2.Start();

            producerThread1.Join();
            consumerThread1.Join();

            producerThread2.Join();
            consumerThread2.Join();

            bfs.Dispose();
            logger.Debug("ALL THREADS DONE");
            if (true) return;
        }

        public class TestProducer
        {
            BufferedFileSystem bfs;
            string fileName;
            bool persistent;
            public TestProducer(BufferedFileSystem bfs, string fileName, bool persistent)
            {
                this.bfs = bfs;
                this.fileName = fileName;
                this.persistent = persistent;
            }
            public void Run()
            {
                int max = 9;
                for (int i = 0; i <= max; i++)
                {
                    Thread.Sleep(1000);
                    using (var cachedItem = bfs.GetBinaryQueue(fileName,this.persistent))
                    {
                        var bqueue = (cachedItem.value as BinaryQueueBufferedFile);
                        var bwriter = bqueue.BinaryWriter;
                        bwriter.Write(i.ToString());
                        bqueue.Commit();
                        if (i == max) bqueue.SetEof();
                    }
                }
            }
        }
        public class TestConsumer
        {
            BufferedFileSystem bfs;
            string fileName;
            bool persistent;
            public TestConsumer(BufferedFileSystem bfs, string fileName,bool persistent)
            {
                this.bfs = bfs;
                this.fileName = fileName;
                this.persistent = persistent;
            }
            public void Run()
            {
                try
                {
                    for (; ; )
                    {
                        using (var cachedItem = bfs.GetBinaryQueue(fileName,this.persistent))
                        {
                            var bqueue = (cachedItem.value as BinaryQueueBufferedFile);
                            var breader = bqueue.BinaryReader;
                            string s = breader.ReadString();
                            logger.Debug("consumed {0}", s.Nvl("NULL"));
                        }
                    }
                }
                catch (EndOfStreamException e)
                {
                    logger.Debug("reached end of stream: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Prove that files get flushed and reopened as needed
        /// </summary>
        public static void TestCaching()
        {
            int maxFiles = 1;
            int writeStartBytes = 1;
            int writeIncrBytes = 2;
            int writeMaxBytes = 4;
            int readBytes = 1;
            BufferedFileSystem bfs = new BufferedFileSystem(maxFiles, writeStartBytes, writeIncrBytes, writeMaxBytes, readBytes);

            logger.Debug("---- t1 ----");
            using (var cachedItem = bfs.GetBinaryQueue(@"d:\t1.txt",true))
            {
                var bqueue = (cachedItem.value as BinaryQueueBufferedFile);
                var bwriter = bqueue.BinaryWriter;
                bwriter.Write("aa");
                bqueue.Commit();

                var breader = bqueue.BinaryReader;
                string s = breader.ReadString();
                logger.Debug("read=" + s.Nvl("NULL"));
            }
            logger.Debug("---- t2 ----");
            using (var cachedItem = bfs.GetBinaryQueue(@"d:\t2.txt", true))
            {
                var bqueue = (cachedItem.value as BinaryQueueBufferedFile);
                var bwriter = bqueue.BinaryWriter;
                bwriter.Write("a");
            }
            logger.Debug("---- t1 ----");
            using (var cachedItem = bfs.GetBinaryQueue(@"d:\t1.txt", true))
            {
                var bqueue = (cachedItem.value as BinaryQueueBufferedFile);
                var bwriter = bqueue.BinaryWriter;
                bwriter.Write("bb");
            }
            bfs.Dispose();
            logger.Debug("done");
        }

        /// <summary>
        /// Prove non persistent queue can dump to disk and that we can catchup.
        /// </summary>
        public static void TestConsumerCatchup()
        {
            bool persist = false;
            string file = @"d:\t_catchup.txt";
            int maxFiles = 1;
            int writeStartBytes = 2;
            int writeIncrBytes = 0;
            int writeMaxBytes = 2;
            int readBytes = 2;
            using (BufferedFileSystem bfs = new BufferedFileSystem(maxFiles, writeStartBytes, writeIncrBytes, writeMaxBytes, readBytes))
            {
                using (var cachedItem = bfs.GetBinaryQueue(file, persist))
                {
                    var bqueue = (cachedItem.value as BinaryQueueBufferedFile);
                    var bwriter = bqueue.BinaryWriter;
                    var breader = bqueue.BinaryReader;
                    string s;

                    s = "a";
                    logger.Debug("WRITE: " + s);
                    bwriter.Write(s);
                    bqueue.Commit();

                    s = breader.ReadString();
                    logger.Debug("READ: " + s);

                    s = "b";
                    logger.Debug("WRITE: " + s);
                    bwriter.Write(s); //should end up on disk
                    bqueue.Commit();

                    s = "c";
                    logger.Debug("WRITE: " + s);
                    bwriter.Write(s);
                    bqueue.Commit();

                    s = breader.ReadString();
                    logger.Debug("READ: " + s);

                    s = breader.ReadString();
                    logger.Debug("READ: " + s);
                }
            }
            logger.Debug("done");
        }


        public static void Test1()
        {
            var fs = UnbufferedFileLoader.GetUnbufferedFileStream("d:\\t.txt", FileAccess.Write, FileShare.Write, FileMode.CreateNew);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write("hello");
            bw.Close();
        }

        public readonly int maxOpenFiles;
        public readonly int writeBufferStartBytes;
        public readonly int writeBufferIncrBytes;
        public readonly int writeBufferMaxBytes;
        public readonly int readBufferBytes;
        public readonly BufferedFileCache bufferedFileCache;
        public MvmEngine mvm;
        public BufferedFileSystem(int maxOpenFiles, int writeBufferStartBytes, int writeBufferIncrBytes, int writeBufferMaxBytes, int readBufferBytes)
        {
            this.maxOpenFiles = maxOpenFiles;
            this.writeBufferStartBytes = writeBufferStartBytes;
            this.writeBufferIncrBytes = writeBufferIncrBytes;
            this.writeBufferMaxBytes = writeBufferMaxBytes;
            this.readBufferBytes = readBufferBytes;
            this.bufferedFileCache = new BufferedFileCache(this);
        }

        #region Binary Queue

        public BinaryQueueBufferedFile CreatePersistentBinaryQueue(string fileName,object state)
        {
            return new BinaryQueueBufferedFile(this, fileName, true, state);
        }

        public BinaryQueueBufferedFile CreateBinaryQueue(string fileName, object state)
        {
            return new BinaryQueueBufferedFile(this, fileName, false, state);
        }

        public LockingLruCache<string, IBufferedFile>.LockingLruCacheItem GetBinaryQueue(string fileName, bool isPersistent)
        {
            LockingLruCache<string, IBufferedFile>.LockingLruCacheItem item;
            if(isPersistent)
                item = this.bufferedFileCache.Retreive(fileName, CreatePersistentBinaryQueue);
            else
                item = this.bufferedFileCache.Retreive(fileName, CreateBinaryQueue);
            return item;
        }

        #endregion

        #region Object Queue

        public BinaryQueueBufferedFile CreatePersistentObjectQueue(string fileName,object state)
        {
            return new ObjectQueueBufferedFile(this, fileName, true, state);
        }

        public ObjectQueueBufferedFile CreateObjectQueue(string fileName, object state)
        {
            return new ObjectQueueBufferedFile(this, fileName, false, state);
        }

        public LockingLruCache<string, IBufferedFile>.LockingLruCacheItem GetObjectQueue(string fileName, bool isPersistent)
        {
            LockingLruCache<string, IBufferedFile>.LockingLruCacheItem item;
            if (isPersistent)
                item = this.bufferedFileCache.Retreive(fileName, CreatePersistentObjectQueue);
            else
                item = this.bufferedFileCache.Retreive(fileName, CreateObjectQueue);
            return item;
        }

        // you can get a file and accept you are getting the pre-existing saved state.
        // you can get a new file asking for specific state... but do not use
        // when you reteive the file from the cache... it will call restore state.

        

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // flush anything that needs to be flushed.
            this.bufferedFileCache.FlushAllUnlockedItems();
            if (this.bufferedFileCache.CountLocked > 0)
            {
                string msg = "Cannot dispose BufferedFileSystem while there are still locked files:";
                msg+=this.bufferedFileCache.LockedItems.JoinStrings(",");
                throw new Exception(msg);
            }
        }

        #endregion



        /// <summary>
        /// Flushes file out of BFS, returning true if it was in BFS, else false. Errors if the 
        /// file is currently locked.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool FlushFile(string file)
        {
            lock (this.bufferedFileCache)
            {
                bool result=this.bufferedFileCache.RemoveSpecificItem(file,true);
                return result;
            }
        }

        public List<string> Dir(string dir)
        {
            lock (this.bufferedFileCache)
            {
                var bufferedFiles = this.bufferedFileCache.GetKeys().Where(k => k.StartsWith(dir)).ToList();
                List<string> physicalFiles = new List<string>();
                if (Directory.Exists(dir)) physicalFiles = Directory.GetFiles(dir).ToList();
                var unionFiles = bufferedFiles.Union(physicalFiles).ToList();
                return unionFiles;
            }
        }


        // File state needs to be stored in the buffered file system, not the lower levels so that you 
        // blindly delete a file and have the delete the state.



        /// <summary>
        /// Deletes a file from the buffered file system and the real file system returns true if 
        /// there was a file in either to delete.
        /// </summary>
        /// <param name="file"></param>
        public bool DeleteFile(string file)
        {
            lock (this.bufferedFileCache)
            {
                bool inFs = false;
                bool inBfs=this.bufferedFileCache.RemoveSpecificItem(file,false);
                FileInfo f = new FileInfo(file);
                if (f.Exists)
                {
                    f.Delete();
                    inFs = true;
                }
                return inBfs || inFs;
            }
        }

        /// <summary>
        /// Deletes dir/* from the buffered file system
        /// </summary>
        /// <param name="dir"></param>
        public void DeleteDirContents(string dir)
        {
            lock (this.bufferedFileCache)
            {
                var bufferedFiles = this.bufferedFileCache.GetKeys().Where(k => k.StartsWith(dir)).ToList();
                // make sure all the buffered files have been cleared out of bfs otherwise, 
                // we might start writing to it again and pickup on the last buffer.
                foreach (string f in bufferedFiles)
                {
                    //logger.Debug("removing buffered file {0}", f);
                    if (!this.bufferedFileCache.RemoveSpecificItem(f,false))
                    {
                        throw new Exception("RemoveSpecificItem failed to find [" + f + "]");
                    }
                }
                List<string> physicalFiles = new List<string>();
                if (Directory.Exists(dir)) physicalFiles = Directory.GetFiles(dir).ToList();
                var unionFiles = bufferedFiles.Union(physicalFiles).ToList();
                foreach (string f in unionFiles)
                {

                    if (File.Exists(f))
                    {
                        logger.Debug("deleting physical file {0}", f);
                        File.Delete(f);
                    }
                }
            }
        }
        
        /// <summary>
        /// Returns fake file info which has length of file on disk
        /// or 0 if file only exists in buffer. TBD, can improve this
        /// to get the file size in the buffer.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public FakeFileInfo GetFileInfo(string file)
        {
            var fi = new FileInfo(file);
            if (fi.Exists)
            {
                return new FakeFileInfo() { Name = file, Length = fi.Length };
            }
            lock (this.bufferedFileCache)
            {
                if (this.bufferedFileCache.ContainsKey(file))
                {
                    return new FakeFileInfo() { Name = file, Length = 0 };
                }
                else
                {
                    throw new Exception("Cannot get file info for unknown file " + file);
                }
            }
        }
        /// <summary>
        /// Returns true if file exists on disk or in bfs.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool FileExists(string file)
        {
            var fi = new FileInfo(file);
            if (fi.Exists)
            {
                return true;
            }
            lock (this.bufferedFileCache)
            {
                if (this.bufferedFileCache.ContainsKey(file))
                {
                    return true;
                }
            }
            return false;
        }
    }
    public class FakeFileInfo{
        public string Name;
        public long Length;

        public static readonly LengthComparerObj LengthComparer = new LengthComparerObj();
        public class LengthComparerObj:IComparer<FakeFileInfo>
        {
            public int Compare(FakeFileInfo x, FakeFileInfo y)
            {
               return x.Length.CompareTo(y.Length);
            }
        }
    }
}
