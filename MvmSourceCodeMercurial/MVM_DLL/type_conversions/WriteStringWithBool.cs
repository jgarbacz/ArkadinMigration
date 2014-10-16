using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{

    class WriteStringWithBool : IConvertWritable, IWritable
    {
        IWriteString writable;

        public WriteStringWithBool()
        {
        }

        public WriteStringWithBool(IWritable writable)
        {
		    this.writable=(IWriteString)writable;
        }
        
        public IWritable ConvertWritable(IWritable writable)
        {
            return new WriteStringWithBool(writable);
        }

        public object WriteObject(ModuleContext mc, object value)
        {
            bool b = (bool)value;
            string s = b ? "1" : "0";
            this.writable.Write(mc, s);
            return s;
        }

        public object WriteObject(ModuleContext mc, IReadable value)
        {
            bool b = (bool)value.ReadObject(mc);
            string s = b ? "1" : "0";
            this.writable.Write(mc, s);
            return s;
        }

        public Type GetWriteType()
        {
            return typeof(bool); // dunno if this is right?
        }

    }
}