using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
namespace MVM
{

    /**
     * Provides common cursor runtime support for cursors
     * MAIN OBJECTIVES
     * -new object for every cursor row
     * -can request a class of object
     * -cursor's all support the same sub-procs
     * -ctrl when and object is deleted
     * 
     * there's two things.. there the cursor setup... the the run...
     * 
     * 
    <counter_select>
      <from>1</from>
      <to>3</to>
      <cursor>TEMP.csr</cursor>
      <cursor_delete>'next'|never|'clear'</cursor_delete> 
      <cursor_class>'erd'|'basic'</cursor_class>
      <loop>
        <print>OBJECT(TEMP.csr).value</print>
      </loop>
    </counter_select>
    
     */

   

    /**
     * 
     * 
     */
    public abstract class CursorCommon : CursorCommonBase,ICursor
    {
        public CursorCommon(ModuleContext mc, ICursorSetupCommon cursorSetup)
            :base(mc,cursorSetup)
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
        public CursorStatus Next(ModuleContext mc,out IObjectData outputObject)
        {
            //logger.Info("[CursorCommon]"+this.GetType().Name+"." + this.CursorInstId + ".Next()");
            this.ReleaseCursorOid();
            CursorStatus csrStatus = this.CursorNext(mc,  out outputObject);
            if (csrStatus == CursorStatus.HAS_ROW)
            {
                this.UpdateCursorObject(outputObject);
            }
            else if(csrStatus==CursorStatus.EOF)
            {
                this.Eof = true;
            }
            return csrStatus;
        }

        /// <summary>
        /// Optionally looks at the inputObj (which may be null) to produce an output object. 
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="inputObj"></param>
        /// <param name="outputObj"></param>
        /// <returns></returns>
        protected abstract CursorStatus CursorNext(ModuleContext mc, out IObjectData outputObj);
    }

}
