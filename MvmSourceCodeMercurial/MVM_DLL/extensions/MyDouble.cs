using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    public static class MyDouble
    {

        public static string StripFractionalTrailingZeros(this double d)
        {
           return d.ToString().StripFractionalTrailingZeros();
        }

    }
}
