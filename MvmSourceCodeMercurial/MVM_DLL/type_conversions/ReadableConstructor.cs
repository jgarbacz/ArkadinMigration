using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    public class ReadableConstructor : IReadable
    {
        private System.Type type;
        public ReadableConstructor(System.Type type)
        {
            this.type = type;
        }
  
        public Type GetReadType()
        {
            return this.type;
        }

        public object ReadObject(ModuleContext mc)
        {
            return Activator.CreateInstance(type);
        }

    }
}

