using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Security.Cryptography;
using NLog;

namespace MVM
{
    public class MvmClusterSuper : MvmClusterBase
    {
        public static int waitForSlavesTimeoutSecs = 60;

        public bool startupProfilerNode = false;

        public MvmClusterSuper(MvmEngine mvm, int nodeId, bool startupProfilerNode)
            : base(mvm, nodeId)
        {
            // Force TFID 0 to be: object_id, object_type
            if (GetTfid(new string[] { "object_id", "object_type" }) != 0)
            {
                throw new Exception("Error, forcing tfid 0");
            }
            this.startupProfilerNode = startupProfilerNode;
        }

        public void ServerCredentialsGet()
        {
            string query = "select server,username,password from mvm_server_credentials";
            var clusterNodesConfig = DbUtils.DbQueryToListOfDictionary(null, mvm.GetDefaultDbLogin(), query);
            foreach (var r in clusterNodesConfig)
            {
                string server = r.GetValueOrNull("server").Nvl(r["SERVER"]);
                string username = r.GetValueOrNull("username").Nvl(r["USERNAME"]);
                string epassword = r.GetValueOrNull("password").Nvl(r["PASSWORD"]);
                string password = TestCrypto.DecryptHexString(epassword);
                this.RegisterServerCredentials(server, username, password);
            }
        }

        // configured ports
        public enum PortStatus { Open, Active, Failed };
        public Dictionary<string, Dictionary<int, PortStatus>> MachinePortStatus = new Dictionary<string, Dictionary<int, PortStatus>>();

        // Start allocating node ids at 1 since 0 is reserved for the super
        private int NextNodeId = 1;

        /// <summary>
        /// Gets the next available nodeId
        /// </summary>
        public int GetNextNodeId()
        {
            return NextNodeId++;
        }

        /// <summary>
        /// Tells the super node to allow a node on the passed machine/port
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="port"></param>
        public MvmClusterNode RegisterConfiguredNode(Dictionary<string, string> nodeDictionary)
        {
            int nodeId = this.GetNextNodeId();
            nodeDictionary["node_id"] = nodeId.ToString();
            var node = new MvmClusterNode(this, nodeDictionary);
            this.AddKnownNode(node);
            return node;
        }

        public List<MvmClusterNode> GetFreeNodes()
        {
            List<MvmClusterNode> result = this.GetKnownNodes().Where(n => !n.active).ToList();
            result.ForEach(n => { n.active = true; });
            return result;
        }

        public void RegisterMachinePortRange(string machine, int portStart, int portEnd)
        {
            machine = machine.ToLower();
            lock (this.MachinePortStatus)
            {
                if (this.MachinePortStatus.ContainsKey(machine))
                    return;
                this.MachinePortStatus[machine] = new Dictionary<int, MvmClusterSuper.PortStatus>();
                for (int portNo = portStart; portNo <= portEnd; portNo++)
                {
                    this.MachinePortStatus[machine][portNo] = MvmClusterSuper.PortStatus.Open;
                }
            }
        }

        public Dictionary<string, Dictionary<string, string>> serverCredentials = new Dictionary<string, Dictionary<string, string>>();

        public bool GetServerLogin(string server, out string username, out string password)
        {
            username = null;
            password = null;
            if (!serverCredentials.ContainsKey(server)) return false;
            foreach (var entry in this.serverCredentials[server])
            {
                username = entry.Key;
                password = entry.Value;
                return true;
            }
            return false;
        }
        public void RegisterServerCredentials(string server, string username, string password)
        {

            lock (this.MachinePortStatus)
            {
                if (!serverCredentials.ContainsKey(server)) serverCredentials[server] = new Dictionary<string, string>();
                serverCredentials[server][username] = password;
            }
        }

        public void MarkPortFailure(string machine, int port)
        {
            machine = machine.ToLower();
            lock (this.MachinePortStatus)
            {
                this.MachinePortStatus[machine][port] = PortStatus.Failed;
            }
        }

        public override int GetFreePort(int clientNodeId, string clientMachine)
        {
            clientMachine = clientMachine.ToLower();
            lock (this.MachinePortStatus)
            {
                int port = -1;
                foreach (var entry in this.MachinePortStatus[clientMachine].Where(e => e.Value == PortStatus.Open))
                {
                    port = entry.Key;
                    break;
                }
                if (port == -1) return -1;
                this.MachinePortStatus[clientMachine][port] = PortStatus.Active;

                // updates the known nodes info if this is not the super.
                if (clientNodeId != this.NodeId)
                {
                    MvmClusterNode clusterNode;
                    if (this.TryGetKnownNode(clientNodeId, out clusterNode))
                    {
                        clusterNode.port = port;
                        clusterNode.machine = clientMachine;
                        //logger.Debug("Updated known info for node_id={0} port={1} machine={2}", clusterNode.nodeId, clusterNode.port, clusterNode.machine);
                    }
                    else
                    {
                        throw new Exception("Cannot request port info for an unknown node_id: " + clientNodeId);
                    }
                }

                return port;
            }
        }

