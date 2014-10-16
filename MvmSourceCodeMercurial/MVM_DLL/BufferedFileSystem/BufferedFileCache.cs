using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    /// <summary>
    /// LRU Locking cache used to store IBufferedFiles.
    /// </summary>
    public class BufferedFileCache : LockingLruCache<string, IBufferedFile>
    {
        private readonly BufferedFileSystem bufferedFileSystem;
        
        public BufferedFileCache(BufferedFileSystem bufferedFileSystem)
            : base(bufferedFileSystem.maxOpenFiles)
        {
            this.bufferedFileSystem = bufferedFileSystem;
        }

        protected override object FlushItem(LockingLruCache<string, IBufferedFile>.LockingLruCacheItem item,bool keepState)
        {
            IBufferedFile bufferedFile = item.value;
            return bufferedFile.FlushToFile(keepState);
        }
        
        
    }
}
