using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace MetraTech.Foundation.Security
{
  /// <summary>
  /// Crypto implementation using ProtectedData
  /// </summary>
  public class ProtectedDataCrypto
  {
    private readonly byte [] _Entropy = null;

    /// <summary>
    /// Create a new ProtectedData Crypto instance
    /// </summary>
    /// <param name="entropy">The entropy to use</param>
    public ProtectedDataCrypto ( byte [] entropy )
    {
      _Entropy = entropy;
    }

    /// <summary>
    /// Decrypt a cipher
    /// </summary>
    /// <param name="cipher">The cipher text</param>
    /// <returns>The plain text</returns>
    public byte [] Decrypt ( byte [] cipher )
    {
      if ( cipher == null )
      {
        throw new ArgumentNullException ( "cipher" );
      }
      return ProtectedData.Unprotect ( cipher, _Entropy, DataProtectionScope.LocalMachine );
    }

    /// <summary>
    /// Encrypt plain text
    /// </summary>
    /// <param name="plaintext">the plain text</param>
    /// <returns>The cipher text</returns>
    public byte [] Encrypt ( byte [] plaintext )
    {
      if ( plaintext == null )
      {
        throw new ArgumentNullException ( "plaintext" );
      }
      return ProtectedData.Protect ( plaintext, _Entropy, DataProtectionScope.LocalMachine );
    }

    public byte [] Hash ( byte [] plaintext, ref string keyId )
    {
      throw new NotSupportedException ();
    }
  }
}
