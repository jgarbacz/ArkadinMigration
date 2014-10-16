using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public class DynamicParser<T>
       where T : IDynamicParser, new()
    {
        public static readonly T Default = new T();
    }
}
