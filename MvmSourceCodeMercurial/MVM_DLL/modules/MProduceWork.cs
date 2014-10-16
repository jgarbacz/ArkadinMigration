using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;

namespace MVM
{

    /*
    <produce>
    <cursor>${CSR}<cursor>
    <producer_cursor_field>'cursor'</producer_cursor_field>
    <object_type>'ACCOUNT'<object_type>  
    <object_id>'ACCOUNT|'~OBJECT(OBJECT.cursor).acct_ext_id<object_id> 
    <batch_size>1000<batch_size>                                            
    <consumer_proc_name>'processing'</consumer_proc_name>             
    </produce>
     */
    class MProduceWork : IModuleSetup, IModuleRun
    {
       // private static long genCtr = 0;
        private string cursorSyntax;
        private string objectType;
        private string objectId;
        private string batchSize;
        private string consumerProcName;
        private string producerCursorFieldSyntax;
        private IReadString cursorParsed;
        private IReadString producerCursorFieldParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MProduceWork m = new MProduceWork();
            // xml extraction
            m.cursorSyntax = me.SelectNodeInnerText("./cursor");
            m.objectType = me.SelectNodeInnerText("./object_type");
            m.objectId = me.SelectNodeInnerText("./object_id");
            m.batchSize = me.SelectNodeInnerText("./batch_size", "GLOBAL.produce_batch_size");
            m.consumerProcName = me.SelectNodeInnerText("./consumer_proc_name");
            m.producerCursorFieldSyntax = me.SelectNodeInnerText("./producer_cursor_field", "'cursor'");

            // parsing
            m.cursorParsed = mc.ParseSyntax(m.cursorSyntax);
            m.producerCursorFieldParsed = mc.ParseSyntax(m.producerCursorFieldSyntax);

            // runtime
            run.Add(m);
        }


        public static int producerCtr = 1;
        public void Run(ModuleContext mc)
        {
            // build up the string to copy from the cursor to the object
            string csrOid = this.cursorParsed.Read(mc);
            ICursor cursor = (ICursor)mc.globalContext.GetNamedClassInst(csrOid);

            // Get the procNameSyntax to the caller
            string callback = mc.workMgr.CreateCallback(mc.procInst).ToString();

            // get the name of the object field on the producer that has the cursor
            string producerCursorField = this.producerCursorFieldParsed.Read(mc);

            // spawn a producer object
            string producerOid = mc.Spawn("PRODUCER");
            mc.WriteObjectField(producerOid, producerCursorField, csrOid);
            mc.WriteObjectField(producerOid, "ctr", "1");  // ctr needs to be the number of workers that have the producer procInst
            mc.WriteObjectField(producerOid, "callback", callback);

            //List<string> copyFields = new List<string>();
            //List<string> RobPortThisToNotUseCodeGen;
//            if(1+1==2)throw new Exception("need to port this to not use code gen");
//            foreach (string field in RobPortThisToNotUseCodeGen)
//            {
//                copyFields.Add("<do>OBJECT(TEMP.oid)." + field + "=OBJECT(${CSR})." + field + "</do>");
//            }
//            string copyCsr = copyFields.Join("\r\n");

//            // generate a name for the producer procContext
//            string producerProcName = "GENERATED_PRODUCER_" + (genCtr++).ToString();

//            // build up the procContext string
//            string producerProc =
//@"
// <proc name='${PRODUCER}'>
//      <finest>'PRODUCE==>csr_oid=' ~ ${CSR} ~ ', CSR.eof=' ~ OBJECT(${CSR}).eof</finest>
//      <if>
//        <condition>!OBJECT(${CSR}).eof</condition>
//        <then>
//          <finest>'Start by self queuing producer ot=${PRODUCER} oid='~OBJECT.object_id</finest>
//          <do>OBJECT.ctr+=1</do>
//          <queue_proc_for_current_object>
//            <name>'${PRODUCER}'</name>
//          </queue_proc_for_current_object>
//          <do>TEMP.batch_ctr=${BATCHSIZE}</do>
//          <while>
//            <condition>!OBJECT(${CSR}).eof &amp;&amp; TEMP.batch_ctr GT 0</condition>
//            <loop>
//              <do>TEMP.batch_ctr-=1</do>
//              <do>TEMP.oid=${OBJECTID}</do>
//              <create_cluster>TEMP.cluster_oid</create_cluster>
//              <spawn>
//                <object_type>${OBJECTTYPE}</object_type>
//                <object_id>TEMP.oid</object_id>
//                <cluster_object_id>TEMP.cluster_oid</cluster_object_id>
//              </spawn>
//              <finest>'PRODUCED CLUSTER='~TEMP.cluster_oid~', having object objectType='~${OBJECTTYPE}~',oid='~TEMP.oid</finest>
//${COPYCSR}
//              <queue_proc_for_object_in_cluster>
//                <name>${CONSUMER}</name>
//                <object_id>TEMP.cluster_oid</object_id>
//                <cluster_object_id>TEMP.cluster_oid</cluster_object_id>
//              </queue_proc_for_object_in_cluster>
//              <cursor_next>${CSR}</cursor_next>
//            </loop>
//          </while>
//        </then>
//        <else>
//            <do>OBJECT.ctr-=1</do>
//            <finest>'DONE PRODUCING with ${PRODUCER}, ctr=' ~ OBJECT.ctr</finest>
//            <if>
//              <condition>OBJECT.ctr==1</condition>
//              <then>
//              <callback>OBJECT.callback</callback>
//              <finest>'Removing producer object from cache=' ~ OBJECT.object_id</finest>
//              <remove_object_from_cache>OBJECT.object_id</remove_object_from_cache>
//             </then>
//           </if>
//        </else>
//      </if>
//  </proc>
//";


//            //Console.WriteLine("GENERATED PRODUCER:");
//            //Console.WriteLine(producerProc);
//            string producerCsrSyntax = "OBJECT." + producerCursorField;
//            copyCsr = copyCsr.Replace("${CSR}", producerCsrSyntax);
//            producerProc = producerProc.Replace("${CSR}", producerCsrSyntax);
//            producerProc = producerProc.Replace("${CONSUMER}", this.consumerProcName);
//            producerProc = producerProc.Replace("${OBJECTTYPE}", this.objectType);
//            producerProc = producerProc.Replace("${OBJECTID}", this.objectId);
//            producerProc = producerProc.Replace("${BATCHSIZE}", this.batchSize);
//            producerProc = producerProc.Replace("${COPYCSR}", copyCsr);
//            producerProc = producerProc.Replace("${PRODUCER}", producerProcName);

//            //Console.WriteLine("GENERATED PRODUCER:");
//            //Console.WriteLine(producerProc);

//            // TBD: come up w/ a way to eventually destroy this procContext
//            // parse and call the generated procContext.
//            XmlElement procElem = ProcLoader.ParseInputXmlString(producerProc);
//            mc.ReadXmlProcFromElem(producerProcName, procElem);
//            int producerProcId = mc.GetProcId(producerProcName);

//            //mc.CallProcForCurrentObjectNested(producerProcId); 
//            mc.CallProcForObjectNoCallback(producerProcId, producerOid);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("produce:" + this.cursorSyntax);
        }
    }


}
