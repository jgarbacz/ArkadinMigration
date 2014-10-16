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
     * HARD CODED TO ASSUME ESCAPES
     * input_ctrl, and fields are all evaluated just 1 time (at setup) for performance purposes
     * input/output_file are evaluated at runtime.
      <file_convert>
      <input_file>"C:\\_ROB\\mvm\\test_file_sort\\oa.dat.txt"</input_file>
      <input_ctrl>"C:\\_ROB\\mvm\\test_file_sort\\oa.ctl.txt"</input_ctrl>
      <output_file>"C:\\_ROB\\mvm\\test_file_sort\\oa.filtered.dat.txt"</output_file>
      <output_ctrl>"C:\\_ROB\\mvm\\test_file_sort\\oa.ctl.txt"</output_ctrl>
      <bad_file>"C:\\_ROB\\mvm\\test_file_sort\\oa.filtered.dat.txt"</bad_file>
      <new_record_delim>GLOBAL.output_field_delim</new_record_delim>
      <new_field_delim>GLOBAL.output_record_delim</new_field_delim>
      </file_convert>
     */
    class MFileConvert : IModuleSetup, IModuleRun
    {
        // from xml
        private string inputFileSyntax;
        private string outputFileSyntax;
        private string badFileSyntax;
        private string inputCtrlSyntax;
        private string outputCtrlSyntax;
        private string newFieldDelimSyntax;
        private string newRecordDelimSyntax;
        private string numGoodSyntax;
        private string numBadSyntax;

        // from setup
        private IReadString inputFileParsed;
        private IReadString outputFileParsed;
        private IReadString badFileParsed;
        private IReadString inputCtrlParsed;
        private IReadString outputCtrlParsed;
        private IWriteString numGoodParsed;
        private IWriteString numBadParsed;

        private string fieldDelim;
        private string recordDelim;
        private int numFields;
        private static char escape = '\\';
        private string newFieldDelim;
        private string newRecordDelim;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MFileConvert m = new MFileConvert();
            // xml extraction
            m.inputFileSyntax = me.SelectNodeInnerText("./input_file");
            m.outputFileSyntax = me.SelectNodeInnerText("./output_file");
            m.badFileSyntax = me.SelectNodeInnerText("./bad_file");
            m.inputCtrlSyntax = me.SelectNodeInnerText("./input_ctrl");
            m.outputCtrlSyntax = me.SelectNodeInnerText("./output_ctrl");
            m.newFieldDelimSyntax = me.SelectNodeInnerText("./new_field_delim");
            m.newRecordDelimSyntax = me.SelectNodeInnerText("./new_record_delim");
            m.numGoodSyntax = me.SelectNodeInnerText("./num_good");
            m.numBadSyntax = me.SelectNodeInnerText("./num_bad");
            // parsing
            m.inputFileParsed = mc.ParseSyntax(m.inputFileSyntax);
            m.outputFileParsed = mc.ParseSyntax(m.outputFileSyntax);
            m.badFileParsed = mc.ParseSyntax(m.badFileSyntax);
            m.inputCtrlParsed = mc.ParseSyntax(m.inputCtrlSyntax);
            m.outputCtrlParsed = mc.ParseSyntax(m.outputCtrlSyntax);
            m.numGoodParsed = mc.ParseWritableSyntax(m.numGoodSyntax);
            m.numBadParsed = mc.ParseWritableSyntax(m.numBadSyntax);

            // read the new delims
            m.newFieldDelim = mc.ParseSyntax(m.newFieldDelimSyntax).Read(mc).InterpolateEscapes();
            m.newRecordDelim = mc.ParseSyntax(m.newRecordDelimSyntax).Read(mc).InterpolateEscapes();

            // read ctrl file and get format info
            string inputCtrl = m.inputCtrlParsed.Read(mc);
            OracleDumpCtrlFile ctrlFile;
            try
            {
                ctrlFile = new OracleDumpCtrlFile(inputCtrl);
            }
            catch (Exception e)
            {
                throw new Exception("Error, cannot do file_sort. ctrl file:" + m.inputCtrlSyntax + "=" + inputCtrl + " is not valid", e);
            }
            m.numFields = ctrlFile.GetOrderedFieldNames().Count;
            m.fieldDelim = ctrlFile.fieldDelim;
            m.recordDelim = ctrlFile.recordDelim;
            List<string> fieldNames = ctrlFile.GetOrderedFieldNames();

            // write out the new ctrl file
            string outputCtrl = m.outputCtrlParsed.Read(mc);
            ctrlFile.fieldDelim = m.newFieldDelim;
            ctrlFile.recordDelim = m.newRecordDelim;
            ctrlFile.ToFile(outputCtrl);

            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            long numGood = 0;
            long numBad = 0;
            string inputFileName = this.inputFileParsed.Read(mc);
            string outputFileName = this.outputFileParsed.Read(mc);
            string badFileName = this.badFileParsed.Read(mc);
            char[] fdelArr = this.fieldDelim.ToCharArray();
            int fdelLen = fdelArr.Length;
            int fdelMax = fdelLen - 1;
            RecordReaderEscape rr = new RecordReaderEscape(inputFileName, this.recordDelim);
            StreamWriter cvtSw = new StreamWriter(outputFileName);
            StreamWriter badSw = null;
            StringBuilder sw = new StringBuilder();
            string line;
            while ((line = rr.ReadLine()) != null)
            {
                //Console.WriteLine("line=[" + line + "]");
                char[] ln = line.ToCharArray();
                int colNo = 0;
                bool escapeOn = false;
                bool nextEscapeOn = false;
                int fdelPos = 0;
                int lnPos = 0;
                int lnMrk = lnPos;
                for (; ; )
                {
                    escapeOn = nextEscapeOn;
                    // if at end of string, flush the rest to string buffer, write col, and cut out
                    if (lnPos >= ln.Length)
                    {
                        if (escapeOn || (colNo != (numFields - 1)))
                        {
                            numBad++; 
                            string msg = "";
                            int foundFlds = colNo + 1;
                            if (escapeOn) msg += "[dangling escape]";
                            if (colNo != (numFields - 1)) msg += "[found " + foundFlds + " need " + numFields + "]";
                            if (badSw == null) badSw = new StreamWriter(badFileName);
                            badSw.Write(msg);
                            badSw.Write(line);
                            badSw.Write(recordDelim);
                        }
                        else
                        {
                            numGood++;
                            int len = lnPos - lnMrk;
                            sw.Append(ln, lnMrk, len);
                            if (colNo < (numFields - 1)) sw.Append(newFieldDelim); // not needed
                            sw.Append(newRecordDelim);
                            cvtSw.Write(sw);
                        }
                        sw.Length = 0;
                        break;
                    }
                    // get current char
                    char c = ln[lnPos];
                    // if we're not escaped, but this char is an escape, then flush up to this char and set mark ahead of it
                    if ((!escapeOn) && c == escape)
                    {
                        int len = lnPos - lnMrk;
                        sw.Append(ln, lnMrk, len);
                        lnPos++;
                        lnMrk = lnPos;
                        nextEscapeOn = true;
                        continue;
                    }
                    // set next escape on
                    nextEscapeOn = c == escape ? !escapeOn : false;
                    // if we think we're in a delim, but we don't match next byte, rollback
                    if (fdelPos > 0 && !c.Equals(fdelArr[fdelPos]))
                    {
                        lnPos = lnPos - fdelPos + 1;
                        fdelPos = 0;
                        continue;
                    }
                    // check the field delim
                    if ((!escapeOn) && c.Equals(fdelArr[fdelPos]))
                    {
                        if (fdelPos == fdelMax)
                        {
                            int len = lnPos - lnMrk + 1;
                            len -= fdelLen;
                            sw.Append(ln, lnMrk, len);
                            if (colNo < (numFields - 1)) sw.Append(newFieldDelim);
                            lnPos++;
                            lnMrk = lnPos;
                            colNo++;
                            continue;
                        }
                        // just increment to the next spot
                        fdelPos += 1;
                    }
                    lnPos++;
                }
            }
            cvtSw.Close();
            if (badSw != null) badSw.Close();
            this.numGoodParsed.Write(mc, numGood.ToString());
            this.numBadParsed.Write(mc, numBad.ToString());
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("file_grep:" + this.inputFileSyntax);
        }
    }
}
