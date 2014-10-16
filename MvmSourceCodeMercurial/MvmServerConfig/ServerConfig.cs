using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetraTech.Foundation.Security
{
  public static class ServerConfigManager
  {
    public static ServerConfig GetServerConfig ( string rmpPath, string serverType )
    {
      var serversPath = System.IO.Path.Combine ( rmpPath, "Config", "ServerAccess", "servers.xml" );
      var file = new System.IO.FileInfo ( serversPath );
      using ( var stream = file.OpenRead () )
      {
        var doc = System.Xml.Linq.XDocument.Load ( stream );
        var sds = from item in doc.Descendants ( "server" ) select item;
        foreach ( var sd in sds )
        {
          var elem = sd.Element ( "servertype" );
          if ( elem.Value.Equals ( serverType, StringComparison.InvariantCultureIgnoreCase ) )
          {
            var conf = new ServerConfig () { ServerType = elem.Value };
            var keyManager = new SessionKeyManager ();
            elem = sd.Element ( "servername" );
            if ( elem != null )
            {
              conf.ServerName = elem.Value;
            }
            elem = sd.Element ( "databasename" );
            if ( elem != null )
            {
              conf.DatabaseName = elem.Value;
            }
            elem = sd.Element ( "databasedriver" );
            if ( elem != null )
            {
              conf.DatabaseDriver = elem.Value;
            }
            elem = sd.Element ( "databasetype" );
            if ( elem != null )
            {
              conf.DatabaseType = elem.Value;
            }
            elem = sd.Element ( "username" );
            if ( elem != null )
            {
              conf.Username = elem.Value;
            }
            elem = sd.Element ( "password" );
            if ( elem != null )
            {
              conf.Password = elem.Value;
              if ( !string.IsNullOrEmpty ( conf.Password ) )
              {
                var encrypted = elem.Attribute ( "encrypted" );
                if ( encrypted != null && !string.IsNullOrEmpty ( encrypted.Value ) && encrypted.Value.Equals ( "true", StringComparison.InvariantCultureIgnoreCase ) )
                {
                  var keyId = conf.Password.Substring ( 0, 36 );
                  var key = conf.Password.Substring ( 36 );
                  keyManager.SessionKeyFileName = System.IO.Path.Combine(rmpPath, "Config", "Security", "sessionkeys.xml");
                  conf.RijndaelCrypto = keyManager.GetCryptoByKeyId ( keyId );
                  conf.Password = Encoding.UTF8.GetString ( conf.RijndaelCrypto.Decrypt ( Convert.FromBase64String ( key ) ) );

                  // The tx_password starts with the PasswordHash GUID, not the DatabasePassword GUID
                  conf.KeyId = keyManager.GetKeyIdByKeyClass("PasswordHash");
                }
              }
            }
            elem = sd.Element ( "numretries" );
            if ( elem != null && !string.IsNullOrEmpty ( elem.Value ) )
            {
              conf.NumRetries = Convert.ToInt32 ( elem.Value );
            }
            elem = sd.Element ( "timeout" );
            if ( elem != null && !string.IsNullOrEmpty ( elem.Value ) )
            {
              conf.Timeout = Convert.ToInt32 ( elem.Value );
            }
            elem = sd.Element ( "priority" );
            if ( elem != null && !string.IsNullOrEmpty ( elem.Value ) )
            {
              conf.Priority = Convert.ToInt32 ( elem.Value );
            }
            elem = sd.Element ( "secure" );
            if ( elem != null && !string.IsNullOrEmpty ( elem.Value ) )
            {
              conf.Secure = Convert.ToInt32 ( elem.Value );
            }
            elem = sd.Element ( "portnumber" );
            if ( elem != null && !string.IsNullOrEmpty ( elem.Value ) )
            {
              conf.PortNumber = Convert.ToInt32 ( elem.Value );
            }
            elem = sd.Element ( "dtcenabled" );
            if ( elem != null && !string.IsNullOrEmpty ( elem.Value ) )
            {
              conf.DtcEnabled = Convert.ToInt32 ( elem.Value );
            }
            return conf;

          }
        }
      }
      throw new ApplicationException ( string.Format ( "No server def found for: {0}", serverType ) );
    }
  }

  public class ServerConfig
  {
    public string ServerType
    {
      get;
      set;
    }

    public string ServerName
    {
      get;
      set;
    }

    public string DatabaseName
    {
      get;
      set;
    }

    public string DatabaseDriver
    {
      get;
      set;
    }

    public string DatabaseType
    {
      get;
      set;
    }

    public string Username
    {
      get;
      set;
    }

    public string Password
    {
      get;
      set;
    }

    public int NumRetries
    {
      get;
      set;
    }

    public int Timeout
    {
      get;
      set;
    }

    public int Priority
    {
      get;
      set;
    }

    public int Secure
    {
      get;
      set;
    }

    public int PortNumber
    {
      get;
      set;
    }

    public int DtcEnabled
    {
      get;
      set;
    }

    public string KeyId
    {
      get;
      set;
    }

    public RijndaelCrypto RijndaelCrypto
    {
      get;
      set;
    }
  }
}
