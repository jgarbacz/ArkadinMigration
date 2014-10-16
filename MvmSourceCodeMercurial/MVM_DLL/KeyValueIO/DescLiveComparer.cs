using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    /// <summary>
    /// This class makes it easy for another class to create a raw comparer version of itself
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DescLiveComparer<T>:IComparer<T>
        where T : IBinarySerializableComparable<T>
    {

        public readonly IComparer<T> inputComparer;
        public DescLiveComparer(IComparer<T> inputComparer){
            this.inputComparer = inputComparer;
        }

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            int ascValue=this.inputComparer.Compare(x,y);
            int dscValue = 0 - ascValue;
            return dscValue;
        }

        #endregion
    }


   
}