        // Maps machine names to IP addresses to avoid slow name lookups
        // Not used any more, since the name lookup only seems to be slow when done within TcpClient
        public static Dictionary<string, string> serverNameMap = new Dictionary<string, string>();
        public static string ResolveNetworkName(string name)
        {
            name = name.ToLower();
            if (serverNameMap.ContainsKey(name))
            {
                return serverNameMap[name];
            }
            if (MvmClusterNode.isLocalhost(name))
            {
                name = System.Environment.MachineName.ToLower();
            }
            string ip = MvmClusterNode.ResolveIPAddress(name);
            if (!name.Equals(ip))
            {
                serverNameMap[name] = ip;
            }
            return ip;
        }

        public void ReadMvmClusterTable(string mvmClusterName)
        {
            // the mvm cluster is a super
            MvmClusterSuper superNode = this as MvmClusterSuper;

            // This is the xml configured or db specified cluster nodes.
            List<Dictionary<string, string>> clusterNodesConfig = new List<Dictionary<string, string>>();

            var dbInfo = this.mvm.GetDefaultDbLogin();

            string query = "select mvm_cluster_name,server,num_instances,port_start,port_end,bin,exe,group_id,is_master from mvm_clusters where mvm_cluster_name=" + mvmClusterName.q();
            clusterNodesConfig = DbUtils.DbQueryToListOfDictionary(dbInfo.type, dbInfo.server, dbInfo.db, dbInfo.user, dbInfo.pw, query);
            if (clusterNodesConfig.Count == 0)
            {
                throw new Exception("Error, did not find an entry in mvm_clusters on server=" + dbInfo.server + " db=" + dbInfo.db + " for mvm_cluster_name=[" + mvmClusterName + "]");
            }
            foreach (var r in clusterNodesConfig.OrderBy(x => x["server"]))
            {
                int numInstances = r["num_instances"].ToInt();
                string server = MvmClusterNode.ResolveLocalName(r["server"]);
                int portStart = r["port_start"].NotNullOrEmpty() ? r["port_start"].ToInt() : MvmClusterCommon.DefaultPortStart;
                int portEnd = r["port_end"].NotNullOrEmpty() ? r["port_end"].ToInt() : MvmClusterCommon.DefaultPortEnd;
                string bin = r["bin"].NotNullOrEmpty() ? r["bin"] : MvmEngine.ExecutableDir;
                string exe = r["exe"].NotNullOrEmpty() ? r["exe"] : MvmEngine.ExecutableName;
                superNode.RegisterMachinePortRange(server, portStart, portEnd);
                for (int instanceNum = 1; instanceNum <= numInstances; instanceNum++)
                {
                    // only the first instance can be master
                    if (instanceNum > 1) r["is_master"] = "0";
                    r["instance_num"] = instanceNum.ToString();
                    r["server"] = server;
                    r["port_start"] = portStart.ToString();
                    r["port_end"] = portEnd.ToString();
                    r["bin"] = bin;
                    r["exe"] = exe;
                    RegisterConfiguredNode(r);
                }
            }
            var masterNode = this.GetKnownNodes().Where(n => n.isMaster).FirstOrDefault();
            if (masterNode == null)
            {
                this.mvm.globalContext["master_id"] = this.NodeId.ToString(); // 0 is master
                mvm.Log("super node is also the master node]");
            }
            else
            {
                this.mvm.globalContext["master_id"] = masterNode.nodeId.ToString();
            }
            mvm.Log("master node is node_id=[" + this.mvm.globalContext["master_id"] + "]");
        }

