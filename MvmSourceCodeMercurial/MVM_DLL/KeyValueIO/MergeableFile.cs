using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
namespace MVM
{
    public enum Direction { Forward, Reverse };

    public class MergeableFile<K, V> : IMergeEnumerator<K, V>, IMergeEnumerable<K, V>
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        private FileStream fileStream;
        private BinaryReader breader;
        public int Count { get; set; }
        public readonly string FileName;

        public Direction direction;

        /// <summary>
        /// Returns a raw enumerator for the remaining records. Note that 
        /// this.Current is considered consumed and is not one of the remaining
        /// records. Since we already pull this.Next from the binary reader we
        /// need to reserialize it to a new stream and concatenate that stream
        /// with the binary reader.
        /// </summary>
        /// <returns></returns>
        public IRawMergeEnumerator<K, V> GetRawKeyValuePairEnumerator()
        {
            MergeableQueue<K,V> queue = new MergeableQueue<K,V>();
            queue.Add(this.Next);
            IRawMergeEnumerator<K, V> firstRaw = new LiveToRawEnumerator<K, V>(queue.GetMergeEnumerator(),keySerializer,valueSerializer);
            IRawMergeEnumerator<K, V> secondRaw;
            if (this.direction == Direction.Forward)
            {
                secondRaw = new RawFileEnumeratorForward<K, V>(this.fileStream, this.Count);
            }
            else
            {
                secondRaw = new RawFileEnumeratorReverse<K, V>(this.fileStream, this.Count);
            }
            return new ConcatenatedRawEnumerator<K,V>(
                new List<IRawMergeEnumerator<K,V>>(){
                    firstRaw,
                    secondRaw
                });
        }

        readonly ISerializer<K> keySerializer;
        readonly ISerializer<V> valueSerializer;



        /// <summary>
        ///  Builds a mergeable file from the passed Raw rows.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="rows"></param>
        public MergeableFile(Direction direction, string fileName, IEnumerable<RawKeyValuePair<K, V>> rows, ISerializer<K> keySerializer,ISerializer<V> valueSerializer)
        {
            this.direction = direction;
            this.keySerializer = keySerializer;
            this.valueSerializer = valueSerializer;

            this.FileName = fileName;
            FileInfo fileInfo = new FileInfo(this.FileName);
            fileInfo.Directory.CreateIfNotThere();

            this.PersistRawRowsToFile(rows);
            // open a reader into the file
            this.fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            this.breader = new BinaryReader(this.fileStream);
            if (this.direction == Direction.Reverse) this.fileStream.Position = this.fileStream.Length;
            this.ReadNextValue();
        }

        /// <summary>
        /// Build a mergeable file from passed rows
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="rows"></param>
        public MergeableFile(Direction direction, string fileName, IEnumerable<KeyValuePair<K, V>> rows, ISerializer<K> keySerializer, ISerializer<V> valueSerializer)
        {
            this.direction = direction;
            this.keySerializer = keySerializer;
            this.valueSerializer = valueSerializer;
            
            this.FileName = fileName;
            FileInfo fileInfo = new FileInfo(this.FileName);
            fileInfo.Directory.CreateIfNotThere();

            this.PersistLiveRowsToFile(rows);
            // open a reader into the file
            this.fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            this.breader = new BinaryReader(this.fileStream);
            if (this.direction == Direction.Reverse) this.fileStream.Position = this.fileStream.Length;
            this.ReadNextValue();
        }

        /// <summary>
        /// Build a mergeable file from a passed file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="keySerializer"></param>
        /// <param name="valueSerializer"></param>
        public MergeableFile(Direction direction, string fileName, ISerializer<K> keySerializer, ISerializer<V> valueSerializer)
        {
            this.direction = direction;
            this.FileName = fileName;
            this.keySerializer = keySerializer;
            this.valueSerializer = valueSerializer;
            this.Count = -1;
            // open a reader into the file
            this.fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            this.breader = new BinaryReader(this.fileStream);
            if (this.direction == Direction.Reverse) this.fileStream.Position = this.fileStream.Length;
            this.ReadNextValue();
        }

