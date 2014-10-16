using System;
using System.Collections.Generic;

using System.Text;

using Antlr.Runtime.Tree;

namespace MVM
{
    public class AccessString : BaseLiteral
    {
        override public IReadString SetupRead(SyntaxMaster SyntaxMaster, ITree tree)
        {
            string stringValue = tree.GetChild(0).Text;
            stringValue = stringValue.StripQuotes();
            return new LiteralString(stringValue);
        }
    }
}
