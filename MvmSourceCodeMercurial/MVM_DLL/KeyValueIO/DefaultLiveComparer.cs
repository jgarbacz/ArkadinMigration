using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    /// <summary>
    /// Creates a default comparer from a comparable type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DefaultLiveComparer<T>:IComparer<T>
        where T : IBinarySerializableComparable<T>
    {
        public DefaultLiveComparer(){
        }

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            return x.CompareTo(y);
        }

        #endregion
    }


    /// <summary>
    /// Creates a default dynamic comparer for a comparable<object>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DynamicDefaultLiveComparer<T> : IComparer<object>
        where T : IComparable<object>
    {
        public DynamicDefaultLiveComparer()
        {
        }
        public int Compare(object x, object y)
        {
            IComparable<object> tx = x as IComparable<object>;
            IComparable<object> ty = y as IComparable<object>;
            return tx.CompareTo(ty);
        }
    }
}
