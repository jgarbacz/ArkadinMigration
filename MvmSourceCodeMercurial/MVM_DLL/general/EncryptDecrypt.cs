using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace MVM
{
   public class TestCrypto
    {

       public static string HexStringToString(string hex)
       {
           StringBuilder sb = new StringBuilder();
           foreach (string hexVal in hex.Cluster(2))
           {
               int intVal = Convert.ToInt32(hexVal, 16);
               char c = Convert.ToChar(intVal);
               sb.Append(c);
           }
           return sb.ToString();
       }

       public static byte[] HexStringToBytes(string hex)
       {
           MemoryStream ms = new MemoryStream();
           foreach (string hexVal in hex.Cluster(2))
           {
              byte b = (byte)Convert.ToInt32(hexVal, 16);
              ms.WriteByte(b);
           }
           return ms.ToArray();
       }

        public static void Main()
        {
            try
            {
                
                string key = "0A-83-3D-B5-C4-12-3F-3C-4D-1A-84-74-08-C5-6E-20-45-D7-46-11-85-D0-75-7A";
                byte[] keyBytes = HexStringToBytes(key.Replace("-",""));
               
                string iv = "1B-3D-3B-59-4F-7C-7A-94";
                byte[] ivBytes = HexStringToBytes(iv.Replace("-", ""));

                Console.WriteLine("key   [" + BitConverter.ToString(keyBytes) + "] keysize=" + keyBytes.Length);
                Console.WriteLine("iv   [" + BitConverter.ToString(ivBytes) + "] iv.length=" + ivBytes.Length);

                // Create a string to encrypt.
                string sData = " 1234567 8";

                Console.WriteLine("input   [" + sData + "] len="+sData.Length);
                // Encrypt the string to an in-memory buffer.
                byte[] Data = EncryptTextToMemory(sData, keyBytes, ivBytes);
                Console.WriteLine("encrypt [" + BitConverter.ToString(Data) + "] len=" + Data.Length);

                // Decrypt the buffer back to a string.
                string Final = DecryptTextFromMemory(Data, keyBytes, ivBytes);
                Console.WriteLine("output  [" + Final + "] len="+Final.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("DONE");
        }

        private static byte[] keyBytes = HexStringToBytes("0A-83-3D-B5-C4-12-3F-3C-4D-1A-84-74-08-C5-6E-20-45-D7-46-11-85-D0-75-7A".Replace("-",""));
        private static byte[] ivBytes = HexStringToBytes("1B-3D-3B-59-4F-7C-7A-94".Replace("-", ""));

        public static string EncryptToHexString(string input)
        {
            byte[] Data = EncryptTextToMemory(input, keyBytes, ivBytes);
            return BitConverter.ToString(Data).Replace("-","");
        }

        public static string DecryptHexString(string input)
        {
            byte[] inputBytes= HexStringToBytes(input);
            string output = DecryptTextFromMemory(inputBytes, keyBytes, ivBytes);
            return output.TrimEnd('\0');
        }

        public static byte[] EncryptTextToMemory(string Data, byte[] Key, byte[] IV)
        {
            try
            {
                // Create a MemoryStream.
                MemoryStream mStream = new MemoryStream();

                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                CryptoStream cStream = new CryptoStream(
                    mStream,
                    new TripleDESCryptoServiceProvider().CreateEncryptor(Key, IV),
                    CryptoStreamMode.Write
                );

                // Convert the passed string to a byte array.
                byte[] toEncrypt = new ASCIIEncoding().GetBytes(Data);

                // Write the byte array to the crypto stream and flush it.
                cStream.Write(toEncrypt, 0, toEncrypt.Length);
                cStream.FlushFinalBlock();

                // Get an array of bytes from the 
                // MemoryStream that holds the 
                // encrypted data.
                byte[] ret = mStream.ToArray();

                // Close the streams.
                cStream.Close();
                mStream.Close();

                // Return the encrypted buffer.
                return ret;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }

        }

        public static string DecryptTextFromMemory(byte[] Data, byte[] Key, byte[] IV)
        {
            try
            {
                // Create a new MemoryStream using the passed 
                // array of encrypted data.
                MemoryStream msDecrypt = new MemoryStream(Data);

                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                CryptoStream csDecrypt = new CryptoStream(
                    msDecrypt,
                    new TripleDESCryptoServiceProvider().CreateDecryptor(Key, IV),
                    CryptoStreamMode.Read
                    );

                // Create buffer to hold the decrypted data.
                byte[] fromEncrypt = new byte[Data.Length];

                // Read the decrypted data out of the crypto stream
                // and place it into the temporary buffer.
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

                //Convert the buffer into a string and return it.
                return new ASCIIEncoding().GetString(fromEncrypt);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }
    }
}

