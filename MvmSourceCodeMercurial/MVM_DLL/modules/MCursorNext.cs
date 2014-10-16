using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MCursorNext: IModuleSetup,IModuleRun
    {
        private string cursorOidSyntax;
        private IReadString cursorOidReader;
        private IWriteString cursorOidWriter;

        private string cursorInstIdSyntax;
        private IReadString cursorInstIdReader;

        private bool lookupViaCursorObjectId;
        private string isEofSyntax;
        private IWriteString isEofParsed;

        // <cursor_next>
        // <cursor></cursor>
        // <cursor_inst_id></cursor_inst_id>
        // <cursor_is_eof></cursor_is_eof>
        // </cursor_next>
        // 
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MCursorNext m = new MCursorNext();
           
            if (!me.HasChildElements())
            {
                m.lookupViaCursorObjectId = true;
                m.cursorOidSyntax = me.InnerText;
                m.cursorOidReader = mc.ParseSyntax(m.cursorOidSyntax);
                m.cursorOidWriter = mc.ParseWritableSyntax(m.cursorOidSyntax);
                run.Add(m);
            }
            else
            {
                m.lookupViaCursorObjectId = false;
                m.cursorOidSyntax = me.SelectNodeInnerText("./cursor");
                m.cursorOidReader = mc.ParseSyntax(m.cursorOidSyntax);
                m.cursorOidWriter = mc.ParseWritableSyntax(m.cursorOidSyntax);
                m.cursorInstIdSyntax = me.SelectNodeInnerText("./cursor_inst_id");
                m.cursorInstIdReader = mc.ParseSyntax(m.cursorInstIdSyntax);
                m.isEofSyntax = me.SelectNodeInnerText("is_eof");
                m.isEofParsed = mc.ParseWritableSyntax(m.isEofSyntax);
                run.Add(m);
                // when you do next with cursor_inst_id you need to get the new oid.
                if(m.cursorInstIdSyntax.NotNullOrEmpty()){
                    run.Add(mc.GetModuleRun(
                    @"<cursor_inst_id_to_object_id>
                      <cursor_inst_id>" + m.cursorInstIdSyntax + @"</cursor_inst_id>
                      <cursor>" + m.cursorOidSyntax+@"</cursor>
                    </cursor_inst_id_to_object_id>"
                    ));
                }
                if (m.isEofSyntax.NotNullOrEmpty())
                {
                    run.Add(mc.GetModuleRun(
                   @"<cursor_is_eof>
                      <cursor_inst_id>" + m.cursorInstIdSyntax + @"</cursor_inst_id>
                      <is_eof>" + m.isEofSyntax + @"</is_eof>
                    </cursor_is_eof>"
                   ));
                }
            }
        }

        public void Run(ModuleContext mc)
        {
            ICursor cursor;
            if (lookupViaCursorObjectId)
            {
                string csrOid = this.cursorOidReader.Read(mc);
                if(!mc.LookupCursorViaOid(csrOid,out cursor)){
                    this.cursorOidWriter.Write(mc,"");
                    return;
                }
            }
            else
            {
                string csrInstId = this.cursorInstIdReader.Read(mc);
                if (!mc.LookupCursorViaInstId(csrInstId, out cursor))
                {
                    this.cursorOidWriter.Write(mc, "");
                    return;
                }
            }
            if (cursor.Eof)
            {
                // maybe clear it too??
                this.cursorOidWriter.Write(mc, "");
                return;
            }

            IObjectData outputObject;
            CursorStatus csrStatus=cursor.Next(mc,out outputObject);
            switch (csrStatus)
            {
                case CursorStatus.HAS_ROW:
                    {
                        if(this.cursorOidWriter!=null)
                            this.cursorOidWriter.Write(mc,outputObject.objectId);
                        break;
                    }
                case CursorStatus.EOF:
                    {
                        if (this.cursorOidWriter != null) 
                            this.cursorOidWriter.Write(mc, "");
                        break;
                    }
                case CursorStatus.PARENT_NEXT:
                    {
                        if (this.cursorOidWriter != null && outputObject != null) 
                            this.cursorOidWriter.Write(mc, outputObject.objectId);
                        break;
}
                case CursorStatus.YIELD:
                    {
                       mc.YieldAndRepeat();
                       break;
                    }
                default: throw new Exception("unexpected csrStatus=" + csrStatus);
            }
        }
    }
}
