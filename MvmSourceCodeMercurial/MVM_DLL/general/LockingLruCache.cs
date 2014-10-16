using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
namespace MVM
{


    /// <summary>
    /// Provides a cache with a finite capacity that keeps its most recently read or written items. 
    /// Subclass can override <code>FlushItem()</code> to fire a method call when items are flushed from cache.
    /// When items are retreived they are locked until they are unlocked via the Dispose() method.
    /// 
    /// when an item is flushed, it may need to keep some state in memory. so flushing returns a state object
    /// which will be passed to the contructor next time.
    /// 
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public abstract class LockingLruCache<K, V>
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// When an item is flushed, it can return a state object to be passed to the constuctor next time
        /// the item is retrieved.
        /// </summary>
        private static Dictionary<K, object> savedState = new Dictionary<K, object>();


        public int NumLockedItems
        {
            get
            {
                lock (this)
                {
                    return this.lockedList.Count;
                }
            }
        }

        public int NumUnLockedItems
        {
            get
            {
                lock (this)
                {
                    return this.unlockedList.Count;
                }
            }
        }

        public int NumItemsInCached
        {
            get
            {
                lock (this)
                {
                    return map.Count;
                }
            }
        }

        public int NumItemsSavedState
        {
            get
            {
                lock (this)
                {
                    return savedState.Count;
                }
            }
        }

        #region Intermal members
        /// <summary>
        /// Max items in the LRU
        /// </summary>
        private int capacity;

        /// <summary>
        /// This is the dictionary for the cache. It points into either unlockedList or lockedList
        /// </summary>
        private Dictionary<K, LinkedListNode<LockingLruCacheItem>> map = new Dictionary<K, LinkedListNode<LockingLruCacheItem>>();

        /// <summary>
        /// LRU for items that are unlocked.
        /// </summary>
        private LinkedList<LockingLruCacheItem> unlockedList = new LinkedList<LockingLruCacheItem>();

        /// <summary>
        /// List of items that are locked.
        /// </summary>
        private LinkedList<LockingLruCacheItem> lockedList = new LinkedList<LockingLruCacheItem>();


        /// <summary>
        /// Instanciates the dictionary with a fixed capacity
        /// </summary>
        /// <param name="capacity_"></param>
        public LockingLruCache(int capacity_)
        {
            capacity = capacity_;
        }

      
        /// <summary>
        /// Retrieves and locks the item
        /// </summary>
        /// <param name="k"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool Retreive(K k, out LockingLruCacheItem v)
        {
            lock (this)
            {
                LinkedListNode<LockingLruCacheItem> node;
                if (map.TryGetValue(k, out node))
                {
                    node.Value.Lock();
                    v= node.Value;
                    return true;
                }
                v = null;
                return false;
            }
        }

        /// <summary>
        /// This delegate creates object of type V when passed type K
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public delegate V Constructor (K k, object state);

        /// <summary>
        /// Retrieves and locks the item, creating a new item if necessary
        /// </summary>
        /// <param name="k"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public LockingLruCacheItem Retreive(K k, Constructor constructor)
        {
            lock (this)
            {
                LockingLruCacheItem item;
                if (this.Retreive(k, out item))
                {
                    return item;
                }
                object state = null;
                lock (savedState)
                {
                    savedState.TryGetValue(k, out state);
                }
                V value = constructor(k,state);
                item=this.AddNewItem(k, value);
                item.Lock();
                return item;
            }
        }
        /// <summary>
        /// Adds a new unlocked item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public LockingLruCacheItem AddNewItem(K key, V val)
        {
            lock (this)
            {
                LinkedListNode<LockingLruCacheItem> node;
                if (map.TryGetValue(key, out node))
                {
                    throw new Exception("key " + key + " already exists");
                }
                if (map.Count >= capacity)
                {
                    if (!removeLruUnlockedItem())
                    {

                        throw new Exception("Error, all items in LockingLruCache are locked: locked keys are:"+map.Keys.JoinStrings(",".AppendLine()));
                    }
                }
                LockingLruCacheItem cacheItem = new LockingLruCacheItem(this,key, val);
                node = new LinkedListNode<LockingLruCacheItem>(cacheItem);
                unlockedList.AddLast(node);
                map.Add(key, node);
                return node.Value;
            }
        }

