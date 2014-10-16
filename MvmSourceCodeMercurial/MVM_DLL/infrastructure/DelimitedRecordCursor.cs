using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MVM
{
    // -Interates through 1 or more files
    // -Gives a hook to record fields on each record
    // -Configurable field and record delims
    public class DelimitedRecordCursor : DelimitedRecordCursorWithFormatHeader
    {
        public DelimitedRecordCursor(ModuleContext mc, CursorSetupCommon cursorSetup, List<string> files, string fieldDelim, string recordDelim, List<string> orderedFieldNames, string recordOffset, string recordLength, string encoding, bool trim, string format)
            : base(mc, cursorSetup, files, fieldDelim, recordDelim, orderedFieldNames, recordOffset, recordLength, encoding, trim, format)
        {
        }

        public override void SetupHeader()
        {
        }
    }
}
