using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    // 
    class ReadStringAsInt : IConvertReadable, IReadable
    {
        IReadString readable;

        public ReadStringAsInt()
        {
        }

        public ReadStringAsInt(IReadable readable)
        {
		    this.readable=(IReadString)readable;
        }
        
        public IReadable ConvertReadable(IReadable readable)
        {
            return new ReadStringAsInt(readable);
        }

        public object ReadObject(ModuleContext mc)
        {
            string s = this.readable.Read(mc);
            return (object)int.Parse(s);
        }

        public Type GetReadType()
        {
            return typeof(int);
        }
       
    }
}