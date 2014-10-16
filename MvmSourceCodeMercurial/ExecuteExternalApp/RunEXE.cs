using System;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Collections;
using MetraTech;
using MetraTech.UsageServer;
using MetraTech.Interop.MTAuth;
using MetraTech.Interop.Rowset;
using MetraTech.Interop.MeterRowset;
using System.Runtime.InteropServices;
using System.Text;

namespace MetraTech.Baseline.Adapters
{
  public class RunEXE : MetraTech.UsageServer.IRecurringEventAdapter2
  {

    // IRecurringEventAdapter Properties 
    public bool SupportsScheduledEvents { get { return true; } }
    public bool SupportsEndOfPeriodEvents { get { return true; } }
    public ReverseMode Reversibility { get { return ReverseMode.Custom; } }
    public bool AllowMultipleInstances { get { return false; } }

    /// <summary>
    /// Specifies whether the adapter can process billing groups as a group
    /// of accounts, as individual accounts or if it
    /// cannot process billing groups at all.
    /// This setting is only valid for end-of-period adapters.
    /// </summary>
    /// <returns>BillingGroupSupportType</returns>
    public BillingGroupSupportType BillingGroupSupport { get { return BillingGroupSupportType.Account; } }

    /// <summary>
    /// Specifies whether this adapter has special constraints on the membership
    /// of a billing group.
    /// This setting is only valid for adapters that support billing groups.
    /// </summary>
    /// <returns>True if constraints exist, false otherwise</returns>
    public bool HasBillingGroupConstraints { get { return false; } }

    public RunEXE () { }

    // Member Variables
    private IRecurringEventRunContext mContext;
    private Logger mLogger;
    private static string mAdapterName = "ExecuteExternalApp";

    private static string mAdapterInstanceName;
    private string _appName;
    private string _workingDirectory;
    private string _commandArgs;
    private string _reverseArgs = null;
    private static StringBuilder _standardOut;
    private static StringBuilder _standardError;

    /// <summary>
    /// Perform any one time or start up initialization, including reading our config file
    /// for instance specific information
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="configFile"></param>
    /// <param name="sessionContext"></param>
    /// <param name="limitedInit"></param>
    public void Initialize ( string eventName, string configFile, IMTSessionContext sessionContext, bool limitedInit )
    {

      if ( limitedInit )
      {
        if ( mLogger == null )
        {
          mLogger = new Logger ( "[" + mAdapterName + "]" );
        }
        mLogger.LogDebug ( "Intializing " + mAdapterName + " (limited)" );
      }
      else
      {
        Debug.Assert ( sessionContext != null );

        mLogger = new Logger ( "[" + mAdapterName + "]" ); ;

        //Load the custom adapter settings from the given config file
        try
        {
          XmlDocument xmldoc = new XmlDocument ();
          xmldoc.Load ( configFile );
          mAdapterInstanceName = xmldoc.SelectSingleNode ( "//AdapterName" ).InnerText;


          ReadConfig ( configFile, xmldoc, sessionContext );
        }
        catch ( Exception ex )
        {
          string info = string.Format ( "Adapter[{0}]: Unable to read configuration file [{1}]:{2}", mAdapterInstanceName, configFile, ex.Message );
          mLogger.LogError ( info );
          throw new Exception ( info, ex );
        }

        mLogger.LogDebug ( "Read configuration for {0} from {1}", mAdapterInstanceName, configFile );
      }

    }

    /// <summary>
    /// Perform the actual work of the adapter
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string Execute ( IRecurringEventRunContext context )
    {
      mContext = context;
      string info = string.Format ( "Executing {0} in context: {1}", mAdapterInstanceName, context );
      mLogger.LogDebug ( info );
      context.RecordInfo ( info );

      if ( context.EventType == MetraTech.UsageServer.RecurringEventType.EndOfPeriod )
      {
        info = string.Format ( "Executing in EOP mode for Interval {0} and Billing Group {1}", context.UsageIntervalID, context.BillingGroupID );
        // EOP Adapter code here...
      }
      else
      {
        info = string.Format ( "Executing in Scheduled mode for Start Date {0} and End Date {1}", context.StartDate, context.EndDate );
        // Scheduled Adapter code here...
      }

      // Common (EOP/Scheduled) Execute code here...
      RunExecutable ( context, false );

      info = "Success";
      mLogger.LogInfo ( info );

      return info;
    }

