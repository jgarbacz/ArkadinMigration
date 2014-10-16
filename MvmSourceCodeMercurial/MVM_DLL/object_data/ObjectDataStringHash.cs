using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using NLog;

namespace MVM
{
    public class ObjectDataStringHash : ObjectDataBase, IObjectData
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

# region static members
        /// <summary>
        /// Returns a readnull object
        /// </summary>
        /// <param name="objectCache"></param>
        /// <returns></returns>
        public static ObjectDataStringHash GetReadNull(ObjectCache objectCache)
        {
            ObjectDataStringHash readNull = new ObjectDataStringHash(objectCache,"N","READ_NULL");
            return readNull;
        }
        /// <summary>
        /// Returns a write null object
        /// </summary>
        /// <param name="objectCache"></param>
        /// <returns></returns>
        public static ObjectDataStringHash GetWriteNull(ObjectCache objectCache)
        {
            ObjectDataStringHash writeNull = new ObjectDataStringHash(objectCache,"n","WRITE_NULL");
            return writeNull;
        }

        /// <summary>
        /// To create a thread object
        /// </summary>
        /// <returns></returns>
        public static ObjectDataStringHash GetThreadObjectData(ObjectCache objectCache)
        {
            ObjectDataStringHash threadObjectData = new ObjectDataStringHash(objectCache,"THREAD","THREAD");
            return threadObjectData;
        }

        /// <summary>
        /// To create a global object
        /// </summary>
        /// <returns></returns>
        public static ObjectDataStringHash GetGlobalObjectData(ObjectCache objectCache)
        {
            ObjectDataStringHash globalObjectData = new ObjectDataStringHash(objectCache, "GLOBAL", "GLOBAL");
            return globalObjectData;
        }

        ///// <summary>
        ///// Serialized a new object with only 1 field, object_type.
        ///// </summary>
        ///// <param name="bwriter"></param>
        ///// <param name="objectType"></param>
        //public static void SerializeNewObject(BinaryWriter bwriter, string objectType)
        //{
        //    bwriter.Write7BitEncodedInt(1);
        //    bwriter.Write("object_type");
        //    bwriter.Write(objectType);
        //}

        
#endregion

#region instance members


        private readonly Dictionary<string, string> _fields = new Dictionary<string, string>();

        public override string objectId
        {
            get
            {
                return this._fields["object_id"];
            }
            set
            {
                this["object_id"] = value;
            }
        }

        /// <summary>
        /// To create an object in a cluster
        /// </summary>
        /// 
        /// <param name="objectCache"></param>
        /// <param name="objectId"></param>
        /// <param name="objectType"></param>
        public ObjectDataStringHash(ObjectCache objectCache, string objectId, string objectType)
            : base(objectCache, objectId, objectType)
        {
        }

        public ObjectDataStringHash(ObjectCache objectCache): base(objectCache) { }


        /// <summary>
        /// Inherits all the fields from source onto target
        /// </summary>
        /// <param name="sourceObjectId"></param>
        public override void InheritAll(string sourceObjectId)
        {
            using (IObjectData srcObj = this.ObjectCache.CheckOut(sourceObjectId))
            {
                this._fields.AddAll(srcObj.FieldKeyValuesPairs);
                this._fields["object_id"] = this.objectId;
            }
        }


        public override IObjectData CloneObjectData()
        {
            return this.CloneMe() as IObjectData;
        }

        /// <summary>
        /// With the exception of object_id, returns an exact deep copy of this object.
        /// </summary>
        /// <returns></returns>
          public ObjectDataStringHash CloneMe()
          {
              ObjectDataStringHash clone = this.ObjectCache.CreateAndGetObject(this.objectType);

              // ** bad cloning results in really hard bugs to track down so be
              // ** very careful to deep clone where needed.

              // strings are immutable so this is safe
              string assignedOid = clone.objectId;
              clone._fields.AddAll(this._fields);
              clone.objectId = assignedOid;
              return clone;
          }

        /// <summary>
        /// Clears all the field except the object_id, and object_type
        /// </summary>
        public override void Clear()
        {
            lock (this)
            {
                string objectId = this.objectId;
                string objectType = this.objectType;
                this._fields.Clear();
                this["object_id"] = objectId;
                this["object_type"] = objectType;
            }
        }
        
