using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISerializer<T> 
    {
        /// <summary>
        /// Serializes the input object contents to the binary stream
        /// </summary>
        /// <param name="bwriter"></param>
        void Serialize(T input, BinaryWriter bwriter);
        /// <summary>
        /// Instanciates an object of type T, sets its internal properties by 
        /// reading them from the binary stream and return the new object.
        /// </summary>
        /// <param name="breader"></param>
        T Deserialize(BinaryReader breader);
    }
    
   

   

}
