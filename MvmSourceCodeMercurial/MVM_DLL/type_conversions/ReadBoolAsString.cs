using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    // 
    class ReadBoolAsString : IConvertReadable, IReadable
    {
        IReadBool readable;

        public ReadBoolAsString()
        {
        }

        public ReadBoolAsString(IReadable readable)
        {
            this.readable = (IReadBool)readable;
        }
        
        public IReadable ConvertReadable(IReadable readable)
        {
            return new ReadBoolAsString(readable);
        }

        public object ReadObject(ModuleContext mc)
        {
            bool b = this.readable.Read(mc);
            return b ? "1":"0";
        }

        public Type GetReadType()
        {
            return typeof(bool);
        }
       
    }
}