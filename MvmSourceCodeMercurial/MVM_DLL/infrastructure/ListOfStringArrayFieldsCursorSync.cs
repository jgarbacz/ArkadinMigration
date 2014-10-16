using System;
using System.Collections.Generic;
using System.Text;
namespace MVM
{
    class ListOfStringArrayFieldsCursorSync : CursorCommonLinqEnabled, ICursor
    {
        private List<string[]> records;
        private int rowIdx = -1;
        public ListOfStringArrayFieldsCursorSync(ModuleContext mc, ICursorSetupCommon cursorSetup, List<string> orderedFieldNames, List<string[]> rows)
            : base(mc, cursorSetup)
        {
            this.orderedFieldNames = orderedFieldNames;
            this.records = rows;
        }

        public override IObjectData CursorNext()
        {
            
                // If no more rows set eof
                if (this.records == null || ++rowIdx > (records.Count - 1))
                {
                    return null;
                }

                // Pull out the next record
                string[] orderedFieldValues;
                lock (this.records)
                {
                    orderedFieldValues = this.records[rowIdx];
                }
                using (var csrObj = this.CreateNewObject())
                {
                // Copy the record into the object
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
