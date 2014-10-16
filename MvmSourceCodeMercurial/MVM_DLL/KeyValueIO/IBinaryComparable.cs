using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public interface IBinaryComparable
    {
        int CompareTo(byte[] buf1, int offset1, int len1, byte[] buf2, int offset2, int len2);
    }
}
