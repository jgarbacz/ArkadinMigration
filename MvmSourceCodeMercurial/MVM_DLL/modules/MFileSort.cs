using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    /*
<sort_file>
<input_file>'C:\\_ROB\\mvm'</input_file>
<sorted_file>'C:\\_ROB\\mvm'</sorted_file>
<ctrl>'C:\\_ROB\\mvm\\test_ctrl.txt</ctrl>
<field name="blah"/>
</sort_file>
     */

    class MFileSort: IModuleSetup,IModuleRun
    {
        // from xml
        private string inputFileSyntax;
        private string sortedFileSyntax;
        private string ctrlSyntax;
        private List<string> fieldsSyntax=new List<string>();
        
        // from setup
        private IReadString inputFileParsed;
        private IReadString sortedFileParsed;
        private IReadString ctrlParsed;
        private List<IReadString> fieldsParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MFileSort m = new MFileSort();
            // xml extraction
            m.inputFileSyntax = me.SelectNodeInnerText("./input_file");
            m.sortedFileSyntax = me.SelectNodeInnerText("./sorted_file");
            m.ctrlSyntax = me.SelectNodeInnerText("./ctrl");
            m.fieldsSyntax = me.SelectNodesInnerText("./field");
           
            // parsing
            m.inputFileParsed = mc.ParseSyntax(m.inputFileSyntax);
            m.sortedFileParsed = mc.ParseSyntax(m.sortedFileSyntax);
            m.ctrlParsed = mc.ParseSyntax(m.ctrlSyntax);
            m.fieldsParsed = mc.ParseSyntax(m.fieldsSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string inputFileName = this.inputFileParsed.Read(mc);
            string sortedFileName = this.sortedFileParsed.Read(mc);

            // read ctrl file and get format info
            string ctrl = this.ctrlParsed.Read(mc);
            OracleDumpCtrlFile ctrlFile;
            try
            {
                ctrlFile = new OracleDumpCtrlFile(ctrl);
            }
            catch (Exception e)
            {
                throw new Exception("Error, cannot do file_sort. ctrl file:" +this.ctrlSyntax+"="+ctrl+" is not valid", e);
            }
            string fieldDelim =ctrlFile.fieldDelim;
            string recordDelim = ctrlFile.recordDelim;
            List<string> fieldNames=ctrlFile.GetOrderedFieldNames();

            int[] colNos=new int[this.fieldsParsed.Count];
            int idx = 0;
            foreach (IReadString f in this.fieldsParsed)
            {
                string fieldName = f.Read(mc);
                int colNo = fieldNames.IndexOf(fieldName);
                if (colNo < 0) throw new Exception("Error, can't sort by field [" + fieldName + "] because it isn't in the ctrl file [" + ctrl + "]");
                colNos[idx++] = colNo;
            }
            // run the sort
            MergeSort.MergeSortFile(1024*1024*100,inputFileName, sortedFileName, fieldDelim, recordDelim, colNos);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("file_sort:"+this.inputFileSyntax);
        }
    }
}
