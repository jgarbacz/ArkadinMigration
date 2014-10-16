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
    public class MRemoteQueueProc : IModuleSetup, IModuleRun
    {
        public string procSyntax;
        public IReadString procParsed;

        public string processIdSyntax;
        public IReadString processIdParsed;

        public string batchIdSyntax;
        public IReadString readBatchIdParsed;
        public IWriteString writeBatchIdParsed;

        public string objectIdSyntax;
        public IReadString objectIdParsed;

        public string remoteObjectTypeSyntax;
        public IReadString remoteObjectTypeParsed;

        public string includeObjectFieldsInOutputSyntax;
        public IReadString includeObjectFieldsInOutputParsed;

        public bool hasParams;
        public List<IReadString> inputReads = new List<IReadString>();
        public List<string> inputParams = new List<string>();

        public ProcInfo procInfo;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            mc.mvm.StartupCluster();
            MRemoteQueueProc m = new MRemoteQueueProc();
            m.processIdSyntax = me.SelectNodeInnerText("./process_id");
            m.processIdParsed = mc.ParseSyntax(m.processIdSyntax);
            m.procSyntax = me.SelectNodeInnerText("./name");
            m.procParsed = mc.ParseSyntax(m.procSyntax);

            m.includeObjectFieldsInOutputSyntax = me.SelectNodeInnerText("./include_object_fields_in_output", "0");
            m.includeObjectFieldsInOutputParsed = mc.ParseSyntax(m.includeObjectFieldsInOutputSyntax);

            XmlElement remoteObjectTypeElem = me.SelectSingleElem("./new_remote_object_type");
            if (remoteObjectTypeElem != null)
            {
                m.remoteObjectTypeSyntax = remoteObjectTypeElem.InnerText;
                m.remoteObjectTypeParsed = mc.ParseSyntax(m.remoteObjectTypeSyntax);
            }

            XmlElement objectIdElem = me.SelectSingleElem("./object_id");
            if (objectIdElem != null)
            {
                m.objectIdSyntax = objectIdElem.InnerText;
                m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            }

            // batch id which if set uses  existing batch id. if not set sets it to a guid.
            m.batchIdSyntax = me.SelectNodeInnerText("./batch_id");
            m.readBatchIdParsed = mc.ParseSyntax(m.batchIdSyntax);
            m.writeBatchIdParsed = mc.ParseWritableSyntax(m.batchIdSyntax);

            // Process parameters
            string procName = m.procParsed.Read(mc);
            m.procInfo =mc.GetProcInfo(procName) ;
            ProcessInputProcParams(m.procInfo, me, mc, out m.inputReads, out  m.inputParams);

            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string procName = this.procInfo.localName;
            string procNameSpace = this.procInfo.nameSpace;
            string nodeId = this.processIdParsed.Read(mc);
            bool includeObjectFieldsInOutput=includeObjectFieldsInOutputParsed.Read(mc).Equals("1");

            int nodeIdInt = nodeId.ToInt();
            SocketHandler socketHandler = mc.mvm.mvmCluster.GetClusterNode(nodeIdInt).SocketHandler;

            // get/set batch id
            string batchIdStr = this.readBatchIdParsed.Read(mc);
            long batchId;
            if (batchIdStr.IsNullOrEmpty())
            {
                batchId = mc.mvm.remoteWorkMgr.CreateBatch();
                this.writeBatchIdParsed.Write(mc, batchId.ToString());
            }
            else
            {
                batchId = long.Parse(batchIdStr);
            }

            // need to register this proc call in a table so we can query the database or 
            // have triggers on this tables.
            // for a a given batch, save the workId.
            WorkInfo w = mc.mvm.remoteWorkMgr.CreateWork(batchId);
            w.procName = procName;
            w.nodeId = nodeIdInt;
            w.priority = MessagePriority.InteruptProcCall;
            w.status = WorkStatus.WaitingToStart;

            // serialize the input object to send
            byte[] serializedObject;
            {
                MemoryStream mstream = new MemoryStream();
                BinaryWriter bwriter = new BinaryWriter(mstream);
                IObjectData obj;
                if (this.remoteObjectTypeParsed != null)
                {
                    string remoteObjectType = this.remoteObjectTypeParsed.Read(mc);
                    obj = mc.mvm.objectCache.CreateAndGetObjectOrphan(remoteObjectType);
                }
                else if (this.objectIdParsed != null)
                {
                        string objectId = objectIdParsed.Read(mc);
                        obj = mc.objectCache.CheckOut(objectId);
                    }
                    else
                    {
                        obj = mc.objectData;
                    }
                    obj.Serialize(bwriter);
                bwriter.Flush();
                serializedObject = mstream.ToArray();
            }

            // Serialize the input values.
            byte[] serializedParams;
            {
                MemoryStream mstream = new MemoryStream();
                BinaryWriter bwriter = new BinaryWriter(mstream);
                bwriter.Write7BitEncodedInt(this.inputParams.Count);
                int paramNo = 0;
                foreach (string paramName in this.inputParams)
                {
                    string paramValue = inputReads[paramNo++].Read(mc); 
                    bwriter.Write(paramName);
                    bwriter.Write(paramValue);
                }
                bwriter.Flush();
                serializedParams = mstream.ToArray();
            }

            QueueProcMessage msg = new QueueProcMessage(procName,procNameSpace,w.workId, MessagePriority.InteruptProcCall, serializedObject, serializedParams,includeObjectFieldsInOutput);
            socketHandler.messageOutbox.Add(msg);
        }


        // Used by remote call proc
        public static void ProcessInputProcParams(
            ProcInfo procInfo,
            XmlElement me,
            ModuleContext mc,
            out List<IReadString> inputReads,
            out List<string> inputParams)
        {
            inputReads = new List<IReadString>();
            inputParams = new List<string>();
            var paramsSyntax = me.SelectNodes("./param").ToNameTextDictionary();
            foreach (var paramElem in procInfo.paramElems)
            {
                string name = paramElem.GetAttribute("name");
                string mode = paramElem.GetAttributeDefault("mode", "in");
                string defaultValue = paramElem.GetAttribute("default");
                if (mode.EqualsIgnoreCase("in") || mode.EqualsIgnoreCase("in out"))
                {
                    string src;
                    if (!paramsSyntax.ContainsKey(name))
                    {
                        if (!defaultValue.Equals(""))
                        {
                            src = defaultValue;
                        }
                        else
                        {
                            throw new Exception("Cannot call proc [" + procInfo.procName + "] without passing required parameter [" + name + "]");
                        }
                    }
                    else
                    {
                        src = paramsSyntax[name];
                    }
                    inputReads.Add(mc.ParseSyntax(src));
                    inputParams.Add(name);
                }
            }

        }

    }
}