        public void addLiveRows(IEnumerable<KeyValuePair<K, V>> rows)
        {
            // open the file, append all the keys and values to it
            this.breader.Close();
            using (FileStream fs = new FileStream(FileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (BinaryWriter bwriter = new BinaryWriter(fs))
                {
                    foreach (KeyValuePair<K, V> row in rows)
                    {
                        this.Count++;

                        // do same thing in PersistRawRowsToFile
                        long startPos = fs.Position;

                        MemoryStream tempMs = new MemoryStream();
                        BinaryWriter tempBw = new BinaryWriter(tempMs);
                        // write the key to the temp buffer,
                        // write the keysize, followed by the serialized key
                        this.keySerializer.Serialize(row.Key, tempBw);
                        //row.Key.Serialize(tempBw);
                        int keySize = (int)tempMs.Position;
                        bwriter.Write7BitEncodedInt(keySize);
                        bwriter.Write(tempMs.GetBuffer(), 0, keySize);
                        // write the value to the temp buffer
                        // write the value size, followed by the serialized value
                        tempMs.Position = 0;
                        this.valueSerializer.Serialize(row.Value, tempBw);
                        //row.Value.Serialize(tempBw);
                        int ValSize = (int)tempMs.Position;
                        bwriter.Write7BitEncodedInt(ValSize);
                        bwriter.Write(tempMs.GetBuffer(), 0, ValSize);

                        // do same thing in PersistRawRowsToFile
                        long endPos = fs.Position;
                        int recLen = (int)(endPos - startPos);
                        bwriter.Write(recLen);
                    }
                }
            }
            this.fileStream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            this.breader = new BinaryReader(this.fileStream);
            if (this.direction == Direction.Reverse) this.fileStream.Position = this.fileStream.Length;
            this.ReadNextValue();
        }

        private void PersistLiveRowsToFile(IEnumerable<KeyValuePair<K, V>> rows)
        {
            // open the file, write all the keys and values to it.
            using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (BinaryWriter bwriter = new BinaryWriter(fs))
                {
                    foreach (KeyValuePair<K, V> row in rows)
                    {
                        this.Count++;
                        
                        // do same thing in PersistRawRowsToFile
                        long startPos = fs.Position;

                        MemoryStream tempMs = new MemoryStream();
                        BinaryWriter tempBw = new BinaryWriter(tempMs);
                        // write the key to the temp buffer,
                        // write the keysize, followed by the serialized key
                        this.keySerializer.Serialize(row.Key, tempBw);
                        //row.Key.Serialize(tempBw);
                        int keySize = (int)tempMs.Position;
                        bwriter.Write7BitEncodedInt(keySize);
                        bwriter.Write(tempMs.GetBuffer(), 0, keySize);
                        // write the value to the temp buffer
                        // write the value size, followed by the serialized value
                        tempMs.Position = 0;
                        this.valueSerializer.Serialize(row.Value, tempBw);
                        //row.Value.Serialize(tempBw);
                        int ValSize = (int)tempMs.Position;
                        bwriter.Write7BitEncodedInt(ValSize);
                        bwriter.Write(tempMs.GetBuffer(), 0, ValSize);

                        // do same thing in PersistRawRowsToFile
                        long endPos = fs.Position;
                        int recLen = (int)(endPos - startPos);
                        bwriter.Write(recLen);
                    }
                }
            }
        }

