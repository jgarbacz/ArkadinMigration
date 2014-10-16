using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{
    /// <summary>
    /// When passed a ISerializable, this generates a 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DynamicSerializer<T> : IDynamicSerializer
       where T : class,ISerializable<T>, new()
    {
        public static DynamicSerializer<T> Default = new DynamicSerializer<T>();
        private DynamicSerializer() { }
        public void Serialize(object input, BinaryWriter bwriter)
        {
            (input as T).Serialize(bwriter);
        }

        object IDynamicSerializer.Deserialize(BinaryReader breader)
        {
            T t = new T();
            t.Deserialize(breader);
            return t;
        }
    }
}
