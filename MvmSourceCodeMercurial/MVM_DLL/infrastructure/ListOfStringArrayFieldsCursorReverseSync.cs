using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    /// <summary>
    /// This class creates a cursor that interates through a <code>List<string[]></code> in reverse order
    /// It is safe for other threads to insert rows while this this thread is iterating through. It is
    /// not safe for other threads to RemoveSpecificItem rows while this iterator is active. Assuming inserts
    /// are appended, this iterator goes in reverse so it will not see new inserts.
    /// 
    /// This locks the list on access.
    /// </summary>
    public class ListOfStringArrayFieldsCursorReverseSync : CursorCommonLinqEnabled, ICursor
    {
        private List<string[]> records;
        private int rowIdx;
        public ListOfStringArrayFieldsCursorReverseSync(ModuleContext mc, ICursorSetupCommon cursorSetup, List<string> orderedFieldNames, List<string[]> rows)
        :base(mc,cursorSetup)
        {
            this.orderedFieldNames = orderedFieldNames;
            this.records = rows;
            this.rowIdx = rows != null ? rows.Count : 0;
        }

        public override IObjectData CursorNext()
        {
            
            // If no more rows set eof
            if (this.records == null || --rowIdx < 0)
            {
                return null;
            }

            // Pull out the next record
            string[] orderedFieldValues;
            lock (this.records)
            {
                orderedFieldValues = this.records[rowIdx];
            }
            
            // Copy the record into the object
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
