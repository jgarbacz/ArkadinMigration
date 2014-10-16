using System;
using System.Collections.Generic;

using System.Text;

using Antlr.Runtime.Tree;

namespace MVM
{
    public class AccessBool : BaseLiteral
    {
        override public IReadString SetupRead(SyntaxMaster SyntaxMaster, ITree tree)
        {
            string v = tree.GetChild(0).Text.ToLower();
            string myV = v.Equals("true") ? "1" : "0";
            return new LiteralString(myV);
        }
    }
}
