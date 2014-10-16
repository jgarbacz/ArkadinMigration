using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MVM
{
    // -Interates through 1 or more files
    // -Gives a hook to each line in the file
    // -Configurable line separator
    // -Writes line to specified cursor field.
    public class DelimitedFileCursor : DelimitedRecordCursorWithFormatHeader
    {
        public DelimitedFileCursor(ModuleContext mc, CursorSetupCommon cursorSetup, List<string> files, string recordDelim, string fieldName, string recordOffset, string recordLength, string encoding, bool trim, string format)
            : base(mc, cursorSetup, files, null, recordDelim, new List<string>() { fieldName }, recordOffset, recordLength, encoding, trim, format)
        {
        }

        public override void SetupHeader()
        {
        }

        public override string[] GetFieldArray(string line)
        {
            return new string[] { this.trim ? line.Trim() : line };
        }
    }
}
