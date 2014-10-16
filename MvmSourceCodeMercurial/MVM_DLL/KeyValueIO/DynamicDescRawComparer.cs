using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public class DynamicDescRawComparer: IComparer<RawValue<object>>
    {
        public readonly IComparer<RawValue<object>> inputComparer;

        public DynamicDescRawComparer(IComparer<RawValue<object>> inputComparer)
        {
            this.inputComparer = inputComparer;
        }

        #region IComparer<RawValue<T>> Members

        public int Compare(RawValue<object> x, RawValue<object> y)
        {
            return 0 - this.inputComparer.Compare(x, y);
        }

        #endregion
    }
}
