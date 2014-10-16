using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using NLog;

namespace MVM
{
    public class ObjectDataFormatted : ObjectDataFormattedBase, IObjectData
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();



        public ObjectDataFormatted(ObjectCache objectCache, string objectId, string objectType, string feedbackName)
            : base(objectCache, objectId, objectType, feedbackName)
        {
        }

        public ObjectDataFormatted(ObjectCache objectCache)
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
        public ObjectDataFormatted CloneMe()
        {
            ObjectDataFormatted clone = this.objectCache.CreateAndGetObjectDataFormatted(this.objectType, this.feedbackName);
            this.CloneMeOnto(clone);
            return clone;
        }

        /// <summary>
        /// Clears all the field except the object_id, and object_type
        /// </summary>
        public override void Clear()
        {
            this.ClearFormattedFields();
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
                this.SetFormattedField(ufn, value);
            }
        }


        public static int classId = 3;
        public override int ClassId { get { return classId; } }

        /// <summary>
        /// Writes out format id followed by field values.
        /// </summary>
        /// <param name="bwriter"></param>
        protected override void SerializeSpecific(BinaryWriter bwriter)
        {
            this.SerializeFormattedFields(bwriter);
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
                ObjectDataFormatted objData = new ObjectDataFormatted(mvmCluster.mvm.objectCache);
                obj = objData;
                return objData.DeserializeSpecific(BinaryReader, mvmCluster, objData);
            }
            catch (EndOfStreamException e)
            {
                string msg = "Error partial object read on deserialize for type " + this.GetType().FullName;
                logger.Fatal(msg);
                throw new Exception(msg, e);
            }
        }

    }
}
