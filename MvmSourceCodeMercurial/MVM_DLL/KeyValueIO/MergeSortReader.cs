using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
using NGenerics.DataStructures.Trees;
using System.Threading;
namespace MVM
{

    public class NaturalMergeSortReader<K, V> : MergeSortReader<K, V>
        where K : IBinarySerializableComparable<K>, new()
        where V : ISerializable<V>, new()
    {
        public static readonly IComparer<K> defaultComparer = new DefaultLiveComparer<K>();
        public NaturalMergeSortReader()
            : base(defaultComparer)
        {
        }
    }

    /// <summary>
    /// Merge sorts <code>IMergeEnumerator<K,V></code>. 
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class MergeSortReader<K, V> : IEnumerable<KeyValuePair<K, V>>
    {
        protected MergeSortReader() { }

        public static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Maintain a sorted tree of all the things we are merging.
        /// </summary>
        private RedBlackTree<K, LinkedList<IMergeEnumerator<K, V>>> sortedEnumerators;

        /// <summary>
        /// This is the comparer we'll use to sort and merge keys.
        /// </summary>
        public readonly IComparer<K> liveComparer;
        public readonly IComparer<KeyValuePair<K, V>> liveKeyValuePairComparer;


        /// <summary>
        /// Constructor with override comparer
        /// </summary>
        /// <param name="comparer"></param>
        public MergeSortReader(IComparer<K> liveComparer)
        {
            this.liveComparer = liveComparer;
            this.liveKeyValuePairComparer = new KeyValuePairKeyComparer<K,V>(this.liveComparer);
            this.sortedEnumerators = new RedBlackTree<K, LinkedList<IMergeEnumerator<K, V>>>(this.liveComparer);
        }


        /// <summary>
        /// Adds an MergeEnumerable item to be merged where <code>mergeEnumerable.GetEnumerator().Next</code> will be 
        /// the next value to be merged.
        /// </summary>
        /// <param name="mergeEnumerable"></param>
        public void InsertMergeEnumerable(IMergeEnumerable<K, V> mergeEnumerable)
        {
            IMergeEnumerator<K, V> mergeEnumerator = mergeEnumerable.GetMergeEnumerator();
            this.InsertMergeEnumerator(mergeEnumerator);
        }

        /// <summary>
        /// Inserts an enumerator where enumerator.Next is set to the next item 
        /// to be merged.
        /// </summary>
        /// <param name="mergeNode"></param>
        public void InsertMergeEnumerator(IMergeEnumerator<K, V> mergeEnumerator)
        {
            if (mergeEnumerator.HasNext)
            {
                //logger.Error("INSERT:" + mergeEnumerator.Next.Key);
                this.sortedEnumerators.Enqueue(mergeEnumerator.Next.Key, mergeEnumerator);
            }
            else
                logger.Info("no need to insert if it does not have next node.");
        }

        /// <summary>
        /// Removes an enumerator
        /// </summary>
        protected bool RemoveMergeEnumerator(IMergeEnumerator<K, V> mergeEnumerator)
        {
            K treeSortKey = mergeEnumerator.Next.Key;
            var linkedList = this.sortedEnumerators[treeSortKey];
            if (linkedList == null)
                return false;
            if (!linkedList.Remove(mergeEnumerator))
            {
                return false;
            }
            if (linkedList.Count == 0)
            {
                this.sortedEnumerators.Remove(treeSortKey);
            }
            return true;
        }

        public List<K> CurrentKeys
        {
            get
            {
                return this.sortedEnumerators.GetOrderedKeys().ToList();
            }
        }


      

        /// <summary>
        /// Sub classes can override this to do some work before the enumerator is instanciated. 
        /// MergeSortReaderWriter uses this to sort any unsorted data it has.
        /// </summary>
        public virtual void InstanciateEnumeratorHook()
        {
            lock (this)
            {
                logger.Info("FIRING INSTANCIATE HOOK!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            }
        }


        public bool LoopInProgress
        {
            get
            {
                lock (this)
                {
                    return this.EnumeratorCount > 0;
                }
            }
        }

        public int EnumeratorCount = 0;
        public void RegisterEnumerator()
        {
            lock (this)
            {
                this.EnumeratorCount++;
                //logger.Info("** after reg:enum count={0}", this.EnumeratorCount);
            }
        }

        public void RegisterDisposedEnumerator()
        {
            lock (this)
            {
                this.EnumeratorCount--;
                //logger.Info("** after disp:enum count={0}", this.EnumeratorCount);
            }
        }
        

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            lock (this)
            {
                if (this.EnumeratorCount == 0)
                {
                    this.InstanciateEnumeratorHook();
                }
                var myEnumerator = new Enumerator(this);
                return myEnumerator;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        protected class Enumerator : IEnumerator<KeyValuePair<K, V>>
        {
            // set to true when there is a loop, meaning  
            public bool LoopInProgress;
            private MergeSortReader<K, V> parent;
            private RedBlackTree<K, LinkedList<IMergeEnumerator<K, V>>> sortedEnumerators;
            public KeyValuePair<K, V> current;

            public Enumerator(MergeSortReader<K, V> parent)
            {
                this.parent = parent;
                this.sortedEnumerators = this.parent.sortedEnumerators;
                this.parent.RegisterEnumerator();
            }
           
            public bool MoveNext()
            {
                IMergeEnumerator<K, V> nextEnumerator;
                K key;

               while(this.sortedEnumerators.TryDequeue(out key, out nextEnumerator))
                {
                    //logger.Error("REMOVE:" + key);
                    if (nextEnumerator.MoveNext())
                    {
                        current = nextEnumerator.Current;
                        // if the enumerator has another value, be sure to put it back.
                        if (nextEnumerator.HasNext)
                        {
                            this.sortedEnumerators.Enqueue(nextEnumerator.Next.Key, nextEnumerator);
                        }
                        else
                        {
                            //logger.Info("Disposing totally merged enumerator:" + nextEnumerator.GetType().FullName);
                            nextEnumerator.Dispose();
                        }
                        return true;
                    }
                    //else
                    //{
                    //    throw new Exception("It is unexpected that MoveNext did not have a value,nextEnumeratorType=" + nextEnumerator.GetType().ToString());
                    //}
                 }

                current = default(KeyValuePair<K, V>);
                return false;
            }

            public KeyValuePair<K, V> Current
            {
                get { return this.current; }
            }
            public void Dispose()
            {
                this.parent.RegisterDisposedEnumerator();
            }
            object System.Collections.IEnumerator.Current
            {
                get { return this.current; }
            }
            public void Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}
