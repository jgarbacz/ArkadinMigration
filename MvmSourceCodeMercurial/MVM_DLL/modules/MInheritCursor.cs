using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

/*
<inherit_cursor>
<cursor>TEMP.csr</cursor>
<object_id>TEMP.oid</object_id>
</index_select>
 */

namespace MVM
{
    class MInheritCursor: IModuleSetup
    {
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            throw new Exception("inherit cursor no long supported, inherit object instead");
            //// xml extraction
            //string cursorSyntax = me.SelectNodeInnerText("./cursor");
            //string objectSyntax = me.SelectNodeInnerText("./object_id");
            //string target = "OBJECT.";
            //if (objectSyntax != null) target = "OBJECT(" + objectSyntax + ").";
            //string source = "OBJECT(" + cursorSyntax + ").";
            //string csrOid = mc.ParseSyntax(cursorSyntax).Read(mc);
            //SchedulerMaster sm = mc.schedulerMaster;
            //ICursor cursor = (ICursor)mc.globalContext.GetNamedClassInst(csrOid);
            //foreach (string field in cursor.GetOrderedFieldNames())
            //{
            //    string cp = "<do>" + target + field + "=" + source + field + "</do>";
            //    run.Add(sm.GetModuleRun(cp));
            //}
        }

      
    }
    }
