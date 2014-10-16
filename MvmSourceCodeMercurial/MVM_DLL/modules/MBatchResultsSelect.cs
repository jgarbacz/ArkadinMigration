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
            <name>batch_output_select</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType>
                    <xs:sequence>
                        <xs:element name='batch_id' type='xs:string' minOccurs='1' maxOccurs='1' datatype='string' mode='in' description='batch identifier'/>
                        <xs:group ref='cursor_operation_group'/>
                    </xs:sequence>
                </xs:complexType>
            </xsd>
            <doc>
                <category>Batch Operations</category>
                <description>Selects the output of a batch</description>
            </doc>
        </module_config>
    ")]
    class MBatchOutputSelect : BaseModuleSetup, IModuleRun
    {
        private string batchIdSyntax;
        private IReadString batchIdParsed;
        private CursorSetupCommon cursorSetup;
        public override  void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MBatchOutputSelect m = new MBatchOutputSelect();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            m.batchIdSyntax = m.SelectSingleNode(me, "./batch_id");
            m.batchIdParsed = mc.ParseSyntax(m.batchIdSyntax);
            run.Add(m);
            m.cursorSetup.AddCursorSubProcs(me, mc, run);
        }

        public void Run(ModuleContext mc)
        {
            // get the batch to select
            string batchId = this.batchIdParsed.Read(mc);

           // lookup the batch
            long batchIdLong=long.Parse(batchId);
            WorkBatch workBatch=mc.mvm.remoteWorkMgr.LookupBatch(batchIdLong);

            // Instanciate a cursor
            var csr = new BatchOutputSelectCursor(mc, this.cursorSetup, workBatch);

            // clear out the batch
            mc.mvm.remoteWorkMgr.ClearBatch(batchIdLong);
    }
    }

    public class BatchOutputSelectCursor : CursorCommonLinqEnabled, ICursor
    {
        public WorkBatch workBatch;
        public IEnumerator<WorkInfo> workInfoEnum;
        public BatchOutputSelectCursor(ModuleContext mc, CursorSetupCommon setup, WorkBatch workBatch)
         :base(mc,setup)
        {
            this.workBatch = workBatch;
            this.workInfoEnum = workBatch.GetWorkEnumerator();
        }

        public override IObjectData CursorNext()
        {

                // if empty set to false
                if (!this.workInfoEnum.MoveNext())
                {
                    return null;
                }
                using (var csrObj = this.CreateNewObject())
                {
                // Otherwise copy the outputs and return true
                foreach (var entry in this.workInfoEnum.Current.outputs as Dictionary<string,string>)
                {
                    csrObj[entry.Key] = entry.Value;
                }
                return csrObj;
            }
        }

        public override void CursorClear()
        {
            // no resources to free
        }

    }
}
