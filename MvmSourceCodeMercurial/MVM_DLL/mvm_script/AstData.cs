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

    /// <summary>
    /// This class represents antlr data nodes as opposed to xml like nodes.
    /// </summary>
    public class AstData : AstNode
    {
        /// <summary>
        /// Override the default constructor
        /// </summary>
        /// <param name="t"></param>
        public AstData(IToken t,ILocation location)
            : base(t,location)
        {  
        }

        /// <summary>
        /// Returns the antlr text
        /// </summary>
        override public string Name
        {
            get
            {
                return this.Text;
            }
        }
       
    }
}
