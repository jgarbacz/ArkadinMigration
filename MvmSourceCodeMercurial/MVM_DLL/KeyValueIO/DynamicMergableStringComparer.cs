using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public class DynamicMergableStringComparer:IDynamicMergeableComparer
    {
        #region IDynamicMergeableComparer Members

        public IComparer<object> DynamicLiveComparer
        {
            get { throw new NotImplementedException(); }
        }

        public IComparer<RawValue<object>> DynamicRawComparer
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
