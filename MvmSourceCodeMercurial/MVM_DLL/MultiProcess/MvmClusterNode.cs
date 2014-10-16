using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;
using NLog;

namespace MVM
{
    public class MvmClusterNode
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public readonly MvmClusterBase mvmCluster;
        public int nodeId;
        public string machine;
        public bool isMaster = false;
        public bool isProfiler = false;
        public int port;
        public int numInstances;
        public int instanceNum;
        public string bin = "";
        public string exe="";
        public string groupId;
        public bool is64bit;
        public string pid="";
        public string ExeFullName { get { return Path.Combine(this.bin, this.exe); } }

        // shortcuts
        public string portStr { get { return this.port.ToString(); } }
        public string nodeIdStr { get { return this.nodeId.ToString(); } }
        public MvmEngine mvm { get { return this.mvmCluster.mvm; } }
        
        // set as we go
        public bool active = false;
        private SocketHandler socketHandler;


        public void Serialize(BinaryWriter bwriter)
        {
            bwriter.Write(this.nodeId);
            bwriter.Write(this.machine);
            bwriter.Write(this.port);
        }

        public MvmClusterNode(MvmClusterBase mvmCluster,BinaryReader breader)
        {
            this.mvmCluster = mvmCluster;
            this.nodeId=breader.ReadInt32();
            this.machine = breader.ReadString();
            this.bin = "";
            this.port = breader.ReadInt32();
        }
        
        /// <summary>
        /// Instanciate from a dictionary
        /// </summary>
        /// <param name="mvmCluster"></param>
        /// <param name="dic"></param>
        public MvmClusterNode(MvmClusterBase mvmCluster, Dictionary<string, string> dic)
        {
            this.mvmCluster = mvmCluster;
            this.nodeId = dic["node_id"].ToInt();
            this.UpdateWithDictionary(dic);
        }

        public void UpdateWithDictionary(Dictionary<string, string> dic)
        {
            if (dic.ContainsNonNullOrEmptyValue("is_master")) this.isMaster = dic["is_master"].Equals("1");
            if (dic.ContainsNonNullOrEmptyValue("is_profiler")) this.isProfiler = dic["is_profiler"].Equals("1");
            if (dic.ContainsNonNullOrEmptyValue("server")) this.machine = dic["server"];
            if (dic.ContainsNonNullOrEmptyValue("port")) this.port = dic["port"].ToInt();
            if (dic.ContainsNonNullOrEmptyValue("is64bit")) this.is64bit = dic["is64bit"].Equals("1");
            if (dic.ContainsNonNullOrEmptyValue("bin")) this.bin = dic["bin"];
            if (dic.ContainsNonNullOrEmptyValue("exe")) this.exe = dic["exe"];
            if (dic.ContainsNonNullOrEmptyValue("num_instances")) this.numInstances = dic["num_instances"].ToInt();
            if (dic.ContainsNonNullOrEmptyValue("instance_num")) this.instanceNum = dic["instance_num"].ToInt();
            if (dic.ContainsNonNullOrEmptyValue("pid")) this.pid = dic["pid"];
        }

