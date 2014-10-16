using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    class ListOfStringArrayFieldsCursorReverse : CursorCommonLinqEnabled, ICursor
    {
        private List<string[]> rows;
        private int rowIdx;
        public ListOfStringArrayFieldsCursorReverse(ModuleContext mc, ICursorSetupCommon cursorSetup, List<string> orderedFieldNames, List<string[]> rows)
            : base(mc, cursorSetup)
        {
            this.orderedFieldNames = orderedFieldNames;
            this.rows = rows;
            this.rowIdx = rows!=null?rows.Count:0;
        }
        public override IObjectData CursorNext()
        {
                if (this.rows == null || --rowIdx < 0)
                {
                    return null;
                }
                string[] orderedFieldValues = this.rows[rowIdx];
                using (var csrObj = this.CreateNewObject())
                {
                for (int i = 0; i < this.orderedFieldNames.Count; i++)
                {
                    csrObj[this.orderedFieldNames[i]] = orderedFieldValues[i];
                }
                return csrObj;
            }
        }
        public override void CursorClear()
        {
            // no resources to free
        }
    }
}
