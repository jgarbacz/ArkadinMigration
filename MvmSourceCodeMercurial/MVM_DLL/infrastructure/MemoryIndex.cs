using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace MVM
{
    public class MemoryIndex : IIndex
    {
        public Dictionary<string, int> FieldIndexes { get { return this.fieldIndexes; } }
        private Dictionary<string, int> fieldIndexes = new Dictionary<string, int>();
        private List<string> orderedFieldNames;
        private List<string> orderedKeyFields;
        private List<int> orderedKeyFieldsIdx = new List<int>();
        public Dictionary<StringArray, List<string[]>> index = new Dictionary<StringArray, List<string[]>>();
        CursorOrder defaultLoopOrder;
        private bool useContext;
        private bool unique;
        private int indexCount = 0;

        private List<int> objectIdFieldIdxes = new List<int>();
        private Dictionary<int, int> isObjectIdFieldIdx = new Dictionary<int, int>();
        public bool HasObjectIdFields{get{return this.objectIdFieldIdxes!=null&&this.objectIdFieldIdxes.Count >1;}}
        private bool IsObjectIdFieldIndex(int fieldIdx)
        {
            return isObjectIdFieldIdx.ContainsKey(fieldIdx);
        }

        public string IndexName { get; private set; }

        // create an empty MemoryIndex
        public MemoryIndex(string indexName, List<string> orderedFieldNames, List<string> orderedKeyFields, CursorOrder defaultCursorOrder, bool useContext, bool unique, List<int> objectIdFieldIdxes)
        {
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
                if (idx < 0) throw new Exception("Error, key fields must be value fields");
                this.orderedKeyFieldsIdx.Add(idx);
            }
            int id = 0;
            foreach (string f in this.orderedFieldNames)
            {
                this.fieldIndexes[f] = id++;
            }
            this.objectIdFieldIdxes = objectIdFieldIdxes;
            this.objectIdFieldIdxes.ForEach(i => this.isObjectIdFieldIdx[i] = i);
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
        
        public ICursor IndexSelect(ModuleContext mc,ICursorSetupCommon cursorSetup, List<string> orderedKeyValues,CursorOrder order)
        {
            CursorOrder myOrder = order;
            if (myOrder == CursorOrder.Default) myOrder = this.defaultLoopOrder;
            StringArray compoundKey = new StringArray(orderedKeyValues.ToArray());
            ICursor csr;
            List<string[]> rows = index.GetValueDefaulted(compoundKey, null);
            if (myOrder == CursorOrder.Forward) csr = new ListOfStringArrayFieldsCursor(mc, cursorSetup, this.orderedFieldNames, rows);
            else if (myOrder == CursorOrder.Reverse) csr = new ListOfStringArrayFieldsCursorReverse(mc, cursorSetup, this.orderedFieldNames, rows);
            else throw new Exception("Unexpected cursor order:" + myOrder);
            return csr;
        }

        public ICursor IndexSelectKeys(ModuleContext mc,ICursorSetupCommon cursorSetup,List<string>notused)
        {

            List<StringArray> rows = index.Keys.ToList(); //note: this is a copy to avoid concurrent modification issues
            ICursor csr = new ListOfStringArrayObjFieldsCursor(mc, cursorSetup, this.orderedKeyFields, rows);
            return csr;
        }

        public void IndexInsert(ModuleContext mc, List<string> orderedFieldValues)
        {
            List<string> orderedKeyValues = new List<string>();
            foreach (int idx in this.orderedKeyFieldsIdx)
            {
               // mc.mvm.Log("KEY FIELD=" + orderedFieldValues[idx]);
                orderedKeyValues.Add(orderedFieldValues[idx]);
            }
            StringArray compoundKey = new StringArray(orderedKeyValues.ToArray());
            if (!this.index.ContainsKey(compoundKey))
            {
                this.index[compoundKey] = new List<string[]>();
            }
            else if (this.unique)
            {
                throw new Exception("Cannot insert duplicates [" + compoundKey.Join(",") + "] into unique index: " + this.IndexName);
            }
            this.index[compoundKey].Add(orderedFieldValues.ToArray());
            indexCount++;
        }

        public string IndexInsertIfNone(ModuleContext mc, List<string> orderedFieldValues)
        {
            List<string> orderedKeyValues = new List<string>();
            foreach (int idx in this.orderedKeyFieldsIdx)
            {
                orderedKeyValues.Add(orderedFieldValues[idx]);
            }
            StringArray compoundKey = new StringArray(orderedKeyValues.ToArray());
            if (!this.index.ContainsKey(compoundKey))
            {
                this.index[compoundKey] = new List<string[]>();
                this.index[compoundKey].Add(orderedFieldValues.ToArray());
                indexCount++;
                return "1";
            }
            return "";
        }

        public List<string> GetOrderedValueFields()
        {
            return this.orderedFieldNames;
        }

        #region IIndex Members

        public string IndexUpdate(ModuleContext mc, List<string> orderedKeyValues, Dictionary<string, string> updateValues)
        {
            StringArray compoundKey = new StringArray(orderedKeyValues.ToArray());
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
            string retval = "1";
            string[] keys = values.Keys.ToArray();
            StringArray compoundKey = new StringArray(orderedKeyValues.ToArray());
            if (this.index.ContainsKey(compoundKey))
            {
                List<string[]> currvals;
                index.TryGetValue(compoundKey, out currvals);
                foreach (var kv in keys)
                {
                    values[kv] = currvals[0][this.fieldIndexes[kv]];
                }
            }
            else
            {
                foreach (var kv in keys)
                {
                    values[kv] = "";
                }
                retval = "";
            }
            return retval;
        }

        public string IndexClear(ModuleContext mc)
        {
            indexCount = 0;
            index.Clear();
            return "1";
        }

        public void IndexDrop(ModuleContext mc)
        {
            this.IndexClear(mc);
            mc.globalContext.RmNamedClassInst(this.IndexName);
        }

        public void IndexClose(ModuleContext mc)
        {
            throw new NotImplementedException();
        }

        public string IndexRemove(ModuleContext mc, List<string> orderedKeyValues, IndexRemovalOption removalOption)
        {

            StringArray compoundKey = new StringArray(orderedKeyValues.ToArray());

            if (index.ContainsKey(compoundKey))
            {
                if (removalOption != IndexRemovalOption.All)
                {
                    List<string[]> currvals;
                    index.TryGetValue(compoundKey, out currvals);
                    if (currvals.Count > 1)
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
                        return "1";
                    }
                }
                indexCount -= index[compoundKey].Count;
                index.Remove(compoundKey);
                return "1";
            }
            else
            {
                return "0";
            }
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
            if (!this.index.TryGetValue(compoundKey, out records)) return;
            if (clearSource) this.index.Remove(compoundKey);

            // Push the records
            foreach (var srcRecord in records)
            {
                string[] outRecord = tgtRecordTemplate.Clone() as string[];
                foreach (var kv in tgtIdxSrcIdxMap)
                {
                    outRecord[kv.Key] = srcRecord[kv.Value];
                    this.SendObjectIfNeeded(mvm, socketHandler, srcRecord, kv.Value, clearSource, sentObjectIds);
                }
                // send a push message.
                //Console.WriteLine("ABOUT TO SEND:" + outRecord.Join(","));
                PushIndexMessage msg = new PushIndexMessage(tgtName, outRecord);
                socketHandler.messageOutbox.Add(msg, msg.Priority);
            }
        }


        /// <summary>
        /// Pushes entire memory index.
        /// </summary>
        public void PushAll(MvmEngine mvm, SocketHandler socketHandler, bool clearSource, string tgtIndexName, string[] tgtRecordTemplate, List<KeyValuePair<int, int>> tgtIdxSrcIdxMap)
        {
            Dictionary<string, bool> sentObjectIds = new Dictionary<string, bool>();
            foreach (var indexEntry in this.index)
            {
                List<string[]> records = indexEntry.Value;
                // lock the records
                foreach (var srcRecord in records)
                {
                    string[] outRecord = tgtRecordTemplate.Clone() as string[];
                    foreach (var kv in tgtIdxSrcIdxMap)
                    {
                        outRecord[kv.Key] = srcRecord[kv.Value];
                        this.SendObjectIfNeeded(mvm, socketHandler, srcRecord, kv.Value, clearSource,sentObjectIds);
                    }
                    //Console.WriteLine("ABOUT TO SEND:" + outRecord.Join(","));
                    PushIndexMessage msg = new PushIndexMessage(tgtIndexName, outRecord);
                    socketHandler.messageOutbox.Add(msg, msg.Priority);
                }
            }
            if (clearSource) this.IndexClear(null);
        }


        public void SendObjectIfNeeded(MvmEngine mvm, SocketHandler socketHandler, string[] srcRecord, int srcFieldIdx, bool clearSource, Dictionary<string, bool> sentObjectIds)
        {
            throw new Exception("MemoryIndex no longer used??");
            //// Send objects for object_id fields
            //if (this.IsObjectIdFieldIndex(srcFieldIdx))
            //{
            //    string srcOid = srcRecord[srcFieldIdx];
            //    if (srcOid.NotNullOrEmpty())
            //    {
            //        using (IObjectData obj = mvm.workMgr.objectCache.CheckOut(srcOid))
            //        {
            //            if (!obj.IsNullObject())
            //            {
            //                if (!sentObjectIds.ContainsKey(obj.objectId))
            //                {
            //                    sentObjectIds[obj.objectId] = true;
            //                    SendGlobalObjectMessage objMsg = new SendGlobalObjectMessage(obj);
            //                    socketHandler.messageOutbox.Add(objMsg);
            //                    if (clearSource) mvm.workMgr.objectCache.RemoveObjectData(srcOid);
            //                }
            //                else
            //                {
            //                    //mvm.Log("skip sending dupe object_id-" + obj.objectId);
            //                }
            //            }
            //        }
            //    }
            //}
                            }
        #endregion
    }
}
