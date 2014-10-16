using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Linq;
using NLog;
namespace MVM
{
    public class MSendRemoteResults : IModuleSetup, IModuleRun
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public string callerProcessIdSyntax;
        public IReadString callerProcessIdParsed;

        public string workIdSyntax;
        public IReadString workIdParsed;

        public string remoteProcNameSyntax;
        public IReadString remoteProcNameParsed;

        public string remoteProcNameSpaceSyntax;
        public IReadString remoteProcNameSpaceParsed;

        public string includeObjectFieldsInOutputSyntax;
        public IReadString includeObjectFieldsInOutputParsed;


        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MSendRemoteResults m = new MSendRemoteResults();
            m.callerProcessIdSyntax = "TEMP.caller_process_id";
            m.callerProcessIdParsed = mc.ParseSyntax(m.callerProcessIdSyntax);

            m.workIdSyntax = "TEMP.work_id";
            m.workIdParsed = mc.ParseSyntax(m.workIdSyntax);

            m.remoteProcNameSyntax = "TEMP.remote_proc_name";
            m.remoteProcNameParsed = mc.ParseSyntax(m.remoteProcNameSyntax);

            m.remoteProcNameSpaceSyntax = "TEMP.remote_proc_namespace";
            m.remoteProcNameSpaceParsed = mc.ParseSyntax(m.remoteProcNameSpaceSyntax);

            m.includeObjectFieldsInOutputSyntax = "TEMP.include_object_fields_in_output";
            m.includeObjectFieldsInOutputParsed = mc.ParseSyntax(m.includeObjectFieldsInOutputSyntax);

            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string callerProcessId = this.callerProcessIdParsed.Read(mc);
            string workIdStr = this.workIdParsed.Read(mc);
            long workId = long.Parse(workIdStr);
            string remoteProcNameSpace = this.remoteProcNameSpaceParsed.Read(mc);
            string remoteProcName = this.remoteProcNameParsed.Read(mc);
            
            bool includeObjectFieldsInOutput = this.includeObjectFieldsInOutputParsed.Read(mc).Equals("1");

            int nodeIdInt = callerProcessId.ToInt();
            SocketHandler socketHandler = mc.mvm.mvmCluster.GetClusterNode(nodeIdInt).SocketHandler;

 
            // create a dictionary for the output values
            Dictionary<string, string> outputDictionary = new Dictionary<string, string>();

            // decide if the object is to be treated as an output. if so, copy it in now.
            if (includeObjectFieldsInOutput)
            {
                foreach (var entry in mc.objectData.SelectExternalFields())
                {
                    outputDictionary[entry.Key] = entry.Value;
                }
            }

            // copy the proc output params into the output dictionary
            int remoteProcId = mc.schedulerMaster.GetProcId(remoteProcNameSpace, remoteProcName);
            ProcInfo remoteProcInfo = mc.schedulerMaster.GetProcInfo(remoteProcId);
            foreach (string outParam in remoteProcInfo.paramElems.Where(e=>e.GetAttribute("mode").In("out","in out")).Select(e=>e.GetAttribute("name"))){
                string outVal = mc.tempContext[outParam];
                outputDictionary[outParam] = outVal;
            }

            // wipe out the current cluster and all descendents
            //string clusterObjectId = mc.objectData["cluster_id"];
            //using (Cluster cluster = mc.objectCache.CheckOut(clusterObjectId))
            //{
            //    foreach (string memberOid in cluster.objectIds.Keys)
            //    {
            //        IObjectData x;
            //        //mc.mvm.Log("Purging object_id" + memberOid);
            //        mc.objectCache.TryRemoveObjectData(memberOid, out x);
            //    }
            //}

            // create and send the result message
            //logger.Debug("sending results to {0}", callerProcessId);
            QueueProcResultsMessage msg = new QueueProcResultsMessage(workId, WorkStatus.Complete, outputDictionary);
            socketHandler.messageOutbox.Add(msg, msg.Priority);

            
            // now decrement the ref counter so this objects gets cleaned up
            mc.objectData.RefRemove();
            
        }
    }
}
