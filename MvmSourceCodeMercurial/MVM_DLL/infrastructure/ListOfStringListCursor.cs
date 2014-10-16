using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    public class MultiFieldListCursor : CursorCommonLinqEnabled, ICursor
    {
        public List<List<string>> stringList;
        public int stringListIdx = -1;
        public MultiFieldListCursor(ModuleContext mc, CursorSetupCommon cursorSetup, List<string> orderedFieldNames, List<List<string>> stringList)
            : base(mc, cursorSetup)
        {
            this.orderedFieldNames = orderedFieldNames;
            this.stringList = stringList;
        }

        public override IObjectData CursorNext()
        {
                if (this.stringList == null || ++stringListIdx > (stringList.Count - 1))
                {
                    return null;
                }
                using (var csrObj = this.CreateNewObject())
                {
                for (int i = 0; i < this.stringList[this.stringListIdx].Count; i++)
                {
                    csrObj[this.orderedFieldNames[i]] = this.stringList[this.stringListIdx][i];
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
