using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MCursorInstIdToObjectId: IModuleSetup,IModuleRun
    {

        private string cursorInstIdSyntax;
        private IReadString cursorInstIdParsed;
        private string cursorObjectIdSyntax;
        private IWriteString cursorObjectIdParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MCursorInstIdToObjectId m = new MCursorInstIdToObjectId();
            m.cursorInstIdSyntax = me.SelectNodeInnerText("cursor_inst_id");
            m.cursorInstIdParsed = mc.ParseSyntax(m.cursorInstIdSyntax);
            m.cursorObjectIdSyntax = me.SelectNodeInnerText("cursor");
            m.cursorObjectIdParsed = mc.ParseWritableSyntax(m.cursorObjectIdSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string cursorInstId = this.cursorInstIdParsed.Read(mc);
            ICursor cursor = mc.globalContext.GetNamedClassInst(cursorInstId) as ICursor;
            this.cursorObjectIdParsed.Write(mc, cursor.CursorOid.Nvl(""));
        }
    }
}
