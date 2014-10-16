using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MObjectDeltaClearOriginals: IModuleSetup,IModuleRun
    {
        private string objectIdSyntax;
        private IReadString objectIdParsed;
      
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MObjectDeltaClearOriginals m = new MObjectDeltaClearOriginals();
            m.objectIdSyntax = me.SelectNodeInnerText("./object_id");
            m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
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
                    deltaObj.ClearOriginals();
                }
            }
        }
    }
}
