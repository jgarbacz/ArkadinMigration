using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    class ListOfStringArrayObjFieldsCursor : CursorCommonLinqEnabled, ICursor
    {
        private List<StringArray> rows;
        private int rowIdx = -1;
        public ListOfStringArrayObjFieldsCursor(ModuleContext mc, ICursorSetupCommon cursorSetup, List<string> orderedFieldNames, List<StringArray> rows)
            : base(mc, cursorSetup)
        {
            this.orderedFieldNames = orderedFieldNames;
            this.rows = rows;

        }

        public override IObjectData CursorNext()
        {

                if (this.rows == null || ++rowIdx > (rows.Count - 1))
                {
                    return null;
                }
                StringArray orderedFieldValues = this.rows[rowIdx];
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
