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
    public class AstNodeNamer : AstNode
    {

        /// <summary>
        /// Override the default constructor
        /// </summary>
        /// <param name="t"></param>
        public AstNodeNamer(IToken t,ILocation location)
            : base(t, location)
        {  
        }

        /// <summary>
        /// Returns the attribute name
        /// </summary>
        override public string Name
        {
            get
            {
                return this.GetChild(0).Text;
            }
        }

        /// <summary>
        /// Returns the attribute value
        /// </summary>
        new public AstNode Value
        {
            get
            {
                return (AstNode)this.GetChild(0).GetChild(0);
            }
        }

        
    }
}
