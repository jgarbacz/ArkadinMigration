using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{
    /// <summary>
    /// Creates a RawFileEnumeratorReverse by reading from the BinaryReader.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class RawFileEnumeratorReverse<K, V> : IRawMergeEnumerator<K,V>
    {
        private FileStream fileStream;
        private BinaryReader breader;
        public RawFileEnumeratorReverse(FileStream stream,int count)
        {
            this.fileStream = stream;
            this.breader = new BinaryReader(this.fileStream);
            this.Count = count;
            this.SetNext();
        }
        public RawKeyValuePair<K, V> Current{get;private set;}  
        public RawKeyValuePair<K, V> Next{get;private set;}
        public bool HasNext { get; private set; }
      
        public void Dispose()
        {
        }
        object System.Collections.IEnumerator.Current
        {
            get { return this.Current; }
        }


        private bool SetNext()
        {
            this.HasNext = false;
            if (this.fileStream.Position < 4) return false;

            this.fileStream.Position -= 4;
            int recLen = this.breader.ReadInt32();
            this.fileStream.Position -= (4 + recLen);

            // try to get the next value. if we hit eof
            // then HasNext is false
            int keySize;
            try
            {
                keySize = this.breader.Read7BitEncodedInt();
            }
            catch
            {
                this.HasNext = false;
                return this.HasNext;
            }

            this.Count--;
            RawKeyValuePair<K, V> next;
            next.Key.buffer = this.breader.ReadBytes(keySize);
            next.Key.offset = 0;
            next.Key.length = next.Key.buffer.Length;

            int valSize = this.breader.Read7BitEncodedInt();
            next.Value.buffer = this.breader.ReadBytes(valSize);
            next.Value.offset = 0;
            next.Value.length = next.Value.buffer.Length;

            this.Next = next;
            this.HasNext = true;

            this.fileStream.Position -= recLen;

            return this.HasNext;
        }

        public bool MoveNext()
        {
            if (!this.HasNext) return false;
            this.Current = this.Next;
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
            get;
            private set;
        }

        #endregion
    }


}
