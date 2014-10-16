using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
using NGenerics.DataStructures.Trees;
namespace MVM
{
    public class NaturalRawMergeSortReader<K, V> : RawMergeSortReader<K, V>
        where K : IBinarySerializableComparable<K>, new()
        where V : ISerializable<V>, new()
    {
        public static readonly IComparer<RawValue<K>> defaultComparer = new K().GetRawComparer();
        public NaturalRawMergeSortReader()
            : base(defaultComparer)
        {
        }
    }

    /// <summary>
    /// Merges multiple IEnumerator<RawKeyValuePair<K,V>> into a single IEnumerator<RawKeyValuePair<K,V>>.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class RawMergeSortReader<K, V> : IEnumerable<RawKeyValuePair<K, V>>
    {
        protected RawMergeSortReader() { }
        public static Logger logger = LogManager.GetCurrentClassLogger();
        private RedBlackTree<RawValue<K>, LinkedList<IRawMergeEnumerator<K, V>>> sortedEnumerators;

        /// <summary>
        /// Constructor
        /// </summary>
        public RawMergeSortReader(IComparer<RawValue<K>> rawComparer)
        {
            sortedEnumerators = new RedBlackTree<RawValue<K>, LinkedList<IRawMergeEnumerator<K, V>>>(rawComparer);
        }

        /// <summary>
        /// Adds an RawMergeEnumerable item to be merged where <code>mergeEnumerable.GetEnumerator().Next</code> will be 
        /// the next value to be merged.
        /// </summary>
        /// <param name="mergeEnumerable"></param>
        public void InsertMergeEnumerable(IRawMergeEnumerable<K, V> mergeEnumerable)
        {
            IRawMergeEnumerator<K, V> mergeEnumerator = mergeEnumerable.GetRawMergeEnumerator();
            this.InsertMergeEnumerator(mergeEnumerator);
        }

        /// <summary>
        /// Inserts an enumerator where enumerator.Next is set to the next item 
        /// to be merged.
        /// </summary>
        /// <param name="mergeNode"></param>
        public void InsertMergeEnumerator(IRawMergeEnumerator<K, V> mergeEnumerator)
        {            
            if (mergeEnumerator.HasNext)
                this.sortedEnumerators.Enqueue(mergeEnumerator.Next.Key, mergeEnumerator);
            else
                logger.Info("no need to insert if it does not have next node.");
        }

        /// <summary>
        /// Removes an enumerator
        /// </summary>
        protected bool RemoveMergeEnumerator(IRawMergeEnumerator<K, V> mergeEnumerator)
        {
            RawValue<K> treeSortKey = mergeEnumerator.Next.Key;
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

        /// <summary>
        /// Removes and inserts the enumerator. Call this if you know that
        /// enumerator.Next has changed even though it has not been consumed.
        /// </summary>
        /// <param name="mergeNode"></param>
        /// <returns></returns>
        protected void ReinsertMergeEnumerator(IRawMergeEnumerator<K, V> mergeEnumerator)
        {
            if (!this.RemoveMergeEnumerator(mergeEnumerator))
                throw new Exception("Unexpected that remove tree returned false");
            this.ReinsertMergeEnumerator(mergeEnumerator);
        }

        #region IEnumerable

        /// <summary>
        /// This class has a singleton enumerator
        /// </summary>
        private Enumerator myEnumerator;

        public IEnumerator<RawKeyValuePair<K, V>> GetEnumerator()
        {
            if (this.myEnumerator == null)
                this.myEnumerator = new Enumerator(this);
            return this.myEnumerator;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected class Enumerator : IEnumerator<RawKeyValuePair<K, V>>
        {
            private RawMergeSortReader<K, V> parent;
            private RawKeyValuePair<K, V> current;
            private RedBlackTree<RawValue<K>, LinkedList<IRawMergeEnumerator<K, V>>> sortedEnumerators;
            public Enumerator(RawMergeSortReader<K, V> parent)
            {
                this.parent = parent;
                this.sortedEnumerators = this.parent.sortedEnumerators;
            }


            public bool MoveNext()
            {
                IRawMergeEnumerator<K, V> nextEnumerator;
                RawValue<K> key;
                if (this.sortedEnumerators.TryDequeue(out key, out nextEnumerator))
                {
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
                    throw new Exception("It is unexpected that MoveNext did not have a value");
                }
                current = default(RawKeyValuePair<K, V>);
                return false;
            }
            public RawKeyValuePair<K, V> Current
            {
                get { return this.current; }
            }
            public void Dispose()
            {
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

        #endregion

    }
}
