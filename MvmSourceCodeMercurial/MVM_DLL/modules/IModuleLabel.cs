using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{

    /// <summary>
    /// If a newModule implements this iterface other newModules in the same
    /// proc can jump directly to it.
    /// </summary>
    public interface IModuleLabel
    {
        /// <summary>
        /// Reruns the label for the newModule
        /// </summary>
        /// <returns></returns>
        string GetLabel();
    }
}
