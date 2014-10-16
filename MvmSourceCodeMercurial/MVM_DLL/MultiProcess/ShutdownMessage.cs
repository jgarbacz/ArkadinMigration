using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;
namespace MVM
{

    /// <summary>
    /// Requests anything but expect a result either way.
    /// </summary>
    public class ShutdownAbortMessage : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.ShutdownAbort; }
        }
        
       
        public ShutdownAbortMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public ShutdownAbortMessage(string messageText)
            : base()
        {
            this.messageText = messageText;
        }

        protected ShutdownAbortMessage(long messageId, string messageText)
            : base(messageId)
        {
            this.messageText = messageText;
        }


        public string messageText;

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return ShutdownAbortMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 15;


        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            string messageText = breader.ReadString();
            ShutdownAbortMessage msg = new ShutdownAbortMessage(messageId,messageText);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.messageText);
        }

        public override void ProcessMessage(MessageReceiver receiver)
        {
            receiver.mvm.Log("*".repeat(80));
            receiver.mvm.Log("* SHUTDOWN ABORT from node_id=[" + receiver.clientNodeId + "] reason=[" + this.messageText + "]");
            receiver.mvm.Log("*".repeat(80));
            
            // If we are on the super node, then take down all the kids before exiting.
            if (receiver.mvm.IsSuperNode)
            {
                receiver.mvm.ShutdownAbort("node_id [" + receiver.clientNodeId + "] errored.", receiver.clientNodeId.ToInt());
            }

            // If this is the profiler node, write any results we have
            if (receiver.mvmCluster.IsProfilerNode)
            {
                receiver.mvm.mvmClusterProfiler.WriteProfile();
            }

            receiver.mvm.FlushNLog();
            Environment.Exit(2);
        }
    }
}
