using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class SynchronizedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _innerDict;
        private readonly Object _syncRoot = new object();

        public object SyncRoot
        { get { return _syncRoot; } }

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value)
        {
            lock (_syncRoot)
            {
                _innerDict.Add(key, value);
            }
        }

        public bool ContainsKey(TKey key)
        {
            lock (_syncRoot)
            {
                return _innerDict.ContainsKey(key);
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                lock (_syncRoot)
                {
                    return _innerDict.Keys;
                }
            }
        }

        public bool Remove(TKey key)
        {
            lock (_syncRoot)
            {
                return _innerDict.Remove(key);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_syncRoot)
            {
                return _innerDict.TryGetValue(key, out value);
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                lock (_syncRoot)
                {
                    return _innerDict.Values;
                }
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (_syncRoot)
                {
                    return _innerDict[key];
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _innerDict[key] = value;
                }
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            lock (_syncRoot)
            {
                (_innerDict as ICollection<KeyValuePair<TKey, TValue>>).Add(item);
            }
        }

        public void Clear()
        {
            lock (_syncRoot)
            {
                _innerDict.Clear();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            lock (_syncRoot)
            {
                return (_innerDict as ICollection<KeyValuePair<TKey, TValue>>).Contains(item);
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock (_syncRoot)
            {
                (_innerDict as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, arrayIndex);
            }
        }

        public int Count
        {
            get
            {
                lock (_syncRoot)
                {
                    return _innerDict.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (_syncRoot)
            {
                return (_innerDict as ICollection<KeyValuePair<TKey, TValue>>).Remove(item);
            }
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _innerDict.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _innerDict.GetEnumerator();
        }

        #endregion

        public SynchronizedDictionary()
        {
            _innerDict = new Dictionary<TKey, TValue>();
        }
    }
}
