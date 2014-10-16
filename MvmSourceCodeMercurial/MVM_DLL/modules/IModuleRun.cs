using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    
    /// <summary>
    /// Defines the interface that newModules must support. 
    /// Although we cannot enforce it in the interface, it is required that
    /// instances of a newModule be thread safe.
    /// </summary>
    public interface IModuleRun
    {
        /// <summary>
        /// Perform the newModule
        /// </summary>
        /// <param name="mc"></param>
        void Run(ModuleContext mc);
    }
}
