using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime.Tree;

namespace MVM
{
    abstract public class BaseTernaryOpSetup:ISetupSyntax
    {
        abstract public IReadString CreateRun(IReadString first, IReadString second, IReadString third); 

        public IReadString SetupRead(SyntaxMaster syntaxMaster, Antlr.Runtime.Tree.ITree tree)
        {
            ITree firstTree = tree.GetChild(0);
            ITree secondTree = tree.GetChild(1);
            ITree thirdTree = tree.GetChild(2);
            IReadString firstParsed = syntaxMaster.SetupRead(firstTree);
            IReadString secondParsed = syntaxMaster.SetupRead(secondTree);
            IReadString thirdParsed = syntaxMaster.SetupRead(thirdTree);
            return this.CreateRun(firstParsed, secondParsed,thirdParsed);
        }

        public IReadString SetupWrite(SyntaxMaster SyntaxMaster, Antlr.Runtime.Tree.ITree leftTree, Antlr.Runtime.Tree.ITree rightTree)
        {
            throw new Exception("Error, cannot write to ternary operator");
        }
        public IReadString SetupWrite(SyntaxMaster SyntaxMaster, Antlr.Runtime.Tree.ITree leftTree, IReadString right)
        {
            throw new Exception("Error, cannot write to binary operator");
        }
    }
}
