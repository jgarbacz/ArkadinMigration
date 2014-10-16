using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime.Tree;

namespace MVM
{
    public class OpConcat:BaseBinaryOpSetup
    {
        public override IReadString CreateRun(IReadString left, IReadString right)
        {
            return new ConcatRun(left, right);
        }
    }

    class ConcatRun : ReadStringBase
    {
        private readonly IReadString left;
        private readonly IReadString right;
        public ConcatRun(IReadString left, IReadString right)
        {
            this.left = left;
            this.right = right;
        }

        #region IModuleRecurse Members

        public override string Read(ModuleContext mc)
        {
            return this.left.Read(mc)+this.right.Read(mc);
        }

        #endregion
    }

}
