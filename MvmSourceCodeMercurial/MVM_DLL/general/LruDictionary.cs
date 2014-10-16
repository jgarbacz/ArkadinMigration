using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    /// <summary>
    /// Provides a dictionary with a finite capacity that keeps its most recently read or written items. 
    /// Subclass can override <code>FlushItem()</code> to fire a method call when items are flushed from cache.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public abstract class LruDictionary<K, V>:IDictionary<K,V>
    {
        #region Intermal members
        /// <summary>
        /// Max items in the LRU
        /// </summary>
        private int capacity;

        /// <summary>
        /// This is the dictionary for the cache
        /// </summary>
        private Dictionary<K, LinkedListNode<LruDictionaryItem>> map = new Dictionary<K, LinkedListNode<LruDictionaryItem>>();

        /// <summary>
        /// Provides the LRU portion where last item in the list is the most recently used.
        /// </summary>
        private LinkedList<LruDictionaryItem> list = new LinkedList<LruDictionaryItem>();

        /// <summary>
        /// Retreives an item or the default value and makes it MRU.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private V retrieve(K key)
        {
            lock (this)
            {
                LinkedListNode<LruDictionaryItem> node;
                if (map.TryGetValue(key, out node))
                {
                    V value = node.Value.value;
                    list.Remove(node);
                    list.AddLast(node);
                    return value;
                }
                return default(V);
            }
        }
        /// <summary>
        /// Tries to retrieve and makes it MRU.
        /// </summary>
        /// <param name="k"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private bool tryGetValue(K k, out V v)
        {
            lock (this)
            {
                LinkedListNode<LruDictionaryItem> node;
                if (map.TryGetValue(k, out node))
                {
                    v = node.Value.value;
                    list.Remove(node);
                    list.AddLast(node);
                    return true;
                }
                v = default(V);
                return false;
            }
        }
        /// <summary>
        /// Stores an item and makes it MRU.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        private void store(K key, V val)
        {
            lock (this)
            {
                LinkedListNode<LruDictionaryItem> node;
                if (map.TryGetValue(key, out node))
                {
                    list.Remove(node);
                    list.AddLast(node);
                    return;
                }
                if (map.Count >= capacity)
                {
                    removeLruItem();
                }
                LruDictionaryItem cacheItem = new LruDictionaryItem(key, val);
                node = new LinkedListNode<LruDictionaryItem>(cacheItem);
                list.AddLast(node);
                map.Add(key, node);
            }
        }
       
        /// <summary>
        /// Removes the least recently used item.
        /// </summary>
        private void removeLruItem()
        {
            lock (this)
            {
                LinkedListNode<LruDictionaryItem> node = list.First;
                list.RemoveFirst();
                map.Remove(node.Value.key);
                this.FlushItem(node.Value);
            }
        }

        /// <summary>
        /// Removes an item with a specific key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool removeSpecificItem(K key)
        {
            lock (this)
            {
                LinkedListNode<LruDictionaryItem> node;
                if (map.TryGetValue(key, out node))
                {
                    list.Remove(node);
                    map.Remove(key);
                    this.FlushItem(node.Value);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Returns true if the cache currently contains the key
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        private bool containsKey(K k)
        {
            lock (this)
            {
                return map.ContainsKey(k);
            }
        }
        /// <summary>
        /// Clears the entire lru dictionary.
        /// </summary>
        private void clear()
        {
            lock (this)
            {
                while (this.list.Count > 0)
                {
                    this.removeLruItem();
                }
            }
        }

        /// <summary>
        /// Returns number of member currently in LruDictionary
        /// </summary>
        private int count
        {
            get { lock (this) { return this.list.Count; } }
        }


        # endregion

        # region Abstract members
        
        /// <summary>
        /// Called anytime the an LRU item is flushed.
        /// </summary>
        /// <param name="item"></param>
        protected abstract void FlushItem(LruDictionary<K,V>.LruDictionaryItem item);

        #endregion

        #region Public members.
       
        /// <summary>
        /// Represents and item in the dictionary
        /// </summary>
        public class LruDictionaryItem
        {
            public K key;
            public V value;
            public LruDictionaryItem(K k, V v)
            {
                key = k;
                value = v;
            }
        }

       
        /// <summary>
        /// Instanciates the dictionary with a fixed capacity
        /// </summary>
        /// <param name="capacity_"></param>
        public LruDictionary(int capacity_)
        {
            capacity = capacity_;
        }

        /// <summary>
        /// Returns true if the dictionary has the key
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public bool Contains(K k)
        {
            return this.containsKey(k);
        }

        /// <summary>
        /// Clears the entire lru dictionary.
        /// </summary>
        public void Clear()
        {
            this.clear();
        }

        /// <summary>
        /// Indexed access to the dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public V this[K key]
        {
            get
            {
                    return this.retrieve(key);
            }
            set
            {
                    this.store(key, value);
            }
        }

        /// <summary>
        /// Tries to get a dictionary value
        /// </summary>
        /// <param name="k"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool TryGetValue(K k, out V v)
        {
            return this.tryGetValue(k, out v);
        }
      
        /// <summary>
        /// Adds item to dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(K key, V value)
        {
            this.store(key,value);
        }

        /// <summary>
        /// True if key exists in dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(K key)
        {
                return this.containsKey(key);
        }

        /// <summary>
        /// Return number of items in the LruDictionary
        /// </summary>
        public int Count
        {
            get { return this.count; }
        }

        /// <summary>
        /// Always false
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// PurgeCluster an item with the specified key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(K key)
        {
            return this.removeSpecificItem(key);
        }

        /// <summary>
        /// PurgeCluster an item from the dictionary
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<K, V> item)
        {
            return this.removeSpecificItem(item.Key);
        }

        /// <summary>
        /// Add an item to the dictionary
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<K, V> item)
        {
            this.store(item.Key, item.Value);
        }

        /// <summary>
        /// Returns true if dictionary contains the item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<K, V> item)
        {
            return this.ContainsKey(item.Key);
        }

        # region Unsupported IDictionary<K,V> members
        
        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public ICollection<K> Keys
        {
            get { throw new NotImplementedException(); }
        }

        public ICollection<V> Values
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}

