using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{

    /*
     * <md5>
     * <input>OBJECT.password</input>
     * <output>OBJECT.hashcode</output>
     * </md5>
     */
    class MMd5Hash : IModuleSetup, IModuleRun
    {
        private string valueSyntax;
        private IReadString valueParsed;

        private string hashSyntax;
        private IWriteString hashParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MMd5Hash m = new MMd5Hash();
            // xml extraction
            m.valueSyntax = me.SelectNodeInnerText("./input");
            m.hashSyntax = me.SelectNodeInnerText("./output");
            // parsing
            m.valueParsed = mc.ParseSyntax(m.valueSyntax);
            m.hashParsed = mc.ParseWritableSyntax(m.hashSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string value=this.valueParsed.Read(mc);
            string hash = this.GetMD5(value);
            this.hashParsed.Write(mc,hash);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("md5:" + this.valueSyntax);
        }

        
        private System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
        public string GetMD5(string plainText)
        {
            // Convert the input string to a byte array and compute the hash
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(plainText));

            // Create a new Stringbuilder to collect the bytes and create a string
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
