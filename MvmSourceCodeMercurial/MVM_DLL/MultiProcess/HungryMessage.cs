//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.IO;
//using NLog;
//namespace MVM
//{

//    /// <summary>
//    /// Requests anything but expect a result either way.
//    /// </summary>
//    public class HungryMessage : MvmMessage
//    {
//        public static Logger logger = LogManager.GetCurrentClassLogger();
//        public override int Priority
//        {
//            get { return MessagePriority.HungryMessage; }
//        }
        
       
//        public HungryMessage(MvmMessageDeserializer dummy)
//            : base(dummy)
//        {
//        }

//        public HungryMessage()
//            : base()
//        {
//        }

//        protected HungryMessage(long messageId)
//            : base(messageId)
//        {
//        }

//        /// <summary>
//        /// Returns the message type
//        /// </summary>
//        public override byte MessageType { get { return HungryMessage.StaticMessageType; } }
//        public static readonly byte StaticMessageType = 21;


//        /// <summary>
//        /// Instanciate a new object from the buffer
//        /// </summary>
//        /// <param name="buffer"></param>
//        /// <returns></returns>
//        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
//        {
//            HungryMessage msg = new HungryMessage(messageId);
//            return msg;
//        }

//        /// <summary>
//        /// Returns buffer with message specific info
//        /// </summary>
//        /// <returns></returns>
//        protected override void SerializeMessagePayload(BinaryWriter bwriter)
//        {
//        }

//        public override void ProcessMessage(MessageReceiver receiver)
//        {
//            logger.Info("Received Hungry message from node {0}",receiver.socketHandler.clientNodeId);
//            receiver.mvmCluster.SetHungryBit(receiver.socketHandler.clientNodeIdInt);
//        }
//    }
//}
