using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MCursorClear: IModuleSetup,IModuleRun
    {
        private string cursorOidSyntax;
        private IReadString cursorOidReader;

        private string cursorInstIdSyntax;
        private IReadString cursorInstIdReader;

        private bool lookupViaCursorObjectId = true;
        

        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MCursorClear m = new MCursorClear();
            if (!me.HasChildElements())
            {
                m.lookupViaCursorObjectId = true;
                m.cursorOidSyntax = me.InnerText;
                m.cursorOidReader = mc.ParseSyntax(m.cursorOidSyntax);
            }
            else
            {
                m.lookupViaCursorObjectId = false;
                m.cursorOidSyntax = me.SelectNodeInnerText("./cursor");
                m.cursorOidReader = mc.ParseSyntax(m.cursorOidSyntax);
                m.cursorInstIdSyntax = me.SelectNodeInnerText("./cursor_inst_id");
                m.cursorInstIdReader = mc.ParseSyntax(m.cursorInstIdSyntax);
            }
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            if (lookupViaCursorObjectId)
            {
                string csrOid = this.cursorOidReader.Read(mc);
                ICursor csr;
                if (mc.LookupCursorViaOid(csrOid, out csr))
                {
                    //mc.mvm.Log("cursor_clear: csrObjectId=" + csrOid + ", csrInstId=" + csr.CursorInstId + ", proc=" + mc.procName);
                    csr.Clear(mc);
                }
                else
                {
                    //mc.mvm.Log("cursor_clear: csrObjectId=" + csrOid + ", NO INSTANCE TO CLEAR" + ", proc=" + mc.procName);
                }
            }
            else
            {
                string csrInstId = this.cursorInstIdReader.Read(mc);
                ICursor csr;
                if (mc.LookupCursorViaInstId(csrInstId,out csr))
                {
                    //mc.mvm.Log("cursor_clear: csrInstId=" + csr.CursorInstId + ", proc=" + mc.procName);
                    csr.Clear(mc);
}
                else
                {
                    //mc.mvm.Log("cursor_clear: csrInstId=" + csrInstId + ", NO INSTANCE TO CLEAR" + ", proc=" + mc.procName);
                }
            }
        }
    }
}
