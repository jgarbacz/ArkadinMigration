using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MObjectDeltaHasOriginalChanges: IModuleSetup,IModuleRun
    {
        private string objectIdSyntax;
        private IReadString objectIdParsed;
        private string valueSyntax;
        private IWriteString valueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MObjectDeltaHasOriginalChanges m = new MObjectDeltaHasOriginalChanges();
            m.objectIdSyntax = me.SelectNodeInnerText("./object_id");
            m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            m.valueSyntax = me.SelectNodeInnerText("./value");
            m.valueParsed = mc.ParseWritableSyntax(m.valueSyntax);
            run.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            string objectId=this.objectIdParsed.Read(mc);
            using (IObjectData obj = mc.objectCache.CheckOut(objectId))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj == null) throw new Exception("not a delta tracked object");
                if (deltaObj != null)
                {
                    this.valueParsed.Write(mc,deltaObj.HasOriginalChanges ? "1":"0");
                }
            }
        }
    }
}
