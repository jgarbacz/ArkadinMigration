using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{

    /// <summary>
    /// Cursor that extend this can only return EOF or HAS_ROW. If they can live with this restriction
    /// then they can be used with LINQ.
    /// </summary>
    public abstract class CursorCommonLinqEnabled : CursorCommonMeta, IEnumerable<IObjectData>
    {
        public CursorCommonLinqEnabled(ModuleContext mc, ICursorSetupCommon cursorSetup):base(mc,cursorSetup)
        {
        }

        protected override CursorStatus CursorNext(ModuleContext mc, out IObjectData outputObj)
        {
            outputObj = this.CursorNext();
            if (outputObj!=null)
            {
                return CursorStatus.HAS_ROW;
            }
            outputObj = null;
            return CursorStatus.EOF;
        }

        protected override void CursorClear(ModuleContext mc)
        {
            this.CursorClear();
        }

        /// <summary>
        /// Next that does not require the module context
        /// </summary>
        /// <returns></returns>
        public string Next()
        {
            IObjectData outputObj = this.CursorNext();
            if (outputObj != null)
            {
                this.UpdateCursorObject(outputObj);
            }
            else
            {
                this.Eof = true;
            }
            return this.CursorOid;
        }

        /// <summary>
        /// returns a null meaning EOF or a valid IObjectData.
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
     
        public abstract IObjectData CursorNext();

        /// <summary>
        ///  Clears any resources associated to this specific cursor type
        /// </summary>
        /// <param name="mc"></param>
        public abstract void CursorClear();


        #region IEnumerable<IObjectData> Members

        public IEnumerator<IObjectData> GetEnumerator()
        {
            return new ObjectDataEnumerator(this);
        }

        #endregion
        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }


    /// <summary>
    /// This class allows us to use an Mvm object cursor with Linq.
    /// </summary>
    public class ObjectDataEnumerator : IEnumerator<IObjectData>
    {
        CursorCommonLinqEnabled cursor;
        public ObjectDataEnumerator(CursorCommonLinqEnabled cursor)
        {
            this.cursor = cursor;
        }

        #region IEnumerator<IObjectData> Members

        public IObjectData Current
        {
            get { return this.cursor.UseCursorObject(); }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.cursor.Clear(null);
        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get { return this.Current; }
        }

        public bool MoveNext()
        {
            IObjectData outputObj;
            var csrStatus=this.cursor.Next(null, out outputObj);
            if (csrStatus != CursorStatus.HAS_ROW && csrStatus != CursorStatus.EOF)
            {
                throw new Exception("Error cursor returned status=" + csrStatus + " so it does not support linq access");
            }
            return !this.cursor.Eof;
        }

        public void Reset()
        {
            throw new NotImplementedException("Reset not implemented for object cursors");
        }

        #endregion
    }

    
}
