using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime.Tree;

namespace MVM
{
    public class OpGt : BaseBinaryOpSetup
    {
        public override IReadString CreateRun(IReadString left, IReadString right)
        {
            return new OpGtRun(left, right);
        }
    }

    public class OpGtRun : ReadStringBase
    {
            private readonly IReadString left;
            private readonly IReadString right;
            public OpGtRun(IReadString left, IReadString right)
            {
                this.left = left;
                this.right = right;
            }
            public override string Read(ModuleContext mc)
            {
                string left = this.left.Read(mc);
                string right = this.right.Read(mc);
                return left.IsNumericGt(right) ? "1" : "0";
            }
    }
}
