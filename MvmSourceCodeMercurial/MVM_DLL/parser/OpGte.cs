using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime.Tree;

namespace MVM
{
    public class OpGte : BaseBinaryOpSetup
    {
        public override IReadString CreateRun(IReadString left, IReadString right)
        {
            return new OpGteRun(left, right);
        }
    }

    public class OpGteRun : ReadStringBase
    {
            private readonly IReadString left;
            private readonly IReadString right;
            public OpGteRun(IReadString left, IReadString right)
            {
                this.left = left;
                this.right = right;
            }
            public override string Read(ModuleContext mc)
            {
                string left = this.left.Read(mc);
                string right = this.right.Read(mc);
                return left.IsNumericGte(right) ? "1" : "0";
            }
    }
}
