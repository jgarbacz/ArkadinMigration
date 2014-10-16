using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{
    public interface IObjectData : IDisposable,ISerializable<IObjectData>
    {
        int ClassId { get; }
        
        /// <summary>
        /// Read or write a field by ufn
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        string this[int ufn] { get; set; }

        /// <summary>
        /// Read or write a field by name
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        string this[string fieldName] { get; set; }
        
        /// <summary>
        /// Ability to loop through fields as key value pairs. 
        /// </summary>
        IEnumerable<KeyValuePair<string, string>> FieldKeyValuesPairs { get; }
        
        /// <summary>
        /// Returns number of fields
        /// </summary>
        int FieldCount { get; }

        /// <summary>
        /// Ability to loop through field names
        /// </summary>
        IEnumerable<string> FieldNames { get; }

        /// <summary>
        /// Automically decrements a field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        string Decrement(string fieldName);

        /// <summary>
        /// Automically increments a field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        string Increment(string fieldName);

        /// <summary>
        /// Returns only the external fields a key value pair
        /// </summary>
        /// <returns></returns>
        IEnumerable<KeyValuePair<string, string>> SelectExternalFields();
        
        /// <summary>
        /// Returns true if this object is the special 'null' object.
        /// </summary>
        /// <returns></returns>
        bool IsNullObject();

        /// <summary>
        /// Returns the object id of the current object
        /// </summary>
        string objectId { get; set; }

        /// <summary>
        /// Returns the object type
        /// </summary>
        string objectType { get; set; }
        
        /// <summary>
        /// Returns ref to the object cache
        /// </summary>
        ObjectCache ObjectCache { get;  }
        
        /// <summary>
        /// Serializes the object and returns the bytes.
        /// </summary>
        /// <returns></returns>
        byte[] Serialize();
        
        /// <summary>
        /// Serializes the object to the passed BinaryWriter
        /// </summary>
        /// <param name="bwriter"></param>
        new void Serialize(BinaryWriter bwriter);
        
        /// <summary>
        /// Deserializes an object of a specific type and returns true if there is an object of false if at EndOfStream
        /// </summary>
        /// <param name="BinaryReader"></param>
        /// <param name="mvmCluster"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeserializeSpecific(BinaryReader BinaryReader, MvmClusterBase mvmCluster, out IObjectData obj);

        /// <summary>
        /// Deletes the object from the object cache.
        /// </summary>
        /// <returns></returns>
        bool Delete();
        
        /// <summary>
        /// Clears all the field except the object_id and object_type
        /// </summary>
        void Clear();
        
        /// <summary>
        /// Sets all external fields to ''
        /// </summary>
        /// <param name="exclusions"></param>
        void NullFields(IDictionary<string, bool> exclusions);
        
        /// <summary>
        /// Get or Set the cursor the object is linked to
        /// </summary>
        string CursorInstId { get; set; }

        /// <summary>
        /// Copies the field values from the source object onto the current object.
        /// </summary>
        /// <param name="sourceObjectId"></param>
        void InheritFieldValues(string sourceObjectId);

        /// <summary>
        /// Returns number of references to this object
        /// </summary>
        int RefCount { get; set; }

        /// <summary>
        /// Returns the object id of this object which is a reference to this object.
        /// </summary>
        /// <returns></returns>
        string RefGet();

        /// <summary>
        /// Indicates that someone has released a reference to this object and returns the RefCount remaining.
        /// </summary>
        /// <returns></returns>
        int RefRemove();

        /// <summary>
        /// Remove a reference without having it fire the delete on zero. 
        /// </summary>
        /// <returns></returns>
       int  UnsafeRefRemoveNoDelete();

        /// <summary>
        /// Create and return a full and exact copy of this object but with a new object_id
        /// </summary>
        /// <returns></returns>
       IObjectData CloneObjectData();

       bool ContainsField(string fieldName);

        


       bool RemoveObjectField(string fieldName);
    }
}
