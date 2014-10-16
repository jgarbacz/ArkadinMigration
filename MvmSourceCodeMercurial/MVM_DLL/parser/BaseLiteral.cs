using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime.Tree;
namespace MVM
{
    abstract public class BaseLiteral:ISetupSyntax
    {
        abstract public IReadString SetupRead(SyntaxMaster SyntaxMaster, ITree tree);
        public IReadString SetupWrite(SyntaxMaster SyntaxMaster, ITree leftTree, ITree rightTree)
        {
            string v = leftTree.GetChild(0).Text;
            throw new Exception("Cannot write to literal value: [" + v+"]");
        }
        public IReadString SetupWrite(SyntaxMaster SyntaxMaster, ITree leftTree, IReadString right)
        {
            string v = leftTree.GetChild(0).Text;
            throw new Exception("Cannot write to literal value: [" + v + "]");
        }

    }
}
