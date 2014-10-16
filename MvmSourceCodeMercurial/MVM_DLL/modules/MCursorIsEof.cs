using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
namespace MVM
{
    class MCursorIsEof: IModuleSetup,IModuleRun
    {
        private string cursorOidSyntax;
        private IReadString cursorOidReader;
        private string cursorInstIdSyntax;
        private IReadString cursorInstIdParsed;
        private string isEofSyntax;
        private IWriteString isEofParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MCursorIsEof m = new MCursorIsEof();
            m.cursorOidSyntax = me.SelectNodeInnerText("cursor");
            m.cursorOidReader = mc.ParseSyntax(m.cursorOidSyntax);
            m.cursorInstIdSyntax = me.SelectNodeInnerText("cursor_inst_id");
            m.cursorInstIdParsed = mc.ParseSyntax(m.cursorInstIdSyntax);
            m.isEofSyntax = me.SelectNodeInnerText("is_eof");
            m.isEofParsed = mc.ParseWritableSyntax(m.isEofSyntax);
            run.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            ICursor cursor;
            if (this.cursorInstIdParsed != null)
            {
                string cursorInstId = this.cursorInstIdParsed.Read(mc);
                cursor=mc.LookupCursorViaInstId(cursorInstId);
            }
            else if (this.cursorOidReader != null)
            {
                string cursorOid = this.cursorOidReader.Read(mc);
                cursor = mc.LookupCursorViaOid(cursorOid);
            }
            else
            {
                throw new Exception("unexpected cursor_is_eof");
            }

            this.isEofParsed.Write(mc, cursor.Eof ? "1":"0");
        }
    }
}
