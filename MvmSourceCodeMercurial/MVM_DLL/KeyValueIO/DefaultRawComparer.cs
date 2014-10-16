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
    public class DefaultRawComparer<T>:IComparer<RawValue<T>>
        where T : IBinarySerializableComparable<T>,new()
    {
        
        private readonly T template=new T();

        public DefaultRawComparer()
        {
        }

        #region IComparer<RawValue<T>> Members

        public int Compare(RawValue<T> x, RawValue<T> y)
        {
            return template.CompareTo(x.buffer,x.offset,x.length,y.buffer,y.offset,y.length);
        }

        #endregion
    }

   
}
