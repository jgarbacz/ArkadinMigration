using System;
namespace MVM
{
    public interface ICursorSetupCommon
    {
        
        /// <summary>
        /// If cursor type is 'erd' this is the feedback name to use.
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        string GetCursorTypeFeedbackName(ModuleContext mc);
        
        
        string GetCursorOid(ModuleContext mc);
        string GetCursorType(ModuleContext mc);
        
        string GetCursorValue(ModuleContext mc);
        IWriteString writeCursorParsed { get; set; }
        void SetCursorOid(ModuleContext mc, string cursorOid);
        
        string GetCursorDelete(ModuleContext mc);


        bool NeedsHeaderObject { get;  }

        /// <summary>
        /// Returns the cursorInstId if one is already assigned (usually not, except for proc select) or null.
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        string GetCursorInstId(ModuleContext mc);

        /// <summary>
        /// Sets the select output cursorInsId to the passed value
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="cursorInstId"></param>
        void SetCursorInstId(ModuleContext mc, string cursorInstId);
    }
}
