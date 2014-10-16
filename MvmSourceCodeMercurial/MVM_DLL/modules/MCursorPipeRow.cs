using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
namespace MVM
{

    /*
     * <pipe_row>OBJECT.some_cursor_field_that_is_an_object_id<pipe_row>
     */
    public class MCursorPipeRow : IModuleSetup, IModuleRun
    {
        private string pipeRowSyntax;
        private IReadString pipeRowParsed;
        private CursorSetupCommon cursorSetup;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MCursorPipeRow m = new MCursorPipeRow();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            m.pipeRowSyntax = me.SelectNodeInnerText("./pipe_row");
            m.pipeRowParsed = mc.ParseSyntax(m.pipeRowSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            CursorPipeRowCursor outputCursor = new CursorPipeRowCursor(mc, this.cursorSetup, pipeRowParsed);
            //mc.mvm.Log("created where output cursor " + outputCursor.CursorInstId);
        }
    }

    public class CursorPipeRowCursor : CursorCommonOp
    {
        private IReadString pipeRowParsed;

        /// <summary>
        /// Sets up the cursor, including how rows should be sorted.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="orderedFieldNames"></param>
        /// <param name="cursorOid"></param>
        /// <param name="objectArrayComparer"></param>
        public CursorPipeRowCursor(ModuleContext mc, CursorSetupCommon cursorSetup, IReadString pipeRowParsed)
            : base(mc, cursorSetup)
        {
            // this.inputCursorInstIdParsed = inputCursorInstIdParsed;
            //logger.Info("CURSOR WHERE HAS OUTPUT CURSOR INST ID=" + this.CursorInstId);
            this.pipeRowParsed = pipeRowParsed;
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
                
                // read the pipe oid
                string pipeRowOid = pipeRowParsed.Read(mc);
                using (IObjectData obj=mc.objectCache.CheckOut(pipeRowOid))
                {
                    outputObj = obj;
                    csrStatus = CursorStatus.HAS_ROW;
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
