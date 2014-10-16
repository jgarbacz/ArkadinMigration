using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Antlr.Runtime.Tree;
namespace MVM
{
    /*
     * Auto tune out a single input cursor.
    <union_all_select>
    <input_cursor_inst_id/>
    <*select/>
    </union_all_select>
   */
    class MUnionAllSelect : IModuleSetup, IModuleRun
    {
        public List<string> cursorInstIdsSyntax = new List<string>();
        public List<IReadString> cursorInstIdsParsed;

        private CursorSetupCommon cursorSetup;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            // get all the input cursors.
            List<XmlElement> inputCursorElems = new List<XmlElement>();
            List<XmlElement> otherCursorElems = new List<XmlElement>();
            foreach (var elem in me.GetChildElems())
            {
                string elemName = elem.LocalName;
                if (elemName.Equals("input_cursor_inst_id") || elemName.EndsWith("select"))
                {
                    inputCursorElems.Add(elem);
                }else{
                    otherCursorElems.Add(elem);
                }
            }

            // Tune for only one element and skip doing the union
            if(inputCursorElems.Count==1){
                var onlyInput=inputCursorElems[0];
                foreach(var elem in otherCursorElems){
                    onlyInput.AppendChildElement(elem);
                }
                 run.Add(mc.GetModuleRun(onlyInput));
                 return;
            }
        
            // if we get here we need to do the union all
            MUnionAllSelect m = new MUnionAllSelect();
            m.cursorSetup = new CursorSetupCommon(me, mc);

            foreach (var elem in inputCursorElems)
            {
                string elemName = elem.LocalName;
                if (elemName.EndsWith("select"))
                {
                    // add generated cursor instance id to the mix
                    string cursorInst = "TEMP." + mc.GetGenSym(elemName + "_is_unioned");
                    elem.AppendTextElement("cursor_inst_id", cursorInst);
                    m.cursorInstIdsSyntax.Add(cursorInst);
                    run.Add(mc.GetModuleRun(elem));
                }
                else if (elemName.EndsWith("input_cursor_inst_id"))
                {
                    // add the passed cursor instance id to the mix
                    string cursorInst = elem.InnerText;
                    m.cursorInstIdsSyntax.Add(cursorInst);
                }
            }
            // parse all the input cursors
            m.cursorInstIdsParsed = mc.ParseSyntax(m.cursorInstIdsSyntax);
           
            // runtime
            run.Add(m);
            m.cursorSetup.AddCursorSubProcs(me, mc, run);
        }
        public void Run(ModuleContext mc)
        {
            // get input cursors
            List<ICursor> cursors = new List<ICursor>();
            foreach (var cursorInstIdParsed in this.cursorInstIdsParsed)
            {
                string cursorInstId = cursorInstIdParsed.Read(mc);
                ICursorBase cursorBase;
                mc.LookupCursorViaInstId(cursorInstId, out cursorBase);
                //cursorBase.DeleteObjectOnNext = false;
                cursors.Add(cursorBase as ICursor);
            }
            var cursor = new CursorUnionAll(mc, this.cursorSetup, cursors);
        }
    }
}
