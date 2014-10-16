using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

/*
 This registers <map_type/> globally
 */
namespace MVM
{
    public class MDefineEntityMapType: IModuleGlobal
    {
        // registers the <entity_map_type>
        public string Global(ProcInfo procInfo, XmlElement me, SchedulerMaster schedulerMaster, Worker worker)
        {
            string fileName=procInfo.location.GetLocation();
            string extensionDir = MvmEngine.GetExtensionDir(fileName);
            me.AppendTextElement("file_name", fileName);
            me.AppendTextElement("extension_dir", extensionDir);
            List<XmlElement> entityMapTypes = new List<XmlElement>();
            entityMapTypes = (List<XmlElement>)worker.workMgr.globalContext.GetNamedClassInst("entity_map_types", entityMapTypes);
            lock (entityMapTypes)
            {
                entityMapTypes.Add(me);
            }
            return null;
        }

       
    }
}
