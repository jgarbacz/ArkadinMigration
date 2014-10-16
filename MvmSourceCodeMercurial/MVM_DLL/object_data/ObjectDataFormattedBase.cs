using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using NLog;

namespace MVM
{
    public abstract class ObjectDataFormattedBase : ObjectDataBase, IObjectData
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static readonly int FLDIDX_OBJECT_ID = 0;
        public static readonly int FLDIDX_OBJECT_TYPE = 1;

        // start out with 2 format fields (tfid=0) object_id, object_type in positions 0 and 1 
        protected string[] formattedFields = new string[2];
        protected int tfid = 0;
        protected int[] ufnIdxMap = new int[] { FLDIDX_OBJECT_ID, FLDIDX_OBJECT_TYPE };
        protected int feedbackId = -1;


        public override string objectId
        {
            get
            {
                return this.formattedFields[0];
            }
            set
            {
                this[0] = value;
            }
        }
        public string feedbackName
        {
            get
            {
                if (this.feedbackId < 0) return null;
                return this.mvmCluster.GetFeedbackName(this.feedbackId);
            }
        }

        public ObjectDataFormattedBase(ObjectCache objectCache, string objectId, string objectType,string feedbackName)
            : base(objectCache, objectId, objectType)
        {
            if (feedbackName.NotNullOrEmpty())
            {
                feedbackId = this.mvmCluster.GetFeedbackId(feedbackName);
            }
        }

        public ObjectDataFormattedBase(ObjectCache objectCache)
            : base(objectCache)
        {
        }

        /// <summary>
        /// Inherits all the fields from source onto target
        /// </summary>
        /// <param name="sourceObjectId"></param>
        public override void InheritAll(string sourceObjectId)
        {
            using (IObjectData srcObj = this.ObjectCache.CheckOut(sourceObjectId))
            {
                foreach (var kv in srcObj.FieldKeyValuesPairs)
                {
                    this[kv.Key] = kv.Value;
                }
                this["object_id"] = this.objectId;
            }
        }

        public void CloneMeOnto(ObjectDataFormattedBase clone)
        {
            string assignedOid = clone.objectId;
            clone.formattedFields = new string[this.formattedFields.Length];
            clone.formattedFields[0] = assignedOid;
            Array.Copy(this.formattedFields, 1, clone.formattedFields, 1, this.formattedFields.Length - 1);
            clone.tfid = this.tfid;
            clone.ufnIdxMap = this.ufnIdxMap;
            clone.feedbackId = this.feedbackId;
        }

        
        protected void ClearFormattedFields() {
            lock (this)
            {
                // object_id=0, object_type=1
                for (int i = 2; i < this.formattedFields.Length; i++)
                {
                    this.formattedFields[i] = "";
                }
            }
        }

        /// <summary>
        ///  get or set an object field by name
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public override string this[string fieldName]
        {
            get
            {
                int ufn = this.mvmCluster.GetUfn(fieldName);
                return this[ufn];
            }
            set
            {
                int ufn = this.mvmCluster.GetUfn(fieldName);
                this[ufn] = value;
            }
        }

        protected void SetFormattedField(int ufn, string val)
        {
            lock (this)
            {
                int nextTfid;
                int formatFieldIdx = this.GetFormatFieldIdx(ufn, out nextTfid);
                if (nextTfid != this.tfid)
                {
                    this.ConvertFormat(nextTfid);
                }
                //logger.Info("WRITE idx="+formatFieldIdx+"="+val);
                this.formattedFields[formatFieldIdx] = val;
            }
        }

        protected void SetFormattedField(string fieldName, string val)
        {
            int ufn = this.mvmCluster.GetUfn(fieldName);
            this.SetFormattedField(ufn, val);
        }

        protected string GetFormattedField(int ufn)
        {
            lock (this)
            {
                int formatFieldIdx = this.GetFormatFieldIdx(ufn);
                if (formatFieldIdx < 0) return "";
                return this.formattedFields[formatFieldIdx];
            }
        }

        
        /// <summary>
        /// returns the field index or -1 if not found
        /// </summary>
        /// <param name="ufn"></param>
        /// <returns></returns>
        public int GetFormatFieldIdx(int ufn)
        {
            int formatFieldIdx;
            if (ufn < ufnIdxMap.Length)
            {
                formatFieldIdx = ufnIdxMap[ufn];
                if (formatFieldIdx >= 0)
                {
                    // field exists in format
                    return formatFieldIdx;
                }
            }
            return -1;
        }

