using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{

    public enum CursorOrder
    {
        Default,
        Forward,
        Reverse
    }

    public enum IndexRemovalOption
    {
        First,
        All,
        Last
    }

    public interface IIndex
    {
        string IndexName { get; }

        Dictionary<string, int> FieldIndexes { get; }

        /// <summary>
        /// Indicates if the index has left to right keys.
        /// </summary>
        /// <returns></returns>
        bool NestedKeys();

        /// <summary>
        /// Indicates that the one should use context defaults when inserting or selecting values.
        /// </summary>
        /// <returns></returns>
        bool UseContext();

        /// <summary>
        /// Returns true if the index is static and cannot change. This tells the caller they can assume
        /// that the format will be the same every time.
        /// </summary>
        /// <returns></returns>
        bool IsStatic();

        /// <summary>
        /// Returns the number of entries in the index for the given set of keys.
        /// </summary>
        /// <returns></returns>
        int GetCount(List<string> keys);

        /// <summary>
        /// returns the first row value for the passed key fields
        /// </summary>
        /// <param name="walkedValues"></param>
        /// <returns></returns>
        string IndexGet(ModuleContext mc, List<string> orderedKeyValues, Dictionary<string, string> values);

        bool IndexGetRow(ModuleContext mc, string[] orderedKeyValues, out string[] values);

        /// <summary>
        /// Returns the ordered list of key fields.
        /// </summary>
        /// <returns></returns>
        List<string> GetOrderedKeyFields();

        /// <summary>
        /// Returns the ordered list of value fields (includes the key fields).
        /// </summary>
        /// <returns></returns>
        List<string> GetOrderedValueFields();

        /// <summary>
        /// Expects the ordered key field values as input and returns the object id of the cursor it creates.
        /// </summary>
        /// <param name="walkedValues"></param>
        /// <returns></returns>
        ICursor IndexSelect(ModuleContext mc, ICursorSetupCommon cursorSetup, List<string> orderedKeyValues, CursorOrder cursorOrder);

        /// <summary>
        /// Returns ICursor that iterates through all the keys of the index
        /// </summary>
        /// <returns></returns>
        ICursor IndexSelectKeys(ModuleContext mc, ICursorSetupCommon cursorSetup, List<string> orderedKeyValues);

        /// <summary>
        /// Expect ordered values and inserts them into the index. 
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="orderedFieldValues"></param>
        void IndexInsert(ModuleContext mc, List<string> orderedFieldValues);

        /// <summary>
        /// inserts the values if the index does not have the keys already. returns "1" or "".
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="orderedFieldValues"></param>
        /// <returns></returns>
        string IndexInsertIfNone(ModuleContext mc, List<string> orderedFieldValues);

        /// <summary>
        /// updates all rows for the passed key fields
        /// </summary>
        /// <param name="walkedValues"></param>
        /// <returns></returns>
        string IndexUpdate(ModuleContext mc, List<string> orderedKeyValues, Dictionary<string, string> updateValues);

        /// <summary>
        /// clears all rows for the passed key fields
        /// </summary>
        /// <param name="walkedValues"></param>
        /// <returns></returns>
        string IndexRemove(ModuleContext mc, List<string> orderedKeyValues, IndexRemovalOption removalOption);

        /// <summary>
        /// clears all rows for the passed key fields
        /// </summary>
        /// <param name="walkedValues"></param>
        /// <returns></returns>
        string IndexClear(ModuleContext mc);

        /// <summary>
        /// makes sure all rows are persisted, if applicable, and closes the index
        /// </summary>
        /// <param name="walkedValues"></param>
        /// <returns></returns>
        void IndexClose(ModuleContext mc);

        /// <summary>
        /// drops the index and frees resources
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        void IndexDrop(ModuleContext mc);

        /// <summary>
        /// Push out a slice of the index
        /// </summary>
        /// <param name="orderedKeyValues"></param>
        /// <param name="socketHandler"></param>
        /// <param name="clearSource"></param>
        /// <param name="tgtName"></param>
        /// <param name="tgtRecordTemplate"></param>
        /// <param name="tgtIdxSrcIdxMap"></param>
        void PushSlice(MvmEngine mvm, string[] orderedKeyValues, SocketHandler socketHandler, bool clearSource, string tgtIndexName, string[] tgtRecordTemplate, List<KeyValuePair<int, int>> tgtIdxSrcIdxMap);

        /// <summary>
        /// push out the entire index
        /// </summary>
        /// <param name="socketHandler"></param>
        /// <param name="clearSource"></param>
        /// <param name="tgtName"></param>
        /// <param name="tgtRecordTemplate"></param>
        /// <param name="tgtIdxSrcIdxMap"></param>
        void PushAll(MvmEngine mvm, SocketHandler socketHandler, bool clearSource, string tgtIndexName, string[] tgtRecordTemplate, List<KeyValuePair<int, int>> tgtIdxSrcIdxMap);
    }
}
