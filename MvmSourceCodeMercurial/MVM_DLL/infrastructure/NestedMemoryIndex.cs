using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace MVM
{
    public class NestedMemoryIndex :IIndex
    {
        public Dictionary<string, int> FieldIndexes { get { return this.fieldIndexes; } }
        private Dictionary<string, int> fieldIndexes = new Dictionary<string, int>();
        private List<string> orderedFieldNames;
        private List<string> orderedKeyFields;

        // CORE INDEX HERE
        // here we have a Dictionary<string,object> which based on depth is points to another
        // Dictionary<string,object> or List<string[]>>
        public Dictionary<string, object> index = new Dictionary<string, object>();

        CursorOrder defaultLoopOrder;
        public bool useContext;
        private bool unique;
        private int indexCount = 0;

        public string IndexName { get; private set; }

        // create an empty NestedMemoryIndex
        public NestedMemoryIndex(string indexName, List<string> orderedFieldNames, List<string> orderedKeyFields, CursorOrder defaultCursorOrder, bool useContext, bool unique)
        {
            this.IndexName = indexName;
            this.useContext = useContext;
            this.unique = unique;
            this.defaultLoopOrder = defaultCursorOrder;
            if (this.defaultLoopOrder == CursorOrder.Default) this.defaultLoopOrder = CursorOrder.Forward;
            this.orderedFieldNames = orderedFieldNames;
            this.orderedKeyFields = orderedKeyFields;
            int id = 0;
            Dictionary<string, int> temp_keys = new Dictionary<string, int>();
            foreach (string k in this.orderedKeyFields)
            {
                temp_keys[k] = 1;
            }
            foreach (string f in this.orderedFieldNames)
            {
                if (!temp_keys.ContainsKey(f))
                {
                    // Unlike other index types, nested indexes do not store the keys in the row
                    this.fieldIndexes[f] = id++;
                }
            }
        }
        public bool NestedKeys()
        {
            return true;
        }

        public bool UseContext()
        {
            return useContext;
        }

        public bool IsStatic()
        {
            return true;
        }

        public int RecursiveCount(object node, List<string> keys, int numKeys, int depth)
        {
            object newnode;
            Dictionary<string, object> ptr;
            string key = null;
            if (depth < keys.Count)
            {
                key = keys[depth];
            }
            if (key != null)
            {
                ptr = node as Dictionary<string, object>;
                if (!ptr.TryGetValue(key, out newnode))
                {
                    return 0;
                }
                else
                {
                    return RecursiveCount(newnode, keys, numKeys, depth + 1);
                }
            }
            else
            {
                if (depth >= numKeys)
                {
                    List<string[]> rows = node as List<string[]>;
                    if (rows != null) {
                        return rows.Count;
                    } else {
                        throw new Exception("Invalid index structure");
                    }
                }
                else
                {
                    int ctr = 0;
                    ptr = node as Dictionary<string, object>;
                    foreach (var entry in ptr)
                    {
                        ctr += RecursiveCount(entry.Value, keys, numKeys, depth + 1);
                    }
                    return ctr;
                }
            }
        }

        public int GetCount(List<string> keys)
        {
            if (keys.Count == 0)
            {
                return indexCount;
            }
            return RecursiveCount(this.index, keys, this.orderedKeyFields.Count, 0);
        }

        public string IndexUpdate(ModuleContext mc, List<string> orderedKeyValues, Dictionary<string, string> updateValues)
        {
            object objPtr = null;
            Dictionary<string, object> dicPtr = this.index;
            foreach (string key in orderedKeyValues)
            {
                if (!dicPtr.TryGetValue(key, out objPtr))
                    return "";
                dicPtr = objPtr as Dictionary<string, object>;
            }
            List<string[]> currvals = objPtr as List<string[]>;
            foreach (var row in currvals)
            {
                foreach (var upd in updateValues)
                {
                    row[this.fieldIndexes[upd.Key]] = upd.Value;
                }
            }
            return "1";
        }

        public bool IndexGetRow(ModuleContext mc, string[] orderedKeyValues, out string[] values)
        {
            object objPtr = null;
            Dictionary<string, object> dicPtr = this.index;
            foreach (string key in orderedKeyValues)
            {
                if (!dicPtr.TryGetValue(key, out objPtr))
                {
                    values = null;
                    return false;
                }
                dicPtr = objPtr as Dictionary<string, object>;
            }
            List<string[]> currvals = objPtr as List<string[]>;
            values = currvals[0];
            return true;
        }

        public string IndexGet(ModuleContext mc, List<string> orderedKeyValues, Dictionary<string, string> values)
        {
            object objPtr = null;
            Dictionary<string, object> dicPtr = this.index;
            string[] keys = values.Keys.ToArray();
            foreach (string key in orderedKeyValues)
            {
                if (!dicPtr.TryGetValue(key, out objPtr))
                {
                    foreach (string kv in keys)
                    {
                        values[kv] = "";
                    }
                    return "";
                }
                dicPtr = objPtr as Dictionary<string, object>;
            }
            List<string[]> currvals = objPtr as List<string[]>;
            foreach (string kv in keys)
            {
                values[kv] = currvals[0][this.fieldIndexes[kv]];
            }
            return "1";
        }

        /// <summary>
        /// return the keys in their natural newOrder
        /// </summary>
        /// <returns></returns>
        public List<string> GetOrderedKeyFields()
        {
            return orderedKeyFields;
        }

        /// <summary>
        /// Lets use pass left to right keys and results in all values to the right.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="orderedKeyValues"></param>
        /// <param name="newOrder"></param>
        /// <returns></returns>
        public ICursor IndexSelect(ModuleContext mc, ICursorSetupCommon cursorSetup, List<string> orderedKeyValues, CursorOrder order)
        {
            CursorOrder myOrder = order;
            if (myOrder == CursorOrder.Default) myOrder = this.defaultLoopOrder;
            var csr = new NestedMemoryIndexCursor(mc, cursorSetup, this.orderedKeyFields, this.orderedFieldNames, orderedKeyValues, myOrder, this.index);
            return csr;
        }

        /// <summary>
        /// Expects keys to be passed left to right. Not all keys should be passed. If all
        /// are passed then there is nothing to do a keys on.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="orderedFieldValues"></param>
        /// <returns></returns>
        public ICursor IndexSelectKeys(ModuleContext mc, ICursorSetupCommon cursorSetup, List<string> orderedKeyValues)
        {
            // this is the name of the key we are selecting
            string keyName = this.orderedKeyFields[orderedKeyValues.Count];
            // deref all the passed keys
            object objPtr;
            Dictionary<string, object> dicPtr = this.index;
            foreach (string key in orderedKeyValues)
            {
                if (!dicPtr.TryGetValue(key, out objPtr))
                    return new NullCursor(mc, cursorSetup);
                dicPtr = objPtr as Dictionary<string, object>;
            }
            if (dicPtr == null) return new NullCursor(mc, cursorSetup);
            // create a cursor with the keys at this level
            List<string> rows = new List<string>(); //note: this is a copy to avoid concurrent modification issues
            foreach (string x in dicPtr.Keys) rows.Add(x);
            var csr = new ListOfStringCursor(mc, cursorSetup, keyName, rows);
            return csr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="orderedFieldValues"></param>
        /// <returns></returns>
        public string IndexInsertIfNone(ModuleContext mc, List<string> orderedFieldValues)
        {
            // copied the regular insert and added this flag
            bool createdList = false; 

            // insert/deref all the keys that were passed
            int idx = 0;
            int maxKeyCount = orderedFieldValues.Count < this.orderedKeyFields.Count //not quite right??
                ? orderedFieldValues.Count
                : this.orderedKeyFields.Count;

            object objPtr = this.index; ;
            for (idx = 0; idx < maxKeyCount; idx++)
            {
                Dictionary<string, object> dicPtr = objPtr as Dictionary<string, object>;
                string key = orderedFieldValues[idx];
                if (dicPtr.ContainsKey(key))
                {
                    objPtr = dicPtr[key];
                }
                else
                {
                    if (idx == (this.orderedKeyFields.Count - 1))
                    {
                        objPtr = new List<string[]>();
                        dicPtr[key] = objPtr;
                        createdList = true;
                    }
                    else
                    {
                        objPtr = new Dictionary<string, object>();
                        dicPtr[key] = objPtr;
                    }
                }
            }

            // if we didn't add the list, then we're not the first row, so return ''
            if (!createdList) return "";


            // if the targetObj ptr is now a row ptr push a row in with the rest of the values
            List<string[]> lstPtr = objPtr as List<string[]>;
            if (lstPtr != null && idx < orderedFieldValues.Count)
            {
                string[] row = new string[orderedFieldValues.Count - idx];
                int colNo = 0;
                for (; idx < orderedFieldValues.Count; idx++)
                {
                    row[colNo++] = orderedFieldValues[idx];
                }
                lstPtr.Add(row);
            }
            indexCount++;

            return "1";
        }
      
        /// <summary>
        /// Inserts into the index. Expects field values to be passed left to right,
        /// but not all must be passed. We only fill out with the values we have.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="orderedFieldValues"></param>
        public void IndexInsert(ModuleContext mc, List<string> orderedFieldValues)
        {
            // insert/deref all the keys that were passed
            int idx = 0;
            int maxKeyCount = orderedFieldValues.Count < this.orderedKeyFields.Count //not quite right??
                ? orderedFieldValues.Count 
                : this.orderedKeyFields.Count;
            
            object objPtr = this.index; ;
            for (idx = 0; idx < maxKeyCount; idx++)
            {
                Dictionary<string, object> dicPtr = objPtr as Dictionary<string, object>;
                string key=orderedFieldValues[idx];
                if (dicPtr.ContainsKey(key))
                {
                    objPtr = dicPtr[key];
                }
                else
                {
                    if (idx == (this.orderedKeyFields.Count - 1))
                    {
                        objPtr = new List<string[]>();
                        dicPtr[key] = objPtr;
                    }
                    else
                    {
                        objPtr = new Dictionary<string,object>();
                        dicPtr[key] = objPtr;
                    }
                }
            }
            // if the targetObj ptr is now a row ptr push a row in with the rest of the values
            List<string[]> lstPtr = objPtr as List<string[]>;
            if (lstPtr !=null && idx < orderedFieldValues.Count)
            {
                if (this.unique && lstPtr.Count > 0)
                {
                    throw new Exception("Cannot insert duplicates [" + orderedFieldValues.Take(idx).ToList().Join(",") + "] into unique index: " + this.IndexName);
                }

                string[] row=new string[orderedFieldValues.Count-idx];
                int colNo = 0;
                for (; idx < orderedFieldValues.Count; idx++)
                {
                    row[colNo++] = orderedFieldValues[idx];
                }
                lstPtr.Add(row);
            }
            indexCount++;
        }

        /// <summary>
        /// Returns the value fields in newOrder
        /// </summary>
        /// <returns></returns>
        public List<string> GetOrderedValueFields()
        {
            return this.orderedFieldNames;
        }

        /// <summary>
        /// Clears the entire index
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        public string IndexClear(ModuleContext mc)
        {
            indexCount = 0;
            index.Clear();
            return "1";
        }

        public void IndexClose(ModuleContext mc)
        {
            throw new NotImplementedException();
        }

        public void IndexDrop(ModuleContext mc)
        {
            this.IndexClear(mc);
            mc.globalContext.RmNamedClassInst(this.IndexName);
        }

        /// <summary>
        /// Backs keys out of the index.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="orderedKeyValues"></param>
        /// <returns></returns>
        public string IndexRemove(ModuleContext mc, List<string> orderedKeyValues, IndexRemovalOption removalOption)
        {
            // if the user did not pass any keys, we should just clear the structure
            if (orderedKeyValues.Count == 0)
            {
                this.IndexClear(null);
                return "1";
            }

            // keep list of dicPtrs as we go so we can back out cleanly.
            List<Dictionary<string, object>> dicPtrs = new List<Dictionary<string, object>>();

            // dereference as far as you can and then RemoveSpecificItem the last value.
            object objPtr = this.index;
            foreach (string k in orderedKeyValues)
            {
                // more keys but objPtr is null means we have nothing to RemoveSpecificItem
                if (objPtr == null)
                {
                    return "0";
                }
                Dictionary<string, object> dicPtr = objPtr as Dictionary<string, object>;
                if (dicPtr.ContainsKey(k))
                {
                    objPtr = dicPtr[k];
                }
                else
                {
                    // if we get here we have a key that does not match so nothing to RemoveSpecificItem
                    return "0";
                }
                dicPtrs.Add(dicPtr);
            }
            
            // if we get here we have dereferenced all the keys so back the entry out.
            int maxIndex = dicPtrs.MaxIndex();
            for (int i = maxIndex; i >= 0; i--)
            {
                // RemoveSpecificItem the key a the current ptr
                if (i == maxIndex && removalOption != IndexRemovalOption.All)
                {
                    object cvals;
                    dicPtrs[i].TryGetValue(orderedKeyValues[i], out cvals);
                    List<string[]> currvals = cvals as List<string[]>;
                    if (currvals != null && currvals.Count > 1)
                    {
                        if (removalOption == IndexRemovalOption.First)
                        {
                            currvals.RemoveAt(0);
                        }
                        else
                        {
                            currvals.RemoveAt(currvals.Count - 1);
                        }
                        indexCount--;
                    }
                    else
                    {
                        if (currvals != null) indexCount -= currvals.Count;
                        dicPtrs[i].Remove(orderedKeyValues[i]);
                    }
                }
                else
                {
                    if (i == maxIndex)
                    {
                        object cvals;
                        dicPtrs[i].TryGetValue(orderedKeyValues[i], out cvals);
                        List<string[]> currvals = cvals as List<string[]>;
                        if (currvals != null) indexCount -= currvals.Count;
                    }
                    dicPtrs[i].Remove(orderedKeyValues[i]);
                }
                if (dicPtrs[i].Count > 0) break ;
            }
            return "1";
        }

        #region IIndex Members


        public void PushSlice(MvmEngine mvm, string[] orderedKeyValues, SocketHandler socketHandler, bool clearSource, string tgtName, string[] tgtRecordTemplate, List<KeyValuePair<int, int>> tgtIdxSrcIdxMap)
        {
            throw new NotImplementedException();
        }

        public void PushAll(MvmEngine mvm, SocketHandler socketHandler, bool clearSource, string tgtName, string[] tgtRecordTemplate, List<KeyValuePair<int, int>> tgtIdxSrcIdxMap)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class NestedMemoryIndexCursor : CursorCommonLinqEnabled, ICursor
    {
        private object structure;
        private List<string> orderedKeyNames;
        private List<string> orderedKeyValues;
        private Walker walker;
        private long rowCtr = 0;
        public NestedMemoryIndexCursor(ModuleContext mc, ICursorSetupCommon cursorSetup, List<string> orderedKeyNames, List<string> orderedFieldNames, List<string> orderedKeyValues, CursorOrder cursorOrder, object structure)
            : base(mc, cursorSetup)
        {
            this.orderedFieldNames = orderedFieldNames;
            this.orderedKeyNames = orderedKeyNames;
            this.orderedKeyValues = orderedKeyValues;
            this.structure = structure;


            //// dereference the all the passed key values
            //object objPtr=this.structure;
            //Dictionary<string, object> dicPtr;
            //List<string[]> lstPtr;
            //foreach (string k in this.orderedKeyValues)
            //{
            //    // more keys but dicPtr is null means we select nothing
            //    if (objPtr == null)
            //    {
            //        this.SetEof();
            //        return;
            //    }
            //    dicPtr = objPtr as Dictionary<string, object>;
            //    if (dicPtr.ContainsKey(k))
            //    {
            //        objPtr = dicPtr[k];
            //    }
            //    else
            //    {
            //        this.SetEof();
            //        return;
            //    }
            //}
            //// if we got null here, the passed keys are still valid so we need to return a single row
            //if (objPtr == null)
            //{
            //    // if we didnt even dereference any keys then 
            //    if (this.orderedKeyValues.Count == 0)
            //    {
            //        this.SetEof();
            //        return;
            //    }
            //    // if we dereferenced keys, we should end up selecting 1 row so set the row
            //    // now and leave the walker null so it doesn't get another row
            //    else
            //    {
            //        using (IObjectData csrObj = this.CreateNewObject())
            //        {
            //            int fldIdx = -1;
            //            foreach (string passedKeyValue in this.orderedKeyValues)
            //            {
            //                fldIdx++;
            //                csrObj[this.orderedFieldNames[fldIdx]] = passedKeyValue;
            //            }
            //        }
            //        this.walker=null;
            //        return;
            //    }
            //}
            //// create the appropriate walker
            //dicPtr = objPtr as Dictionary<string, object>;
            //if (dicPtr != null)
            //    this.walker = new DictionaryWalker(dicPtr);
            //else
            //{
            //    lstPtr = objPtr as List<string[]>;
            //    this.walker = new ListWalker(lstPtr);
            //}

            //// call next on the walker
            //this.Next(mc);
        }

        // using (IObjectData csrObj = this.CreateNewObject())

        public override IObjectData CursorNext()
        {
           

                //////////////////////////////////////////////////
                // First row only code
                //////////////////////////////////////////////////
                if (++this.rowCtr == 1)
                {
                    // dereference the all the passed key values
            object objPtr=this.structure;
                    Dictionary<string, object> dicPtr;
                    List<string[]> lstPtr;
                    foreach (string k in this.orderedKeyValues)
                    {
                        // more keys but dicPtr is null means we select nothing
                        if (objPtr == null)
                        {
                            return null;
                        }
                        dicPtr = objPtr as Dictionary<string, object>;
                        if (dicPtr.ContainsKey(k))
                        {
                            objPtr = dicPtr[k];
                        }
                        else
                        {
                            return null;
                        }
                    }
                    // if we got null here, the passed keys are still valid so we need to return a single row
                    if (objPtr == null)
                    {
                        // if we didnt even dereference any keys then 
                        if (this.orderedKeyValues.Count == 0)
                        {
                            return null;
                        }
                        // if we dereferenced keys, we should end up selecting 1 row so set the row
                        // now and leave the walker null so it doesn't get another row
                        using (IObjectData csrObj = this.CreateNewObject())
                        {
                            int fldIdx = -1;
                            foreach (string passedKeyValue in this.orderedKeyValues)
                            {
                                fldIdx++;
                                csrObj[this.orderedFieldNames[fldIdx]] = passedKeyValue;
                            }

                    this.walker=null;
                            return csrObj;
                        }

                    }
                    // create the appropriate walker
                    dicPtr = objPtr as Dictionary<string, object>;
                    if (dicPtr != null)
                        this.walker = new DictionaryWalker(dicPtr);
                    else
                    {
                        lstPtr = objPtr as List<string[]>;
                        this.walker = new ListWalker(lstPtr);
                    }
                }


                //////////////////////////////////////////////////
                // always on code
                //////////////////////////////////////////////////
                {
                    if (this.walker == null)
                    {
                        return null;
                    }

                    List<string> walkedValues = new List<string>();
                    // if no more values set eof and return
                    if (!this.walker.Next(walkedValues))
                    {
                        return null;
                    }
                    using (IObjectData csrObj = this.CreateNewObject())
                    {
                        // otherwise update the cursor
                        int fldIdx = -1;
                        // TBD: if we want we could set the keys on cursor instanciation
                        // and not ever update them again.
                        foreach (string passedKeyValue in this.orderedKeyValues)
                        {
                            fldIdx++;
                            csrObj[this.orderedFieldNames[fldIdx]] = passedKeyValue;
                        }
                        foreach (string walkedValue in walkedValues)
                        {
                            fldIdx++;
                            csrObj[this.orderedFieldNames[fldIdx]] = walkedValue;
                        }
                        return csrObj;
                    }
                }
            
        }

        public override void CursorClear()
        {
        }

        public interface Walker
        {
            bool Next(List<string> curKeys);
        }

        public class ListWalker : Walker
        {
            public List<string[]> rows;
            public int rowIdx = -1;
            public ListWalker(List<string[]> rows)
            {
                this.rows = rows;
            }
            public bool Next(List<string> curKeys)
            {
                if (++rowIdx > rows.MaxIndex()) return false;
                string[] row = rows[this.rowIdx];
                string[] orderedFieldValues = this.rows[rowIdx];
                for (int i = 0; i < row.Length; i++)
                {
                    curKeys.Add(row[i]);
                }
                return true;
            }
        }

        public class DictionaryWalker : Walker
        {
            public Dictionary<string, object> map;
            public int keyIdx = -1;
            public List<string> keys;
            public string curKey;
            public Walker child;
            public DictionaryWalker(Dictionary<string, object> map)
            {
                this.map = map;
                this.keys = map.Keys.ToList();
            }
            // return true if it successfully added a key, else false
            public bool Next(List<string> curKeys)
            {
                // if we already have a child setup, just call next one child
                // if the child adds keys, return true, otherwise null out the child
                if (child != null)
                {
                    curKeys.Add(this.curKey);
                    if (this.child.Next(curKeys)) return true;
                    // otherwise null out the child 
                    // and RemoveSpecificItem our current key
                    curKeys.RemoveLast();
                    this.child = null;
                }

                // if we are here we have no child to advance so we need to advance our own keys

                // if we don't have another key, just return false which indicates that
                if (++this.keyIdx > (this.keys.MaxIndex())) return false;

                // get the next key and add it curKeys
                this.curKey = this.keys[this.keyIdx];
                curKeys.Add(this.curKey);
                // if someone removed this key out from under us goto the next key 
                // still return the key but there will be no child keys/values
                object value;
                this.map.TryGetValue(this.curKey, out value);
                // if we dont have a child to get keys from then our key is still valid
                // so return true
                if (value == null) return true;
                // otherwise value is not null so we need to setup a child and try to get 
                // a key from it.
                if (value is Dictionary<string, object>)
                {
                    this.child = new DictionaryWalker(value as Dictionary<string, object>);
                }
                else
                {
                    this.child = new ListWalker(value as List<string[]>);
                }
                this.child.Next(curKeys);
                return true;
            }
        }
    }
}
