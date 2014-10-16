using System;
using System.Collections.Generic;

using System.Text;
using System.IO;

namespace MVM
{
    // The approach in this class is to parse the record as bytes and then convert to 
    // chars. This has the approach that we can track our logical byte file position as we 
    // go. This is useful for seeking into a file and scanning for the next record. This
    // only works for character sets like ascii, UTF8, or UTF16 (if we only seek to even bytes)
    public class RecordReader : MVMReader
    {
        public string fileName;
        public string rdel;
        public byte[] rdelArr;
        public int rdelMax;
        public int rdelLen;
        //public FileStream fileStream;
        public GlobalFileStream fs;
        // logical file position in bytes
        public long logicalPos=0;
        // byte buffer
        public byte[] bb = new byte[8192];
        // current index into in bb
        public int bbPos = 0;
        // useful length of bb
        public int bbLen = 0; 

        public RecordReader(string fileName, string recordDelim)
        {
            this.fileName = fileName;
            this.rdel = recordDelim;
            System.Text.UTF8Encoding decoder = new System.Text.UTF8Encoding();
            this.rdelArr = decoder.GetBytes(recordDelim);
            this.rdelLen = this.rdelArr.Length;
            this.rdelMax = this.rdelLen - 1;
            this.fs = new GlobalFileStream(this.fileName);
            //this.fileStream = new FileStream(this.fileName, FileMode.Open, FileAccess.Read);
        }

        // returns position of next byte to be read
        public long Position()
        {
            return logicalPos;
        }

        // Seeks to the next logical byte position in the file to read seek(0) read first byte (i think)
        public void Seek(long byteOffset)
        {
            // Do not seek if we're still in the buffer
            long minLogicalPos = this.logicalPos - this.bbPos;
            long maxLogicalPos =  minLogicalPos + this.bbLen - 1;
            if (minLogicalPos <= byteOffset && byteOffset <= maxLogicalPos)
            {
                this.bbPos = (int)(byteOffset - minLogicalPos);
                this.logicalPos = byteOffset;
                return;
            }

            //ORIG
            //this.fileStream.Seek(byteOffset,SeekOrigin.Begin);
            this.fs.Seek(byteOffset);
            this.bbLen = 0;
            this.bbPos = 0;
            this.logicalPos = byteOffset;
        }

        // Returns a line without its record delimiter
        //let resumeProcInst grow accomodate the max len of record
        public string ReadLine()
        {
            //Console.WriteLine("start logical pos[" + this.logicalPos + "]");
            //if (this.logicalPos == 16232)
            //{
            //    Console.WriteLine("break");
            //}
            // mark the start of the record
            int cbMrk = bbPos;
            // track delimiter matching
            int rdelPos = 0;
            for (; ; )
            {
                // if char buffer is empty
                if (this.bbPos >= this.bbLen)
                {
                    // if we need to extend our buffer to accomodate the record, do so now
                    if (cbMrk == 0 && bbLen == bb.Length)
                    {
                        byte[] extendedCb = new byte[bb.Length * 2];
                        System.Array.Copy(bb, extendedCb, bbLen);
                        bb = extendedCb;
                        continue;
                    }
                    // compact the buffer
                    if (cbMrk > 0)
                    {
                        bbLen =bbPos - cbMrk;
                        System.Array.Copy(bb, cbMrk, bb, 0, bbLen);
                        bbPos = bbPos - cbMrk; 
                        cbMrk = 0;
                    }
                    // load the buffer starting at current position
                    int readCnt=this.fs.Read(bb, bbPos, bb.Length - bbPos);
                    // if we didn't get anything
                    if (readCnt == 0)
                    {
                        int len=bbLen - cbMrk;
                        if (rdelPos == 0) return null;
                        logicalPos += len;
                        string line = System.Text.UTF8Encoding.UTF8.GetString(this.bb, cbMrk, len);
                        throw new Exception("incomplete last line ["+line+"]");
                    } 
                    bbLen = bbPos + readCnt;
                }

                // get current char
                byte c = this.bb[this.bbPos];

                // if we think we're in a delim, but we don't match next byte, rollback
                if (rdelPos > 0 && !c.Equals(this.rdelArr[rdelPos]))
                {
                    bbPos = this.bbPos - rdelPos + 1;
                    rdelPos = 0;
                    continue;
                }

                // check the record delim
                if (c.Equals(rdelArr[rdelPos]))
                {
                    if (rdelPos == rdelMax)
                    {
                        int len = bbPos - cbMrk + 1;
                        logicalPos += len;
                        len -= rdelLen;
                        string line = System.Text.UTF8Encoding.UTF8.GetString(this.bb, cbMrk, len);
                        //Console.WriteLine("line[" + line + "]");
                        this.bbPos++;
                        cbMrk = this.bbPos;
                        return line;
                    }
                    // just increment to the next spot
                    rdelPos += 1;
                }
                this.bbPos++;
            }
        }

        public void Close()
        {
            if (this.fs != null) this.fs.Close();
        }

        public static void Test()
        {
            string file = "C:\\_ROB\\mvm\\test_record_reader\\input.txt";
            RecordReader rr = new RecordReader(file, "<\r\n");
            string ln;
            while ((ln = rr.ReadLine()) != null)
            {
                Console.WriteLine("[" + ln + "]");
            }
            Console.WriteLine("DONE!");
        }

        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion
    }

}
