using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    public interface IReadInt
    {
        /// <summary>
        /// This runtime method evaluates syntax and returns the resulting int.
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        int Read(ModuleContext mc);
    }

    
}
