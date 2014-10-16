using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGenerics.DataStructures.Trees;
using NGenerics.Patterns.Visitor;

namespace MVM
{

    /// <summary>
    /// Entension methods for <code>RedBlackTree<K,LinkedList<V>></code>.
    /// </summary>
    static class MyRedBlackTreeOfLinkedList
    {
        
        /// <summary>
        /// Removes and returns the first item from the first key in the btree.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="LinkedList"></param>
        /// <param name="item"></param>
        public static bool TryDequeue<K,V>(this RedBlackTree<K,LinkedList<V>> btree, out K key, out V value)
        {
            if (btree.IsEmpty)
            {
                key=default(K);
                value = default(V);
                return false;
            }
            var iter = btree.GetOrderedEnumerator();
            while (iter.MoveNext())
            {
                var kv = iter.Current;
                key = kv.Key;
                LinkedList<V> dupeQueue = kv.Value;
                if (dupeQueue.Count > 0)
                {
                    value = dupeQueue.Dequeue();
                    if (dupeQueue.Count == 0) btree.Remove(key);
                    return true;
                }
                btree.Remove(key);
            }
            key = default(K);
            value = default(V);
            return false;
        }

        /// <summary>
        /// Removes and returns the first item from the first key in the btree.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="LinkedList"></param>
        /// <param name="item"></param>
        public static bool TryDequeue<K, V>(this RedBlackTree<K, LinkedList<V>> btree, out KeyValuePair<K,V> keyValuePair)
        {
            if (btree.IsEmpty)
            {
                keyValuePair = default(KeyValuePair<K, V>);
                return false;
            }
            var iter = btree.GetOrderedEnumerator();
            while (iter.MoveNext())
            {
                var kv = iter.Current;
                var key = kv.Key;
                LinkedList<V> dupeQueue = kv.Value;
                if (dupeQueue.Count > 0)
                {
                    var value = dupeQueue.Dequeue();
                    if (dupeQueue.Count == 0) btree.Remove(key);
                    keyValuePair = new KeyValuePair<K, V>(key,value);
                    return true;
                }
                btree.Remove(key);
            }
            keyValuePair = default(KeyValuePair<K, V>);
            return false;
        }

        /// <summary>
        /// Lets you look a the next ordered value without dequeuing it
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="btree"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Peek<K, V>(this RedBlackTree<K, LinkedList<V>> btree, out K key, out V value)
        {
            if (btree.IsEmpty)
            {
                key = default(K);
                value = default(V);
                return false;
            }
            var iter = btree.GetOrderedEnumerator();
            while (iter.MoveNext())
            {
                var kv = iter.Current;
                key = kv.Key;
                LinkedList<V> dupeQueue = kv.Value;
                if (dupeQueue.Count > 0)
                {
                    value = dupeQueue.Peek();
                    return true;
                }
            }
            key = default(K);
            value = default(V);
            return false;
        }

        /// <summary>
        /// Lets you look a the next ordered value without dequeuing it
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="btree"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Peek<K, V>(this RedBlackTree<K, LinkedList<V>> btree, out KeyValuePair<K,V> keyValuePair)
        {
            if (btree.IsEmpty)
            {
                keyValuePair = default(KeyValuePair<K, V>);
                return false;
            }
            var iter = btree.GetOrderedEnumerator();
            while (iter.MoveNext())
            {
                var kv = iter.Current;
                var key = kv.Key;
                LinkedList<V> dupeQueue = kv.Value;
                if (dupeQueue.Count > 0)
                {
                    var value = dupeQueue.Peek();
                    keyValuePair=new KeyValuePair<K, V>(key, value);
                    return true;
                }
            }
            keyValuePair = default(KeyValuePair<K, V>);
            return false;
        }

        public static void Enqueue<K,V>(this RedBlackTree<K, LinkedList<V>> btree, K key, V node)
        {
            LinkedList<V> dupeQueue;
            if (btree.TryGetValue(key, out dupeQueue))
            {
                dupeQueue.Enqueue(node);
            }
            else
            {
                dupeQueue = new LinkedList<V>();
                dupeQueue.Enqueue(node);
                btree.Add(key, dupeQueue);
            }
        }



        public static void Enqueue<K, V>(this RedBlackTree<K, LinkedList<V>> btree, KeyValuePair<K, V> keyValuePair)
        {
            var key = keyValuePair.Key;
            var node = keyValuePair.Value;
            LinkedList<V> dupeQueue;
            if (btree.TryGetValue(key, out dupeQueue))
            {
                dupeQueue.Enqueue(node);
               // Console.WriteLine("TREELEN="+dupeQueue.Count);
            }
            else
            {
                dupeQueue = new LinkedList<V>();
                dupeQueue.Enqueue(node);
                btree.Add(key, dupeQueue);
            }
           // Console.WriteLine("TREELEN=" + dupeQueue.Count);
        }

       
        /// <summary>
        /// Walks through the values in order.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<V> GetOrderedValues<K, V>(this RedBlackTree<K, LinkedList<V>> btree)
        {
            var btreeLooper= btree.GetOrderedEnumerator();
            while(btreeLooper.MoveNext()){
              KeyValuePair<K,LinkedList<V>> x= btreeLooper.Current;
                foreach (var y in x.Value)
                {
                    yield return y;
                }
            }
        }

        /// <summary>
        /// Walks through the keys in order.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<K> GetOrderedKeys<K, V>(this RedBlackTree<K, LinkedList<V>> btree)
        {
            var btreeLooper = btree.GetOrderedEnumerator();
            while (btreeLooper.MoveNext())
            {
                yield return btreeLooper.Current.Key;
            }
}

    }
}
