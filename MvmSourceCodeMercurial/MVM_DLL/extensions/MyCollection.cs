using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    public static class MyCollection
    {
        private static Random rng = new Random();
        public static ICollection<T> Shuffle<T>(this ICollection<T> c)
        {
            T[] a = new T[c.Count];
            c.CopyTo(a, 0);
            byte[] b = new byte[a.Length];
            rng.NextBytes(b);
            Array.Sort(b, a);
            return new List<T>(a);
        }
    }
}
