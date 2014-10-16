using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using NLog;

namespace MVM
{
    // This class accepts and services connections from other nodes in the cluster
    public class SlaveListener : BaseListener
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public readonly string machine = System.Environment.MachineName;
        private readonly List<SocketHandler> socketHandlers = new List<SocketHandler>();
        public readonly Dictionary<string, SocketHandler> socketHandlerMap = new Dictionary<string, SocketHandler>();
        public readonly MvmEngine mvm;
        public readonly MvmClusterBase mvmCluster;
        public int clientNo = 0;

        public SlaveListener(MvmEngine mvm, int portNo)
            : base(IPAddress.Any, portNo, "L" + mvm.nodeId.ToString())
        {
            this.mvm = mvm;
            this.mvmCluster = this.mvm.mvmCluster;
            logger.Debug("Kick off listener thread [{0}] on port [{1}]", this.thread.Name, this.port);
        }

        protected override void HandleClient(TcpClient tcpClient)
        {
            logger.Debug("Someone is initiating a connection");
            BinaryReader breader = new BinaryReader(tcpClient.GetStream());
            BinaryWriter bwriter = new BinaryWriter(tcpClient.GetStream());

            // Read the slave id
            int clientIdInt = breader.ReadInt32();
            string clientId = clientIdInt.ToString();

            // If I do not have permision to connect, then refuse the connection. 
            if (!this.mvmCluster.GetPermissionToConnect(clientIdInt, false))
            {
                logger.Debug("Listener does not have permission to connect to client with nodeId=" + clientIdInt + " so refuse the connection");
                RefuseConnection(bwriter, clientIdInt);
                return;
            }

            // If client does not have a node_id yet, assign a new one
            if (clientIdInt < 0)
            {
                if (this.mvmCluster.IsSuperNode)
                {
                    int newId = this.mvm.mvmClusterSuper.GetNextNodeId();
                    this.mvmCluster.RemapConnection(clientIdInt, newId);
                    clientIdInt = newId;
                    clientId = clientIdInt.ToString();
                }
                else
                {
                    logger.Error("Received connection attempt from invalid nodeId [" + clientId + "], which should not be possible!");
                    RefuseConnection(bwriter, clientIdInt);
                    return;
                }
            }

            // Let client know we've accepted their connection.
            bwriter.Write(true);
            bwriter.Write(clientIdInt);
            bwriter.Flush();
            this.clientNo += 1;

            logger.Debug("Connected to client with node_id=" + clientId + ", total connections=" + Convert.ToString(this.clientNo));
            SocketHandler clientSocketHandler = new SocketHandler(mvm, tcpClient, breader, bwriter, clientId, true);

            // register the socket handler with the cluster
            this.mvmCluster.ListenerAddSocket(clientSocketHandler);
            this.mvmCluster.RegisterConnection(clientIdInt);

            // start the handler
            clientSocketHandler.Start();
        }

        protected override void OnStopListener()
        {
            mvm.Log("Listener thread " + this.thread.Name + " exiting");
        }

        public void RefuseConnection(BinaryWriter bwriter, int nodeId)
        {
            bwriter.Write(false);
            bwriter.Write(nodeId);
            bwriter.Flush();
        }

        public SocketHandler LookupSocketHandler(string slaveId)
        {
            lock (this.socketHandlerMap)
            {
                if (this.socketHandlerMap.ContainsKey(slaveId)) return this.socketHandlerMap[slaveId];
                return null;
            }
        }

        public void StopAcceptingClients()
        {
            logger.Debug("Telling listener to stop");
            this.Stop();
            logger.Debug("Listener has stopped");
        }
    }
}
