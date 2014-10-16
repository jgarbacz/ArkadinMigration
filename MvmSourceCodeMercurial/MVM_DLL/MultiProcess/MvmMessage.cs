using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Runtime.CompilerServices;
namespace MVM
{
    /// <summary>
    /// This is a dummy class 
    /// </summary>
    public class MvmMessageDeserializer
    {
    }

    /// <summary>
    /// Base class for all mvm messages. Assigns messageId for new messages.
    /// </summary>
    public abstract class MvmMessage
    {
        /// <summary>
        /// Counter for messages. Assume we'll never go over a long.
        /// </summary>
        private static long nextMessageId = 0;

        /// <summary>
        /// Unique id for the message
        /// </summary>
        public long MessageId;

        /// <summary>
        /// Priority for the message
        /// </summary>
        public abstract int Priority { get; }

        /// <summary>
        /// Estimates the message size in bytes so that the outbox has the ability to block when there too many messages over a certain size.
        /// </summary>
        public virtual int EstimatedMessageBytes { get { return 0; } }

        /// <summary>
        /// Constructor so we can instanciate a dummy version of this class for
        /// deserializing purposes only. The message it self is not valid, only 
        /// they object it deserializes is.
        /// </summary>
        public MvmMessage(MvmMessageDeserializer nullDummy)
        {
        }

        /// <summary>
        /// Constructor for new messages
        /// </summary>
        public MvmMessage()
        {
            this.MessageId = nextMessageId++;
        }

        /// <summary>
        /// Constructor for deserialized messages
        /// </summary>
        protected MvmMessage(long messageId)
        {
            this.MessageId = messageId;
        }

        /// <summary>
        /// Returns a buffer to be send over the socket
        /// </summary>
        /// <returns></returns>
        public void Serialize(BinaryWriter bwriter)
        {
            bwriter.Write(this.MessageType);
            bwriter.Write(this.MessageId);
            this.SerializeMessagePayload(bwriter);
            bwriter.Flush();
        }

        /// <summary>
        /// Reads the begining of the buffer to get its message type.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte GetMessageType(BinaryReader breader)
        {
            return breader.ReadByte();
        }

        /// <summary>
        /// Takes in buffer from socket and returns an object buffer doesn't
        /// include the messagetype or messageid.
        /// </summary>
        /// <returns></returns>
        public MvmMessage Deserialize(MessageReceiver receiver, BinaryReader breader)
        {
            long messageId = breader.ReadInt64();
            MvmMessage msg = DeserializeMessagePayload(receiver,messageId, breader);
            return msg;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public abstract byte MessageType { get; }

        /// <summary>
        /// Instanciate a new object from the buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected abstract MvmMessage DeserializeMessagePayload(MessageReceiver receiver,long messageId, BinaryReader breader);

        /// <summary>
        /// Returns buffer with message specific info
        /// </summary>
        /// <returns></returns>
        protected abstract void SerializeMessagePayload(BinaryWriter bwriter);


        /// <summary>
        /// The receiver calls this method when it receives it.
        /// </summary>
        /// <param name="receiver"></param>
        public abstract void ProcessMessage(MessageReceiver receiver);
        
        /// <summary>
        /// My testing to be sure we can freeze/thaw and get the same results
        /// </summary>
        public static void Test()
        {

            MemoryStream strm = new MemoryStream();
            BinaryWriter bwriter = new BinaryWriter(strm);
            BinaryReader breader = new BinaryReader(strm);
            
            

            //MvmRequestLogMessage reqLog = new MvmRequestLogMessage();
            
            //Console.WriteLine("sending reqLog [" + reqLog.Serialize()+ "]");

            //string readBuf;
            //readBuf= reqLog.Serialize();
            //MvmRequestLogMessage reqLog2 = reqLog.Deserialize(readBuf) as MvmRequestLogMessage;
            //Console.WriteLine("received reqLog2 [" + reqLog2.Serialize() + "]");


            //MvmResponseLogMessage resLog = new MvmResponseLogMessage(reqLog2.MessageId, "here is the log stuff");
            //Console.WriteLine("sending resLog [" + resLog.Serialize() + "]");

            //readBuf = resLog.Serialize();
            //MvmResponseLogMessage resLog2 = resLog.Deserialize(readBuf) as MvmResponseLogMessage;
            //Console.WriteLine("received resLog2 [" + resLog2.Serialize() + "]");
        }
    }
}
