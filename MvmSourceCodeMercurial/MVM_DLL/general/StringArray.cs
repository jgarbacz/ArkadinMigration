using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{

    // wrapper class for a string array so it can be put into a Dictionary to have compound keys.
    public class StringArray:IEquatable<StringArray>,IComparable<StringArray>
    {
        public readonly string[] array;
        private int h=0;
        
        public StringArray(int size)
        {
            this.array = new string[size];
        }

        public StringArray(IList<string> stringList)
        {
            this.array = new string[stringList.Count];
            for (int i = 0; i < this.array.Length; i++) this.array[i] = stringList[i];
        }

        public StringArray(params string[] strings)
        {
            this.array=strings;
        }

        
        public string this[int idx]
        {
            get
            {
                return this.array[idx];
            }
            set
            {
                this.array[idx] = value;
            }
        }

        public int Length
        {
            get
            {
                return this.array.Length;
            }
        }

        // approach is similiar to java.util.ArrayList.getHashCode()
        override public int GetHashCode()
        {
            if (h != 0) return h;
            h = 1;
            for (int i = 0; i < array.Length; i++)
            {
                string s = array[i];
                h = 31 * h + (s == null ? 0 : s.GetHashCode());
            }
            return h;
        }
        
        // Joins the list elements
        public string Join(string delim)
        {
            return this.array.Join(delim);
        }

        // returns true if all elements of this.array equal all elements of that.array
        public bool Equals(StringArray that)
        {
            if (this == that) return true;
            if (that == null) return false;
            if (this.array == null && that.array == null) return true;
            if (this.array == null || that.array == null) return false;
            if (this.array.Length != that.array.Length) return false;
            if (this.GetHashCode() != that.GetHashCode()) return false;
            for (int i = 0; i < this.array.Length; i++)
            {
                if (this.array[i] == null && that.array[i] == null) continue;
                if (this.array[i] == null || that.array[i] == null) return false;
                if (this.array[i].GetHashCode() != that.array[i].GetHashCode()) return false;
                if (!this.array[i].Equals(that.array[i])) return false;
            }
            return true;
        }

        public static void Test(){
            StringArray ab = new StringArray(new string[] { "a", "b" });
            StringArray AB = new StringArray(new string[] { "a", "b" });
            StringArray ba = new StringArray(new string[] { "b", "a" });
            Console.WriteLine(ab.GetHashCode());
            Console.WriteLine(AB.GetHashCode());
            Console.WriteLine(ba.GetHashCode());
            Console.WriteLine(ab.Equals(AB));
            Console.WriteLine(AB.Equals(ab));
            Console.WriteLine(ab.Equals(ba));
        }

        #region IComparable<StringArray> Members

        public int CompareTo(StringArray that)
        {
            int minLen = this.array.Length < that.array.Length ? this.array.Length : that.array.Length;
            for (int i = 0; i < minLen; i++)
            {
                string thisStr = this.array[i];
                string thatStr = that.array[i];
                //int cmp = thisStr.CompareTo(thatStr);
                int cmp = thisStr.CompareToValid(thatStr);
                if (cmp != 0) return cmp;
            }
            if (this.array.Length < that.array.Length) return -1;
            if (this.array.Length > that.array.Length) return 1;
            return 0;
        }

        #endregion
    }


}
