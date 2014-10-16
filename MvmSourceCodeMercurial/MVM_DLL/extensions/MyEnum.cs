using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public static class MyEnum
    {
        public static int ToInt(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }
    }
}
