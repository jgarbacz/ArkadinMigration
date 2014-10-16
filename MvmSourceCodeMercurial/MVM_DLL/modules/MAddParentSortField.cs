using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    class MAddParentSortField: BaseModuleSetup, IModuleRun
    {
        // from xml
        private string inputFileSyntax;
        private string inputCtrlSyntax;
        private string outputFileSyntax;
        private string outputCtrlSyntax;
        private string orphanFileSyntax;
        private string orphanCtrlSyntax;
        private string sortFieldSyntax;
        private string parentIndexSyntax;
        private string parentKeyFieldSyntax;
        private string childIndexSyntax;
        private string childKeyFieldSyntax;
        private string numGoodSyntax;
        private string numOrphansSyntax;
        // from setup
        private IReadString inputFileParsed;
        private IReadString inputCtrlParsed;
        private IReadString outputFileParsed;
        private IReadString outputCtrlParsed;
        private IReadString orphanFileParsed;
        private IReadString orphanCtrlParsed;
        private IReadString sortFieldParsed;
        private IReadString parentIndexParsed;
        private IReadString parentKeyFieldParsed;
        private IReadString childIndexParsed;
        private IReadString childKeyFieldParsed;
        private IWriteString numGoodParsed;
        private IWriteString numOrphansParsed;

        private List<string> fieldsSyntax = new List<string>();
        
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MAddParentSortField m = new MAddParentSortField();
            // xml extraction
            m.inputFileSyntax = m.SelectSingleNode(me, "./input_file");
            m.inputCtrlSyntax = m.SelectSingleNode(me, "./input_ctrl");
            m.outputFileSyntax = m.SelectSingleNode(me, "./output_file");
            m.outputCtrlSyntax = m.SelectSingleNode(me, "./output_ctrl");
            m.orphanFileSyntax = m.SelectSingleNode(me, "./orphan_file");
            m.orphanCtrlSyntax = m.SelectSingleNode(me, "./orphan_ctrl");
            m.sortFieldSyntax = m.SelectSingleNode(me, "./sort_field");
            m.parentIndexSyntax = m.SelectSingleNode(me, "./parent_index");
            m.parentKeyFieldSyntax = m.SelectSingleNode(me, "./parent_key_field");
            m.childIndexSyntax = m.SelectSingleNode(me, "./child_index");
            m.childKeyFieldSyntax = m.SelectSingleNode(me, "./child_key_field");
            m.numGoodSyntax = m.SelectSingleNode(me, "./num_good");
            m.numOrphansSyntax = m.SelectSingleNode(me, "./num_orphans");
            // parsing
            m.inputFileParsed = mc.ParseSyntax(m.inputFileSyntax);
            m.inputCtrlParsed = mc.ParseSyntax(m.inputCtrlSyntax);
            m.outputFileParsed = mc.ParseSyntax(m.outputFileSyntax);
            m.outputCtrlParsed = mc.ParseSyntax(m.outputCtrlSyntax);
            m.orphanFileParsed = mc.ParseSyntax(m.orphanFileSyntax);
            m.orphanCtrlParsed = mc.ParseSyntax(m.orphanCtrlSyntax);
            m.sortFieldParsed = mc.ParseSyntax(m.sortFieldSyntax);
            m.parentIndexParsed = m.parentIndexSyntax!=null?mc.ParseSyntax(m.parentIndexSyntax):null;
            m.parentKeyFieldParsed = m.parentKeyFieldSyntax!=null?mc.ParseSyntax(m.parentKeyFieldSyntax):null;
            m.childIndexParsed = m.childIndexSyntax!=null?mc.ParseSyntax(m.childIndexSyntax):null;
            m.childKeyFieldParsed = m.childKeyFieldSyntax!=null?mc.ParseSyntax(m.childKeyFieldSyntax):null;
            m.numGoodParsed = mc.ParseWritableSyntax(m.numGoodSyntax);
            m.numOrphansParsed = mc.ParseWritableSyntax(m.numOrphansSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            // evaluate the parsing
            string inputFile = this.inputFileParsed.Read(mc);
            string inputCtrl = this.inputCtrlParsed.Read(mc);
            string outputFile = this.outputFileParsed.Read(mc);
            string outputCtrl = this.outputCtrlParsed.Read(mc);
            string orphanFile = this.orphanFileParsed.Read(mc);
            string orphanCtrl = this.orphanCtrlParsed.Read(mc);
            string sortField = this.sortFieldParsed.Read(mc);
            string parentIndex = this.parentIndexParsed != null ? this.parentIndexParsed.Read(mc) : null;
            string parentKeyField = this.parentKeyFieldParsed != null ? this.parentKeyFieldParsed.Read(mc) : null;
            string childIndex = this.childIndexParsed != null ? this.childIndexParsed.Read(mc) : null;
            string childKeyField = this.childKeyFieldParsed != null ? this.childKeyFieldParsed.Read(mc) : null;

            // read the ctrl file
            OracleDumpCtrlFile inputCtrlFile;
            try
            {
                inputCtrlFile = new OracleDumpCtrlFile(inputCtrl);
            }
            catch (Exception e)
            {
                throw new Exception("Error, cannot do add_parent_sort_field. ctrl file:" + this.inputCtrlSyntax + "=" + inputCtrl + " is not valid", e);
            }
            List<string> fieldNames = inputCtrlFile.GetOrderedFieldNames();

            // write out the output ctrl file with the sort field
            OracleDumpCtrlFile outputCtrlFile = new OracleDumpCtrlFile(inputCtrl);
            outputCtrlFile.orderedFieldNames.Insert(0, sortField);
            outputCtrlFile.ToFile(outputCtrl);

            // setup parent index lookup if there is one
            int parentKeyColNo = -1;
            Dictionary<string, string> parentIndexMap = null; 
            if (parentIndex != null)
            {
                parentKeyColNo = fieldNames.IndexOf(parentKeyField);
                if (parentKeyColNo < 0) throw new Exception("Error, parent key field [" + parentKeyField + "] not in ctrl file [" + inputCtrl + "]");
                parentIndexMap = (Dictionary<string, string>)mc.globalContext.GetNamedClassInst(parentIndex);
            }

            // setup child index write if there is one
            int childKeyColNo = -1;
            Dictionary<string, string> childIndexMap=null;
            if (childIndex != null)
            {
                childKeyColNo = fieldNames.IndexOf(childKeyField);
                if (childKeyColNo < 0) throw new Exception("Error, child key field [" + childKeyField + "] not in ctrl file [" + inputCtrl + "]");
                childIndexMap = new Dictionary<string, string>();
                mc.globalContext.SetNamedClassInst(childIndex, childIndexMap);
            }

            
            StreamWriter orphanSw=null;
            StreamWriter sw = new StreamWriter(outputFile);
            string rdel = inputCtrlFile.recordDelim;
            string fdel = inputCtrlFile.fieldDelim;
            RecordReader rr = new RecordReader(inputFile, rdel);
            string[] fdelArr = new String[] { fdel };
            string ln;
            long lnNo = 0;
            long numOrphans = 0;
            while ((ln = rr.ReadLine()) != null)
            {
                lnNo++;
                string[] cols = ln.Split(fdelArr, StringSplitOptions.None);

                // Lookup or assign the sort key
                string sortKey;
                if (parentKeyColNo >=0)
                {
                    string parentValue = cols[parentKeyColNo];
                    if (!parentIndexMap.ContainsKey(parentValue))
                    {
                        if (orphanSw == null)
                        {
                            orphanSw = new StreamWriter(orphanFile);
                            inputCtrlFile.ToFile(orphanCtrl);
                        }
                        orphanSw.Write(ln);
                        orphanSw.Write(inputCtrlFile.recordDelim);
                        numOrphans++;
                        continue;
                    }
                    sortKey = parentIndexMap[parentValue] + "-" + lnNo.ToString();
                }
                else
                {
                    sortKey = lnNo.ToString();
                }

                // write out the new record
                sw.Write(sortKey);
                sw.Write(fdel);
                sw.Write(ln);
                sw.Write(rdel);

                // if we need to update the child index
                if (childKeyColNo >= 0)
                {
                    string childValue = cols[childKeyColNo];
                    childIndexMap[childValue] = sortKey;
                }
            }
            rr.Close();
            sw.Close();
            if (orphanSw != null) orphanSw.Close();
            this.numGoodParsed.Write(mc,(lnNo - numOrphans).ToString());
            this.numOrphansParsed.Write(mc, numOrphans.ToString());
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("add_parent_sort_field:");
        }
    }
}
