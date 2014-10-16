using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime.Tree;

namespace MVM
{
    public class OpAssign:ISetupSyntax
    {
        #region ISetupSyntax Members

        public IReadString SetupRead(SyntaxMaster syntaxMaster, Antlr.Runtime.Tree.ITree tree)
        {
            ITree leftTree = tree.GetChild(0);
            ITree rightTree = tree.GetChild(1);
            IReadString v = syntaxMaster.SetupWrite(leftTree, rightTree);
            return new Assign(v);
        }

        public IReadString SetupWrite(SyntaxMaster SyntaxMaster, Antlr.Runtime.Tree.ITree leftTree, Antlr.Runtime.Tree.ITree rightTree)
        {
            throw new Exception("unexpected");
        }

        #endregion

        #region ISetupSyntax Members


        public IReadString SetupWrite(SyntaxMaster SyntaxMaster, ITree leftTree, IReadString right)
        {
            throw new Exception("unexpected");
        }

        #endregion
    }

    class Assign : ReadStringBase
    {
        private readonly IReadString v;
        public Assign(IReadString v)
        {
            this.v = v;
        }

        #region IModuleRecurse Members

        public override string Read(ModuleContext mc)
        {
            return v.Read(mc);
        }

        #endregion
    }

}
