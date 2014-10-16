using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using System.IO;
using MVM;

namespace MvmScript
{
    public class AstNodeAdaptor : CommonTreeAdaptor
    {
        public ILocation location;
        public AstNodeAdaptor()
        {
        }
        public AstNodeAdaptor(ILocation location)
        {
            this.location = location;
        }
        override public Object Create(IToken payload)
        {
            if (payload != null)
            {
                switch (payload.Type)
                {
                    case MvmScriptLexer.Ast_Element: return new AstElement(payload,this.location);
                    case MvmScriptLexer.Ast_NodeNamer: return new AstNodeNamer(payload,this.location);
                    case MvmScriptLexer.Ast_Value: return new AstValue(payload,this.location);
                }
            }
            return new AstData(payload,this.location); 
        }
    }
}
