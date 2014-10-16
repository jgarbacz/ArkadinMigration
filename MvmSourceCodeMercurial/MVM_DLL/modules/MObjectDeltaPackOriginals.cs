using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
namespace MVM
{
    class MObjectDeltaPackOriginals: IModuleSetup,IModuleRun
    {
        private string objectIdSyntax;
        private IReadString objectIdParsed;
        private string fieldNameSyntax;
        private IReadString fieldNameParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MObjectDeltaPackOriginals m = new MObjectDeltaPackOriginals();
            m.objectIdSyntax = me.SelectNodeInnerText("./object_id");
            m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            m.fieldNameSyntax = me.SelectNodeInnerText("./field");
            m.fieldNameParsed = mc.ParseSyntax(m.fieldNameSyntax);
            run.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            string objectId=this.objectIdParsed.Read(mc);
            string fieldName = this.fieldNameParsed.Read(mc);
            using (IObjectData obj = mc.objectCache.CheckOut(objectId))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj == null) throw new Exception("not a delta tracked object");
                if (deltaObj != null)
                {
                    deltaObj.PackOriginals(fieldName);
                }
            }
        }
    }
}
