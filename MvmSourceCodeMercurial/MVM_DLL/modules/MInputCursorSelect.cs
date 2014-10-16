using System;
using System.Collections.Generic;
using Oracle.DataAccess.Client;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;

namespace MVM
{
    [Module(@"
        <module_config>
            <name>input_cursor_select</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType>
                    <xs:sequence>
                        <xs:group ref='db_info_group'/>
                        <xs:choice>
                            <xs:element name='input_cursor_inst_id' type='xs:string' datatype='string' mode='in' description='existing cursor inst id to manipulate'/>
                            <xs:element name='input_cursor' type='xs:string' datatype='string' mode='in' description='existing cursor object id to manipulate'/>
                        </xs:choice>
                        <xs:group ref='cursor_operation_group'/>
                    </xs:sequence>
                </xs:complexType>
            </xsd>
            <doc>
                <category>Cursors</category>
                <description>Lets you manipulate a previously instantiated cursor the same way you would at the time of instantiation</description>
            </doc>
        </module_config>
    ")]
    class MInputCursorSelect : BaseModuleSetup, IModuleRun
    {
        private CursorSetupCommon cursorSetup;
        
        private string inputCursorInstIdSyntax;
        private IReadString inputCursorInstIdParsed;

        private string inputCursorOidSyntax;
        private IReadString inputCursorOidParsed;

        // setup looks at the current db info, and does either oracle or sql
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MInputCursorSelect m = new MInputCursorSelect();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            this.SetupReadString(me, mc, "input_cursor", out m.inputCursorOidSyntax, out m.inputCursorOidParsed);
            this.SetupReadString(me, mc, "input_cursor_inst_id", out m.inputCursorInstIdSyntax, out m.inputCursorInstIdParsed);
            run.Add(m);
            m.cursorSetup.AddCursorSubProcs(me, mc, run);
        }

        public void Run(ModuleContext mc)
        {
            if (this.inputCursorInstIdParsed != null)
            {
                string cursorInstId = this.inputCursorInstIdParsed.Read(mc);
                ICursor cursor = null;
                if (mc.LookupCursorViaInstId(cursorInstId, out cursor))
                {
                    var csr = new CursorWrapper(mc, this.cursorSetup, cursor);
                }
                else
                {
                    var csr = new NullCursor(mc, this.cursorSetup);
                }
            }
            else
            {
                string cursorOid = this.inputCursorOidParsed.Read(mc);
                ICursor cursor = null;
                if (mc.LookupCursorViaOid(cursorOid, out cursor))
                {
                    var csr = new CursorWrapper(mc, this.cursorSetup, cursor);
                }
                else
                {
                    var csr = new NullCursor(mc, this.cursorSetup);
                }
            }
        }
    }


    public class CursorWrapper : CursorCommon
    {
        public CursorWrapper(ModuleContext mc, ICursorSetupCommon cursorSetup, ICursor cursor)
            : base(mc, cursorSetup)
        {
            this.cursor = cursor;
        }

        // generator cursor
        public ICursor cursor;

        // 1 generator, N operators
        protected override CursorStatus CursorNext(ModuleContext mc, out IObjectData outputObj)
        {
            for (; ; )
            {
                // if the cursor is already eof, send eof
                if (this.cursor.Eof)
                {
                    outputObj=null;
                    return CursorStatus.EOF;
                }
                // otherwise, try to read from the cursor
                CursorStatus csrStatus = cursor.Next(mc, out outputObj);
                return csrStatus;
            }
        }

        protected override void CursorClear(ModuleContext mc)
        {
            this.cursor.Clear(mc);
        }
    }
}
