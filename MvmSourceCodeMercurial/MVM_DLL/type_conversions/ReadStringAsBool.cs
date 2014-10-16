using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    // 
    class ReadStringAsBool : IConvertReadable, IReadable
    {
        IReadString readable;

        public ReadStringAsBool()
        {
        }

        public ReadStringAsBool(IReadable readable)
        {
		    this.readable=(IReadString)readable;
        }
        
        public IReadable ConvertReadable(IReadable readable)
        {
            return new ReadStringAsBool(readable);
        }

        public object ReadObject(ModuleContext mc)
        {
            string s = this.readable.Read(mc);
            if (s.Equals("1"))
                return true;
            else
                return (object) false;
        }

        public Type GetReadType()
        {
            return typeof(bool);
        }
       
    }
}