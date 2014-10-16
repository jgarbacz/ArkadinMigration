using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using NLog;
using System.Diagnostics;

namespace MVM
{
    public class ObjectDataFormattedDelta : ObjectDataFormattedBase, IObjectData
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static int classId = 4;
        public override int ClassId { get { return classId; } }

        public ObjectDataFormattedDelta(ObjectCache objectCache, string objectId, string objectType, string feedbackName)
            : base(objectCache, objectId, objectType, feedbackName)
        {
        }

        public ObjectDataFormattedDelta(ObjectCache objectCache)
            : base(objectCache)
        {
        }

        /// <summary>
        /// Clones the object and return IObjectData
        /// </summary>
        /// <returns></returns>
        public override IObjectData CloneObjectData()
        {
            return this.CloneMe() as IObjectData;
        }

        /// <summary>
        /// Clones this object and return is as this type
        /// </summary>
        /// <returns></returns>
        public ObjectDataFormattedDelta CloneMe()
        {
            ObjectDataFormattedDelta clone = this.objectCache.CreateAndGetObjectDataFormattedDelta(this.objectType, this.feedbackName);
            this.CloneMeOnto(clone);

            // the key is a string, the value is a list which we need to shallow copy
            foreach (var entry in this.fieldDeltas)
            {
                List<Delta> clonedList = new List<Delta>(entry.Value);
                clone.fieldDeltas.Add(entry.Key, clonedList);
            }

            // strings and bools are immutable so this is safe
            clone.persistTables.AddAll(this.persistTables);

            // need to copy the string[]
            foreach (var entry in this.configuredUpdateTriggers)
            {
                string[] clonedArr = (string[])entry.Clone();
                clone.configuredUpdateTriggers.Add(clonedArr);
            }
            // simply copy the enum
            clone.deltaState = this.deltaState;

            return clone;
        }

        /// <summary>
        /// Clears all the field except the object_id, and object_type
        /// </summary>
        public override void Clear()
        {
            this.ClearFormattedFields();
            this.fieldDeltas.Clear();
            this.configuredUpdateTriggers.Clear();
            this.persistTables.Clear();
            if (this.insertedOrUpdatedTables != null) this.insertedOrUpdatedTables.Clear();
            if (this.insertedOrUpdatedDbFields != null) this.insertedOrUpdatedDbFields.Clear();
        }

        /// <summary>
        ///  get or set an object field by ufn
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public override string this[int ufn]
        {
            get
            {
                return this.GetFormattedField(ufn);
            }
            set
            {
                this.SetField(ufn, value);
            }
        }

        /// <summary>
        /// Writes out format id followed by field values.
        /// </summary>
        /// <param name="bwriter"></param>
        protected override void SerializeSpecific(BinaryWriter bwriter)
        {
            this.SerializeFormattedFields(bwriter);
            // write out the persistedDeltas
            this.SerializeFieldDeltas(bwriter);
            bwriter.Write(this.persistTables);
            bwriter.Write(this.persistedObjectRefFields);
            bwriter.Write(this.configuredUpdateTriggers);
            bwriter.Write((int)this.deltaState);
        }

        /// <summary>
        /// Deserializes an object returning true if it got one else false if EOF.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="targetObj"></param>
        public override bool DeserializeSpecific(BinaryReader BinaryReader, MvmClusterBase mvmCluster, out IObjectData obj)
        {
            try
            {
                ObjectDataFormattedDelta objData = new ObjectDataFormattedDelta(mvmCluster.mvm.objectCache);
                obj = objData;
                objData.DeserializeSpecific(BinaryReader, mvmCluster, objData);
                objData.DeserializeFieldDeltas(BinaryReader);
                objData.persistTables = BinaryReader.ReadDictionaryOfStringInt32();
                objData.configuredUpdateTriggers = BinaryReader.ReadListOfStringArray();
                objData.deltaState = (DeltaState)Enum.ToObject(typeof(DeltaState), BinaryReader.ReadInt32());
                return true;
            }
            catch (EndOfStreamException e)
            {
                string msg = "Error partial object read on deserialize for type " + this.GetType().FullName;
                logger.Fatal(msg);
                throw new Exception(msg, e);
            }
        }


        /// <summary>
        /// Returns all the version of the fields.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, string>> GetFieldVersions(string fieldName)
        {
            int ufn = this.mvmCluster.GetUfn(fieldName);
            string currentValue=this[ufn];
            List<Delta>deltaList;
            if (this.fieldDeltas.TryGetValue(ufn, out deltaList))
            {
                int ssValueDeltaListIdx = 0;
                // special case the persisted and original
                {
                    Delta delta0 = deltaList[0];

                    // first delta is always persisted value
                    yield return new KeyValuePair<string, string>("persisted", delta0.value);

                    // if the first delta has rcn!=-1 it is ALSO the original value
                    if (delta0.rcn != RCN_PERSIST_NE_ORIG)
                    {
                        //yield return new KeyValuePair<string, string>("original", delta0.value);
                        ssValueDeltaListIdx = 1;
                    }
                    // otherwise, the original is the the next delta
                    else
                    {
                        // if only one delta current is the original
                        if (deltaList.Count == 1)
                        {
                            yield return new KeyValuePair<string, string>("original", currentValue);
                            ssValueDeltaListIdx = int.MaxValue;
                        }
                        // otherwise the second delta is the original
                        else
                        {
                            Delta delta1 = deltaList[1];
                            yield return new KeyValuePair<string, string>("original", delta1.value);
                            ssValueDeltaListIdx=2;
                        }
                    }
                }
                // print the rcn stamped current values
                for (; ssValueDeltaListIdx <= deltaList.Count; ssValueDeltaListIdx++)
                {
                    // otherwise, we already took care of original and persisted so do snapshot values
                    // i points to the value, the rcn for i is at i-1
                    string snapValue = ssValueDeltaListIdx == deltaList.Count ? currentValue : deltaList[ssValueDeltaListIdx].value;
                    long snapRcn = deltaList[ssValueDeltaListIdx - 1].rcn;
                    yield return new KeyValuePair<string, string>("RCN" + snapRcn, snapValue);
                }
            }
            else
            {
                yield return new KeyValuePair<string, string>("persisted", currentValue);
            }
        }
       
