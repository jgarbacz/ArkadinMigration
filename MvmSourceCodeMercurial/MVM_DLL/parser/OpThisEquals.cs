using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime;
using Antlr.Runtime.Tree;


namespace MVM
{
    public class OpThisEquals:ISetupSyntax
    {
        private string op;
        public OpThisEquals(string op)
        {
            this.op = op;
        }
        
        #region ISetupSyntax Members

        public IReadString SetupRead(SyntaxMaster syntaxMaster, Antlr.Runtime.Tree.ITree tree)
        {
            ITree leftTree = tree.GetChild(0);
            ITree rightTree = tree.GetChild(1); 
            ITree assignTree=new CommonTree(new CommonToken(tree.Type,"="));
            ITree addTree = new CommonTree(new CommonToken(tree.Type, this.op));
            addTree.AddChild(leftTree);
            addTree.AddChild(rightTree);
            assignTree.AddChild(leftTree);
            assignTree.AddChild(addTree);
            //Console.WriteLine(assignTree.ToStringTree());
            IReadString v = syntaxMaster.SetupRead(assignTree);
            return v;
        }

        public IReadString SetupWrite(SyntaxMaster SyntaxMaster, Antlr.Runtime.Tree.ITree leftTree, Antlr.Runtime.Tree.ITree rightTree)
        {
            throw new Exception("unexpected");
        }

        #endregion

        #region ISetupSyntax Members


        public IReadString SetupWrite(SyntaxMaster SyntaxMaster, ITree leftTree, IReadString right)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
