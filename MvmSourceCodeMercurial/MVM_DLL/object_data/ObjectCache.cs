using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

namespace MVM
{
    public class ObjectCache
    {
        // This is the cache, it is threadContext safe
        public readonly Dictionary<string, IObjectData> objects = new Dictionary<string, IObjectData>();

        // Dummy object that gets returned  for reading
        private readonly IObjectData readNull;

        // Dummy object that gets returns for writing
        private readonly IObjectData writeNull;

        // Sometimes you create an object you don't care what the id of that object is. 
        private int genObjectId = 0;

        public string mvmObjectPrefix
        {
            get
            {
                return this.mvm.mvmObjectPrefix;
            }
        }

        // constructor
        public readonly WorkMgr workMgr;
        public ObjectCache(WorkMgr workMgr)
        {
            this.workMgr = workMgr;
            this.readNull = ObjectDataStringHash.GetReadNull(this);
            this.writeNull = ObjectDataStringHash.GetWriteNull(this);
        }

        // flatten hierarchy
        public MvmEngine mvm
        {
            get
            {
                return this.workMgr.mvm;
            }
        }

        public bool DeleteObject(string objectId)
        {
            lock (this)
            {
                IObjectData obj;
                if (this.objects.TryGetValue(objectId, out obj))
                {
                    obj.Delete();
                    return true;
                }
                throw new Exception("Cannot delete object that does not exists in object cache. invalid object id [" + objectId + "]");
            }
        }

        // Looks up an object by type and id and returns it
        public IObjectData CheckOut(string objectId)
        {
            lock (this)
            {
                IObjectData obj;
                if (this.objects.TryGetValue(objectId, out obj)) return obj;
                return this.readNull;
            }
        }

        // Releases and object
        public void CheckIn(IObjectData objectData)
        {
        }

        // returns the next object id
        private string NextObjectId()
        {
            return mvmObjectPrefix + Interlocked.Increment(ref this.genObjectId);
        }

        // Creates and returns an ObjectData, object does not go in cache.
        public ObjectDataStringHash CreateAndGetObjectOrphan(string objectType)
        {
            string objectId = this.NextObjectId();
            ObjectDataStringHash obj = new ObjectDataStringHash(this, objectId, objectType);
            return obj;
        }

        // Creates and returns an ObjectData, inserts it into the cache.
        public ObjectDataStringHash CreateAndGetObject(string objectType)
        {
            ObjectDataStringHash obj = this.CreateAndGetObjectOrphan(objectType);
            this.InsertObjectData(obj);
            return obj;
        }

        //// Creates and returns an ObjectDataDelta
        //public ObjectDataFormattedDelta CreateAndGetObjectDataDelta(string objectType)
        //{
        //    string objectId = this.NextObjectId();
        //    ObjectDataFormattedDelta obj = new ObjectDataFormattedDelta(this, objectId, objectType);
        //    this.InsertObjectData(obj);
        //    return obj;
        //}

        // Creates and returns an ObjectDataDelta
        public ObjectDataFormatted CreateAndGetObjectDataFormatted(string objectType, string feedbackName)
        {
            string objectId = this.NextObjectId();
            ObjectDataFormatted obj = new ObjectDataFormatted(this, objectId, objectType, feedbackName);
            this.InsertObjectData(obj);
            return obj;
        }

        // Creates and returns an ObjectDataDelta
        public ObjectDataFormattedDelta CreateAndGetObjectDataFormattedDelta(string objectType, string feedbackName)
        {
            string objectId = this.NextObjectId();
            ObjectDataFormattedDelta obj = new ObjectDataFormattedDelta(this, objectId, objectType, feedbackName);
            this.InsertObjectData(obj);
            return obj;
        }


        /// <summary>
        /// Deserializes an object from the BinaryReader and adds it to this objectCache. Returns
        /// false if EOF.
        /// </summary>
        /// <param name="BinaryReader"></param>
        /// <param name="objectData"></param>
        /// <returns></returns>
        public bool DeserializeObject(BinaryReader BinaryReader, out IObjectData objectData)
        {
            if (ObjectDataBase.Deserialize(BinaryReader, this.mvm.mvmCluster, out objectData))
            {
                this.AddOrMergeObject(objectData);
                return true;
            }
            return false;
        }



        /// <summary>
        /// Adds an object to the object cache, merging it with a pre-existing live object.
        /// If an object was written out to disk to save memory, but comes back into memory
        /// we overlay any pre-existing in memory (ie slimmed down) object. Fields set on
        /// the in memory object take precedence over the fields in the passed object.
        /// </summary>
        /// <param name="deserializedObj"></param>
        public void AddOrMergeObject(IObjectData deserializedObj)
        {
            lock (this)
            {
                if (deserializedObj.objectId.IsNullOrEmpty()) deserializedObj.objectId = this.NextObjectId();


                // when adding a deserialized object. If there is already a live
                // object, then assume the live object values take precedence over
                // the serialized values. So simply copy the live over the deserialized
                // and replace the object in the cache.
                IObjectData liveObj;
                if (this.objects.TryGetValue(deserializedObj.objectId, out liveObj))
                {
                    // if the live object and the deserialized object are actually the same, then
                    // there is nothing to do.
                    if (liveObj == deserializedObj)
                    {
                        //this.mvm.Error("AddOrMergeObject with same object so do nothing");
                        return;
                    }

                    deserializedObj.InheritFieldValues(liveObj.objectId);
                    this.objects[deserializedObj.objectId] = deserializedObj;
                    // assume live obj ref count is the correct one.
                    deserializedObj.RefCount = liveObj.RefCount;
                    //this.mvm.Log("[AddOrMergeObject] MERGE OBJECT OID=" + deserializedObj.objectId + ",OT=" + deserializedObj.objectType + ",refcnt=" + deserializedObj.RefCount);
                }
                else
                {
                    this.InsertObjectData(deserializedObj);
                }
            }
        }

