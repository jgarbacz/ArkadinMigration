using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using NLog;

namespace MVM
{
    public abstract class ObjectDataBase : IObjectData, ISerializable<IObjectData>,ICloneable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static readonly int UFN_OBJECT_ID = 0;
        public static readonly int UFN_OBJECT_TYPE = 1;

        #region ShortCuts

        public MvmEngine mvm { get { return this.ObjectCache.mvm; } }
        public MvmClusterBase mvmCluster { get { return this.mvm.mvmCluster; } }
        public SchemaMaster schemaMaster { get { return this.mvm.globalContext.schemaMaster; } }


        public readonly ObjectCache objectCache;
        public ObjectCache ObjectCache
        {
            get {return this.objectCache;}
        }


        public string objectType { 
            get{
                return this["object_type"];
            }
            set
            {
                this["object_type"] = value;
            }
        }

        public abstract string objectId
        {
            get;
            set;
        }

        //public string objectId
        //{
        //    get
        //    {
        //        return this["object_id"];
        //    }
        //    set
        //    {
        //        this["object_id"] = value;
        //    }
        //}
        #endregion

        public abstract IEnumerable<KeyValuePair<string, string>> FieldKeyValuesPairs
        {
            get;
            }

        
        /// <summary>
        /// This is so we can tie a ICursor to a IObject
        /// </summary>
        public string CursorInstId{get;set;}

            
        #region Constructors
            
        protected ObjectDataBase(ObjectCache objectCache) {
            this.objectCache = objectCache;
        }

        /// <summary>
        /// To create an object in a cluster
        /// </summary>
        /// 
        /// <param name="objectCache"></param>
        /// <param name="objectId"></param>
        /// <param name="objectType"></param>
        public ObjectDataBase(ObjectCache objectCache, string objectId, string objectType)
        {
            this.objectCache = objectCache;
            this.objectType = objectType;
            this.objectId = objectId;
            //this["object_id"] = this.objectId;
            //this["object_type"] = this.objectType;
            //logger.Debug("cons " + this.objectId + " type=" + this.objectType);
        }
       
        #endregion

        #region Utility methods


        /// <summary>
        /// get or set an object field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public abstract string this[string fieldName] { get; set; }


        /// <summary>
        /// get or set an object by UFN
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public abstract string this[int ufn] { get; set; }

