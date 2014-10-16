using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
namespace MVM
{

    /*
     * 
     * <cursor_execute>
     *  <cursor_inst_id>TEMP.passed_in</cursor_inst_id>
     *  <execute pass_thru='true' parallel='true'>
     *      <do>'whatever'</do>
     *      <print>'whatever2'~OBJECT.junk</print>
     *      <pipe_row>TEMP.some_obj</pipe_row>
     *  </execute>
     * <cursor_execute>
     * 
     * generate a producer proc like this:
     * 
     * <proc name='generated_producer'>
     *  <param name='pipe_cursor'/>
     *  <param name='object_id'/>
     *  <do>'whatever'</do>
     *  <print>'whatever2'~OBJECT.junk</print>
     *  <pipe_row>TEMP.some_obj</pipe_row> 
     * </proc>
     * 
     * - with every *new* input object
     *      - instanciate the generated_producer
     *      - open a pipe_cursor to the generated_producer
     *      - pulse the pipe_cursor until EOF
     *      - if(passThru) pipe out the input object.
     * 
     */
    public class MCursorExecute : IModuleSetup, IModuleRun
    {
        public XmlElement executeElem;
        private CursorSetupCommon cursorSetup;
        private CursorSetupCommon pipeCursorSetup;
        private string genProducerProcName;
        private string prodProc;
        private int producerProcId;

        public bool pipeRow;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MCursorExecute m = new MCursorExecute();
            m.cursorSetup = new CursorSetupCommon(me, mc);

            // need to generate a producer proc
            m.executeElem = me.SelectSingleElem("./execute");
            m.pipeRow = m.executeElem.GetAttribute("pipe_row").In("", "true");
            m.genProducerProcName=mc.GetGenSym("pl_exec");
            m.prodProc =
            @"<proc name=" + m.genProducerProcName.q()+@">
                  <param name='pipe_cursor'/>
                  <param name='object_id'/>
                " + m.executeElem.InnerXml + 
            @"</proc>";
            mc.ReadXmlConfigFromString(m.prodProc);
            m.producerProcId=mc.GetProcId(m.genProducerProcName);

            // define a standalone cursorSetupCommon for the pipeCursor
            m.pipeCursorSetup = new CursorSetupCommon(me.CreateElement("blah"), mc);

            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            CursorExecuteCursor outputCursor = new CursorExecuteCursor(mc, this.cursorSetup, this.pipeCursorSetup,this.producerProcId,this.pipeRow);
            //mc.mvm.Log("created execute output cursor " + outputCursor.CursorInstId);
        }
    }

    public class CursorExecuteCursor : CursorCommonOp
    {
        public CursorExecuteCursor(ModuleContext mc, CursorSetupCommon cursorSetup, CursorSetupCommon pipeCursorSetup, int producerProcId, bool passThru)
            : base(mc, cursorSetup)
        {
            this.pipeCursorSetup = pipeCursorSetup;
            this.producerProcId = producerProcId;
            this.passThru = passThru;
            //logger.Info("CURSOR EXECUTE HAS OUTPUT CURSOR INST ID=" + this.CursorInstId);
        }

        private CursorSetupCommon pipeCursorSetup;
        private int producerProcId;
        public enum State { SETUP_PROC, PULSE_PROC, PROC_AT_EOF, INVALID }
        public State nextState = State.SETUP_PROC;
        public int nullInputCtr;
        public PipeCursor pipeCursor;
        public bool passThru;

        // we keep the last input object because until pipeCursor hits eof
        // the last input is still good.
        public IObjectData lastInputObject;

        public override CursorStatus CursorNext(ModuleContext mc, IObjectData inputObj, out IObjectData outputObj)
        {
            outputObj = null;
            CursorStatus csrStatus = CursorStatus.EOF;


            // if the input object is null, try to use the last input object 
            if (inputObj == null && lastInputObject != null)
            {
                inputObj = lastInputObject;
               // logger.Info("LAST INPUT OBJECT IS STILL VALID:" + inputObj.objectId);
            }
            lastInputObject = inputObj;

            // if the input object is not null
            if (inputObj != null)
            {
                // temporarily make the current object the input object
                var currentObject = mc.objectData;
                mc.objectData = inputObj;

            NEXT_STATE:
                switch (nextState)
                {
                    case State.SETUP_PROC:
                        {
                            // instanciate the producer proc and open a pipe cursor to it.
                            ProcInst newWork = mc.GetProcToProcId(producerProcId, inputObj.objectId);
                            newWork.isSync = mc.procInst.isSync;
                            //newWork.objectId = inputObj.objectId;
                            newWork.procContext = new ProcContext();
                            newWork.procContext.tempContext["object_id"] = currentObject.objectId;
                            newWork.callbackId = mc.workMgr.CreateCallback(mc.procInst);
                            long producerProcInstId = mc.workMgr.CreateCallback(newWork);
                            // instanciate the pipe cursor, then add it to the new work's parameters
                            pipeCursor = new PipeCursor(mc, this.pipeCursorSetup, producerProcInstId);
                            newWork.procContext.tempContext["pipe_cursor"] = pipeCursor.CursorInstId;

                            // pulse it
                            nextState = State.PULSE_PROC;
                            goto NEXT_STATE;
                        }
                    case State.PULSE_PROC:
                        {
                            csrStatus = this.pipeCursor.Next(mc, out outputObj);
                            if (csrStatus == CursorStatus.EOF)
                            {
                                nextState = State.PROC_AT_EOF;
                                goto NEXT_STATE;
                            }
                            break;
                        }
                    case State.PROC_AT_EOF:
                        {
                            // when the proc is at EOF, the last input object is not longer valid
                            this.lastInputObject = null;
                            this.pipeCursor.Clear(mc);

                            // if pass thru is on, pipe out the input object.
                            if (this.passThru)
                            {
                                outputObj = inputObj;
                                csrStatus = CursorStatus.HAS_ROW;
                                //logger.Info("PIPE CURSOR HIT EOF, RETURN INPUT OBJECT");
                                this.nextState = State.SETUP_PROC;
                            }
                            else
                            {
                               // logger.Info("PIPE CURSOR HIT EOF, REQUEST NEW FROM PARENT");
                                //inputObj.Delete();
                                csrStatus = CursorStatus.PARENT_NEXT;
                                this.nextState = State.SETUP_PROC;
                            }
                            break;
                        }
                    case State.INVALID:
                        {
                           throw new Exception("CursorExecute in invalid state. It was resumed after it returned EOF.");
                        }
                    default: throw new Exception("unexpected state:" + nextState);
                }

                // restore the current object
                mc.objectData = currentObject;

                // reset null input ctr
                nullInputCtr = 0;
            }
            // First time it is given null, request from parent.
            else
            {
                // First InputObject is null, try to consume from parent.
                if (++nullInputCtr == 1)
                {
                   // logger.Info("*** EXECUTE CALLED WITH NULL first time");
                    csrStatus = CursorStatus.PARENT_NEXT;
                }
                // Second time InputObject is null, we no that no more are coming to return EOF.
                else
                {
                   // logger.Info("*** EXECUTE CALLED WITH NULL second time");
                    csrStatus = CursorStatus.EOF;
                    // one we return EOF next state where we have a nonnull input object
                    // must call the proc again.
                    this.nextState = State.INVALID;
                }
            }
            return csrStatus;
        }

        protected override void CursorClear(ModuleContext mc)
        {
            //this.pipeCursor.Clear(mc);
        }
    }
}
