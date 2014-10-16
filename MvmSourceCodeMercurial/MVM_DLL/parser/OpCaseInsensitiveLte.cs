using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime.Tree;

namespace MVM
{
    public class OpCaseInsensitiveLte : BaseBinaryOpSetup
    {
        public override IReadString CreateRun(IReadString left, IReadString right)
        {
            return new OpCaseInsensitiveLteRun(left, right);
        }
    }

    public class OpCaseInsensitiveLteRun : ReadStringBase
    {
            private readonly IReadString left;
            private readonly IReadString right;
            public OpCaseInsensitiveLteRun(IReadString left, IReadString right)
            {
                this.left = left;
                this.right = right;
            }
            public override string Read(ModuleContext mc)
            {
                string left = this.left.Read(mc).ToLower();
                string right = this.right.Read(mc).ToLower();
                int result=left.CompareTo(right);
                return left.IsLteIgnoreCase(right) ? "1" : "0";
            }
    }
}
