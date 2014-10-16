using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public interface IBufferedFile
    {
        /// <summary>
        /// Flush the file to disk, closing file handles, return null or a state object that needs
        /// to be saved to resume next time the file is opened. 
        /// </summary>
        object FlushToFile(bool keepState);
    }
}