        /// <summary>
        /// Only the super node can startup mvm nodes. This starts up any inactive nodes, makes sure they
        /// are alive and returns.
        /// </summary>
        public int StartupNodes()
        {
            List<int> slaveIds = new List<int>();
            Dictionary<string, List<string[]>> remoteCommands = new Dictionary<string, List<string[]>>();
            lock (this)
            {
                Dictionary<int, string> slaveCommands = new Dictionary<int, string>();
                List<MvmClusterNode> clusterNodes = this.GetFreeNodes();
                this.MaxNodeId += clusterNodes.Count;
                foreach (var node in clusterNodes)
                {
                    StartSingleNode(node, remoteCommands, slaveCommands);
                    slaveIds.Add(node.nodeId);
                } // end node id loop

                // Connect to all the remote launchers
                foreach (var remote in remoteCommands)
                {
                    string machine = remote.Key;
                    List<string[]> commands = remote.Value;
                    logger.Debug("Connecting to remote launcher on machine: " + machine);
                    try
                    {
                        string myMachine = MvmClusterNode.ResolveIPAddress(MvmClusterNode.ResolveLocalName(machine));
                        TcpClient tcpClient = new TcpClient(myMachine, MvmClusterCommon.LauncherPort);
                        tcpClient.NoDelay = true; // set this to true or client waits 1/2 sec before sending

                        BinaryReader breader = new BinaryReader(tcpClient.GetStream());
                        BinaryWriter bwriter = new BinaryWriter(tcpClient.GetStream());

                        SynchronizeConfig(bwriter, breader);

                        StartNodes(bwriter, commands);

                        // Signal we're done sending commands
                        bwriter.Write(ListenerCommandType.LastCommand.ToInt());

                        tcpClient.Close();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error communicating with remote mvm server [" + machine + "] port [" + MvmClusterCommon.LauncherPort + "]", e);
                    }
                }

                // Wait for all newly-started child nodes to connect to us
                Dictionary<int, ConnStatus> connStatus;
                if (!this.WaitForConnections(slaveIds, waitForSlavesTimeoutSecs, out connStatus))
                {
                    logger.Fatal("Timed out waiting for child nodes to start, timeout seconds=" + waitForSlavesTimeoutSecs);
                    foreach (var entry in connStatus.Where(e => e.Value != ConnStatus.Connected))
                    {
                        string cmd = slaveCommands[entry.Key];
                        logger.Fatal("node_id=" + entry.Key + ", last status=" + entry.Value.ToString() + ", launch command=[" + cmd + "]");
                    }
                    throw new Exception("TIMED OUT WAITING FOR CHILD NODES TO CONNECT TO SUPER");
                }
                else
                {
                    logger.Info("All child nodes have connected to us.");
                }

                // Wait for all child nodes to tell us they are initialized
                Dictionary<int, bool> initStatus;
                if (!this.WaitForInitializations(slaveIds, waitForSlavesTimeoutSecs, out initStatus))
                {
                    logger.Fatal("Timed out waiting for child nodes to initialize, timeout seconds=" + waitForSlavesTimeoutSecs);
                    foreach (var entry in initStatus.Where(e => !e.Value))
                    {
                        string cmd = slaveCommands[entry.Key];
                        logger.Fatal("node_id=" + entry.Key + ", launch command=[" + cmd + "]");
                    }
                    throw new Exception("TIMED OUT WAITING FOR CHILD NODES TO INITIALIZE");
                }
                else
                {
                    logger.Info("All child nodes have initialized.");
                }

                if (slaveIds.Count > 0)
                {
                    // push the updated node info to all nodes
                    PushNodeInfo();

                    if (this.GetKnownNodes().Where(n => n.isProfiler).ToList().Count > 0)
                    {
                        this.ProfileAllNodes();
                    }
                }
            }
            return slaveIds.Count;
        }

        // TODO: suicide thread on each slave node that kills self if haven't initialized/talked to super within a timeout

        private bool analyzed = false;
        public Dictionary<string, byte[]> fileHashes = new Dictionary<string, byte[]>();
        public Dictionary<string, byte[]> fileContents = new Dictionary<string, byte[]>();

        // Figures out what data every listener will need to have
        public void AnalyzeConfig()
        {
            if (analyzed)
            {
                return;
            }
            analyzed = true;

            // Currently the listener must have mvm xml config files,
            // -file/-xml arguments passed to mvm, and any linked binaries under RMP.
            SHA1 FileHasher = this.mvm.FileHasher;
            Dictionary<string, string> assemblyList = new Dictionary<string, string>();
            MvmClusterCommon.GetRMPAssemblies(MyReflection.GetAssembly(), assemblyList);
            foreach (var asm in assemblyList)
            {
                string pathRelativeToRMP = MvmClusterCommon.PathRelativeToRMP(this.mvm.rmpDir, asm.Value);
                byte[] fileBytes = File.ReadAllBytes(asm.Value);
                this.fileHashes[pathRelativeToRMP] = FileHasher.ComputeHash(fileBytes);
                this.fileContents[pathRelativeToRMP] = fileBytes;
            }
            foreach (var fhMap in this.mvm.XmlFileHashMap)
            {
                this.fileHashes[fhMap.Value.remoteFilename] = FileHasher.ComputeHash(fhMap.Value.data);
                this.fileContents[fhMap.Value.remoteFilename] = fhMap.Value.data;
            }
        }

