using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    public interface IModuleGlobal
    {
        // 
        // 
        /// <summary>
        /// This function gets a hook to a globalContext newModule xml element and has the opportunity to
        /// modify it and do globalContext things before it gets parsed to run. It return success or
        /// failure. If it returns failure the machine will try to run it again hoping that
        /// some dependencies got sorted out.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="schedulerMaster"></param>
        /// <param name="worker"></param>
        /// <returns>null for success, else error message</returns>
        string Global(ProcInfo procInfo,XmlElement me, SchedulerMaster schedulerMaster, Worker worker);
    }
}
