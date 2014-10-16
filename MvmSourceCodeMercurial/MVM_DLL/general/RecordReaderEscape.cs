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
    class RecordReaderEscape:IDisposable
    {
        public string fileName;
        public string rdel;
        public byte[] rdelArr;
        public int rdelMax;
        public int rdelLen;
        public FileStream fs;
        // logical file position
        public long logicalPos;
        // byte buffer
        public byte[] bb = new byte[8192];
        // current index into in bb
        public int bbPos = 0;
        // useful length of bb
        public int bbLen = 0;
        //public static byte escape = new System.Text.UTF8Encoding().GetBytes("\\")[0];
        public byte escape;
        public Encoding encoder;
        public RecordReaderEscape(string fileName, string recordDelim)
        {
            
            //this.encoder=Encoding.GetEncoding(1252);// windows default
            //this.encoder = Encoding.GetEncoding(819);//intercall ace
            this.encoder = Encoding.GetEncoding("ISO8859-1");

            this.fileName = fileName;
            this.rdel = recordDelim;
            //System.Text.UTF8Encoding decoder = new System.Text.UTF8Encoding();
            this.escape = encoder.GetBytes("\\")[0];
            this.rdelArr = encoder.GetBytes(recordDelim);
            this.rdelLen = this.rdelArr.Length;
            this.rdelMax = this.rdelLen - 1;
            this.fs = new FileStream(this.fileName,FileMode.Open,FileAccess.Read);
        }

        // returns position of next byte to be read
        public long Position
        {
            get
            {
                return logicalPos;
            }
        }

        // Seeks to the logical byte position in the file
        public void Seek(long byteOffset)
        {
            this.fs.Seek(byteOffset,SeekOrigin.Begin);
            this.bbLen = 0;
            this.bbPos = 0;
            this.logicalPos = byteOffset;
        }

        // Returns a line without its record delimiter
        //let resumeProcInst grow accomodate the max len of record
        public string ReadLine()
        {
            bool escapeOn = false;
            bool nextEscapeOn = false;
            // mark the start of the record
            int cbMrk = bbPos;
            // track delimiter matching
            int rdelPos = 0;
            for (; ; )
            {
                // set escapeOn
                escapeOn = nextEscapeOn;

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
                        if (rdelPos == 0) return null;
                        int len=bbLen - cbMrk;
                        logicalPos += len;
                        string line = this.encoder.GetString(this.bb, cbMrk, len);
                        //string line = System.Text.UTF8Encoding.UTF8.GetString(this.bb, cbMrk, len);
                        throw new Exception("incomplete last line ["+line+"]");
                    } 
                    bbLen = bbPos + readCnt;
                }

                // get current char
                byte c = this.bb[this.bbPos];
                //Console.WriteLine("bbpos="+this.bbPos + " b=[" + c + "]" + " c=[" + System.Text.UTF8Encoding.UTF8.GetString(new byte[] { c }) + "]" + " rdel_pos=[" + rdelPos + "]" + " escapeOn=[" + escapeOn + "]");

                // if we think we're in a delim, but we don't match next byte, rollback
                if (rdelPos > 0 && !c.Equals(this.rdelArr[rdelPos]))
                {
                    bbPos = this.bbPos - rdelPos + 1;
                    rdelPos = 0;
                    continue;
                }

                // set next escape on
                nextEscapeOn = c == escape ? !escapeOn : false;

                // check the record delim
                if ((!escapeOn) && c.Equals(rdelArr[rdelPos]))
                {
                    if (rdelPos == rdelMax)
                    {
                        int len = bbPos - cbMrk + 1;
                        logicalPos += len;
                        len -= rdelLen;

                        //for (int i = 0; i < len; i++) Console.WriteLine(this.bb[cbMrk + i]);
                        //Console.WriteLine();
                        string line = this.encoder.GetString(this.bb, cbMrk, len);
                        //string line = System.Text.UTF8Encoding.UTF8.GetString(this.bb, cbMrk, len);
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
            //if (this.sr != null) this.sr.FlushToFile();
        }

        public static void Test()
        {



            Console.WriteLine("DEFAULT ENCODING="+Encoding.Default.EncodingName);
            Console.WriteLine("DEFAULT ENCODING=" + Encoding.Default.CodePage);
            //string file = "C:\\_ROB\\mvm\\test_record_reader_escape\\input.txt";
            //RecordReaderEscape rr = new RecordReaderEscape(file, "<\r\n");
            StreamWriter cvtSw = new StreamWriter("C:/_MIGRATION/acct_mig/data/input_data/ROBOUT.txt",false,Encoding.Default);
            //string file = "C:/_MIGRATION/acct_mig/data/input_data/mbreuer.txt";
            string file = "C:/_MIGRATION/acct_mig/data/input_data/gonz.txt";
            RecordReaderEscape rr = new RecordReaderEscape(file, "|\r\n");
            string ln;
            while ((ln = rr.ReadLine()) != null)
            {
                cvtSw.WriteLine(ln);
                Console.WriteLine("[" + ln + "]");
            }
            Console.WriteLine("DONE!");
            cvtSw.Close();
        }

        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion
    }

}