        // Sync all config files with a remote machine
        public void SynchronizeConfig(BinaryWriter bwriter, BinaryReader breader)
        {
            AnalyzeConfig();

            // Send the command type
            bwriter.Write(ListenerCommandType.SynchronizeConfig.ToInt());

            // Send our machine name and run id for the listener to identify the sandbox
            bwriter.Write(MvmEngine.MachineName);
            bwriter.Write(this.mvm.globalContext["mvm_run_id"].ToInt());

            // Here are the synchronization steps:
            // - super sends a dict of filename/hash pairs that the listener needs to have
            // - remote listener replies with a subset dict of filename/hash pairs that it lacks
            // - super sends a dict of filename/data pairs
            // (all filenames here are relative to RMP)

            bwriter.Write(this.fileHashes);
            bwriter.Flush();

            List<string> neededByListener = breader.ReadListOfString();

            Dictionary<string, byte[]> sendToListener = new Dictionary<string, byte[]>();
            foreach (var f in neededByListener)
            {
                sendToListener[f] = this.fileContents[f];
            }
            bwriter.Write(sendToListener);
            bwriter.Flush();
        }

        // Send the remote commands
        public void StartNodes(BinaryWriter bwriter, List<string[]> commands)
        {
            // Send the command type
            bwriter.Write(ListenerCommandType.StartNodes.ToInt());

            // Send our machine name and run id for the listener to identify the sandbox
            bwriter.Write(MvmEngine.MachineName);
            bwriter.Write(this.mvm.globalContext["mvm_run_id"].ToInt());

            // Send the commands as a string array
            bwriter.Write(commands);
            bwriter.Flush();
        }

        public void StartSingleNode(MvmClusterNode node, Dictionary<string, List<string[]>> remoteCommands, Dictionary<int, string> slaveCommands)
        {
            // kick off processes for each slave
            this.slaveInitialized.Reset();

            string super_id = this.SuperNode.nodeIdStr;
            string super_machine = this.SuperNode.machine;
            string super_port = this.SuperNode.portStr;
            string slave_id = node.nodeId.ToString();
            string slave_machine = node.machine;
            string slave_user = "";
            string slave_password = "";
            bool hasPassedCredentials = GetServerLogin(node.machine, out slave_user, out slave_password);
            if (hasPassedCredentials)
            {
                logger.Debug("Connect via passed credentials");
            }
            else
            {
                logger.Debug("Connect with cached credentials");
            }

            string slaveArgs;
            if (node.isProfiler)
            {
                slaveArgs = " -profile -profiler_id=" + slave_id + " -profiler_machine=" + slave_machine;
                if (!this.mvm.globalContext["profiler_output_period"].Equals(""))
                {
                    slaveArgs += " -profiler_output_period=" + this.mvm.globalContext["profiler_output_period"];
                }
            }
            else
            {
                slaveArgs = " -slave_id=" + slave_id + " -slave_machine=" + slave_machine;
            }
            string nlog_file = this.mvm.globalContext["nlog_config"];
            string nlog_param = "nlog";
            string nlog_param_value = mvm.nlogConfigFile("NLog.slave.config");
            if (nlog_file.NotNullOrEmpty() && File.Exists(nlog_file))
            {
                nlog_param = "nlog_config";
                nlog_param_value = nlog_file;
            }
            string format = "-master_id={0} -super_id={1} -super_machine={2} -super_port={3} -mvm_run_id={4} -mvm_startup_date={5} -amp_local_usage_table={6} -{7}={8}";
            string executableArgs = String.Format(format,
                mvm.globalContext["master_id"],
                super_id,
                super_machine,
                super_port,
                mvm.globalContext["mvm_run_id"],
                mvm.globalContext["mvm_startup_date"].qq(),
                mvm.globalContext["amp_local_usage_table"],
                nlog_param,
                nlog_param_value
            );
            executableArgs += slaveArgs;
            if (this.mvm.globalContext["override_extensions_dir"].NotNullOrEmpty())
            {
                executableArgs += " \"-override_extensions_dir=" + this.mvm.globalContext["override_extensions_dir"] + "\"";
            }
            if (this.mvm.globalContext["data_directory"].NotNullOrEmpty())
            {
                executableArgs += " \"-data_directory=" + this.mvm.globalContext["data_directory"] + "\"";
            }

            // force log switches down to kid
            foreach (var f in mvm.globalContext.globalObjectData.FieldKeyValuesPairs)
            {
                if (f.Key.StartsWith("log_") && f.Value.Equals("1"))
                {
                    executableArgs += " -" + f.Key + "=1";
                }
            }

            // Is this node on our local machine?
            bool isLocalCommand = slave_machine.EqualsIgnoreCase(System.Environment.MachineName);
            string command;

            if (isLocalCommand)
            {
                string executable = node.ExeFullName;
                command = MvmClusterCommon.GetCommand(executable, executableArgs);

                logger.Debug("calling: " + command);
                foreach (var output in MvmClusterCommon.RunCommand(executable, executableArgs, true))
                {
                    mvm.Log("[shell output] " + output);
                }
            }
            else
            {
                string executable = node.exe;  // later we'll prefix the exe with the sandbox directory
                command = MvmClusterCommon.GetCommand(executable, executableArgs);

                // Queue the commands up and send them to the remote launcher(s) all at once
                logger.Debug("calling remotely: " + command);
                remoteCommands.GetAddValueDefaulted(slave_machine, new List<string[]>()).Add(new string[] { executable, executableArgs });
            }
            slaveCommands[node.nodeId] = command;
        }

