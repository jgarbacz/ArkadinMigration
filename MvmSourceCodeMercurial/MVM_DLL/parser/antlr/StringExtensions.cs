using System;
using System.Collections.Generic;

using System.Text;

namespace MyExtensions
{
    static class StringExtensions
    {
        /// <summary>
        /// Repeats the string n times and returns it.
        /// </summary>
        /// <param name="s">string to repeat</param>
        /// <param name="n">number of times to repeat it</param>
        /// <returns>repeated string</returns>
        public static string repeat(this string s, int n){
            if (n <= 0) return "";
            StringBuilder sb=new StringBuilder(s.Length*n);
            for(int i=0;i<n;i++) sb.Append(s);
            return sb.ToString();
        }
    }
}
