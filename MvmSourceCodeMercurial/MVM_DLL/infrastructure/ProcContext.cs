using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    public class ProcContext
    {
        // Set by the worker, shouldn't be serialized
        public ThreadContext threadContext;

        // Set by the constructor, needs to persist and span workers
        public readonly TempContext tempContext;

        // When procContext A, calls another procContext B, A.childProcContext=B.procContext
        public ProcContext childProcContext;

        // when we call another proc but with the same scope we set this before the call
        // so we can back up the scope when we get back.
        public int snapshotScopeDepth;

        // Contructor (creates new threadContext context)
        public ProcContext()
        {
            this.tempContext = new TempContext(this);
        }

        // Constructor (uses passed threadContext context)
        public ProcContext(TempContext tempContext)
        {
            this.tempContext = tempContext;
        }

        // Workers must set the threadContext context before they run the procInst
        public void SetThreadContext(ThreadContext threadContext)
        {
            this.threadContext = threadContext;
        }
    }
}
