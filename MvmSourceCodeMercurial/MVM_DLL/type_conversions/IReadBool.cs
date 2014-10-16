using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    public interface IReadBool
    {
        /// <summary>
        /// This runtime method evaluates syntax and returns the resulting bool.
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        bool Read(ModuleContext mc);
    }

    
}
