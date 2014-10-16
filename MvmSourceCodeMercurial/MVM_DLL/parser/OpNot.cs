using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime.Tree;

namespace MVM
{
    public class OpNot : BaseUnaryOpSetup
    {
        public override IReadString CreateRun(IReadString left)
        {
            return new OpNotRun(left);
        }
    }

    public class OpNotRun : ReadStringBase
    {
            private readonly IReadString left;
            public OpNotRun(IReadString left)
            {
                this.left = left;
            }
            public override string Read(ModuleContext mc)
            {
                string v = this.left.Read(mc);
                if(v.Equals("1")) return "0";
                if (v.Equals("0")) return "1";
                throw new Exception("Error, cannot do not on non boolean value=["+v+"]");
            }
    }
}
