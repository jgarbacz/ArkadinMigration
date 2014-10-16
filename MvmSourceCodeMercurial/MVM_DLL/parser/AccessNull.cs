using System;
using System.Collections.Generic;

using System.Text;

using Antlr.Runtime.Tree;

namespace MVM
{
    public class AccessNull : BaseLiteral
    {
        override public IReadString SetupRead(SyntaxMaster SyntaxMaster, ITree tree)
        {
            return new LiteralString("");
        }
    }
}
