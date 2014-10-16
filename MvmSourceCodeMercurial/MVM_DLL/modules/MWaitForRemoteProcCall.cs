using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace MVM
{

    /*
  
     */
    public class MWaitForRemoteProcCall : IModuleSetup, IModuleRun
    {
        public string processIdSyntax;
        public IReadString processIdParsed;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MWaitForRemoteProcCall m = new MWaitForRemoteProcCall();
            m.processIdSyntax = me.SelectNodeInnerText("./process_id");
            m.processIdParsed = mc.ParseSyntax(m.processIdSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string processId = this.processIdParsed.Read(mc);
            SocketAccesor socketAccessor = (SocketAccesor)mc.globalContext.GetNamedClassInst(processId);
            SocketHandler socketHandler = socketAccessor.GetSocketHandler(5000);
            
            // ok here we need to tell the receiver to send a request for a proc call and wake us up
            // when it gets one.
            
            // the message reciever sends the request

            // consume until we hit a proc call message. then call it.
            // telling b side that we'll accept a proc. 

            // the receiver needs to send 

           // string msg = "call_proc:" + proc;
           // socketHandler.messageOutbox.Send(new MvmMessage(msg),MessagePriority.ProcCall);
        }
    }
}
