using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

/*
<index_get>
    <index>OBJECT.MY_INDEX</index>
    <field name='key_field'>'1'</field>
    <field name='value_field'>TEMP.value</field>
    <then>
      :
    </then>
    <else>
      :
    </else>
</index_get>
*/

namespace MVM
{
    class MIndexGet : IModuleSetup, IModuleRun
    {
        private string indexNameSyntax;
        private string indexName;
        private string isMatchSyntax;
        private List<int> fieldIndices = new List<int>();
        
        private List<IWriteString> fieldWrites = new List<IWriteString>();
        private IWriteString isMatchParsed;
        private Dictionary<string, IWriteString> fieldValuesParsedMap = new Dictionary<string, IWriteString>();
        private List<IReadString> orderedKeysParsed = new List<IReadString>();
        private IIndex globalIndex;
        private byte keyCount;
        private bool ifNeeded;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MIndexGet m = new MIndexGet();

            // xml extraction
            m.indexNameSyntax = me.SelectNodeInnerText("./index");

            // For now, only support static index names
            m.indexName = mc.SyntaxReadString(m.indexNameSyntax);

            // Since our index is static, lookup and sort out the format now.
            m.globalIndex = (IIndex)mc.globalContext.GetNamedClassInst(m.indexName);

            // Ask the index what newOrder it wants its key fields in so we can setup our ordered values to be passed to it.
            if (m.globalIndex.UseContext())
            {
                Overrider overrider = new Overrider(me);
                foreach (string fieldName in m.globalIndex.GetOrderedKeyFields())
                {
                    IReadString parsedValue = mc.ParseSyntax(overrider.GetSyntax(fieldName));
                    m.orderedKeysParsed.Add(parsedValue);
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
                        m.orderedKeysParsed.Add(parsedValue);
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
                foreach (var passed in passedFields)
                {
                    int fieldIndex = m.globalIndex.FieldIndexes[passed.Key];
                    m.fieldIndices.Add(fieldIndex);
                    m.fieldWrites.Add(mc.ParseWritableSyntax(passed.Value));
                    m.fieldValuesParsedMap[passed.Key] = mc.ParseWritableSyntax(passed.Value);
                }
            }

            m.isMatchSyntax = "TEMP." + mc.GetGenSym("ismatch");
            //m.isMatchSyntax = "TEMP._j";
            m.isMatchParsed = mc.ParseWritableSyntax(m.isMatchSyntax);

            XmlElement loopElem = me.SelectSingleElem("./loop");
            XmlElement csrElem = me.SelectSingleElem("./cursor");
            if (loopElem != null || csrElem != null)
            {
                throw new Exception("index_get cannot have loop or cursor elements!");
            }

            XmlDocument doc = me.OwnerDocument;
            XmlElement thenElem = me.SelectSingleElem("./then");
            XmlElement elseElem = me.SelectSingleElem("./else");
            XmlElement ifElem = null;

            m.ifNeeded = false;
            // Check for then/else clauses
            if (thenElem != null || elseElem != null)
            {
                m.ifNeeded = true;
                ifElem = doc.CreateElement("if");
                XmlElement conditionElem = doc.CreateElement("condition");
                conditionElem.InnerText = m.isMatchSyntax + " eq '1'";
                ifElem.AppendChild(conditionElem);
                if (thenElem != null)
                {
                    ifElem.AppendChild(thenElem);
                }
                if (elseElem != null)
                {
                    ifElem.AppendChild(elseElem);
                }
                run.Add(mc.GetModuleRun("<do>" + m.isMatchSyntax + " = ''</do>"));
            }

            m.keyCount = (byte) m.orderedKeysParsed.Count;
            // Add the runtime module
            
            run.Add(m);

            // Add the then/else stuff
            if (ifElem != null)
            {
                run.Add(mc.GetModuleRun(ifElem));
            }
        }

        public void Run(ModuleContext mc)
        {
            string[] orderedKeys = new string[this.keyCount];
            //Dictionary<string, string> values = new Dictionary<string, string>();
            string[] values;
            byte row_counter = 0;

            foreach (var k in this.orderedKeysParsed)
            {
                orderedKeys[row_counter++] = k.Read(mc);
            }
            //foreach (var v in this.fieldValuesParsedMap)
            //{
            //    values[v.Key] = "";
            //}
            //string retval = this.globalIndex.IndexGet(mc, orderedKeys, values);
            //string retval = this.globalIndex.IndexGetRow(mc, orderedKeys, out values);
            if (this.globalIndex.IndexGetRow(mc, orderedKeys, out values))
            {
                if (this.ifNeeded)
                {
                    this.isMatchParsed.Write(mc, "1");
                }
                row_counter = 0;
                foreach (var v in fieldIndices)
                {
                    fieldWrites[row_counter++].Write(mc, values[v]);
                    //this.fieldValuesParsedMap[v.Key].Write(mc, v.Value);
                }
                
            }
            else
            {
                if (this.ifNeeded)
                {
                    this.isMatchParsed.Write(mc, "");
                }
            }
        }
    }
}
