using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
namespace MVM
{
   
    /// <summary>
    /// Creates a raw enumerator from live one. This wrapper allows us to easily
    /// merge a live KeyValuePair<K,V> with other RawKeyValuePair<K,V>.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class LiveToRawEnumerator<K, V> : IRawMergeEnumerator<K,V>
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        IMergeEnumerator<K, V> liveEnumerator;
        RawKeyValuePair<K, V> rawCurrent = new RawKeyValuePair<K, V>();
        RawKeyValuePair<K, V> rawNext = new RawKeyValuePair<K, V>();
        MemoryStream keyMs = new MemoryStream();
        MemoryStream valMs = new MemoryStream();
        BinaryWriter keyWriter;
        BinaryWriter valWriter;
        readonly ISerializer<K> keySerializer;
        readonly ISerializer<V> valueSerializer;

        /// <summary>
        /// Creates a raw mergeable enumerator from the live one. The first
        /// value to be returned is row.MoveNext().
        /// </summary>
        /// <param name="rows"></param>
        public LiveToRawEnumerator(IMergeEnumerator<K, V> liveEnumerator, ISerializer<K> keySerializer,ISerializer<V> valueSerializer)
        {
            this.liveEnumerator = liveEnumerator;
            this.keyWriter = new BinaryWriter(keyMs);
            this.valWriter = new BinaryWriter(valMs);
            this.keySerializer = keySerializer;
            this.valueSerializer = valueSerializer;
            this.SetNext();
        }

        private bool SetNext()
        {
            if (this.liveEnumerator.HasNext)
            {
                this.keyMs.Position = 0;
                this.keySerializer.Serialize(this.liveEnumerator.Next.Key, this.keyWriter);
                //this.liveEnumerator.Next.Key.Serialize(this.keyWriter);
                this.rawNext.Key.buffer = this.keyMs.ToArray();
                this.rawNext.Key.offset = 0;
                this.rawNext.Key.length = (int)this.keyMs.Position;

                //logger.Info("LiveToRaw: key=" + System.Text.Encoding.ASCII.GetString(this.rawNext.Key.buffer, this.rawNext.Key.offset, this.rawNext.Key.length));

                this.valMs.Position = 0;
                this.valueSerializer.Serialize(this.liveEnumerator.Next.Value,this.valWriter);
                //this.liveEnumerator.Next.Value.Serialize(this.valWriter);
                this.rawNext.Value.buffer = this.valMs.ToArray();
                this.rawNext.Value.offset = 0;
                this.rawNext.Value.length = (int)this.valMs.Position;
            }
            return this.HasNext;
        }

        public RawKeyValuePair<K, V> Current
        {
            get { return this.rawCurrent; }
        }

        public RawKeyValuePair<K, V> Next
        {
            get { return this.HasNext?this.rawNext:default(RawKeyValuePair<K,V>); }
        }

        public bool HasNext
        {
            get
            {
                return liveEnumerator.HasNext;
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
            // if there is no next value, just return false
            if (!this.HasNext) return false;
            // set the raw current to the raw next which we've already 
            // converted.
            this.rawCurrent = this.rawNext;
            // move the cursor and try to set next again.
            this.liveEnumerator.MoveNext();
            this.SetNext();
            return true;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        #region IRawMergeEnumerator<K,V> Members

        public int Count
        {
            get { return this.liveEnumerator.Count; }
        }

        #endregion
    }
}
