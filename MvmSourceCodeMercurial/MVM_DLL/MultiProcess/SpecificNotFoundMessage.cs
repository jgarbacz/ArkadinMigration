using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{
    /// <summary>
    /// Requests a message with a specific priority. The receiver will
    /// respond with a matching message or SpecificNotFoundMessage.
    /// </summary>
    public class SpecificNotFoundMessage : MvmMessage
    {
        public override int Priority
        {
            get { return MessagePriority.SpecificNotFoundMessage; }
        }
        
        public int SpecificPriority { get; set; }

        public SpecificNotFoundMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        public SpecificNotFoundMessage(int minPriority)
            : base()
        {
            this.SpecificPriority=minPriority;
        }

        protected SpecificNotFoundMessage(long messageId,int specificPriority)
            : base(messageId)
        {
            this.SpecificPriority = specificPriority;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return SpecificNotFoundMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 17;


        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            int minPriority = breader.ReadInt32();
            SpecificNotFoundMessage msg = new SpecificNotFoundMessage(messageId, minPriority);
            return msg;
        }

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(SpecificPriority);
        }

        public override void ProcessMessage(MessageReceiver receiver)
        {
            receiver.AddInteruptPriorityOverride(this.SpecificPriority);
        }
    }
}