        public void PushNodeInfo()
        {
            List<Dictionary<string, string>> listDic = new List<Dictionary<string, string>>();
            foreach (var n in this.GetKnownNodes())
            {
                listDic.Add(n.ToDictionary());
            }
            foreach (var n in this.GetKnownNodes())
            {
                if (n.nodeId == 0)
                    continue;
                n.SocketHandler.messageOutbox.Add(new UpdateClusterInfoMessage(listDic));
            }
        }

        public bool WaitForConnections(List<int> slaveIds, int waitForSlavesTimeoutSecs, out Dictionary<int, ConnStatus> connStatus)
        {
            // wait for the slaves to connect to super me.
            connStatus = new Dictionary<int, ConnStatus>();
            foreach (int nodeId in slaveIds)
            {
                connStatus[nodeId] = ConnStatus.Uknown;
            }
            int waitingOn = slaveIds.Count;
            logger.Debug("Waiting for " + waitingOn + " slaves to connect to me");
            DateTime timeout = DateTime.Now.AddSeconds(waitForSlavesTimeoutSecs);
            while (waitingOn > 0 && DateTime.Now < timeout)
            {
                int newlyConnected = 0;
                this.newConnection.Reset();
                var waitingEntries = connStatus.Where(e => e.Value != ConnStatus.Connected).ToList();
                foreach (var entry in waitingEntries)
                {
                    int slaveNodeId = entry.Key;
                    ConnStatus newStatus = this.GetConnStatus(slaveNodeId);
                    connStatus[slaveNodeId] = newStatus;
                    if (newStatus == ConnStatus.Connected)
                    {
                        logger.Debug("Slave id=" + slaveNodeId + " is up and connected to me");
                        waitingOn--;
                        newlyConnected += 1;
                    }
                }
                if (waitingOn == 0) break;
                if (newlyConnected == 0)
                {
                    this.newConnection.WaitOne(waitForSlavesTimeoutSecs * 1000);
                }
            }
            return waitingOn == 0;
        }

        public bool WaitForInitializations(List<int> slaveIds, int waitForSlavesTimeoutSecs, out Dictionary<int, bool> initStatus)
        {
            // wait for the slaves to connect to super me.
            initStatus = new Dictionary<int, bool>();
            foreach (int nodeId in slaveIds)
            {
                initStatus[nodeId] = false;
            }
            int waitingOn = slaveIds.Count;
            logger.Debug("Waiting for " + waitingOn + " child nodes to initialize");
            DateTime timeout = DateTime.Now.AddSeconds(waitForSlavesTimeoutSecs);
            while (waitingOn > 0 && DateTime.Now < timeout)
            {
                int newlyInitialized = 0;
                this.slaveInitialized.Reset();
                var waitingEntries = initStatus.Where(e => !e.Value).ToList();
                foreach (var entry in waitingEntries)
                {
                    int slaveNodeId = entry.Key;
                    bool initialized = this.IsSlaveInitialized(slaveNodeId);
                    initStatus[slaveNodeId] = initialized;
                    if (initialized)
                    {
                        logger.Debug("Child node=" + slaveNodeId + " is initialized");
                        waitingOn--;
                        newlyInitialized += 1;
                    }
                }
                if (waitingOn == 0) break;
                if (newlyInitialized == 0)
                {
                    this.slaveInitialized.WaitOne(waitForSlavesTimeoutSecs * 1000);
                }
            }
            return waitingOn == 0;
        }

