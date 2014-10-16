using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{

    /// <summary>
    /// Serializable and preserves type information
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISerializable<T>:IDynamicSerializable
    {
    }

    /// <summary>
    /// Serializable without type information
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDynamicSerializable
    {
        /// <summary>
        /// Serializes the object contents to the binary stream
        /// </summary>
        /// <param name="bwriter"></param>
        void Serialize(BinaryWriter bwriter);
        /// <summary>
        /// Sets internal properties of the object by reading them from the binary stream
        /// </summary>
        /// <param name="breader"></param>
        void Deserialize(BinaryReader breader);
    }
}
