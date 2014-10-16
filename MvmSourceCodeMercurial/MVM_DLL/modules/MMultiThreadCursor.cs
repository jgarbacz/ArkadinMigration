using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MVM
{

    /*

<multi_thread_cursor>
	<cursor>TEMP.csr</cursor>
	<cursor_temp_alias>TEMP.csr</cursor_temp_alias>
	<batch_size>10</batch_size>
	<same_temp_scope>1</same_temp_scope>
	<is_asynchronous>0</is_asynchronous>
	<run>
		<copy_cursor_to_temp>TEMP.csr</copy_cursor_to_temp>
		... inline whatever ...
	</run>
</multi_thread_cursor>
     
*/

    class MMultiThreadCursor : IModuleSetup
    {
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            // parse the xml
            string cursorSyntax = me.SelectNodeInnerText("./cursor");
            string cursorTempObjectSyntax = me.SelectNodeInnerText("./cursor_temp_object", "TEMP."+mc.GetGenSym("csr_tmp_obj"));
            string sameTempScopeSyntax = me.SelectNodeInnerText("./same_temp_scope", "1");
            string batchSizeSyntax = me.SelectNodeInnerText("./batch_size", "GLOBAL.produce_batch_size");

            // generate some names
            string producerProcName = mc.GetGenSym("GENERATED_PRODUCER"); ;
            string outerConsumerProcName = producerProcName + "_OuterConsumer";
            string innerConsumerProcName = producerProcName + "_InnerConsumer";
            string tempMyWorkCtr = "TEMP." + mc.GetGenSym("my_work_ctr");
            string tempProducerOid = "TEMP." + mc.GetGenSym("producer_oid");
            string tempSnapMyWorkCtr = "TEMP." + mc.GetGenSym("snap_my_work_ctr");
            //string tempClusterOid = "TEMP." + mc.GetGenSym("cluster_oid");
            
            // define the outer consumer procContext whose job is to call inner consumer code the user specified
            // and then cleanup the tempContext cursor object.
            string outerConsumer =
               "<proc name='" + outerConsumerProcName + "'>" +
               "<call_proc_for_current_object_nested>" +
               "<name>" + innerConsumerProcName + "</name>" +
               "</call_proc_for_current_object_nested>" +
               "<finest>'[" + outerConsumerProcName + "-'~OBJECT.object_id~'] try clearing cursor [" + cursorTempObjectSyntax + "] with oid='~" + cursorTempObjectSyntax + "~', ot='~OBJECT(" + cursorTempObjectSyntax + ").object_type</finest>" +
               "<cursor_clear>" + cursorTempObjectSyntax + "</cursor_clear>" +
               "</proc>";
            mc.ReadXmlConfigFromString(outerConsumer);

            // define the inner consumer from the passed user config to run.
            XmlElement innerConsumer = me.SelectSingleElem("./run");
            mc.ReadXmlProcFromElem(innerConsumerProcName, innerConsumer);

            // create a procContext that fires when each procInst is complete
            string ctrCallbackProcName = producerProcName + "_MyWorkCtr";
            string ctrCallback =
              @"<proc name='" + ctrCallbackProcName + "'>" +
              @"<atomic_decrement>" +
              @"<object_id>OBJECT.object_id</object_id>" +
              @"<field_name>'my_work_ctr'</field_name>" +
              @"<output>"+tempMyWorkCtr+"</output>" +
              @"</atomic_decrement>" +
              @"<finest>'FINISHED: oid='~OBJECT.object_id~', my_work_ctr='~"+tempMyWorkCtr+"</finest>" +
              @"</proc>";
            mc.ReadXmlConfigFromString(ctrCallback);

            // Define the producer procContext whose job is to spawn a tempContext cursor object and queue the internal
            // consumers up in batches. 
            string producerProc =
@"
 <proc name='${PRODUCER}'>
    <synchronized dynamic_name='OBJECT.sync_name'>
      <do>"+tempSnapMyWorkCtr+@"=OBJECT.my_work_ctr</do> <!-- here we grab a consistent value -->
      <finest>'[${PRODUCER}-'~OBJECT.object_id~'] BEGIN object_id='~OBJECT.object_id~', csr_oid='~${CSR}~', my_work_ctr='~" + tempSnapMyWorkCtr + @"~', my_producer_ctr='~OBJECT.my_producer_ctr</finest>
      <do>OBJECT.my_producer_ctr-=1</do>
      <!-- if no ppl doing work && cursor empty && last producer, do callback to audit point -->
      <if>
        <condition>("+tempSnapMyWorkCtr+@" eq 0) and (OBJECT.my_producer_ctr eq 0) and (OBJECT(${CSR}).eof)</condition>
        <then>
           <finest>'[${PRODUCER}-'~OBJECT.object_id~'] Parallel work complete with ${PRODUCER}, my_work_ctr='~"+tempSnapMyWorkCtr+@"~' CSR.eof='~OBJECT(${CSR}).eof~', my_producer_ctr='~OBJECT.my_producer_ctr~', csr_oid='~${CSR}</finest>
            <finest>'[${PRODUCER}-'~OBJECT.object_id~'] Set callback flag to callback='~OBJECT.callback~' to outside of parallel'</finest>      
           <do>OBJECT.run_callback=1</do>
           
           <finest>'[${PRODUCER}-'~OBJECT.object_id~'] END'</finest>
        </then>
        <else>
          <!-- if ppl doing work || cursor not empty, need to self queue -->
          <if>
            <condition>("+tempSnapMyWorkCtr+@" GT 0) || (!OBJECT(${CSR}).eof)</condition>
            <then>
              <if>
                <condition>OBJECT(${CSR}).eof</condition>
                <then>
                  <sleep>1000</sleep>
                </then>
              </if>
              <do>OBJECT.my_producer_ctr+=1</do>
              <finest>'[${PRODUCER}-'~OBJECT.object_id~'] Self queuing producer ot=${PRODUCER} oid='~OBJECT.object_id</finest>
              <queue_proc_for_current_object_nested_on_stack>
                <name>'${PRODUCER}'</name>
              </queue_proc_for_current_object_nested_on_stack>
            </then>
          </if>
          <!-- if cursor not empty, produce a batch of work -->
          <if>
            <condition>!OBJECT(${CSR}).eof</condition>
            <then>
              <do>OBJECT.batch_ctr=OBJECT.batch_size</do>
              <while>
                <condition>(!OBJECT(${CSR}).eof) and (OBJECT.batch_ctr GT 0)</condition>
                <loop>
                  <atomic_increment>
                    <object_id>OBJECT.object_id</object_id>
                    <field_name>'my_work_ctr'</field_name>
                    <output>${TEMP_MY_WORK_CTR}</output>
                  </atomic_increment>
                  <finest>'[${PRODUCER}-'~OBJECT.object_id~'] produce my_work_ctr='~${TEMP_MY_WORK_CTR}</finest>
                  <do>OBJECT.batch_ctr-=1</do>
                  <!-- create_cluster>${TEMP_CLUSTER_OID}</create_cluster -->
                  <sterilize_cursor>
                    <input>${CSR}</input>
                    <output>${TEMP_CURSOR_OBJECT}</output>
                  </sterilize_cursor>
                  <finest>'[${PRODUCER}-'~OBJECT.object_id~'] created sterile cursor [${TEMP_CURSOR_OBJECT}] oid='~${TEMP_CURSOR_OBJECT}~', ot='~OBJECT(${TEMP_CURSOR_OBJECT}).object_type</finest>
                  <queue_proc_for_object_nested>
                    <name>'${CONSUMER}'</name>
                    <object_id>OBJECT.caller_oid</object_id>
                    <callback_proc_name>'${COUNTER_CALLBACK}'</callback_proc_name>
                    <callback_object_id>OBJECT.object_id</callback_object_id>
                  </queue_proc_for_object_nested>
                  <cursor_next>${CSR}</cursor_next>
                </loop>
              </while>
            </then>
          </if>
          <finest>'[${PRODUCER}-'~OBJECT.object_id~'] END object_id='~OBJECT.object_id~', csr_oid='~${CSR}~', my_work_ctr='~"+tempSnapMyWorkCtr+@"~', my_producer_ctr='~OBJECT.my_producer_ctr</finest>
        </else>
      </if>
      <finest>'[${PRODUCER}-'~OBJECT.object_id~'] about to exit syncend'</finest>
    </synchronized>
    <finest>'[${PRODUCER}-'~OBJECT.object_id~'] exited syncend'</finest>
    <if>
      <condition>OBJECT.run_callback</condition>
      <then>
        <finest>'[${PRODUCER}-'~OBJECT.object_id~'] Firing callback='~OBJECT.callback~' to outside of parallel'</finest>
        <callback>OBJECT.callback</callback>  
      </then>
    </if>
          
  </proc>
";
            producerProc = producerProc.Replace("${CSR}", "OBJECT.cursor");
            producerProc = producerProc.Replace("${TEMP_CURSOR_OBJECT}", cursorTempObjectSyntax);
            producerProc = producerProc.Replace("${CONSUMER}", outerConsumerProcName);
            producerProc = producerProc.Replace("${PRODUCER}", producerProcName);
            producerProc = producerProc.Replace("${COUNTER_CALLBACK}", ctrCallbackProcName);
            producerProc = producerProc.Replace("${TEMP_MY_WORK_CTR}", tempMyWorkCtr);
            //producerProc = producerProc.Replace("${TEMP_CLUSTER_OID}", tempClusterOid);
            mc.ReadXmlConfigFromString(producerProc);

            // emit config that spawns a producer object and calls the producer procContext for it.
            string runtimeModules =
                "<proc name='fake'>" +
                "<block>" +
                    "<finest>'[PRIMARY-" + producerProcName + "-to_spawn] begin PRODUCER block'</finest>" +
                    "<spawn>" +
                    "<object_type>'PRODUCER'</object_type>" +
                    "<object_id>"+tempProducerOid+"</object_id>" +
                    "<no_cluster>1</no_cluster>" +
                    "</spawn>" +
                    "<get_guid>OBJECT(" + tempProducerOid + ").sync_name</get_guid>" +
                    "<do>OBJECT(" + tempProducerOid + ").caller_oid=OBJECT.object_id</do>" +
                    "<do>OBJECT(" + tempProducerOid + ").cursor=" + cursorSyntax + "</do>" +
                    "<do>OBJECT(" + tempProducerOid + ").my_producer_ctr=1</do>" +
                    "<do>OBJECT(" + tempProducerOid + ").my_work_ctr=0</do>" +
                    "<do>OBJECT(" + tempProducerOid + ").batch_size=" + batchSizeSyntax + "</do>" +
                    "<finest>'[PRIMARY-" + producerProcName + "-'~" + tempProducerOid + "~'] created producer object having oid='~" + tempProducerOid + "~', sync_name='~OBJECT(" + tempProducerOid + ").sync_name~', batch_size='~OBJECT(" + tempProducerOid + ").batch_size</finest>" +
                    "<callback_create_with_scope_pop>OBJECT(" + tempProducerOid + ").callback</callback_create_with_scope_pop>" +
                    "<finest>'[PRIMARY-" + producerProcName + "-'~" + tempProducerOid + "~'] created callback on producer object with callbackid='~OBJECT(" + tempProducerOid + ").callback</finest>" +
                    "<call_proc_for_object_nested_no_callback_on_stack>" +
                    "<name>" + producerProcName + "</name>" +
                    "<object_id>" + tempProducerOid + "</object_id>" +
                    "</call_proc_for_object_nested_no_callback_on_stack>" +
                    "<finest>'[PRIMARY-" + producerProcName + "-'~" + tempProducerOid + "~'] Removing producer from cache oid=' ~ " + tempProducerOid + "</finest>" +
                    "<remove_object_from_cache>" + tempProducerOid + "</remove_object_from_cache>" +
                "</block>" +
                "</proc>";
            run.AddRange(mc.ReadXmlModules(runtimeModules));
        }


        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("parallel process cursor:");
        }
    }


}
