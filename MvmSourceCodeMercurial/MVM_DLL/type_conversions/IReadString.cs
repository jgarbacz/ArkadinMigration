using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    public interface IReadString:IReadable
    {
        /// <summary>
        /// This runtime method evaluates syntax and returns the resulting string.
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        string Read(ModuleContext mc);
    }

    
}
