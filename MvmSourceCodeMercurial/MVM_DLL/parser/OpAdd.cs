﻿using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime.Tree;

namespace MVM
{
    public class OpAdd : BaseBinaryOpSetup
    {
        public override IReadString CreateRun(IReadString left, IReadString right)
        {
            return new OpAddRun(left, right);
        }
    }

    public class OpAddRun : ReadStringBase
    {
        private readonly IReadString left;
        private readonly IReadString right;
        public OpAddRun(IReadString left, IReadString right)
        {
            this.left = left;
            this.right = right;
        }
        public override string Read(ModuleContext mc)
        {
            decimal left, right, result;
            if (!decimal.TryParse(this.left.Read(mc), out left))
            {
                
                return "";
            }
            if (!decimal.TryParse(this.right.Read(mc), out right))
            {
                return "";
            }
            result = left + right;
            return result.ToString();
        }
    }
}
