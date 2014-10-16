using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{
    /// <summary>
    /// Data class for dynamic keys. Anytime you use a dynamic key, you must use
    /// a DynamicKeyComparer and a DynamicSerializableFactory. Unlike our static
    /// types like <code>Text</code> and <code>Number</code>, a DynamicKey cannot 
    /// be fully self-described by its Type. It is described by instance info.
    /// </summary>
    public class DynamicKey : IBinarySerializableComparable<DynamicKey>
    {
        public override string ToString()
        {
            if (this.objects == null) return "?!NullDynamicKey!?";
            if (this.objects.Length == 0) return "?!ZeroLengthDynamicKey!?";
            return "{" + this.objects.JoinStrings(",") + "}";
        }

        public DynamicKey()
        {
        }

        public object[] objects;
        public DynamicKey(object[] objects)
        {
            this.objects = objects;
        }

        #region BinarySerializableComparable<Number> Members

        public int CompareTo(byte[] buf1, int offset1, int len1, byte[] buf2, int offset2, int len2)
        {
            throw new Exception("Error, DynamicKeys cannot be compared directly. Use a custom comparer.");
        }

        #endregion

        #region SerializableComparable<Number> Members

        public void Serialize(BinaryWriter bwriter)
        {
            throw new Exception("Error, DynamicKeys cannot be serialized directly. Use a custom serializer.");

        }

        public void Deserialize(BinaryReader breader)
        {
            throw new Exception("Error, DynamicKeys cannot be serialized directly. Use a custom serializer.");

        }

        #endregion

        #region IComparable<Number> Members

        public int CompareTo(DynamicKey other)
        {
            throw new Exception("Error, DynamicKeys cannot be compared directly. Use a custom comparer.");
        }

        #endregion

        #region BinarySerializableComparable<DynamicKey> Members

        private static IComparer<RawValue<DynamicKey>> myRawComparer = new DefaultRawComparer<DynamicKey>();
        public IComparer<RawValue<DynamicKey>> GetRawComparer()
        {
            return new DynamicKeyRawComparerShouldNotBeUsed();
        }
    }

        #endregion

    public class DynamicKeyRawComparerShouldNotBeUsed : IComparer<RawValue<DynamicKey>>
    {
        #region IComparer<RawValue<DynamicKey>> Members

        public int Compare(RawValue<DynamicKey> x, RawValue<DynamicKey> y)
        {
            throw new Exception("Error, DynamicKeys require that you pass a separate comparer.");
        }

        #endregion
    }

}