        public void Shutdown()
        {
            // Go no further if we've already sent out an abort message, as the workers are
            // shutting down and there's nothing to wait for.
            if (this.mvm.shutdownAbortInProcess)
            {
                return;
            }

            List<MvmClusterNode> workerNodes = this.GetWorkerNodes();

            // First shutdown all my worker nodes
            if (workerNodes.Count > 0)
            {
                logger.Info("Shutting down [" + workerNodes.Count + "] worker nodes");

                // start a batch to shutdown all the worker nodes
                long batchId = this.mvm.remoteWorkMgr.CreateBatch();
                WorkBatch batch = this.mvm.remoteWorkMgr.LookupBatch(batchId);

                // send shutdown message to all of them
                foreach (MvmClusterNode workerNode in workerNodes)
                {
                    // Need to wait for a response so do this through the work manager
                    WorkInfo w = this.mvm.remoteWorkMgr.CreateWork(batchId);
                    w.procName = "Shutdown";
                    w.nodeId = workerNode.nodeId;
                    w.priority = MessagePriority.Interupt;
                    w.status = WorkStatus.WaitingToStart;
                    // create and send the message.
                    ShutdownRequest msg = new ShutdownRequest(w.workId, this.NodeId);
                    //logger.Info("Sending shutdown message to node id=[" + workerNode.nodeId + "] workId=[" + w.workId+"]");
                    workerNode.SocketHandler.messageOutbox.Add(msg);
                }

                // Wait for the batch to complete
                BlockingWaitBatchEvent blockingWait = new BlockingWaitBatchEvent();
                batch.AddBatchCompleteEvent(blockingWait);

                logger.Info("Waiting for worker nodes to respond that they have shutdown");
                blockingWait.WaitForBatchComplete();
                NLog.LogManager.Flush();
                logger.Info("All worker nodes are now shutdown");
            }
            else
            {
                logger.Info("Supernode has no worker nodes to shutdown");
            }

            if (this.SelfNode != null)
            {
                logger.Info("Disconnect the socket where we talk to ourself");
                this.SelfNode.Disconnect();
            }

            logger.Info("Shutdown nodes complete");
        }


        # region Track Slave Initialization
        // initializedSlaves[node_id]=true when slave has finished its global initialization and is idle.
        private AutoResetEvent slaveInitialized = new AutoResetEvent(false);
        public Dictionary<int, bool> initializedSlaves = new Dictionary<int, bool>();
        public bool IsSlaveInitialized(int slaveNodeId)
        {
            lock (initializedSlaves)
            {
                return initializedSlaves.ContainsKey(slaveNodeId);
            }
        }
        public void SetSlaveInitialized(int slaveNodeId, Dictionary<string, string> properties)
        {
            lock (initializedSlaves)
            {
                slaveInitialized.Set();
                initializedSlaves[slaveNodeId] = true;
                MvmClusterNode slaveNode = this.GetClusterNode(slaveNodeId);
                slaveNode.UpdateWithDictionary(properties);
            }
        }
        # endregion

        #region UFNs and TFIDs

        /// <summary>
        /// Given a feedback name return its feedbackid
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public override int GetFeedbackId(string feedbackName)
        {
            return this.GetCreateFeedbackId(feedbackName);
        }

        /// <summary>
        /// Given a feedbackId retun its feedbackName
        /// </summary>
        /// <param name="ufn"></param>
        /// <returns></returns>
        public override string GetFeedbackName(int feedbackId)
        {
            return FeedbackEncoder.GetItem(feedbackId);
        }


        /// <summary>
        /// Given a fieldName return its UFN
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public override int GetUfn(string fieldName)
        {
            return this.GetCreateUfn(fieldName);
        }

        /// <summary>
        /// Given a UFN retun its fieldName
        /// </summary>
        /// <param name="ufn"></param>
        /// <returns></returns>
        public override string GetFieldName(int ufn)
        {
            return UfnEncoder.GetItem(ufn);
        }

        public class SlaveLast
        {
            public int lastFeedbackId = -1;
            public int lastUfn = -1;
            public int lastTfid = -1;
        }
        // slaveLast[slave_node_id]= last values the slave knows about.
        public List<SlaveLast> slaveNodeIdLastValues = new List<SlaveLast>();

