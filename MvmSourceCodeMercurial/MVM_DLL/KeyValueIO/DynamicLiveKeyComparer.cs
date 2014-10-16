using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    /// <summary>
    /// Comparison class for live dynamic keys
    /// </summary>
    public class DynamicLiveKeyComparer : IComparer<DynamicKey>
    {
        readonly IComparer<object>[] comparers;
        public DynamicLiveKeyComparer(IComparer<object>[] comparers)
        {
            this.comparers = comparers;
        }
        public int Compare(DynamicKey x, DynamicKey y)
        {
            object[] xObjects = x.objects;
            object[] yObjects = y.objects;
            for (int i = 0; i < comparers.Length; i++)
            {
                IComparer<object> cmp = this.comparers[i];
                object xObject = xObjects[i];
                object yObject = yObjects[i];
                int val = cmp.Compare(xObject, yObject);
                if (val != 0) return val;
            }
            return 0;
        }
    }
}
