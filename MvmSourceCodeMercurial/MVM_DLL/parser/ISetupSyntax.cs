using System;
using System.Collections.Generic;

using System.Text;

using Antlr.Runtime.Tree;

namespace MVM
{
    public interface ISetupSyntax
    {
        // reads syntax tree 
        IReadString SetupRead(SyntaxMaster syntaxMaster, ITree tree);
        // leftTree=rightTree 
        IReadString SetupWrite(SyntaxMaster syntaxMaster, ITree leftTree, ITree rightTree);
        // leftTree=right.run()
        IReadString SetupWrite(SyntaxMaster SyntaxMaster, Antlr.Runtime.Tree.ITree leftTree, IReadString right);
    }
}
