using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime.Tree;

namespace MVM
{
    public class OpCaseInsensitiveOrNumericNe : BaseBinaryOpSetup
    {
        public override IReadString CreateRun(IReadString left, IReadString right)
        {
            return new OpCaseInsensitiveOrNumericNeRun(left, right);
        }
    }

    public class OpCaseInsensitiveOrNumericNeRun : ReadStringBase
    {
            private readonly IReadString left;
            private readonly IReadString right;
            public OpCaseInsensitiveOrNumericNeRun(IReadString left, IReadString right)
            {
                this.left = left;
                this.right = right;
            }
            public override string Read(ModuleContext mc)
            {
                string left = this.left.Read(mc);
                string right = this.right.Read(mc);
                return left.EqualsIgnoreCaseOrEqualsNumeric(right) ? "0":"1";
            }
    }
}
