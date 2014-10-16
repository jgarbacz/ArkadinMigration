using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    interface IConvertReadable
    {
        /// <summary>
        /// Converts something readable into something else that is readable.
        /// </summary>
        /// <param name="readable"></param>
        /// <returns></returns>
        IReadable ConvertReadable(IReadable readable);
    }
}
