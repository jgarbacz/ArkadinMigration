using System;
using System.Text;
using System.Reflection;

//using MetraTech.Security;
//using MetraTech.Security.Crypto;
using System.Security.Cryptography;

namespace MVM
{
    // This stuff was originally in the DataMigrationEncryption DLL; now switched over to load everything lazily
    public static class MetranetEncryption
    {
        // TODO: do credit card encryption

        /// <summary>
        /// Given an unencrypted credit card number it returns T_PS_PAYMENT_INSTRUMENT.nm_account_number
        /// </summary>
        /// <param name="creditCardAccountNumber"></param>
        /// <returns></returns>
        //public static string CreditCardEncrypt(string creditCardAccountNumber)
        //{
        //    CryptoManager cryptoManager = new CryptoManager();
        //    string creditCardEncrypt = cryptoManager.Encrypt(CryptKeyClass.PaymentInstrument, creditCardAccountNumber);
        //    return creditCardEncrypt;
        //}

        /// <summary>
        /// Given an unencrypted credit card number it returns T_PAYMENT_INSTRUMENT.tx_hash
        /// </summary>
        /// <param name="creditCardAccountNumber"></param>
        /// <returns></returns>
        //public static string CreditCardHash(string creditCardAccountNumber)
        //{
        //    CryptoManager cryptoManager = new CryptoManager();
        //    string md5Hash = GetMD5(creditCardAccountNumber);
        //    string saltedAccount = md5Hash + creditCardAccountNumber;
        //    string creditCardHash = cryptoManager.Hash(HashKeyClass.PaymentMethodHash, saltedAccount);
        //    return creditCardHash;
        //}

        /// <summary>
        /// Given an unencrypted credit card number it returns T_PS_PAYMENT_INSTRUMENT.nm_account_number and T_PS_PAYMENT_INSTRUMENT.nm_account_number
        /// </summary>
        /// <param name="creditCardAccountNumber"></param>
        /// <param name="creditCardHash"></param>
        /// <param name="creditCardEncrypt"></param>
        //public static void CreditCardHashAndEncrypt(string creditCardAccountNumber, out string creditCardHash, out string creditCardEncrypt)
        //{
        //    creditCardHash = CreditCardHash(creditCardAccountNumber);
        //    creditCardEncrypt = CreditCardEncrypt(creditCardAccountNumber);
        //    return;
        //}

        public static bool failed = false;
        public static string assemblyName = "MetraTech.Security";
        //public static string assemblyName = "MetraTech.Security.Crypto";
        public static string className = "MetraTech.Security.PasswordManager";
        public static System.Reflection.Assembly assembly = null;
        public static Type classType = null;
        public static MethodInfo initMethod = null;
        public static MethodInfo hashMethod = null;
        public static object passwordManager = null;

        /// <summary>
        /// Given a text T_USER_CREDENTIALS.nm_login, T_USER_CREDENTIALS.nm_space and a password, it returns the hashed password T_USER_CREDENTIALS.tx_password 
        /// </summary>
        /// <param name="accountType"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string PasswordHash(string nmLogin, string nmSpace, string password)
        {
            //PasswordManager passwordManager = new PasswordManager();
            //passwordManager.Initialize(nmLogin, nmSpace);
            //string passwordHash = passwordManager.HashNewPassword(password);
            if (failed)
            {
                return "";
            }
            if (assembly == null)
            {
                try
                {
                    assembly = System.Reflection.Assembly.Load(assemblyName);
                    classType = assembly.GetType(className);
                    initMethod = classType.GetMethod("Initialize");
                    hashMethod = classType.GetMethod("HashNewPassword");
                }
                catch
                {
                    failed = true;
                    return "";
                    //throw new Exception("Could not make reflected encryption call: " + e);
                }
            }
            passwordManager = assembly.CreateInstance(className);
            initMethod.Invoke(passwordManager, new object[] { nmLogin, nmSpace });
            string passwordHash = (string)hashMethod.Invoke(passwordManager, new object[] { password });
            return passwordHash;
        }

        // copied by kyle from somewhere in core...
        static private string GetMD5(string plainText)
        {
            MD5 md5Hasher = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(plainText));

            // Create a new Stringbuilder to collect the bytes and create a string.
            StringBuilder builder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return builder.ToString();
        }
    }
}
