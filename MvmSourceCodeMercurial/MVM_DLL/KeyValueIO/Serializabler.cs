using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{

    public class Serializabler<T>:ISerializer<T>
        where T : ISerializable<T>,new()
    {
        private Serializabler() { }
        public static Serializabler<T> Default = new Serializabler<T>();

        public void Serialize(T input, BinaryWriter bwriter)
        {
            input.Serialize(bwriter);
        }

        public T Deserialize(BinaryReader breader)
        {
            T output=new T();
            output.Deserialize(breader);
            return output;
        }
    }
}
