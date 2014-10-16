using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MObjectDeltaSetOriginal : IModuleSetup, IModuleRun
    {
        private string objectIdSyntax;
        private IReadString objectIdParsed;
        private string fieldNameSyntax;
        private IReadString fieldNameParsed;
        private string valueSyntax;
        private IReadString valueParsed;
        private string fieldSyntax;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {


            MObjectDeltaSetOriginal m = new MObjectDeltaSetOriginal();
            m.fieldNameSyntax = me.SelectNodeInnerText("./field_name");
            m.objectIdSyntax = me.SelectNodeInnerText("./object_id");
            m.fieldSyntax = me.SelectNodeInnerText("./field");
            if (m.fieldSyntax != null)
            {
                if (!mc.GetObjectFieldParts(m.fieldSyntax, out m.fieldNameSyntax, out m.objectIdSyntax))
                    throw new Exception("Error passed field=[" + m.fieldSyntax + "] is not valid object field syntax");
            }
            m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            m.fieldNameParsed = mc.ParseSyntax(m.fieldNameSyntax);
            m.valueSyntax = me.SelectNodeInnerText("./value");
            m.valueParsed = mc.ParseSyntax(m.valueSyntax);
            run.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            string objectId = this.objectIdParsed.Read(mc);
            string fieldName = this.fieldNameParsed.Read(mc);
            string fieldValue = this.valueParsed.Read(mc);
            using (IObjectData obj = mc.objectCache.CheckOut(objectId))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj == null) throw new Exception("not a delta tracked object");
                if (deltaObj != null)
                {
                    deltaObj.SetOriginal(fieldName, fieldValue);

                }
            }
        }
    }
}
