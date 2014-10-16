using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace MVM
{
    public class CursorPipeline : CursorCommon
    {
        public CursorPipeline(ModuleContext mc, ICursorSetupCommon cursorSetup, ICursor cursor, List<ICursorOp> cursorOps)
            : base(mc, cursorSetup)
        {
            this.cursor = cursor;
            // pipeline now owns the input cursor, make sure it does not delete on next
            //this.cursor.DeleteObjectOnNext = false;
            this.cursorOps = cursorOps;
            this.nextOpIdx = cursorOps.MaxIndex();
        }

        // generator cursor
        public ICursor cursor;

        // pipelined operators
        public List<ICursorOp> cursorOps;

        // keeps state of current operator
        private int nextOpIdx;

        // current input pipeline object
        private IObjectData inputObj;
      
        // these are the states of this module
        public enum State { READ_CURSOR, READ_CURSOR_RESULT, READ_PIPELINE, READ_PIPELINE_RESULT };

        // start out by reading the last operator in the pipeline
        public State nextState = State.READ_PIPELINE;

        // 1 generator, N operators
        protected override CursorStatus CursorNext(ModuleContext mc, out IObjectData outputObj)
        {
            for (; ; )
            {
            NEXT_STATE:
                switch (nextState)
                {
                    case State.READ_PIPELINE:
                        {
                            ICursorOp currOp = cursorOps[nextOpIdx];
                            //logger.Info("PL exec " + currOp.GetType().Name + "." + currOp.CursorInstId + ".next(" + (inputObj != null ? inputObj.objectId : "NULL") + ")");
                            CursorStatus csrStatus = currOp.Next(mc, inputObj, out outputObj);
                            //logger.Info("PL done " + currOp.GetType().Name + "." + currOp.CursorInstId + ".next() returned " + (outputObj != null ? outputObj.objectId : "NULL") + ",status=" + csrStatus.ToString());
                            // Sets nextOpIdx and nextState based on the csrStatus
                            switch (csrStatus)
                            {
                                // If you have a row, keep pushing this through the pipeline or return it
                                // if at the end of the pipeline.
                                case CursorStatus.HAS_ROW:
                                    {
                                        nextState=State.READ_PIPELINE;
                                        if(nextOpIdx==cursorOps.MaxIndex()){
                                            inputObj = null;
                                            return CursorStatus.HAS_ROW;
                                        }
                                        inputObj = outputObj;
                                        nextOpIdx++;
                                        goto NEXT_STATE;
                                    }

                                // If cursor is at EOF, then make sure we put a null input object down the pipeline
                                // the whole pipeline is not necessarily finished as the null input object might unlock
                                // some buffered up objects for example in blah->sort()->where() sort buffers until it
                                // hits a null input object.
                                case CursorStatus.EOF:
                                    {
                                        inputObj = null;
                                        nextState=State.READ_PIPELINE;
                                        if (nextOpIdx == cursorOps.MaxIndex())
                                        {
                                            return CursorStatus.EOF;
                                        }
                                        nextOpIdx++;
                                        goto NEXT_STATE;
                                    }
                                // If the operator wants the next object get it from the parent
                                // cursorOp or cursor.
                                case CursorStatus.PARENT_NEXT:
                                    {
                                        inputObj = null;
                                        if (nextOpIdx == 0)
                                        {
                                            nextState=State.READ_CURSOR;
                                            goto NEXT_STATE;
                                        }
                                        nextState=State.READ_PIPELINE;
                                        nextOpIdx--;
                                        goto NEXT_STATE;
                                    }
                                // If cursor wants to yield, then do so. That cursor better have put in 
                                // a proc call and with an appropriate callback.
                                case CursorStatus.YIELD:
                                    {
                                        // yield mean when this resume do the same thing you just did.
                                        nextState = State.READ_PIPELINE;
                                        return csrStatus;
                                    }
                                default: throw new Exception("unexpected csrStatus=" + csrStatus);
                            }
                            throw new Exception("not expected");
                        }
          
                    case State.READ_CURSOR:
                        {
                            // if the cursor is already eof, send null input down the pipeline
                            if (this.cursor.Eof)
                            {
                                        // push the null object to the pipeline as it might free up more objects.
                                        inputObj = null;
                                        nextState = State.READ_PIPELINE;
                                        goto NEXT_STATE;
                            }
                            // otherwise, try to read from the cursor
                            CursorStatus csrStatus = cursor.Next(mc, out outputObj);
                            //logger.Info("PL exec " + cursor.GetType().Name + "." + cursor.CursorInstId + ".next() returned status "+csrStatus);
                            switch (csrStatus)
                            {
                                case CursorStatus.HAS_ROW:
                                    {
                                        nextState = State.READ_CURSOR_RESULT;
                                        goto NEXT_STATE;
                                    }
                                case CursorStatus.EOF:
                                    {
                                        // push the null object to the pipeline as it might free up more objects.
                                        inputObj = null;
                                        nextState = State.READ_PIPELINE;
                                        goto NEXT_STATE;
                                    }
                                case CursorStatus.YIELD:
                                    {
                                        // yield mean when this is resumed, do the same thing you just did.
                                        nextState = State.READ_CURSOR;
                                        return csrStatus;
                                    }
                                default: throw new Exception("unexpected cursor status:" + csrStatus);
                            }
                        }
                    case State.READ_CURSOR_RESULT:
                        {
                            inputObj = mc.objectCache.CheckOut(cursor.CursorOid);
                            //logger.Info("PL exec " + cursor.GetType().Name + "." + cursor.CursorInstId + ".next() object " + cursor.CursorOid.Nvl("NULL"));
                            nextState = State.READ_PIPELINE;
                            goto NEXT_STATE;
                        }
                    default: throw new Exception("unexpected state:" + nextState);
                }
            }
        }

        protected override void CursorClear(ModuleContext mc)
        {
            this.cursor.Clear(mc);
            foreach (var csr in this.cursorOps) csr.Clear(mc);
        }
    }


    // write a union cursor... which takes in N input cursors
    
    


}