        private void PersistRawRowsToFile(IEnumerable<RawKeyValuePair<K, V>> rows){
             // open the file, write all the keys and values to it.
            using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (BinaryWriter bwriter = new BinaryWriter(fs))
                {
                    foreach (RawKeyValuePair<K, V> row in rows)
                    {
                        this.Count++;

                        // do same thing in PersistRawRowsToFile
                        long startPos = fs.Position;

                        MemoryStream tempMs = new MemoryStream();
                        BinaryWriter tempBw = new BinaryWriter(tempMs);
                        // write the keysize, followed by the serialized key
                        bwriter.Write7BitEncodedInt(row.Key.length);
                        bwriter.Write(row.Key.buffer, row.Key.offset, row.Key.length);

                        //logger.Debug("MergeableFile.AddKey: " + System.Text.Encoding.ASCII.GetString(row.Key.buffer, row.Key.offset, row.Key.length));

                        // write the value size, followed by the serialized value
                        bwriter.Write7BitEncodedInt(row.Value.length);
                        bwriter.Write(row.Value.buffer, row.Value.offset, row.Value.length);

                        // do same thing in PersistRawRowsToFile
                        long endPos = fs.Position;
                        int recLen = (int)(endPos - startPos);
                        bwriter.Write(recLen);
                    }
                }
            }
        }

        /// <summary>
        /// Number of rows consumed.
        /// </summary>
        public long CountConsumed { get; private set; }

        private KeyValuePair<K, V> current;
        public KeyValuePair<K, V> Current
        {
            get
            {
                return this.current;
            }
        }

        private KeyValuePair<K, V> next;
        public KeyValuePair<K, V> Next
        {
            get
            {
                return this.next;
            }
        }

        public bool HasNext { get; private set; }
        private bool ReadNextValue()
        {
            bool result;
            if (this.direction == Direction.Forward)
                result= this.ReadNextValueForward();
            else
                result = this.ReadNextValueReverse();

            //logger.Error("[mf] rnv hasnext=" + this.HasNext + ", count=" + this.Count + ", cons=" + this.CountConsumed + "-"+result);
                 
            return result;
        }

        private bool ReadNextKv()
        {

            this.HasNext = false;
            int keySize;
            try
            {
                keySize = this.breader.Read7BitEncodedInt();
            }
            catch 
            {
                return false;
            }
            this.HasNext = true;
            K key = this.keySerializer.Deserialize(this.breader);
            int ValSize = this.breader.Read7BitEncodedInt();
            V Val = this.valueSerializer.Deserialize(this.breader);
            this.next = new KeyValuePair<K, V>(key, Val);
            return true;
        }

        private bool ReadNextValueReverse()
        {
            //if we cannot backup 4 bytes
            this.HasNext = false;
            if (this.fileStream.Position < 4) return false;

            this.fileStream.Position -= 4;
            int recLen = this.breader.ReadInt32();
            this.fileStream.Position -= (4+recLen);

            bool result = this.ReadNextKv();
            
            //logger.Info("READ SET NEXT TO - " + this.next.Key.ToString());

            // back up and leave it ready to read forward...
            this.fileStream.Position -= recLen;
            return result;
        }


        private bool ReadNextValueForward()
        {
            this.HasNext = false;
            if (this.fileStream.Position >= this.fileStream.Length)
            {
                return false;
            }

            bool result = this.ReadNextKv();
            
            // read the rec len to be read for next forward read
            int junkRecLen = this.breader.ReadInt32();
            
            //logger.Info("READ SET NEXT TO - " + this.next.Key.ToString());
            return result;
        }

        public bool MoveNext()
        {
            // make the next value the current value and lookup the new
            // next value.
            if (!this.HasNext) return false;
            this.Count--;
            this.CountConsumed++;
            this.current = this.next;
            this.ReadNextValue();
            //logger.Info("MOVE SET CURRENT " + this.Current.Key.ToString());
            return true;
        }

        public void Close()
        {
            this.breader.Close();
        }
        public void Dispose()
        {
        }

        object System.Collections.IEnumerator.Current
        {
            get { return this.Current; }
        }

       

        public void Reset()
        {
            throw new NotImplementedException();
        }

        #region IMergeEnumerable<K,V> Members

        public IMergeEnumerator<K, V> GetMergeEnumerator()
        {
            return this;
        }
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return this;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this;
        }
        #endregion

    }


}
