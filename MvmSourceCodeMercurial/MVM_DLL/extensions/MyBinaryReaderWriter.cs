using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MVM
{
    /// <summary>
    /// Extension methods for serializing and deserializing data.
    /// </summary>
    public static class MyBinaryReaderWriter
    {
        # region Serialize/Deserialize integers with 7 bit encoding

        /// <summary>
        /// Writes an int in a packed format where every 8th bit tells you to keep keep reading.
        /// In the worse case it takes 5 bytes instead of 4 bytes to store the int. In the best case
        /// it takes 1 byte to store the int (when under 128).
        /// </summary>
        /// <param name="binaryWriter"></param>
        /// <param name="value"></param>
        public static void Write7BitEncodedInt(this BinaryWriter binaryWriter, int value)
        {
            // Write out an int 7 bits at a time. The high bit of the byte,
            // when on, tells reader to continue reading more bytes.
            uint v = (uint)value; // support negative numbers
            while (v >= 0x80)
            {
                binaryWriter.Write((byte)(v | 0x80));
                v >>= 7;
            }
            binaryWriter.Write((byte)v);
        }

        /// <summary>
        /// Reads a 7 bit encoded int.
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns></returns>
        public static int Read7BitEncodedInt(this BinaryReader binaryReader)
        {
            // Read out an int 7 bits at a time. The high bit
            // of the byte when on means to continue reading more bytes.
            int count = 0;
            int shift = 0;
            byte b;
            do
            {
                b = binaryReader.ReadByte();
                count |= (b & 0x7F) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);
            return count;
        }

        public static int Read7BitEncodedInt(this byte[] buf, ref int idx)
        {
            // Read out an int 7 bits at a time. The high bit
            // of the byte when on means to continue reading more bytes.
            int count = 0;
            int shift = 0;
            byte b;
            do
            {
                b = buf[idx++];
                count |= (b & 0x7F) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);
            return count;
        }

        public static int ByteArrayCompareTo(
            byte[] buf1, int start1, int len1,
            byte[] buf2, int start2, int len2)
        {
            if (buf1 == buf2 &&
                start1 == start2 &&
                len1 == len2)
            {
                return 0;
            }
            int end1 = start1 + len1;
            int end2 = start2 + len2;
            for (int i = start1, j = start2; i < end1 && j < end2; i++, j++)
            {
                byte b1 = buf1[i];
                byte b2 = buf2[j];
                if (b1 != b2)
                {
                    return b1 - b2;
                }
            }
            return len1 - len2;
        }

        # endregion


        # region Serialize/Deserialize List<string> and string[]
        /// <summary>
        /// Writes the list of string[] to the binaryWriter 
        /// </summary>
        /// <param name="binaryWriter"></param>
        /// <param name="list"></param>
        public static void Write(this BinaryWriter binaryWriter, List<string[]> list)
        {
            binaryWriter.Write7BitEncodedInt(list.Count);
            foreach (var item in list) binaryWriter.Write(item);
        }

        /// <summary>
        /// Writes the list of string to the binaryWriter in the format of:
        /// [itemCount as 7BitEncodedInt]
        /// [item as string]
        /// [item as string]
        /// :
        /// </summary>
        /// <param name="binaryWriter"></param>
        /// <param name="list"></param>
        public static void Write(this BinaryWriter binaryWriter, List<string> list)
        {
            binaryWriter.Write7BitEncodedInt(list.Count);
            foreach (string item in list) binaryWriter.Write(item);
        }
        /// <summary>
        /// Writes the string array to the binaryWriter in the format of:
        /// [length as 7BitEncodedInt]
        /// [item as string]
        /// [item as string]
        /// :
        /// </summary>
        /// <param name="binaryWriter"></param>
        /// <param name="array"></param>
        public static void Write(this BinaryWriter binaryWriter, string[] array)
        {
            if (array == null)
            {
                binaryWriter.Write7BitEncodedInt(0);
                return;
            }
            binaryWriter.Write7BitEncodedInt(array.Length);
            foreach (string item in array) binaryWriter.Write(item);
        }

        /// <summary>
        /// Deserializes list of string from <code>binaryWriter.Write(List<string>)</code>
        /// or <code>binaryWriter.write(string[])</code>
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns></returns>
        public static List<string> ReadListOfString(this BinaryReader binaryReader)
        {
            int count = binaryReader.Read7BitEncodedInt();
            List<string> output = new List<string>(count);
            for (int i = 0; i < count; i++)
                output.Add(binaryReader.ReadString());
            return output;
        }


        /// <summary>
        /// Deserializes list of string[] from <code>binaryWriter.Write(List<string[]>)</code>
        /// or <code>binaryWriter.write(string[][])</code>
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> ReadListOfDictionaryOfStringString(this BinaryReader binaryReader)
        {
            int count = binaryReader.Read7BitEncodedInt();
            List<Dictionary<string, string>> output = new List<Dictionary<string, string>>(count);
            for (int i = 0; i < count; i++)
            {
                var dic = binaryReader.ReadDictionaryOfStringString();
                output.Add(dic);
            }
            return output;
        }

        /// <summary>
        /// Writes List<Dictionary<string, string>>  to the binaryWriter 
        /// </summary>
        /// <param name="binaryWriter"></param>
        /// <param name="list"></param>
        public static void Write(this BinaryWriter binaryWriter, List<Dictionary<string, string>> listDic)
        {
            binaryWriter.Write7BitEncodedInt(listDic.Count);
            foreach (var item in listDic) binaryWriter.Write(item);
        }


        /// <summary>
        /// Deserializes list of string[] from <code>binaryWriter.Write(List<string[]>)</code>
        /// or <code>binaryWriter.write(string[][])</code>
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns></returns>
        public static List<string[]> ReadListOfStringArray(this BinaryReader binaryReader)
        {
            int count = binaryReader.Read7BitEncodedInt();
            List<string[]> output = new List<string[]>(count);
            for (int i = 0; i < count; i++)
            {
                string[] arr = binaryReader.ReadArrayOfString();
                output.Add(arr);
            }
            return output;
        }

        /// <summary>
        /// Deserializes string[] from <code>binaryWriter.Write(List<string>)</code>
        /// or <code>binaryWriter.write(string[])</code>
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns></returns>
        public static string[] ReadArrayOfString(this BinaryReader binaryReader)
        {
            int count = binaryReader.Read7BitEncodedInt();
            if (count == 0)
                return new string[] { };
            string[] output = new string[count];
            for (int i = 0; i < count; i++)
                output[i] = binaryReader.ReadString();
            return output;
        }

        #endregion


        #region Serialize/Deserialize List<int> and int[]

        /// <summary>
        /// Writes the list of string to the binaryWriter in the format of:
        /// [itemCount as 7BitEncodedInt]
        /// [item as int]
        /// [item as int]
        /// :
        /// </summary>
        /// <param name="binaryWriter"></param>
        /// <param name="list"></param>
        public static void Write(this BinaryWriter binaryWriter, List<int> list)
        {
            binaryWriter.Write7BitEncodedInt(list.Count);
            foreach (int item in list) binaryWriter.Write(item);
        }

        /// <summary>
        /// Writes an int array to the binaryWriter in te format of:
        /// [length as 7BitEncodedInt]
        /// [item as int]
        /// [item as int]
        /// :
        /// </summary>
        /// <param name="binaryWriter"></param>
        /// <param name="array"></param>
        public static void Write(this BinaryWriter binaryWriter, int[] array)
        {
            binaryWriter.Write7BitEncodedInt(array.Length);
            foreach (int item in array) binaryWriter.Write(item);
        }
        /// <summary>
        /// Deserializes list of int from <code>binaryWriter.Write(List<int>)</code>
        /// or <code>binaryWriter.write(string[])</code>
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns></returns>
        public static List<int> ReadListOfInt32(this BinaryReader binaryReader)
        {
            int count = binaryReader.Read7BitEncodedInt();
            List<int> output = new List<int>(count);
            for (int i = 0; i < count; i++)
                output.Add(binaryReader.ReadInt32());
            return output;
        }

        /// <summary>
        /// Deserializes int[] from <code>binaryWriter.Write(List<int>)</code>
        /// or <code>binaryWriter.write(string[])</code>
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns></returns>
        public static int[] ReadArrayOfInt32(this BinaryReader binaryReader)
        {
            int count = binaryReader.Read7BitEncodedInt();
            if (count == 0)
                return new int[] { };
            int[] output = new int[count];
            for (int i = 0; i < count; i++)
                output[i] = binaryReader.ReadInt32();
            return output;
        }


        #endregion

        #region Serialize/Deserialize byte[]

        /// <summary>
        /// Writes a byte array to the binaryWriter in the format of:
        /// [length as 7BitEncodedInt]
        /// [item as byte]
        /// [item as byte]
        /// :
        /// </summary>
        /// <param name="binaryWriter"></param>
        /// <param name="array"></param>
        public static void WriteArrayOfByte(this BinaryWriter binaryWriter, byte[] array)
        {
            binaryWriter.Write7BitEncodedInt(array.Length);
            if (array.Length > 0)
            {
                binaryWriter.Write(array);
            }
        }

        /// <summary>
        /// Deserializes byte[] from <code>binaryWriter.WriteArrayOfByte(byte[])</code>
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns></returns>
        public static byte[] ReadArrayOfByte(this BinaryReader binaryReader)
        {
            int count = binaryReader.Read7BitEncodedInt();
            if (count == 0)
                return new byte[] { };
            byte[] output = binaryReader.ReadBytes(count);
            return output;
        }
        #endregion

        #region Serialize/Deserialize IDictionary<string,string>

        public static void Write(this BinaryWriter binaryWriter, IDictionary<string, string> dic)
        {
            binaryWriter.Write7BitEncodedInt(dic.Count);
            foreach (var entry in dic)
            {
                binaryWriter.Write(entry.Key);
                binaryWriter.Write(entry.Value);
            }
        }

        public static void Write(this BinaryWriter binaryWriter, IDictionary<string, bool> dic)
        {
            binaryWriter.Write7BitEncodedInt(dic.Count);
            foreach (var entry in dic)
            {
                binaryWriter.Write(entry.Key);
                binaryWriter.Write(entry.Value);
            }
        }

        public static void Write(this BinaryWriter binaryWriter, IDictionary<string, int> dic)
        {
            binaryWriter.Write7BitEncodedInt(dic.Count);
            foreach (var entry in dic)
            {
                binaryWriter.Write(entry.Key);
                binaryWriter.Write(entry.Value);
            }
        }

        public static void Write(this BinaryWriter binaryWriter, IDictionary<string, byte[]> dic)
        {
            binaryWriter.Write7BitEncodedInt(dic.Count);
            foreach (var entry in dic)
            {
                binaryWriter.Write(entry.Key);
                binaryWriter.WriteArrayOfByte(entry.Value);
            }
        }

        public static Dictionary<string, string> ReadDictionaryOfStringString(this BinaryReader binaryReader)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Deserialize(binaryReader);
            return dic;
        }

        public static Dictionary<string, bool> ReadDictionaryOfStringBool(this BinaryReader binaryReader)
        {
            Dictionary<string, bool> dic = new Dictionary<string, bool>();
            dic.Deserialize(binaryReader);
            return dic;
        }

        public static Dictionary<string, int> ReadDictionaryOfStringInt32(this BinaryReader binaryReader)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            dic.Deserialize(binaryReader);
            return dic;
        }

        public static Dictionary<string, byte[]> ReadDictionaryOfStringByteArray(this BinaryReader binaryReader)
        {
            Dictionary<string, byte[]> dic = new Dictionary<string, byte[]>();
            dic.Deserialize(binaryReader);
            return dic;
        }

        public static void Serialize(this IDictionary<string, string> dic, BinaryWriter bwriter)
        {
            bwriter.Write(dic);
        }

        public static byte[] Serialize(this IDictionary<string, string> dic)
        {
            using (MemoryStream mstream = new MemoryStream())
            {
                using (BinaryWriter bwriter = new BinaryWriter(mstream))
                {
                    bwriter.Write(dic);
                    bwriter.Flush();
                }
                return mstream.ToArray();
            }
        }
        public static IDictionary<string, string> Deserialize(this IDictionary<string, string> dic, BinaryReader breader)
        {
            int fieldCount = breader.Read7BitEncodedInt();
            for (int i = 0; i < fieldCount; i++)
            {
                string fieldName = breader.ReadString();
                string fieldValue = breader.ReadString();
                dic[fieldName] = fieldValue;
            }
            return dic;
        }
        public static IDictionary<string, bool> Deserialize(this IDictionary<string, bool> dic, BinaryReader breader)
        {
            int fieldCount = breader.Read7BitEncodedInt();
            for (int i = 0; i < fieldCount; i++)
            {
                string fieldName = breader.ReadString();
                bool fieldValue = breader.ReadBoolean();
                dic[fieldName] = fieldValue;
            }
            return dic;
        }

        public static IDictionary<string, int> Deserialize(this IDictionary<string, int> dic, BinaryReader breader)
        {
            int fieldCount = breader.Read7BitEncodedInt();
            for (int i = 0; i < fieldCount; i++)
            {
                string fieldName = breader.ReadString();
                int fieldValue = breader.ReadInt32();
                dic[fieldName] = fieldValue;
            }
            return dic;
        }

        public static IDictionary<string, byte[]> Deserialize(this IDictionary<string, byte[]> dic, BinaryReader breader)
        {
            int fieldCount = breader.Read7BitEncodedInt();
            for (int i = 0; i < fieldCount; i++)
            {
                string fieldName = breader.ReadString();
                byte[] fieldValue = breader.ReadArrayOfByte();
                dic[fieldName] = fieldValue;
            }
            return dic;
        }
        #endregion


        public static void Write(this BinaryWriter binaryWriter, IDictionary<int, bool> dic)
        {
            binaryWriter.Write7BitEncodedInt(dic.Count);
            foreach (var entry in dic)
            {
                binaryWriter.Write(entry.Key);
                binaryWriter.Write(entry.Value);
            }
        }

        public static Dictionary<int, bool> Deserialize(this Dictionary<int, bool> dic, BinaryReader breader)
        {
            int fieldCount = breader.Read7BitEncodedInt();
            for (int i = 0; i < fieldCount; i++)
            {
                var fieldName = breader.ReadInt32();
                var fieldValue = breader.ReadBoolean();
                dic[fieldName] = fieldValue;
            }
            return dic;
        }

        public static void Write(this BinaryWriter binaryWriter, Dictionary<string, Dictionary<int, bool>> dic)
        {
            binaryWriter.Write7BitEncodedInt(dic.Count);
            foreach (var entry in dic)
            {
                binaryWriter.Write(entry.Key);
                binaryWriter.Write(entry.Value);
            }
        }

        public static Dictionary<string, Dictionary<int, bool>> Deserialize(Dictionary<string, Dictionary<int, bool>> dic, BinaryReader breader)
        {
            int fieldCount = breader.Read7BitEncodedInt();
            for (int i = 0; i < fieldCount; i++)
            {
                var fieldName = breader.ReadString();
                var fieldValue = new Dictionary<int, bool>().Deserialize(breader);
                dic[fieldName] = fieldValue;
            }
            return dic;
        }
    }




}
