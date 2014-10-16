using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{

    /*
    
    <produce>
    <cursor>${CSR}<cursor>
    <producer_cursor_field>'cursor'</producer_cursor_field>
    <spawn>
        <object_type></object_type>
        <inherit_parent>1|0</inherit_parent>
    </spawn>
    <same_temp_scope>1|0</same_temp_scope>
    <copy_cursor_to_object>1|0</copy_cursor_to_object>
    <copy_cursor_to_temp>1|0</copy_cursor_to_temp> 
    <batch_size>1000<batch_size>                                            
    <consumer_proc_name>'processing'</consumer_proc_name> 
    <consumer_proc>
        <print>'yeah in line'</print>
    </consumer_proc>
    </produce>
     */
    class MProduce : IModuleSetup, IModuleRun
    {
       // private static long genCtr = 0;
        private string cursorSyntax;
        private string objectType;
        private string objectId;
        private string inheritFromParentSyntax;
        private IReadString inheritFromParentParsed;
        private string batchSize;
        private string consumerProcName;
        private string producerCursorFieldSyntax;
        private IReadString cursorParsed;
        private IReadString producerCursorFieldParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MProduce m = new MProduce();
            // xml extraction
            m.cursorSyntax = me.SelectNodeInnerText("./cursor");
            m.objectType = me.SelectNodeInnerText("./object_type");
            m.inheritFromParentSyntax = me.SelectNodeInnerText("./inherit_parent", "0");
            m.objectId = me.SelectNodeInnerText("./object_id");
            m.batchSize = me.SelectNodeInnerText("./batch_size","GLOBAL.produce_batch_size");
            m.consumerProcName = me.SelectNodeInnerText("./consumer_proc_name");
            m.producerCursorFieldSyntax = me.SelectNodeInnerText("./producer_cursor_field","'xxGenProducerCsrXX'");
            // parsing
            m.cursorParsed = mc.ParseSyntax(m.cursorSyntax);
            m.producerCursorFieldParsed = mc.ParseSyntax(m.producerCursorFieldSyntax);
            m.inheritFromParentParsed = mc.ParseSyntax(m.inheritFromParentSyntax);
            // runtime
            run.Add(m);
        }


        public static int producerCtr=1;
        public void Run(ModuleContext mc)
        {
            string inherit = this.inheritFromParentParsed.Read(mc);

            // build up the string to copy from the cursor to the object
            string csrOid = this.cursorParsed.Read(mc);
            ICursor cursor = (ICursor)mc.globalContext.GetNamedClassInst(csrOid);

            // Get the procNameSyntax to the caller
            string callback = mc.workMgr.CreateCallback(mc.procInst).ToString();

            // get the name of the object field on the producer that has the cursor
            string producerCursorField = this.producerCursorFieldParsed.Read(mc);

            // spawn a producer object
            string producerOid = mc.Spawn("PRODUCER");
            if(inherit.Equals("1")){
                // copy parent fields to producer which eventually get onto the produced records.
                mc.InheritObject(mc.objectData.objectId, producerOid);
            }
            mc.WriteObjectField(producerOid, producerCursorField, csrOid);
            mc.WriteObjectField(producerOid, "my_producer_ctr", "1"); // at start of procContext this is number of producer, self included  
            mc.WriteObjectField(producerOid, "callback", callback);
            mc.WriteObjectField(producerOid, "my_work_ctr", "0");

            List<string> copyFields = new List<string>();
            if(1+1==2)throw new Exception("Rob port this to not use code gen");
            //foreach (string field in cursor.GetOrderedFieldNames())
            //{
            //    copyFields.Add("<do>OBJECT(TEMP.oid)." + field + "=OBJECT(${CSR})." + field + "</do>");
            //}
//            string copyCsr = copyFields.Join("\r\n");

//            // generate a name for the producer procContext
//            string producerProcName = "GENERATED_PRODUCER_" + (genCtr++).ToString();

//            // create a consumer procContext that calls the consumer procContext, and then cleans up the cluster.
//            string internalConsumerProc = producerProcName + "_internal_consumer";
//            string internalConsumer = 
//                "<proc name='" + internalConsumerProc + "'>"+
//                "<call_proc_for_current_object>"+
//                "<name>" + this.consumerProcName + "</name>"+
//                "</call_proc_for_current_object>"+
//                "<remove_cluster_from_cache/>" +
//                "</proc>";
//            XmlElement internalConsumerProcElem = ProcLoader.ParseInputXmlString(internalConsumer);
//            mc.ReadXmlProcFromElem(internalConsumerProc, internalConsumerProcElem);
//            //Console.WriteLine("GENERATED INTERNAL CONSUMER:");
//            //Console.WriteLine(internalConsumer);

//            // create a procContext that fires when each procInst is complete
//            string ctrCallbackProcName = producerProcName + "_WorkDoneCb";
//            string ctrCallback =
//              @"<proc name='"+ctrCallbackProcName+"'>" +
//              @"<synchronized name='" + ctrCallbackProcName + "'>" +
//              @"<do>OBJECT.my_work_ctr-=1</do>" +
//              @"<debug>'FINISHED: my_work_ctr='~OBJECT.my_work_ctr</debug>" +
//              @"</synchronized>" +
//              @"</proc>";
//            mc.ReadXmlConfigFromString(ctrCallback);

//            // build up the procContext string
//            string producerProc =
//@"
// <proc name='${PRODUCER}'>
//    <synchronized name='" + ctrCallbackProcName + @"'>
//      <debug>'[${PRODUCER}] BEGIN object_id='~OBJECT.object_id~', csr_oid='~${CSR}~', CSR.eof='~OBJECT(${CSR}).eof~', my_work_ctr='~OBJECT.my_work_ctr~', my_producer_ctr='~OBJECT.my_producer_ctr</debug>
//      <do>OBJECT.my_producer_ctr-=1</do>
//      <!-- if no ppl doing work && cursor empty && last producer, do callback to audit point -->
//      <if>
//        <condition>(OBJECT.my_work_ctr eq 0) and (OBJECT.my_producer_ctr eq 0) and (OBJECT(${CSR}).eof)</condition>
//        <then>
//           <debug>'[${PRODUCER}] Parallel work complete with ${PRODUCER}, my_work_ctr='~OBJECT.my_work_ctr~' CSR.eof='~OBJECT(${CSR}).eof~', my_producer_ctr='~OBJECT.my_producer_ctr</debug>
//           <callback>OBJECT.callback</callback>
//           <debug>'[${PRODUCER}] Removing producer object from cache=' ~ OBJECT.object_id</debug>
//           <remove_object_from_cache>OBJECT.object_id</remove_object_from_cache>
//        </then>
//      </if>
//      <!-- if ppl doing work || cursor not empty, need to self queue -->
//      <if>
//        <condition>(OBJECT.my_work_ctr GT 0) || (!OBJECT(${CSR}).eof)</condition>
//        <then>
//          <if>
//            <condition>OBJECT(${CSR}).eof</condition>
//            <then>
//              <sleep>1000</sleep>
//            </then>
//          </if>
//          <do>OBJECT.my_producer_ctr+=1</do>
//          <debug>'[${PRODUCER}] Self queuing producer ot=${PRODUCER} oid='~OBJECT.object_id</debug>
//          <queue_proc_for_current_object>
//            <name>'${PRODUCER}'</name>
//          </queue_proc_for_current_object>
//        </then>
//      </if>
//      <!-- if cursor not empty, produce a batch of work -->
//      <if>
//        <condition>!OBJECT(${CSR}).eof</condition>
//        <then>
//          <do>TEMP.batch_ctr=${BATCHSIZE}</do>
//          <while>
//            <condition>(!OBJECT(${CSR}).eof) and (TEMP.batch_ctr GT 0)</condition>
//            <loop>
//              <do>OBJECT.my_work_ctr+=1</do>
//              <debug>'[${PRODUCER}] produce my_work_ctr='~OBJECT.my_work_ctr</debug>
//              <do>TEMP.batch_ctr-=1</do>
//              <do>TEMP.oid=${OBJECTID}</do>
//              <create_cluster>TEMP.cluster_oid</create_cluster>
//              <spawn>
//                <object_type>${OBJECTTYPE}</object_type>
//                <object_id>TEMP.oid</object_id>
//                <cluster_object_id>TEMP.cluster_oid</cluster_object_id>
//                <inherit_parent>${INHERIT}</inherit_parent>
//              </spawn>
//              <debug>'[${PRODUCER}] spawned objectType='~${OBJECTTYPE}~',oid='~TEMP.oid</debug>
//${COPYCSR}
//              <queue_proc_for_object_in_cluster>
//                <name>${CONSUMER}</name>
//                <object_id>TEMP.oid</object_id>
//                <cluster_object_id>TEMP.cluster_oid</cluster_object_id>
//                <callback_proc_name>" + ctrCallbackProcName + @"</callback_proc_name>
//                <callback_object_id>OBJECT.object_id</callback_object_id>
//              </queue_proc_for_object_in_cluster>
//              <cursor_next>${CSR}</cursor_next>
//            </loop>
//          </while>
//        </then>
//      </if>
//      <debug>'[${PRODUCER}] END object_id='~OBJECT.object_id~', csr_oid='~${CSR}~', CSR.eof='~OBJECT(${CSR}).eof~', my_work_ctr='~OBJECT.my_work_ctr~', my_producer_ctr='~OBJECT.my_producer_ctr</debug>
//    </synchronized>
//  </proc>
//";
//            //Console.WriteLine("GENERATED PRODUCER:");
//            //Console.WriteLine(producerProc);
//            string producerCsrSyntax = "OBJECT." + producerCursorField;
//            copyCsr = copyCsr.Replace("${CSR}", producerCsrSyntax);
//            producerProc = producerProc.Replace("${CSR}", producerCsrSyntax);
//            producerProc = producerProc.Replace("${CONSUMER}", internalConsumerProc.q());
//            producerProc = producerProc.Replace("${OBJECTTYPE}", this.objectType);
//            producerProc = producerProc.Replace("${OBJECTID}", this.objectId);
//            producerProc = producerProc.Replace("${BATCHSIZE}", this.batchSize);
//            producerProc = producerProc.Replace("${COPYCSR}", copyCsr);
//            producerProc = producerProc.Replace("${PRODUCER}", producerProcName);
//            producerProc = producerProc.Replace("${INHERIT}", inherit);

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
