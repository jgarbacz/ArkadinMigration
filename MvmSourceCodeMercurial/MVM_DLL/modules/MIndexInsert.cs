using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

/*

<index_insert>
<index>OBJECT.MY_INDEX</index>
<field name='field_1'>'1'</field>
</index_insert>
 */

namespace MVM
{
    class MIndexInsert: IModuleSetup,IModuleRun
    {
        private string indexNameSyntax; 
        private string indexName;
        private Dictionary<string, IReadString> fieldNameValParsedMap = new Dictionary<string, IReadString>();
        private List<IReadString> orderedValuesParsed = new List<IReadString>();
        private IIndex globalIndex;

        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MIndexInsert m = new MIndexInsert();

            // xml extraction
            m.indexNameSyntax = me.SelectNodeInnerText("./index");
            Overrider overrides = new Overrider(me);

            // ...parsing and setup...

            // For now, only support static index names
            if (!mc.IsLiteralString(m.indexNameSyntax)) throw new Exception("Error, expecting string literal for index name, not [" + m.indexNameSyntax + "]");
            m.indexName = mc.GetLiteralString(m.indexNameSyntax);

            // Since our index is static, lookup and sort out the format now.
            m.globalIndex = (IIndex) mc.globalContext.GetNamedClassInst(m.indexName);

            // Ask the index what newOrder it wants its fields in so we can setup our ordered values to be passed to it.
            foreach (string fieldName in m.globalIndex.GetOrderedValueFields())
            {
                m.orderedValuesParsed.Add(mc.ParseSyntax(overrides.GetSyntax(fieldName)));
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
            this.globalIndex.IndexInsert(mc,orderedValues);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot insert into index [" + this.globalIndex.IndexName + "]:" + e.Message, e);
            }
        }
}
}
