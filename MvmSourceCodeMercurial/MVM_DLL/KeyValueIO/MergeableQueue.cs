using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{
    public class MergeableQueue<K, V> :  IMergeEnumerable<K, V>
        //where K : IBinarySerializableComparable<K>
        //where V : ISerializable<V>
    {
        private Queue<KeyValuePair<K, V>> queue = new Queue<KeyValuePair<K, V>>();
        private Enumerator myEnumerator;
        public int Count { get { return this.queue.Count; } }
        public bool IsDisposed { get; set; }
        public long CountConsumed { get; set; }
            
        public MergeableQueue()
        {
            
        }

        public void Add(KeyValuePair<K, V> kv)
        {
            this.queue.Enqueue(kv);
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
            private readonly MergeableQueue<K, V> parent;
            public Enumerator(MergeableQueue<K, V> parent)
            {
                this.parent = parent;
            }

            /// <summary>
            /// Number of rows consumed.
            /// </summary>
            public bool IsDisposed { get { return this.parent.IsDisposed; } set { this.parent.IsDisposed = value; } }
            public int Count { get { return this.parent.queue.Count; } }
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
                    return this.HasNext?this.parent.queue.Peek():default(KeyValuePair<K,V>);
                }
            }
            public bool HasNext
            {
                get
                {
                    return this.parent.queue.Count > 0;
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
                this.current = this.parent.queue.Dequeue();
                return true;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }
        
       
    }
}
