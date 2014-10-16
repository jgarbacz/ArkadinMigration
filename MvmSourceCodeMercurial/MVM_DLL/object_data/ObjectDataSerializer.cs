using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{
    /// <summary>
    /// This class allows IObjectData to be a value in KeyValueIO.
    /// </summary>
    public class ObjectDataSerializer : ISerializer<IObjectData>
    {
        public readonly MvmEngine mvm;
        public MvmClusterBase mvmCluster {
            get{
                return this.mvm.mvmCluster;
            }
        }
        public ObjectDataSerializer(MvmEngine mvm)
        {
            this.mvm = mvm;
        }

        #region ISerializer<IObjectData> Members

        public void Serialize(IObjectData input, BinaryWriter bwriter)
        {
            input.Serialize(bwriter);
        }

        public IObjectData Deserialize(BinaryReader breader)
        {
            IObjectData outputObject;
            ObjectDataBase.Deserialize(breader,this.mvmCluster, out outputObject);
            return outputObject;
        }

        #endregion
    }
}
