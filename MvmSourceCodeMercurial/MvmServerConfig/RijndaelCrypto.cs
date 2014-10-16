using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace MetraTech.Foundation.Security
{
  /// <summary>
  /// Crypto implementation using Rijndael algorithm
  /// </summary>
  public class RijndaelCrypto
  {
    private readonly byte [] _Key;
    private readonly byte [] _IV;

    /// <summary>
    /// Create a new RijndaelCrypto instance
    /// </summary>
    /// <param name="key">The key to use</param>
    /// <param name="iv">The initialization vector to use</param>
    public RijndaelCrypto ( byte [] key, byte [] iv )
    {
      this._Key = key;
      this._IV = iv;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage ( "Microsoft.Reliability", "CA2000:Dispose objects before losing scope" )]
    private RijndaelManaged GetAlgorithm ()
    {
      var rij = new RijndaelManaged ();
      rij.BlockSize = 256;
      rij.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
      rij.Key = _Key;
      rij.IV = _IV;
      return rij;
    }

    /// <summary>
    /// Decrypt a cipher
    /// </summary>
    /// <param name="cipher">The cipher text</param>
    /// <returns>The plain text</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage ( "Microsoft.Usage", "CA2202:Do not dispose objects multiple times" )]
    public byte [] Decrypt ( byte [] cipher )
    {
      if ( cipher == null )
      {
        throw new ArgumentNullException ( "cipher" );
      }
      using ( var rij = GetAlgorithm () )
      {
        using ( var dec = rij.CreateDecryptor () )
        {
          using ( var mem = new System.IO.MemoryStream ( cipher ) )
          {
            using ( var strm = new System.Security.Cryptography.CryptoStream ( mem, dec, System.Security.Cryptography.CryptoStreamMode.Read ) )
            {
              using ( var rdr = new System.IO.StreamReader ( strm ) )
              {
                return Encoding.UTF8.GetBytes ( rdr.ReadToEnd () );
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// Encrypt plain text
    /// </summary>
    /// <param name="plaintext">the plain text</param>
    /// <returns>The cipher text</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage ( "Microsoft.Usage", "CA2202:Do not dispose objects multiple times" )]
    public byte [] Encrypt ( byte [] plaintext )
    {
      if ( plaintext == null )
      {
        throw new ArgumentNullException ( "plaintext" );
      }
      using ( var rij = GetAlgorithm () )
      {
        using ( var mem = new System.IO.MemoryStream () )
        {
          using ( var enc = rij.CreateEncryptor () )
          {
            using ( var strm = new System.Security.Cryptography.CryptoStream ( mem, enc, System.Security.Cryptography.CryptoStreamMode.Write ) )
            {
              strm.Write ( plaintext, 0, plaintext.Length );
              strm.FlushFinalBlock ();
              return mem.ToArray ();
            }
          }
        }
      }
    }

    public byte [] Hash ( byte [] plaintext, ref string keyId )
    {
      using ( var hmac = new HMACSHA512 ( this._Key ) )
      {
        return hmac.ComputeHash ( plaintext );
      }
    }
  }

}
