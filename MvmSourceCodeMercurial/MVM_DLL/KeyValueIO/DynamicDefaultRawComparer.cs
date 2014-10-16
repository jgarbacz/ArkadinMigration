using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public class DynamicDefaultRawComparer<T> : IComparer<RawValue<object>>
       where T : IBinaryComparable, new()
    {
        private static readonly T template = new T();
        public DynamicDefaultRawComparer()
        {
        }
        #region IComparer<RawValue<object>> Members

        public int Compare(RawValue<object> x, RawValue<object> y)
        {
            return template.CompareTo(x.buffer, x.offset, x.length, y.buffer, y.offset, y.length);
        }

        #endregion
    }
}
