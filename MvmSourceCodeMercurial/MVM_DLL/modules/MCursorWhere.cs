using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
namespace MVM
{

    /*
     * <cursor_where>
     * <cursor_inst_id><cursor_inst_id>
     * <cursor_where>
     */
    public class MCursorWhere : IModuleSetup, IModuleRun
    {
        private string whereSyntax;
        private IReadString whereParsed;
        private CursorSetupCommon cursorSetup;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MCursorWhere m = new MCursorWhere();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            m.whereSyntax = me.SelectNodeInnerText("./where");
            m.whereParsed = mc.ParseSyntax(m.whereSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            CursorWhereCursor outputCursor = new CursorWhereCursor(mc, this.cursorSetup, whereParsed);
            //mc.mvm.Log("created where output cursor " + outputCursor.CursorInstId);
        }
    }

    public class CursorWhereCursor : CursorCommonOp
    {
        private IReadString whereParsed;

        /// <summary>
        /// Sets up the cursor, including how rows should be sorted.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="orderedFieldNames"></param>
        /// <param name="cursorOid"></param>
        /// <param name="objectArrayComparer"></param>
        public CursorWhereCursor(ModuleContext mc, CursorSetupCommon cursorSetup, IReadString whereParsed)
            : base(mc, cursorSetup)
        {
            // this.inputCursorInstIdParsed = inputCursorInstIdParsed;
            //logger.Info("CURSOR WHERE HAS OUTPUT CURSOR INST ID=" + this.CursorInstId);
            this.whereParsed = whereParsed;
        }

        public int nullInputObjCtr = 0;
        public override CursorStatus CursorNext(ModuleContext mc, IObjectData inputObj, out IObjectData outputObj)
        {
            outputObj = null;
            CursorStatus csrStatus = CursorStatus.EOF;

            // if the input object is not null
            if (inputObj != null)
            {
                // temporarily make the current object the input object
                var currentObject = mc.objectData;
                mc.objectData = inputObj;

                // evaluate the boolean condition
                string whereBool = whereParsed.Read(mc);
                if (whereBool.Equals("1"))
                {
                    outputObj = inputObj;
                    csrStatus = CursorStatus.HAS_ROW;
                }
                else
                {
                    // no longer needed, garbage collection will deal with it.
                    //inputObj.RefRemove();
                    csrStatus = CursorStatus.PARENT_NEXT;
                }

                // restore the current object
                mc.objectData = currentObject;

                // reset state to 0
                nullInputObjCtr = 0;
            }
            // First time it is given null, request from parent.
            else if (++nullInputObjCtr == 1)
            {
                //logger.Info("*** WHERE CALLED WITH NULL case 1");
                csrStatus = CursorStatus.PARENT_NEXT;
            }
            // Second time it is given null, return EOF
            else
            {
                //logger.Info("*** WHERE CALLED WITH NULL case 2");
                csrStatus = CursorStatus.EOF;
            }
            return csrStatus;
        }

        protected override void CursorClear(ModuleContext mc)
        {
        }
    }
}
