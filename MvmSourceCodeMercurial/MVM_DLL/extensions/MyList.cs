using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{

    // extension methods for List<T>
    static class MyList
    {

        /// <summary>
        /// Creates a list that includes the passed item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static List<T> AsList<T>(this T item)
        {
            List<T> list = new List<T>();
            list.Add(item);
            return list;
        }

        /// <summary>
        /// Removes the last element of the list and returns true if it had an element
        /// to RemoveSpecificItem.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool RemoveLast<T>(this List<T> list)
        {
            if (list == null) return false;
            if (list.Count == 0) return false;
            list.RemoveAt(list.Count - 1);
            return true;
        }

        /// <summary>
        /// Sets the item at the passed idx, extending the list if necessary, pads the list out with the default value for the type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static void Set<T>(this List<T> list, int index, T item)
        {
            if (list == null) return;
            int needs =  (index + 1) - list.Count;
            for (int i = 0; i < needs; i++) list.Add(default(T));
            list[index] = item;
        }

        /// <summary>
        /// Sets the item at the passed idx, extending the list if necessary, pads the list out with the passed default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static void Set<T>(this List<T> list, int index, T item, T defaultValue)
        {
            if (list == null) return;
            int needs = (index + 1) - list.Count;
            for (int i = 0; i < needs; i++) list.Add(defaultValue);
            list[index] = item;
        }

        /// <summary>
        /// If index is valid return the value at the index, else the defaultValue. Does not expand the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetValueDefaulted<T>(this List<T> list, int index, T defaultValue)
        {
            if (index < list.Count) return list[index];
            return defaultValue;
        }



        /// <summary>
        /// If index is valid return the value at the index, else the default value for the type. Does not expand the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetValueDefaulted<T>(this List<T> list, int index)
        {
            if (index < list.Count) return list[index];
            return default(T);
        }

        /// <summary>
        /// Makes sure the list has atleast the minimum count. pads the list out with the passed default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="count"></param>
        /// <param name="defaultValue"></param>
        public static void SetMinCount<T>(this List<T> list, int count,T defaultValue)
        {
            if (list == null) return;
            int needs = count - list.Count;
            if (needs <= 0) return;
            for (int i = 0; i < needs; i++) list.Add(defaultValue);
        }
       
        /// <summary>
        /// Returns the max index of a list which is list.count-1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int MaxIndex<T>(this List<T> list)
        {
            return list.Count - 1;
        }

        public static List<T> GetDistinct<T>(this List<T> list)
        {
            List<T> output = new List<T>(list.Count);
            foreach (var elem in list.Distinct())
            {
                output.Add(elem);
            }
            return output;
        }

        /// <summary>
        /// Returns a new list which is the reverse of the current list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> GetReverse<T>(this List<T> list)
        {
            List<T> output = new List<T>(list.Count);
            for (int i = list.Count-1; i >=0; i--)
            {
                output.Add(list[i]);
            }
            return output;
        }

        // swaps all elements with the passed value 
        public static List<T> SwapAll<T>(this List<T> list, T swapValue)
        {
            List<T> output = new List<T>();
            foreach (T elem in list)
            {
                output.Add(swapValue);
            }
            return output;
        }


        // prefix all elements with the prefix 
        public static List<string> PrefixAll<T>(this List<T> list, string prefix)
        {
            List<string> output = new List<string>();
            foreach (T elem in list)
            {
                output.Add(prefix + elem.ToString());
            }
            return output;
        }

        // Surrounds all elements with the prefix and the suffix returning a new string list
        public static List<string> SurroundAll<T>(this List<T> list, string prefix, string suffix)
        {
            List<string> output = new List<string>();
            foreach (T elem in list)
            {
                output.Add(prefix + elem.ToString() + suffix);
            }
            return output;
        }

        // Joins elements together with the delimiter
        public static string Join<T>(this IList<T> list, char delim)
        {
            return list.Join(Convert.ToString(delim));
        }

        // Joins elements together with no delimiter
        public static string Join<T>(this IList<T> list)
        {
            return list.Join("");
        }

        // Joins elements together with a delimiter
        public static string Join<T>(this IList<T> list, string delim)
        {
            if (list.Count <= 0) return "";
            StringBuilder output = new StringBuilder();
            foreach (T elem in list)
            {
                if (elem == null) output.Append("null");
                else output.Append(elem.ToString());
                output.Append(delim);
            }
            return output.ToString().Substring(0, output.Length - delim.Length);
        }

        // Joins elements together with a leading delimiter
        public static string JoinWithLeadingDelim<T>(this IList<T> list, string delim)
        {
            StringBuilder output = new StringBuilder();
            foreach (T elem in list)
            {
                output.Append(delim);
                output.Append(elem.ToString());
            }
            return output.ToString();
        }

        // Joins elements together with a trailing delimiter
        public static string JoinWithTrailingDelim<T>(this IList<T> list, string delim)
        {
            StringBuilder output = new StringBuilder();
            foreach (T elem in list)
            {
                output.Append(elem.ToString());
                output.Append(delim);
            }
            return output.ToString();
        }

        // Joins elements together with both a leading and a trailing delimiter
        public static string JoinWithLeadingAndTrailingDelim<T>(this IList<T> list, string delim)
        {
            if (list.Count == 0) return "";
            StringBuilder output = new StringBuilder(delim);
            foreach (T elem in list)
            {
                output.Append(elem.ToString());
                output.Append(delim);
            }
            return output.ToString();
        }

        // Writes the list out as lines
        public static void WriteLines<T>(this IList<T> list)
        {
            foreach (T elem in list)
            {
                Console.WriteLine(elem.ToString());
            }
        }

        // Returns a new list which is the intersection of the passed lists
        public static List<T> Intersection<T>(this List<T> aList, List<T> bList)
        {
            List<T> output = new List<T>();
            foreach (T a in aList)
            {
                if (bList.Contains(a)) output.Add(a);
            }
            //Console.WriteLine("INTERSECTION:"+XList.Join(",", output));
            return output;
        }

        // Returns a new list which is the superset of the passed lists, (removes dupes)
        public static List<T> Superset<T>(this List<T> aList, List<T> bList)
        {
            List<T> output = new List<T>();
            foreach (T a in aList)
            {
                if (!output.Contains(a)) output.Add(a);
            }
            foreach (T b in bList)
            {
                if (!output.Contains(b)) output.Add(b);
            }
            return output;
        }

        // Returns a new list which is the superset of all passed lists, (removes dupes)
        public static List<T> Superset<T>(this List<T> aList, params List<T>[] moreLists)
        {
            List<T> output = new List<T>();
            foreach (T x in aList) if (!output.Contains(x)) output.Add(x);
            foreach (List<T> bList in moreLists)
            {
                foreach (T x in bList) if (!output.Contains(x)) output.Add(x);
            }
            return output;
        }

        // binary searches the list (assumes sorted and default comparator) inserts the object
        // and return the index where it inserted it.
        public static int InsertSorted<T>(this List<T> list, T value)where T:IComparable<T>
        {
            int idx = list.BinarySearchForInsertIdx(value);
            list.Insert(idx, value);
            return idx;
        }

        // Returns the index where we should insert the value into the sorted list
        // uses the default comparator
        public static int BinarySearchForInsertIdx<T>(this List<T> list, T value) where T:IComparable<T>
        {
            int low = 0; 
            int high=list.Count - 1;
            while (low <= high)
            {
                int mid = (low + high) / 2;
                int test = value.CompareTo(list[mid]);
                if (test < 0) high = mid - 1;
                else if (test > 0) low = mid + 1;
                else return mid;
            }
            return high > low ? high : low;
        }




        public static List<T> CopyShallow<T>(this List<T> list)
        {
            List<T> copy = new List<T>();
            copy.AddRange(list);
            return copy;
        }

        public static void Tests(){
            List<int> list = new List<int>();
            list.InsertSorted(8);
            list.InsertSorted(9);
            list.InsertSorted(7);
            list.InsertSorted(7);
            list.InsertSorted(1);
            list.InsertSorted(20);
            list.InsertSorted(3);
            list.InsertSorted(4);
            Console.WriteLine(list.Join(","));
            Console.ReadKey();
        }
    }

}
