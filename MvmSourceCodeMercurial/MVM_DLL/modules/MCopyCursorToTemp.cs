using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

/*

 <copy_cursor_to_temp>
 <cursor>TEMP.cursor</cursor>
 <prefix>'pre_'</prefix>
 <suffix>'_sfx'</suffix>
 </copy_cursor_to_temp>
 
 */

namespace MVM
{
    class MCopyCursorToTemp: IModuleSetup
    {
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            throw new Exception("copy_cursor_to_temp no longer supported. now just inherit the cursor object to the temp scope");
            //// xml extraction
            //string cursorSyntax = me.SelectNodeInnerText("./cursor");
            //string prefixSyntax = me.SelectNodeInnerText("./prefix","''");
            //string suffixSyntax = me.SelectNodeInnerText("./suffix", "''");
            //string prefix = mc.SyntaxReadString(prefixSyntax);
            //string suffix = mc.SyntaxReadString(suffixSyntax);
            //string target = "TEMP.";
            //string source = "OBJECT(" + cursorSyntax + ").";
            //string csrOid = mc.ParseSyntax(cursorSyntax).Read(mc);
            //SchedulerMaster sm = mc.schedulerMaster;
            //ICursor cursor = (ICursor)mc.globalContext.GetNamedClassInst(csrOid);
            //foreach (string field in cursor.GetOrderedFieldNames())
            //{
            //    string sourceField = source + field;
            //    string targetField =target + prefix+ field+suffix; 
            //    string cp = "<do>" + targetField + "=" + sourceField + "</do>";
            //    run.Add(sm.GetModuleRun(cp));
            //}
        }
    }
        }
