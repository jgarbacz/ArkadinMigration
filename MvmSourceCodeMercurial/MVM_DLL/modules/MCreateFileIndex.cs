using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

/*
    <create_index_on_sorted_file>
      <index>'INPUT_INDEX'</index>
      <file>'C:\\_ROB\\mvm\\input.txt'</file>
      <ctrl>'C:\\_ROB\\mvm\\ctrl.txt'</ctrl>
      <key_field>'field_1'</key_field>
      <key_field>'field_2'</key_field>
    </create_index_on_sorted_file>
*/

namespace MVM
{
    public class MCreateIndexOnSortedFile: IModuleSetup,IModuleRun
    {
        // from xml
        private string indexSyntax;
        private string inputFileSyntax;
        private string ctrlFileSyntax;
        private List<string> keyFieldsSyntax = new List<string>();
        // from setup
        private string indexName;
        private IReadString ctrlParsed;
        private IReadString inputFileParsed;
        private List<IReadString> keyFieldsParsed = new List<IReadString>();
        private List<IReadString> fieldNamesParsed = new List<IReadString>();

        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MCreateIndexOnSortedFile m = new MCreateIndexOnSortedFile();
            // xml extraction
            m.indexSyntax = me.SelectNodeInnerText("./index");
            m.inputFileSyntax = me.SelectNodeInnerText("./file");
            m.ctrlFileSyntax = me.SelectNodeInnerText("./ctrl");


            m.keyFieldsSyntax = me.SelectNodesInnerText("./key_field");

            // index must be literal
            if (!mc.IsLiteralString(m.indexSyntax)) throw new Exception("Error, expecting the index name to be a literal string: ["+m.indexSyntax+"]");
            m.indexName = mc.GetLiteralString(m.indexSyntax);

            m.ctrlParsed = mc.ParseSyntax(m.ctrlFileSyntax);
            m.inputFileParsed = mc.ParseSyntax(m.inputFileSyntax);
            m.keyFieldsParsed = mc.ParseSyntax(m.keyFieldsSyntax);

            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string fileName=this.inputFileParsed.Read(mc);
            string ctrl = this.ctrlParsed.Read(mc);
            OracleDumpCtrlFile ctrlFile = new OracleDumpCtrlFile(ctrl);
            string fieldDelim = ctrlFile.fieldDelim;
            string recordDelim = ctrlFile.recordDelim;
            List<string> orderedFieldNames = ctrlFile.GetOrderedFieldNames();
            List<string> orderedKeyFields = new List<string>();
            foreach (IReadString keyFieldParsed in this.keyFieldsParsed)
            {
                string keyFieldName = keyFieldParsed.Read(mc);
                orderedKeyFields.Add(keyFieldName);
                // make sure that we're indexing a field we know of
                int indexColNo = orderedFieldNames.IndexOf(keyFieldName);
                if (indexColNo < 0) throw new Exception("Error, cannot index file. Key field=[" + keyFieldName + "] not in format [" + orderedFieldNames.Join(",") + "]");
            }

            // register the FileIndex globally so we can find it and select from it
            FileIndex fileIndex=FileIndex.CreateFileIndex(
                fileName,
                fieldDelim,
                recordDelim,
                orderedFieldNames,
                orderedKeyFields);
            // create an object that represents this globalContext entity
            mc.globalContext.SetNamedClassInst(this.indexName, fileIndex);
            
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("create file index:"+this.indexName);
        }
    }
}
