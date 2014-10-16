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
    public class AstValue : AstNode
    {
         /// <summary>
        /// Override the default constructor
        /// </summary>
        /// <param name="t"></param>
        public AstValue(IToken t,ILocation location) :base(t,location)
        {  
        }

        /// <summary>
        /// Returns the text
        /// </summary>
        override public string Name
        {
            get
            {
                return this.GetChild(0).Text;
            }
        }

      
    }
}
