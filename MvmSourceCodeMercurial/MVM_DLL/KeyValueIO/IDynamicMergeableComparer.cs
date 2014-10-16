using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public interface IDynamicMergeableComparer
    {
        IComparer<object> DynamicLiveComparer { get; }
        IComparer<RawValue<object>> DynamicRawComparer { get; }
    }
}