        /// <summary>
        ///  get or set an object field by field name
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public override string this[string fieldName]
        {
            get
            {
                lock (this)
                {
                    string output;
                    if (this._fields.TryGetValue(fieldName, out output)) return output;
                    return "";
                }
            }
            set
            {
                lock (this)
                {
                    this._fields[fieldName] = value;
                }
            }
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
                lock (this)
                {
                    string fieldName = this.mvm.mvmCluster.GetFieldName(ufn);
                    return this[fieldName];
                }
            }
            set
            {
                lock (this)
                {
                    string fieldName = this.mvm.mvmCluster.GetFieldName(ufn);
                    this[fieldName] = value;
                }
            }
        }


        protected string formatString;
        protected int formatId;
        protected int formatIdFieldCount;
        protected string[] formatFields;
        public int GetFormatId()
        {
            if (formatId == FormatId.PAIRS) return this.formatId;

            // if format id is valid, and field cound hasn't changed as assume it is still valid
            if (formatId > FormatId.NULL && this.FieldCount == this.formatIdFieldCount) return this.formatId;

            // otherwise get the format id by talking to the super node.
            this.formatFields = this.FieldNames.OrderBy(x => x).ToArray();
            this.formatString = this.formatFields.Join(",");
            // get the format id and snap the field count.
            this.formatId = this.ObjectCache.mvm.mvmCluster.GetFormatId(this.formatString);
            this.formatIdFieldCount = this.FieldCount;
            return this.formatId;
        }

        public static int classId = 1;
        public override int ClassId { get { return classId; } }

        /// <summary>
        /// Writes out format id followed by field values.
        /// </summary>
        /// <param name="bwriter"></param>
        protected override void SerializeSpecific(BinaryWriter bwriter)
        {
            // get the format id
            int formatId = this.GetFormatId();
            // write the formatted object
            //logger.Info("[SerializeSpecific]write object with format_id=[{0}]", formatId);
            bwriter.Write7BitEncodedInt(formatId);
            foreach (string f in this.formatFields)
            {
                string fieldValue = this._fields[f];
                //logger.Info("[SerializeSpecific]write field at {2}: {0}={1}", f, fieldValue, (bwriter.BaseStream as QueueBufferedFileStream).fileBuffer.WritePosition);
                bwriter.Write(fieldValue);
            }
            //logger.Info("[SerializeSpecific]serialize: " + this.objectId + " with refCnt=" + this.RefCount);
        }


        /// <summary>
        /// Deserializes an object returning true if it got one else false if EOF.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="targetObj"></param>
        public override bool DeserializeSpecific(BinaryReader BinaryReader, MvmClusterBase mvmCluster, out IObjectData obj)
        {
            obj = null;
            // read the format id trapping eof.
            int formatId;
            try
            {
                //logger.Info("read format_id at {0}",(this.BinaryReader.BaseStream as QueueBufferedFileStream).fileBuffer.ReadPosition);
                formatId = BinaryReader.Read7BitEncodedInt();
                //logger.Info("[DeserializeSpecific]write format_id=[{0}]", formatId);
            
                // Create an object of the appropriate type.
                ObjectDataStringHash objData = new ObjectDataStringHash(mvmCluster.mvm.objectCache);
                obj = objData;

                // Ask cluster for the format fields for the id
                //logger.Info("lookup format for {0}", formatId);
                string[] formatFields = mvmCluster.GetFormatFields(formatId);

                // read each of the fields into the passed object.
                foreach (string fieldName in formatFields)
                {
                    string fieldValue = BinaryReader.ReadString();
                    //logger.Info("read at {2}: {0}={1}", fieldName, fieldValue, (this.BinaryReader.BaseStream as QueueBufferedFileStream).fileBuffer.ReadPosition);
                    obj[fieldName] = fieldValue;
                }
                objData._fields["object_id"] = obj.objectId;
                return true;
            }
            catch (EndOfStreamException e)
            {
                string msg = "Error partial object read on deserialize for type " + this.GetType().FullName;
                logger.Fatal(msg);
                throw new Exception(msg, e);
            }

           
        }


#endregion


        public override IEnumerable<string> FieldNames
        {
            get { return this._fields.Keys; }
        }

        public override bool ContainsField(string fieldName)
        {
            return this._fields.ContainsKey(fieldName) ;
        }

        public override bool RemoveObjectField(string fieldName)
        {
            return this._fields.Remove(fieldName);
        }

        public override IEnumerable<KeyValuePair<string, string>> FieldKeyValuesPairs
        {
            get { return this._fields; }
        }

        public override int FieldCount
        {
            get { return this._fields.Count; }
        }
    }
}
