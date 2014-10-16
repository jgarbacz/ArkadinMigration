using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    public class ListOfStringCursor : CursorCommonLinqEnabled, ICursor
    {
        public string cursorFieldName;
        public List<string> stringList;
        public int stringListIdx = -1;
        public ListOfStringCursor(ModuleContext mc, ICursorSetupCommon cursorSetup, string fieldName, List<string> stringList)
            : base(mc, cursorSetup)
        {
            this.orderedFieldNames = new List<string> { fieldName }; ;
            this.stringList = stringList;
        }

        public override IObjectData CursorNext()
        {
            
                if (this.stringList == null || ++this.stringListIdx > (stringList.Count - 1))
                {
                    return null;
                }
                using (var csrObj = this.CreateNewObject())
                {
                csrObj[this.orderedFieldNames[0]] = this.stringList[this.stringListIdx];
                return csrObj;
            }
        }

        public override void CursorClear()
        {
            // no resources to free
        }
    }
}