        /// <summary>
        /// Removes the least recently unlocked item, returning true for success, false if nothing unlocked to remove.
        /// </summary>
        /// <returns></returns>
        public bool removeLruUnlockedItem()
        {
            lock (this)
            {
                if (this.unlockedList.Count > 0)
                {
                    LinkedListNode<LockingLruCacheItem> node = unlockedList.First;
                    unlockedList.RemoveFirst();
                    map.Remove(node.Value.key);
                    object state = this.FlushItem(node.Value,true);
                    if (state != null)
                    {
                        lock (savedState)
                        {
                            savedState[node.Value.key] = state;
                        }
                        //logger.Debug("LLC: removed and flushed LRU item {0}, saved state for next time", node.Value.key.ToString());

                    }
                    else
                    {
                        //logger.Debug("LLC: removed and flushed LRU item {0}, no state to store", node.Value.key.ToString());
                    }
                    return true;
                }
                return false;
            }
        }



        /// <summary>
        /// Removes an item with a specific key. Throws exception if item is locked. If keep
        /// stateState is true, then we will flush the item and store the returned state object.
        /// This object will be passed to the contructor next time someone tries to get an object
        /// with this same key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveSpecificItem(K key, bool keepState)
        {
            //logger.Debug("LLC: cache={0} state={1} locked={2} unlocked={3}", this.NumItemsInCached, this.NumItemsSavedState, this.NumLockedItems, this.NumUnLockedItems);
            if (this.NumItemsInCached > 7)
            {
                lock (this)
                {
                    //logger.Debug(this.map.Keys.JoinStrings("\r\n"));
                }
            }
            lock (this)
            {
                LinkedListNode<LockingLruCacheItem> node;
                if (map.TryGetValue(key, out node))
                {
                    if (node.Value.lockCtr > 0)
                    {
                        throw new Exception("Error, cannot remove " + key.ToString() + " from cache when it still has " + node.Value.lockCtr + " locks");
                    }
                    try
                    {
                        unlockedList.Remove(node);
                    }
                    catch (Exception e)
                    {
                        throw (e);
                    }
                    map.Remove(key);
                    object state = this.FlushItem(node.Value, keepState);
                    if (!keepState && state != null) 
                        throw new Exception("Error, called flush item with keepState=false but returned a state object. This object would get lost so this is an error");
                    if (state != null)
                    {
                        lock (savedState)
                        {
                            savedState[key] = state;
                        }
                        //logger.Debug("LLC: removed and flushed {0}, saved state for next time", key.ToString());
                    }
                    else
                    {
                        //logger.Debug("LLC: removed and flushed {0}, no state to store", key.ToString());
                    }
                    return true;
                }
                // otherwise item is not in cache but we might need to remove previously saved state.
                else
                {
                    if (!keepState)
                    {
                        lock (savedState)
                        {
                            if (savedState.Remove(key))
                            {
                                //logger.Debug("LLC: try remove {0} but not in cache, removing saved state.", key.ToString());
                            }
                            else
                            {
                                //logger.Debug("LLC: try remove {0} but not in cache, no saved state to remove.", key.ToString());
                            }
                        }
                    }
                    else
                    {
                        //logger.Debug("LLC: try remove {0} but not in cache, keepState is set leave what is there", key.ToString());
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Returns true if the cache currently contains the key
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public bool ContainsKey(K k)
        {
            lock (this)
            {
                return map.ContainsKey(k);
            }
        }

        /// <summary>
        /// Return list of cache keys
        /// </summary>
        /// <returns></returns>
        public List<K> GetKeys()
        {
            lock (this)
            {
                return map.Keys.ToList();
            }
        }

        /// <summary>
        /// Flushes all the unlocked items.
        /// </summary>
        public void FlushAllUnlockedItems()
        {
            lock (this)
            {
                while (this.unlockedList.Count > 0)
                {
                    this.removeLruUnlockedItem();
                }
            }
        }

        /// <summary>
        /// Returns number of members currently in LockingLruCache
        /// </summary>
        public int Count
        {
            get { lock (this) { return this.map.Count; } }
        }

        public List<K> LockedItems
        {
            get
            {
                lock (this)
                {
                    return this.lockedList.Select(n => n.key).ToList();
                }
            }
        }

        /// <summary>
        /// Returns number of items that are locked
        /// </summary>
        public int CountLocked
        {
            get { lock (this) { return this.lockedList.Count; } }
        }

        /// <summary>
        /// Returns number of items that are unlocked
        /// </summary>
        public int CountUnLocked
        {
            get { lock (this) { return this.unlockedList.Count; } }
        }

        /// <summary>
        /// Called by LockingLruCacheItem to move item from locked to unlocked
        /// </summary>
        /// <param name="item"></param>
        public void _MvLockedToUnlocked(LockingLruCacheItem item)
        {
            lock (this)
            {
                LinkedListNode<LockingLruCacheItem> node;
                if (map.TryGetValue(item.key, out node))
                {
                    //logger.Debug("_MvLockedToUnlocked");
                    lockedList.Remove(node);
                    unlockedList.AddLast(node);
                    
                }
                else
                {
                    throw new Exception("unexpected");
                }
            }
        }
        /// <summary>
        /// Called by LockingLruCacheItem to move item from unlocked to unlocked
        /// </summary>
        /// <param name="item"></param>
        public void _MvUnlockedToLocked(LockingLruCacheItem item)
        {
            lock (this)
            {
                LinkedListNode<LockingLruCacheItem> node;
                if (map.TryGetValue(item.key, out node))
                {
                    //logger.Debug("_MvUnlockedToLocked");
                    unlockedList.Remove(node);
                    lockedList.AddLast(node);
                   
                }
                else
                {
                    throw new Exception("unexpected");
                }
            }
        }



# endregion

# region Abstract members

        /// <summary>
        /// Called anytime the an LRU item is flushed. If keepState is true the implementer should try to keep state
        /// externally (for example, flushing buffers, then closing files) and/or return a state object.
        /// </summary>
        /// <param name="item"></param>
        protected abstract object FlushItem(LockingLruCache<K, V>.LockingLruCacheItem item, bool keepState);

#endregion


        /// <summary>
        /// Represents and item in the cache
        /// </summary>
        public class LockingLruCacheItem : IDisposable
        {
            public K key;
            public V value;
            public readonly LockingLruCache<K, V> cache;
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="cache"></param>
            /// <param name="k"></param>
            /// <param name="v"></param>
            public LockingLruCacheItem(LockingLruCache<K, V> cache, K key, V value)
            {
                this.cache = cache;
                this.key = key;
                this.value = value;
                this.lockCtr = 0;
            }
            /// <summary>
            /// Number of locks on this item.
            /// </summary>
            public int lockCtr { get; private set; }
            /// <summary>
            /// Add a lock to the item, returns total lock count.
            /// </summary>
            /// <returns></returns>
            public int Lock()
            {
                lock (this)
                {
                    if (++this.lockCtr == 1)
                    {
                        this.cache._MvUnlockedToLocked(this);
                    }
                    //logger.Debug("locking {0} of type {1}, count={2}", this.key.ToString(), this.value.ToString(), this.lockCtr);
                    return this.lockCtr;
                }
            }
            /// <summary>
            /// Removes a lock from the item, returns total lock count.
            /// </summary>
            /// <returns></returns>
            public int Unlock()
            {
                lock (this)
                {
                    if (--this.lockCtr == 0)
                    {
                        this.cache._MvLockedToUnlocked(this);
                    }
                    //logger.Debug("unlocking {0} of type {1}, count={2}",this.key.ToString(), this.value.ToString(), this.lockCtr);
                    return this.lockCtr;
                }
            }
            /// <summary>
            /// Disposing the item removes a lock from it
            /// </summary>
            public void Dispose()
            {
                this.Unlock();
            }
        }

       
    }
}

