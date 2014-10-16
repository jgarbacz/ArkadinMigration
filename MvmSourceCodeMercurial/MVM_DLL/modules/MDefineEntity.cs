using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

/*
 This registers <entity/> globally

 */
namespace MVM
{
    public class MDefineEntity: IModuleGlobal
    {
        // registers the <entity>
        public string Global(ProcInfo procInfo, XmlElement me, SchedulerMaster schedulerMaster, Worker worker)
        {
            string fileName = procInfo.location.GetLocation();
            string extensionDir = MvmEngine.GetExtensionDir(fileName);
            me.AppendTextElement("file_name", fileName);
            me.AppendTextElement("extension_dir", extensionDir);
            List<XmlElement> entities = new List<XmlElement>();
            entities = (List<XmlElement>)worker.workMgr.globalContext.GetNamedClassInst("entities", entities);
            lock (entities)
            {
                entities.Add(me);
            }
            // no longer want the entity to be part of the proc since it is not a valid module.
            me.ParentNode.RemoveChild(me);
            return null;
        }

       
    }
}
