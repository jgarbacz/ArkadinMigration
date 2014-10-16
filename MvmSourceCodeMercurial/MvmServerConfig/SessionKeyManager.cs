using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace MetraTech.Foundation.Security
{
  /// <summary>
  /// Class to hold a set of session keys, which are loaded from an XML file
  /// </summary>
  public class SessionKeyManager
  {
    private ProtectedDataCrypto PDCrypto = new ProtectedDataCrypto ( GetEntropy () );
    /// <summary>
    /// allows you to change the ICrypto used for managing the keys and initialization vectors
    /// </summary>
    public ProtectedDataCrypto KeyClassCrypto
    {
      set
      {
        if ( value == null )
        {
          throw new ArgumentNullException ( "value" );
        }
        PDCrypto = value;
      }
    }

    /// <summary>
    /// create a new session key manager with specified object factory
    /// </summary>
    public SessionKeyManager ( )
    {
    }

    private Dictionary<string, KeyParameters> KeyClassesByName = new Dictionary<string, KeyParameters> ();
    private Dictionary<string, KeyParameters> KeyClassesById = new Dictionary<string, KeyParameters> ();
    private Dictionary<string, string> KeyIdsByName = new Dictionary<string, string> ();

    /// <summary>
    /// Initialize the set of key classes from the file
    /// </summary>
    /// <param name="filename">the filename to load from</param>
    private void Init ( string filename )
    {
      var file = new System.IO.FileInfo ( filename );
      using ( var stream = file.OpenRead () )
      {
        var doc = System.Xml.Linq.XDocument.Load ( stream );
        var kcs = from item in doc.Descendants ( "keyClass" ) select item;
        foreach ( var kc in kcs )
        {
          var name = kc.Attribute ( "name" );
          var keys = from item in kc.Descendants ( "key" ) select item;
          foreach ( var key in keys )
          {
            var current = key.Attribute ( "current" );
            var id = key.Element ( "id" );
            var val = key.Element ( "value" );
            var iv = key.Element ( "iv" );
            var kp = new KeyParameters () { Key = DecodeString((string) val), InitializationVector = DecodeString ((string) iv) };
            if ( "true".Equals ( (string) current, StringComparison.OrdinalIgnoreCase ) )
            {
              KeyClassesByName [ (string) name ] = kp;
            }
            KeyClassesById [ (string) id ] = kp;
            KeyIdsByName [ (string) name ] = (string) id;
          }
        }
      }

    }

    /// <summary>
    /// Decode the supplied string.  This will:
    /// - de-base64 the string
    /// - decrypt the key class
    /// - convert to UTF8
    /// - de-base64 the string
    /// </summary>
    /// <param name="inValue">The string to decode</param>
    /// <returns>the decoded string</returns>
    protected byte[] DecodeString (string inValue)
    {
      var keyRaw = Convert.FromBase64String ( inValue );
      var keyDec = PDCrypto.Decrypt ( keyRaw );
      var keyStr = Encoding.UTF8.GetString ( keyDec );
      return Convert.FromBase64String ( keyStr );
    }

    private string _sessionKeyFileName;
    /// <summary>
    /// the filename containing the session keys
    /// </summary>
    public string SessionKeyFileName
    {
      get
      {
        return _sessionKeyFileName;
      }
      set
      {
        if ( string.IsNullOrEmpty ( value ) )
        {
          throw new ArgumentNullException ( "value" );
        }
        _sessionKeyFileName = value;
      }
    }

    /// <summary>
    /// retrieve the Crypto to use for a given key class
    /// </summary>
    /// <param name="keyClass">The key class to retrieve</param>
    /// <returns>The Crypto</returns>
    public RijndaelCrypto GetCryptoByKeyClass ( string keyClass )
    {
      Init ( SessionKeyFileName );
      var kc = this.KeyClassesByName [ keyClass ];
      // 2) instantiate crypto with key/iv
      // TODO: also support RsaCrypto
      var crypto = new RijndaelCrypto ( kc.Key, kc.InitializationVector );
      return crypto;
    }

    /// <summary>
    /// retrieve the Crypto to use for a given key class
    /// </summary>
    /// <param name="keyId">The key id to retrieve</param>
    /// <returns>The Crypto</returns>
    public RijndaelCrypto GetCryptoByKeyId ( string keyId )
    {
      Init ( SessionKeyFileName );
      var kc = this.KeyClassesById [ keyId ];
      // 2) instantiate crypto with key/iv
      // TODO: also support RsaCrypto
      var crypto = new RijndaelCrypto ( kc.Key, kc.InitializationVector );
      return crypto;
    }

    /// <summary>
    /// retrieve the key id for a given key class name
    /// </summary>
    /// <param name="keyClass">The key class to retrieve</param>
    /// <returns>The key id</returns>
    public string GetKeyIdByKeyClass ( string keyClass )
    {
      Init ( SessionKeyFileName );
      return this.KeyIdsByName[ keyClass ];
    }

    /// <summary>
    /// this is a hardcoded entropy inherited from legacy MetraNet
    /// </summary>
    private const Int32 _Seed = 987654321;

    /// <summary>
    /// Entropy needs to be consistent.
    /// 
    /// Provides a very thin layer of extra security, but its hardcoded (the seed) so even that is arguable
    /// </summary>
    /// <returns>The entropy to use</returns>
    private static byte [] GetEntropy ()
    {
      var random = new Random ( _Seed );
      var entropy = new StringBuilder ();
      for ( int i = 0; i < 5; i++ )
      {
        entropy.Append ( random.Next () );
      }
      return Encoding.UTF8.GetBytes ( entropy.ToString () );
    }

    /// <summary>
    /// internal class used to hold the key and initialization vector
    /// </summary>
    class KeyParameters
    {
      /// <summary>
      /// the initialization vector
      /// </summary>
      public byte [] InitializationVector { get; set; }
      /// <summary>
      /// the key
      /// </summary>
      public byte [] Key { get; set; }
    }
  }

}
