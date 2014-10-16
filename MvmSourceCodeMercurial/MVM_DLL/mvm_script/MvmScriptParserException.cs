using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;

namespace MvmScript
{
    public abstract class MvmScriptParserException:System.ApplicationException
    {
        virtual public int LineNo { get; set; }
        virtual public IToken Token { get; set; }
        virtual public string Text { get; set;}
        virtual public int LinePosition { get; set; }
        virtual public ILocation FromLocation { get; set; }
    }
}
