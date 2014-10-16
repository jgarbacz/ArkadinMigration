using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using RaptorDB;

namespace MVM
{
    public class RaptorDBIndex : IIndex
    {
        public static string stringDelimiter = "\r";  // must be a single byte

        // CORE INDEX HERE
        public RaptorDB<string> index;

        public Dictionary<string, int> FieldIndexes { get { return this.fieldIndexes; } }
        private Dictionary<string, int> fieldIndexes = new Dictionary<string, int>();
        private List<string> orderedFieldNames;
        private List<string> orderedKeyFields;
        private List<int> orderedKeyFieldsIdx = new List<int>();
        private List<int> objectIdFieldIdxes = new List<int>();
        private Dictionary<int, int> isObjectIdFieldIdx = new Dictionary<int, int>();
        private string baseFileName;
        private CursorOrder defaultLoopOrder;
        private bool useContext;
        private bool unique;
        public bool HasObjectIdFields
        {
            get
            {
                return this.objectIdFieldIdxes != null && this.objectIdFieldIdxes.Count > 1;
            }
        }
        private bool IsObjectIdFieldIndex(int fieldIdx)
        {
            return isObjectIdFieldIdx.ContainsKey(fieldIdx);
        }

        public string IndexName { get; private set; }

        MvmEngine mvm;

        public RaptorDBIndex(MvmEngine mvm, string indexName, string fileName, List<string> orderedFieldNames, List<string> orderedKeyFields, CursorOrder defaultCursorOrder, bool useContext, bool unique, List<int> objectIdFieldIdxes)
        {
            this.mvm = mvm;
            this.baseFileName = fileName;
            this.IndexName = indexName;
            this.useContext = useContext;
            this.unique = unique;
            this.defaultLoopOrder = defaultCursorOrder;
            if (this.defaultLoopOrder == CursorOrder.Default)
            {
                this.defaultLoopOrder = CursorOrder.Forward;
            }
            this.orderedFieldNames = orderedFieldNames;
            this.orderedKeyFields = orderedKeyFields;
            foreach (string f in this.orderedKeyFields)
            {
                int idx = this.orderedFieldNames.IndexOf(f);
                if (idx < 0)
                {
                    throw new Exception("Error, key fields must be value fields");
                }
                this.orderedKeyFieldsIdx.Add(idx);
            }
            int id = 0;
            foreach (string f in this.orderedFieldNames)
            {
                this.fieldIndexes[f] = id++;
            }
            this.objectIdFieldIdxes = objectIdFieldIdxes;
            this.objectIdFieldIdxes.ForEach(i => this.isObjectIdFieldIdx[i] = i);
            this.OpenIndex();
        }

        public void OpenIndex()
        {
            //this.index = new RaptorDBString(this.baseFileName, true);
            this.index = RaptorDB<string>.Open(this.baseFileName, 255, true);
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
                return (int)this.index.Count();
            }
            throw new Exception("GetCount not supported on keys of a RaptorDB index");
        }

        public List<string> GetOrderedKeyFields()
        {
            return orderedKeyFields;
        }

        public ICursor IndexSelect(ModuleContext mc, ICursorSetupCommon cursorSetup, List<string> orderedKeyValues, CursorOrder order)
        {
            CursorOrder myOrder = order;
            if (myOrder == CursorOrder.Default)
            {
                myOrder = this.defaultLoopOrder;
            }
            string compoundKey = orderedKeyValues.Join(RaptorDBIndex.stringDelimiter);
            ICursor csr = new RaptorDBCursor(mc, cursorSetup, this, this.orderedFieldNames, myOrder, compoundKey);
            return csr;
        }

        public ICursor IndexSelectKeys(ModuleContext mc, ICursorSetupCommon cursorSetup, List<string> notused)
        {
            // Useless for RaptorDB, since we can't enumerate just the keys without getting all the values as well
            throw new Exception("IndexSelectKeys not supported on keys of a RaptorDB index; use index_select_all instead");
        }

