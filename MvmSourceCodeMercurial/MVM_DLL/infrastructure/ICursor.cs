using System;
using System.Collections.Generic;
using System.Text;
namespace MVM
{
    public interface ICursorBase
    {
        ///
        /// This is a hook for a cursor to free any resources it tied up. 
        /// Called by <cursor_clear>TEMP.csr</cursor_clear> or after cursor sub procs.
        ///
        void Clear(ModuleContext mc);
        ///
        /// This is the instance identifier for the cursor
        ///
        string CursorInstId { get; }
        ///
        /// This is the data object id for the cursor
        ///
        string CursorOid { get; }
        /// <summary>
        /// Returns true if the cursor is at end of file (Eof)
        /// </summary>
        bool Eof { get; }
        /// <summary>
        /// Creates a new cursor data object of the appropriate type. It assume you've 
        /// properly dealt with the previous current object.
        /// </summary>
        /// <returns></returns>
        IObjectData CreateNewObject();
        /// <summary>
        /// Indicates if the cursor should delete the data object on next
        /// </summary>
        //bool DeleteObjectOnNext { get; set; }
    }

    public interface ICursor:ICursorBase
    {
        CursorStatus Next(ModuleContext mc, out IObjectData outputObject);  
}

    public interface ICursorOp : ICursorBase
    {
        CursorStatus Next(ModuleContext mc, IObjectData inputObject, out IObjectData outputObject);  
    }
}
