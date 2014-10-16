using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
   public class MyComparers
    {
        public static void Test()
        {
            string[] bob=new string[2];
            object[] ooo = bob;
            IComparer<object>[] comparers = new IComparer<object>[2];
            comparers[0] = new ObjectStringCompareDesc();
            comparers[1] = new ObjectDoubleCompareAsc();
            IComparer<object[]> myCmp = new ObjectArrayCompare(comparers);
            List<object[]> list = new List<object[]>();
            list.Add(new object[] { "a", 2 });
            list.Add(new object[] { "a", 1 });
            list.Add(new object[] { "b", 99.999 });
            list.Add(new object[] { "b", 99.98 });
            list.Add(new object[] { "b", -1});
            list.Sort(myCmp);
            list.ForEach(x => Console.WriteLine(x[0] + "," + x[1]));
        }
        public class ObjectDoubleCompareAsc : IComparer<object>
        {
            public int Compare(object x, object y)
            {
                var tx = System.Convert.ToDouble(x);
                var ty = System.Convert.ToDouble(y);
                return tx.CompareTo(ty);
            }
        }
        public class ObjectDoubleCompareDesc : IComparer<object>
        {
            public int Compare(object x, object y)
            {
                var tx = System.Convert.ToDouble(x);
                var ty = System.Convert.ToDouble(y);
                int asc = tx.CompareTo(ty);
                if (asc > 0) return -1;
                if (asc < 0) return 1;
                return 0;
            }
        }
        public class ObjectStringCompareAsc : IComparer<object>
        {
            public int Compare(object x, object y)
            {
                var tx = System.Convert.ToString(x);
                var ty = System.Convert.ToString(y);
                return tx.CompareTo(ty);
            }
        }
        public class ObjectStringCompareDesc : IComparer<object>
        {
            public int Compare(object x, object y)
            {
                var tx = System.Convert.ToString(x);
                var ty = System.Convert.ToString(y);
                int asc = tx.CompareTo(ty);
                if (asc > 0) return -1;
                if (asc < 0) return 1;
                return 0;
            }
        }
        public class ObjectArrayCompare : IComparer<object[]>
        {
            IComparer<object>[] comparers;
            public ObjectArrayCompare(IComparer<object>[] comparers)
            {
                this.comparers = comparers;
            }
            public int Compare(object[] x, object[] y)
            {
                for (int i = 0; i < x.Length; i++)
                {
                    IComparer<object> cmp = comparers[i];
                    int val = cmp.Compare(x[i], y[i]);
                    if (val != 0) return val;
                }
                return 0;
            }
        }

    }
}
