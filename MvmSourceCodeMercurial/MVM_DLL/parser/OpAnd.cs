using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime.Tree;

namespace MVM
{
    public class OpAnd : BaseBinaryOpSetup
    {
        public override IReadString CreateRun(IReadString left, IReadString right)
        {
            return new OpAndRun(left, right);
        }
    }

    public class OpAndRun : ReadStringBase
    {
            private readonly IReadString left;
            private readonly IReadString right;
            public OpAndRun(IReadString left, IReadString right)
            {
                this.left = left;
                this.right = right;
            }
            public override string Read(ModuleContext mc)
            {
                string left = this.left.Read(mc);
                if (left.Equals("0")) return "0";
                if (!left.Equals("1")) throw new Exception("Error, invalid boolean ["+left+"]");
                string right = this.right.Read(mc);
                if (right.Equals("0")) return "0";
                if (!right.Equals("1")) throw new Exception("Error, invalid boolean[" + right + "]");
                return "1";
            }
    }
}
