using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace MVM
{
    [Module(@"
        <module_config>
            <name>batch_select</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType>
                    <xs:sequence>
                        <xs:element name='batch_start' type='xs:string' minOccurs='0' maxOccurs='1' default='""batch_start""'/>
                        <xs:element name='batch_end' type='xs:string' minOccurs='0' maxOccurs='1' default='""batch_end""'/>
                        <xs:element name='first_number' type='xs:string' minOccurs='0' maxOccurs='1' datatype='integer' default='1'/>
                        <xs:element name='last_number' type='xs:string' minOccurs='0' maxOccurs='1' datatype='integer' default='1'/>
                        <xs:element name='num_batches' type='xs:string' minOccurs='0' maxOccurs='1' datatype='integer' default='1'/>
                        <xs:group ref='cursor_operation_group'/>
                    </xs:sequence>
                </xs:complexType>
            </xsd>
            <doc>
                <category>Batch Operations</category>
                <description>Selects from a batch</description>
            </doc>
        </module_config>
    ")]
    class MBatchSelect : BaseModuleSetup, IModuleRun
    {
        // from xml
        private string batchStartSyntax;
        private string batchEndSyntax;
        private string firstNumberSyntax;
        private string lastNumberSyntax;
        private string numBatchesSyntax;
        
        // from setup
        private IReadString batchStartParsed;
        private IReadString batchEndParsed;
        private IReadString firstNumberParsed;
        private IReadString lastNumberParsed;
        private IReadString numBatchesParsed;

        private CursorSetupCommon cursorSetup;
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MBatchSelect m = new MBatchSelect();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            // xml extraction
            m.batchStartSyntax = m.SelectSingleNode(me, "./batch_start");
            m.batchEndSyntax = m.SelectSingleNode(me, "./batch_end");
            m.firstNumberSyntax = m.SelectSingleNode(me, "./first_number");
            m.lastNumberSyntax = m.SelectSingleNode(me, "./last_number");
            m.numBatchesSyntax = m.SelectSingleNode(me, "./num_batches");
            // parsing
            m.batchStartParsed = mc.ParseSyntax(m.batchStartSyntax);
            m.batchEndParsed = mc.ParseSyntax(m.batchEndSyntax);
            m.firstNumberParsed = mc.ParseSyntax(m.firstNumberSyntax);
            m.lastNumberParsed = mc.ParseSyntax(m.lastNumberSyntax);
            m.numBatchesParsed = mc.ParseSyntax(m.numBatchesSyntax);
            // runtime
            run.Add(m);
            m.cursorSetup.AddCursorSubProcs(me, mc, run);
        }

        public void Run(ModuleContext mc)
        {
            // setup the cursor
            string batchStartField=this.batchStartParsed.Read(mc);
            string batchEndField = this.batchEndParsed.Read(mc);
            long firstNumber = long.Parse(this.firstNumberParsed.Read(mc));
            long lastNumber = long.Parse(this.lastNumberParsed.Read(mc));
            long numBatches = long.Parse(this.numBatchesParsed.Read(mc));
            var csr = new BatchSelectCursor(mc, this.cursorSetup, batchStartField, batchEndField, firstNumber, lastNumber, numBatches);
        }
   }


    public class BatchSelectCursor: CursorCommonLinqEnabled,ICursor
    {
        public string batchStartField;
        public string batchEndField;
        public long batchesRemaining;
        public long batchStart;
        public long batchSize;
        public long batchEnd;
        public long numberRemaining;
        public BatchSelectCursor(ModuleContext mc, CursorSetupCommon setup, string batchStartField, string batchEndField, long firstNumber, long lastNumber, long numBatches)
            : base(mc, setup)
        {
            this.batchStartField = batchStartField;
            this.batchEndField = batchEndField;
            this.orderedFieldNames.Add(batchStartField);
            this.orderedFieldNames.Add(batchEndField);
            this.batchEnd = firstNumber-1;
            this.numberRemaining = lastNumber-firstNumber+1;
            this.batchesRemaining = numBatches + 1;
        }
        
        public override IObjectData CursorNext()
        {
            if (numberRemaining <= 0)
            {
                return null;
            }
            batchesRemaining -= 1;
            batchStart = this.batchEnd + 1;
            batchSize = (long)Math.Ceiling((double)numberRemaining / (double)batchesRemaining);
            if (batchSize <= 0)
            {
                return null;
            }
            batchEnd = batchStart + batchSize - 1;
            numberRemaining -= batchSize;
            using (var csrObj = this.CreateNewObject())
            {
                csrObj[batchStartField] = batchStart.ToString();
                csrObj[batchEndField] = batchEnd.ToString();
                return csrObj;
            }
        }
        public override void CursorClear()
        {
            // no resources to free
        }
    }
}
