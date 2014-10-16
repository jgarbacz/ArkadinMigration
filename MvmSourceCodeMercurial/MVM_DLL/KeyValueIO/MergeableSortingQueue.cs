using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
using NGenerics.DataStructures.Trees;
namespace MVM
{

    public class NaturalMergeableSortingQueue<K, V> : MergeableSortingQueue<K, V>
        where K : IBinarySerializableComparable<K>
        where V : ISerializable<V>
    {
        public static readonly IComparer<K> defaultComparer =  new DefaultLiveComparer<K>();
        public NaturalMergeableSortingQueue()
            : base(defaultComparer)
        {
        }
    }

    /// <summary>
    /// This class is meant to be a child of a MergeSortReader. It allows objects to be 
    /// added. When you add an object, it returns the comparison of the added object to 
    /// the existing current object.
    /// </summary>
    public class MergeableSortingQueue<K, V> : IMergeEnumerable<K, V>
    {
        protected MergeableSortingQueue(){}

        /// <summary>
        /// Stores all pairs but the current.
        /// </summary>
        private readonly RedBlackTree<K, LinkedList<V>> sortingTree;
        public long CountConsumed { get; set; }

        public bool IsDisposed { get; set; }
        
        public int Count { 
            get; 
            set; 
        }

        private readonly IComparer<K> comparer;
       
        public MergeableSortingQueue(IComparer<K> comparer)
        {
            this.comparer = comparer;
            this.sortingTree = new RedBlackTree<K, LinkedList<V>>(this.comparer);
        }


        /// <summary>
        /// Adds the pair to the sorting queue. 
        /// </summary>
        /// <param name="kv"></param>
        /// <returns></returns>
        public void Add(KeyValuePair<K, V> newKv)
        {
            this.sortingTree.Enqueue(newKv);
            this.Count++;
        }
     
        #region IMergeEnumerable<K,V> Members

        private Enumerator myEnumerator;
        public IMergeEnumerator<K, V> MergeEnumerator
        {
            get
            {
                return this.GetMergeEnumerator();
            }
        }
        public IMergeEnumerator<K, V> GetMergeEnumerator()
        {
            if (this.myEnumerator == null)
                this.myEnumerator = new Enumerator(this);
            return this.myEnumerator;
        }
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return this.GetMergeEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetMergeEnumerator();
        }
        
        public class Enumerator : IMergeEnumerator<K, V>
        {
            private readonly MergeableSortingQueue<K, V> parent;
            private readonly RedBlackTree<K, LinkedList<V>> sortingTree;
            public Enumerator(MergeableSortingQueue<K, V> parent)
            {
                this.parent = parent;
                this.sortingTree=parent.sortingTree;
            }
            public int Count { get { return this.parent.Count; } set { this.parent.Count = value; } }
            public long CountConsumed { get { return this.parent.CountConsumed; } set { this.parent.CountConsumed = value; } }
            public bool IsDisposed { get { return this.parent.IsDisposed; } set { this.parent.IsDisposed = value; } }
            private KeyValuePair<K, V> current;
            public KeyValuePair<K, V> Current
            {
                get { return current; }
            }

            public KeyValuePair<K, V> Next
            {
                get {
                    KeyValuePair<K, V> next;
                    if (this.sortingTree.Peek(out next)) 
                        return next;
                    return next;
                }
            }

            public bool HasNext
            {
                get
                {
                    return this.sortingTree.Count>0;
                }
            }

            
            object System.Collections.IEnumerator.Current
            {
                get { return this.Current; }
            }

            public bool MoveNext()
            {
                if (this.Count <= 0) 
                    return false;
                this.Count--;
                this.CountConsumed++;
                if (!this.sortingTree.TryDequeue(out this.current)){
                    throw new Exception("unexpected");
                }
                return true;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            #region IDisposable Members

            public void Dispose()
            {
                this.IsDisposed = true;
            }

            #endregion

        }
        #endregion
    }
}
