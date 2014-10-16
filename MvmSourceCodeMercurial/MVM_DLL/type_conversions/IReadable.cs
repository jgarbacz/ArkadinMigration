using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    public interface IReadable
    {
        /// <summary>
        /// This runtime method evaluates syntax and returns the resulting object.
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        object ReadObject(ModuleContext mc);
        /// <summary>
        /// This runtime method tells you the System.Type of the object that will be returned when
        /// you call <code>ReadObject()</code>. It is called during setup to determin if we need
        /// to wrap this IReadable object with some IReadable type conversion.
        /// </summary>
        /// <returns></returns>
        System.Type GetReadType();
    }
}
