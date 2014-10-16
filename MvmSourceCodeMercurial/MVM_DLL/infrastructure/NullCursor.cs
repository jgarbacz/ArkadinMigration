using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    /// <summary>
    /// This cursor always returns eof, but can have orderFieldNames metadata.
    /// </summary>
    public class NullCursor : CursorCommonLinqEnabled, ICursor
    {
        public NullCursor(ModuleContext mc, ICursorSetupCommon cursorSetup
            )
            : base(mc, cursorSetup)
        {
        }

        public NullCursor(ModuleContext mc, ICursorSetupCommon cursorSetup
            , string fieldName)
            : base(mc, cursorSetup)
        {
            this.orderedFieldNames.Add(fieldName);
        }

        public NullCursor(ModuleContext mc, ICursorSetupCommon cursorSetup
            , List<string> orderedFieldNames)
            : base(mc, cursorSetup)
        {
            this.orderedFieldNames = orderedFieldNames;
        }

        public override IObjectData CursorNext()
        {
            return null;
        }

        public override void CursorClear()
        {
            // no resources to free
        }
    }
}
