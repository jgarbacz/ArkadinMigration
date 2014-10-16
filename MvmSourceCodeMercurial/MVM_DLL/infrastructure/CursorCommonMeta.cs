using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public abstract class CursorCommonMeta : CursorCommon
    {
        public CursorCommonMeta(ModuleContext mc, ICursorSetupCommon cursorSetup)
            :base(mc,cursorSetup)
        {
        }
        
        /// <summary>
        /// Cursor metadata
        /// </summary>
        public List<string> orderedFieldNames { get; set; }

        /// <summary>
        /// Returns the field name metadata if it was set.
        /// </summary>
        /// <returns></returns>
        public List<string> GetOrderedFieldNames()
        {
            return this.orderedFieldNames;
        }   
    }
}
