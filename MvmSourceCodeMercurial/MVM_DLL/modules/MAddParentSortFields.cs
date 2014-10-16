using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    [Module(@"
        <module_config>
            <name>add_parent_sort_field</name>
            <name>add_parent_sort_fields</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType>
                    <xs:sequence>
                        <xs:element name='input_file' type='xs:string'/>
                        <xs:element name='input_ctrl' type='xs:string'/>
                        <xs:element name='output_file' type='xs:string'/>
                        <xs:element name='output_ctrl' type='xs:string'/>
                        <xs:element name='orphan_file' type='xs:string'/>
                        <xs:element name='orphan_ctrl' type='xs:string'/>
                        <xs:element name='sort_field' type='xs:string'/>
                        <xs:element name='parent_index' type='xs:string' minOccurs='0'/>
                        <xs:element name='parent_key_field' type='xs:string' minOccurs='0' maxOccurs='unbounded'/>
                        <xs:element name='child_index' type='xs:string' minOccurs='0'/>
                        <xs:element name='child_key_field' type='xs:string' minOccurs='0' maxOccurs='unbounded'/>
                        <xs:element name='num_good' type='xs:string' mode='out'/>
                        <xs:element name='num_orphans' type='xs:string' mode='out'/>
                    </xs:sequence>
                </xs:complexType>
            </xsd>
            <doc>
                <description>Adds parent sort fields</description>
            </doc>
        </module_config>
    ")]
    class MAddParentSortFields : BaseModuleSetup, IModuleRun
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
        private List<string> parentKeyFieldsSyntax;
        private string childIndexSyntax;
        private List<string> childKeyFieldsSyntax;
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
        private List<IReadString> parentKeyFieldsParsed;
        private IReadString childIndexParsed;
        private List<IReadString> childKeyFieldsParsed;
        private IWriteString numGoodParsed;
        private IWriteString numOrphansParsed;

        private List<string> fieldsSyntax = new List<string>();
        
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MAddParentSortFields m = new MAddParentSortFields();
            // xml extraction
            m.inputFileSyntax = m.SelectSingleNode(me, "./input_file");
            m.inputCtrlSyntax = m.SelectSingleNode(me, "./input_ctrl");
            m.outputFileSyntax = m.SelectSingleNode(me, "./output_file");
            m.outputCtrlSyntax = m.SelectSingleNode(me, "./output_ctrl");
            m.orphanFileSyntax = m.SelectSingleNode(me, "./orphan_file");
            m.orphanCtrlSyntax = m.SelectSingleNode(me, "./orphan_ctrl");
            m.sortFieldSyntax = m.SelectSingleNode(me, "./sort_field");
            m.parentIndexSyntax = m.SelectSingleNode(me, "./parent_index");
            m.parentKeyFieldsSyntax = m.SelectMultipleNodes(me, "./parent_key_field");
            m.childIndexSyntax = m.SelectSingleNode(me, "./child_index");
            m.childKeyFieldsSyntax = m.SelectMultipleNodes(me, "./child_key_field");
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
            m.parentKeyFieldsParsed = mc.ParseSyntax(m.parentKeyFieldsSyntax);
            m.childIndexParsed = m.childIndexSyntax!=null?mc.ParseSyntax(m.childIndexSyntax):null;
            m.childKeyFieldsParsed = mc.ParseSyntax(m.childKeyFieldsSyntax);
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
            List<string> parentKeyFields = new List<string>();
            foreach (var parentKeyFieldParsed in this.parentKeyFieldsParsed)
            {
                string parentKeyField = parentKeyFieldParsed.Read(mc);
                parentKeyFields.Add(parentKeyField);
            }            
            string childIndex = this.childIndexParsed != null ? this.childIndexParsed.Read(mc) : null;
            List<string> childKeyFields = new List<string>();
            foreach (var childKeyFieldParsed in this.childKeyFieldsParsed)
            {
                string childKeyField = childKeyFieldParsed.Read(mc);
                childKeyFields.Add(childKeyField);
            }


            // read the ctrl file
            OracleDumpCtrlFile inputCtrlFile;
            try
            {
                inputCtrlFile = new OracleDumpCtrlFile(inputCtrl);
            }
            catch (Exception e)
            {
                throw new Exception("Error, cannot do add_parent_sort_fields. ctrl file:" + this.inputCtrlSyntax + "=" + inputCtrl + " is not valid", e);
            }
            List<string> fieldNames = inputCtrlFile.GetOrderedFieldNames();

            // write out the output ctrl file with the sort field
            OracleDumpCtrlFile outputCtrlFile = new OracleDumpCtrlFile(inputCtrl);
            outputCtrlFile.orderedFieldNames.Insert(0, sortField);
            outputCtrlFile.ToFile(outputCtrl);

            int[] parentKeyColNos = null;
            Dictionary<StringArray, string> parentIndexMap = null;
            if (parentIndex != null)
            {
                parentIndexMap = (Dictionary<StringArray, string>)mc.globalContext.GetNamedClassInst(parentIndex);
                parentKeyColNos = new int[parentKeyFields.Count];
                for(int i=0;i<parentKeyFields.Count;i++)
                {
                    string parentKeyField=parentKeyFields[i];
                    int parentKeyColNo = fieldNames.IndexOf(parentKeyField);
                    if (parentKeyColNo < 0) throw new Exception("Error, parent key field [" + parentKeyField + "] not in ctrl file [" + inputCtrl + "]");
                    parentKeyColNos[i] = parentKeyColNo;
                }
            }

            // setup parent index lookup if there is one
            //int parentKeyColNo = -1;
            //Dictionary<string, string> parentIndexMap = null; 
            //if (parentIndex != null)
            //{
            //    parentKeyColNo = fieldNames.IndexOf(parentKeyField);
            //    if (parentKeyColNo < 0) throw new Exception("Error, parent key field [" + parentKeyField + "] not in ctrl file [" + inputCtrl + "]");
            //    parentIndexMap = (Dictionary<string, string>)mc.globalContext.GetNamedClassInst(parentIndex);
            //}

            int[] childKeyColNos = null;
            Dictionary<StringArray, string> childIndexMap = null;
            if (childIndex != null)
            {
                childIndexMap = new Dictionary<StringArray, string>();
                mc.globalContext.SetNamedClassInst(childIndex, childIndexMap);
                childKeyColNos = new int[childKeyFields.Count];
                for (int i = 0; i < childKeyFields.Count; i++)
                {
                    string childKeyField = childKeyFields[i];
                    int childKeyColNo = fieldNames.IndexOf(childKeyField);
                    if (childKeyColNo < 0) throw new Exception("Error, child key field [" + childKeyField + "] not in ctrl file [" + inputCtrl + "]");
                    childKeyColNos[i] = childKeyColNo;
                }
            }

            // setup child index write if there is one
            //int childKeyColNo = -1;
            //Dictionary<string, string> childIndexMap=null;
            //if (childIndex != null)
            //{
            //    childKeyColNo = fieldNames.IndexOf(childKeyField);
            //    if (childKeyColNo < 0) throw new Exception("Error, child key field [" + childKeyField + "] not in ctrl file [" + inputCtrl + "]");
            //    childIndexMap = new Dictionary<string, string>();
            //    mc.globalContext.SetNamedClassInst(childIndex, childIndexMap);
            //}

            
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
                if (parentKeyColNos!=null)
                {
                    StringArray parentValue = new StringArray(parentKeyColNos.Length);
                    for (int i = 0; i < parentKeyColNos.Length; i++)
                    {
                        int parentKeyColNo=parentKeyColNos[i];
                        parentValue[i] = cols[parentKeyColNo];
                    }
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
                if (childKeyColNos != null)
                {
                    StringArray childValue = new StringArray(childKeyColNos.Length);
                    for (int i = 0; i < childKeyColNos.Length; i++)
                    {
                        int childKeyColNo = childKeyColNos[i];
                        childValue[i] = cols[childKeyColNo];
                    }
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
