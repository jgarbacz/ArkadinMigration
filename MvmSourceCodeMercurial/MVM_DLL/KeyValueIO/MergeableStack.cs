using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{
    public class MergeableStack<K, V> : IMergeEnumerable<K, V>
        //where K : IBinarySerializableComparable<K>
        //where V : ISerializable<V>
    {
        private LinkedList<KeyValuePair<K, V>> list = new LinkedList<KeyValuePair<K, V>>();
        private Enumerator myEnumerator;
        public int Count { get { return this.list.Count; } }
        public bool IsDisposed { get; set; }
        public long CountConsumed { get; set; }

        // We need to enumerate the objects in the stack both forwards and backwards.
        // In the simple case of a bunch of reverse-order objects in memory, we want
        // sorted_object_file_select to return the objects in forward order.  But when
        // the objects overflow memory and get serialized to MergeSortReaderWriter's
        // reverseFile, we need to enumerate the objects in reverse order since the
        // underlying MergeableFile is in reverse order (otherwise we could not append
        // to it multiple times).  This flag provides the enumeration direction.
        public SortOrder outputDirection = SortOrder.Forward;

        public MergeableStack()
        {
        }

        public void Add(KeyValuePair<K, V> kv)
        {
            // The list will end up with the objects in reverse order
            this.list.AddLast(kv);
        }

        #region IMergeEnumerable<K,V> Members

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
        #endregion

        public class Enumerator : IMergeEnumerator<K, V>
        {
            private readonly MergeableStack<K, V> parent;
            public Enumerator(MergeableStack<K, V> parent)
            {
                this.parent = parent;
            }

            /// <summary>
            /// Number of rows consumed.
            /// </summary>
            public bool IsDisposed { get { return this.parent.IsDisposed; } set { this.parent.IsDisposed = value; } }
            public int Count { get { return this.parent.list.Count; } }
            public long CountConsumed { get { return this.parent.CountConsumed; } set { this.parent.CountConsumed = value; } }

            private KeyValuePair<K, V> current;

            public KeyValuePair<K, V> Current
            {
                get
                {
                    return this.current;
                }
            }

            public KeyValuePair<K, V> Next
            {
                get
                {
                    if (!this.HasNext)
                    {
                        return default(KeyValuePair<K, V>);
                    }
                    return this.parent.outputDirection == SortOrder.Forward ? this.parent.list.Last() : this.parent.list.First();
                }
            }
            public bool HasNext
            {
                get
                {
                    return this.parent.list.Count > 0;
                }
            }

            public void Dispose()
            {
                this.IsDisposed = true;
            }

            object System.Collections.IEnumerator.Current
            {
                get { return this.Current; }
            }

            public bool MoveNext()
            {
                if (this.Count <= 0) return false;
                this.CountConsumed++;
                if (this.parent.outputDirection == SortOrder.Forward)
                {
                    this.current = this.parent.list.Last();
                    this.parent.list.RemoveLast();
                }
                else
                {
                    this.current = this.parent.list.First();
                    this.parent.list.RemoveFirst();
                }
                return true;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}
