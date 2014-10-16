using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MRemoveSpawnedObjectFromProc : IModuleSetup, IModuleRun
    {
        /*
         * <remove_spawned_object_from_proc>TEMP.spawned_oid</remove_spawned_object_from_proc>
         * 
         */

        string objectIdSyntax;
        IReadString objectIdParsed;
        
        public void Setup(XmlElement me,ModuleContext mc, List<IModuleRun> run)
        {
            MRemoveSpawnedObjectFromProc m = new MRemoveSpawnedObjectFromProc();
            m.objectIdSyntax = me.InnerText;
            m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string objectId=this.objectIdParsed.Read(mc);
            mc.procInst.RemoveSpawnedOid(objectId);
        }
    }
}
