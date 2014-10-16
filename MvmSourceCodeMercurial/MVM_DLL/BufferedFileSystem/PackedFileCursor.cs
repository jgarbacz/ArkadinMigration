using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public class PackedFileCursor : CursorCommonLinqEnabled, ICursor
    {
        // set by constructor
        private string fileName;
        private bool blocking;

        // constructor
        public PackedFileCursor(ModuleContext mc, ICursorSetupCommon cursorSetup, string fileName, bool blocking)
            : base(mc, cursorSetup)
        {
            this.fileName = fileName;
            this.blocking = blocking;
        }

        public override IObjectData CursorNext()
        {
                using (var f = this.mvm.globalContext.bfs.GetObjectQueue(fileName, true))
                {
                    var bqueue = (f.value as ObjectQueueBufferedFile);
                    bqueue.Blocking = this.blocking;
                    IObjectData csrObj;
                    if (bqueue.ReadObjectIntoObjectCache(this.mvm.mvmCluster, out csrObj))
                    {
                        return csrObj;
                    }
                    else
                    {
                        return null;
                    }
                }
        }

        public override void CursorClear()
        {
        }
    }
}
