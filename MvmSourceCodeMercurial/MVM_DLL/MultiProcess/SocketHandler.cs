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
    /// <summary>
    /// This class works for either client end or server end. It provides and way to send
    /// thing immediatly or to put them in the outbox. It also services reading from
    /// the other end.
    /// </summary>
    public class SocketHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public string myNodeId
        {
            get
            {
                return this.mvm.nodeId.ToString();
            }
        }
        
        public readonly MvmEngine mvm;
        public readonly TcpClient clientSocket;
        public readonly BinaryReader breader;
        public readonly BinaryWriter bwriter;
        public readonly string clientNo;
        public readonly string clientNodeId;
        public readonly int clientNodeIdInt;
        public readonly MessageReceiver messageReceiver;
        public readonly MessageSender messageSender;
        public readonly MessageOutbox messageOutbox;
        public readonly bool isServer;

        /// <summary>
        /// Constructor. Can be called from either the listener or from a worker thread initiating a connection.
        /// </summary>
        /// <param name="mvm"></param>
        /// <param name="tcpClient"></param>
        /// <param name="clientNodeId"></param>
        public SocketHandler(MvmEngine mvm, TcpClient tcpClient,BinaryReader breader,BinaryWriter bwriter, string clientNodeId,bool isServer)
        {
            this.mvm = mvm;
            this.clientSocket = tcpClient;
            this.breader = breader;
            this.bwriter = bwriter;
            this.clientNodeIdInt = clientNodeId.ToInt();
            this.clientNodeId = clientNodeId;
            this.isServer = isServer;
            this.messageSender = new MessageSender(this);
            this.messageOutbox = new MessageOutbox(this);
            this.messageReceiver = new MessageReceiver(this);
        }
        public void Start()
        {
            this.messageReceiver.StartReceiving();
        }


        # region disconnecting

        private object _disconnectLock = new object();
        private bool SentDisconnectRequest = false;
        // some external thread 
        // this could be by an external thread if i am the initiater
        // this could be the receiver thread if i am on the other end.
        public void SendDisconnectRequest()
        {
            lock (this._disconnectLock)
            {
                if (!this.SentDisconnectRequest)
                {
                    this.SentDisconnectRequest = true;
                    logger.Debug("Sending disconnect request to:" + this.clientNodeId);
                    this.messageSender.SendImmediate(new DisconnectRequest());
                }
            }
        }
        // This is always call from the message receiver
        public void ReceiveDisconnectRequest()
        {
            lock (this._disconnectLock)
            {
                if (!this.SentDisconnectRequest)
                {
                    //logger.Debug("Received INITIATING disconnect request from:" + this.clientNodeId);
                    this.SentDisconnectRequest = true;
                    //logger.Debug("Sending CONFIRMATION disconnect request to:" + this.clientNodeId);
                    this.messageSender.SendImmediate(new DisconnectRequest());
                }
                else
                {
                    //logger.Debug("Received CONFIRMATION disconnect request from:" + this.clientNodeId);
                }
                // tell the receiver to stop
                //logger.Debug("Stop receiving messages from:" + this.clientNodeId);
                this.messageReceiver.StopReceiving();
            }
        }

        /// <summary>
        /// Returns when the socket has been disconnected and the receiver is stopped.
        /// </summary>
        public void Disconnect()
        {
            this.SendDisconnectRequest();
            logger.Debug("waiting for receiver to exit");
            this.messageReceiver.messageReceiverThread.Join();
            logger.Debug("receiver has exited");
        }

        #endregion


        public int? MaxWaitingPriority
        {
            get
            {
                return this.messageReceiver.MaxWaitingPriority;
            }
        }
    }
}
