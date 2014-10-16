using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;

/*
<inherit_cursor_default>
<cursor>TEMP.csr</cursor>
<object_id>TEMP.oid</object_id>
</index_select_default>
 */

namespace MVM
{
    class MInheritCursorDefault: IModuleSetup
    {
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            throw new Exception("inherit_cursor_default no longer supported");

            //// xml extraction
            //string cursorSyntax = me.SelectNodeInnerText("./cursor");
            //string objectSyntax = me.SelectNodeInnerText("./object_id");
            //string prefixSyntax = me.SelectNodeInnerText("./prefix");
            //string suffixSyntax = me.SelectNodeInnerText("./suffix");
            //string prefix = prefixSyntax==null?"":mc.SyntaxReadString(prefixSyntax);
            //string suffix = suffixSyntax == null ? "" : mc.SyntaxReadString(suffixSyntax);
            
            //string target = "OBJECT.";
            //if (objectSyntax != null) target = "OBJECT(" + objectSyntax + ").";
            //string source = "OBJECT(" + cursorSyntax + ").";
            //string csrOid = mc.ParseSyntax(cursorSyntax).Read(mc);
            //SchedulerMaster sm = mc.schedulerMaster;
            //ICursor cursor = (ICursor)mc.globalContext.GetNamedClassInst(csrOid);
            
            //foreach (string field in cursor.GetOrderedFieldNames())
            //{
            //    string targetField = target + prefix+field+suffix;
            //    string sourceField = source + field;
               
            //    string cp = "<do>" + targetField + "=" + targetField + " ne ''?" + targetField + ":" + sourceField + "</do>";
            //    //Console.WriteLine(cp);
            //    run.Add(sm.GetModuleRun(cp));
            //}
        }


    }
    }
