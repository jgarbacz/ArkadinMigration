using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public enum CursorStatus
    {
        /**
         * Will not return any more objects.
         */
        EOF,
        /**
         * Return 1 object. No callback.
         */
        HAS_ROW,
        /**
        * Return 1 object. 
        */
        //HAS_MANY_ROWS,
        /**
         * Returned no rows. Request input new row from parent.
         */
        PARENT_NEXT,
        /**
         * Cursor/op needs to yield and already setup the callback. When
         * you resume, you should recall the cursor/op in exactly the same way.
         * It is up to the cursor/op to deal with its state internally.
         */
        YIELD
    }
}
