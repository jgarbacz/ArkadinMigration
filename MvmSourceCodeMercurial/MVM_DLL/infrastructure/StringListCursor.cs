using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    public class SingleFieldListCursor : CursorCommonLinqEnabled, ICursor
    {
        public string cursorFieldName;
        public List<string> stringList;
        public int stringListIdx = -1;
        public SingleFieldListCursor(ModuleContext mc, CursorSetupCommon cursorSetup, List<string> stringList)
            : base(mc, cursorSetup)
        {
            this.cursorFieldName = cursorSetup.GetCursorValue(mc);
            this.stringList = stringList;
            this.orderedFieldNames=new List<string>{cursorFieldName};
        }

        public override IObjectData CursorNext()
        {

                stringListIdx++;
                if (this.stringList!=null&&stringListIdx < this.stringList.Count)
                {
                    using (var csrObj = this.CreateNewObject())
                    {
                    csrObj[this.cursorFieldName]= this.stringList[this.stringListIdx];
                    return csrObj;
                    }
                }
                return null;

        }

        public override void CursorClear()
        {
            // no resources to free
        }
}
}
