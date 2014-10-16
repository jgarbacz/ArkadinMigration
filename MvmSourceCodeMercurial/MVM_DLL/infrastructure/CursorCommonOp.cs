using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{

    public abstract class CursorCommonOp : CursorCommonBase,ICursorOp
    {

        public CursorCommonOp(ModuleContext mc, ICursorSetupCommon cursorSetup)
            : base(mc, cursorSetup)
        {  
        }


        /// <summary>
        /// Goes to the next (or first) interation of the cursor that may take an input object.
        /// if CursorNext()
        /// returns false, it creates a new object with OBJECT.object_type='EOF' and OBJECT.eof=1.
        /// If CursorNext() return true and then it will
        /// set CURRENT.eof=0; otherwise, it is expected that some externally (like pipe
        /// cursor) will instanciate a new object and with appropriate eof settings.
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        public CursorStatus Next(ModuleContext mc, IObjectData inputObject, out IObjectData outputObject)
        {
            //logger.Info("[CursorCommon]" + this.GetType().Name + "." + this.CursorInstId + ".Next()");
            this.ReleaseCursorOid();
            CursorStatus csrStatus = this.CursorNext(mc, inputObject, out outputObject);
            if (csrStatus == CursorStatus.HAS_ROW)
            {
                this.UpdateCursorObject(outputObject);
            }
            else if (csrStatus == CursorStatus.EOF)
            {
                this.Eof = true;
            }
            return csrStatus;
        }

        public abstract CursorStatus CursorNext(ModuleContext mc, IObjectData inputObj, out IObjectData outputObj);

       
    }
}
