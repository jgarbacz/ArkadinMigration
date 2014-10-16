using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace MVM
{
    // -Interates through 1 or more files
    // -Gives a hook to record fields on each record
    // -Configurable field and record delims
    public class DelimitedRecordCursorWithFormatHeader : CursorCommonLinqEnabled, ICursor
    {
        // set by constructor
        protected bool trim = false;
        protected bool allowInvalidRecords = false;
        protected string format;
        protected List<string> files = new List<string>();
        protected string fieldDelim;
        protected string recordDelim;
        protected string[] fieldDelimArray;

        // keeps state between calls to next
        protected int filesIdx = -1;
        protected string file = "";
        protected MVMReader reader = null;
        protected long startOffset = 0;
        protected string recordOffsetField;
        protected string recordLengthField;
        protected string encoding;

        // constructor
        public DelimitedRecordCursorWithFormatHeader(ModuleContext mc, CursorSetupCommon cursorSetup, List<string> files, string fieldDelim, string recordDelim, List<string> orderedFieldNames, string recordOffset, string recordLength, string encoding, bool trim, string format)
            : this(mc, cursorSetup, files, fieldDelim, recordDelim, recordOffset, recordLength, encoding, trim, format)
        {
            this.orderedFieldNames = orderedFieldNames;
        }

        public DelimitedRecordCursorWithFormatHeader(ModuleContext mc, CursorSetupCommon cursorSetup, List<string> files, string fieldDelim, string recordDelim, string recordOffset, string recordLength, string encoding, bool trim, string format)
            : base(mc, cursorSetup)
        {
            this.files = files;
            this.fieldDelim = fieldDelim;
            this.fieldDelimArray = new string[] { this.fieldDelim };
            this.recordDelim = recordDelim;
            this.recordOffsetField = recordOffset;
            this.recordLengthField = recordLength;
            this.encoding = encoding;
            this.orderedFieldNames = new List<string>();
            this.trim = trim;
            this.format = format;
            this.allowInvalidRecords = mc.globalContext["allow_invalid_records"].Equals("1");
        }

        public virtual void SetupHeader()
        {
            string header = this.reader.ReadLine();
            string[] headerValues = header.Split(fieldDelimArray, StringSplitOptions.None);
            if (this.trim)
            {
                for (int i = 0; i < headerValues.Length; i++)
                {
                    headerValues[i] = headerValues[i].Trim();
                }
            }
            this.orderedFieldNames.Clear();
            this.orderedFieldNames.AddRange(headerValues);
        }

        public override IObjectData CursorNext()
        {
            string line = null;
            for (; ; )
            {
                if (reader != null) line = reader.ReadLine();
                if (line == null)
                {
                    if (this.filesIdx >= (this.files.Count - 1))
                    {
                        return null;
                    }
                    this.filesIdx++;
                    this.file = this.files[this.filesIdx];
                    //Console.WriteLine("Opening file [" + this.file + "]");
                    if (this.encoding.IsNullOrEmpty())
                    {
                        this.reader = new RecordReader(file, this.recordDelim);
                    }
                    else
                    {
                        this.reader = new TextOffsetReader(file, Encoding.GetEncoding(encoding), recordDelim);
                    }
                    startOffset = 0;
                    SetupHeader();
                    continue;
                }
                string[] fieldValues = this.GetFieldArray(line);
                using (var csrObj = this.CreateNewObject(this.format))
                {
                    for (int i = 0; i < this.orderedFieldNames.Count; i++)
                    {
                        csrObj[this.orderedFieldNames[i]] = fieldValues[i];
                    }
                    if (this.allowInvalidRecords && fieldValues.Length > orderedFieldNames.Count)
                    {
                        for (int i = orderedFieldNames.Count; i < fieldValues.Length; i++)
                        {
                            csrObj["[missing field " + i.ToString() + "]"] = fieldValues[i];
                        }
                    }
                    if (this.recordOffsetField != null)
                    {
                        long currentPos = reader.Position();
                        csrObj[this.recordOffsetField] = startOffset.ToString();
                        if (this.recordLengthField != null)
                        {
                            csrObj[this.recordLengthField] = (currentPos - startOffset).ToString();
                        }
                        startOffset = currentPos;
                    }
                    return csrObj;
                }
            }
        }

        public virtual string[] GetFieldArray(string line)
        {
            string[] fieldValues;
            if (this.trim)
            {
                fieldValues = line.Split(fieldDelimArray, StringSplitOptions.None).Select(s => s.Trim()).ToArray();
            }
            else
            {
                fieldValues = line.Split(fieldDelimArray, StringSplitOptions.None);
            }
            if (fieldValues.Length != orderedFieldNames.Count)
            {
                if (this.allowInvalidRecords)
                {
                    if (fieldValues.Length < orderedFieldNames.Count)
                    {
                        string[] newFieldValues = new string[orderedFieldNames.Count];
                        Array.Copy(fieldValues, newFieldValues, fieldValues.Length);
                        for (int i = fieldValues.Length; i < newFieldValues.Length; i++)
                        {
                            newFieldValues[i] = "[missing value]";
                        }
                        fieldValues = newFieldValues;
                    }
                }
                else
                {
                    StringBuilder error = new StringBuilder();
                    error.AppendLine();
                    for (int i = 0; i < Math.Max(fieldValues.Length, orderedFieldNames.Count); i++)
                    {
                        error.AppendLine((i < orderedFieldNames.Count ? orderedFieldNames[i] : "[missing field]") + ": " + (i < fieldValues.Length ? fieldValues[i] : "[missing value]"));
                    }
                    throw new Exception("Wrong number of fields in file " + this.file + ". Expecting [" + orderedFieldNames.Count + "], found [" + fieldValues.Length + "], data: " + error);
                }
            }
            return fieldValues;
        }

        public override void CursorClear()
        {
            if (this.reader != null)
            {
                this.reader.Close();
                this.reader = null;
            }
        }
    }
}
