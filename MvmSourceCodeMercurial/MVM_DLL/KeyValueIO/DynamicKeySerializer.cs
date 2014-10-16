using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{

    
    /// <summary>
    /// Serialization factory for dynamic keys
    /// </summary>
    public class DynamicKeySerializer : ISerializer<DynamicKey>
    {
        public readonly IDynamicSerializer[] serializers;
        MemoryStream keyMs = new MemoryStream();
        BinaryWriter keyBw;
        public DynamicKeySerializer(params IDynamicSerializer[] serializers)
        {
            this.serializers = serializers;
            this.keyBw = new BinaryWriter(this.keyMs);
        }
        public void Serialize(DynamicKey input, BinaryWriter bwriter)
        {
            for (int i = 0; i < this.serializers.Length; i++)
            {
                IDynamicSerializer serializer = serializers[i];
                object obj = input.objects[i];
                // serialize the object to a temp key buffer
                this.keyMs.Position = 0;
                serializer.Serialize(obj, this.keyBw);
                int keyLen=(int)this.keyMs.Position;
                // Write packed key len
                bwriter.Write7BitEncodedInt(keyLen);
                // Write binary key
                bwriter.Write(this.keyMs.ToArray(), 0, keyLen); // TBD: there is prob a faster way then making new array
            }
        }
        public DynamicKey Deserialize(BinaryReader breader)
        {
            object[] objects = new IDynamicSerializable[this.serializers.Length];
            for (int i = 0; i < this.serializers.Length; i++)
            {
                IDynamicSerializer serializer = serializers[i];
                // read the key length.
                int keyLen = breader.Read7BitEncodedInt();
                object obj = serializer.Deserialize(breader);
                objects[i] = obj;
            }
            return new DynamicKey(objects);
        }
    }
}