        /// <summary>
        /// Returns an exact copy of this object but with a new object id
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return (object) this.CloneObjectData();
        }

        /// <summary>
        /// Returns an exact copy of this object but with a new object id
        /// </summary>
        /// <returns></returns>
        public abstract IObjectData CloneObjectData();

        /// <summary>
        /// Inherists all the fields from source onto target including history
        /// </summary>
        /// <param name="sourceObjectId"></param>
        public abstract void InheritAll(string sourceObjectId);



        public abstract IEnumerable<string> FieldNames { get; }

        public abstract bool ContainsField(string fieldName);

        public abstract bool RemoveObjectField(string fieldName);

        public abstract int FieldCount
        {
            get;
        }

        /// <summary>
        ///  Inherits all the field values from source to target but not the source field history
        /// </summary>
        /// <param name="sourceObjectId"></param>
        public void InheritFieldValues(string sourceObjectId)
        {
            //logger.Info("CALL INHERIT "+sourceObjectId+"->"+this.objectId);
            string myObjectId = this.objectId;
            using (var srcObj = this.ObjectCache.CheckOut(sourceObjectId))
            {
                foreach (var entry in srcObj.FieldKeyValuesPairs)
                {
                    //logger.Info("SET "+entry.Key+"="+ entry.Value);
                        this[entry.Key] = entry.Value;
                    }
                }
            this.objectId = myObjectId;
            //logger.Info("DONE INHERIT " + sourceObjectId + "->" + this.objectId);
            }

        /// <summary>
        /// Nulls out all fields not in exclusions
        /// </summary>
        /// <param name="exclusions"></param>
        public void NullFields(IDictionary<string, bool> exclusions)
        {
            string myObjectId = this.objectId;
            string myObjectType = this.objectType;
            foreach (var f in this.FieldNames.ToList())
            {
                if(!exclusions.ContainsKey(f))this[f]="";
            }
            this["object_id"] = myObjectId;
            this["object_type"] = myObjectType;
        }

        /// <summary>
        /// Deletes the object from its object cache.
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            this.ObjectCache.RemoveObjectData(this.objectId);
            return false;
        }

        /// <summary>
        /// Returns true if this object is considered a null object.
        /// </summary>
        /// <returns></returns>
        public bool IsNullObject()
        {
            if (this.objectId.In("n", "N")) return true;
            return false;
        }

        /// <summary>
        /// List of fields that are considered internal
        /// </summary>
        private static string[] InternalFields = new string[] { "object_id", "object_type" };

        /// <summary>
        /// Select all fields but object_id and object_type
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, string>> SelectExternalFields()
        {
            return this.FieldKeyValuesPairs.Where(e => e.Key.NotIn(InternalFields));
        }

        /// <summary>
        /// Clears all the field except the object_id and object_type
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Atomically increments a field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string Increment(string fieldName)
        {
            lock (this)
            {
                if (!this.ContainsField(fieldName))
                {
                    this[fieldName] = "1";
                    return "1";
                }
                long before = long.Parse(this[fieldName]);
                long after = before + 1;
                string value = after.ToString();
                this[fieldName] = value;
                return value;
            }
        }
        
        /// <summary>
        /// Atomically decrements a field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string Decrement(string fieldName)
        {
            lock (this)
            {
                if (!this.ContainsField(fieldName))
                {
                    this[fieldName] = "-1";
                    return "-1";
                }
                long before = long.Parse(this[fieldName]);
                long after = before - 1;
                string value = after.ToString();
                this[fieldName] = value;
                return value;
            }
        }

        #endregion
   
        #region Refs and Garbage Collections
        public int RefCount { get;  set; }
        public string RefGet()
        {
            this.RefCount++;
            return this.objectId;
        }

        public int UnsafeRefRemoveNoDelete()
        {
            return --this.RefCount;
        }

        public int RefRemove()
        {
            this.RefCount--;
            if (this.RefCount <= 0)
            {
                if(this.objectType.Equals("READ_NULL")){
                    this.mvm.Log("ref count hit zero delete object of type:" + this.objectType + ",oid=" + this.objectId + ",refctr=" + this.RefCount);

                }
                //this.mvm.Log("ref count hit zero delete object of type:" + this.objectType+",oid="+this.objectId+",refctr="+this.RefCount);
                this.Delete();
            }
            return this.RefCount;
        }

        /// <summary>
        /// Checks the object back into the cache
        /// </summary>
        public void Dispose()
        {
            if (this.objectCache == null)
            {
                logger.Error("how is this object cache null:" + this.ToString());
            }
            this.ObjectCache.CheckIn(this);
        }
       

        #endregion

        #region Object serialize/deserialize

        /// <summary>
        /// Subclasses must provide a classId so we know the type of an object to be serialized.
        /// </summary>
        public abstract int ClassId { get; }

        /// <summary>
        /// Subclasses override this to serialize their data
        /// </summary>
        /// <param name="bwriter"></param>
        /// <param name="existingFormatIds"></param>
        protected abstract void SerializeSpecific(BinaryWriter bwriter);

        public static IObjectData[] DeserializeMap=new IObjectData[5];
        static ObjectDataBase()
        {
            DeserializeMap[1] = new ObjectDataStringHash(null);
            DeserializeMap[2] = null;
            DeserializeMap[3] = new ObjectDataFormatted(null);
            DeserializeMap[4] = new ObjectDataFormattedDelta(null);
            // sanity check
            for (int classId=1;classId<DeserializeMap.Length;classId++)
            {
                IObjectData obj = DeserializeMap[classId];
                if(obj!=null&&obj.ClassId!=classId) throw new Exception("Coding bug: array idx ["+classId+"] does not match class id ["+obj.ClassId+"] for type ["+obj.GetType().FullName+"]");
        }
        }

        /// <summary>
        /// Serialized the fields like we do a hash
        /// </summary>
        /// <returns></returns>
        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw=new BinaryWriter(ms);
            this.Serialize(bw);
            bw.Flush();
            return ms.ToArray();
        }


        /// <summary>
        /// This writes the classId followed by the serialized object
        /// </summary>
        /// <param name="bwriter"></param>
        public void Serialize(BinaryWriter bwriter)
        {
            bwriter.Write7BitEncodedInt(this.ClassId);
            this.SerializeSpecific(bwriter);
            //logger.Info("SERIALIZED oid=" + this.objectId + ", refs=" + this.RefCount);
        }


        /// <summary>
        /// Deserializes an IObjectData from the passed BinaryReader. Returns false if EOF. Does
        /// NOT automatically insert it into the ObjectCache.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="targetObj"></param>
        public static bool Deserialize(BinaryReader BinaryReader, MvmClusterBase mvmCluster, out IObjectData obj)
        {
            obj=null;
            IObjectData deserializerObject;
            try
            {
                //logger.Info("ObjectDataBase.Deserialize");
                int classId = BinaryReader.Read7BitEncodedInt();
                //logger.Info("ObjectDataBase.Deserialize classId=" + classId);

                deserializerObject = DeserializeMap[classId];

                if (deserializerObject == null) {
                    throw new Exception("Error trying to deserialize an object. Read classId=" + classId + ", which is not valid");
                }
}
            catch 
            {
                return false;
            }
            try
            {
                //int refCnt = BinaryReader.Read7BitEncodedInt(); ;
                //logger.Info("ObjectDataBase.Deserialize type=" + deserializerObject.GetType().FullName);

                deserializerObject.DeserializeSpecific(BinaryReader,mvmCluster,out obj);
                //obj.RefCount = refCnt;
                //logger.Info("DESERIALIZED oid=" + obj.objectId + ", refs=" + obj.RefCount);
                return true;
            }
            catch (EndOfStreamException e)
            {
                string msg = "Error partial object read on deserialize";
                logger.Fatal(msg);
                throw new Exception(msg, e);
            }
        }
        public abstract bool DeserializeSpecific(BinaryReader BinaryReader, MvmClusterBase mvmCluster, out IObjectData obj);
       
        #endregion

        #region Debugging
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.objectId+"/"+this.objectType+", class="+this.GetType().Name+",refs="+this.RefCount+"fields="+this.FieldKeyValuesPairs.Select(kv=>kv.Key+"="+kv.Value).JoinStrings(","));
            return sb.ToString();
        }
        #endregion

        #region Mvm exported functions

       
        /// <summary>
        /// Print the contents of an object
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="object_id"></param>
        /// <returns></returns>
        /// <category>Object Access</category>
        [MvmExport(@"print_object")]
        public static void print_object(ModuleContext mc, string object_id){
            using (IObjectData obj = mc.objectCache.CheckOut(object_id))
            {
                mc.mvm.Info(
                    dump_object(mc,object_id)
                    );
            }
        }
        /// <summary>
        /// Returns the contents of an object 
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="object_id"></param>
        /// <returns></returns>
        /// <category>Object Access</category>
        [MvmExport(@"dump_object")]
        public static string dump_object(ModuleContext mc, string object_id)
        {
            object outObj;
            if (mc.globalContext.namedClassInstMap.TryGetValue("DUMP_OBJECT_MASKING", out outObj))
            {
                MemoryIndexSync index=outObj as MemoryIndexSync;
            }

            StringBuilder sb = new StringBuilder();
            using (IObjectData obj = mc.objectCache.CheckOut(object_id))
            {
                sb.AppendLine("OBJECT: " + obj.objectId + "/" + obj.objectType+" (");
                sb.AppendLine("  type=[" + obj.GetType().Name + "]");
                sb.AppendLine("  ref_count=[" + obj.RefCount + "]");
                ObjectDataFormatted fobj = obj as ObjectDataFormatted;
                if (fobj != null)
                {
                    sb.AppendLine("  feedbackName=[" + fobj.feedbackName + "]");
                }
                foreach (var f in obj.FieldKeyValuesPairs)
                {
                    sb.AppendLine("  "+f.Key + "=[" + f.Value + "]");
                }
                sb.Append(")"); 
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns fields like: field1=[value1],field2=[value2]... 
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="object_id"></param>
        /// <returns></returns>
        /// <category>Object Access</category>
        [MvmExport(@"dump_sorted_fields")]
        public static string dump_sorted_fields(ModuleContext mc, string object_id)
        {
            StringBuilder sb = new StringBuilder();
            using (IObjectData obj = mc.objectCache.CheckOut(object_id))
            {
                return obj.FieldKeyValuesPairs.Where(f=>f.Key.NotIn("object_id")).OrderBy(ff=>ff.Key).Select(fff=>fff.Key+"=["+fff.Value+"]").JoinStrings(",");
            }
        }


        /// <summary>
        /// Makes an exact copy of the passed object_id and returns you the new object_id of the clone
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="object_id"></param>
        /// <returns></returns>
        /// <category>Object Access</category>
        [MvmExport(@"clone_object")]
        public static string clone_object(ModuleContext mc, string object_id)
        {
            using (IObjectData obj = mc.objectCache.CheckOut(object_id))
            {
             string spawnedObjectId=obj.CloneObjectData().objectId;
             // this needs to do what spawn does...
             mc.procInst.RegisterSpawnedOid(spawnedObjectId);
             return spawnedObjectId;
            }
        }

        /// <summary>
        /// Returns the ref count for the passed object
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="object_id"></param>
        /// <returns></returns>
        /// <category>Object Access</category>
        [MvmExport(@"object_ref_count")]
        public static string object_ref_count(ModuleContext mc, string object_id)
        {
            using (IObjectData obj = mc.objectCache.CheckOut(object_id))
            {
                return obj.RefCount.ToString();
            }
        }

        #endregion

        #region IDynamicSerializable Members

        void IDynamicSerializable.Serialize(BinaryWriter bwriter)
        {
            throw new NotImplementedException();
        }

        void IDynamicSerializable.Deserialize(BinaryReader breader)
        {
            throw new NotImplementedException();
        }

        #endregion






    }
}
