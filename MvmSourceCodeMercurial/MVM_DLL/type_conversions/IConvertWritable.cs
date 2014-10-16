using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    interface IConvertWritable
    {
        /// <summary>
        /// Converts a writable object that expects a certain type to expect a different type. For example
        /// there might be a writable object that expects a string, a convert is able to make it accept an int.
        /// </summary>
        /// <param name="writable"></param>
        /// <returns></returns>
        IWritable ConvertWritable(IWritable writable);
    }
}
