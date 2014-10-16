using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    class NumericFunctions
    {
        public static readonly string NEWLINE = "".AppendLine();

        #region perl like numeric functions

        /// <summary>
        /// absolute value function  
        /// </summary>
        /// <param name="value">Input</param>
        /// <returns>Returns the absolute value of its argument</returns>
        /// <category>Numeric Functions</category>
        [MvmExport("abs")]
        public static decimal substr(decimal value)
        {
            return System.Math.Abs(value);
        }
        
        #endregion

        # region other functions

        /// <summary>
        /// Performs modulus
        /// </summary>
        /// <returns>modulus</returns>
        /// <category>Numeric Functions</category>
        [MvmExport("mod")]
        public static string mod(string numerator, string denominator)
        {
            if(numerator.IsNullOrEmpty()) return "";
            if(denominator.IsNullOrEmpty()) return numerator;
            Decimal num=Decimal.Parse(numerator);
            Decimal den=Decimal.Parse(denominator);
            return (num % den).ToString();
        }

        #endregion
    }
}
