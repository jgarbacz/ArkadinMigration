using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{

    /// <summary>
    /// Decimal wrapper class tuned for key/value IO.
    /// </summary>
    public class Number : IBinarySerializableComparable<Number>, IDynamicSerializable, IComparable<object>,IDynamicParser
    {
        public Number()
        {
        }

        public Number(decimal s)
        {
            this.Value = s;
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        public decimal Value
        {
            get;
            set;
        }

        #region BinarySerializableComparable<Number> Members

        public int CompareTo(byte[] buf1, int offset1, int len1, byte[] buf2, int offset2, int len2)
        {
            // TBD: tune this to do comparisons in binary. Need a better understanding of how
            // decimals are represented in binary to do this. For now just instanciate the
            // objects and compare.
            decimal d1 = new BinaryReader(new MemoryStream(buf1, offset1, len1)).ReadDecimal();
            decimal d2 = new BinaryReader(new MemoryStream(buf2, offset2, len2)).ReadDecimal();
            return d1.CompareTo(d2);
        }

        #endregion

        #region SerializableComparable<Number> Members

        public void Serialize(BinaryWriter bwriter)
        {
            bwriter.Write(this.Value);
        }

        public void Deserialize(BinaryReader breader)
        {
            this.Value = breader.ReadDecimal();
        }

        #endregion

        #region IComparable<Number> Members

        public int CompareTo(Number other)
        {
            return this.Value.CompareTo(other.Value);
        }
        public int CompareTo(object other)
        {
            Number otherNumber = other as Number;
            return this.Value.CompareTo(otherNumber.Value);
        }

        #endregion

        #region BinarySerializableComparable<Number> Members


        private static IComparer<RawValue<Number>> myRawComparer = new DefaultRawComparer<Number>();
        public IComparer<RawValue<Number>> GetRawComparer()
        {
            return myRawComparer;
        }

        #endregion

        #region IDynamicParser Members

        public object Parse(string input)
        {
           decimal d=decimal.Parse(input);
           return new Number(d);
        }

        #endregion
    }

}
