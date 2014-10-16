using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    public static class MyDecimal
    {

        public static string StripFractionalTrailingZeros(this decimal d)
        {
           return d.ToString().StripFractionalTrailingZeros();
        }

    }
}
