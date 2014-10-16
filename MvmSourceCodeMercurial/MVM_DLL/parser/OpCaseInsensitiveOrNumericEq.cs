using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime.Tree;

namespace MVM
{
    public class OpCaseInsensitiveOrNumericEq : BaseBinaryOpSetup
    {
        public override IReadString CreateRun(IReadString left, IReadString right)
        {
            return new OpCaseInsensitiveOrNumericEqRun(left, right);
        }
    }

    public class OpCaseInsensitiveOrNumericEqRun : ReadStringBase
    {
            private readonly IReadString left;
            private readonly IReadString right;
            public OpCaseInsensitiveOrNumericEqRun(IReadString left, IReadString right)
            {
                this.left = left;
                this.right = right;
            }
            public override string Read(ModuleContext mc)
            {
                string left = this.left.Read(mc);
                string right = this.right.Read(mc);
                return left.EqualsIgnoreCaseOrEqualsNumeric(right) ? "1":"0";
            }
    }
}
