using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
namespace MVM
{
    public abstract class CursorCommonBase
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Hook to mvm
        /// </summary>
        public readonly MvmEngine mvm;
        /// <summary>
        /// Unique identifier of the cursor instance so we can retrieve cursor from globalContext.
        /// </summary>
        public string CursorInstId { get; set; }
        /// <summary>
        /// Current dynamic object for the cursor.
        /// </summary>
        public string CursorOid { get; set; }

        /// <summary>
        /// Hook to setup parameters for the cursor
        /// </summary>
        public ICursorSetupCommon cursorSetup;

        // erd | basic
        public string cursorType;
        public string cursorTypeFeedbackName;

        /// <summary>
        /// Instanciates the cursor
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="cursorSetup"></param>
        public CursorCommonBase(ModuleContext mc, ICursorSetupCommon cursorSetup)
        {
            this.mvm = mc.mvm;
            this.cursorSetup = cursorSetup;

            // assign a new instance id and load it into global
            this.CursorInstId = cursorSetup.GetCursorInstId(mc);
            if (this.CursorInstId.IsNullOrEmpty())
            {
                this.CursorInstId = mc.GetGenSym("csrInstId");
                this.cursorSetup.SetCursorInstId(mc, this.CursorInstId);
            }
            mc.globalContext.SetNamedClassInst(this.CursorInstId, this);
            //logger.Info("REGISTERED CURSOR:" + this.CursorInstId + ", type=" + this.GetType().FullName);
            this.cursorType = this.cursorSetup.GetCursorType(mc);
            this.cursorTypeFeedbackName = this.cursorSetup.GetCursorTypeFeedbackName(mc);

            // if the setup specifies that a header object is needed then instanciate one.
            if (cursorSetup.NeedsHeaderObject)
            {
                using (IObjectData header = this.CreateNewObject())
                {
                    //logger.Info("Instanciated header object:" + header.objectId);
                    header["is_header"] = "1";
                    this.UpdateCursorObject(header);
                    this.cursorSetup.SetCursorOid(mc, this.CursorOid);
                }
            }
        }

        /// <summary>
        /// True if the cursor has reached end of file (Eof), meaning the current row is invalid.
        /// </summary>
        public bool Eof
        {
            get;
            set;
        }

        /// <summary>
        /// Clears the cursor and associated resources.
        /// should it be an error to clear a cursor 2x? no...
        /// </summary>
        /// <param name="mc"></param>
        public void Clear(ModuleContext mc)
        {
            //logger.Info("[CursorCommon]" + this.GetType().Name + "." + this.CursorInstId + ".Clear()");
            this.CursorClear(mc);
            this.ReleaseCursorOid();
            this.mvm.globalContext.RmNamedClassInst(this.CursorInstId);
        }

        /// <summary>
        /// Delete current cursor object
        /// </summary>
        public void ReleaseCursorOid()
        {
            if (this.CursorOid != null)
            {
                IObjectData toReleaseObj = this.UseCursorObject();
                if (!toReleaseObj.objectId.Equals(this.CursorOid))
                {
                    throw new Exception("Error, to release oid=" + toReleaseObj.objectId + "!=" + CursorOid);
                }
                var refCount = toReleaseObj.RefRemove();
                //logger.Info("[CursorCommon]" + this.GetType().Name + "." + this.CursorInstId + ".Release(" + this.CursorOid + ") refCount=" + refCount);
                this.CursorOid = null;
            }
            else
            {
                //logger.Info("[CursorCommon]" + this.GetType().Name + "." + this.CursorInstId + ".Release(NOTHING)");
            }
        }

        /// <summary>
        /// Returns the cursor object which needs to be Disposed()
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        public IObjectData UseCursorObject()
        {
            return this.mvm.objectCache.CheckOut(this.CursorOid);
        }

        /// <summary>
        /// This spawns a new object of the correct class and cluster
        /// and sets this.CursorOid and OBJECT(this.cursorOid).CursorInstId
        /// 
        /// Cursor either should use this function or they need to call
        /// ConvertCursorObject(). Otherwise they risk not giving an object
        /// of the requested type.
        /// </summary>
        /// <returns></returns>
        public IObjectData CreateNewObject()
        {
            IObjectData obj;
            if (this.cursorType.Equals("erd"))
            {
                obj = this.mvm.objectCache.CreateAndGetObjectDataFormattedDelta("CURSOR", "xx");//this.cursorTypeFeedbackName);
            }
            else
            {
                obj = this.mvm.objectCache.CreateAndGetObject("CURSOR");
            }
            return obj;
        }

        public IObjectData CreateNewObject(string feedbackName)
        {
            if (feedbackName == null)
            {
                return this.CreateNewObject();
            }
            else
            {
                return this.mvm.objectCache.CreateAndGetObjectDataFormatted("CURSOR", feedbackName);
            }
        }

        /// <summary>
        /// Updates the cursor to point to the passed object.  
        /// TBD: convert the object class type if needed.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IObjectData UpdateCursorObject(IObjectData obj)
        {
            this.CursorOid = obj.RefGet();
            obj.CursorInstId = this.CursorInstId;
            return obj;
        }

        /// <summary>
        /// Clears resources associated to this cursor.
        /// </summary>
        /// <param name="mc"></param>
        protected abstract void CursorClear(ModuleContext mc);
    }
}
