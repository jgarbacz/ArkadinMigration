using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public class DynamicDescMergableComparer : IDynamicMergeableComparer
    {
        public readonly IDynamicMergeableComparer inputComparer;
        public DynamicDescMergableComparer(IDynamicMergeableComparer inputComparer)
        {
            this.inputComparer = inputComparer;
            this.DynamicLiveComparer = new DynamicDescLiveComparer(inputComparer.DynamicLiveComparer);
            this.DynamicRawComparer = new DynamicDescRawComparer(inputComparer.DynamicRawComparer);
        }

        #region IDynamicMergeableComparer Members

        public IComparer<object> DynamicLiveComparer
        {
            get;
            private set;
        }

        public IComparer<RawValue<object>> DynamicRawComparer
        {
            get;
            private set;
        }

        #endregion
    }
}
