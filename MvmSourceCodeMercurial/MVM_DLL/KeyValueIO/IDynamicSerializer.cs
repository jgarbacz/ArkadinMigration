using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{
    public interface IDynamicSerializer
    {
        /// <summary>
        /// Serializes the input object contents to the binary stream
        /// </summary>
        /// <param name="bwriter"></param>
        void Serialize(object input, BinaryWriter bwriter);
        /// <summary>
        /// Instanciates an object of type T, sets its internal properties by 
        /// reading them from the binary stream and returns the new object.
        /// </summary>
        /// <param name="breader"></param>
        object Deserialize(BinaryReader breader);
    }
}
