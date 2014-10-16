using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{

    /// <summary>
    /// This is the binary equivelent of a value
    /// </summary>
    public struct RawValue<T>
    {
        public byte[] buffer;
        public int offset;
        public int length;

        public RawValue(byte[] buffer,int offset,int length){
            this.buffer = buffer;
            this.offset = offset;
            this.length = length;
        }

        public string AsciiValue
        {
            get
            {
                return System.Text.Encoding.ASCII.GetString(buffer, offset, length);
            }
        }

        public string HexValue
        {
            get
            {
                return BitConverter.ToString(buffer, offset, length);
            }
        }
    }
}
