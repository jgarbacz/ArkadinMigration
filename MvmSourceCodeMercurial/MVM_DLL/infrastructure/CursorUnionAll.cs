using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace MVM
{
    public class CursorUnionAll : CursorCommon
    {
        public CursorUnionAll(ModuleContext mc, ICursorSetupCommon cursorSetup, List<ICursor> inputCursors)
            : base(mc, cursorSetup)
        {
            this.inputCursors = inputCursors;
            // make sure the input cursor do not delete on next because they do no own the objects
            //foreach (var inputCsr in this.inputCursors) inputCsr.DeleteObjectOnNext = false;
        }

        // input cursors to be unioned
        public List<ICursor> inputCursors;
        public int inputCursorIdx = 0;
      
        // these are the states of this module
        public enum State { READ_CURSOR, READ_CURSOR_RESULT, READ_PIPELINE, READ_PIPELINE_RESULT };

        // start out by reading the last operator in the pipeline
        public State nextState = State.READ_PIPELINE;

        // 1 generator, N operators
        protected override CursorStatus CursorNext(ModuleContext mc, out IObjectData outputObj)
        {
            for (; ; )
            {
                ICursor currCursor = this.inputCursors[inputCursorIdx];
                CursorStatus csrStatus = currCursor.Next(mc, out outputObj);
                switch (csrStatus)
                {
                    case CursorStatus.HAS_ROW:
                        {
                            return csrStatus;
                        }
                    case CursorStatus.EOF:
                        {
                            // clear the current cursor
                            currCursor.Clear(mc);
                            // when at EOF, see if we can goto the next cursor
                            // if we can great, if not return EOF.
                            if (++this.inputCursorIdx <= this.inputCursors.MaxIndex())
                            {
                                continue;
                            }
                            return csrStatus;
                        }
                    case CursorStatus.YIELD:
                        {
                            return csrStatus;
                        }
                    default: throw new Exception("unexpected cursor status:" + csrStatus);
                }
            }
        }

        protected override void CursorClear(ModuleContext mc)
        {
            // tbd, clear any remaining open cursors
        }
    }
}