    /// <summary>
    /// Undo or reverse any work done by the adapter during the call to Execute for the same interval or period
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string Reverse ( IRecurringEventRunContext context )
    {
      string info;
      if ( context.EventType == MetraTech.UsageServer.RecurringEventType.EndOfPeriod )
      {
        info = string.Format ( "Reversing in EOP mode for Interval {0} and Billing Group {1}", context.UsageIntervalID, context.BillingGroupID );
        // EOP Adapter code here...

      }
      else
      {
        info = string.Format ( "Reversing in Scheduled mode for Start Date {0} and End Date {1}", context.StartDate, context.EndDate );
        // Scheduled Adapter code here...
      }

      // Common (EOP/Scheduled) Reverse/Undo code here...
      if ( _reverseArgs != null )
      {
        RunExecutable ( context, true );
      }

      mLogger.LogInfo ( info );
      context.RecordInfo ( info );

      info = "Success";
      return info;
    }


    public void CreateBillingGroupConstraints ( int intervalID, int materializationID )
    {
      throw new ApplicationException ( string.Concat ( RunEXE.mAdapterName, ". CreateBillingGroupConstraints should not have been called: billing group constraits are not enforced by this adapter - check the HasBillingGroupConstraints property." ) );
    }
    public void SplitReverseState ( int parentRunID, int parentBillingGroupID, int childRunID, int childBillingGroupID )
    {
      throw new ApplicationException ( string.Concat ( RunEXE.mAdapterName, ". SplitReverseState should not have been called: reverse is not needed for this adapter - check the Reversibility property." ) );
    }
    public void Shutdown ()
    {
      this.mLogger.LogDebug ( "Shutdown" );
    }

