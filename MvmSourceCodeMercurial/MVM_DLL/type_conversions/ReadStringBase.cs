using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    public abstract class ReadStringBase:IReadString
    {
        public abstract string Read(ModuleContext mc);

        public object ReadObject(ModuleContext mc)
        {
            return this.Read(mc);
        }

        public Type GetReadType()
        {
            return typeof(string);
        }
    }
}
