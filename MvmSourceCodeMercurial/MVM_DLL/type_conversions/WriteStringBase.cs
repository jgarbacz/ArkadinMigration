using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    public abstract class WriteStringBase:IWriteString
    {
        public abstract string Write(ModuleContext mc, string value);
        public abstract string Write(ModuleContext mc, IReadString value);

        public object WriteObject(ModuleContext mc, object value)
        {
            return Write(mc, (string)value);
        }

        public object WriteObject(ModuleContext mc, IReadable value)
        {
            return Write(mc, (IReadString)value);
        }

        public Type GetWriteType()
        {
            return typeof(string);
        }
    }
}
