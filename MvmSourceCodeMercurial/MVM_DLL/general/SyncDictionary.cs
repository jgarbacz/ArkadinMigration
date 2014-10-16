using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
namespace MVM
{
    /// <summary>
    /// Wraps all access methods with lock(this)
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class SyncDictionary<K, V>:IDictionary<K,V>
    {
        private Dictionary<K, V> map = new Dictionary<K, V>();
     
        // THIS IS UNSAFE
        public Dictionary<K, V> UnsafeGetInnerDictionary()
        {
            return this.map;
        }
        
        
        public V this[K key]
        {
            get
            {
                lock (this)
                {
                    return this.Get(key);
                }
            }
            set
            {
                lock (this)
                {
                    this.Add(key, value);
                }
            }
        }

        public bool ContainsKey(K key)
        {
            lock (this)
            {
                return map.ContainsKey(key);
            }
        }
        
        public V Get(K key)
        {
            lock (this)
            {
                return map[key];
            }
        }

        public V GetOrNull(K key)
        {
            lock (this)
            {
                return map.ContainsKey(key)?map[key]:default(V);
        }
        }

        public void Add(K key, V value)
        {
            lock (this)
            {
                map.Add(key, value);
            }
        }

        public bool AddWithTimeout(K key, V value, int timeout)
        {
            lock (this)
            {
                map.Add(key, value);
                return true;
            }
        }

        public AddOrUpdateStatus AddOrUpdate(K key, V value)
        {
            lock (this)
            {
                V result = default(V);
                if (map.TryGetValue(key, out result))
                {
                    if (result.Equals(value))
                    {
                        return AddOrUpdateStatus.Unchanged;
                    }
                    else
                    {
                        return AddOrUpdateStatus.Updated;
                    }
                }
                else
                {
                    map.Add(key, value);
                    return AddOrUpdateStatus.Added;
                }
            }
        }

        // adds the value to the map if there isn't one
        // returns true if it added the value, else false.
        public bool AddIfNull(K key, V value)
        {
            lock (this)
            {
                if (map.ContainsKey(key)) return false;
                map.Add(key, value);
                return true;
            }
        }

        // adds the value to the map if there isn't one
        // returns true if it added the value, else false.
        public V RemoveAndGet(K key)
        {
            lock (this)
            {
                V val = default(V);

                if (map.ContainsKey(key))
                {
                    val = map[key];
                    map.Remove(key);
                }

                return val;
            }
        }

        public void Remove(K key)
        {
            lock (this)
            {
                map.Remove(key);
            }
        }



        public enum AddOrUpdateStatus
        {
            Added,
            Updated,
            Unchanged
        };


        #region IDictionary<K,V> Members


        public ICollection<K> Keys
        {
            get
            {
                lock (this) return this.Keys.ToList();
            }
        }

        bool IDictionary<K, V>.Remove(K key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(K key, out V value)
        {
            lock (this)
            {
                return this.map.TryGetValue(key, out value);
            }
        }

        public ICollection<V> Values
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region ICollection<KeyValuePair<K,V>> Members

        public void Add(KeyValuePair<K, V> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<KeyValuePair<K,V>> Members

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }



    // Dictionary that supports multiple readers and 1 streamWriter
    public class OrigSyncDictionary<K, V>
    {
        private ReaderWriterLock rwLock = new ReaderWriterLock();
        private Dictionary<K, V> map = new Dictionary<K, V>();

        public V this[K key]
        {
            get
            {
                return this.Get(key);
            }
            set
            {
                this.Add(key, value);
            }
        }

        public bool ContainsKey(K key)
        {
            rwLock.AcquireReaderLock(-1);
            
            try
            {
                return map.ContainsKey(key);
            }
            finally
            {
                rwLock.ReleaseReaderLock();
            }
        }

        public V Get(K key)
        {
            rwLock.AcquireReaderLock(-1);
            try
            {
                return map[key];
            }
            finally
            {
                rwLock.ReleaseReaderLock();
            }
        }

        public V GetOrNull(K key)
        {
            rwLock.AcquireReaderLock(-1);
            try
            {
                return map.ContainsKey(key) ? map[key] : default(V);
            }
            finally
            {
                rwLock.ReleaseReaderLock();
            }
        }

        public void Add(K key, V value)
        {
            rwLock.AcquireWriterLock(-1);
            
            try
            {
                map.Add(key, value);
            }
            finally
            {
                rwLock.ReleaseWriterLock();
            }
        }

        public bool AddWithTimeout(K key, V value, int timeout)
        {
            rwLock.AcquireReaderLock(timeout);
            if (rwLock.IsReaderLockHeld)
            {
                try
                {
                    map.Add(key, value);
                }
                finally
                {
                    rwLock.ReleaseWriterLock();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public AddOrUpdateStatus AddOrUpdate(K key, V value)
        {
            //rwLock.EnterUpgradeableReadLock();
            rwLock.AcquireReaderLock(-1);
            try
            {
                V result = default(V);
                if (map.TryGetValue(key, out result))
                {
                    if (result.Equals(value))
                    {
                        return AddOrUpdateStatus.Unchanged;
                    }
                    else
                    {
                        rwLock.AcquireWriterLock(-1);
                        try
                        {
                            map[key] = value;
                        }
                        finally
                        {
                            rwLock.ReleaseWriterLock();
                        }
                        return AddOrUpdateStatus.Updated;
                    }
                }
                else
                {
                    rwLock.AcquireWriterLock(-1);
                    try
                    {
                        map.Add(key, value);
                    }
                    finally
                    {
                        rwLock.ReleaseWriterLock();
                    }
                    return AddOrUpdateStatus.Added;
                }
            }
            finally
            {
                //rwLock.ExitUpgradeableReadLock();
                rwLock.ReleaseReaderLock();
            }
        }

        // adds the value to the map if there isn't one
        // returns true if it added the value, else false.
        public bool AddIfNull(K key, V value)
        {
            rwLock.AcquireReaderLock(-1);
            try
            {
                if (map.ContainsKey(key)) return false;
            }
            finally
            {
                rwLock.ReleaseReaderLock();
            }

            rwLock.AcquireWriterLock(-1);
            try
            {
                if (map.ContainsKey(key)) return false;
                map.Add(key, value);
            }
            finally
            {
                rwLock.ReleaseWriterLock();
            }
            return true;
        }

        // adds the value to the map if there isn't one
        // returns true if it added the value, else false.
        public V RemoveAndGet(K key)
        {
            V val = default(V);
            rwLock.AcquireWriterLock(-1);
            try
            {
                if (map.ContainsKey(key))
                {
                    val = map[key];
                    map.Remove(key);
                }
            }
            finally
            {
                rwLock.ReleaseWriterLock();
            }
            return val;
        }

        public void Remove(K key)
        {
            rwLock.AcquireWriterLock(-1);
            try
            {
                map.Remove(key);
            }
            finally
            {
                rwLock.ReleaseWriterLock();
            }
        }

        public enum AddOrUpdateStatus
        {
            Added,
            Updated,
            Unchanged
        };

    }
}
