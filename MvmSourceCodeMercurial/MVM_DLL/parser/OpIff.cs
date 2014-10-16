using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime.Tree;

namespace MVM
{
    public class OpIff : BaseTernaryOpSetup
    {
       
    
        public override IReadString CreateRun(IReadString first, IReadString second, IReadString third)
        {
            return new OpIffRun(first, second, third);
        }
}

    public class OpIffRun : ReadStringBase
    {
            private readonly IReadString first;
            private readonly IReadString second;
            private readonly IReadString third;
            public OpIffRun(IReadString first, IReadString second, IReadString third)
            {
                this.first = first;
                this.second = second;
                this.third = third;
            }
            public override string Read(ModuleContext mc)
            {
                string myBool=this.first.Read(mc);
                if(myBool.Equals("1")){
                    return this.second.Read(mc);
                }
                if (myBool.Equals("0"))
                {
                    return this.third.Read(mc);
                }
                throw new Exception("Invalid boolean value ["+myBool+"]");
            }
    }
}
