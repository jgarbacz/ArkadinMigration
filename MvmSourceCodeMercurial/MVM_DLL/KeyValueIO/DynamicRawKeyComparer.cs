using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    /// <summary>
    /// Comparison class for raw dynamic keys
    /// </summary>
    public class DynamicRawKeyComparer : IComparer<RawValue<DynamicKey>>
    {
        readonly IComparer<RawValue<object>>[] comparers;
        public DynamicRawKeyComparer(IComparer<RawValue<object>>[] comparers)
        {
            this.comparers = comparers;
        }
        public int Compare(RawValue<DynamicKey> x, RawValue<DynamicKey> y)
        {
            int xOffset = x.offset;
            int yOffset = y.offset;
            for (int i = 0; i < comparers.Length; i++)
            {
                IComparer<RawValue<object>> cmp = this.comparers[i];

                
                int xLength = x.buffer.Read7BitEncodedInt(ref xOffset);
                var xRawValue = new RawValue<object>(x.buffer, xOffset, xLength);

                int yLength = y.buffer.Read7BitEncodedInt(ref yOffset);
                var yRawValue = new RawValue<object>(y.buffer, yOffset, yLength);

                int val = cmp.Compare(xRawValue, yRawValue);
                if (val != 0) return val;

                xOffset += xLength;
                yOffset += yLength;
            }
            return 0;
        }
    }
}
