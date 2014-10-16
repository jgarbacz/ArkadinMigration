using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

/*
 This registers <entity_group/> globally

 */
namespace MVM
{
    public class MDefineEntityGroup: IModuleGlobal
    {
        // registers the <enitity_group>
        public string Global(ProcInfo procInfo, XmlElement me, SchedulerMaster schedulerMaster, Worker worker)
        {
            string fileName=procInfo.location.GetLocation();
            string extensionDir = MvmEngine.GetExtensionDir(fileName);
            me.AppendTextElement("file_name", fileName);
            me.AppendTextElement("extension_dir", extensionDir);
            List<XmlElement> entityGroups = new List<XmlElement>();
            entityGroups = (List<XmlElement>)worker.workMgr.globalContext.GetNamedClassInst("entity_groups", entityGroups);
            lock (entityGroups)
            {
                entityGroups.Add(me);
            }
            return null;
        }

       
    }
}
