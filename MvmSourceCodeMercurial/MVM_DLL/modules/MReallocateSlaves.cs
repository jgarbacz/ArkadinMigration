//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Xml;
//using System.Threading;
//using System.Net.Sockets;
//using System.Net;
//using System.IO;
//using NLog;
//namespace MVM
//{

//    /*
   
//  <reallocate_slaves>
//  <to_node_id>localhost</to_node_id>
//  </reallocate_slaves>
//    */
//    public class MReallocateSlaves : IModuleSetup, IModuleRun
//    {
//        public string toNodeIdSyntax;
//        public IReadString toNodeIdParsed;
      
//        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
//        {
//            MReallocateSlaves m = new MReallocateSlaves();
//            m.toNodeIdSyntax = me.SelectNodeInnerText("./to_node_id");
//            m.toNodeIdParsed = mc.ParseSyntax(m.toNodeIdSyntax);
//            run.Add(m);
//        }

//        public void Run(ModuleContext mc)
//        {
//            string toNodeId=this.toNodeIdParsed.Read(mc);
//            if(toNodeId==null)
//                throw new Exception("Error, to_node_id must not be null");
//            // only the super node 0 can startup slaves.
//            MvmClusterSuper superNode = mc.mvmCluster as MvmClusterSuper;
//            if (superNode != null)
//            {

//                superNode.ReallocateSlaves(toNodeId.ToInt());
//            }
//            else
//            {
//                throw new Exception("for now, expecting reallocate_slaves to only be called one time from the super node");
//            }
//        }
//    }
//}