        /// <summary>
        /// Returns the field index and sets nextTfid if field not found in current TFID.
        /// </summary>
        /// <param name="ufn"></param>
        /// <param name="nextTfid"></param>
        /// <returns></returns>
        public int GetFormatFieldIdx(int ufn, out int nextTfid)
        {
            // first try the local struct
            int formatFieldIdx;
            if (ufn < ufnIdxMap.Length)
            {
                formatFieldIdx = ufnIdxMap[ufn];
                if (formatFieldIdx >= 0)
                {
                    nextTfid = tfid;
                    //logger.Info("RETURN LOCAL: ufn=["+ufn+"] idx=" + formatFieldIdx);
                    return formatFieldIdx;
                }
            }
            // this is going to expand the format
            formatFieldIdx=this.mvmCluster.GetFormatFieldIdx(this.feedbackId,this.tfid, ufn, out nextTfid);
            //logger.Info("RETURN REMOTE:ufn=[" + ufn + "] idx=" + formatFieldIdx);
            return formatFieldIdx;
        }


        // Expands current format to the next Tfid.
        private void ConvertFormat(int nextTfid)
        {
            //logger.Info("CONVERT TFID=" + this.tfid + "->" + nextTfid);
            //logger.Info("BEFORE:"+this.ToString());
            int currTfid = this.tfid;
            var plan=this.mvmCluster.GetTfidTransitionPlan(this.tfid, nextTfid);
            this.formattedFields = this.formattedFields.ArrayCopyWithPlan(plan, "");
            this.SetTfid(nextTfid);
            //logger.Info("AFTER:" + this.ToString());
        }

        private void SetTfid(int nextTfid)
        {
            this.tfid = nextTfid;
            this.ufnIdxMap = this.mvmCluster.GetUfnIdxMap(this.tfid);
        }

        protected void SerializeFormattedFields(BinaryWriter bwriter)
        {
            bwriter.Write7BitEncodedInt(this.tfid);
            bwriter.Write(this.feedbackId);
            bwriter.Write(this.formattedFields);
        }

        
       protected bool DeserializeSpecific(BinaryReader BinaryReader, MvmClusterBase mvmCluster, ObjectDataFormattedBase objData)
       {
            try
            {
                //logger.Info("read format_id at {0}",(this.BinaryReader.BaseStream as QueueBufferedFileStream).fileBuffer.ReadPosition);
                int myTfid = BinaryReader.Read7BitEncodedInt();
                int myFeedbackId = BinaryReader.ReadInt32();
                string[] myFields = BinaryReader.ReadArrayOfString();
                objData.formattedFields = myFields;
                objData.SetTfid(myTfid);
                objData.feedbackId = myFeedbackId;
                return true;
            }
            catch (EndOfStreamException e)
            {
                string msg = "Error partial object read on deserialize for type " + this.GetType().FullName;
                logger.Fatal(msg);
                throw new Exception(msg, e);
            }
        }

        public override IEnumerable<string> FieldNames
        {
            get { return this.mvmCluster.GetTfidFields(this.tfid); }
        }

        public override bool ContainsField(string fieldName)
        {
            return this.mvmCluster.GetFormatFieldIdx(this.tfid,this.mvmCluster.GetUfn(fieldName))>=0;
        }

        public override bool RemoveObjectField(string fieldName)
        {
            throw new NotImplementedException("cannot remove object field on formatted object");
        }

        public override IEnumerable<KeyValuePair<string, string>> FieldKeyValuesPairs
        {
            get {
                string[] fieldNames=this.mvmCluster.GetTfidFields(this.tfid);
                List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>(fieldNames.Length);
                for(int i=0; i<fieldNames.Length;i++)
                {
                    string fieldName=fieldNames[i];
                    string fieldValue=this.formattedFields[i];
                    pairs.Add(new KeyValuePair<string,string>(fieldName,fieldValue));
                }
               return pairs;
            }
        }

        /// <summary>
        /// TBD: tune this... we should statically know TFID->[UFN1, UFN2...]
        /// </summary>
        public IEnumerable<KeyValuePair<int, string>> FieldUfnValuesPairs
        {
            get
            {
                logger.Info("ROB TUNE THIS!");
                string[] fieldNames = this.mvmCluster.GetTfidFields(this.tfid);
                List<KeyValuePair<int, string>> pairs = new List<KeyValuePair<int, string>>(fieldNames.Length);
                for (int i = 0; i < fieldNames.Length; i++)
                {
                    string fieldName = fieldNames[i];
                    int ufn = this.mvmCluster.GetUfn(fieldName);
                    string fieldValue = this.formattedFields[i];
                    pairs.Add(new KeyValuePair<int, string>(ufn, fieldValue));
                }
                return pairs;
            }
        }

        public override int FieldCount
        {
            get { return this.formattedFields.Length; }
        }
    }
}
