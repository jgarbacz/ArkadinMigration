using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;
using NLog;

namespace MVM
{
    /*
   
    <proc_select>
      <name>'count_to_x'</name>
      <param name='x'>3</param>
      <cursor>TEMP.csr</cursor>
      <loop>
        <print>'CONSUMER.value='~OBJECT(TEMP.csr).value</print>
      </loop>
    </proc_select>
     
     */

    public class MProcSelect : IModuleSetup
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

       
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            CursorSetupCommon cursorSetup = new CursorSetupCommon(me, mc);
            string cursorSyntax = cursorSetup.cursorSyntax;
            string procNameSyntax = me.SelectNodeInnerText("./name");

            string pipeCursorInstIdSyntax = cursorSetup.cursorInstIdSyntax;
            if (pipeCursorInstIdSyntax.IsNullOrEmpty())
            {
                pipeCursorInstIdSyntax = "TEMP." + mc.GetGenSym("pipe_csr_inst_id");
            }
            string producerProcInstSyntax = "TEMP." + mc.GetGenSym("producer_proc_inst");
           
            // need to assign the pipeCursorInstId
            {
                IWriteString pipeCursorInstIdParsed = mc.ParseWritableSyntax(pipeCursorInstIdSyntax);
                run.Add(new AssignPipeCursorInstId(pipeCursorInstIdParsed));
            }

            // setup the producer saving the proc id
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<setup_call_proc_for_current_object>");
                sb.AppendLine("<name>" + procNameSyntax + "</name>");
                sb.AppendLine("<proc_inst_id>" + producerProcInstSyntax + "</proc_inst_id>");
                foreach (var paramElem in me.SelectElements("./param"))
                {
                    sb.AppendLine(paramElem.OuterXml);
                }
                sb.AppendLine("<param name='pipe_cursor'>" + pipeCursorInstIdSyntax + "</param>");
                sb.AppendLine("</setup_call_proc_for_current_object>");
                run.Add(mc.GetModuleRun(sb.ToString()));
            }
            
            // open up the pipe
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<open_pipe_cursor>");
                sb.AppendLine("<proc_inst_id>" + producerProcInstSyntax + "</proc_inst_id>");
                sb.AppendLine("<cursor_inst_id>" + pipeCursorInstIdSyntax + "</cursor_inst_id>");
                sb.AppendLine("</open_pipe_cursor>");
                run.Add(mc.GetModuleRun(sb.ToString()));
            }

            // Add procInst for then/loop/else
            cursorSetup.AddCursorSubProcs(me, mc, run);
        }
 
    }
    public class AssignPipeCursorInstId : IModuleRun
    {
        public IWriteString pipeCursorInstIdParsed;
        public AssignPipeCursorInstId(IWriteString pipeCursorInstIdParsed)
        {
            this.pipeCursorInstIdParsed = pipeCursorInstIdParsed;
        }

        public void Run(ModuleContext mc)
        {
            string pipeCursorInstId = mc.GetGenSym("pipeCsrInstId");
            //mc.mvm.Trace("assigned pipeCsrInstId=" + pipeCursorInstId);
            this.pipeCursorInstIdParsed.Write(mc, pipeCursorInstId);
        }
    }


    public class MOpenPipeCursor : IModuleRun, IModuleSetup
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        private string procInstIdSyntax;
        private IReadString procInstIdParsed;
        private CursorSetupCommon cursorSetup;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MOpenPipeCursor m = new MOpenPipeCursor();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            m.procInstIdSyntax = me.SelectNodeInnerText("proc_inst_id");
            m.procInstIdParsed = mc.ParseSyntax(m.procInstIdSyntax);
            run.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            // The consumer is resposible for seting up the cursor that the producer
            // writes to.
            long producerInstId = this.procInstIdParsed.Read(mc).ToLong();
            PipeCursor pipeCursor = new PipeCursor(mc, this.cursorSetup, producerInstId);
        }
    }


    public class PipeCursor : CursorCommon, ICursor
    {

        //public static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Producer proc instance id (callback id)
        /// </summary>
        public long producerProcInstId;

        // constructor
        public PipeCursor(ModuleContext mc, ICursorSetupCommon setup, long producerInstId)
            : base(mc, setup)
        {
            this.producerProcInstId = producerInstId;
            //logger.Trace("PIPECSR constructed with csrInstId=" + this.CursorInstId);
        }


        private enum State { CALL_PROC, READ_RESULT }
        private State nextState = State.CALL_PROC;

        // increments the cursor
        protected override CursorStatus CursorNext(ModuleContext mc, out IObjectData outputObject)
        {
            switch (nextState)
            {
                case State.CALL_PROC:
                    {
                        // null out the cursorOid so we know if it didn't get set
                        this.CursorOid = null;
                        //logger.Trace("pipe_cusor.CursorNext " + nextState + " resuming procInstId=" + this.producerProcInstId);
                        outputObject = null;
                        mc.workMgr.PulseCallback(mc.procInst, this.producerProcInstId);
                        mc.procInst.nextModuleOrder = mc.procDefinition.GetNextModuleOrder(mc.moduleIdx);
                        mc.moduleStatus = ModuleStatus.Yield;
                        this.nextState = State.READ_RESULT;
                        return CursorStatus.YIELD;
                    }
                case State.READ_RESULT:
                    {
                        if (this.nextObject!=null)
                        {
                            outputObject = this.nextObject;
                            this.nextObject = null;
                            //logger.Trace("pipe_cusor.CursorNext " + nextState + "  reading results procInstId=" + this.producerProcInstId + " read oid=" + outputObject.objectId);
                            this.nextState = State.CALL_PROC;
                            return CursorStatus.HAS_ROW;
                        }
                        //logger.Trace("pipe_cusor.CursorNext " + nextState + "  reading results procInstId=" + this.producerProcInstId + " at EOF");

                        outputObject = null;
                        return CursorStatus.EOF;
                    }
                default: throw new Exception("unexpected state:" + nextState);
}
            }

        protected override void CursorClear(ModuleContext mc)
        {
            //logger.Trace("pipe_cusor.CursorClear deleting procInstId=" + this.producerProcInstId);
            mc.ProcInstDelete(this.producerProcInstId);
        }


        private IObjectData nextObject;
        public void SetNextObject(string nextCursorOid)
        {
            this.nextObject = this.mvm.objectCache.CheckOut(nextCursorOid);
            //logger.Trace("[pipe_row] set pipecursor.nextobject to [" + this.nextObject.objectId + "] refs="+this.nextObject.RefCount);
        }

    }
}
