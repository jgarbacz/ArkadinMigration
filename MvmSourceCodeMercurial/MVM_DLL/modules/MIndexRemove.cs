using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.Linq;
/*
<index_remove>
<index>OBJECT.MY_INDEX</index>
<num_rows></num_rows>
<field name='field_1'>'1'</field>
<removal_option>'last'</removal_option>  <!-- can be 'first', 'last', or 'all' -->
</index_remove>
*/

namespace MVM
{
    class MIndexRemove: IModuleSetup,IModuleRun
    {
        private string numRowsSyntax;
        private Dictionary<string, string> fieldNameValSyntaxMap = new Dictionary<string, string>();
        private string indexNameSyntax; 
        private string indexName;
        private string optionSyntax;
        private string optionParsed;
        IndexRemovalOption removalOption;
        private Dictionary<string, IReadString> fieldNameValParsedMap = new Dictionary<string, IReadString>();
        private IWriteString numRowsParsed;
        private List<IReadString> orderedValuesParsed = new List<IReadString>();
        private IIndex globalIndex;

        //private int loopProcId = -1;

        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MIndexRemove m = new MIndexRemove();

            // xml extraction
            m.numRowsSyntax = me.SelectNodeInnerText("./num_rows","GLOBAL.num_rows");
            m.indexNameSyntax = me.SelectNodeInnerText("./index");
            foreach (XmlElement elem in me.SelectNodes("./field"))
            {
                string fieldName = elem.GetAttribute("name");
                string fieldValue = elem.InnerText;
                m.fieldNameValSyntaxMap[fieldName] = fieldValue;
            }
            m.optionSyntax = me.SelectNodeInnerText("./removal_option", "''");

            // ...parsing and setup...

            // The numRows must be writable
            m.numRowsParsed = mc.ParseWritableSyntax(m.numRowsSyntax);

            // For now, only support static index names
            if (!mc.IsLiteralString(m.indexNameSyntax)) throw new Exception("Error, expecting string literal for index name, not [" + m.indexNameSyntax + "]");
            m.indexName = mc.GetLiteralString(m.indexNameSyntax);

            // For now, assume field names are not quoted (b/c it is ugly).
            foreach (string fieldName in m.fieldNameValSyntaxMap.Keys)
            {
                string fieldValueSyntax = m.fieldNameValSyntaxMap[fieldName];
                IReadString fieldValueParsed = mc.ParseSyntax(fieldValueSyntax);
                m.fieldNameValParsedMap[fieldName] = fieldValueParsed;
            }

            // Since our index is static, lookup and sort out the format now.
            m.globalIndex = (IIndex) mc.globalContext.GetNamedClassInst(m.indexName);

            // Interpret the removal options enum
            if (!mc.IsLiteralString(m.optionSyntax)) throw new Exception("Error, expecting string literal for removal option, not [" + m.optionSyntax + "]");
            m.optionParsed = mc.GetLiteralString(m.optionSyntax);
            if (m.optionParsed.EqualsIgnoreCase("first"))
            {
                m.removalOption = IndexRemovalOption.First;
            }
            else if (m.optionParsed.EqualsIgnoreCase("last"))
            {
                m.removalOption = IndexRemovalOption.Last;
            }
            else
            {
                m.removalOption = IndexRemovalOption.All;
            }

            // Ask the index what newOrder it wants its key fields in so we can setup our ordered values to be passed to it.
            if (m.globalIndex.NestedKeys())
            {
                var passedFields = me.SelectNodes("field").ToNameTextDictionary();
                foreach (string fieldName in m.globalIndex.GetOrderedKeyFields())
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
                            throw new Exception("Error removing from nested key index. Keys must be passed left to right without gaps. You passed [" + passedFields.Keys.ToList().JoinStrings(",") + "] but did not pass field to left of it named [" + fieldName + "]");
                        }
                    }
                }
                if (passedFields.Count > 0)
                {
                    throw new Exception("Error removing from nested key index. You attempted to a select with nonkey field(s) [" + passedFields.Keys.ToList().Join(",") + "]");
                }
            }
            else
            {

                foreach (string fieldName in m.globalIndex.GetOrderedKeyFields())
                {
                    if (!m.fieldNameValParsedMap.ContainsKey(fieldName))
                    {
                        throw new Exception("Error, cannot remove from index [" + m.indexName + "], missing value for field [" + fieldName + "]");
                    }
                    m.orderedValuesParsed.Add(m.fieldNameValParsedMap[fieldName]);
                }
            }

            
            // Add the runtime select modudle
            run.Add(m);

        }

        public void Run(ModuleContext mc)
        {
            try
            {
            List<string> orderedValues=new List<string>();
            foreach(IReadString v in this.orderedValuesParsed) orderedValues.Add(v.Read(mc));
                string numRows = this.globalIndex.IndexRemove(mc, orderedValues, this.removalOption);
                this.numRowsParsed.Write(mc, numRows);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot insert_remove from index [" + this.globalIndex.IndexName + "]:" + e.Message, e);
            }
        }
}
}
