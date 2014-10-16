using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MObjectDeltaSelect: IModuleSetup,IModuleRun
    {
        private string objectIdSyntax;
        private IReadString objectIdParsed;
        private CursorSetupCommon cursorSetup;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MObjectDeltaSelect m = new MObjectDeltaSelect();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            m.objectIdSyntax = me.SelectNodeInnerText("./object_id","OBJECT.object_id");
            m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            run.Add(m);
            m.cursorSetup.AddCursorSubProcs(me, mc, run);
        }
        public static readonly List<string> orderedFieldNames = new List<string>() { "field_name", "current", "original" };
        public void Run(ModuleContext mc)
        {
            string objectId=this.objectIdParsed.Read(mc);
            using (IObjectData obj = mc.objectCache.CheckOut(objectId))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj == null) throw new Exception("not a delta tracked object");
                if (deltaObj != null)
                {
                    var changedRecords = deltaObj.GetPersistedDeltaCursor();
                    var csr = new MultiFieldListCursor(mc, this.cursorSetup, orderedFieldNames, changedRecords);
                }
            }
        }
    }
}
