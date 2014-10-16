using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
namespace MVM
{
    /*
     * <cursor_top>
     * <cursor_inst_id><cursor_inst_id>
     * <cursor_top>
     */
    public class MCursorTop : IModuleSetup, IModuleRun
    {
        private string topSyntax;
        private IReadString topParsed;
        private CursorSetupCommon cursorSetup;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MCursorTop m = new MCursorTop();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            m.topSyntax = me.SelectNodeInnerText("./top");
            m.topParsed = mc.ParseSyntax(m.topSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            CursorTopCursor outputCursor = new CursorTopCursor(mc, this.cursorSetup, topParsed);
            mc.mvm.Log("created top output cursor " + outputCursor.CursorInstId);
        }
    }

    public class CursorTopCursor : CursorCommonOp
    {
        private IReadString topParsed;
        private long top = 1;
        private long ctr = 0;
        /// <summary>
        /// Sets up the cursor, including how rows should be sorted.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="orderedFieldNames"></param>
        /// <param name="cursorOid"></param>
        /// <param name="objectArrayComparer"></param>
        public CursorTopCursor(ModuleContext mc, CursorSetupCommon cursorSetup, IReadString topParsed)
            : base(mc, cursorSetup)
        {
            // this.inputCursorInstIdParsed = inputCursorInstIdParsed;
            logger.Info("CURSOR TOP HAS OUTPUT CURSOR INST ID=" + this.CursorInstId);
            this.topParsed = topParsed;
            this.top=long.Parse(this.topParsed.ReadOrDefault(mc, "1"));
        }

        public int nullInputObjCtr = 0;
        public override CursorStatus CursorNext(ModuleContext mc, IObjectData inputObj, out IObjectData outputObj)
        {
            CursorStatus csrStatus=CursorStatus.EOF;
            outputObj = null;
            // if the input object is not null, we need to buffer it
            if (inputObj != null)
            {
                
                if ((ctr + 1) <= top)
                {
                    ctr++;
                    outputObj = inputObj;
                    csrStatus = CursorStatus.HAS_ROW;
                }
                else
                {
                    csrStatus = CursorStatus.EOF;
                }

                // reset state to 0
                nullInputObjCtr = 0;
            }
            else if (++nullInputObjCtr == 1)
            {
                csrStatus = CursorStatus.PARENT_NEXT;
            }
            else
            {
                csrStatus = CursorStatus.EOF;
            }
            return csrStatus;

        }

        protected override void CursorClear(ModuleContext mc)
        {
        }


    }
}
