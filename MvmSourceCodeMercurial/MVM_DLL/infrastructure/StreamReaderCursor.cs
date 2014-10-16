using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MVM
{
    public class StreamReaderCursor : CursorCommonLinqEnabled, ICursor
    {
        public string cursorFieldName;
        public StreamReader streamReader;
        public StreamReaderCursor(ModuleContext mc, CursorSetupCommon cursorSetup, string cursorFieldName, StreamReader streamReader)
            :base(mc,cursorSetup)
        {
            this.cursorFieldName = cursorFieldName;
            this.streamReader = streamReader;
            this.orderedFieldNames.Add(cursorFieldName);
        }
        public override IObjectData CursorNext()
        {
                if (this.streamReader.EndOfStream)
                {
                    return null;
                }
                using (var csrObj = this.CreateNewObject())
                {
            string streamValue=this.streamReader.ReadLine();
                csrObj[this.cursorFieldName]= streamValue;
                return csrObj;
            }
        }

        public override void CursorClear()
        {
            if (streamReader != null) streamReader.Close();
        }

    }
}
