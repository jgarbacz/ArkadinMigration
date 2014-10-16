using System;
using System.Collections.Generic;
using System.Text;
using Antlr.Runtime.Tree;

namespace MVM
{
    public interface ISetupWritable
    {
        IWriteString SetupWritable(SyntaxMaster syntaxMaster, ITree leftTree);
    }
}
