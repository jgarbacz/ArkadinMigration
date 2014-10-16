using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{
    /// <summary>
    /// Concatenates the passed RawEnumerators. This is a total pass thru
    /// class. It does nothing but point into the passed enumerators. In
    /// other words it does not read any values that are not consumed.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class ConcatenatedRawEnumerator<K, V> : IRawMergeEnumerator<K,V>
    {

        private LinkedList<IRawMergeEnumerator<K,V>> enumQueue;
        public ConcatenatedRawEnumerator(IEnumerable<IRawMergeEnumerator<K,V>> enums)
        {
            // instanciate a queue of enumerators
            this.enumQueue = new LinkedList<IRawMergeEnumerator<K, V>>();
            // only add the enumerators that have next values. this allows
            // us to know that if there is more then one enumerator in the queue
            // that we are guaranteed to have a next value.
            foreach (var item in enums){
                if (item.HasNext) this.enumQueue.Enqueue(item);
            }
        }

        public RawKeyValuePair<K, V> Current
        {
            get { return this.enumQueue.Peek().Current; }
        }

        public RawKeyValuePair<K, V> Next
        {
            get {
                if (this.enumQueue.Peek().HasNext)
                    return this.enumQueue.Peek().Next;
                else if (this.enumQueue.Count > 1)
                    return this.enumQueue.Peek(2).Next;
                else
                return default(RawKeyValuePair<K,V>); 
            }
        }

        public bool HasNext
        {
            get {
                if (this.enumQueue.Peek().HasNext) 
                    return true;
                else if (this.enumQueue.Count > 1)
                    return true;
                else
                    return false;
            }
        }

        public void Dispose()
        {
        }

        object System.Collections.IEnumerator.Current
        {
            get { return this.Current; }
        }

        public bool MoveNext()
        {
            for (; ; )
            {
                if (this.enumQueue.Peek().MoveNext()) return true;
                this.enumQueue.Dequeue();
                if (this.enumQueue.Count == 0){
                    return false;
                }
            }
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get{
               return this.enumQueue.Select(x => x.Count).Sum();
            }
        }
    }


}
