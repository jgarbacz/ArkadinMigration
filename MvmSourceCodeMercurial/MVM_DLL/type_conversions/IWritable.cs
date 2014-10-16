using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    public interface IWritable
    {
        /// <summary>
        /// This runtime method writes the passed object to the location it was setup to write to.
        /// The passed value must already be the type of <code>this.GetWriteType()</code>.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        object WriteObject(ModuleContext mc, object value);
        /// <summary>
        /// This runtime method writes the passed object to the location it was setup to write to.
        /// The passed <code>value.Read(mc)</code> must return the type of <code>this.GetWriteType()</code>.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        object WriteObject(ModuleContext mc, IReadable value);
        /// <summary>
        /// Returns the <code>System.Type</code> this object expect to write.
        /// </summary>
        /// <returns></returns>
        System.Type GetWriteType();
    }
}
