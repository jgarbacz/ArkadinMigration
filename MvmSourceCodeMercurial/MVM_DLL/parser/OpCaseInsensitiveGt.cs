using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime.Tree;

namespace MVM
{
    public class OpCaseInsensitiveGt : BaseBinaryOpSetup
    {
        public override IReadString CreateRun(IReadString left, IReadString right)
        {
            return new OpCaseInsensitiveGtRun(left, right);
        }
    }

    public class OpCaseInsensitiveGtRun : ReadStringBase
    {
            private readonly IReadString left;
            private readonly IReadString right;
            public OpCaseInsensitiveGtRun(IReadString left, IReadString right)
            {
                this.left = left;
                this.right = right;
            }
            public override string Read(ModuleContext mc)
            {
                string left = this.left.Read(mc);
                string right = this.right.Read(mc);
                return left.IsGtIgnoreCase(right) ? "1" : "0";
            }
    }
}
