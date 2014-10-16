using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{

    class WriteStringWithInt : IConvertWritable, IWritable
    {
        IWriteString writable;

        public WriteStringWithInt()
        {
        }

        public WriteStringWithInt(IWritable writable)
        {
		    this.writable=(IWriteString)writable;
        }
        
        public IWritable ConvertWritable(IWritable writable)
        {
            return new WriteStringWithInt(writable);
        }

        public object WriteObject(ModuleContext mc, object value)
        {
            int i = (int)value;
            string s = i.ToString();
            this.writable.Write(mc, s);
            return s;
        }

        public object WriteObject(ModuleContext mc, IReadable value)
        {
            int i = (int)value.ReadObject(mc);
            string s = i.ToString();
            this.writable.Write(mc, s);
            return s;
        }

        public Type GetWriteType()
        {
            return typeof(int);
        }

    }
}