using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    /// <summary>
    /// For given a type that is <code>IComparable<object></code> and <code>IBinaryComparable</code> this
    /// generates a <code>IDynamicMergeableComparer<object> Default and Descending Default Comparer. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DynamicMergeableComparer<T> : IDynamicMergeableComparer
        where T : IComparable<object>, IBinaryComparable, new()
    {
        public static readonly IComparer<object> DefaultLiveComparer = new DynamicDefaultLiveComparer<T>();
        public static readonly IComparer<RawValue<object>> DefaultRawComparer = new DynamicDefaultRawComparer<T>();
        public static readonly DynamicMergeableComparer<T> Default = new DynamicMergeableComparer<T>(DefaultLiveComparer, DefaultRawComparer);

        public static readonly IComparer<object> DefaultLiveComparerDesc = new DynamicDescLiveComparer(DefaultLiveComparer);
        public static readonly IComparer<RawValue<object>> DefaultRawComparerDesc = new DynamicDescRawComparer(DefaultRawComparer);
        public static readonly DynamicMergeableComparer<T> DefaultDesc = new DynamicMergeableComparer<T>(DefaultLiveComparerDesc, DefaultRawComparerDesc);

        public IComparer<object> DynamicLiveComparer { get; private set; }
        public IComparer<RawValue<object>> DynamicRawComparer { get; private set; }

        /// <summary>
        /// Generate an IMergeableComparer<T> or IMergeableComparer for a given type.
        /// </summary>
        /// <param name="LiveComparer"></param>
        /// <param name="RawComparer"></param>
        public DynamicMergeableComparer(IComparer<object> DynamicLiveComparer, IComparer<RawValue<object>> DynamicRawComparer)
        {
            this.DynamicLiveComparer = DynamicLiveComparer;
            this.DynamicRawComparer = DynamicRawComparer;
        }
    }
}
