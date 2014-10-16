using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
using System.Text.RegularExpressions;
using System.Threading;
namespace MVM
{

    /// <summary>
    /// This message requests port/machine for a node id. 
    /// </summary>
    public class GetSlaveNodeIdsRequestMessage : MvmMessage
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public override int Priority
        {
            get { return MessagePriority.GetSlaveNodeIdsRequestMessage; }
        }
        
       
        public GetSlaveNodeIdsRequestMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public GetSlaveNodeIdsRequestMessage(long workId,int requestCount,int masterId,string masterMachine,int masterPort)
            : base()
        {
            this.workId = workId;
            this.requestCount = requestCount;
            this.masterId = masterId;
            this.masterMachine = masterMachine;
            this.masterPort = masterPort;
        }

        protected GetSlaveNodeIdsRequestMessage(long messageId, long workId, int requestCount, int masterId, string masterMachine, int masterPort)
            : base(messageId)
        {
            this.workId = workId;
            this.requestCount = requestCount;
            this.masterId = masterId;
            this.masterMachine = masterMachine;
            this.masterPort = masterPort;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return GetSlaveNodeIdsRequestMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 9;

        public long workId;


        public int requestCount;
        public int masterId;
        public string masterMachine;
        public int masterPort;


        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            long workId = breader.ReadInt64();
            int requestCount = breader.ReadInt32();
            int masterId = breader.ReadInt32();
            string masterMachine = breader.ReadString();
            int masterPort = breader.ReadInt32();
            GetSlaveNodeIdsRequestMessage msg = new GetSlaveNodeIdsRequestMessage(messageId, workId,requestCount,masterId,masterMachine,masterPort);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.workId);
            bwriter.Write(this.requestCount);
            bwriter.Write(this.masterId);
            bwriter.Write(this.masterMachine);
            bwriter.Write(this.masterPort);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        public override void ProcessMessage(MessageReceiver receiver)
        {
            logger.Debug("get slave starter going...");
            // We better be on a super because only a super can launch a slave.
            MvmClusterSuper super = receiver.mvm.mvmClusterSuper;
            // Need to startup slaves in their own thread so we do not tie up this message receiver.
            SlaveStarter slaveStarter = new SlaveStarter(super, receiver.socketHandler, this.workId,this.requestCount, masterId, masterMachine, masterPort);
            ThreadStart starterThreadStart = new ThreadStart(slaveStarter.StartupSlaves);
            Thread starterThread = new Thread(starterThreadStart);
            starterThread.Name = "SlaveStarter";
            starterThread.Start();
        }

        public class SlaveStarter
        {
            MvmClusterSuper super;
            SocketHandler requestSocketHandler;
            long requestWorkId;
            int requestCount;
            int masterId;
            string masterMachine;
            int masterPort;
            public SlaveStarter(MvmClusterSuper super, SocketHandler requestSocketHandler, long requestWorkId, int requestCount, int masterId, string masterMachine, int masterPort)
            {
                this.super=super;
                this.requestSocketHandler = requestSocketHandler;
                this.requestWorkId = requestWorkId;
                this.requestCount = requestCount;
                this.masterId = masterId;
                this.masterMachine = masterMachine;
                this.masterPort = masterPort;
            }
            public void StartupSlaves(){
                try
                {
                    //super.StartupSlaves(requestSocketHandler, requestWorkId, requestCount, masterId, masterMachine, masterPort);
                    super.SetupMvmClusterSuper();
                    super.StartupNodes();
                }
                catch (Exception e)
                {
                    string msg="Error in slave starter thread ["+e.Message+"]" + e.GetStackTraceRecursive();
                    logger.Fatal(msg);
                    throw new Exception(msg,e);
                    
                }
            }
        }
    }
}
