using System;
using System.Collections.Generic;
////using System.Linq;
using System.Text;
using System.IO;

namespace MVM
{
    class RecordReaderStandAlone
    {
        public string fileName;
        public string rdel;
        public char[] rdelArr;
        public int rdelMax;
        public int rdelLen;
        public FileStream fs;
        public StreamReader sr;
        // char buffer
        public char[] cb = new char[8192];
        // current index into in resumeProcInst
        public int cbPos = 0;
        // useful length of resumeProcInst
        public int cbLen = 0; 

        public RecordReaderStandAlone(string fileName, string recordDelim)
        {
            this.fileName = fileName;
            this.rdel = recordDelim;
            this.rdelArr = recordDelim.ToCharArray();
            this.rdelLen = this.rdelArr.Length;
            this.rdelMax = this.rdelLen - 1;
            //Console.WriteLine("opening file: " + fileName);
            this.fs = new FileStream(this.fileName,FileMode.Open,FileAccess.Read);
            this.sr = new StreamReader(this.fs, Encoding.UTF8, false);
        }

        // Seeks to the logical byte position in the file
        public void Seek(long byteOffset)
        {
            this.fs.Seek(byteOffset,SeekOrigin.Begin);
            this.cbLen = 0;
            this.cbPos = 0;
        }

        // Returns a line without its record delimiter
        //let resumeProcInst grow accomodate the max len of record
        public string ReadLine()
        {
            // mark the start of the record
            int cbMrk = cbPos;
            // track delimiter matching
            int rdelPos = 0;
            for (; ; )
            {
                // if char buffer is empty
                if (this.cbPos >= this.cbLen)
                {
                    // if we need to extend our buffer to accomodate the record, do so now
                    if (cbMrk == 0 && cbLen == cb.Length)
                    {
                        char[] extendedCb = new char[cb.Length * 2];
                        System.Array.Copy(cb, extendedCb, cbLen);
                        cb = extendedCb;
                        continue;
                    }
                    // compact the buffer
                    if (cbMrk > 0)
                    {
                        cbLen =cbPos - cbMrk;
                        System.Array.Copy(cb, cbMrk, cb, 0, cbLen);
                        cbPos = cbPos - cbMrk; 
                        cbMrk = 0;
                    }
                    // load the buffer starting at current position
                    int readCnt=this.sr.Read(cb, cbPos, cb.Length - cbPos);
                    // if we didn't get anything
                    if (readCnt == 0)
                    {
                        //if (rdelPos == 0) return null;
                        if ((cbLen - cbMrk) == 0) return null;
                        string line = new string(cb, cbMrk, cbLen - cbMrk);
                        throw new Exception("incomplete last line ["+line+"]");
                    } 
                    cbLen = cbPos + readCnt;
                }

                // get current char
                char c = this.cb[this.cbPos];

                // if we think we're in a delim
                if (rdelPos > 0)
                {
                    if (c.Equals(this.rdelArr[rdelPos]))
                    {
                        if (rdelPos == rdelMax)
                        {
                            int len = cbPos - cbMrk + 1 - rdelLen;
                            string line = new string(this.cb, cbMrk, len);
                            this.cbPos++;
                            cbMrk = this.cbPos;
                            return line;
                        }
                        this.cbPos++;
                        rdelPos++;
                        continue;
                    }
                    else
                    {
                        // roll back
                        cbPos = this.cbPos - rdelPos + 1;
                        rdelPos = 0;
                        continue;
                    }
                }
                if (c.Equals(rdelArr[rdelPos])) 
                    rdelPos += 1;
                this.cbPos++;
            }
        }

        public void Close()
        {
            if (this.fs != null) this.fs.Close();
            if (this.sr != null) this.sr.Close();
        }

        public static void Test()
        {
            string file = "C:\\_ROB\\mvm\\test_record_reader\\input.txt";
            RecordReaderStandAlone rr = new RecordReaderStandAlone(file, "<\r\n");
            string ln;
            while ((ln = rr.ReadLine()) != null)
            {
                Console.WriteLine("[" + ln + "]");
            }
            Console.WriteLine("DONE!");
        }
    }

}
