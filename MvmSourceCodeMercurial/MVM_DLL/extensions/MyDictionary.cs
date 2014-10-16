using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    public static class MyDictionary
    {

        public static void AddAllDeep<K, V>(this IDictionary<K, V> thisDictionary, IDictionary<K, V> dictionary)
            where K:ICloneable
            where V:ICloneable
        {
            foreach (var entry in dictionary) 
            {
                K kClone = (K)entry.Key.Clone();
                V vClone = (V)entry.Value.Clone();
                thisDictionary[kClone] = vClone;
            }
        }
        

        //public static void AddAll<K, V>(this IDictionary<K, V> thisDictionary,  IDictionary<K, V> dictionary)
        public static void AddAll<K, V>(this IDictionary<K, V> thisDictionary,  IEnumerable<KeyValuePair<K, V>> dictionary)
        {
            foreach (var entry in dictionary) 
                thisDictionary[entry.Key] = entry.Value;
        }

        public static bool TryGetRemoveValue<K, V>(this IDictionary<K, V> dictionary, K key, out V value)
        {
            if (dictionary.TryGetValue(key, out value))
            {
                dictionary.Remove(key);
                return true;
            }
            return false;
        }

        public static bool ContainsNonNullOrEmptyValue<K>(this IDictionary<K, string> dictionary, K key)
        {
            string value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value.NotNullOrEmpty();
            }
            return false;
        }

        /// <summary>
        /// Inserts into dictionary if key does not already exists. Returns true if inserted, else false.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool InsertIfNone<K, V>(this IDictionary<K, V> dictionary, K key, V value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                return true;
            }
            return false;
        }

        public static Dictionary<K, V> CopyShallow<K, V>(this IDictionary<K, V> dictionary)
        {
            Dictionary<K, V> copy = new Dictionary<K, V>(dictionary.Count);
            foreach(var entry in dictionary){
                copy[entry.Key]=entry.Value;
            }
            return copy;
        }

        public static K GetFirstKey<K, V>(this IDictionary<K, V> dictionary)
        {
            K firstKey = default(K);
            foreach (K key in dictionary.Keys)
            {
                firstKey=key;
                break;
            }
            return firstKey;
        }

        public static V GetFirstValue<K, V>(this IDictionary<K, V> dictionary)
        {
            V firstValue = default(V);
            foreach (V value in dictionary.Values)
            {
                firstValue = value;
                break;
            }
            return firstValue;
        }

        /// <summary>
        /// Returns true if the contents of d1 and d2 are the same.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool SameAs<K, V>(this IDictionary<K, V> d1, IDictionary<K, V> d2)
        {
            if (d1 == null && d2 == null) return true;
            if (d1 == null || d2==null) return false;
            if (d1.Count!=d2.Count) return false;
            foreach (var k in d1.Keys)
            {
                if (!d1[k].Equals(d2[k])) return false;
            }
            return true;
        }

        public static IDictionary<K, V> AsDictionary<K, V>(this IEnumerable<KeyValuePair<K, V>> KeyValuePairEnumerable)
        {
            if(KeyValuePairEnumerable==null)return null;
            IDictionary<K, V> output=KeyValuePairEnumerable as  IDictionary<K, V>;
            if(output!=null) return output;
            output=new Dictionary<K,V>();
            output.AddAll(KeyValuePairEnumerable);
            return output;
        }

        /// <summary>
        /// Returns true if the contents of d1 and d2 are the same.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool SameKeyValuesAs<K, V>(this IDictionary<K, V> d1, IEnumerable<KeyValuePair<K, V>> d2)
        {
            if (d1 == null && d2 == null) return true;
            if (d1 == null || d2 == null) return false;
            int d2Count = 0;
            foreach (var kv2 in d2)
            {
                d2Count++;
                if (d2Count > d1.Count) return false;
                V d1v;
                if(!d1.TryGetValue(kv2.Key,out d1v)){
                    return false;
                }
                if (!d1v.Equals(kv2.Value)) return false;
            }
            if (d2Count != d1.Count) return false;
            return true;
        }


        /// <summary>
        /// Returns true if values of d1 and d2 are the same excluding the excude keys.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <param name="exclude"></param>
        /// <returns></returns>
        public static bool SameAsExcluding<K, V>(this IDictionary<K, V> d1, IDictionary<K, V> d2, IDictionary<K, V> exclude)
        {
            if (exclude == null||exclude.Count==0) return d1.SameAs(d2);
            if (d1 == null && d2 == null) return true;
            if (d1 == null || d2 == null) return false;
            if (d1.Count != d2.Count) return false;
            foreach (var k in d1.Keys)
            {
                if (exclude.ContainsKey(k)) continue;
                if (!d1[k].Equals(d2[k])) return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if values of d1 and d2 are the same for the include keys.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public static bool SameAsIncluding<K, V>(this IDictionary<K, V> d1, IDictionary<K, V> d2, IDictionary<K, V> include)
        {
            if (include == null) return true;
            if (d1 == null && d2 == null) return true;
            if (d1 == null || d2 == null) return false;
            if (d1.Count != d2.Count) return false;
            foreach (var k in include.Keys)
            {
                bool d1HasKey=d1.ContainsKey(k);
                bool d2HasKey=d2.ContainsKey(k);
                if (d1HasKey && d2HasKey)
                {
                    if (!d1[k].Equals(d2[k])) return false;
                    else continue;
                }
                if (d1HasKey || d2HasKey) return false;
            }
            return true;
        }


        /// <summary>
        /// Returns true if contents of d1 and d2 are the same taking into account include and exclude.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <param name="include"></param>
        /// <param name="exclude"></param>
        /// <returns></returns>
        public static bool SameAs<K, V>(this IDictionary<K, V> d1, IDictionary<K, V> d2, IDictionary<K, V> include, IDictionary<K, V> exclude)
        {
            if (include == null)
            {
                if (exclude == null || exclude.Count == 0) return d1.SameAs(d2);
                else return d1.SameAsExcluding(d2, exclude);
            }
            else if (exclude==null || exclude.Count==0)
            {
               return d1.SameAsIncluding(d2, include);
            }
            foreach (var k in include.Keys){
                if (exclude.ContainsKey(k)) continue;
                if(d1[k].Equals(d2[k])) return false; 
            }
            return true;
        }

        public static V GetValueDefaulted<K, V>(this IDictionary<K, V> dictionary, K key, V defaultValue)
        {
            V value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }
            return defaultValue;
        }

        public static V GetAddValueDefaulted<K, V>(this IDictionary<K, V> dictionary, K key, V defaultValue)
        {
            V value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }
            dictionary[key] = defaultValue;
            return defaultValue;
        }

        public static V GetValueOrNull<K, V>(this IDictionary<K, V> dictionary, K key)
        {
            V value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }
            return default(V);
        }
    }
}
