using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public class DynamicDescLiveComparer : IComparer<object>
    {
        public readonly IComparer<object> inputComparer;
        public DynamicDescLiveComparer(IComparer<object> inputComparer)
        {
            this.inputComparer = inputComparer;
        }
        public int Compare(object x, object y)
        {
            int ascValue = this.inputComparer.Compare(x, y);
            int dscValue = 0 - ascValue;
            return dscValue;
        }
    }
}
