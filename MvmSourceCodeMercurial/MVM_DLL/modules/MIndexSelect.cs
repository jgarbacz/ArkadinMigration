using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

/*
<index_select>
<index>OBJECT.MY_INDEX</index>
<cursor>TEMP.MY_CURSOR</cursor>
<field name='field_1'>'1'</field>
<loop>
  :
</loop>
<then>
  :
</then>
<else>
  :
</else>
</index_select>
 */

namespace MVM
{
    class MIndexSelect: IModuleSetup,IModuleRun
    {
        private string indexNameSyntax; 
        private string indexName;
        private Dictionary<string, IReadString> fieldNameValParsedMap = new Dictionary<string, IReadString>();
        private List<IReadString> orderedValuesParsed = new List<IReadString>();
        private IIndex globalIndex;
        private string cursorOrderString;
        private CursorOrder cursorOrder = CursorOrder.Forward;
        private CursorSetupCommon cursorSetup;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MIndexSelect m = new MIndexSelect();
            m.cursorSetup = new CursorSetupCommon(me, mc);

            // xml extraction
            m.indexNameSyntax = me.SelectNodeInnerText("./index");

            // get the cursor newOrder
            m.cursorOrderString = mc.SyntaxReadString(me.SelectNodeInnerText("./cursor_order", "'default'"));
            if (m.cursorOrderString.Equals("default")) m.cursorOrder = CursorOrder.Default;
            else if (m.cursorOrderString.Equals("reverse")) m.cursorOrder = CursorOrder.Reverse;
            else if (m.cursorOrderString.Equals("forward")) m.cursorOrder = CursorOrder.Forward;
            else throw new Exception("Unexpected cursor_order [" + m.cursorOrderString + "]");

            // For now, only support static index names
            m.indexName = mc.SyntaxReadString(m.indexNameSyntax);

            // Since our index is static, lookup and sort out the format now.
            //mc.mvm.Log("MIndexSelect mark A");
            //mc.mvm.Log(System.Diagnostics.Process.GetCurrentProcess().WorkingSet64.ToString());
            m.globalIndex = (IIndex) mc.globalContext.GetNamedClassInst(m.indexName);
            //mc.mvm.Log(System.Diagnostics.Process.GetCurrentProcess().WorkingSet64.ToString());

            // Ask the index what newOrder it wants its key fields in so we can setup our ordered values to be passed to it.
            if (m.globalIndex.UseContext())
            {
                Overrider overrider = new Overrider(me);
                foreach (string fieldName in m.globalIndex.GetOrderedKeyFields())
                {
                    IReadString parsedValue = mc.ParseSyntax(overrider.GetSyntax(fieldName));
                    m.orderedValuesParsed.Add(parsedValue);
                }
            }
            else
            {
                var passedFields = me.SelectNodes("field").ToNameTextDictionary();
                var keyFieldNames = m.globalIndex.GetOrderedKeyFields();
                foreach (string fieldName in keyFieldNames)
                {
                    if (passedFields.ContainsKey(fieldName))
                    {
                        IReadString parsedValue = mc.ParseSyntax(passedFields[fieldName]);
                        m.orderedValuesParsed.Add(parsedValue);
                        passedFields.Remove(fieldName);
                    }
                    else
                    {
                        if (passedFields.Count > 0)
                        {
                            throw new Exception("Error selecting from memory index. Keys must be selected left to right without gaps. You attempted to select [" + passedFields.Keys.ToList().Join(",") + "] but did not pass field to left of it named [" + fieldName + "]");
                        }
                    }
                }
                if (passedFields.Count > 0)
                {
                    throw new Exception("Error selecting memory index. You attempted to a select with nonkey field(s) [" + passedFields.Keys.ToList().Join(",") + "]");
                }
            }

            // Add the runtime select modudle
            run.Add(m);
            m.cursorSetup.AddCursorSubProcs(me, mc, run);
        }

        public void Run(ModuleContext mc)
        {
            try
            {
                List<string> orderedValues=new List<string>();
                foreach(IReadString v in this.orderedValuesParsed) orderedValues.Add(v.Read(mc));
                var csr = this.globalIndex.IndexSelect(mc, this.cursorSetup, orderedValues, cursorOrder);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot insert_select from index [" + this.globalIndex.IndexName + "]:" + e.Message, e);
            }
        }
}
}