        /// <summary>
        /// Packs up new TFID and UFN info after the lastTfid and last Ufn.
        /// </summary>
        /// <param name="lastUfn"></param>
        /// <param name="lastTfid"></param>
        /// <param name="firstUfn"></param>
        /// <param name="ufnFieldNames"></param>
        /// <param name="firstTfid"></param>
        /// <param name="tfidUfnArrayMaps"></param>
        public TfidUfnResponseMessage SendNewUfnsAndTfidsToNode(long workId, int slaveNodeId)
        {
            bool foundDeltas = false;
            int firstFeedbackId;
            string[] feedbackNames;
            int firstUfn;
            string[] ufnFieldNames;
            int firstTfid;
            int[] tfidUfnArrayMaps;

            // Lookup the last thing we sent this node
            SlaveLast last;
            lock (slaveNodeIdLastValues)
            {
                for (int i = slaveNodeIdLastValues.Count; i <= slaveNodeId; i++)
                {
                    slaveNodeIdLastValues.Add(new SlaveLast());
                }
                last = slaveNodeIdLastValues[slaveNodeId];
            }

            lock (last)
            {
                // pack up feedbackIds
                {
                    int maxFeedbackId = this.FeedbackEncoder.MaxIndex;
                    if (last.lastFeedbackId < maxFeedbackId)
                    {
                        firstFeedbackId = last.lastFeedbackId + 1;
                        feedbackNames = this.FeedbackEncoder.GetItems(firstFeedbackId);
                        last.lastFeedbackId = maxFeedbackId;
                        foundDeltas = true;
                    }
                    else
                    {
                        firstFeedbackId = -1;
                        feedbackNames = new string[] { };
                    }
                }

                // pack up ufns
                {
                    int maxUfn = this.UfnEncoder.MaxIndex;
                    if (last.lastUfn < maxUfn)
                    {
                        firstUfn = last.lastUfn + 1;
                        ufnFieldNames = this.UfnEncoder.GetItems(firstUfn);
                        last.lastUfn = maxUfn;
                        foundDeltas = true;
                    }
                    else
                    {
                        firstUfn = -1;
                        ufnFieldNames = new string[] { };
                    }
                }


                // pack up [tfid, #fields, ufn1, idx1, ufn2, idx2...]...
                {
                    lock (TfidUfnIdxMap)
                    {
                        int maxTfid = this.TfidUfnIdxMap.Count - 1;
                        if (last.lastTfid < maxTfid)
                        {

                            List<int> tfidUfnArrayMapsList = new List<int>();
                            for (int tfid = last.lastTfid + 1; tfid <= maxTfid; tfid++)
                            {
                                tfidUfnArrayMapsList.Add(tfid);
                                int numFields = this.GetTfidFields(tfid).Length;
                                tfidUfnArrayMapsList.Add(numFields);
                                int[] ufns = this.GetTfidUfns(tfid);
                                tfidUfnArrayMapsList.AddRange(ufns);
                            }
                            firstTfid = last.lastTfid + 1;
                            tfidUfnArrayMaps = tfidUfnArrayMapsList.ToArray();
                            last.lastTfid = maxTfid;
                            foundDeltas = true;
                        }
                        else
                        {
                            firstTfid = -1;
                            tfidUfnArrayMaps = new int[] { };
                        }
                    }
                }
                // if we got anything or if a response is required send it.
                if (workId >= 0 || foundDeltas)
                {
                    //logger.Info("PACK: firstFeedbackId=" + firstFeedbackId + "=[" + feedbackNames.JoinStrings(",") + "]");
                    //logger.Info("PACK: firstUfn=" + firstUfn + "=[" + ufnFieldNames.JoinStrings(",") + "]");
                    //logger.Info("PACK: firstTfid=" + firstTfid + "=[" + tfidUfnArrayMaps.JoinStrings(",") + "]");
                    TfidUfnResponseMessage response = new TfidUfnResponseMessage(workId, firstFeedbackId, feedbackNames, firstUfn, ufnFieldNames, firstTfid, tfidUfnArrayMaps);
                    return response;
                }

                return null;
            }
        }

        /// <summary>
        /// Returns the field index for a tfid/ufn. -1 means field not found.
        /// </summary>
        /// <param name="tfid"></param>
        /// <param name="ufn"></param>
        /// <returns></returns>
        public override int GetFormatFieldIdx(int tfid, int ufn)
        {
            lock (TfidUfnIdxMap)
            {
                //Debug.Assert(tfid < TfidUfnIdxMap.Count);
                int[] ufnIdxMap = TfidUfnIdxMap[tfid];
                if (ufn >= ufnIdxMap.Length) return -1;
                return ufnIdxMap[ufn];
            }
        }

        /// <summary>
        /// Returns a array that maps UFN->fieldIndex
        /// </summary>
        /// <param name="tfid"></param>
        /// <returns></returns>
        public override int[] GetUfnIdxMap(int tfid)
        {
            lock (TfidUfnIdxMap)
            {
                //Debug.Assert(tfid < TfidUfnIdxMap.Count);
                return TfidUfnIdxMap[tfid];
            }
        }


