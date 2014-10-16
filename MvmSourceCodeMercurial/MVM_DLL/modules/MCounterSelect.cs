using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Antlr.Runtime.Tree;
namespace MVM
{
    /*
    <counter_select>
    <from>0</from>
    <to>10</to>
    <increment>2</increment>
    <cursor>TEMP.csr</cursor>
    <cursor_value>'ctr'</cursor_value>
    <loop>
        <print>'OBJECT.ctr='~OBJECT.ctr</print>
    </loop>
    </counter_select>
   */
    class MCounterSelect: IModuleSetup,IModuleRun
    {
        private string fromSyntax;
        private string toSyntax;
        private string incrementSyntax;
        private IReadString fromParsed;
        private IReadString toParsed;
        private IReadString incrementParsed;
        private CursorSetupCommon cursorSetup;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MCounterSelect m = new MCounterSelect();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            // xml extraction
            m.fromSyntax = me.SelectNodeInnerText("./from","1");
            m.toSyntax = me.SelectNodeInnerText("./to","1");
            m.incrementSyntax = me.SelectNodeInnerText("./increment","1");
            // parsing
            m.fromParsed = mc.ParseSyntax(m.fromSyntax);
            m.toParsed = mc.ParseSyntax(m.toSyntax);
            m.incrementParsed = mc.ParseSyntax(m.incrementSyntax);
            // runtime
            run.Add(m);
            m.cursorSetup.AddCursorSubProcs(me, mc, run);
        }
        public void Run(ModuleContext mc)
        {
            string counterField = this.cursorSetup.GetCursorValue(mc);
            long fromVal = int.Parse(this.fromParsed.Read(mc));
            long toVal = int.Parse(this.toParsed.Read(mc));
            long incrVal = int.Parse(this.incrementParsed.Read(mc));
            var csr = new CtrCursor(mc, this.cursorSetup, fromVal, toVal, incrVal);
        }
    }

    /// <summary>
    /// Cursors should work in a way that they start out having metadata and calling 
    /// next gets you the first row (and still have metadata).
    /// </summary>
    public class CtrCursor : CursorCommonLinqEnabled, ICursor
    {
        public string counterField;
        public long fromVal;
        public long toVal;
        public long incrVal;
        public CtrCursor(ModuleContext mc, CursorSetupCommon setup, long fromVal, long toVal, long incrVal)
            : base(mc, setup)
        {
            this.counterField = setup.GetCursorValue(mc);
            this.fromVal = fromVal-incrVal;
            this.toVal = toVal;
            this.incrVal = incrVal;
            this.orderedFieldNames=new List<string>{counterField};
        }
       

        public override IObjectData CursorNext()
        {
            if (fromVal == toVal)
            {
                return null;
            }
            else
            {
                using (var csrObj = this.CreateNewObject())
                {
                    this.fromVal += incrVal;
                    csrObj[counterField] = fromVal.ToString();
                    return csrObj;
                }
            }
        }

        public override void CursorClear()
        {
            // no resources to free
        }
    }
}
