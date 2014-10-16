using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime.Tree;
using NLog;

namespace MVM
{
    public class OpDivide : BaseBinaryOpSetup
    {
        public override IReadString CreateRun(IReadString left, IReadString right)
        {
            return new OpDivideRun(left, right);
        }
    }

    public class OpDivideRun : ReadStringBase
    {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            private readonly IReadString left;
            private readonly IReadString right;
            public OpDivideRun(IReadString left, IReadString right)
            {
                this.left = left;
                this.right = right;
            }
            public override string Read(ModuleContext mc)
            {
                decimal left, right, result;
                if (!decimal.TryParse(this.left.Read(mc), out left)) return "";
                if (!decimal.TryParse(this.right.Read(mc), out right)) return "";
                if (right.Equals(0))
                {
                  logger.Error("instead of divide by zero, return NULL string");
                  return ""; // instead of divide by zero exception we return null string.
                }
                result = left / right;
                return result.ToString();

            }
    }
}
