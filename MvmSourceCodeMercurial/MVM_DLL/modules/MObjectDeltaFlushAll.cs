using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MObjectDeltaFlushAll: IModuleSetup,IModuleRun
    {
        //private string objectIdSyntax;
        //private IReadString objectIdParsed;
        //private string tableNameSyntax;
        //private IReadString tableNameParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MObjectDeltaFlushAll m = new MObjectDeltaFlushAll();
            //m.tableNameSyntax = me.SelectNodeInnerText("./table_name");
            //m.objectIdSyntax = me.SelectNodeInnerText("./object_id");
            //m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            //m.tableNameParsed = mc.ParseSyntax(m.tableNameSyntax);
            run.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            ObjectDataFormattedDelta.FlushAll(mc.mvm);
            
            //string objectId=this.objectIdParsed.Read(mc);
            //string tableName = this.tableNameParsed.Read(mc);
            //using (IObjectData obj = mc.objectCache.CheckOut(objectId))
            //{
            //    ObjectDataDelta deltaObj = obj as ObjectDataDelta;
            //    if (deltaObj == null) throw new Exception("not a delta tracked object");
            //    if (deltaObj != null)
            //    {
            //        deltaObj.AddPersistTable(tableName);
            //    }
            //}
        }
    }
}