        public override int GetTfid(string[] fieldNames)
        {
            return this.GetCreateTfid(fieldNames);
        }

        /// <summary>
        /// Given a tfid, return the field names.
        /// </summary>
        /// <param name="tfid"></param>
        /// <returns></returns>
        public override string[] GetTfidFields(int tfid)
        {
            return TfidEncoder.GetItem(tfid).array;
        }


        #endregion


        #region Relative Change Number (RCN) batching

        /// <summary>
        /// This is the Relative Change Number (RCN) lock. 
        /// </summary>
        private object rcnBatchLock = new object();
        /// <summary>
        /// Next time someone request an RCN batch this is the first rcn they get.
        /// </summary>
        private long nextRcnBatchStart = 1;

        /// <summary>
        /// Gets an rcn batch as defined by the first rcn 'batchStart' and batchSize which
        /// indicates the number of rcn's allotted.
        /// </summary>
        /// <param name="reqBatchSize"></param>
        /// <param name="batchStart"></param>
        /// <param name="batchSize"></param>
        public override void GetRcnBatch(int reqBatchSize, out long batchStart, out int batchSize)
        {
            batchSize = reqBatchSize;
            lock (rcnBatchLock)
            {
                batchStart = nextRcnBatchStart;
                nextRcnBatchStart += batchSize;
            }
        }

        #endregion

        /// <summary>
        /// Override the base version since the super node never needs to send a message to himself.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public override MvmClusterNode GetClusterNode(int nodeId)
        {
            if (this.NodeId == nodeId)
                return this.SuperNode;
            MvmClusterNode clusterNode;
            if (this.TryGetKnownNode(nodeId, out clusterNode))
            {
                return clusterNode;
            }
            throw new Exception("Error, unknown node_id=" + nodeId + "]");
        }

        public List<MvmClusterNode> GetWorkerNodes()
        {
            // Used in Shutdown() -- this will include the profiler
            return this.GetKnownNodes().Where(n => !n.IsSuper).ToList();
        }

        /// <summary>
        ///  Called once by the SuperNode
        /// </summary>
        public void SetupMvmClusterSuper()
        {
            lock (this)
            {
                if (this.MyListener != null) return;

                string machineName = System.Environment.MachineName;
                string bin = MvmEngine.ExecutableDir;
                string is64bit = IntPtr.Size == 8 ? "1" : "0";

                if (this.SuperNode == null)
                {
                    // the super node is the starting node, so it is not defined in the mvm cluster table.
                    this.SuperNode = new MvmClusterNode(
                        this,
                        new Dictionary<string, string>{
                        {"node_id",this.NodeId.ToString()}, 
                        {"server",machineName},
                        {"bin",bin},
                        {"is64bit",is64bit},
                        {"is_profiler","0"},
                        {"num_instances","1"},
                        {"instance_num","1"},
                        {"pid",MvmEngine.ProcessId.ToString()}
                    }
                    );

                    // try all our ports until one works
                    for (; ; )
                    {
                        try
                        {
                            this.SuperNode.port = this.Port = this.GetFreePort(this.SuperNode.nodeId, machineName);
                            if (this.SuperNode.port < 0)
                                throw new Exception("Error, cannot setup supernode - no ports available for:" + machineName);
                            this.MasterNode = this.SuperNode;
                            this.MyListener = new SlaveListener(this.mvm, this.SuperNode.port);
                        }
                        catch
                        {
                            this.MarkPortFailure(machineName, this.SuperNode.port);
                            logger.Warn("Cannot open port {0} on machine {1} so try another port.", this.SuperNode.port, machineName);
                            continue;
                        }
                        logger.Debug("Super is listening on port {0}", this.SuperNode.port);
                        break;
                    }
                }

                if (this.startupProfilerNode)
                {
                    Dictionary<string, string> profilerDict = new Dictionary<string, string>{
                        {"server", machineName},
                        {"bin", bin},
                        {"exe", MvmEngine.ExecutableName},
                        {"is64bit", IntPtr.Size == 8 ? "1" : "0"},
                        {"is_profiler", "1"}
                    };
                    RegisterConfiguredNode(profilerDict);
                    this.startupProfilerNode = false;
                }
            }
        }

        public void ShutdownAbort(string reason, params int[] skipNodeIds)
        {
            foreach (var node in this.GetKnownNodes())
            {
                if (skipNodeIds == null || node.nodeId.NotIn(skipNodeIds))
                {
                    this.mvm.Log("Sending shutdown abort to node: " + node.nodeId);
                    node.ShutdownAbort(reason);
                }
            }
        }
    }
}
