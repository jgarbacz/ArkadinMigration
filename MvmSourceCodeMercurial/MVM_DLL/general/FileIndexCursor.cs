using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MVM
{
    public class FileIndexCursor : CursorCommonLinqEnabled, ICursor
    {
        private FileIndex fileIndex;
        private long offset;
        private RecordReader rr;
        private string fieldDelim;
        private string recordDelim;
        private string[] fieldDelimArray;
        private List<int> keyFieldColNos;
        private List<string> keyFieldValues;

        // constructor
        public FileIndexCursor(ModuleContext mc, ICursorSetupCommon cursorSetup, FileIndex fileIndex, long offset, string fieldDelim, string recordDelim, List<string> orderedFieldNames, List<int> keyFieldColNos, List<string> keyFieldValues)
            : base(mc, cursorSetup)
        {
            this.keyFieldColNos = keyFieldColNos;
            this.keyFieldValues = keyFieldValues;
            this.fieldDelim = fieldDelim;
            this.fieldDelimArray = new string[] { fieldDelim };
            this.recordDelim = recordDelim;
            this.orderedFieldNames = orderedFieldNames;
            this.fileIndex = fileIndex;
            this.offset = offset;
            this.rr = new RecordReader(this.fileIndex.fileName, recordDelim);
            rr.Seek(this.offset);
        }


        public override IObjectData CursorNext()
        {
            string line;
            for (; ; )
            {
                line = rr.ReadLine();
                //line = tr.ReadLine();
                if (line == null)
                {
                    return null;
                }
                   
                // split up the line into fields and make sure we got the right number of them
                string[] fieldValues = line.Split(fieldDelimArray, StringSplitOptions.None);
                if (fieldValues.Length != orderedFieldNames.Count)
                {
                    //TBD: uncomment this
                    Console.WriteLine("Wrong number of fields. Expecting [" + orderedFieldNames.Count + "], found [" + fieldValues.Length + "] data=[" + line + "]");
                    Console.WriteLine("fileName= [" + this.fileIndex.fileName + "]");
                    Console.WriteLine("offset= [" + this.offset + "]");
                    Console.WriteLine("fieldDelim= [" + this.fieldDelim + "]");
                    Console.WriteLine("recordDelim= [" + this.recordDelim + "]");
                    throw new Exception("Wrong number of fields. Expecting [" + orderedFieldNames.Count + "], found [" + fieldValues.Length + "] data=[" + line + "]");
                }
                    
                // make sure we're still on a row for the same key
                    for(int i=0;i<keyFieldColNos.Count;i++)
                {
                    int colNo = keyFieldColNos[i];
                    string keyFieldValue = keyFieldValues[i];
                    string datFieldValue = fieldValues[colNo];
                    if (!keyFieldValue.Equals(datFieldValue))
                    {
                        return null;
                    }
                }

                // update our offset for next time
                this.offset += line.Length + 2;
                    
                // copy into the cursor
                // TBD convert this to syntax so we get array tuning
                using (var csrObj = this.CreateNewObject())
                {
                    for (int i = 0; i < this.orderedFieldNames.Count; i++)
                    {
                        csrObj[this.orderedFieldNames[i]] = fieldValues[i];
                    }
                    return csrObj;
                }
            }
        }

        public override void CursorClear()
        {
            if (this.rr != null) this.rr.Close();
        }
    }
}