        public byte[] GetSerializedFieldValues(List<string> orderedFieldValues)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bwriter = new BinaryWriter(ms);
            for (int i = 0; i < orderedFieldValues.Count; i++)
            {
                if (this.IsObjectIdFieldIndex(i))
                {
                    string oid = orderedFieldValues[i];
                    if (oid.NotNullOrEmpty())
                    {
                        IObjectData obj = this.mvm.objectCache.ObjectGet(oid);
                        obj.Serialize(bwriter);
                    }
                }
                else
                {
                    bwriter.Write(orderedFieldValues[i]);
                }
            }
            byte[] buffer = ms.GetBuffer();
            return buffer.Take((int)ms.Position).ToArray();
        }

        public string[] GetDeserializedFieldValues(byte[] data)
        {
            string[] fieldValues = new string[this.orderedFieldNames.Count];
            MemoryStream ms = new MemoryStream(data);
            BinaryReader breader = new BinaryReader(ms);
            for (int i = 0; i < orderedFieldNames.Count; i++)
            {
                if (this.IsObjectIdFieldIndex(i))
                {
                    IObjectData obj;
                    this.mvm.objectCache.DeserializeObject(breader, out obj);
                    fieldValues[i] = obj.objectId;
                }
                else
                {
                    fieldValues[i] = breader.ReadString();
                }
            }
            return fieldValues;
        }

        public void IndexInsert(ModuleContext mc, List<string> orderedFieldValues)
        {
            List<string> orderedKeyValues = new List<string>();
            foreach (int idx in this.orderedKeyFieldsIdx)
            {
                orderedKeyValues.Add(orderedFieldValues[idx]);
            }
            string compoundKey = orderedKeyValues.Join(RaptorDBIndex.stringDelimiter);
            byte[] compountValue = GetSerializedFieldValues(orderedFieldValues);
            if (this.unique)
            {
                string outval;
                if (this.index.Get(compoundKey, out outval))
                {
                    throw new Exception("Cannot insert duplicates [" + compoundKey + "] into unique index: " + this.IndexName);
                }
            }
            this.index.Set(compoundKey, compountValue);
        }

        public string IndexInsertIfNone(ModuleContext mc, List<string> orderedFieldValues)
        {
            List<string> orderedKeyValues = new List<string>();
            foreach (int idx in this.orderedKeyFieldsIdx)
            {
                orderedKeyValues.Add(orderedFieldValues[idx]);
            }
            string compoundKey = orderedKeyValues.Join(RaptorDBIndex.stringDelimiter);
            byte[] outval;
            if (!this.index.Get(compoundKey, out outval))
            {
                byte[] compountValue = GetSerializedFieldValues(orderedFieldValues);
                this.index.Set(compoundKey, compountValue);
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
            // Raptor doesn't currently support updates, so save all current rows, delete and then reinsert them with updated values
            string retval = "0";
            string compoundKey = orderedKeyValues.Join(RaptorDBIndex.stringDelimiter);

            List<string> rows = new List<string>();
            foreach (var row in this.Enumerate(compoundKey, CursorOrder.Forward))
            {
                foreach (var upd in updateValues)
                {
                    row[this.fieldIndexes[upd.Key]] = upd.Value;
                }
                rows.Add(row.Join(RaptorDBIndex.stringDelimiter));
                retval = "1";
            }

            this.IndexRemove(mc, orderedKeyValues, IndexRemovalOption.All);
            foreach (var row in rows)
            {
                this.index.Set(compoundKey, row);
            }

            return retval;
        }

        public bool IndexGetRow(ModuleContext mc, string[] orderedKeyValues, out string[] values)
        {
            string compoundKey = orderedKeyValues.Join(RaptorDBIndex.stringDelimiter);
            foreach (var row in this.Enumerate(compoundKey, CursorOrder.Forward))
            {
                values = row;
                return true;
            }
            values = null;
            return false;
        }

        public string IndexGet(ModuleContext mc, List<string> orderedKeyValues, Dictionary<string, string> values)
        {
            string[] keys = values.Keys.ToArray();
            string compoundKey = orderedKeyValues.Join(RaptorDBIndex.stringDelimiter);
            foreach (var row in this.Enumerate(compoundKey, CursorOrder.Forward))
            {
                foreach (var kv in keys)
                {
                    values[kv] = row[this.fieldIndexes[kv]];
                }
                return "1";
            }
            foreach (var kv in keys)
            {
                values[kv] = "";
            }
            return "";
        }

        public IEnumerable<string[]> Enumerate(string compoundKey, CursorOrder order)
        {
            if (compoundKey == null)
            {
                // Enumerate the entire index
                foreach (var e in this.index.EnumerateStorageFile())
                {
                    yield return GetDeserializedFieldValues(e.Value);
                }
            }
            else
            {
                IEnumerable<int> duplicates;
                if (order == CursorOrder.Forward)
                {
                    duplicates = index.GetDuplicates(compoundKey);
                }
                else if (order == CursorOrder.Reverse)
                {
                    duplicates = index.GetDuplicates(compoundKey).Reverse();
                }
                else
                {
                    throw new Exception("Unexpected cursor order: " + order);
                }

                // GetDuplicates() returns nothing if there's just one row for that key, so need to also call Get()
                bool haveDuplicates = false;
                foreach (var i in duplicates)
                {
                    haveDuplicates = true;
                    yield return GetDeserializedFieldValues(this.index.FetchRecordBytes(i));
                }

                if (!haveDuplicates)
                {
                    byte[] outval;
                    if (this.index.Get(compoundKey, out outval))
                    {
                        yield return GetDeserializedFieldValues(outval);
                    }
                }
            }
        }

        public void IndexDelete()
        {
            this.index.Shutdown();
            foreach (var file in FileUtils2.GlobToList(this.baseFileName + "*"))
            {
                File.Delete(file);
            }
        }

        public string IndexClear(ModuleContext mc)
        {
            this.IndexDelete();
            this.OpenIndex();
            return "1";
        }

        public void IndexClose(ModuleContext mc)
        {
            this.index.Shutdown();
        }

        public void IndexDrop(ModuleContext mc)
        {
            this.IndexDelete();
            mc.globalContext.RmNamedClassInst(this.IndexName);
        }

        public string IndexRemove(ModuleContext mc, List<string> orderedKeyValues, IndexRemovalOption removalOption)
        {
            if (removalOption != IndexRemovalOption.All)
            {
                throw new Exception("RaptorDB index only supports removing all rows");
            }
            string compoundKey = orderedKeyValues.Join(RaptorDBIndex.stringDelimiter);
            string outval;
            if (this.index.Get(compoundKey, out outval))
            {
                this.index.RemoveKey(compoundKey);
                return "1";
            }
            return "0";
        }

        /// <summary>
        /// Pushes a slice of the memory index.
        /// </summary>
        public void PushSlice(MvmEngine mvm, string[] orderedKeyValues, SocketHandler socketHandler, bool clearSource, string tgtName, string[] tgtRecordTemplate, List<KeyValuePair<int, int>> tgtIdxSrcIdxMap)
        {
            throw new Exception("Not supported yet for RaptorDB index");
        }

        /// <summary>
        /// Pushes entire memory index.
        /// </summary>
        public void PushAll(MvmEngine mvm, SocketHandler socketHandler, bool clearSource, string tgtIndexName, string[] tgtRecordTemplate, List<KeyValuePair<int, int>> tgtIdxSrcIdxMap)
        {
            throw new Exception("Not supported yet for RaptorDB index");
        }

        public void SendObjectIfNeeded(MvmEngine mvm, SocketHandler socketHandler, string[] srcRecord, int srcFieldIdx, bool clearSource, Dictionary<string, bool> sentObjectIds)
        {
            throw new Exception("Not supported yet for RaptorDB index");
        }
        #endregion
    }

    class RaptorDBCursor : CursorCommonLinqEnabled, ICursor
    {
        private RaptorDB<string> index;
        private IEnumerator<string[]> enumerator;
        public RaptorDBCursor(ModuleContext mc, ICursorSetupCommon cursorSetup, RaptorDBIndex dbidx, List<string> orderedFieldNames, CursorOrder order, string key)
            : base(mc, cursorSetup)
        {
            this.index = dbidx.index;
            this.orderedFieldNames = orderedFieldNames;

            IEnumerable<int> duplicates;
            if (order == CursorOrder.Forward)
            {
                duplicates = index.GetDuplicates(key);
            }
            else if (order == CursorOrder.Reverse)
            {
                duplicates = index.GetDuplicates(key).Reverse();
            }
            else
            {
                throw new Exception("Unexpected cursor order: " + order);
            }
            this.enumerator = dbidx.Enumerate(key, CursorOrder.Forward).GetEnumerator();
        }

        public RaptorDBCursor(ModuleContext mc, ICursorSetupCommon cursorSetup, RaptorDBIndex dbidx, List<string> orderedFieldNames)
            : base(mc, cursorSetup)
        {
            // In this case, we're enumerating all the keys/values in the index
            this.index = dbidx.index;
            this.orderedFieldNames = orderedFieldNames;
            this.enumerator = dbidx.Enumerate(null, CursorOrder.Forward).GetEnumerator();
        }

        public override IObjectData CursorNext()
        {
            if (this.enumerator.MoveNext())
            {
                string[] values = this.enumerator.Current;
                using (var csrObj = this.CreateNewObject())
                {
                    for (int i = 0; i < this.orderedFieldNames.Count; i++)
                    {
                        csrObj[this.orderedFieldNames[i]] = values[i];
                    }
                    return csrObj;
                }
            }
            else
            {
                return null;
            }
        }

        public override void CursorClear()
        {
            // no resources to free
        }
    }
}