        /// <summary>
        /// Dumps the passed object to a string
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="object_id"></param>
        /// <returns></returns>
        /// <category>Delta Tracking</category>
        [MvmExport(@"dump_object_delta")]
        public static string dump_object_delta(ModuleContext mc, string object_id)
        {
            StringBuilder sb = new StringBuilder();
            using (IObjectData obj = mc.objectCache.CheckOut(object_id))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj == null)
                {
                    return ObjectDataBase.dump_object(mc, object_id);
                }
                sb.AppendLine("OBJECT: " + obj.objectId + "/" + obj.objectType + " (");
                sb.AppendLine("  type=[" + obj.GetType().Name + "]");
                sb.AppendLine("  ref_count=[" + obj.RefCount + "]");
                sb.AppendLine("  deltastate=[" + deltaObj.deltaState + "]");
                sb.AppendLine("  tables=[" + deltaObj.persistTables.Select(kv => kv.Key + "=" + StatusNames[kv.Value]).JoinStrings(",") + "]");
                sb.AppendLine("  tableObjRefFields=[" + deltaObj.persistedObjectRefFields.SelectMany(x=>x.Value.Select(y=>x.Key+"."+mc.mvmCluster.GetFieldName(y.Key))).JoinStrings(",") + "]");
                foreach (var f in obj.FieldKeyValuesPairs.OrderBy(x=>x.Key))
                {
                    string fieldName = f.Key;
                    string currentValue = f.Value;
                    sb.Append("  " + fieldName + ":");
                    foreach (var entry in deltaObj.GetFieldVersions(fieldName))
                    {
                        sb.Append(" "+entry.Key + "=[" + entry.Value + "]");
                    }
                    sb.AppendLine();
                }
                sb.Append(")");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Dumps the delta object in raw form for .net code level debugging.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="object_id"></param>
        /// <returns></returns>
        /// <category>Delta Tracking</category>
        [MvmExport(@"dump_object_delta_raw")]
        public static string dump_object_delta_raw(ModuleContext mc, string object_id)
        {
            StringBuilder sb = new StringBuilder();
            using (IObjectData obj = mc.objectCache.CheckOut(object_id))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj == null)
                {
                    return ObjectDataBase.dump_object(mc, object_id);
                }
                sb.AppendLine("OBJECT: " + obj.objectId + "/" + obj.objectType + "[raw] (");
                sb.AppendLine("  type=[" + obj.GetType().Name + "]");
                sb.AppendLine("  ref_count=[" + obj.RefCount + "]");
                sb.AppendLine("  deltastate=[" + deltaObj.deltaState + "]");
                sb.AppendLine("  tables=[" + deltaObj.persistTables.Select(kv=>kv.Key+"="+StatusNames[kv.Value]).JoinStrings(",") + "]");
                foreach (var f in obj.FieldKeyValuesPairs)
                {
                    sb.Append("  " + f.Key + "=[" + f.Value + "]");
                    List<Delta> deltaList;
                    int ufn = deltaObj.mvmCluster.GetUfn(f.Key);
                    if (deltaObj.fieldDeltas.TryGetValue(ufn, out deltaList))
                    {
                        sb.Append(deltaList.Select(d => "(" + d.rcn.ToString() + "," + d.value + ")").JoinStrings());
                    }
                    sb.AppendLine();
                }
                sb.Append(")");
            }
            return sb.ToString();
        }


        /// <summary>
        /// Dumps the passed object to a string,showing original values only.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="object_id"></param>
        /// <returns></returns>
        /// <category>Delta Tracking</category>
        [MvmExport(@"dump_object_delta_original")]
        public static string dump_object_delta_original(ModuleContext mc, string object_id)
        {
            StringBuilder sb = new StringBuilder();
            using (IObjectData obj = mc.objectCache.CheckOut(object_id))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj == null)
                {
                    return ObjectDataBase.dump_object(mc, object_id);
                }
                sb.AppendLine("OBJECT: " + obj.objectId + "/" + obj.objectType + " [original values](");
                sb.AppendLine("  type=[" + obj.GetType().Name + "]");
                sb.AppendLine("  ref_count=[" + obj.RefCount + "]");
                sb.AppendLine("  deltastate=[" + deltaObj.deltaState + "]");
                foreach (var f in obj.FieldKeyValuesPairs)
                {
                    string fieldName = f.Key;
                    string fieldValue = deltaObj.GetOriginal(fieldName);
                    sb.Append("  " + fieldName + "=[" + fieldValue + "]");
                    sb.AppendLine();
                }
                sb.Append(")");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Dumps the passed object to a string,showing persisted values only.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="object_id"></param>
        /// <returns></returns>
        /// <category>Delta Tracking</category>
        [MvmExport(@"dump_object_delta_persisted")]
        public static string dump_object_delta_persisted(ModuleContext mc, string object_id)
        {
            StringBuilder sb = new StringBuilder();
            using (IObjectData obj = mc.objectCache.CheckOut(object_id))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj == null)
                {
                    return ObjectDataBase.dump_object(mc, object_id);
                }
                sb.AppendLine("OBJECT: " + obj.objectId + "/" + obj.objectType + " [persisted values](");
                sb.AppendLine("  type=[" + obj.GetType().Name + "]");
                sb.AppendLine("  ref_count=[" + obj.RefCount + "]");
                sb.AppendLine("  deltastate=[" + deltaObj.deltaState + "]");
                foreach (var f in obj.FieldKeyValuesPairs)
                {
                    string fieldName = f.Key;
                    string fieldValue = deltaObj.GetPersisted(fieldName);
                    sb.Append("  " + fieldName + "=[" + fieldValue + "]");
                    sb.AppendLine();
                }
                sb.Append(")");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns the state of the passed object
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="object_id"></param>
        /// <returns></returns>
        /// <category>Delta Tracking</category>
        [MvmExport(@"object_delta_get_state")]
        public static string object_delta_get_state(ModuleContext mc, string object_id)
        {
            using (IObjectData obj = mc.objectCache.CheckOut(object_id))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj != null)
                {
                    return deltaObj.deltaState.ToString();
                }
                
            }
            return "NOT_DELTA_OBJ";
        }

        /// <summary>
        /// Set the state on the passed object
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="object_id"></param>
        /// <param name="deltaState"></param>
        /// <returns></returns>
        /// <category>Delta Tracking</category>
        [MvmExport(@"object_delta_set_state")]
        public static void object_delta_set_state(ModuleContext mc, string object_id,string deltaState)
        {
            using (IObjectData obj = mc.objectCache.CheckOut(object_id))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj != null)
                {
                    DeltaState deltaStateEnum = (DeltaState)Enum.Parse(typeof(DeltaState), deltaState, true);
                    deltaObj.deltaState = deltaStateEnum;
                }
                else
                {
                    throw new Exception("Error, object_id=[" + object_id + "] is not a delta object");
                }
            }
        }

        /// <summary>
        /// Returns the original value for a field
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="object_id"></param>
        /// <param name="field_name"></param>
        /// <returns></returns>
        /// <category>Delta Tracking</category>
        [MvmExport(@"object_delta_get_original")]
        public static string object_delta_get_original(ModuleContext mc, string object_id, string field_name)
        {
            using (IObjectData obj = mc.objectCache.CheckOut(object_id))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj != null)
                {
                    return deltaObj.GetOriginalValue(field_name);
                }
                else
                {
                    throw new Exception("Error, object_id=[" + object_id + "] is not a delta object");
                }
            }
        }

        /// <summary>
        /// Sets the original value for a delta object
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="object_id"></param>
        /// <param name="field_name"></param>
        /// <param name="field_value"></param>
        /// <returns></returns>
        /// <category>Delta Tracking</category>
        [MvmExport(@"object_delta_set_original")]
        public static void object_delta_get_original(ModuleContext mc, string object_id, string field_name,string field_value)
        {
            using (IObjectData obj = mc.objectCache.CheckOut(object_id))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj != null)
                {
                    deltaObj.SetOriginal(field_name,field_value);
                }
                else
                {
                    throw new Exception("Error, object_id=[" + object_id + "] is not a delta object");
                }
            }
        }

        /// <summary>
        /// Returns the persisted value for a field
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="object_id"></param>
        /// <param name="field_name"></param>
        /// <returns></returns>
        /// <category>Delta Tracking</category>
        [MvmExport(@"object_delta_get_persisted")]
        public static string object_delta_get_persisted(ModuleContext mc, string object_id, string field_name)
        {
            using (IObjectData obj = mc.objectCache.CheckOut(object_id))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj != null)
                {
                    return deltaObj.GetPersistedValue(field_name);
                }
                else
                {
                    throw new Exception("Error, object_id=[" + object_id + "] is not a delta object");
                }
            }
        }

        /// <summary>
        /// Returns a snapshot of the passed object.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="object_id"></param>
        /// <param name="field_name"></param>
        /// <param name="ssn"></param>
        /// <returns></returns>
        /// <category>Delta Tracking</category>
        [MvmExport(@"object_delta_get_snapshot")]
        public static string object_delta_get_persisted(ModuleContext mc, string object_id, string field_name, string ssn)
        {
            long ssnLong = long.Parse(ssn);
            using (IObjectData obj = mc.objectCache.CheckOut(object_id))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj != null)
                {
                    return deltaObj.GetSnapshotValue(field_name,ssnLong);
                }
                else
                {
                    throw new Exception("Error, object_id=[" + object_id + "] is not a delta object");
                }
            }
        }

        /// <summary>
        /// Rolls back the passed object to a prior snapshot
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="object_id"></param>
        /// <param name="ssn"></param>
        /// <returns></returns>
        /// <category>Delta Tracking</category>
        [MvmExport(@"object_delta_rollback")]
        public static void object_delta_rollback(ModuleContext mc, string object_id,string ssn)
        {
            long ssnLong;
            using (IObjectData obj = mc.objectCache.CheckOut(object_id))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj != null)
                {
                    if (ssn.Equals("original"))
                    {
                        deltaObj.RollbackToOriginalState();
                    }
                    else if (ssn.Equals("persisted"))
                    {
                        deltaObj.RollbackToPersistedState();
                    }
                    else if (long.TryParse(ssn, out ssnLong))
                    {
                        deltaObj.RollbackToSnapshot(ssnLong);
                    }
                    else
                    {
                        throw new Exception("Error in object_delta_rollback passed ssn=["+ssn+"] is not a number or 'original' or 'persisted'");
                    }
                }
                else
                {
                    throw new Exception("Error, object_id=[" + object_id + "] is not a delta object");
                }
            }
        }

        /// <summary>
        /// Forgets all snapshots greater than or equal to the passed ssn. This does not rollback the 
        /// current view, it throws away delta information to save space.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="object_id"></param>
        /// <param name="ssn"></param>
        /// <returns></returns>
        /// <category>Delta Tracking</category>
        [MvmExport(@"object_delta_forget_snapshots")]
        public static void object_delta_forget_snapshots(ModuleContext mc, string object_id, string ssn)
        {
            long ssnLong;
            using (IObjectData obj = mc.objectCache.CheckOut(object_id))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj != null)
                {
                    if (long.TryParse(ssn, out ssnLong))
                    {
                        deltaObj.ForgetSnapshots(ssnLong);
                    }
                    else
                    {
                        throw new Exception("Error in object_delta_forget_snapshots passed ssn=[" + ssn + "] is not a number ");
                    }
                }
                else
                {
                    throw new Exception("Error, object_id=[" + object_id + "] is not a delta object");
                }
            }
        }


        public enum DeltaState { NoTracking, SettingPersisted, SettingOriginals, SettingNew }
        public DeltaState deltaState = DeltaState.NoTracking;
               
        // fieldDeltas{ufn}[i]=([ssn,value])
        // this stores the 'value' of 'field' BEFORE snapshot 'ssn'
        private Dictionary<int, List<Delta>> fieldDeltas = new Dictionary<int, List<Delta>>();


        /// <summary>
        /// serializes the object in a way that persists between runs.
        /// give you original value, and persisted value, which is the now current value.
        /// can use format id.
        /// objectId,objectType,format_id|persistedVal1|origVal1...
        /// </summary>
        public string SerializeForDb()
        {
            StringBuilder sb = new StringBuilder();
            List<string> sortedFieldNames = this.FieldNames.Where(f => f.NotIn("object_id", "object_type")).OrderBy(f => f).ToList();
            List<string> sortedTableNames = this.persistTables.Keys.OrderBy(f => f).ToList();
            // table1,field1,table2,field2...
            List<string> objectRefFields=this.persistedObjectRefFields
                .OrderBy(x => x.Key)
                .SelectMany(
                    y => y.Value.SelectMany(
                        z => new List<string> { y.Key, this.mvmCluster.GetFieldName(z.Key) }
                    )).ToList();
            int objectRefFieldsFormatId = this.mvmCluster.GetFormatId(objectRefFields);
            int tablesFormatId = this.mvm.mvmCluster.GetFormatId(sortedTableNames);
            int formatId = this.mvm.mvmCluster.GetFormatId(sortedFieldNames);
            sb.Append(this.objectId);
            sb.Append("|");
            sb.Append(this.objectType);
            sb.Append("|");
            sb.Append(tablesFormatId);
            sb.Append("|");
            sb.Append(objectRefFieldsFormatId);
            sb.Append("|");
            sb.Append(formatId);
            foreach (var fieldName in sortedFieldNames)
            {
                sb.Append("|");
                sb.Append(this[fieldName]); // this current val is now the persisted for next time
            }
            foreach (var fieldName in sortedFieldNames)
            {
                sb.Append("|");
                sb.Append(GetOriginal(fieldName)); // the original never changes
            }
            return sb.ToString();
        }

        /// <summary>
        /// Deserializes the text into a delta object and returns the oid.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <category>Delta Tracking</category>
        [MvmExport("deserialize_object_id_field")]
        public static string DeserializeFromDb(ModuleContext mc,string text)
        {
            if (text.IsNullOrEmpty()) return "";
            string[] parts=text.Split('|');
            int partIdx = 0;
            
            string objectId = parts[partIdx++];
            string objectType = parts[partIdx++];
            string tablesFormatIdStr = parts[partIdx++];
            string objRefFieldsFormatIdStr = parts[partIdx++];

            int tablesFormatId = tablesFormatIdStr.ToInt();
            string[] tables = mc.mvmCluster.GetFormatFields(tablesFormatId);
            int objectRefFieldsFormatId = objRefFieldsFormatIdStr.ToInt();
            string[] objectRefFields = mc.mvmCluster.GetFormatFields(objectRefFieldsFormatId);
            string formatIdStr = parts[partIdx++];
            int formatId=formatIdStr.ToInt();
            string[] formatFields = mc.mvmCluster.GetFormatFields(formatId);
            ObjectDataFormattedDelta obj = new ObjectDataFormattedDelta(mc.objectCache, objectId, objectType, "");
            // add the tables
            foreach (var tableName in tables)
            {
                obj.AddPersistTable(tableName,STATUS_HAS_ROW);
            }
            // add objectref fields
            for (int i = 0; i < objectRefFields.Length;i+=2)
            {
                string tableName = objectRefFields[i];
                string fieldName = objectRefFields[i+1];
                obj.AddPersistObjectRefField(tableName, fieldName);
            }
            // add the persisted values
            obj.deltaState = DeltaState.SettingPersisted;
            foreach (var fieldName in formatFields)
            {
                string fieldVal=parts[partIdx++];
                obj[fieldName] = fieldVal;
            }
            // add the original values
            obj.deltaState = DeltaState.SettingOriginals;
            foreach (var fieldName in formatFields)
            {
                string fieldVal = parts[partIdx++];
                obj[fieldName] = fieldVal;
            }
            obj.deltaState = DeltaState.SettingNew;
            mc.objectCache.AddOrMergeObject(obj);
            return obj.objectId;
        }

        private void SerializeFieldDeltas(BinaryWriter bwriter)
        {
            bwriter.Write7BitEncodedInt(fieldDeltas.Count);
            foreach (var entry in this.fieldDeltas)
            {
                int ufn = entry.Key;
                List<Delta> deltaList = entry.Value;
                bwriter.Write(ufn);
                bwriter.Write7BitEncodedInt(deltaList.Count);
                foreach(var delta in deltaList){
                    bwriter.Write(delta.rcn);
                    bwriter.Write(delta.value);
                }
            }
        }
        private void DeserializeFieldDeltas(BinaryReader breader)
        {
            int fieldDeltasCount = breader.Read7BitEncodedInt();
            for (int i = 0; i < fieldDeltasCount; i++)
            {
                int ufn = breader.ReadInt32();
                int deltaListCount = breader.Read7BitEncodedInt();
                List<Delta> deltaList = new List<Delta>(deltaListCount);
                for (int j = 0; j < deltaListCount; j++)
                {
                    long rcn=breader.ReadInt64();
                    string value= breader.ReadString();
                    Delta delta = new Delta(rcn,value);
                    deltaList.Add(delta);
                }
                this.fieldDeltas[ufn] = deltaList;
            }
        }

      
        private long CurrentRcn
        {
            get
            {
                return this.mvm.mvmCluster.CurrentRcn;
            }
        }


        public string GetSnapshotValue(string fieldName, long rcn)
        {
            int ufn = this.mvmCluster.GetUfn(fieldName);
            return this.GetSnapshotValue(ufn, rcn);
        }

        public string GetSnapshotValue(int ufn,long rcn)
        {
            // if no deltas, snapshot value is current value
            List<Delta> deltaList;
            if (!this.fieldDeltas.TryGetValue(ufn, out deltaList))
            {
                return this[ufn];
            }
            if (deltaList.Count == 0)
            {
                this.fieldDeltas.Remove(ufn);
                return this[ufn];
            }
            // otherwise the earliest delta after the passed ssn
            string snapshotValue=this[ufn];
            int idx = deltaList.Count;
            while ((idx - 1) >= 0 && rcn < deltaList[idx - 1].rcn)
            {
                idx--;
                snapshotValue=deltaList[idx].value;
            }
            return snapshotValue;
        }


        public string GetOriginal(string fieldName)
        {
            int ufn = this.mvmCluster.GetUfn(fieldName);
            return this.GetOriginal(ufn);
        }

        public string GetOriginal(int ufn)
        {
           
            // if no deltas the current is the original
            List<Delta> deltaList;
            if (!this.fieldDeltas.TryGetValue(ufn, out deltaList))
            {
                return this[ufn];
            }

            // if the first delta has rcn!=-1 it is the original value
            Delta firstDelta=deltaList[0];
            if(firstDelta.rcn!=RCN_PERSIST_NE_ORIG){
                return firstDelta.value;
            }

            // otherwise the first delta is 0, if there is no other deltas the original is
            // the current value.
            if(deltaList.Count==1){
                return this[ufn];
            }

            // otherwise the original is the 2nd delta.
            Delta secondDelta = deltaList[1];
            return secondDelta.value;
        }
        public void SetOriginal(string fieldName, string value)
        {
            int ufn = this.mvmCluster.GetUfn(fieldName);
             this.SetOriginal(ufn,value);
        }
        public void SetOriginal(int ufn,string value)
        {

            // if no deltas the current is the original
            List<Delta> deltaList;
            if (!this.fieldDeltas.TryGetValue(ufn, out deltaList))
            {
                this[ufn]=value;
                return;
            }

            // if the first delta has rcn!=-1 it is the original value
            Delta firstDelta = deltaList[0];
            if (firstDelta.rcn != RCN_PERSIST_NE_ORIG)
            {
                firstDelta.value=value;
                return;
            }

            // otherwise the first delta is 0, if there is no other deltas the original is
            // the current value.
            if (deltaList.Count == 1)
            {
                this[ufn]=value;
                return;
            }

            // otherwise the original is the 2nd delta.
            Delta secondDelta = deltaList[1];
            secondDelta.value=value;
        }

        /// <summary>
        /// Gets list of the originals that differ from current view
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetChangedOriginals()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            foreach (var delta in this.fieldDeltas)
            {
                int ufn=delta.Key;
                string fieldName=this.mvmCluster.GetFieldName(ufn);
                string currValue = this[ufn];
                string origValue = this.GetOriginal(ufn);
                if (!currValue.Equals(origValue))
                {
                    list.Add(new KeyValuePair<string, string>(fieldName, origValue));
                }
            }
            return list;
        }
       
        public string GetPersisted(string fieldName)
        {
            int ufn = this.mvmCluster.GetUfn(fieldName);
            return GetPersisted(ufn);
        }
        
        public string GetPersisted(int ufn)
        {

            // if no deltas the current is the persisted
            List<Delta> deltaList;
            if (!this.fieldDeltas.TryGetValue(ufn, out deltaList))
            {
                return this[ufn];
            }
            // otherwise the persisted is the first delta
            Delta firstDelta = deltaList[0];
            return firstDelta.value;
        }

        public List<KeyValuePair<string, string>> GetChangedPersisted()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            foreach (var delta in this.fieldDeltas)
            {
                int ufn = delta.Key;
                string fieldName = this.mvmCluster.GetFieldName(ufn);
                string currValue = this[ufn];
                string persistedValue = this.GetPersisted(ufn);
                if (!currValue.Equals(persistedValue))
                {
                    list.Add(new KeyValuePair<string, string>(fieldName, persistedValue));
                }
            }
            return list;
        }

        private void SetField(int ufn, string fieldValue)
        {
            switch (deltaState)
                {
                    case DeltaState.NoTracking:
                        {
                            // if not tracking just set current view
                            this.SetFormattedField(ufn, fieldValue);
                            return;
                        }
                    case DeltaState.SettingPersisted:
                        {
                            // if we are in the setting persisted state, then we update the current view
                            Debug.Assert(fieldDeltas.Count==0, "Not expecting any field deltas while state is "+deltaState);
                            this.SetFormattedField(ufn, fieldValue);
                            return;
                        }
                    case DeltaState.SettingOriginals:
                    {
                        // if we are in set originals, then any deltas we set are 0 deltas meaning deltas from 
                        // persisted
                        string previousValue=this[ufn];
                        // if previous value is same as current value, this is a noop
                        if (fieldValue.Equals(previousValue)) return;
                        // otherwise set the current value and add a persisted delta
                        // there should be no deltas for this field name.
                        Debug.Assert(!fieldDeltas.ContainsKey(ufn), "Not expecting to set original value for a field ufn=[" + ufn + "] more than once");
                        fieldDeltas[ufn] = new List<Delta>() { new Delta(RCN_PERSIST_NE_ORIG, previousValue) };
                        this.SetFormattedField(ufn, fieldValue);
                        return;
                    }
                    case DeltaState.SettingNew:
                    {
                        // when setting new...the first delta an original '1' delta, otherwise it uses
                        // the last snapshot.

                        // get the previous value from the current view
                        string previousValue = this[ufn];
                        
                        // if the current value is the same as the previous value, this is a noop, just return
                        if (fieldValue.Equals(previousValue)) return;

                        // we've already stored the previous value so we can just update the max value
                        this.SetFormattedField(ufn, fieldValue);

                        // since we've already set this field it is possible that it has a delta stored for it. if it
                        // does have a delta already and that delta is is for the same ssn as this one the we can 
                        // simply update the current view. Otherwise we need to create a new delta entry.
                        List<Delta> deltaList;
                        if (!this.fieldDeltas.TryGetValue(ufn, out deltaList))
                        {
                            fieldDeltas[ufn] = new List<Delta>() { new Delta(this.CurrentRcn, previousValue) };
                            return;
                        }
                        // if the last delta's ssn is the same as this one, we've already stored it so we do not need to do
                        // that again.
                        var lastDelta = deltaList[deltaList.Count - 1];
                        if (lastDelta.rcn == this.CurrentRcn)
                        {
                            // if the current value is the same as the last delta's value then we also
                            // don't need the value from the last delta.
                            if (lastDelta.value.Equals(fieldValue))
                            {
                                deltaList.RemoveLast();
                                if (deltaList.Count == 0) this.fieldDeltas.Remove(ufn);
                            }
                            return;
                        }

                        // otherwise, there has been a snapshot since the last field change so we do need to store the delta
                        deltaList.Add(new Delta(this.CurrentRcn, previousValue));
                        
                        return;
                    }
            }
        }

      
        #region overridden members

        /// <summary>
        /// Inherits all the fields from source onto target including history
        /// </summary>
        /// <param name="sourceObjectId"></param>
        public override void InheritAll(string sourceObjectId)
        {
            // ** ROB this method is dangerous... figure out where we use 
            // ** it and make sure we're not causing cloning errors.
            using (var srcObj = this.ObjectCache.CheckOut(sourceObjectId))
            {
                string saveObjectId = this.objectId;
                foreach (var kv in srcObj.FieldKeyValuesPairs)
                {
                    this[kv.Key] = kv.Value;
                }
                this["object_id"] = saveObjectId;


                ObjectDataFormattedDelta srcObjDelta = srcObj as ObjectDataFormattedDelta;
                if (srcObjDelta != null)
                {
                    this.fieldDeltas.AddAll(srcObjDelta.fieldDeltas);
                    this.persistTables.AddAll(srcObjDelta.persistTables);
                    this.configuredUpdateTriggers.AddRange(srcObjDelta.configuredUpdateTriggers);
                    this.deltaState = srcObjDelta.deltaState;
                }
            }
        }

       

        #region Snapshots


        /// <summary>
        /// This RCN is reserved for when you write persisted values, then set overwrite them
        /// with original values. This will only ever appear in index 0 of delta fields.
        /// </summary>
        public static readonly long RCN_PERSIST_NE_ORIG = -1L;
        /// <summary>
        /// This RCN is reserved for when the persisted value is the same as the original value and the
        /// change cannot be attributed to a specific 'real' snapshot because that snapshot was forgotten. Basically this 
        /// number will is lower then any real snapshot RCN so getting a snapshotted value is guaranteed to pick this up.
        /// </summary>
        public static readonly long RCN_PERSIST_EQ_ORIG = 0L;



        // here is how to read this:
        //   a=[]  (-1,a_persist)(1,a_orig)(2,a_new1)(3,a_new2)
        //          * a_persist is the persisted value
        //          * up until rcn 1, the value was a_orig
        //          * up until rcn 2, the value was a_new1
        //          * up until rcn 3, the value was a_new2

        //   b=[b_new3]    (-1,b_persist)(1,)(2,b_new1)(3,b_new2)
        //   c=[c_new3]    (1,)(2,c_new1)(3,c_new2)
        //   d=[d_new3]    (2,)(3,d_new2)



        /// <summary>
        /// Rollback the object to its persisted state throwing away any snapshots since then including the original view.
        /// </summary>
        public void RollbackToPersistedState()
        {
            foreach (int ufn in this.fieldDeltas.Keys.ToList())
            {
                List<Delta> deltaList = this.fieldDeltas[ufn];
                // make the first delta the current value and throw away deltas
                string persistedValue = deltaList[0].value;
                this.fieldDeltas.Remove(ufn);
                this.SetFormattedField(ufn, persistedValue);
               
            }
        }

        /// <summary>
        /// Rollback the object to its original state throwing away any snapshots since. Persisted snapshot is still available.
        /// </summary>
        public void RollbackToOriginalState()
        {
            foreach (int ufn in this.fieldDeltas.Keys.ToList())
            {
                // possible cases...
                // case 0: nothing                          -> should not have this case but handle it anyways           
                // case 1: (0,persisted)                    -> current is original           
                // case 2: (0,persisted)(99,original)()...  -> current is some new value
                // case 3: (99,persisted&original)()...     -> current is some new value 
                
                // search the field list from 0 to forward for speed as well be truncating the list
                List<Delta> deltaList = this.fieldDeltas[ufn];
                
                // case 0:
                if(deltaList.Count==0){
                    fieldDeltas.Remove(ufn);
                }
                if (deltaList[0].rcn == RCN_PERSIST_NE_ORIG)
                {
                    // case 1:
                    if(deltaList.Count==1){
                        continue;
                    }
                    // case 2:
                    {
                        this.SetFormattedField(ufn, deltaList[1].value);
                        //this._fields[fieldName] = deltaList[1].value;
                        if(deltaList.Count > 2){
                            deltaList.RemoveRange(1,deltaList.Count-1);
                        }
                        continue;
                    }
                }
                //case 3
                {
                   // this._fields[fieldName] = deltaList[0].value;
                    this.SetFormattedField(ufn, deltaList[0].value);
                    this.fieldDeltas.Remove(ufn);
                }
            }
        }

        /// <summary>
        /// Rollback the object to look as it did when a snapshot ssn happened
        /// </summary>
        /// <param name="rcn"></param>
        public void RollbackToSnapshot(long rcn)
        {
            foreach (int ufn in this.fieldDeltas.Keys.ToList())
            {
                // search the field list from the end to the beginning to tune for
                // rolling back to the most recent snapshot
                List<Delta> deltaList = this.fieldDeltas[ufn];
                if (deltaList.Count == 0)
                {
                    this.fieldDeltas.Remove(ufn);
                    continue;
                }
                // find the earliest delta after the passed ssn
                int idx = deltaList.Count;
                while ((idx - 1) >= 0 && rcn < deltaList[idx - 1].rcn )
                {
                    idx--;
                }
                // rollback nothing
                if (idx >= deltaList.Count) continue;

                // rollback thru the index
                //this._fields[fieldName]=deltaList[idx].value;
                this.SetFormattedField(ufn, deltaList[idx].value);
                deltaList.RemoveRange(idx, deltaList.Count - idx);
                if (deltaList.Count == 0)
                {
                    this.fieldDeltas.Remove(ufn);
                }
            }
        }

        /// <summary>
        /// Property for VS debugging of field deltas structure.
        /// </summary>
        public string DebugFieldDeltas
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var entry in this.fieldDeltas)
                {
                    string line = entry.Key + ": " + entry.Value.Select(d => "(" + d.ToString() + ")").JoinStrings("");
                    sb.AppendLine(line);
                }
                return sb.ToString();
            }
        }

       
        /// <summary>
        /// Forgets all snapshots greater than or equal to the passed minForgottenSsn. This does not rollback the 
        /// current view, it throws away delta information to save space.
        /// </summary>
        /// <param name="minForgottenSsn"></param>
        public void ForgetSnapshots(long minForgottenSsn)
        {
            foreach (int ufn in this.fieldDeltas.Keys.ToList())
            {
                // search the field list from the end to the beginning 
                List<Delta> deltaList = this.fieldDeltas[ufn];
                if (deltaList.Count == 0)
                {
                    this.fieldDeltas.Remove(ufn); // housekeeping. do not need to keep an empty list
                    continue;
                }

                // work backwards deciding whether to:
                // 1) remove the current index
                // 2) update the current index.ssn=currentssn
                // 3) break.
                for (int idx = deltaList.Count - 1; idx >= 0; idx--)
                {
                    if (minForgottenSsn < deltaList[idx].rcn)
                    {
                        // if we're on second to last delta and the last one is ssn=0, then we should update this and break
                        if (idx == 1 && deltaList[0].rcn == RCN_PERSIST_NE_ORIG)
                        {
                            deltaList[0].rcn = this.CurrentRcn;
                            break;
                        }
                        // if we're on idx 0, then for this field persisted=orig. Then at some point in the future
                        // the value was changed. So make it look that way. Force the rcn to 1 so that any snapshot will
                        // we need something that is before any snapshot... 
                        if (idx == 0)
                        {
                            Debug.Assert(deltaList[0].rcn != RCN_PERSIST_NE_ORIG);
                            deltaList[0].rcn = RCN_PERSIST_EQ_ORIG;
                            break;
                        }
                        // otherwise, just remove the delta
                        deltaList.RemoveAt(idx);
                        if (deltaList.Count == 0)
                        {
                            this.fieldDeltas.Remove(ufn);
                        }
                        continue;
                    }
                    break;
                }
            }
        }

        #endregion


        /// <summary>
        /// This makes the original view the same as . It does not mess with the persisted view.
        /// </summary>
        public void ClearOriginals()
        {
           
            throw new Exception("no longer used");
        }

        #endregion

        #region persistence

        // true if row, else false
        public static readonly int STATUS_NO_ROW=1;
        public static readonly int STATUS_HAS_ROW=2;
        public static readonly int STATUS_UNKNOWN=3;
        public static readonly Dictionary<int, string> StatusNames = new Dictionary<int, string>(){
            {STATUS_HAS_ROW,"HAS_ROW"},
            {STATUS_NO_ROW,"NO_ROW"},
            {STATUS_UNKNOWN,"UNKNOWN"},
            {0,"INVALID"}
        };

        public Dictionary<string, int> persistTables = new Dictionary<string, int>();

        // specifys that that an object is already persisted in the table
        public void AddPersistFromTable(string tableName)
        {
            if (this.persistTables == null) this.persistTables = new Dictionary<string, int>();
            this.persistTables[tableName] = STATUS_HAS_ROW;
        }

        // specifies that an object should be persisted in the table.
        public bool AddPersistTable(string tableName)
        {
            if (this.persistTables == null) this.persistTables = new Dictionary<string, int>();
            if (!this.persistTables.ContainsKey(tableName))
            {
                this.persistTables[tableName] = STATUS_NO_ROW;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets the table status, returns true if this is the first time or a noop, false if this is updating the table status to a new value.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableStatus"></param>
        /// <returns></returns>
        public bool AddPersistTable(string tableName,int tableStatus)
        {
            if (this.persistTables == null) this.persistTables = new Dictionary<string, int>();
            if (!this.persistTables.ContainsKey(tableName))
            {
                this.persistTables[tableName] = tableStatus;
                return true;
            }
            else if (this.persistTables[tableName] == tableStatus)
            {
                return true;
            }
            this.persistTables[tableName] = tableStatus;
            return false;
        }


        // persistedObjectRefFields{table_name}{ufn}=true, says this field contains an object
        // ref that we should serialize into this db field.
        public Dictionary<string, Dictionary<int, bool>> persistedObjectRefFields =new Dictionary<string, Dictionary<int, bool>>();
        public void AddPersistObjectRefField(string tableName,string fieldName)
        {
            int ufn=this.mvmCluster.GetUfn(fieldName);
            if(!persistedObjectRefFields.ContainsKey(tableName)) this.persistedObjectRefFields[tableName]=new Dictionary<int,bool>();
            if(!persistedObjectRefFields[tableName].ContainsKey(ufn)) this.persistedObjectRefFields[tableName][ufn]=true;
        }

        [MvmExport("object_delta_add_object_ref_field")]
        public static void AddPersistedObjectRefField(ModuleContext mc, string object_id, string tableName, string fieldName)
        {
            using (IObjectData obj = mc.mvm.objectCache.CheckOut(object_id))
            {
                ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                if (deltaObj != null)
                {
                    deltaObj.AddPersistObjectRefField(tableName, fieldName);
                }
                else
                {
                    throw new Exception("Error, object_id=[" + object_id + "] is not a delta object");
                }
            }
        }

        public StaticDbLoginInfo staticDbLoginInfo;

        /// <summary>
        /// Flushes all the the changes that may be queued up.
        /// every format gets its own bulk loader but not its own table name...
        /// want to truncate tables on a table by table basis, not loader basis.
        /// </summary>
        public static void FlushAll(MvmEngine mvm)
        {
            // if no commands to run, then nothing to flush
            if (bulkLoaders.Count == 0 && bulkUpdaters.Count==0)
            {
                if (mvm.globalContext["log_erd"].Equals("1")) mvm.Log("NO POSSIBLE CHANGES TO COMMIT");
                return;
            }

            // need to flush all the bulk loaders with data.
            // need to generate a pl/sql block to run all the updates and inserts.
            foreach (IBulkLoader loader in bulkLoaders.Values)
            {
                loader.Flush();
            }

            foreach (IBulkLoader loader in bulkPushedUsage.Values)
            {
                loader.Flush();
            }

            foreach (BulkUpdater updater in bulkUpdaters.Values)
            {
                updater.Flush();
            }


            // generate a pl/sql block to fire all the updates/inserts then all the truncates.
            string upserts = bulkCommands.Values.JoinStrings(" ".AppendLine());

            StaticDbLoginInfo dbInfo = mvm.GetDefaultDbLogin();
            string cmd;
            // get list of all temp tables
            List<string> tempTables = new List<string>();
            tempTables.AddRange(bulkLoaders.Values.Select(t => t.TableName).Distinct());
            tempTables.AddRange(bulkUpdaters.Values.Select(t => t.BulkTableName).Distinct());
            tempTables.AddRange(bulkPushedUsage.Values.Select(t => t.TableName).Distinct());
            // build the update command
            if (dbInfo.type.Equals("oracle"))
            {
                string truncates = tempTables.Distinct().Select(t => "delete " + t + ";").JoinStrings();
                cmd = "declare begin " + upserts + " " + truncates + "end;";
            }
            else
            {
                string truncates = tempTables.Distinct().Select(t => "exec ('truncate table " + t + "');").JoinStrings();
                // http://msdn.microsoft.com/en-us/library/ms175976.aspx
                cmd =
@"begin 
BEGIN TRY 
declare @errmsg NVARCHAR(4000);
SET XACT_ABORT ON; 
BEGIN TRANSACTION; 
" + upserts + @" 
" + truncates + @" 
COMMIT TRANSACTION; 
END TRY 
BEGIN CATCH 
IF (XACT_STATE()) = -1 
BEGIN
ROLLBACK TRANSACTION;
   select @errmsg=ERROR_MESSAGE() 
   RAISERROR (@errmsg, 16, 1)
END; 
IF (XACT_STATE()) = 1
BEGIN
COMMIT TRANSACTION;
END; 
END CATCH; 
end;";
            }

            if(mvm.globalContext["log_erd"].Equals("1")) mvm.Log("CALL ERD COMMIT COMMAND:" + cmd );
            DbUtils.DbExecute(dbInfo, cmd);
            if (mvm.globalContext["log_erd"].Equals("1")) mvm.Log("DONE ERD COMMIT COMMAND:" + cmd);
        }

        // incudes all bulk update/insert/delete commands
        public static Dictionary<StringArray, string> bulkCommands = new Dictionary<StringArray, string>();
        // bulk inserters and deleters mixed in
        public static Dictionary<StringArray, IBulkLoader> bulkLoaders = new Dictionary<StringArray, IBulkLoader>();
        // bulk updaters only
        public static Dictionary<StringArray, BulkUpdater> bulkUpdaters = new Dictionary<StringArray, BulkUpdater>();

        public static Dictionary<StringArray, IBulkLoader> bulkPushedUsage=new Dictionary<StringArray,IBulkLoader>();

        public static IBulkLoader GetBulkPushedUsage(string type, string server, string database, string user, string password, string table, int commitSize)
        {
            lock (bulkLoaders)
            {
                StringArray key = new StringArray(type, server, database, user, table);
                if (bulkPushedUsage.ContainsKey(key)) return bulkPushedUsage[key];
                if (type.Equals("oracle"))
                {
                    throw new Exception("get pushed usage working on oracle");
                    //OracleBulkLoader bcp = new OracleBulkLoader(server, database, user, password, table, commitSize);
                    //bulkPushedUsage[key] = bcp;
                    //string bulkCommand = "exec mvm_push_usage(" + table.q() + ");";
                    //bulkCommands[key] = bulkCommand;
                    //return bcp;
                }
                else if (type.Equals("sql"))              {
                    SqlBulkLoader bcp = new SqlBulkLoader(server, database, user, password, table, commitSize);
                    bulkPushedUsage[key] = bcp;
                    string bulkCommand =
                        //delete any rows with a new usage interval > current new_usage_interval
                    " delete a from agg_pushed_usage a where exists (select 1 from  " + table + " b where a.id_sess=b.id_sess and ( (a.old_usage_interval > b.old_usage_interval) or (a.old_usage_interval = b.old_usage_interval and b.old_usage_interval=b.new_usage_interval )));".AppendLine()
                        //update any existing rows with the usage_interval_id
                    + " WITH CTE AS (select old.new_usage_interval old_0,new.new_usage_interval new_0 from agg_pushed_usage old," + table + " new where old.id_sess=new.id_sess) UPDATE CTE set old_0=new_0;".AppendLine()
                        //insert the current row if not there
                    + " insert into agg_pushed_usage  (id_acc,id_sess,old_usage_interval,new_usage_interval) (select a.id_acc,a.id_sess,a.old_usage_interval,a.new_usage_interval from  " + table + " a left outer join agg_pushed_usage b on a.id_sess=b.id_sess and a.old_usage_interval=b.old_usage_interval where b.old_usage_interval is null and a.old_usage_interval < a.new_usage_interval);";
                    bulkCommands[key] = bulkCommand;
                    return bcp;
                }
                else
                {
                    throw new Exception("GetBulkPushedUsage: Error, unknown db_type=[" + type + "]");
                }
            }
        }

        public static IBulkLoader GetBulkInserter(string type, string server, string database, string user, string password, string table, int commitSize, string targetTable)
        {
            lock (bulkLoaders)
            {
                StringArray key = new StringArray(type, server, database, user, table);
                if (bulkLoaders.ContainsKey(key)) return bulkLoaders[key];
                if (type.Equals("oracle"))
                {
                    OracleBulkLoader bcp = new OracleBulkLoader(server, database, user, password, table, commitSize);
                    bulkLoaders[key] = bcp;
                    string bulkCommand = "insert into " + targetTable + " (" + bcp.orderedFieldNames.JoinStrings(",") + ") (select " + bcp.orderedFieldNames.JoinStrings(",") + " from " + table + ");";
                    bulkCommands[key] = bulkCommand;
                    return bcp;
                }
                else if (type.Equals("sql"))
                {
                    SqlBulkLoader bcp = new SqlBulkLoader(server, database, user, password, table, commitSize);
                    bulkLoaders[key] = bcp;
                    string bulkCommand = "insert into " + targetTable + " (" + bcp.orderedFieldNames.JoinStrings(",") + ") (select " + bcp.orderedFieldNames.JoinStrings(",") + " from " + table + ");";
                    bulkCommands[key] = bulkCommand;
                    return bcp;
                }
                else
                {
                    throw new Exception("GetBulkInserter: Error, unknown db_type=[" + type + "]");
                }
            }
        }

        public static IBulkLoader GetBulkUpserter(string type, string server, string database, string user, string password, string table, int commitSize, string targetTable, string[] pkCols)
        {
            lock (bulkLoaders)
            {

                StringArray key = new StringArray(type, server, database, user, table, "upserter");
                if (bulkLoaders.ContainsKey(key)) return bulkLoaders[key];
                if (type.Equals("oracle"))
                {
                    throw new Exception("GetBulkUpserter not done for Oracle");
                    //OracleBulkLoader bcp = new OracleBulkLoader(server, database, user, password, table, commitSize);
                    //bulkLoaders[key] = bcp;
                    //string bulkCommand = "insert into " + targetTable + " (" + bcp.orderedFieldNames.JoinStrings(",") + ") (select " + bcp.orderedFieldNames.JoinStrings(",") + " from " + table + ");";
                    //bulkCommands[key] = bulkCommand;
                    //return bcp;
                }
                else if (type.Equals("sql"))
                {
                    SqlBulkLoader bcp = new SqlBulkLoader(server, database, user, password, table, commitSize);
                    bulkLoaders[key] = bcp;
                    string bulkCommand = 
                        "merge into " + targetTable + " as Target "
                        + " using "+table+" as Source"
                        + " on " + pkCols.Select(f=>"Source."+f+"=Target."+f).JoinStrings(",")
                        + " when matched then "
                        + " update set " + bcp.orderedFieldNames.Select(f=>"Target."+f+"=Source."+f).JoinStrings(",")
                        + " when not matched then "
                        + " insert (" + bcp.orderedFieldNames.JoinStrings(",") + ") values (" + bcp.orderedFieldNames.Select(f=>"Source."+f).JoinStrings(",") +");";
                    bulkCommands[key] = bulkCommand;
                    return bcp;
                }
                else
                {
                    throw new Exception("GetBulkUpserter: Error, unknown db_type=[" + type + "]");
                }
            }
        }

        public static IBulkLoader GetBulkDeleter(string type, string server, string database, string user, string password, string table, int commitSize, string targetTable, string[] pkCols)
        {
            lock (bulkLoaders)
            {
                StringArray key = new StringArray(type, server, database, user, table, "deleter");
                if (bulkLoaders.ContainsKey(key)) return bulkLoaders[key];
                if (type.Equals("oracle"))
                {

                    OracleBulkLoader bcp = new OracleBulkLoader(server, database, user, password, table, commitSize, pkCols);
                    bulkLoaders[key] = bcp;
                    string bulkCommand = "delete from " + targetTable + " a where exists (select 1 from " + table + " b where " + bcp.orderedFieldNames.Select(f => "a." + f + "=b." + f).JoinStrings(" and ") + ");";
                    bulkCommands[key] = bulkCommand;
                    return bcp;
                }
                else if (type.Equals("sql"))
                {
                    SqlBulkLoader bcp = new SqlBulkLoader(server, database, user, password, table, commitSize, pkCols);
                    bulkLoaders[key] = bcp;
                    string bulkCommand = "delete a from " + targetTable + " a where exists (select 1 from " + table + " b where " + bcp.orderedFieldNames.Select(f => "a." + f + "=b." + f).JoinStrings(" and ") + ");";
                    bulkCommands[key] = bulkCommand;
                    return bcp;
                }
                else
                {
                    throw new Exception("GetBulkDeleter: Error, unknown db_type=[" + type + "]");
                }
            }
        }


        public BulkUpdater GetBulkUpdater(string type, string server, string database, string user, string password, string table, int commitSize, int formatId, string[] formatFields, string targetTable)
        {
            lock (bulkUpdaters)
            {
                // if we already have have a bulk inserter, return it.
                StringArray key = new StringArray(type, server, database, user, table, formatId.ToString());
                if (bulkUpdaters.ContainsKey(key)) return bulkUpdaters[key];

                // otherwise, create one and return it.
                if (type.Equals("oracle"))
                {
                    OracleBulkUpdater bcp = new OracleBulkUpdater(this.schemaMaster,server, database, user, password, table,targetTable, commitSize, formatId,formatFields);
                    bulkUpdaters[key] = bcp;
                    bulkCommands[key] = bcp.bulkCommand;
                    return bcp;
                }
                else if (type.Equals("sql"))
                {
                    SqlBulkUpdater bcp = new SqlBulkUpdater(this.schemaMaster, server, database, user, password, table, targetTable, commitSize, formatId, formatFields);
                    bulkUpdaters[key] = bcp;
                    bulkCommands[key] = bcp.bulkCommand;
                    return bcp;
                }
                else
                {
                    throw new Exception("GetBulkUpdater: Error, unknown db_type=[" + type + "]");
                }
            }
        }

        public string modifiedDateValue;
        public string modifiedDateField;

        /// <summary>
        /// Looks at info gathed in Persist() to persist changes to AGG_USAGE_AUDIT_TRAIL. This is special cased
        /// for AMP usage hook.
        /// </summary>
        public void PersistAudit()
        {
            bool needAuat = this.InsertedOrUpdatedAny;

            // Need to add auat to the inserted or updated db fields list which is used for packing originals. Also
            // if any auat field is modified, we need auat.
            var tableInfo = this.schemaMaster.GetTableInfo(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, this.auditTrailTable);
            foreach (string fieldName in this.GetChangedPersisted().Select(x=>x.Key))
            {
                if (tableInfo.columnInfoDictionary.ContainsKey(fieldName))
                {
                    this.insertedOrUpdatedDbFields[fieldName] = true;
                    needAuat = true;
                }
            }

            // if we do not need auat, just return.
            if (!needAuat) return;

            // TUNE THIS!
            // set packed originals:
            // remove any delta tracking for fields that are cannot be persisted
            //var originalDeltasSnap = this.originalDeltas.Keys.ToArray();
            var originalDeltasSnap = this.GetChangedOriginals().Select(kv=>kv.Key);

            foreach (string f in originalDeltasSnap)
            {
                bool isPersistedSomewhere = false;
                foreach (string table in this.persistTables.Keys)
                {
                    var ti = this.schemaMaster.GetTableInfo(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, table);
                    if (ti.columnInfoDictionary.ContainsKey(f))
                    {
                        isPersistedSomewhere = true;
                        break;
                    }
                }
                // if we get here there is no way the field gets persisted, so remove it.
                if (!isPersistedSomewhere)
                {
                    int ufn = this.mvmCluster.GetUfn(f);
                    this.fieldDeltas.Remove(ufn);
                }
            }

            // set the modified date
            this[this.modifiedDateField] = this.modifiedDateValue;

            // pack the originals
            this[auditTrailField] = this.GetPackedOriginals();

            // upsert the auat   
            int action=this.persistTables.GetValueDefaulted(auditTrailTable, STATUS_NO_ROW);
            if (action == STATUS_HAS_ROW)
            {
                this.UpdateTable(auditTrailTable, insertedOrUpdatedDbFields);
            }
            else if (action == STATUS_NO_ROW)
            {
                this.InsertIntoTable(auditTrailTable, insertedOrUpdatedDbFields);
                // TBD: we can bulk load directly into audit trail if we want.
            }
            else if (action == STATUS_UNKNOWN)
            {
                this.UpsertTable(auditTrailTable, insertedOrUpdatedDbFields);
            }
            else
            {
                throw new Exception("Unexpected table status table=["+auditTrailTable+"] status=["+action+"]");
            }
        }

        /// <summary>
        /// Not really sure why this is still here... looks like we do not use it anymore
        /// </summary>
        public Dictionary<string, bool> insertedOrUpdatedDbFields;

        /// <summary>
        /// Set target login the first time this is called by looking in GLOBAL.target_login
        /// </summary>
        private void SetTargetLogin(){
            if (this.staticDbLoginInfo == null)
            {
                string targetLoginOid = this.mvm.globalContext["target_login"];
                if (targetLoginOid.IsNullOrEmpty()) throw new Exception("Cannot persist object without GLOBAL.target_login set");
                using (IObjectData targetLogin = this.ObjectCache.CheckOut(targetLoginOid))
                {
                    this.staticDbLoginInfo = new StaticDbLoginInfo(targetLogin["database_server"], targetLogin["database_name"], targetLogin["database_user"], targetLogin["database_password"], targetLogin["database_type"]);
                }
            }
        }


        /// <summary>
        /// Persists the changes between the passed persisted object and the current object...
        /// this 'should' be a singleton or static method but I am leaving it as an instance method 
        /// for the wiring into mvm.
        /// </summary>
        public void PersistObjectChanges(ObjectDataFormattedDelta persistedObj, ObjectDataFormattedDelta currentObj)
        {
            this.SetTargetLogin();

            // if persisted is null and current is NOT null, wholesale insert current
            if (persistedObj == null && currentObj != null)
            {
                currentObj.Persist(null, null, null, null);
                return;
            }

            // if persisted is NOT null and current is null, wholesale deleted persisted.
            if (persistedObj != null && currentObj == null)
            {
                persistedObj["erd_deleted"] = "1";
                persistedObj.Persist(null, null, null, null);
                return;
            }

            // delete any table that is in the persisted object but not in the current object
            var deletedTables = persistedObj.persistTables
                .Where(t => t.Value == STATUS_HAS_ROW)
                .Select(t => t.Key)
                .Except(currentObj.persistTables.Select(tt => tt.Key));
            foreach (var deletedTable in deletedTables)
            {
               persistedObj.DeleteTable(deletedTable);
            }

            this.insertedOrUpdatedDbFields = new Dictionary<string, bool>();

            // insert any table that is in the current object but not in the persisted object
            var newTables = currentObj.persistTables
                  .Select(t => t.Key)
                  .Except(persistTables.Select(tt => tt.Key));
            foreach (var newTable in newTables)
            {
                persistedObj.InsertIntoTable(newTable, insertedOrUpdatedDbFields);
            }

            // that is in both current object and persisted object with field changes.
            var updateTables = currentObj.persistTables
                .Select(t => t.Key)
                .Intersect(persistTables.Select(tt => tt.Key));
            foreach (var updateTable in updateTables)
            {
                persistedObj.TryUpdateTable(updateTable, currentObj);
            }
        }

        /// <summary>
        /// Write the updates to a bulk update table and marks the fields that are set to be updated.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="markUpdatedFields"></param>
        private void TryUpdateTable(string table, ObjectDataFormattedDelta currentObj)
        {
            // lookup the table fields, find the intersection and 
            var tableInfo = this.schemaMaster.GetTableInfo(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, table);

            // need to compare the changed fields with the table fields and build a format out of it.
            List<string> updateFields = new List<string>();
            foreach (ColumnInfo colInfo in tableInfo.columnInfo)
            {
                string fieldName = colInfo.name;
                string currentValue = currentObj[fieldName];
                string persistedValue = this.GetPersistedValue(fieldName);
                if (!currentValue.Equals(persistedValue))
                {
                    // if the column is numeric and the fields are numerically equivelent then
                    // the field is not to be treated as a delta.
                    if (colInfo.numeric)
                    {
                        if (currentValue.IsNumericEq(persistedValue))
                        {
                            continue;
                        }
                    }
                    updateFields.Add(fieldName);
                }
            }

            if (updateFields.Count == 0) return;

            // sort the update fields and get a format_id for them.
            updateFields.Sort();
            int formatId = this.mvm.mvmCluster.GetFormatId(updateFields);

            // Get the name of the bulk update table from the schema master since multiple bulk updaters can use the
            // same bulk update table by varying the format_id
            string bulkTable = this.schemaMaster.GetBulkUpdateTableName(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, table);
            BulkUpdater bulkUpdater = GetBulkUpdater(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, bulkTable, 1000, formatId, updateFields.ToArray(), table);

            // construct update row.
            string[] row = new string[bulkUpdater.RowSize];
            int colNo = 0;
            // format_id
            row[colNo++] = formatId.ToString();
            // original pk fields
            foreach (var col in bulkUpdater.orderedPrimaryKeyFields)
            {
                row[colNo++] = this.GetPersistedValue(col);
            }
            // updated fields
            foreach (var fieldName in bulkUpdater.orderedUpdateFields)
            {
                string currentValue=
                row[colNo++] = currentObj[fieldName];
            }
            // insert the row.
            bulkUpdater.InsertRow(row);
            // mark that we inserted
            this.insertedOrUpdatedTables[table] = false;
        }


        private void SerializePersistedObjectRefs()
        {
            // Go thru any persisted nested table fields, if the objects have changed
            // from what is persisted then serialize the object into the field.
            foreach (var t in this.persistedObjectRefFields)
            {
                string tableName = t.Key;
                foreach (var tt in t.Value)
                {
                    int ufn = tt.Key;
                    string object_id = this[ufn];
                    if (object_id.IsNullOrEmpty())
                    {
                        continue;
                    }
                    using (IObjectData obj = this.mvm.objectCache.CheckOut(object_id))
                    {
                        ObjectDataFormattedDelta deltaObj = obj as ObjectDataFormattedDelta;
                        if (deltaObj != null)
                        {
                            if (deltaObj.HasPersistedChanges)
                            {
                                string serializedObj = deltaObj.SerializeForDb();
                                this[ufn] = serializedObj;
                            }
                            else
                            {
                                this[ufn] = this.GetOriginal(ufn); // just in case rollback to orig value so we get no db update.
                            }
                        }
                        else
                        {
                            throw new Exception("Error, object_id=[" + object_id + "] is not a delta object");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Persists the object to the database.
        /// </summary>
        public void Persist(string auditTableName, string modifiedDateField, string modifiedDateValue, string packedOriginalsFieldName)
        {
            this.SetTargetLogin();
            this.auditTrailField = packedOriginalsFieldName;
            this.auditTrailTable = auditTableName;
            this.modifiedDateValue = modifiedDateValue;
            this.modifiedDateField = modifiedDateField;

            this.SerializePersistedObjectRefs();

            // if this object is deleted, setup bulk deletes for al the tables that have rows.
            if (this["erd_deleted"].Equals("1"))
            {
                foreach (var t in this.persistTables)
                {
                    string tableName = t.Key;
                    bool hasRow = t.Value==STATUS_HAS_ROW || t.Value==STATUS_UNKNOWN;
                    if (hasRow)
                    {
                        this.DeleteTable(tableName);
                    }
                }
            }
            // otherwise,figure out what fields need to be inserted,updated,upserted in the db.
            else
            {
                this.insertedOrUpdatedDbFields = new Dictionary<string, bool>();
                foreach (var t in this.persistTables.Where(t => !t.Key.Equals(this.auditTrailTable)))
                {
                    string tableName = t.Key;
                   
                    if(t.Value == STATUS_HAS_ROW)
                    {
                        this.UpdateTable(tableName, insertedOrUpdatedDbFields);
                    }
                    else if (t.Value == STATUS_NO_ROW)
                    {
                        this.InsertIntoTable(tableName, insertedOrUpdatedDbFields);
                    }
                    else if (t.Value == STATUS_UNKNOWN)
                    {
                        this.UpsertTable(tableName, insertedOrUpdatedDbFields);
                    }
                    else
                    {
                        throw new Exception("Unexpected table table=["+t.Key+"] status=["+t.Value+"]");
                    }
                }
            }

            // if the id_usage_interval has changed need to maintain AGG_PUSHED_USAGE
            string newUsageInterval = this["id_usage_interval"];
            string oldUsageInterval = this.GetOriginalValue("id_usage_interval");
            string persistedUsageInterval = this.GetPersistedValue("id_usage_interval");

            string origUsageInterval2 = this["orig_usage_interval"];
            if (origUsageInterval2.IsNullOrEmpty() && !newUsageInterval.Equals(oldUsageInterval))
            {
                this["orig_usage_interval"] = oldUsageInterval;
                this.SetOriginal("orig_usage_interval", oldUsageInterval);
                //logger.Warn("Setting orig to [{0}]", oldUsageInterval);
            }

            //logger.Warn("before updated id_usage_interval from [{0}] to  [{1}] from [{2}]", oldUsageInterval, newUsageInterval, persistedUsageInterval);
            if (!newUsageInterval.Equals(persistedUsageInterval) && persistedUsageInterval.NotNullOrEmpty())
            {
                //logger.Warn("updated id_usage_interval from [{0}] to  [{1}]", oldUsageInterval, newUsageInterval);
                string idAcc = this["id_payee"];
                string idSess = this["id_sess"];

                PushUsage(idAcc, idSess, oldUsageInterval, newUsageInterval);
                
                // Need to blow the original value away
                this.SetOriginal("id_usage_interval", newUsageInterval);

                // Catch the edge of the first time id_usage_interval has been updated. We know we are on the 
                // first time if orig_usage_interval is not set. On this edge we know the old usage interval
                // is the orig_usage_interval so set it and call this.SetOriginalValue() so it doesn't go in
                // AUAT.orig_packed_values
                string origUsageInterval = this["orig_usage_interval"];
                if (origUsageInterval.IsNullOrEmpty())
                {
                    this["orig_usage_interval"] = oldUsageInterval;
                    this.SetOriginal("orig_usage_interval", oldUsageInterval);

                }
            }
            this.SetOriginal("id_usage_interval", newUsageInterval);

        }


        private void PushUsage(string idAcc, string idSess, string oldIdUsageInterval, string newIdUsageInterval)
        {
            string table = "AGG_PUSHED_USAGE";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["old_usage_interval"] = oldIdUsageInterval;
            data["new_usage_interval"] = newIdUsageInterval;
            data["id_acc"] = idAcc;
            data["id_sess"] = idSess;
            // creates the bulk insert table and ensures it has all the columns we need (alters table if needed).
            string bulkTable = this.schemaMaster.GetBulkInsertTableName(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, table);
            //Console.WriteLine("GOT BT: "+table+"=>" + bulkTable);
            IBulkLoader bulkLoader = GetBulkPushedUsage(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, bulkTable, 1000);
            TableInfo tableInfo = this.schemaMaster.GetTableInfo(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, bulkTable);
            var columnsInfo = tableInfo.columnInfo;
            string[] row = new string[columnsInfo.Count];
            int colNo = 0;
            foreach (ColumnInfo col in columnsInfo)
            {
                row[colNo++] = data[col.name];
            }
            bulkLoader.InsertRow(row);
            // mark that we updated
            //this.insertedOrUpdatedTables[table] = true;
        }


        private string auditTrailTable;
        private string auditTrailField;

        /// <summary>
        /// true for an insert we ran, false for an update we ran, no key means neither.
        /// </summary>
        public Dictionary<string, bool> insertedOrUpdatedTables = new Dictionary<string, bool>();

        /// <summary>
        /// Returns true if we actually ran and insert on this table.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool IsInserted(string tableName)
        {
            if (this.insertedOrUpdatedTables.ContainsKey(tableName)) return this.insertedOrUpdatedTables[tableName];
            return false;
        }

        /// <summary>
        /// Returns true if we actually ran an update on this table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool IsUpdated(string tableName)
        {
            if (this.insertedOrUpdatedTables.ContainsKey(tableName)) return !this.insertedOrUpdatedTables[tableName];
            return false;
        }

        /// <summary>
        /// Returns true if we've inserted or updated any table with this object
        /// </summary>
        public bool InsertedOrUpdatedAny
        {
            get
            {
                return this.insertedOrUpdatedTables.Count > 0;
            }
        }

        // client side we want ability to add cols and only alter on commit. or to make it easy, we can
        // flush before alter.
        private void InsertIntoTable(string table, Dictionary<string, bool> markNonPkInsertedFields)
        {
            // creates the bulk insert table and ensures it has all the columns we need (alters table if needed).
            string bulkTable = this.schemaMaster.GetBulkInsertTableName(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, table);
            //Console.WriteLine("GOT BT: "+table+"=>" + bulkTable);
            IBulkLoader bulkLoader = GetBulkInserter(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, bulkTable, 1000, table);
            TableInfo tableInfo = this.schemaMaster.GetTableInfo(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, bulkTable);
            var columnsInfo = tableInfo.columnInfo;
            string[] row = new string[columnsInfo.Count];
            int colNo = 0;
            foreach (ColumnInfo col in columnsInfo)
            {
                markNonPkInsertedFields[col.name] = true;
                row[colNo++] = this[col.name];
            }
            bulkLoader.InsertRow(row);
            // mark that we updated
            this.insertedOrUpdatedTables[table] = true;
        }


        /// <summary>
        /// Bulk upserts a table.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="markNonPkInsertedFields"></param>
        private void UpsertTable(string table, Dictionary<string, bool> markNonPkInsertedFields)
        {
            var targetTableInfo = this.schemaMaster.GetTableInfo(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, table);
           
            // creates the bulk insert table and ensures it has all the columns we need (alters table if needed).
            string bulkTable = this.schemaMaster.GetBulkUpsertTableName(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, table);
            //Console.WriteLine("GOT BT: "+table+"=>" + bulkTable);
            IBulkLoader bulkLoader = GetBulkUpserter(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, bulkTable, 1000, table, targetTableInfo.primaryKeyColumns.ToArray());
            TableInfo tableInfo = this.schemaMaster.GetTableInfo(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, bulkTable);
            var columnsInfo = tableInfo.columnInfo;
            string[] row = new string[columnsInfo.Count];
            int colNo = 0;
            foreach (ColumnInfo col in columnsInfo)
            {
                markNonPkInsertedFields[col.name] = true;
                row[colNo++] = this[col.name];
            }
            bulkLoader.InsertRow(row);
            // mark that we updated
            this.insertedOrUpdatedTables[table] = true;
        }


        public void AddUpdateTriggerToTable(string updateTable, string updateField,string triggeredTable){
            this.configuredUpdateTriggers.Add(new string[] { updateTable, updateField, triggeredTable });
        }

        // deal with triggers in a lazy way so we have easily serializable structs, which 
        // we blow out and index on demand.
        // configuredUpdateTriggers={ update_table,update_field, trigger_table }
        public List<string[]> configuredUpdateTriggers=new List<string[]>();
        // UpdateTriggers[update_table][update_field][trigger_table]=true
        private Dictionary<string, Dictionary<string, Dictionary<string,bool>>> _updateTriggers;
        private Dictionary<string, Dictionary<string, Dictionary<string, bool>>> UpdateTriggers
        {
            get
            {
                if (this._updateTriggers == null)
                {
                    if (this.configuredUpdateTriggers != null)
                    {
                        this._updateTriggers = new Dictionary<string, Dictionary<string, Dictionary<string, bool>>>();
                        foreach (var entry in this.configuredUpdateTriggers)
                        {
                            if (!this._updateTriggers.ContainsKey(entry[0])) this._updateTriggers[entry[0]] = new Dictionary<string, Dictionary<string, bool>>();
                            if (!this._updateTriggers[entry[0]].ContainsKey(entry[1])) this._updateTriggers[entry[0]][entry[1]] = new Dictionary<string, bool>();
                            this._updateTriggers[entry[0]][entry[1]][entry[2]] = true;
                        }
                    }
                }
                return this._updateTriggers;
            }
        }


        /// <summary>
        /// Write the updates to a bulk update table and marks the fields that are set to be updated.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="markUpdatedFields"></param>
        private void UpdateTable(string table, Dictionary<string, bool> markUpdatedFields)
        {
            // lookup the table fields, find the intersection and 
            var tableInfo = this.schemaMaster.GetTableInfo(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, table);

            // need to compare the changed fields with the table fields and build a format out of it.
            List<string> updateFields = new List<string>();
            foreach (var entry in this.GetChangedPersisted().OrderBy(x => x.Key))
            {
                string fieldName=entry.Key;
                string persistedValue=entry.Value;
                ColumnInfo colInfo;
                if (tableInfo.columnInfoDictionary.TryGetValue(fieldName, out colInfo))
                {
                    // if the column is numeric and the fields are numerically equivelent then
                    // the field is not to be treated as a delta.
                    if (colInfo.numeric)
                    {
                        string currentValue = this[fieldName];
                        if (currentValue.IsNumericEq(persistedValue))
                        {
                            continue;
                        }
                    }
                    updateFields.Add(fieldName);
                }
            }

            // if there are any update triggers on the object, check the updateFields 
            // for this table and fire any update table inserts.
            if (this.UpdateTriggers != null)
            {
                Dictionary<string, Dictionary<string, bool>> updateTriggerFields;
                if(this.UpdateTriggers.TryGetValue(table,out updateTriggerFields)){
                    foreach (string updateField in updateFields)
                    {
                        Dictionary<string, bool> updateFiredTables;
                        if (updateTriggerFields.TryGetValue(updateField, out updateFiredTables))
                        {
                            foreach (string firedTable in updateFiredTables.Keys)
                            {
                                // if we are not already persisted, then insert into the the fired table,
                                // otherwise it will be updated and we do not need to do anything.
                                if (!this.persistTables.ContainsKey(firedTable))
                                {
                                    //logger.Info("FIRING INSERT FOR TABLE:" + firedTable);
                                    this.InsertIntoTable(firedTable, markUpdatedFields);
                                }
                            }
                        }
                    }
                }
            }


            

            // cut out if nothing was updated
            if (updateFields.Count == 0) return;

            // sort the update fields and get a format_id for them.
            updateFields.Sort();
            int formatId = this.mvm.mvmCluster.GetFormatId(updateFields);

            // Get the name of the bulk update table from the schema master since multiple bulk updaters can use the
            // same bulk update table by varying the format_id
            string bulkTable = this.schemaMaster.GetBulkUpdateTableName(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, table);
            BulkUpdater bulkUpdater = GetBulkUpdater(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, bulkTable, 1000, formatId, updateFields.ToArray(), table);
            
            // construct update row.
            string[] row = new string[bulkUpdater.RowSize];
            int colNo = 0;
            // format_id
            row[colNo++] = formatId.ToString();
            // original pk fields
            foreach (var col in bulkUpdater.orderedPrimaryKeyFields)
            {
                row[colNo++] = this.GetPersistedValue(col);
            }
            // updated fields
            foreach (var col in bulkUpdater.orderedUpdateFields)
            {
               row[colNo++] = this[col];
               markUpdatedFields[col] = true;
            }
            // insert the row.
            bulkUpdater.InsertRow(row);
            // mark that we inserted
            this.insertedOrUpdatedTables[table] = false;
        }


        
        
        /// <summary>
        /// Write the pk information to a bulk delete table so we can fire the deletes later.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="markUpdatedFields"></param>
        private void DeleteTable(string table)
        {
            // lookup the table fields, find the intersection and 
            var tableInfo = this.schemaMaster.GetTableInfo(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, table);
            var pkCols = tableInfo.primaryKeyColumns.ToArray();

            // error if table does not have a pk
            if (pkCols.Length == 0)
            {
                throw new Exception("Error, cannot delete from a table without a primary key: " + table);
            }

            // make sure the pk fields are not null
            foreach (var pkField in pkCols)
            {
                if (this[pkField].IsNullOrEmpty()) throw new Exception("Error, cannot delete table with null primary key field: " + table + "." + pkField);
            }

            // Get the bulk delete table name
            string bulkTable = this.schemaMaster.GetBulkDeleteTableName(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, table);

            // Get the build delete bulk loader
            IBulkLoader bulkLoader = GetBulkDeleter(this.staticDbLoginInfo.type, this.staticDbLoginInfo.server, this.staticDbLoginInfo.db, this.staticDbLoginInfo.user, this.staticDbLoginInfo.pw, bulkTable, 1000, table, pkCols);

            // add a row
            string[] row = new string[bulkLoader.GetOrderedFieldNames().Count];
            int colNo = 0;
            foreach (var col in bulkLoader.GetOrderedFieldNames())
            {
                row[colNo++] = this[col];
            }
            bulkLoader.InsertRow(row);
        }
        #endregion

       
        #region original values

        /// <summary>
        /// Snapshotted original values that differ from the current value
        /// </summary>
        //public Dictionary<string, string> originalDeltas = new Dictionary<string, string>();

        /// <summary>
        /// Returns the original value
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string GetOriginalValue(string fieldName)
        {
            return this.GetOriginal(fieldName);
        }

        /// <summary>
        /// Returns cursor with field_name,current, and original
        /// </summary>
        /// <returns></returns>
        public List<List<string>> GetOriginalDeltaCursor()
        {
            var changedOriginals = this.GetChangedOriginals();
            List<List<string>> records = new List<List<string>>(changedOriginals.Count);
            foreach (var entry in changedOriginals)
            {
                // field_name, current, originalValue
                var rec = new List<string>() { entry.Key, this[entry.Key], entry.Value };
                records.Add(rec);
            }
            return records;
        }

        /// <summary>
        /// True if there are changes from what was originally there
        /// </summary>
        public bool HasOriginalChanges
        {
            get
            {
                return this.GetChangedOriginals().Count > 0;
            }
        }

        /// <summary>
        /// Packs the originals into a formatted comma delimited string
        /// </summary>
        /// <returns></returns>
        public string GetPackedOriginals()
        {
            // HACK!WE CANNOT STORE LAST MODIFIED DATES IN PACKED ORIGINALS BECAUSE WE DO NOT WANT TO 
            // ROLL THEM BACK TO THEIR ORIGINAL VALUES!!! SO DO NOT PUT THEM IN PACKED ORIGINALS.
            string[] originalsFormatFields = this.GetChangedOriginals().Select(x=>x.Key).Where(f => f.NotIn("last_modified", "last_modified_pv", "last_modified_au")).ToArray();
            Array.Sort(originalsFormatFields);
            string originalsFormatString = originalsFormatFields.JoinStrings(",");
            // packing no changed fields is simply ''
            if (originalsFormatString.Equals("")) return "";
            int formatId = this.ObjectCache.mvm.mvmCluster.GetFormatId(originalsFormatString);
            StringBuilder sb = new StringBuilder();
            sb.Append(formatId);
            foreach (string f in originalsFormatFields)
            {
                sb.Append(",");
                sb.Append(this.GetOriginalValue(f));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Pack original values onto the passed field name
        /// </summary>
        /// <param name="packedFieldName"></param>
        public void PackOriginals(string packedFieldName)
        {
            string packed = this.GetPackedOriginals();
            this[packedFieldName] = packed;
        }

        /// <summary>
        /// Unpacks the original values from the packed field name into the originals hash
        /// </summary>
        /// <param name="packedFieldName"></param>
        public void UnpackOriginals(string packedFieldName)
        {
            string packed = this[packedFieldName];
            // unpacking null does nothing
            if (packed.Equals("")) return;
            string[] fields = packed.Split(',');
            string formatIdStr = fields[0];
            int formatId = formatIdStr.ToInt();
            string[] fieldNames = this.ObjectCache.mvm.mvmCluster.GetFormatFields(formatId);
            int fieldNo = 1;
            this.deltaState = DeltaState.SettingOriginals;
            foreach (string fieldName in fieldNames)
            {
                string fieldValue = fields[fieldNo++];
                //this.SetField(fieldName, fieldValue);
                this[fieldName]= fieldValue;
            }
            this.deltaState = DeltaState.SettingNew;
        }

        # endregion

        #region persisted values

        /// <summary>
        /// Returns the persisted value
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string GetPersistedValue(string fieldName)
        {
            return this.GetPersisted(fieldName);
        }

        
        /// <summary>
        /// Returns cursor with field_name,current, and persisted
        /// </summary>
        /// <returns></returns>
        public List<List<string>> GetPersistedDeltaCursor()
        {
            var changedPersisted = this.GetChangedPersisted();
            List<List<string>> records = new List<List<string>>(changedPersisted.Count);
            foreach (var entry in this.GetChangedPersisted())
            {
                var rec = new List<string>() { entry.Key, this[entry.Key], entry.Value };
                records.Add(rec);
            }
            return records;
        }

        /// <summary>
        /// True if there are changes from what is persisted
        /// </summary>
        public bool HasPersistedChanges
        {
            get
            {
                return this.GetChangedPersisted().Count > 0;
            }
        }

        #endregion
    }
}
