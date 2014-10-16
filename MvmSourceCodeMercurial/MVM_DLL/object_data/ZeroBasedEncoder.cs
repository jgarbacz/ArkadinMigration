using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{

    /// <summary>
    /// This generic class encodes/decodes T items as zero based ints. It is thread safe.
    /// </summary>
    public class ZeroBasedEncoder<T>
    {

        // these structs are kept in sync
        private Dictionary<T, int> ItemIdxMap = new Dictionary<T, int>();
        private List<T> IdxItemMap = new List<T>();




        /// <summary>
        /// Returns number of encoded items
        /// </summary>
        public int Count
        {
            get
            {
                lock (this)
                {
                    return this.IdxItemMap.Count;
                }
            }
        }

        /// <summary>
        /// Returns this.Count-1
        /// </summary>
        public int MaxIndex
        {
            get
            {
                lock (this)
                {
                    return this.IdxItemMap.Count - 1;
                }
            }
        }


        /// <summary>
        /// Returns the idx for the item and sets IsNew to true if the item was new.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public int GetCreateIdx(T item, out bool isNew)
        {
            isNew = false;
            int idx;
            lock (this)
            {
                if (ItemIdxMap.TryGetValue(item, out idx))
                    return idx;
                isNew = true;
                idx = ItemIdxMap.Count;
                ItemIdxMap[item] = idx;
                IdxItemMap.Add(item);
                return idx;
            }
        }

        /// <summary>
        /// Returns the index of the item, creating a new one if needed.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetCreateIdx(T item)
        {
            int idx;
            lock (this)
            {
                if (ItemIdxMap.TryGetValue(item, out idx))
                    return idx;
                idx = ItemIdxMap.Count;
                ItemIdxMap[item] = idx;
                IdxItemMap.Add(item);
                return idx;
            }
        }

        /// <summary>
        /// Gets the index for the item or errors if item is invalid
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetIdx(T item)
        {
            int idx;
            lock (this)
            {
                if (ItemIdxMap.TryGetValue(item, out idx))
                    return idx;
                throw new Exception("Error, no idx for item=[" + item.ToString() + "] not found");
            }
        }

        /// <summary>
        /// Gets the index for the item if it exists and returns true, else return false.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool TryGetIdx(T item, out int idx)
        {
            lock (this)
            {
                return ItemIdxMap.TryGetValue(item, out idx);
            }
        }

        /// <summary>
        /// Returns the item of the index or errors if the index is invalid
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public T GetItem(int idx)
        {
            lock (this)
            {
                if (idx >= IdxItemMap.Count) throw new Exception("Error, no item for idx=[" + idx + "] not found");
                return IdxItemMap[idx];
            }
        }

        /// <summary>
        /// Returns the item of the index or errors if the index is invalid
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool TryGetItem(int idx, out T item)
        {
            lock (this)
            {
                if (idx >= IdxItemMap.Count)
                {
                    item = default(T);
                    return false;
                }
                else
                {
                    item = IdxItemMap[idx];
                    return true;
                }
            }
        }


        /// <summary>
        /// Gets items after and including the passed from idx. GetItems(0) returns all items.
        /// </summary>
        /// <param name="fromIdx"></param>
        /// <returns></returns>
        public T[] GetItems(int startIdx)
        {
            lock (this)
            {
                int size = this.IdxItemMap.Count - startIdx;
                return this.IdxItemMap.GetRange(startIdx, size).ToArray();
            }

        }
    }
}
