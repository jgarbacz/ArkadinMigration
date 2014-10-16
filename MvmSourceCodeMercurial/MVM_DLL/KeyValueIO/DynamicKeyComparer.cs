using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    /// <summary>
    /// Comparison class for dynamic keys
    /// </summary>
    public class DynamicKeyComparer : IMergeableComparer<DynamicKey>
    {
        public readonly IComparer<DynamicKey> liveComparer;
        public readonly IComparer<RawValue<DynamicKey>> rawComparer;

        public DynamicKeyComparer(IComparer<object>[] liveComparers, IComparer<RawValue<object>>[] rawComparers)
        {
            this.liveComparer = new DynamicLiveKeyComparer(liveComparers);
            this.rawComparer = new DynamicRawKeyComparer(rawComparers);
        }

        public DynamicKeyComparer(params IDynamicMergeableComparer[] comparers)
        {
            IComparer<RawValue<object>>[] rawComparers = new IComparer<RawValue<object>>[comparers.Length];
            IComparer<object>[] liveComparers = new IComparer<object>[comparers.Length];
            for (int i = 0; i < comparers.Length; i++)
            {
                liveComparers[i] = comparers[i].DynamicLiveComparer;
                rawComparers[i] = comparers[i].DynamicRawComparer;
            }
            this.liveComparer = new DynamicLiveKeyComparer(liveComparers);
            this.rawComparer = new DynamicRawKeyComparer(rawComparers);
        }
        public IComparer<DynamicKey> LiveComparer
        {
            get { return liveComparer; }
        }
        public IComparer<RawValue<DynamicKey>> RawComparer
        {
            get { return rawComparer; }
        }
    }
    

}
