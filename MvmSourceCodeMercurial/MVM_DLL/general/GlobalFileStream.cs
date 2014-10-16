using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    public class GlobalFileStream
    {
        private GlobalFileReader reader;
        private long position;
        
        public GlobalFileStream(string fileName)
        {
            this.reader = GlobalFileReader.GetReader(fileName);
        }

        public void Seek(long position)
        {
            this.position = position;
        }

        public int Read(byte[] array, int offset, int count)
        {
            int bytesRead = 0;
            int bytesNeeded = count;
            Block block;
            int blockOffset;
            while (bytesNeeded>0)
            {
                this.reader.GetBlock(this.position, out block, out blockOffset);
                int bytesAvail = block.length - blockOffset; // max len
                if (bytesAvail <= 0) break;
                if (bytesAvail > bytesNeeded) bytesAvail = bytesNeeded;
                Array.Copy(block.buf, blockOffset, array, offset, bytesAvail);
                offset += bytesAvail;
                bytesRead += bytesAvail;
                bytesNeeded -= bytesAvail;
                this.position += bytesAvail;
            }
            return bytesRead;
        }

        public void Close()
        {
            if (this.reader != null) this.reader.Close();
        }
    }
}
