using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

using Antlr.Runtime.Tree;

namespace MVM
{
    /*
     *  ctrl_file, and fields are all evaluated just 1 time (at setup) for performance purposes
     *  input/output_file are evaluated at runtime.
      <file_grep>
      <input_file>"C:\\_ROB\\mvm\\test_file_sort\\oa.dat.txt"</input_file>
      <output_file>"C:\\_ROB\\mvm\\test_file_sort\\oa.filtered.dat.txt"</output_file>
      <ctrl_file>"C:\\_ROB\\mvm\\test_file_sort\\oa.ctl.txt"</ctrl_file>
      <field name="accessory_id">83</field>
      </file_grep>
     */
    class MFileGrep: IModuleSetup,IModuleRun
    {
        // from xml
        private string inputFileSyntax;
        private string outputFileSyntax;
        private string ctrlFileSyntax;
        private Dictionary<string, string> fieldsSyntax;
        
        // from setup
        private IReadString inputFileParsed;
        private IReadString outputFileParsed;
        private IReadString ctrlFileParsed;
        private Dictionary<string,IReadString> fieldsParsed=new Dictionary<string,IReadString>();

        // Build and compile the regexes just one time.
        private int[] colNos;
        private Regex[] regexes;
        private string fieldDelim;
        private string recordDelim;
        private int numFields;


        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MFileGrep m = new MFileGrep();
            // xml extraction
            m.inputFileSyntax = me.SelectNodeInnerText("./input_file");
            m.outputFileSyntax = me.SelectNodeInnerText("./output_file");
            m.ctrlFileSyntax = me.SelectNodeInnerText("./ctrl_file");
            m.fieldsSyntax = me.SelectNodes("./field").ToNameTextDictionary();
            // parsing
            m.inputFileParsed = mc.ParseSyntax(m.inputFileSyntax);
            m.outputFileParsed = mc.ParseSyntax(m.outputFileSyntax);
            m.ctrlFileParsed = mc.ParseSyntax(m.ctrlFileSyntax);
            foreach (string field in m.fieldsSyntax.Keys)
            {
                string value = m.fieldsSyntax[field];
                m.fieldsParsed[field] = mc.ParseSyntax(value);
            }

            // read ctrl file and get format info
            string ctrl = m.ctrlFileParsed.Read(mc);
            OracleDumpCtrlFile ctrlFile;
            try
            {
                ctrlFile = new OracleDumpCtrlFile(ctrl);
            }
            catch (Exception e)
            {
                throw new Exception("Error, cannot do file_sort. ctrl file:" + m.ctrlFileSyntax + "=" + ctrl + " is not valid", e);
            }
            m.numFields = ctrlFile.GetOrderedFieldNames().Count;
            m.fieldDelim = ctrlFile.fieldDelim;
            m.recordDelim = ctrlFile.recordDelim;
            List<string> fieldNames = ctrlFile.GetOrderedFieldNames();

            m.colNos = new int[m.fieldsParsed.Count];
            m.regexes = new Regex[m.fieldsParsed.Count];

            int idx = 0;
            foreach (string fieldName in m.fieldsParsed.Keys)
            {
                int colNo = fieldNames.IndexOf(fieldName);
                if (colNo < 0) throw new Exception("Error, can't sort by field [" + fieldName + "] because it isn't in the ctrl file [" + ctrl + "]");
                IReadString regexParsed = m.fieldsParsed[fieldName];
                string regexPat = regexParsed.Read(mc);
                m.colNos[idx] = colNo;
                m.regexes[idx] = new Regex(regexPat,RegexOptions.Compiled);
                idx++;
            }

            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string inputFileName = this.inputFileParsed.Read(mc);
            string outputFileName = this.outputFileParsed.Read(mc);

            // if output=input, make output tempContext, then rename.
            bool isTemp=false;
            if(inputFileName.Equals(outputFileName)){
                isTemp=true;
                outputFileName = System.IO.Path.GetTempFileName(); 
            }


            string[] fdelArr = new String[] { this.fieldDelim };
            // open file and do regexes on it.
            RecordReader rr = new RecordReader(inputFileName, this.recordDelim);
            StreamWriter sw = new StreamWriter(outputFileName);
            string ln;
            while ((ln = rr.ReadLine()) != null)
            {
                string[] cols = ln.Split(fdelArr, StringSplitOptions.None);
                for (int i = 0; i < this.colNos.Length; i++)
                {
                    int colNo=this.colNos[i];
                    Regex r=this.regexes[i];
                    string val = cols[colNo];
                    if (!r.IsMatch(val)) goto skip_print;
                }
                sw.Write(ln);
                sw.Write(this.recordDelim);
            skip_print: { }
            }
            sw.Close();
            rr.Close();

            if (isTemp)
            {
                File.Delete(inputFileName);
                File.Move(outputFileName, inputFileName);
            }
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("file_grep:"+this.inputFileSyntax);
        }
    }
}
