//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.IO;

//namespace MVM
//{
//    public class ReallocateSlavesMessage : MvmMessage
//    {
//        public override int Priority
//        {
//            get { return MessagePriority.ReallocateSlavesMessage; }
//        }
        
       
//        public ReallocateSlavesMessage(MvmMessageDeserializer dummy)
//            : base(dummy)
//        {
//        }

//        public ReallocateSlavesMessage(List<int> slaveNodes)
//            : base()
//        {
//            this.slaveNodeIds = slaveNodes;
//        }

//        protected ReallocateSlavesMessage(long messageId, List<int> slaveNodeIds)
//            : base(messageId)
//        {
//            this.slaveNodeIds = slaveNodeIds;
//        }

//        /// <summary>
//        /// Returns the message type
//        /// </summary>
//        public override byte MessageType { get { return ReallocateSlavesMessage.StaticMessageType; } }
//        public static readonly byte StaticMessageType = 34;


//        public List<int> slaveNodeIds = new List<int>();
       
//        /// <summary>
//        /// Instanciate a new object from the buffer
//        /// </summary>
//        /// <param name="buffer"></param>
//        /// <returns></returns>
//        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
//        {
//            List<int> slaveNodeIds = breader.ReadListOfInt32();
//            ReallocateSlavesMessage msg = new ReallocateSlavesMessage(messageId, slaveNodeIds);
//            return msg;
//        }

//        /// <summary>
//        /// Returns buffer with message specific info
//        /// </summary>
//        /// <returns></returns>
//        protected override void SerializeMessagePayload(BinaryWriter bwriter)
//        {
//            bwriter.Write(this.slaveNodeIds);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="receiver"></param>
//        public override void ProcessMessage(MessageReceiver receiver)
//        {
//            // Register on the cluster somewhere...
//            receiver.mvm.Log("I node_id=["+receiver.myNodeId+"] just recieved slaves ["+this.slaveNodeIds.JoinStrings(","));
//            receiver.mvmCluster.RegisterMySlaves(this.slaveNodeIds);
//        }
//    }
//}
