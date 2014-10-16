using System;
using System.Collections.Generic;

using System.Text;
using System.IO;

namespace MVM
{
    public class Block{
        public byte[] buf;                  // byte buffer
        public int length;                  // length of useful data in readBuf
        public int size;                    // readBuf.length
        public long blockFileOffset;        // offset in file for start of block
        public Block(long blockFileOffset,int size)
        {
            this.blockFileOffset = blockFileOffset;
            this.size=size;
            this.buf = new byte[this.size];
            this.length = 0;
        }
    }

    /**
     * This class provides a queue of read buffer blocks on top of a file. We created this class
     * because when we seek into files we are often seeking into close positions, but not necessarily sequential
     * positions. This greatly increases the number of times we hit the block cache versus having to read a new
     * block from disk.
     * 
     * TBD: Class in NOT threadContext safe.
     */
    public class GlobalFileReader:IDisposable
    {

        // static access that caches the readers
        public static Dictionary<string, GlobalFileReader> Readers = new Dictionary<string, GlobalFileReader>();
        public static GlobalFileReader GetReader(string fileName){
            if(Readers.ContainsKey(fileName)){
                return Readers[fileName];
            }
            GlobalFileReader reader=new GlobalFileReader(fileName);
            Readers[fileName]=reader;
            return reader;
        }

        private static void RemoveReader(string fileName)
        {
            if (Readers.ContainsKey(fileName))
            {
                GlobalFileReader reader = new GlobalFileReader(fileName);
                reader.Close();
                Readers.Remove(fileName);
            }
        }

        // members
        private string fileName;
        private FileStream fs;
        private Dictionary<long, Block> offsetBlockMap = new Dictionary<long, Block>();
        private Queue<Block> blockQueue = new Queue<Block>();
        private int blockSize = 8192*10;
        private int maxBlocks = 10;

        // creates a new GlobalFileReader.
        public GlobalFileReader(string fileName)
        {
            this.fileName = fileName;
            this.fs = new FileStream(this.fileName, FileMode.Open, FileAccess.Read,FileShare.ReadWrite);
        }

        // tell it the file offset and it returns you a block and a block offset to start reading
        // if it returns a block.length==zero you hit EOF.
        // TBD: synchonize
        public void GetBlock(long fileOffset, out Block block, out int blockOffset){
            
            long blockFileOffset = (fileOffset / blockSize) * blockSize;
            blockOffset = (int)(fileOffset - blockFileOffset);
            if (offsetBlockMap.ContainsKey(blockFileOffset))
            {
                block = this.offsetBlockMap[blockFileOffset];
                return;
            }
            //Console.WriteLine(fileName + ":" + fileOffset);
            block = new Block(blockFileOffset,blockSize);
            this.fs.Position = blockFileOffset;
            block.length = this.fs.Read(block.buf, 0, block.size);
            if (this.blockQueue.Count > this.maxBlocks)
            {
                var rmBlock=this.blockQueue.Dequeue();
                this.offsetBlockMap.Remove(rmBlock.blockFileOffset);
            }
            this.blockQueue.Enqueue(block);
            this.offsetBlockMap[block.blockFileOffset] = block;
        }

        // forces the file to be closed, releasing any resources
        public void Close()
        {
            if (this.offsetBlockMap != null)
            {
                offsetBlockMap.Clear();
                offsetBlockMap = null;
            }
            if (this.blockQueue != null)
            {
                blockQueue.Clear();
                blockQueue = null;
            }
            if (this.fs != null)
            {
                fs.Close();
                fs = null;
            }
            Readers.Remove(this.fileName);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion
    }
}
