using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    /// <summary>
    /// We want to treat the client and server the same form the SocketHandler point of view but 
    /// we need to lockup the socket handler if we're on the server since we don't have that hook
    /// until the client connects. This class provided a consistent way to get the sockethandler
    /// for either the client or server.
    /// </summary>
    public class SocketAccesor
    {
        private SocketHandler socketHandler;
        public SocketHandler handler
        {
            get
            {
                if (socketHandler != null) return socketHandler;
                if (this.listener != null)
                {
                    return this.listener.LookupSocketHandler(slaveId);
                }
                throw new Exception("Unexpected, null socketHandler and null listener");
            }
        }
       /// <summary>
       /// Returns the SocketHandler, will keep checking for timeoutMs before erroring.
       /// </summary>
       /// <param name="timeoutMs"></param>
       /// <returns></returns>
        public SocketHandler GetSocketHandler(int timeoutMs){
            var h=this.handler;
            if (h != null) return h;
            var start=System.DateTime.Now;
            while (System.DateTime.Now.Subtract(System.DateTime.Now).TotalMilliseconds < timeoutMs)
            {
                h = this.handler;
                if (h != null) return h;
            }
            throw new Exception("Error timeout trying waiting for slave to connect");
        }

        /// <summary>
        /// Used by the client since the client always has the hook to the SocketHandler
        /// </summary>
        /// <param name="handler"></param>
        public SocketAccesor(SocketHandler socketHandler)
        {
            this.socketHandler = socketHandler;
        }


        private SlaveListener listener;
        private string slaveId;

        /// <summary>
        /// Used by the server since the server needs to lookup the hook to the SocketHandler after
        /// the client has connected.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="slaveId"></param>
        public SocketAccesor(SlaveListener listener,string slaveId)
        {
            this.listener = listener;
            this.slaveId = slaveId;
        }
    }
}
