using System;
using System.Collections.Generic;

using System.Text;

using Antlr.Runtime.Tree;

namespace MVM
{
    public class AccessNumber : BaseLiteral
    {
        override public IReadString SetupRead(SyntaxMaster SyntaxMaster, ITree tree)
        {
            string intValue = tree.GetChild(0).Text;
            return new LiteralString(intValue);
        }
    }
}
