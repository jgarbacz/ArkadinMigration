using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    class StringArrayEqualityComparer:IEqualityComparer<string[]>
    {
        public static void TestHashing()
        {
            IEqualityComparer<string[]> comparer = new StringArrayEqualityComparer();
            Dictionary<string[], int> d = new Dictionary<string[], int>(comparer);
            string[] aa = new string[] { "a", "a" };
            string[] AA = new string[] { "a", "a" };
            string[] ab = new string[] { "a", "b" };

            Console.WriteLine(comparer.GetHashCode(aa));
            Console.WriteLine(comparer.GetHashCode(AA));
            Console.WriteLine(comparer.Equals(aa,AA));

            d[aa] = 1;
            d[ab] = 2;

            Console.WriteLine(d[aa]);
            Console.WriteLine(d[AA]);
            Console.WriteLine(d[ab]);
        }

        public bool Equals(string[] x, string[] y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            if (x.Length != y.Length) return false;
            if (GetHashCode(x) != GetHashCode(y)) return false;
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i].GetHashCode() != y[i].GetHashCode()) return false;
                if (!x[i].Equals(y[i])) return false;
            }
            return true;
        }

        // Uses the same algoritm that java.util.ArrayList uses
        public int GetHashCode(string[] obj)
        {
            int h=1;
            for (int i = 0; i < obj.Length; i++)
            {
                string s = obj[i];
                h = 31 * h + (s == null ? 0 : s.GetHashCode());
            }
            return h;
        }

      
       
    }
}
