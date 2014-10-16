using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    public class ListOfStringArrayFieldsCursor : CursorCommonLinqEnabled, ICursor
    {
        private List<string[]> rows;
        private int rowIdx = -1;
        public ListOfStringArrayFieldsCursor(ModuleContext mc, ICursorSetupCommon cursorSetup, List<string> orderedFieldNames, List<string[]> rows)
        :base(mc,cursorSetup){
            this.orderedFieldNames = orderedFieldNames;
            this.rows = rows;
        }

        public override IObjectData CursorNext()
        {
                if (this.rows == null || ++rowIdx > (rows.Count - 1))
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