        /// <summary>
        /// Adds an object to the object cache, merging it with a pre-existing live object.
        /// If an object was written out to disk to save memory, but comes back into memory
        /// we overlay any pre-existing in memory (ie slimmed down) object. Fields set on
        /// the in memory object take precedence over the fields in the passed object.
        /// </summary>
        /// <param name="deserializedObj"></param>
        public void AddOrOverwriteObject(IObjectData deserializedObj)
        {
            lock (this)
            {
                if (deserializedObj.objectId.IsNullOrEmpty())
                    deserializedObj.objectId = this.NextObjectId();

                IObjectData liveObj;
                if (this.objects.TryGetValue(deserializedObj.objectId, out liveObj))
                {
                    // assume live obj ref count is the correct one.
                    deserializedObj.RefCount = liveObj.RefCount;
                    this.objects[deserializedObj.objectId] = deserializedObj;
                    // this.mvm.Log("[AddOrOverwriteObject] OVERWRITE OBJECT OID=" + deserializedObj.objectId + ",OT=" + deserializedObj.objectType + ",refcnt=" + deserializedObj.RefCount);
                }
                else
                {
                    this.InsertObjectData(deserializedObj);
                }
            }
        }



        // caller must properly dispose of the object
        public IObjectData CreateClusterObject()
        {
            string objectId = this.NextObjectId();
            //this.workMgr.mvm.Log("create_object " + objectId + "," + objectType);
            IObjectData obj = new ObjectDataStringHash(this, objectId, "CLUSTER");
            this.InsertObjectData(obj);
            return obj;
        }

        // create the object and returns the object id
        public string CreateAndGetObjectId(string objectType)
        {
            using (IObjectData obj = CreateAndGetObject(objectType))
            {
                return obj.objectId;
            }
        }

        // safely insert the data into our cache
        public void InsertObjectData(IObjectData objectData)
        {
            lock (this)
            {
                if (this.objects.ContainsKey(objectData.objectId))
                {
                    IObjectData existingObj = this.objects[objectData.objectId];
                    StringBuilder msg = new StringBuilder("Error, duplicate objects with objectId=[" + objectData.objectId + "]");
                    msg.AppendLine("Existing obj:");
                    msg.AppendLine(existingObj.ToString());
                    msg.AppendLine("Adding obj:");
                    msg.AppendLine(objectData.ToString());
                    throw new Exception(msg.ToString());
                }
                //this.mvm.Log("add oid:" + objectData.objectId);
                this.objects[objectData.objectId] = objectData;
            }
        }

        // safely removes the data from our cache
        public IObjectData RemoveObjectData(string objectId)
        {
            try
            {
                lock (this)
                {
                    IObjectData output = this.objects[objectId];
                    if (output == null)
                    {
                        throw new Exception("Error, cannot remove object with oid=" + objectId);
                    }
                    else
                    {
                        //if (objectId.Equals("1:149650"))
                        //{
                        //    this.mvm.Log("rm oid:" + objectId + " trace=[" + System.Environment.StackTrace+"]");
                        //}
                        this.objects.Remove(objectId);
                    }
                    return output;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error, cannot remove object with oid=" + objectId, e);
            }
        }


        public int RefRemove(string objectId)
        {
            try
            {
                lock (this)
                {
                    IObjectData output = this.objects[objectId];
                    if (output == null)
                    {
                        throw new Exception("Error, cannot remove object ref to oid=[" + objectId + "] because object does not exist");
                    }
                    return output.RefRemove();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error, cannot remove object ref to oid=[" + objectId + "]", e);

            }
        }

        // Similar to RefGet(), but just returns the object without incrementing the refctr
        public IObjectData ObjectGet(string objectId)
        {
            try
            {
                lock (this)
                {
                    IObjectData output = this.objects[objectId];
                    if (output == null)
                    {
                        throw new Exception("Error, cannot get object for oid=" + objectId + " because object does not exist");
                    }
                    return output;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error, cannot get object for oid=" + objectId + " because object does not exist", e);
            }
        }

        public string RefGet(string objectId)
        {
            try
            {
                lock (this)
                {
                    IObjectData output = this.objects[objectId];
                    if (output == null)
                    {
                        throw new Exception("Error, cannot get object ref to oid=" + objectId + " because object does not exist");
                    }
                    return output.RefGet();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error, cannot get object ref to oid=" + objectId + " because object does not exist", e);
            }
        }

        // reads an object field through a reference
        public string ReadObjectField(string objectId, string fieldName)
        {
            using (IObjectData obj = this.CheckOut(objectId))
            {
                return obj[fieldName];
            }
        }

        // writes an object field through a reference
        public string WriteObjectField(string objectId, string fieldName, string fieldValue)
        {
            using (IObjectData obj = this.CheckOut(objectId))
            {
                obj[fieldName] = fieldValue;
                return fieldValue;
            }
        }

    }
}
