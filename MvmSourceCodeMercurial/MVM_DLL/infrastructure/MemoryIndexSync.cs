using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MVM
{
    /**
     * Thread save implementation of the memory index which has a compound hash key and 
     * a list of records. 
     * There are 2 tiers of locks: hash and record list.
     * Locks are not held between modules so we cannot deadlock.
     */
    public class MemoryIndexSync : IIndex
    {
        public Dictionary<string, int> FieldIndexes { get { return this.fieldIndexes; } }
        private Dictionary<string, int> fieldIndexes = new Dictionary<string, int>();
        private List<string> orderedFieldNames;
        private List<string> orderedKeyFields;
        private List<int> orderedKeyFieldsIdx = new List<int>();

        // CORE INDEX HERE
        //public Dictionary<StringArray, List<string[]>> index = new Dictionary<StringArray, List<string[]>>();
        // TCF: moving the initialization of the index to the constructor, where we can define its capacity
        public Dictionary<StringArray, List<string[]>> index;

        CursorOrder defaultLoopOrder;
        private bool useContext;
        private bool unique;
        private int indexCount = 0;

        private List<int> objectIdFieldIdxes = new List<int>();
        private Dictionary<int, int> isObjectIdFieldIdx = new Dictionary<int, int>();

        private bool IsObjectIdFieldIndex(int fieldIdx)
        {
            return isObjectIdFieldIdx.ContainsKey(fieldIdx);
        }
        public bool HasObjectIdFields { get { return this.objectIdFieldIdxes != null && this.objectIdFieldIdxes.Count >= 1; } }

        MvmEngine mvm;

        public string IndexName { get; private set; }

        // Default the index to 4 items if the caller does not wish to specify
        public MemoryIndexSync(MvmEngine mvm, string indexName, List<string> orderedFieldNames, List<string> orderedKeyFields, CursorOrder defaultCursorOrder, 
            bool useContext, bool unique, List<int> objectIdFieldIdxes) : 
            this(mvm, indexName, orderedFieldNames, orderedKeyFields, defaultCursorOrder, useContext, unique, objectIdFieldIdxes, 4) {     
        }

        // create an empty MemoryIndexSync
        public MemoryIndexSync(MvmEngine mvm, string indexName, List<string> orderedFieldNames, List<string> orderedKeyFields, CursorOrder defaultCursorOrder, 
            bool useContext, bool unique, List<int> objectIdFieldIdxes, int indexCapacity)
        {
            /*mvm.Log("***MemoryIndexSync Memory Check #1:");
            mvm.Log(String.Format("Memory used: {0}", GC.GetTotalMemory(true).ToString()));
            mvm.Log(String.Format("Memory(2) used: {0}", System.Diagnostics.Process.GetCurrentProcess().WorkingSet64.ToString()));*/

            this.mvm = mvm;

            // New added by TCF...provide ability to define index capacity to limit fragmentation from appending to it later
            this.index = new Dictionary<StringArray, List<string[]>>(indexCapacity);

            /*mvm.Log("***MemoryIndexSync Memory Check #2:");
            mvm.Log(String.Format("Memory used: {0}", GC.GetTotalMemory(true).ToString()));
            mvm.Log(String.Format("Memory(2) used: {0}", System.Diagnostics.Process.GetCurrentProcess().WorkingSet64.ToString()));*/

            this.IndexName = indexName;
            this.useContext = useContext;
            this.unique = unique;
            this.defaultLoopOrder = defaultCursorOrder;
            if (this.defaultLoopOrder == CursorOrder.Default) this.defaultLoopOrder = CursorOrder.Forward;
            this.orderedFieldNames = orderedFieldNames;
            this.orderedKeyFields = orderedKeyFields;
            foreach (string f in this.orderedKeyFields)
            {
                int idx = this.orderedFieldNames.IndexOf(f);
                if (idx < 0) 
                    throw new Exception("Error, key fields must also be value fields");
                this.orderedKeyFieldsIdx.Add(idx);
            }
            int id = 0;
            foreach (string f in this.orderedFieldNames)
            {
                this.fieldIndexes[f] = id++;
            }
            this.objectIdFieldIdxes = objectIdFieldIdxes;
            this.objectIdFieldIdxes.ForEach(i => this.isObjectIdFieldIdx[i] = i);

            /*mvm.Log("***MemoryIndexSync Memory Check #3:");
            mvm.Log(String.Format("Memory used: {0}", GC.GetTotalMemory(true).ToString()));
            mvm.Log(String.Format("Memory(2) used: {0}", System.Diagnostics.Process.GetCurrentProcess().WorkingSet64.ToString()));*/
        }

        public bool NestedKeys()
        {
            return false;
        }

        public bool UseContext()
        {
            return useContext;
        }

        public bool IsStatic()
        {
            return true;
        }

        public int GetCount(List<string> keys)
        {
            if (keys.Count == 0)
            {
                return indexCount;
            }
            StringArray compoundKey = new StringArray(keys.ToArray());
            if (this.index.ContainsKey(compoundKey))
            {
                return this.index[compoundKey].Count;
            }
            return 0;
        }

        public List<string> GetOrderedKeyFields()
        {
            return orderedKeyFields;
        }

        public ICursor IndexSelect(ModuleContext mc, ICursorSetupCommon cursorSetup, List<string> orderedKeyValues, CursorOrder order)
        {
            CursorOrder myOrder = order;
            if (myOrder == CursorOrder.Default) myOrder = this.defaultLoopOrder;
            StringArray compoundKey = new StringArray(orderedKeyValues.ToArray());
            ICursor csr;
            List<string[]> rows = index.GetValueDefaulted(compoundKey, null);
            if (myOrder == CursorOrder.Forward) csr = new ListOfStringArrayFieldsCursorSync(mc, cursorSetup, this.orderedFieldNames, rows);
            else if (myOrder == CursorOrder.Reverse) csr = new ListOfStringArrayFieldsCursorReverseSync(mc, cursorSetup, this.orderedFieldNames, rows);
            else throw new Exception("Unexpected cursor order:" + myOrder);
            return csr;
        }

        public ICursor IndexSelectKeys(ModuleContext mc, ICursorSetupCommon cursorSetup, List<string> notused)
        {
            // Cursor which does NOT need synchronization since we snapshoted the keys.
            List<StringArray> rows = index.Keys.ToList(); //note: this is a copy to avoid concurrent modification issues
            ICursor csr = new ListOfStringArrayObjFieldsCursor(mc, cursorSetup, this.orderedKeyFields, rows);
            return csr;
        }


        private void ObjectFieldRefGet(List<string> orderedFieldValues)
        {
            // If inserting a field of type object_id, then bumps
            // the ref counter for that object.
            if (this.HasObjectIdFields)
            {
                foreach (int oidFieldIdx in this.objectIdFieldIdxes)
                {
                    string oid = orderedFieldValues[oidFieldIdx];
                    if (oid.NotNullOrEmpty())
                    {
                        mvm.objectCache.RefGet(oid);
                    }
                }
            }
        }

        private void ObjectFieldRefRemove(string[] orderedFieldValues)
        {
            // If removing a field of type object_id, then decrement
            // the ref counter for that object.
            if (this.HasObjectIdFields)
            {
                foreach (int oidFieldIdx in this.objectIdFieldIdxes)
                {
                    string oid = orderedFieldValues[oidFieldIdx];
                    if (oid.NotNullOrEmpty())
                    {
                        mvm.objectCache.RefRemove(oid);
                    }
                }
            }
        }

        /// <summary>
        /// Inserts a row into the index. 
        /// </summary>
        /// <param name="mcNotUsed"></param>
        /// <param name="orderedFieldValues"></param>
        public void IndexInsert(ModuleContext mcNotUsed, List<string> orderedFieldValues)
        {

            this.ObjectFieldRefGet(orderedFieldValues);

            // Build the string[] record to insert.
            string[] record = orderedFieldValues.ToArray();

            // Build the compound key
            List<string> orderedKeyValues = new List<string>();
            foreach (int idx in this.orderedKeyFieldsIdx)
            {
                orderedKeyValues.Add(orderedFieldValues[idx]);
            }
            StringArray compoundKey = new StringArray(orderedKeyValues.ToArray());

            // Lookup records
            List<string[]> records;
            lock (this.index)
            {
                // If first row, add record list with long record and return
                if (!this.index.TryGetValue(compoundKey, out records))
                {
                    records = new List<string[]>() { record };
                    this.index[compoundKey] = records;
                    indexCount++;
                    return;
                }
                else if (this.unique)
                {
                   throw new Exception("Cannot insert duplicates [" + compoundKey.Join(",") + "] into unique index: " + this.IndexName);
                }
            }

            // Add the record to records
            lock (records)
            {
                records.Add(record);
                indexCount++;
            }
        }


        public string IndexInsertIfNone(ModuleContext mcNotUsed, List<string> orderedFieldValues)
        {
            // Build the compound key
            List<string> orderedKeyValues = new List<string>();
            foreach (int idx in this.orderedKeyFieldsIdx)
            {
                orderedKeyValues.Add(orderedFieldValues[idx]);
            }
            StringArray compoundKey = new StringArray(orderedKeyValues.ToArray());

            // Lookup records
            lock (this.index)
            {
                // if already has an entry do nothing
                if (this.index.ContainsKey(compoundKey))
                    return "";


                this.ObjectFieldRefGet(orderedFieldValues);

                // Build the string[] record to insert.
                string[] record = orderedFieldValues.ToArray();
                // create a new records list with the single record
                List<string[]> records = new List<string[]>() { record };
                this.index[compoundKey] = records;
                indexCount++;
            }
            return "1";
        }

        public List<string> GetOrderedValueFields()
        {
            return this.orderedFieldNames;
        }

        #region IIndex Members

        public string IndexUpdate(ModuleContext mc, List<string> orderedKeyValues, Dictionary<string, string> updateValues)
        {
            StringArray compoundKey = new StringArray(orderedKeyValues.ToArray());
            lock (this.index)
            {
                if (this.index.ContainsKey(compoundKey))
                {
                    List<string[]> currvals;
                    index.TryGetValue(compoundKey, out currvals);
                    foreach (var row in currvals)
                    {
                        foreach (var upd in updateValues)
                        {
                            row[this.fieldIndexes[upd.Key]] = upd.Value;
                        }
                    }
                    return "1";
                }
            }
            return "";
        }

        public bool IndexGetRow(ModuleContext mc, string[] orderedKeyValues, out string[] values)
        {
            StringArray compoundKey = new StringArray(orderedKeyValues);
            List<string[]> currvals;
            if (index.TryGetValue(compoundKey, out currvals))
            {
                values = currvals[0];
                return true;
            }
            values = null;
            return false;
        }

        public string IndexGet(ModuleContext mc, List<string> orderedKeyValues, Dictionary<string, string> values)
        {
            StringArray compoundKey = new StringArray(orderedKeyValues.ToArray());
            if (this.index.ContainsKey(compoundKey))
            {
                List<string[]> currvals;
                index.TryGetValue(compoundKey, out currvals);
                string[] keys = values.Keys.ToArray();
                foreach (var kv in keys)
                {
                    values[kv] = currvals[0][this.fieldIndexes[kv]];
                }
                return "1";
            }
            return "";
        }

        public string IndexClear(ModuleContext mcNotUsed)
        {
            // If there are object id fields, we need to go through every row
            // and release the object_id reference.
            lock (this.index)
            {
                if (this.HasObjectIdFields)
                {
                    foreach (var rows in this.index.Values)
                    {
                        foreach (var row in rows)
                        {
                            this.ObjectFieldRefRemove(row);
                        }
                    }
                }
                indexCount = 0;
                index.Clear();
            }
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

        public string IndexRemove(ModuleContext mcNotUsed, List<string> orderedKeyValues, IndexRemovalOption removalOption)
        {
            // Build the compound key
            StringArray compoundKey = new StringArray(orderedKeyValues.ToArray());
            if (this.RemoveCompundKey(compoundKey, removalOption)) return "1";
            return "0";
        }

        /// <summary>
        /// Removes the compound key, decrementing object ref ctrs as needed.
        /// </summary>
        /// <param name="compoundKey"></param>
        /// <returns></returns>
        private bool RemoveCompundKey(StringArray compoundKey, IndexRemovalOption removalOption)
        {
            lock (this.index)
            {
                if (this.index.ContainsKey(compoundKey))
                {
                    if (removalOption != IndexRemovalOption.All)
                    {
                        List<string[]> currvals;
                        this.index.TryGetValue(compoundKey, out currvals);
                        if (currvals.Count > 1)
                        {
                            if (removalOption == IndexRemovalOption.First)
                            {
                                if (this.HasObjectIdFields) this.ObjectFieldRefRemove(currvals[0]);
                                currvals.RemoveAt(0);
                            }
                            else
                            {
                                if (this.HasObjectIdFields) this.ObjectFieldRefRemove(currvals[currvals.Count - 1]);
                                currvals.RemoveAt(currvals.Count - 1);
                            }
                            indexCount--;
                            return true;
                        }
                    }
                    if (this.HasObjectIdFields)
                    {
                        foreach (var row in this.index[compoundKey])
                        {
                            this.ObjectFieldRefRemove(row);
                        }
                    }
                    indexCount -= this.index[compoundKey].Count;
                    this.index.Remove(compoundKey);
                    return true;
                }
            }
            // Nothing to remove 
            return false;
        }



        /// <summary>
        /// Pushes a slice of the memory index.
        /// </summary>
        public void PushSlice(MvmEngine mvm, string[] orderedKeyValues, SocketHandler socketHandler, bool clearSource, string tgtName, string[] tgtRecordTemplate, List<KeyValuePair<int, int>> tgtIdxSrcIdxMap)
        {
            Dictionary<string, bool> sentObjectIds = new Dictionary<string, bool>();
            // Build the compound key
            StringArray compoundKey = new StringArray(orderedKeyValues);

            // List of records to push
            List<string[]> records;

            // lock the index
            lock (this.index)
            {
                // If no records, nothing to push
                if (!this.index.TryGetValue(compoundKey, out records)) return;
            }

            // Push the records
            lock (records)
            {
                foreach (var srcRecord in records)
                {
                    string[] outRecord = tgtRecordTemplate.Clone() as string[];
                    foreach (var kv in tgtIdxSrcIdxMap)
                    {
                        outRecord[kv.Key] = srcRecord[kv.Value];
                        this.SendObjectIfNeeded(mvm, socketHandler, srcRecord, kv.Value, sentObjectIds);
                    }
                    // send a push message.
                    //Console.WriteLine("ABOUT TO SEND:" + outRecord.Join(","));
                    PushIndexMessage msg = new PushIndexMessage(tgtName, outRecord);
                    socketHandler.messageOutbox.Add(msg);
                }
            }

            // if we are supposed to clear the source, then do so.
            if (clearSource) this.RemoveCompundKey(compoundKey, IndexRemovalOption.All);
        }




        /// <summary>
        /// Pushes entire memory index.
        /// </summary>
        public void PushAll(MvmEngine mvm, SocketHandler socketHandler, bool clearSource, string tgtIndexName, string[] tgtRecordTemplate, List<KeyValuePair<int, int>> tgtIdxSrcIdxMap)
        {
            Dictionary<string, bool> sentObjectIds = new Dictionary<string, bool>();
            // lock the index
            lock (this.index)
            {
                foreach (var indexEntry in this.index)
                {
                    List<string[]> records = indexEntry.Value;
                    // lock the records
                    lock (records)
                    {
                        foreach (var srcRecord in records)
                        {
                            string[] outRecord = tgtRecordTemplate.Clone() as string[];
                            foreach (var kv in tgtIdxSrcIdxMap)
                            {
                                outRecord[kv.Key] = srcRecord[kv.Value];
                                this.SendObjectIfNeeded(mvm, socketHandler, srcRecord, kv.Value, sentObjectIds);
                            }
                            //Console.WriteLine("ABOUT TO SEND:" + outRecord.Join(","));
                            PushIndexMessage msg = new PushIndexMessage(tgtIndexName, outRecord);
                            socketHandler.messageOutbox.Add(msg, msg.Priority);
                        }
                    }
                }
                // if we are supposed to clear the source then do so
                if (clearSource) this.IndexClear(null);
            }
        }

        public void SendObjectIfNeeded(MvmEngine mvm, SocketHandler socketHandler, string[] srcRecord, int srcFieldIdx, Dictionary<string, bool> sentObjectIds)
        {
            // Send objects for object_id fields
            if (this.IsObjectIdFieldIndex(srcFieldIdx))
            {
                string srcOid = srcRecord[srcFieldIdx];
                if (srcOid.NotNullOrEmpty())
                {
                    using (IObjectData obj = mvm.workMgr.objectCache.CheckOut(srcOid))
                    {
                        if (!obj.IsNullObject())
                        {
                            if (!sentObjectIds.ContainsKey(obj.objectId))
                            {
                                sentObjectIds[obj.objectId] = true;
                                SendGlobalObjectMessage objMsg = new SendGlobalObjectMessage(obj);
                                socketHandler.messageOutbox.Add(objMsg);
                            }
                            else
                            {
                                //mvm.Log("skip sending dupe object_id-" + obj.objectId);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
