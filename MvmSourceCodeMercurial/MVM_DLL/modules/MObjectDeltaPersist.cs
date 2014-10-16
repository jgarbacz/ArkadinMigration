using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MVM
{
    /*
     * 
     * <object_delta_persist>
     * <object_id></object_id>
     * <num_inserts></num_inserts>
     * <num_updates></num_updates>
     * <num_deletes></num_deletes>
     * </object_delta_persist>
     * 
     */
    class MObjectDeltaPersist: IModuleSetup,IModuleRun
    {
        private string objectIdSyntax;
        private IReadString objectIdParsed;
        private string packedOriginalsFieldNameSyntax;
        private IReadString packedOriginalsFieldNameParsed;
        private string auditTableNameSyntax;
        private IReadString auditTableNameParsed;
        private string modifiedDateFieldSyntax;
        private IReadString modifiedDateFieldParsed;
        private string modifiedDateValueSyntax;
        private IReadString modifiedDateValueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MObjectDeltaPersist m = new MObjectDeltaPersist();
            m.objectIdSyntax = me.SelectNodeInnerText("./object_id");
            m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            m.packedOriginalsFieldNameSyntax = me.SelectNodeInnerText("./packed_originals_field_name");
            m.packedOriginalsFieldNameParsed = mc.ParseSyntax(m.packedOriginalsFieldNameSyntax);
            m.auditTableNameSyntax = me.SelectNodeInnerText("./audit_table_name", "''");
            m.auditTableNameParsed = mc.ParseSyntax(m.auditTableNameSyntax);
            m.modifiedDateFieldSyntax = me.SelectNodeInnerText("./modified_date_field");
            m.modifiedDateFieldParsed = mc.ParseSyntax(m.modifiedDateFieldSyntax);
            m.modifiedDateValueSyntax = me.SelectNodeInnerText("./modified_date_value");
            m.modifiedDateValueParsed = mc.ParseSyntax(m.modifiedDateValueSyntax);
            run.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            string objectId = this.objectIdParsed.Read(mc);
            string auditTableName = this.auditTableNameParsed.Read(mc);
            string modifiedDateField = this.modifiedDateFieldParsed == null ? null : this.modifiedDateFieldParsed.Read(mc);
            string modifiedDateValue = this.modifiedDateValueParsed == null ? null : this.modifiedDateValueParsed.Read(mc);
            string packedOriginalsFieldName = this.packedOriginalsFieldNameParsed == null ? null : this.packedOriginalsFieldNameParsed.Read(mc);
            using (IObjectData obj = mc.objectCache.CheckOut(objectId))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj == null) throw new Exception("not a delta tracked object:"+obj.ToString());
                deltaObj.Persist(auditTableName, modifiedDateField,modifiedDateValue,packedOriginalsFieldName);
            }
        }
    }
}
