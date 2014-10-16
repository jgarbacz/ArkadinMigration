using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    public interface IModuleSetup
    {
        // This function should analyze the xml and add any necessary runModules. The current
        // newModule gets replaced and only the run newModules remain.
        void Setup(XmlElement me,ModuleContext mc, List<IModuleRun> run);
    }
}
