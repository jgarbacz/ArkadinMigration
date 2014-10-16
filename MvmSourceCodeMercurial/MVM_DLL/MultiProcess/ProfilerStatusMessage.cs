using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
using System.Threading;

namespace MVM
{
    /// <summary>
    /// Each node sends this message type periodically to the profiler node
    /// </summary>
    public class ProfilerStatusMessage : MvmMessage
    {
        public readonly List<List<StackFrame>> stackList;

        public override int Priority
        {
            get { return MessagePriority.InteruptProcCall; }
        }

        public ProfilerStatusMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        /// <summary>
        /// Constructor for a new message
        /// </summary>
        /// <param name="indexName"></param>
        public ProfilerStatusMessage(List<List<StackFrame>> stack)
            : base()
        {
            this.stackList = stack;
        }

        /// <summary>
        /// Constructor for deserialized message
        /// </summary>
        /// <param name="messageId"></param>
        protected ProfilerStatusMessage(long messageId, List<List<StackFrame>> stack)
            : base(messageId)
        {
            this.stackList = stack;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return ProfilerStatusMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 45;

        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            var stackList = new List<List<StackFrame>>();
            int count = breader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                List<StackFrame> stack = new List<StackFrame>();
                List<Dictionary<string, string>> dictList = breader.ReadListOfDictionaryOfStringString();
                foreach (var entry in dictList)
                {
                    stack.Add(new StackFrame(entry));
                }
                stackList.Add(stack);
            }
            return new ProfilerStatusMessage(messageId, stackList);
        }

        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.stackList.Count);
            foreach (var s in this.stackList)
            {
                List<Dictionary<string, string>> dictList = new List<Dictionary<string, string>>();
                foreach (var frame in s)
                {
                    dictList.Add(frame.ToDictionary());
                }
                bwriter.Write(dictList);
            }
        }

        // This will handle the profile status from the worker node
        public override void ProcessMessage(MessageReceiver receiver)
        {
            receiver.mvm.mvmClusterProfiler.AddToProfile(receiver.clientNodeId.ToInt(), this.stackList);
        }
    }
}
