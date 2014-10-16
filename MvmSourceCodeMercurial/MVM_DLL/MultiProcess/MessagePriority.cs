using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public static class MessagePriority
    {
        // All these messages are processed only when the 
        // the worker thread is idle.
        public static readonly int WaitingProcCall = 1000;
        
        // This is the line in the sand where messages with lower priority
        // will not be handled until process is idle.
        public static readonly int Interupt = 2000;
        public static readonly int BundleMessage = Interupt;
        public static readonly int SetMasterNodeIdMessage = Interupt;
        public static readonly int ProfilerRequestMessage = Interupt;
        public static readonly int RcnRequestMessage = Interupt;
        public static readonly int RcnResponseMessage = Interupt;
        public static readonly int TfidUfnRequestMessage = Interupt;
        public static readonly int TfidUfnResponseMessage = Interupt;

        // These must all be the same priority.
        public static readonly int InteruptProcCall = 3000;
        public static readonly int UpdateClusterInfoMessage = InteruptProcCall;
        public static readonly int ReallocateSlavesMessage = InteruptProcCall;
        public static readonly int PushIndex = InteruptProcCall;
        public static readonly int QueueProcResultsMessage = InteruptProcCall;
       // public static readonly int HungryMessage = InteruptProcCall;
        public static readonly int SpecificNotFoundMessage = InteruptProcCall;
        public static readonly int SpecificRequestMessage = InteruptProcCall;
        public static readonly int AnyNotFoundMessage = InteruptProcCall;
        public static readonly int AnyRequestMessage = InteruptProcCall;
        public static readonly int RequestMessage = InteruptProcCall;

        // all these need to be same priority. 
        public static readonly int GetSlaveNodeIdsRequestMessage = 5000;
        public static readonly int NodeIdInfoRequestMessage = 5000;
        public static readonly int GetSlaveNodeIdsResponseMessage = 5000;
        public static readonly int NodeIdInfoResponseMessage = 5000;
        public static readonly int FormatIdRequestMessage = 5000;
        public static readonly int FormatIdResponseMessage = 5000;
        public static readonly int FormatFieldsRequestMessage = 5000;
        public static readonly int FormatFieldsResponseMessage = 5000;
        public static readonly int PortRequestMessage = 5000;
        public static readonly int PortResponseMessage = 5000;
        public static readonly int UsageRequestMessage = 5000;
        public static readonly int UsageResponseMessage = 5000;
        public static readonly int SlaveInitMessage = 5000;
        public static readonly int DbInfoRequestMessage = 5000;
        public static readonly int DbInfoResponseMessage = 5000;
        
        public static readonly int NotifyMaxPriority = 9998;

        
        public static readonly int DisconnectRequest=9999;
        public static readonly int StopReceivingMessage = 9999;
        public static readonly int ShutdownRequest = 9999;
        public static readonly int ShutdownResponse = 9999;
        public static readonly int Log = 99999;
        public static readonly int ShutdownAbort = 100000;
        // only thing that is sent immediate is from the outbox or a NotifyMaxPriority or Shutdown.
    }
}
