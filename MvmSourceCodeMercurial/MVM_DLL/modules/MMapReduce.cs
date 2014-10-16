using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Antlr.Runtime.Tree;
using System.Threading;
using System.Linq;
using NLog;
namespace MVM
{
    /*
<map_reduce>
  <queue_length>10</queue_length>
  <producer_proc>'get_work'</producer_proc>
  <consumer_proc>'do_work'</consumer_proc>
  <reducer_proc>'reduce_work'</_proc>
</map_reduce>
     */



    public class MMapReduce : IModuleSetup, IModuleRun
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public string producerQueueLengthSyntax;
        public IReadString producerQueueLengthParsed;
        public string producerProcSyntax;
        public IReadString producerProcParsed;
        public XmlElement producerXmlElement;    // used to pull out any producer proc params
        public XmlElement consumerXmlElement;    // used to pull out any consumer proc params
        public string consumerProcSyntax;
        public IReadString consumerProcParsed;
        public string consumerIncludeObjectFieldsInOutputSyntax;
        public IReadString consumerIncludeObjectFieldsInOutputParsed;
        public string reducerProcSyntax;
        public IReadString reducerProcParsed;

        private CursorSetupCommon cursorSetup;
          
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            mc.mvm.StartupCluster();
            MMapReduce m = new MMapReduce();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            m.producerQueueLengthSyntax = me.SelectNodeInnerText("./producer_queue_length");