        /// <summary>
        /// Convert it to a dictionary
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["node_id"] = this.nodeIdStr;
            dic["server"] = this.machine;
            dic["port"] = this.port.ToString();
            dic["is64bit"] = this.is64bit ? "1" : "0";
            dic["is_master"] = this.isMaster ? "1" : "0";
            dic["is_profiler"] = this.isProfiler ? "1" : "0";
            dic["bin"] = this.bin;
            dic["exe"] = this.exe;
            dic["pid"] = this.pid;
            dic["num_instances"] = this.numInstances.ToString();
            dic["instance_num"] = this.instanceNum.ToString();
            return dic;
        }

        public MvmClusterNode(MvmClusterBase mvmCluster, SocketHandler socketHandler)
        {
            this.mvmCluster = mvmCluster;
            this.socketHandler = socketHandler;
            this.nodeId = this.socketHandler.clientNodeIdInt;
            this.machine = "unknown";
            this.bin = "";
            this.port = -1;
        }

        public bool IsConnected
        { 
            get{
                return this.socketHandler != null;
            }
        }
       
        public bool IsSuper
        {
            get
            {
                return this.nodeId == this.mvmCluster.SuperNode.nodeId;
            }
        }

        public void Disconnect()
        {
            if (this.IsConnected)
            {
                this.socketHandler.Disconnect();
            }
            else
            {
                logger.Debug("client " + this.nodeId + " was never connected, so nothing to disconnect");
            }
        }

        public static bool isLocalhost(string name)
        {
            return name.In("localhost", "127.0.0.1", "::1");
        }

        public static string ResolveLocalName(string name)
        {
            if (isLocalhost(name))
            {
                return System.Environment.MachineName;
            }
            return name;
        }

        // Perform the translation to IP address ourself, since TcpClient can take 3-4 seconds to resolve names
        public static string ResolveIPAddress(string name)
        {
            IPAddress[] addresslist = Dns.GetHostAddresses(name);
            foreach (var addr in addresslist)
            {
                // Take the first IPV4 address since IPV6 addresses aren't generally routable on the MT network
                if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return addr.ToString();
                }
            }
            return name;
        }

        public void ListenerSetSocketHandler(SocketHandler sh)
        {
            //this.mvm.Trace("LISTENER ListenerSetSocketHandler NODE_ID=" + sh.clientNodeIdInt.ToString());
            if (this.socketHandler != null)
            {
                this.mvm.Fatal("socket handler for clientNodeId=" + sh.clientNodeIdInt + " is expected to be null");
                throw new Exception("unexpected state 9988733");
            }
            this.socketHandler = sh;
            //this.mvm.Trace("LISTENER SET SOCKET HANDER FOR CLIENT NODE_ID=" + this.socketHandler.clientNodeIdInt.ToString());
        }

        public SocketHandler SocketHandler
        {
            get
            {
                // keep trying to connect.
                int maxAttempts=100;
                int numAttempts = 0;
                while (socketHandler == null)
                {
                    numAttempts++;
                    if (numAttempts > maxAttempts) throw new Exception("Fatal error cannot connect to nodeId=" + this.nodeId + " after " + numAttempts + " attempts");
                    logger.Debug("Get permission to connect to {0}", nodeId); 
                    if (this.mvmCluster.GetPermissionToConnect(this.nodeId, true))
                    {
                        logger.Debug("Granted permission to connect to {0}", nodeId);
                        if (this.Connect())
                        {
                            logger.Debug("Now connected to {0}", nodeId);
                            this.mvmCluster.RegisterConnection(this.nodeId);
                            return this.socketHandler;
                        }
                        else
                        {
                            this.mvmCluster.CancelConnection(this.nodeId);
                            logger.Info("Connection to " + nodeId + " was refused, attempt number" + numAttempts + ", max allowed " + maxAttempts);
                            // flip a coin to see if we sleep.
                            if (RandomUtils.RandomBool())
                            {
                                var sleepTime = RandomUtils.RandomNumber(1000, 5000);
                                logger.Info("Connection to " + nodeId + " was refused, I was picked as the sleeper: {0} ms"+sleepTime);
                                System.Threading.Thread.Sleep(sleepTime);
                            }
                            continue;
                        }
                    }
                    else
                    {
                        // the other end must be connecting to me, wait for that connection to happen
                        logger.Debug(nodeId+ " must be connecting to us, just wait for that to finish, " + numAttempts);
                        System.Threading.Thread.Sleep(RandomUtils.RandomNumber(1000, 10000));
                        continue;
                    }
                }
                return this.socketHandler;
            }
        }

        private bool Connect()
        {
            TcpClient tcpClient;
            try
            {
                string myMachine = MvmClusterNode.ResolveIPAddress(MvmClusterNode.ResolveLocalName(machine));
                tcpClient = new TcpClient(myMachine, port);
                tcpClient.NoDelay = true; // set this to true or client waits 1/2 sec before sending

                BinaryReader breader = new BinaryReader(tcpClient.GetStream());
                BinaryWriter bwriter = new BinaryWriter(tcpClient.GetStream());
                // Send your identity
                bwriter.Write(mvmCluster.NodeId);
                bwriter.Flush();
                // Read if you were approved.
                bool approved = breader.ReadBoolean();
                int newNodeId = breader.ReadInt32();
                if (!approved)
                {
                    tcpClient.Close();
                    return false;
                }
                if (mvmCluster.NodeId < 0 && this.IsSuper && newNodeId >= 0)
                {
                    // Use the new, valid node_id we were given by the super
                    mvmCluster.NodeId = newNodeId;
                    mvm.nodeId = newNodeId;
                }
                this.socketHandler = new SocketHandler(mvm, tcpClient, breader, bwriter, nodeId.ToString(), false);
                this.socketHandler.Start();
            }
            catch (Exception e)
            {
                throw new Exception("Cannot connect to mvm server [" + machine + "] port [" + port + "]", e);
            }
            return true;
        }

        public void ShutdownAbort(string reason)
        {
            if (this.IsConnected)
                this.socketHandler.messageSender.SendImmediate(new ShutdownAbortMessage(reason));
        }
    }
}
