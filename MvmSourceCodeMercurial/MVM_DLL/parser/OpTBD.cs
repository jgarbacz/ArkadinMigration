using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime.Tree;

namespace MVM
{
    public class OpTBD : ISetupSyntax
    {

        #region ISetupSyntax Members

        public IReadString SetupRead(SyntaxMaster syntaxMaster, ITree tree)
        {
            throw new NotImplementedException();
        }

        public IReadString SetupWrite(SyntaxMaster syntaxMaster, ITree leftTree, ITree rightTree)
        {
            throw new NotImplementedException();
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
