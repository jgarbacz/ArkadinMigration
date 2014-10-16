using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{

    /// <summary>
    /// String wrapper class tuned for key/value IO.
    /// </summary>
    public class Text : IBinarySerializableComparable<Text>, IDynamicSerializable, IComparable<object>,IDynamicParser
    {
        public static readonly IDynamicParser DynamicParser=new Text();

        public Text()
        {
        }

        public Text(string s)
        {
            this.Value = s;
        }

        public override string ToString()
        {
            return this.Value;
        }

        public string Value
        {
            get;
            set;
        }

        #region BinarySerializableComparable<Text> Members

        public int CompareTo(byte[] buf1, int offset1, int len1, byte[] buf2, int offset2, int len)
        {
            int length1 = buf1.Read7BitEncodedInt(ref offset1);
            int length2 = buf2.Read7BitEncodedInt(ref offset2);
            return MyBinaryReaderWriter.ByteArrayCompareTo(buf1, offset1, length1, buf2, offset2, length2);
        }

        #endregion

        #region SerializableComparable<Text> Members

        public void Serialize(BinaryWriter bwriter)
        {
            bwriter.Write(this.Value);
        }

        public void Deserialize(BinaryReader breader)
        {
            this.Value = breader.ReadString();
        }

        #endregion

        #region IComparable<Text> Members

        public int CompareTo(Text other)
        {
            return this.Value.CompareToValid(other.Value);
        }

        public int CompareTo(object other)
        {
            Text otherText = other as Text;
            return this.Value.CompareToValid(otherText.Value);
        }

        #endregion


        #region BinarySerializableComparable<Text> Members


        private static IComparer<RawValue<Text>> myRawComparer = new DefaultRawComparer<Text>();
        public IComparer<RawValue<Text>> GetRawComparer()
        {
            return myRawComparer;
        }

        #endregion


        #region IDynamicParser Members

        public object Parse(string input)
        {
            return new Text(input);
        }

        #endregion
    }

}
