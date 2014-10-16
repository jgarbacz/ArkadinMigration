using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;

namespace MVM
{
    /*
     * This:
     * 
     * 
     * <synchronized name='optional name'>
     *  <print>Only 1 at a time goes through here</print>
     *  <print>..1</print>
     *  <print>..2</print>
     *  <print>..3</print>
     * <synchronized> 
     * 
     * Turns into:
     * 
     * <start_sync name="[GeneratedMonitorName]"/>
     * <call_nested_proc>[GeneratedProcName]</call_nexted_proc>
     * <sync_end name="[GeneratedMonitorName]"/>
     * 
     * GeneratedProcName is for the insides of <synchronized>
     */
    class MSynchronized : IModuleSetup
    {
        public void Setup(System.Xml.XmlElement me, ModuleContext mc,List<IModuleRun> runModules)
        {
            string tagName = me.LocalName;
            //string childProcName = mc.procDefinition.initNamespaceProcName + "/" + tagName + "[" + mc.moduleOrder + "]";
            string childProcName = mc.GetChildProcName(tagName);
            mc.ReadXmlProcFromElem(childProcName, me);

            // allow user to specify a name for synchronizing across blocks
            
            string syncName;
            if (me.HasAttribute("name"))
            {
                syncName = "name=" + me.GetAttribute("name").q();
            }
            else if (me.HasAttribute("dynamic_name"))
            {
                syncName = "dynamic_name=" + me.GetAttribute("dynamic_name").q();
            }
            else
            {
                syncName = "name=" + childProcName.q();
            }
            string syncStart = "<sync_start "+syncName+"/>";
            string callProcNest = "<call_proc_for_current_object_nested><name>'" + childProcName + "'</name></call_proc_for_current_object_nested>";
            string syncEnd = "<sync_end " + syncName + "/>";

            SchedulerMaster sm = mc.scheduler.schedulerMaster;
            runModules.Add(sm.GetModuleRun(syncStart));
            runModules.Add(sm.GetModuleRun(callProcNest));
            runModules.Add(sm.GetModuleRun(syncEnd));
        }
    }
}
