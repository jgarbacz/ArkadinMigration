using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MCursorObjectIdToInstId: IModuleSetup,IModuleRun
    {
        private string cursorObjectIdSyntax;
        private IReadString cursorObjectIdParsed;
        private string cursorInstIdSyntax;
        private IWriteString cursorInstIdParsed;
        
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MCursorObjectIdToInstId m = new MCursorObjectIdToInstId();
           
            m.cursorObjectIdSyntax = me.SelectNodeInnerText("cursor");
            m.cursorObjectIdParsed = mc.ParseSyntax(m.cursorObjectIdSyntax);
            m.cursorInstIdSyntax = me.SelectNodeInnerText("cursor_inst_id");
            m.cursorInstIdParsed = mc.ParseWritableSyntax(m.cursorInstIdSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string cursorObjectId = this.cursorObjectIdParsed.Read(mc);

            ICursorBase cursor;
            if (mc.LookupCursorViaOid(cursorObjectId, out cursor))
            {
                this.cursorInstIdParsed.Write(mc, cursor.CursorInstId);
            }
            else
            {
                this.cursorInstIdParsed.Write(mc, "");
            }
        }
    }
}
