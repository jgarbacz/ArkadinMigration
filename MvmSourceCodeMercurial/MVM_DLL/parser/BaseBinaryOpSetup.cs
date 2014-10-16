using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime.Tree;

namespace MVM
{
    abstract public class BaseBinaryOpSetup:ISetupSyntax
    {
        abstract public IReadString CreateRun(IReadString left, IReadString right); 

        public IReadString SetupRead(SyntaxMaster syntaxMaster, Antlr.Runtime.Tree.ITree tree)
        {
            ITree leftTree = tree.GetChild(0);
            ITree rightTree = tree.GetChild(1);
            IReadString leftParsed = syntaxMaster.SetupRead(leftTree);
            IReadString rightParsed = syntaxMaster.SetupRead(rightTree);
            return this.CreateRun(leftParsed, rightParsed);
        }

        public IReadString SetupWrite(SyntaxMaster SyntaxMaster, Antlr.Runtime.Tree.ITree leftTree, Antlr.Runtime.Tree.ITree rightTree)
        {
            throw new Exception("Error, cannot write to binary operator");
        }

        public IReadString SetupWrite(SyntaxMaster SyntaxMaster, Antlr.Runtime.Tree.ITree leftTree, IReadString right)
        {
            throw new Exception("Error, cannot write to binary operator");
        }
    }
}