    #region Private Methods
    private void ReadConfig ( string configFile, XmlDocument xmlDoc, IMTSessionContext context )
    {
      XmlNode configNode = xmlDoc.SelectSingleNode ( "xmlconfig" );
      _appName = configNode.SelectSingleNode ( "externalApp" ).InnerText;
      // default the working directory to env var MTRMPBIN
      _workingDirectory = configNode.SelectSingleNode ( "workingDirectory" ).InnerText.Trim();
      if (_workingDirectory.Equals(""))
      {
          _workingDirectory = System.Environment.GetEnvironmentVariable("MTRMPBIN");
      }
      _commandArgs = configNode.SelectSingleNode ( "commandArgs" ).InnerText;
      XmlNode reverseNode = configNode.SelectSingleNode ( "reverseArgs" );
      if ( reverseNode != null )
      {
        _reverseArgs = reverseNode.InnerText;
      }
    }
    private void RunExecutable ( IRecurringEventRunContext context, bool reverse )
    {
      string outputMsg;
      string outputErrorMsg;
      string fileName;
      _standardOut = new StringBuilder ();
      _standardError = new StringBuilder ();


      if ( _workingDirectory.EndsWith ( @"\" ) )
        fileName = _workingDirectory + _appName;
      else
        fileName = _workingDirectory + "\\" + _appName;

      FileInfo fi = new FileInfo ( fileName );
      if ( !fi.Exists )
      {
        context.RecordWarning ( string.Format ( "{0} executable does not exist.", mAdapterInstanceName ) );
        throw new FileNotFoundException ( string.Format ( "File does not exist ({0}).", fileName ) );
      }

      try
      {
          Interop.MTServerAccess.IMTServerAccessDataSet serverAccess = new Interop.MTServerAccess.MTServerAccessDataSet();
        serverAccess.Initialize ();
        Interop.MTServerAccess.IMTServerAccessData serverInfo = serverAccess.FindAndReturnObject("NetMeter");
        string strUserId = serverInfo.UserName;
        string strPassword = serverInfo.Password;
        string strDBName = serverInfo.ServerName;
        string strIntervalID = string.Empty;
        string strRunID = context.RunID.ToString ();
        string strReverseID = context.RunIDToReverse.ToString ();

        if ( context.EventType == RecurringEventType.EndOfPeriod )
          strIntervalID = context.UsageIntervalID.ToString ();

        string arg = null;
        if ( reverse )
        {
          arg = _reverseArgs.Replace ( "%%UserID%%", strUserId ).Replace ( "%%Password%%", strPassword ).Replace ( "%%ServerName%%", strDBName ).Replace ( "%%IntervalID%%", strIntervalID ).Replace ( "%%RunID%%", strRunID ).Replace ( "%%RunIDToReverse%%", strReverseID );
        }
        else
        {
          arg = _commandArgs.Replace ( "%%UserID%%", strUserId ).Replace ( "%%Password%%", strPassword ).Replace ( "%%ServerName%%", strDBName ).Replace ( "%%IntervalID%%", strIntervalID ).Replace ( "%%RunID%%", strRunID );
        }

        ProcessStartInfo startInfo = new ProcessStartInfo ( fileName );
        startInfo.Arguments = arg;

        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = true;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        startInfo.WorkingDirectory = _workingDirectory;

        mLogger.LogDebug ( string.Format ( "{0} Starting..", mAdapterInstanceName ) );
        mLogger.LogDebug(string.Format("{0} Calling process {1} with arguments: {2}", mAdapterInstanceName, fileName, arg));
        context.RecordInfo(string.Format("{0} Calling process {1} with arguments: {2}", mAdapterInstanceName, fileName, arg));

        Process exeProcess = Process.Start ( startInfo );
        exeProcess.OutputDataReceived += new DataReceivedEventHandler ( exeProcess_OutputDataReceived );
        exeProcess.ErrorDataReceived += new DataReceivedEventHandler ( exeProcess_ErrorDataReceived );
        exeProcess.BeginOutputReadLine ();
        exeProcess.BeginErrorReadLine ();

        exeProcess.WaitForExit ();// (300000);

        if ( exeProcess.ExitCode != 0 )
        {
          context.RecordWarning ( "Execution failed: view detailed error message in MTLog." );
          throw new Exception ( string.Format ( "Error while executing {0}.", mAdapterInstanceName ) );
        }
        else
        {
          context.RecordInfo ( string.Format ( "{0} completed successfully.", mAdapterInstanceName ) );
        }

        exeProcess.Close ();
      }
      catch ( Exception ex )
      {
        mLogger.LogWarning ( "Failed in RunExecutable()" );
        throw new Exception ( ex.Message + "  " );
      }
      finally
      {
        outputMsg = _standardOut.ToString ();// exeProcess.StandardOutput.ReadToEnd();
        outputErrorMsg = _standardError.ToString (); //exeProcess.StandardError.ReadToEnd();
        if ( !string.IsNullOrEmpty ( outputMsg ) )
        {
            mLogger.LogDebug(outputMsg);
            foreach (string msg in outputMsg.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                context.RecordInfo(msg);
            }
        }
        if ( !string.IsNullOrEmpty ( outputErrorMsg ) )
        {
          mLogger.LogError ( outputErrorMsg );
          foreach (string msg in outputErrorMsg.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
          {
              context.RecordWarning(msg);
          }
        }
      }

      return;
    }


    void exeProcess_ErrorDataReceived ( object sender, DataReceivedEventArgs e )
    {
      // Collect the error output.
      if ( !String.IsNullOrEmpty ( e.Data ) )
      {
        // append to previous error output
        _standardError.Append ( Environment.NewLine + e.Data );
      }
    }

    void exeProcess_OutputDataReceived ( object sender, DataReceivedEventArgs e )
    {
      // Collect the standard output.
      if ( !String.IsNullOrEmpty ( e.Data ) )
      {
        // Add the text to the previous standard output.
        _standardOut.Append ( Environment.NewLine + e.Data );
      }
    }
    #endregion

  }
}
