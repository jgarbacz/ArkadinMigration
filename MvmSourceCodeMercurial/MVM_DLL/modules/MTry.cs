using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    /*
 
<try>
	<config>
		<do>TEMP.x=1/0</do>	
	</config>
	<catch exception='DivideByZero'>
		<print>'catch1'</print>
	</catch>
	<catch exception='*'>
		<print>'catch2'</print>
	</catch>
	<finally>
		<print>'finallyblock'</print>
	</finally>
</try>
     */

    class MTry : IModuleSetup
    {
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MTry m = new MTry();
            string outerProcName = null;
            string innerProcName = null;
            foreach (XmlElement elem in me.SelectElements("./config"))
            {
                outerProcName = mc.GetChildProcName("try_config");
                string procConfig = @"
<proc name='" + outerProcName + @"' is_entry='false'>
" + elem.InnerXml + @"
</proc>
";
                XmlElement procElem = MyXml.ParseXmlString(procConfig).DocumentElement;
                mc.ReadXmlProcFromElem(outerProcName, procElem);
                innerProcName = outerProcName;
            }

            int catchNo = 1;
            foreach (XmlElement elem in me.SelectElements("./catch"))
            {
                string catchName = elem.GetAttribute("name");
                outerProcName = mc.GetChildProcName("try_catch" + (catchNo++));
                string procConfig = @"
<proc name='" + outerProcName + @"' is_entry='false'>
    <do>TEMP.exception_name=''</do>
    <do>TEMP.exception_message=''</do>
    <do>TEMP.exception_trace=''</do>
    <block work_type='catch'>
        <call_proc_for_current_object_nested>
        <name>" + innerProcName.q()+ @"</name>
        </call_proc_for_current_object_nested>
        <get_exception_info>
        <catch_name>'"+catchName+@"'</catch_name>
        <exception_name>TEMP.exception_name</exception_name>
        <exception_message>TEMP.exception_message</exception_message>
        <exception_trace>TEMP.exception_trace</exception_trace>
        </get_exception_info>
    </block>
    <if>
        <condition>TEMP.exception_name ne ''</condition>
        <then>
" + elem.InnerXml+ @"
        </then>
    </if>
</proc>
";
                XmlElement procElem = MyXml.ParseXmlString(procConfig).DocumentElement;
                mc.ReadXmlProcFromElem(outerProcName, procElem);
                innerProcName = outerProcName;
            }
            foreach (XmlElement elem in me.SelectElements("./finally"))
            {
                outerProcName = mc.GetChildProcName("try_finally");
                string procConfig = @"
<proc name='" + outerProcName + @"' work_type='finally' is_entry='false'>
    <call_proc_for_current_object_nested>
    <name>" + innerProcName.q() + @"</name>
    </call_proc_for_current_object_nested>
    <block work_type='standard'>
" + elem.InnerXml + @"
    </block>
</proc>
";
                XmlElement procElem = MyXml.ParseXmlString(procConfig).DocumentElement;
                mc.ReadXmlProcFromElem(outerProcName, procElem);
                innerProcName = outerProcName;
            }
            
            // OLD WAY WHICH CAUSE BUG BECAUSE WE DID NOT SNAP UNSNAP THE SCOPE.
            //m.procId = mc.GetProcId(childProcName);
            //run.Add(m);

            // NEW WAY WHICH DoeS THE SNAP UNSNAP
            run.Add(mc.GetModuleRun("<call_proc_for_current_object_nested><name>'" + innerProcName + "'</name></call_proc_for_current_object_nested>"));
        }
    }
}
