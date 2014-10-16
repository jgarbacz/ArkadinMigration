using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MObjectDeltaTableStatus : IModuleSetup, IModuleRun
    {
        private string objectIdSyntax;
        private IReadString objectIdParsed;
        private string tableNameSyntax;
        private IReadString tableNameParsed;
        private string isUpdatedSyntax;
        private IWriteString isUpdatedParsed;
        private string isInsertedSyntax;
        private IWriteString isInsertedParsed;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MObjectDeltaTableStatus m = new MObjectDeltaTableStatus();
            m.tableNameSyntax = me.SelectNodeInnerText("./table_name");
            m.objectIdSyntax = me.SelectNodeInnerText("./object_id", "OBJECT.object_id");
            m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            m.tableNameParsed = mc.ParseSyntax(m.tableNameSyntax);
            m.isUpdatedSyntax = me.SelectNodeInnerText("./is_updated");
            m.isUpdatedParsed = mc.ParseWritableSyntax(m.isUpdatedSyntax);
            m.isInsertedSyntax = me.SelectNodeInnerText("./is_inserted");
            m.isInsertedParsed = mc.ParseWritableSyntax(m.isInsertedSyntax);
            run.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            string objectId = this.objectIdParsed.Read(mc);
            string tableName = this.tableNameParsed.Read(mc);
            using (IObjectData obj = mc.objectCache.CheckOut(objectId))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj == null) throw new Exception("not a delta tracked object");
                if (deltaObj != null)
                {
                    this.isUpdatedParsed.Write(mc, deltaObj.IsUpdated(tableName) ? "1" : "0");
                    this.isInsertedParsed.Write(mc, deltaObj.IsInserted(tableName) ? "1" : "0");
                }
            }
        }
    }
}
