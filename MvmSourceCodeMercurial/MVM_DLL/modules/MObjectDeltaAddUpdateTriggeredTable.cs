using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MObjectDeltaAddUpdateTriggeredTable: IModuleSetup,IModuleRun
    {
        private string objectIdSyntax;
        private IReadString objectIdParsed;

        private string updateTableNameSyntax;
        private IReadString updateTableNameParsed;

        private string updateFieldNameSyntax;
        private IReadString updateFieldNameParsed;

        private string insertTableNameSyntax;
        private IReadString insertTableNameParsed;

        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MObjectDeltaAddUpdateTriggeredTable m = new MObjectDeltaAddUpdateTriggeredTable();
            m.updateTableNameSyntax = me.SelectNodeInnerText("./update_table_name");
            m.updateTableNameParsed = mc.ParseSyntax(m.updateTableNameSyntax);
            m.updateFieldNameSyntax = me.SelectNodeInnerText("./update_field_name");
            m.updateFieldNameParsed = mc.ParseSyntax(m.updateFieldNameSyntax);
            m.insertTableNameSyntax = me.SelectNodeInnerText("./insert_table_name");
            m.insertTableNameParsed = mc.ParseSyntax(m.insertTableNameSyntax);
            m.objectIdSyntax = me.SelectNodeInnerText("./object_id");
            m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            run.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            string objectId=this.objectIdParsed.Read(mc);
            string updateTableName = this.updateTableNameParsed.Read(mc);
            string updateFieldName = this.updateFieldNameParsed.Read(mc);
            string insertTableName = this.insertTableNameParsed.Read(mc);
            using (IObjectData obj = mc.objectCache.CheckOut(objectId))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj == null) throw new Exception("not a delta tracked object");
                if (deltaObj != null)
                {
                    deltaObj.AddUpdateTriggerToTable(updateTableName,updateFieldName,insertTableName);
                }
            }
        }
    }
}
