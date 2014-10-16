using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public class Delta
    {
        public long rcn;
        public string value;
        public Delta(long rcn, string value)
        {
            this.rcn = rcn;
            this.value = value;
        }
        public Delta(Delta copy)
        {
            this.rcn = copy.rcn;
            this.value = copy.value;
        }
        public override string ToString()
        {
            return rcn.ToString() + "," + value;
        }
    }
}
