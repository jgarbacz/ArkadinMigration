using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    /// <summary>
    /// This class makes any desc version of the passed comparer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DescRawComparer<T>:IComparer<RawValue<T>>
        where T : IBinarySerializableComparable<T>,new()
    {
        
        public readonly IComparer<RawValue<T>> inputComparer;

        public DescRawComparer(IComparer<RawValue<T>> inputComparer)
        {
            this.inputComparer = inputComparer;
        }

        #region IComparer<RawValue<T>> Members

        public int Compare(RawValue<T> x, RawValue<T> y)
        {
            return 0-this.inputComparer.Compare(x,y);
        }

        #endregion
    }
}
