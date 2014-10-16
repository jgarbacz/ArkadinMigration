using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace MVM
{

    public class IndexValuePair<T>
    {
        public int index;
        public T value;
        public IndexValuePair(int index, T value)
        {
            this.index = index;
            this.value = value;
        }
    }
    public static class MyEnumerable
    {
        /// <summary>
        /// Adds an index to the iterator which can be accessed by .index and value by .value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IEnumerable<IndexValuePair<T>> SelectIndexValuePairs<T>(this IEnumerable<T> collection)
        {
            int index = 0;
            foreach (T value in collection)
                yield return new IndexValuePair<T>(index++, value);
        }

        /// <summary>
        /// Concatinate an item to the end of a sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> first, T item)
        {
            return first.Concat(item.AsEnumerable());
        }

        /// <summary>
        /// Makes a single item enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IEnumerable<T> AsEnumerable<T>(this T item)
        {
            yield return item;
        }

        /// <summary>
        /// True if there are no results. Same as <code>!this.Any()</code> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool None<T>(this IEnumerable<T> collection)
        {
            return !collection.Any();
        }

        public static string JoinStrings<T>(this IEnumerable<T> collection)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var elem in collection)
                sb.Append(elem.ToString());
            return sb.ToString();
        }
        public static string JoinLines<T>(this IEnumerable<T> collection)
        {
            return collection.JoinStrings("".AppendLine());
        }
        public static string JoinStrings<T>(this IEnumerable<T> collection, string delim)
        {
            if (collection == null) return "";
            IEnumerator<T> enumerator = collection.GetEnumerator();
            if (!enumerator.MoveNext()) return "";
            StringBuilder sb=new StringBuilder();
            for (; ; )
            {
                sb.Append(enumerator.Current.ToString());
                if (enumerator.MoveNext()) sb.Append(delim);
                else break;
            }
            return sb.ToString();
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (T item in collection)
                action(item);
        }

        /// <summary>
        /// Checks whether a collection has same newOrder contents as another collection
        /// </summary>
        /// <param name="value">The current instance object</param>
        /// <param name="compareList">The collection to compare with</param>
        /// <param name="comparer">The comparer object to use to compare each item in the collection.  If null uses EqualityComparer(T).Default</param>
        /// <returns>True if the two collections contain all the same items in the same newOrder</returns>
        public static bool IsEqualTo<TSource>(this IEnumerable<TSource> value, IEnumerable<TSource> compareList, IEqualityComparer<TSource> comparer)
        {
            if (value == compareList)
            {
                return true;
            }
            else if (value == null || compareList == null)
            {
                return false;
            }
            else
            {
                if (comparer == null)
                {
                    comparer = EqualityComparer<TSource>.Default;
                }

                IEnumerator<TSource> enumerator1 = value.GetEnumerator();
                IEnumerator<TSource> enumerator2 = compareList.GetEnumerator();

                bool enum1HasValue = enumerator1.MoveNext();
                bool enum2HasValue = enumerator2.MoveNext();

                try
                {
                    while (enum1HasValue && enum2HasValue)
                    {
                        if (!comparer.Equals(enumerator1.Current, enumerator2.Current))
                        {
                            return false;
                        }

                        enum1HasValue = enumerator1.MoveNext();
                        enum2HasValue = enumerator2.MoveNext();
                    }

                    return !(enum1HasValue || enum2HasValue);
                }
                finally
                {
                    if (enumerator1 != null) enumerator1.Dispose();
                    if (enumerator2 != null) enumerator2.Dispose();
                }
            }
        }

        /// <summary>
        /// Checks whether a collection has same newOrder contents as another collection
        /// </summary>
        /// <param name="value">The current instance object</param>
        /// <param name="compareList">The collection to compare with</param>
        /// <returns>True if the two collections contain all the same items in the same newOrder</returns>
        public static bool IsEqualTo<TSource>(this IEnumerable<TSource> value, IEnumerable<TSource> compareList)
        {
            return IsEqualTo(value, compareList, null);
        }

        /// <summary>
        /// Checks whether a collection has same newOrder contents as another collection
        /// </summary>
        /// <param name="value">The current instance object</param>
        /// <param name="compareList">The collection to compare with</param>
        /// <returns>True if the two collections contain all the same items in the same newOrder</returns>
        public static bool IsEqualTo(this IEnumerable value, IEnumerable compareList)
        {
            return IsEqualTo<object>(value.OfType<object>(), compareList.OfType<object>());
        }

    }

}
