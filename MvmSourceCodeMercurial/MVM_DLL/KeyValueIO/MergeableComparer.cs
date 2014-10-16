using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public class MergeableComparer<T> : IMergeableComparer<T>
      where T : IBinarySerializableComparable<T>, new()
    {
        public static readonly MergeableComparer<T> Default =
            new MergeableComparer<T>(
                new DefaultLiveComparer<T>(),
                new DefaultRawComparer<T>()
                );
        
        public static readonly MergeableComparer<T> DefaultDesc =
            new MergeableComparer<T>(
                new DescLiveComparer<T>(new DefaultLiveComparer<T>()),
                new DescRawComparer<T>(new DefaultRawComparer<T>())
                );

        public IComparer<T> LiveComparer { get; private set; }
        public IComparer<RawValue<T>> RawComparer { get; private set; }


        /// <summary>
        /// Generate an IMergeableComparer<T> or IMergeableComparer for a given type.
        /// </summary>
        /// <param name="LiveComparer"></param>
        /// <param name="RawComparer"></param>
        public MergeableComparer(IComparer<T> LiveComparer, IComparer<RawValue<T>> RawComparer)
        {
            this.LiveComparer = LiveComparer;
            this.RawComparer = RawComparer;
        }
    }


    
}
