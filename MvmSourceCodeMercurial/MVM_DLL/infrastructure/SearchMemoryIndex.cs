using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MVM
{

    /// <summary>
    /// This structure supports N exact match keys followed by a single binary search key resulting in N rows.
    /// </summary>
    public class SearchMemoryIndex : IIndex
    {

        public static void junk()
        {
            List<string> lst=new List<string>();
            lst.Add("20090101"); // 0
            lst.Add("20110101"); // 1
            lst.Add("20110501"); // 2

            Console.WriteLine(lst.BinarySearch("20090101")); //0
            Console.WriteLine(lst.BinarySearch("20110101")); //1
            Console.WriteLine(lst.BinarySearch("20110501")); //2

            Console.WriteLine(lst.BinarySearch("20080101")); //-1
            Console.WriteLine(lst.BinarySearch("20100201")); //-2
            Console.WriteLine(lst.BinarySearch("20110212")); //-3, so 0-(-3)-1=2 so insert at 2, meaning take previous 1
            Console.WriteLine(lst.BinarySearch("20120101")); //-4
        
        }

        public Dictionary<string, int> FieldIndexes { get { return this.fieldIndexes; } }
        private Dictionary<string, int> fieldIndexes = new Dictionary<string, int>();
        public string searchKeyFieldName;
        public int searchKeyFieldIdx;

        public class SearchKeyRows:IComparable<SearchKeyRows>
        {
            public string searchKey;
            public List<string[]> rows;
            public int CompareTo(SearchKeyRows other)
            {
                return this.searchKey.CompareTo(other.searchKey);
            }
            public SearchKeyRows(string searchKey)
            {
                this.searchKey = searchKey;
            }
            public SearchKeyRows(string searchKey, List<string[]> rows)
            {
                this.searchKey = searchKey;
                this.rows = rows;
            }
        }


        private List<string> orderedFieldNames;
        private List<string> orderedKeyFields;
        // includes the search field idx
        private List<int> orderedKeyFieldsIdx = new List<int>();
        // excludes the search field idx
        private List<int> orderedHashKeyFieldsIdx = new List<int>();

        // CORE INDEX HERE
        public Dictionary<StringArray, List<SearchKeyRows>> index = new Dictionary<StringArray, List<SearchKeyRows>>();


        CursorOrder defaultLoopOrder;
        private bool useContext;


        private List<int> objectIdFieldIdxes = new List<int>();
        private Dictionary<int, int> isObjectIdFieldIdx = new Dictionary<int, int>();

        private bool IsObjectIdFieldIndex(int fieldIdx)
        {
            return isObjectIdFieldIdx.ContainsKey(fieldIdx);
        }

        public string IndexName { get; private set; }
       

        // create an empty SearchMemoryIndex
        public SearchMemoryIndex(string indexName,List<string> orderedFieldNames, List<string> orderedKeyFields,string searchKeyFieldName, CursorOrder defaultCursorOrder, bool useContext, List<int> objectIdFieldIdxes)
        {
            this.IndexName = indexName;
            this.useContext = useContext;
            this.defaultLoopOrder = defaultCursorOrder;
            if (this.defaultLoopOrder == CursorOrder.Default) this.defaultLoopOrder = CursorOrder.Forward;
            this.orderedFieldNames = orderedFieldNames;
            this.orderedKeyFields = orderedKeyFields;
            this.searchKeyFieldName = searchKeyFieldName;
            this.searchKeyFieldIdx = this.orderedFieldNames.IndexOf(this.searchKeyFieldName);
            foreach (string f in this.orderedKeyFields)
            {
                int idx = this.orderedFieldNames.IndexOf(f);
                if (idx < 0) throw new Exception("Error, key fields much be value fields");
                if(idx!=this.searchKeyFieldIdx) this.orderedHashKeyFieldsIdx.Add(idx);
                this.orderedKeyFieldsIdx.Add(idx);
            }
            int id = 0;
            foreach (string f in this.orderedFieldNames)
            {
                this.fieldIndexes[f] = id++;
            }
            this.objectIdFieldIdxes = objectIdFieldIdxes;
            this.objectIdFieldIdxes.ForEach(i => this.isObjectIdFieldIdx[i] = i);
            if (this.searchKeyFieldIdx != this.orderedKeyFields.MaxIndex())
            {
                throw new Exception("Error, if key field with type='search' is specified it must be the last key field");
            }
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

        public bool IndexGetRow(ModuleContext mc, string[] orderedKeyValues, out string[] values)
        {
            // pull out the search key field
            string searchKeyFieldValue = orderedKeyValues[this.searchKeyFieldIdx];
            string[] temp_array = new string[orderedKeyValues.Length - 1];
            Array.Copy(orderedKeyValues,0,temp_array,0,this.searchKeyFieldIdx);
            Array.Copy(orderedKeyValues, searchKeyFieldIdx + 1, temp_array, searchKeyFieldIdx, orderedKeyValues.Length - searchKeyFieldIdx - 1);
            //orderedKeyValues.RemoveAt(this.searchKeyFieldIdx);
            StringArray compoundKey = new StringArray(orderedKeyValues.ToArray());
            List<string[]> rows = null;
            if (index.ContainsKey(compoundKey))
            {
                // now search
                List<SearchKeyRows> searchlist = index[compoundKey];
                SearchKeyRows dummy = new SearchKeyRows(searchKeyFieldValue);
                int bsIdx = searchlist.BinarySearch(dummy);
                // found exact key
                if (bsIdx >= 0)
                {
                    rows = searchlist[bsIdx].rows;
                }
                else
                {
                    bsIdx = 0 - bsIdx - 2;
                    // found valid idx
                    if (bsIdx >= 0)
                    {
                        rows = searchlist[bsIdx].rows;
                    }
                }
                values = rows[0];
                return true;
            }
            values = null;
            return false;
        }

        public string IndexGet(ModuleContext mc, List<string> orderedKeyValues, Dictionary<string, string> values)
        {
            // pull out the search key field
            string searchKeyFieldValue = orderedKeyValues[this.searchKeyFieldIdx];
            orderedKeyValues.RemoveAt(this.searchKeyFieldIdx);
            StringArray compoundKey = new StringArray(orderedKeyValues.ToArray());
            List<string[]> rows = null;
            if (index.ContainsKey(compoundKey))
            {
                // now search
                List<SearchKeyRows> searchlist = index[compoundKey];
                SearchKeyRows dummy = new SearchKeyRows(searchKeyFieldValue);
                int bsIdx = searchlist.BinarySearch(dummy);
                // found exact key
                if (bsIdx >= 0)
                {
                    rows = searchlist[bsIdx].rows;
                }
                else
                {
                    bsIdx = 0 - bsIdx - 2;
                    // found valid idx
                    if (bsIdx >= 0)
                    {
                        rows = searchlist[bsIdx].rows;
                    }
                }
                string[] keys = values.Keys.ToArray();
                foreach (var kv in keys)
                {
                    values[kv] = rows[0][this.fieldIndexes[kv]];
                }
                return "1";
            }
            return "";

        }

        public int GetCount(List<string> keys)
        {
            throw new NotImplementedException();
        }

        public List<string> GetOrderedKeyFields()
        {
            return orderedKeyFields;
        }
        
        public ICursor IndexSelect(ModuleContext mc, ICursorSetupCommon cursorSetup, List<string> orderedKeyValues, CursorOrder order)
        {
            // pull out the search key field
            string searchKeyFieldValue = orderedKeyValues[this.searchKeyFieldIdx];
            orderedKeyValues.RemoveAt(this.searchKeyFieldIdx);
            CursorOrder myOrder = order;
            if (myOrder == CursorOrder.Default) myOrder = this.defaultLoopOrder;
            StringArray compoundKey = new StringArray(orderedKeyValues.ToArray());
            List<string[]> rows=null;
            ICursor cursor;
            if (index.ContainsKey(compoundKey))
            {
                // now search
                List<SearchKeyRows> searchlist = index[compoundKey];
                SearchKeyRows dummy=new SearchKeyRows(searchKeyFieldValue);
                int bsIdx = searchlist.BinarySearch(dummy);
                // found exact key
                if (bsIdx >= 0)
                {
                    rows = searchlist[bsIdx].rows;
                }
                else
                {
                    bsIdx = 0 - bsIdx - 2;
                    // found valid idx
                    if (bsIdx >= 0)
                    {
                        rows = searchlist[bsIdx].rows;
                    }
                }
            }
            if (myOrder == CursorOrder.Forward) cursor = new ListOfStringArrayFieldsCursor(mc, cursorSetup, this.orderedFieldNames, rows);
            else if (myOrder == CursorOrder.Reverse) cursor = new ListOfStringArrayFieldsCursorReverse(mc, cursorSetup, this.orderedFieldNames, rows);
            else throw new Exception("Unexpected cursor order:" + myOrder);
            return cursor;
        }

        public ICursor IndexSelectKeys(ModuleContext mc, ICursorSetupCommon cursorSetup, List<string> notused)
        {
            ICursor cursor;
            List<StringArray> rows = new List<StringArray>(); //note: this is a copy to avoid concurrent modification issues
            foreach (var entry in index)
            {
                StringArray key = entry.Key;
                foreach (SearchKeyRows sk in entry.Value)
                {
                    string searchKeyValue = sk.searchKey;
                    string[] rowArr=new string[this.orderedKeyFields.Count];
                    Array.Copy(key.array, 0, rowArr, 0, this.orderedHashKeyFieldsIdx.Count);
                    rowArr[rowArr.Length-1] = searchKeyValue;
                    rows.Add(new StringArray(rowArr));
                }
            }
            cursor = new ListOfStringArrayObjFieldsCursor(mc, cursorSetup, this.orderedKeyFields, rows);
            return cursor;
        }


         public void IndexInsert(ModuleContext mc, List<string> orderedFieldValues){
             this.IndexInsert(mc, orderedFieldValues, false);
         }

         // here we do an exact match on the hash and sort key and only insert if we do 
         // not already have an exact entry for both.
         public string IndexInsertIfNone(ModuleContext mc, List<string> orderedFieldValues)
         {
             return this.IndexInsert(mc, orderedFieldValues, true);
         }

        private string IndexInsert(ModuleContext mc, List<string> orderedFieldValues, bool insertOnlyIfNone)
        {
            string searchKeyValue = orderedFieldValues[this.searchKeyFieldIdx];
            List<string> orderedKeyValues = new List<string>();
            foreach (int idx in this.orderedHashKeyFieldsIdx)
            {
                orderedKeyValues.Add(orderedFieldValues[idx]);
            }
            StringArray compoundKey = new StringArray(orderedKeyValues.ToArray());
            if (!this.index.ContainsKey(compoundKey))
            {
                this.index[compoundKey] = new List<SearchKeyRows>();
            }
            var searchList=this.index[compoundKey];
            // add a new search key row entry if we do not have exact match
            SearchKeyRows searchKeyRows=new SearchKeyRows(searchKeyValue);
            int insertIdx = searchList.BinarySearch(searchKeyRows);

            // no exact match
            if (insertIdx < 0)
            {
                insertIdx = 0 - insertIdx-1;
                searchKeyRows.rows = new List<string[]>();
                if (insertIdx > searchList.MaxIndex())
                {
                    searchList.Add(searchKeyRows);
                }
                else
                {
                    searchList.Insert(insertIdx, searchKeyRows);
                }
                this.index[compoundKey][insertIdx].rows.Add(orderedFieldValues.ToArray());
                return "1";
            }

            // exact match
            if (!insertOnlyIfNone)
            {
                this.index[compoundKey][insertIdx].rows.Add(orderedFieldValues.ToArray());
                return "1";
            }
            return "";
        }

        public List<string> GetOrderedValueFields()
        {
            return this.orderedFieldNames;
        }

        #region IIndex Members

        public string IndexClear(ModuleContext mc)
        {
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

        public string IndexRemove(ModuleContext mc, List<string> orderedKeyValues, IndexRemovalOption removalOption)
        {

            StringArray compoundKey = new StringArray(orderedKeyValues.ToArray());
            if (index.ContainsKey(compoundKey))
            {
                index.Remove(compoundKey);
                return "1";
            }
            else
            {
                return "0";
            }
        }

        public string IndexUpdate(ModuleContext mc, List<string> orderedKeyValues, Dictionary<string, string> updateValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pushes a slice of the memory index.
        /// </summary>
        public void PushSlice(MvmEngine mvm, string[] orderedKeyValues, SocketHandler socketHandler, bool clearSource, string tgtName, string[] tgtRecordTemplate, List<KeyValuePair<int, int>> tgtIdxSrcIdxMap)
        {
            throw new Exception("push not coded for search indexes");
        }


        /// <summary>
        /// Pushes entire memory index.
        /// </summary>
        public void PushAll(MvmEngine mvm, SocketHandler socketHandler, bool clearSource, string tgtIndexName, string[] tgtRecordTemplate, List<KeyValuePair<int, int>> tgtIdxSrcIdxMap)
        {
            throw new Exception("push not coded for search indexes");
        }


        public void SendObjectIfNeeded(MvmEngine mvm, SocketHandler socketHandler, string[] srcRecord, int srcFieldIdx, bool clearSource, Dictionary<string, bool> sentObjectIds)
        {
            throw new Exception("push not coded for search indexes");
        }
        #endregion
    }
}
