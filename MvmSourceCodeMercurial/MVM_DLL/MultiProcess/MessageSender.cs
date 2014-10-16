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
    /// Class to send messages to a client.
    /// </summary>
    public class MessageSender
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly SocketHandler socketHandler;
        private readonly BinaryWriter bwriter;
        private readonly TcpClient clientSocket;
        public readonly string clientNodeId;
        public readonly MvmClusterSuper mvmClusterSuper;
        public MessageSender(SocketHandler socketHandler)
        {
            this.socketHandler = socketHandler;
            this.clientSocket = this.socketHandler.clientSocket;
            this.clientNodeId = this.socketHandler.clientNodeId;
            this.bwriter = this.socketHandler.bwriter;
            this.mvmClusterSuper = this.socketHandler.mvm.mvmClusterSuper;


            ms = new MemoryStream();
            bw = new BinaryWriter(ms);
        }


        MemoryStream ms ;
        BinaryWriter bw ;

        /// <summary>
        /// Thread safe to call by anyone
        /// </summary>
        public void SendImmediate(MvmMessage message)
        {
            lock (this)
            {
                // THIS SERIALIZES DIRECTLY TO THE NETWORK
                //message.Serialize(bwriter);
                //bwriter.Flush();

                // THIS SERIALIZES TO A MEMORY STREAM AND IN 1 CALL TO THE NETWORK
                //MemoryStream ms = new MemoryStream();
                //BinaryWriter bw = new BinaryWriter(ms);
                //message.Serialize(bw);
                //bw.Flush();
                //byte[] msgBuf = ms.ToArray();
                //bwriter.Write(msgBuf);
                //bwriter.Flush();


                // THIS SERIALIZES TO A MEMORY STREAM AND IN 1 CALL TO THE NETWORK
                ms.Position = 0;
                ms.SetLength(0);

                // prefix metadata update message first if needed
                if (mvmClusterSuper != null && this.clientNodeId.ToInt() != mvmClusterSuper.NodeId && !(message is TfidUfnResponseMessage))
                {
                    TfidUfnResponseMessage metaDataMsg = mvmClusterSuper.SendNewUfnsAndTfidsToNode(-1, this.clientNodeId.ToInt());
                    if (metaDataMsg != null)
                    {
                        metaDataMsg.Serialize(bw);
                    }
                }

                message.Serialize(bw);
                bw.Flush();
                bwriter.Write(ms.GetBuffer(), 0, (int)ms.Length);
                bwriter.Flush();

                //logger.Debug("sent msg to {0}, type {1} ", this.clientNodeId, message.GetType().Name); 
            }
        }

    }
}
