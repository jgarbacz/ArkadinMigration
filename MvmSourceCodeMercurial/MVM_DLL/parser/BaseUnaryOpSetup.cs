using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime.Tree;

namespace MVM
{
    abstract public class BaseUnaryOpSetup:ISetupSyntax
    {
        abstract public IReadString CreateRun(IReadString left); 

        public IReadString SetupRead(SyntaxMaster syntaxMaster, Antlr.Runtime.Tree.ITree tree)
        {
            ITree leftTree = tree.GetChild(0);
            IReadString leftParsed = syntaxMaster.SetupRead(leftTree);
            return this.CreateRun(leftParsed);
        }


        public IReadString SetupWrite(SyntaxMaster syntaxMaster, ITree leftTree, ITree rightTree)
        {
            throw new Exception("Error, cannot write to Unary operator");
        }

        public IReadString SetupWrite(SyntaxMaster SyntaxMaster, Antlr.Runtime.Tree.ITree leftTree, IReadString right)
        {
            throw new Exception("Error, cannot write to binary operator");
        }

    }
}
