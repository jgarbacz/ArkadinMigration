using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

/*

<index_select_keys>
<index>OBJECT.MY_INDEX</index>
<cursor>TEMP.MY_CURSOR</cursor>
<loop>
  :
</loop>
<then>
  :
</then>
<else>
  :
</else>
</index_select_keys>
 */

namespace MVM
{
    class MIndexSelectKeys: IModuleSetup,IModuleRun
    {
        private string indexNameSyntax; 
        private string indexName;
        private List<IReadString> orderedValuesParsed = new List<IReadString>();
        private IIndex globalIndex;
        private CursorSetupCommon cursorSetup;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MIndexSelectKeys m = new MIndexSelectKeys();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            // xml extraction
            m.indexNameSyntax = me.SelectNodeInnerText("./index");

            // ...parsing and setup...

            // For now, only support static index names
            if (!mc.IsLiteralString(m.indexNameSyntax)) throw new Exception("Error, expecting string literal for index name, not [" + m.indexNameSyntax + "]");
            m.indexName = mc.GetLiteralString(m.indexNameSyntax);

            // Since our index is static, lookup and sort out the format now.
            m.globalIndex = (IIndex) mc.globalContext.GetNamedClassInst(m.indexName);

            // possible some key fields were passed
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
                        throw new Exception("Error selecting keys from nested memory index. Keys must be selected left to right without gaps. You attempted to select [" + passedFields.Keys.ToList().Join(",") + "] but did not pass field to left of it named [" + fieldName + "]");
                    }
                }
            }
            if (passedFields.Count > 0)
            {
                throw new Exception("Error selecting keys from nested memory index. You attempted to a select with nonkey field(s) [" + passedFields.Keys.ToList().Join(",") + "]");
            }



            // Add the runtime select modudle
            run.Add(m);
            m.cursorSetup.AddCursorSubProcs(me, mc, run);
        }

       
        public void Run(ModuleContext mc)
        {
            try
            {
                List<string> orderedKeyValues = new List<string>();
                this.orderedValuesParsed.ForEach(f => orderedKeyValues.Add(f.Read(mc)));
                var csr = this.globalIndex.IndexSelectKeys(mc, this.cursorSetup, orderedKeyValues);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot insert_select_keys from index [" + this.globalIndex.IndexName + "]:" + e.Message, e);
            }
        }

    }
}