            // THIS REALLY WORKS OFF ANY PRODUCER CURSOR, SO WE CAN EASILY CHANGE
            // THIS TO SUPPORT A PRODUCER_CURSOR INPUT. IN OTHER WORDS PEOPLE
            // CAN PIPE INTO MAP REDUCE. 
            m.producerProcSyntax = me.SelectNodeInnerText("./producer_proc/name");
            m.producerXmlElement = me.SelectSingleElem("./producer_proc");
            m.consumerXmlElement = me.SelectSingleElem("./consumer_proc");
            m.consumerProcSyntax = me.SelectNodeInnerText("./consumer_proc/name");
            m.consumerIncludeObjectFieldsInOutputSyntax = me.SelectNodeInnerText("./consumer_proc/include_object_fields_in_output", "1");
            m.reducerProcSyntax = me.SelectNodeInnerText("./reducer_proc/name");
            m.producerQueueLengthParsed = mc.ParseSyntax(m.producerQueueLengthSyntax);
            m.producerProcParsed = mc.ParseSyntax(m.producerProcSyntax);
            m.consumerProcParsed = mc.ParseSyntax(m.consumerProcSyntax);
            m.reducerProcParsed = mc.ParseSyntax(m.reducerProcSyntax);
            m.consumerIncludeObjectFieldsInOutputParsed = mc.ParseSyntax(m.consumerIncludeObjectFieldsInOutputSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            MapReduceInst mapReduceInst;
            // if we don't have map reduce object in play, read syntax and create one.
            if (mc.tempContext[MapReduceInst.tempFieldName].Equals(""))
            {
                //logger.Debug("Instanciating a map reduce inst");
                string producerQueueLength = this.producerQueueLengthParsed.Read(mc);
                int producerQueueLengthInt = producerQueueLength.ToInt();
                string producerProc = this.producerProcParsed.Read(mc);
                string consumerProc = this.consumerProcParsed.Read(mc);
                bool consumerIncludeObjectFieldsInOutput = this.consumerIncludeObjectFieldsInOutputParsed.Read(mc).Equals("1");
                string reducerProc = this.reducerProcParsed.Read(mc);
                mapReduceInst = new MapReduceInst(mc, this.cursorSetup, producerQueueLengthInt, this.producerXmlElement, producerProc, this.consumerXmlElement, consumerProc, consumerIncludeObjectFieldsInOutput, reducerProc);
                mc.globalContext.SetNamedClassInst(mapReduceInst.globalName, mapReduceInst);
                mc.tempContext[MapReduceInst.tempFieldName] = mapReduceInst.globalName;
            }
            else
            {
                mapReduceInst = mc.globalContext.GetNamedClassInst(mc.tempContext[MapReduceInst.tempFieldName]) as MapReduceInst;
            }
            mapReduceInst.Run(mc);
        }
    }

    public class MapReduceInst
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static readonly string tempFieldName = "map_reduce_inst";
        
        // producer proc
        string producerProc;

        // producer cursor
        ICursor producerCursor;
        string producerCursorInstId;

        // consumer
        string consumerProc;
        bool consumerIncludeObjectFields;
        ProcInfo consumerProcInfo;
        string consumerProcNameSpace;
        Dictionary<string, string> consumerProcInputs = new Dictionary<string, string>();
       
        //reduce
        string reducerProc;
        string reducerOid;
        IObjectData reducerObj;
        int reducerProcId;

        // state info
        long sentCtr = 0;
        long produceCtr = 0;
        long reduceCtr = 0;
        long shortCircuitProduceCtr = 0;
        long shortCircuitSentCtr = 0;
        long shortCircuitReduceCtr = 0;
        Queue<QueueProcMessage> producerQueue = new Queue<QueueProcMessage>();
        int maxProducerQueueLength;
        public enum State { FeedHungrySlaves, ReduceOne, TryToProduce, CallProducerProc, InspectProducerOutput, CheckForCompletion, WaitForUnreducedWork, CircuitBreaker, Complete };
        public State nextState = State.TryToProduce;
        MvmClusterBase mvmCluster;
        RemoteWorkManager remoteWorkManager;
        long batchId;
        WorkBatch batch;

       
        public readonly string globalName;


        # region Variables to determine next state

        /// <summary>
        /// There is no work that has been produced but not sent to a slave outbox.
        /// </summary>
        private bool ProducerQueueEmpty { get { return this.producerQueue.Count == 0; } }
        /// <summary>
        /// There is no work that has been completed but not reduced.
        /// </summary>
        private bool ReducerQueueEmpty { get { return !this.batch.HasUnreducedWork; } }
        /// <summary>
        /// True when all work produced has been reduced. At this point there can be no more work created.
        /// </summary>
        private bool AllWorkComplete { get { return this.produceCtr==this.reduceCtr; } }
        /// <summary>
        /// True means we are in a loop and should short circuit and wait for work to finish
        /// </summary>
        private bool ShortCircuit = false;
        /// <summary>
        /// Flag that indicates that the producer cursor has returned eof.
        /// </summary>
        private bool producerCursorEof = false;

        #endregion

        ICursorSetupCommon cursorSetup;
        public MapReduceInst(ModuleContext mc, ICursorSetupCommon cursorSetup, int maxProducerQueueLength, XmlElement producerElem, string producerProc, XmlElement consumerElem, string consumerProc, bool consumerIncludeObjectFields, string reducerProc)
        {
            // copy in args
            this.cursorSetup = cursorSetup;
            this.maxProducerQueueLength = maxProducerQueueLength;
            this.producerProc = producerProc;
            this.consumerProc = consumerProc;
            this.consumerIncludeObjectFields = consumerIncludeObjectFields;
            this.reducerProc = reducerProc;

            // generate a unique global name so module can call us back
            this.globalName = mc.GetGenSym("mapReduceInst");

            // shortcuts
            this.mvmCluster = mc.mvmCluster;
            this.remoteWorkManager = mc.mvm.remoteWorkMgr;

            // create a batch of work where new work can be added after
            // the batch is in completed state.
            this.batchId = this.remoteWorkManager.CreateBatch(true);
            this.batch = this.remoteWorkManager.LookupBatch(this.batchId);

            // Consumer
            {
            this.consumerProcInfo = mc.GetProcInfo(this.consumerProc);
            this.consumerProcNameSpace = this.consumerProcInfo.nameSpace;

                bool hasParams;
                List<IReadString> inputReads = new List<IReadString>();
                List<IWriteString> inputWrites = new List<IWriteString>();
                List<IReadString> outputReads = new List<IReadString>();
                List<IWriteString> outputWrites = new List<IWriteString>();
                MCallProc.ProcessProcParams(this.consumerProc, consumerElem, mc, out inputReads, out  inputWrites, out  outputReads, out  outputWrites, out hasParams);
                TempContext childTempContext = new TempContext();
                ModuleContext childModuleContext = new ModuleContext();
                childModuleContext.tempContext = childTempContext;
                List<XmlElement> paramList = consumerElem.SelectElements("./param");
                for (int i = 0; i < paramList.Count; i++)
                {
                    consumerProcInputs[paramList[i].GetAttribute("name")] = inputReads[i].Read(mc);
                }
            }

            // setup the producer cursor
            {
                var producerProcId = mc.GetProcId(this.producerProc);
                bool hasParams;
                List<IReadString> inputReads = new List<IReadString>();
                List<IWriteString> inputWrites = new List<IWriteString>();
                List<IReadString> outputReads = new List<IReadString>();
                List<IWriteString> outputWrites = new List<IWriteString>();
                // if the producer proc does not specify a 'pipe_cursor' parameter
                // then specify one now.
                if (!producerElem.GetChildElems().Select(x => x.GetAttribute("name")).Where(n => n.Equals("pipe_cursor")).Any())
                {
                    XmlElement paramElem = producerElem.CreateTextElement("param", "'overwritten'");
                    paramElem.SetAttribute("name", "pipe_cursor");
                    producerElem.AppendChild(paramElem);
                }

                MCallProc.ProcessProcParams(this.producerProc, producerElem, mc, out inputReads, out  inputWrites, out  outputReads, out  outputWrites, out hasParams);
                TempContext childTempContext = new TempContext();
                ModuleContext childModuleContext = new ModuleContext();
                childModuleContext.tempContext = childTempContext;
                for (int i = 0; i < inputReads.Count; i++)
                {
                    inputWrites[i].Write(childModuleContext, inputReads[i].Read(mc));
                }
                mc.procContext.childProcContext = new ProcContext(childTempContext);
                ProcInst producerWork = mc.GetProcToProcId(producerProcId, mc.objectData.objectId);
                producerWork.isSync = mc.procInst.isSync;
                //producerWork.objectId = mc.objectData.objectId;
                producerWork.callbackId = mc.workMgr.CreateCallback(mc.procInst);
                long producerProcInstId = mc.workMgr.CreateCallback(producerWork);
                producerWork.procContext = mc.procContext.childProcContext;
                // instanciate the pipe cursor
                this.producerCursor = new PipeCursor(mc, cursorSetup, producerProcInstId);
                //this.producerCursor.DeleteObjectOnNext = false;
                this.producerCursorInstId = this.producerCursor.CursorInstId;
                producerWork.procContext.tempContext["pipe_cursor"] = producerCursorInstId;
            }

            // spawn a single reducer object that we reuse
            this.reducerProcId = mc.GetProcId(reducerProc);
            this.reducerOid = mc.Spawn("REDUCER");
            this.reducerObj = mc.objectCache.CheckOut(this.reducerOid);

            // stop the reducer from getting cleaned up
            this.reducerObj.RefGet();
        }

        public void Run(ModuleContext mc)
        {
            for (; ; )
            {
            NEXT_STATE:
                //logger.Debug("Enter state: {0} p={1} s={2} r={3}",this.nextState,this.produceCtr,this.sentCtr,this.reduceCtr);
                switch (this.nextState)
                {
                    case State.FeedHungrySlaves:
                        {
                            this.FeedHungrySlaves(mc);
                            // 1: produceCtr=ReduceCtr -> Complete
                            if (this.produceCtr==this.reduceCtr)
                            {
                                this.nextState = State.Complete;
                                goto NEXT_STATE;
                            }
                            // 2: !Rq-E -> ReduceOne
                            if (!this.ReducerQueueEmpty)
                            {
                                this.nextState = State.ReduceOne;
                                goto NEXT_STATE;
                            }
                            // 3: -> CircuitBreaker
                            {
                                this.nextState = State.CircuitBreaker;
                                goto NEXT_STATE;
                            }
                        }
                    case State.ReduceOne:
                        {
                            this.ReduceOne(mc);
                            // 1: default -> TryToProduce
                            {
                                this.nextState = State.TryToProduce;
                                return;
                            }
                        }
                    case State.TryToProduce:
                        {
                            // 1: P-EOF || !PQ.count>PQ.max -> FeedHungrySlaves
                            if (this.producerCursorEof || (this.producerQueue.Count >= this.maxProducerQueueLength))
                            {
                                this.nextState = State.FeedHungrySlaves;
                                goto NEXT_STATE;
                            }
                            // 2: default -> CallProducerProc
                            {
                                this.nextState = State.CallProducerProc;
                                goto NEXT_STATE;
                            }
                        }
                    case State.CallProducerProc:
                        {
                            this.CallProducerProc(mc);
                            // 1: default -> InspectProducerOutput
                            this.nextState = State.InspectProducerOutput;
                            return;
                        }
                    case State.InspectProducerOutput:
                        {
                            this.InspectProducerOutput(mc);
                            // 1: default -> FeedHungrySlaves
                            this.nextState = State.FeedHungrySlaves;
                            goto NEXT_STATE;
                        }
                    case State.CircuitBreaker:
                        {
                            this.CircuitBreaker();
                            // 1: No new work add or sent -> WaitForUnreducedWork
                            if (this.ShortCircuit)
                            {
                                this.nextState = State.WaitForUnreducedWork;
                                goto NEXT_STATE;
                            }
                            // 2: default -> TryToProduce
                            {
                                this.nextState = State.TryToProduce;
                                goto NEXT_STATE;
                            }
                        }
                    case State.WaitForUnreducedWork:
                        {
                            this.WaitForUnreducedWork(mc);
                            // 1: default -> ReduceOne 
                            this.nextState = State.ReduceOne;
                            return;
                        }
                    case State.Complete:
                        {
                            this.Complete(mc);
                            //mc.mvm.Log("returning from MapReduceInst.Run()");
                            return;
                        }
                }
            }
        }

        private void FeedHungrySlaves(ModuleContext mc)
        {
            // Feed as many slaves as we can
            // Note that GetSlaveNodes() already filters out the profiler
            var hungrySlaves = this.mvmCluster.GetSlaveNodes().Where(n => this.batch.NoWorkInProgress(n.nodeId));
            foreach (MvmClusterNode slave in hungrySlaves)
            {
                if (this.producerQueue.Count == 0) break;
                QueueProcMessage msg = this.producerQueue.Dequeue();
                WorkInfo w = this.remoteWorkManager.LookupWork(msg.workId);
                w.status = WorkStatus.WaitingToStart;
                w.nodeId = slave.nodeId;
                //logger.Debug("SEND: work_id={0} to slave node_id={1}", w.workId,slave.nodeId);
                slave.SocketHandler.messageOutbox.Add(msg);
                this.sentCtr++;
            }
        }

        private void ReduceOne(ModuleContext mc)
        {
            // Reduce one thing
            WorkInfo workToReduce;
            if (this.batch.TryGetWorkToReduce(out workToReduce))
            {
                this.reducerObj.Clear();
                Dictionary<string, string> outputs = workToReduce.outputs as Dictionary<string, string>;
                foreach (var entry in outputs) this.reducerObj[entry.Key] = entry.Value;
                mc.CallProcForObjectAndReturn(this.reducerProcId, this.reducerOid);
                mc.YieldAndRepeat();
                this.reduceCtr++;
            }
            else
            {
                throw new Exception("Reduce one should never not have work to reduce");
            }
        }

        private void CallProducerProc(ModuleContext mc)
        {
            IObjectData outputObj;
            var csrStatus=this.producerCursor.Next(mc, out outputObj);
            if (csrStatus != CursorStatus.YIELD)
            {
                throw new Exception("unexpected:"+csrStatus);
            }
            mc.YieldAndRepeat();
        }

        private void InspectProducerOutput(ModuleContext mc)
        {
            IObjectData outputObj;
            var csrStatus = this.producerCursor.Next(mc, out outputObj);
            switch (csrStatus)
            {
                case CursorStatus.EOF:
                    {
                        //logger.Trace("pipe returned eof");
                        this.producerCursorEof = true;
                        break;
                    }
                case CursorStatus.HAS_ROW:
                    {
                        bool isNullRow = !outputObj["null_row"].Equals("");
                        if (!isNullRow)
                        {
                            //logger.Trace("pipe returned real row, oid=" + outputObj.objectId+", refs="+outputObj.RefCount);
                            this.produceCtr++;
                            WorkInfo work = this.remoteWorkManager.CreateWork(this.batchId);
                            work.status = WorkStatus.Produced;
                            QueueProcMessage msg = new QueueProcMessage(
                                this.consumerProc,
                                this.consumerProcNameSpace,
                                work.workId,
                                MessagePriority.WaitingProcCall,
                                outputObj,
                                consumerProcInputs,
                                this.consumerIncludeObjectFields
                                );
                            // delete the cursor object
                            //outputObj.Delete();
                            this.producerQueue.Enqueue(msg);
                        }
                        else
                        {
                            //logger.Trace("pipe returned NULL row, oid=" + outputObj.objectId);
                            //outputObj.Delete();
                        }
                        break;
                    }
                default: throw new Exception("unexpected cursor status:" + csrStatus);
            }
        }

        public void CircuitBreaker()
        {
            long lastProduceCtr = this.shortCircuitProduceCtr;
            this.shortCircuitProduceCtr = this.produceCtr;
            long lastSentCtr = this.shortCircuitSentCtr;
            this.shortCircuitSentCtr = this.sentCtr;
            long lastReduceCtr = this.shortCircuitReduceCtr;
            this.shortCircuitReduceCtr = this.reduceCtr;
            if (
                this.shortCircuitSentCtr == lastSentCtr 
                && this.shortCircuitProduceCtr == lastProduceCtr 
                && this.shortCircuitReduceCtr == lastReduceCtr)
                this.ShortCircuit = true;
            else this.ShortCircuit = false;
        }
        
        private void WaitForUnreducedWork(ModuleContext mc)
        {
            // add an callback event that fires when there is unreduced work in the batch.
            //logger.Trace("waiting for work to compete so we can reduce it");
            long callbackId = mc.workMgr.CreateCallback(mc.procInst);
            CallbackBatchEvent callbackBatchEvent = new CallbackBatchEvent(callbackId);
            this.batch.AddUnreducedWorkEvent(callbackBatchEvent);
            mc.YieldAndRepeat();
        }

        private void Complete(ModuleContext mc)
        {
            // cleanup
            //logger.Debug("Enter compete");
            this.producerCursor.Clear(mc);
            //mc.globalContext.RmNamedClassInst(this.pipeCursorOid);
            mc.tempContext[tempFieldName] = "";
            mc.globalContext.RmNamedClassInst(this.globalName);
            // remove the reducer ref, this should cause it to be cleared.
            //this.reducerObj.Delete();
            this.reducerObj.RefRemove();
            // clear out the workbatch as we no longer need it
            this.remoteWorkManager.ClearBatch(this.batchId);

        }
    }
}
