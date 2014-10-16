using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
namespace MVM
{
    public class MCallProc : IModuleSetup
    {
        public static void ProcessProcParams(string procName,XmlElement me, ModuleContext mc,
            out List<IReadString> inputReads,
            out List<IWriteString> inputWrites,
            out List<IReadString> outputReads,
            out List<IWriteString> outputWrites,
            out bool hasParams)
        {
            inputReads = new List<IReadString>();
            inputWrites = new List<IWriteString>();
            outputReads = new List<IReadString>();
            outputWrites = new List<IWriteString>();
            var paramsSyntax = me.SelectNodes("./param").ToNameTextDictionary();
            var procInfo = mc.GetProcInfo(procName);
            hasParams = procInfo.paramElems.Count > 0;
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
                            throw new Exception("Cannot call proc [" + procName + "] without passing required parameter [" + name + "]");
                        }
                    }
                    else
                    {
                        src = paramsSyntax[name];
                    }
                    string tgt = "TEMP." + name;
                    inputReads.Add(mc.ParseSyntax(src));
                    inputWrites.Add(mc.ParseWritableSyntax(tgt));
                }
                if (mode.EqualsIgnoreCase("out") || mode.EqualsIgnoreCase("in out"))
                {
                    string src = "TEMP." + name;
                    // need to write top level scope for output to make sure it gets defined there.
                    if (mode.Equals("out"))
                    {
                        inputReads.Add(mc.ParseSyntax("''"));
                        inputWrites.Add(mc.ParseWritableSyntax(src));
                    }
                    string tgt;
                    if (!paramsSyntax.ContainsKey(name))
                    {
                        // no copy back if output is passed a literal
                        if (defaultValue.IsQuoted() || defaultValue.IsNumeric())
                        {
                            continue;
                        }
                        if (defaultValue.Equals(""))
                        {
                            throw new Exception("Cannot call proc [" + procName + "] without passing required parameter [" + name + "]");
                        }
                        tgt = defaultValue;
                    }
                    else
                    {
                        tgt = paramsSyntax[name];
                    }
                    outputReads.Add(mc.ParseSyntax(src));
                    outputWrites.Add(mc.ParseWritableSyntax(tgt));
                }
            }

        }




        /*
         * Call procContext for current object
         * <call_proc>
         * <name>this_proc</name>
         * </call_proc>
         * 
         * Call procContext for specific object
         * <call_proc_for_object>
         * <name>this_proc</name>
         * <object_type>ACCOUNT</object_type>
         * <object_id>123123</object_id>
         * </call_proc_for_object>
         * 
         * Call procContext for all object of type
         * <call_proc_for_object_type>
         * <name>this_proc</name>
         * <object_type>.*ACCOUNT</object_type>
         * </call_proc_for_object_type>
         * 
     
         * TODO: support this for piecemeal dynamic approach, with intelligent swapping of 
         * static versions when we know something cannot change. This gives us a single way
         * to specify what we want and still get perfomance benefits of statically knowing.
         * 
         * <call_proc>
         * <object_type><object_type>
         * <object_id></object_id>
         * <same_temp_scope><same_temp_scope>
         * <callback_proc_name></callback_proc_name>
         * <callback_object_id></callback_object_id>
         * <no_callback>1|0</no_callback>
         * <is_asychronous>1|0<is_asynchronous>
         * <parameters..../>
         * <call_proc>
         * 
         */
        public void Setup(System.Xml.XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            string tagName = me.LocalName;
            string procNameSyntax;
            if (tagName.Equals("call_proc"))
                procNameSyntax = me.InnerText;
            else
                procNameSyntax = me.SelectNodeInnerText("./name");
            string procName = mc.ParseSyntax(procNameSyntax).Read(mc);
            string objectType = me.SelectNodeInnerText("./object_type");
            string objectId = me.SelectNodeInnerText("./object_id");

            string procInstId = me.SelectNodeInnerText("./proc_inst_id");

            int procId = mc.GetProcId(procName);

            // process the parameters
            bool hasParams;
            List<IReadString> inputReads = new List<IReadString>();
            List<IWriteString> inputWrites = new List<IWriteString>();
            List<IReadString> outputReads = new List<IReadString>();
            List<IWriteString> outputWrites = new List<IWriteString>();
            ProcessProcParams(procName, me, mc, out inputReads, out  inputWrites, out  outputReads, out  outputWrites, out hasParams);

            // deal w/ callbackif there is one
            string callbackProcName = me.SelectNodeInnerText("./callback_proc_name");
            string callbackObjectId = me.SelectNodeInnerText("./callback_object_id");
            int callbackProcId = -1;
            if (callbackProcName != null)
            {
                callbackProcName = callbackProcName.StripQuotes();
                callbackProcId = mc.GetProcId(callbackProcName);
            }

            // add a debug print when we enter the proc
            if(mc.globalContext["log_proc_calls"].Equals("1"))run.Add(new TraceProc(mc.ParseSyntax(procNameSyntax), TraceProc.TraceDirection.Enter));

            // setup the runtime newModule to call the procContext
            if (tagName.Equals("call_proc_for_object"))
            {
                if (hasParams)
                {
                    run.Add(new CallProcForObjectParams(procId, objectId, mc.ParseSyntax(objectId), inputReads, inputWrites));
                    run.Add(new CopyOutputParams(outputReads, outputWrites));
                }
                else
                {
                    run.Add(new CallProcForObject(procId, objectId, mc.ParseSyntax(objectId)));
                }
            }
            else if (tagName.In("call_dynamic_proc_for_object", "call_dynamic_proc_for_current_object"))
            {
                if (objectId == null) objectId = "OBJECT.object_id";
                run.Add(new CallDynamicProcForObjectParams(mc.ParseSyntax(procNameSyntax), objectId, mc.ParseSyntax(objectId), inputReads, inputWrites));
                run.Add(new CopyOutputParams(outputReads, outputWrites));
            }
            else if (tagName.EndsWith("call_proc_for_current_object") || tagName.EndsWith("call_proc"))
            {
                bool setupOnly = tagName.StartsWith("setup");
                if (hasParams)
                {
                    run.Add(new CallProcForCurrentObjectParams(procId, inputReads, inputWrites, mc.ParseWritableSyntax(procInstId), setupOnly));
                    run.Add(new CopyOutputParams(outputReads, outputWrites));
                }
                else
                {
                    run.Add(new CallProcForCurrentObject(procId, mc.ParseWritableSyntax(procInstId), setupOnly));
                }
            }
            else if (tagName.Equals("call_proc_for_object_nested"))
            {
                run.Add(new MScopeSnapshot());
                run.Add(new CallProcForObjectNested(procId, objectId, mc.ParseSyntax(objectId)));
                run.Add(new MScopeUnsnapshot());
            }
            else if (tagName.Equals("call_proc_for_object_nested_no_callback"))
            {
                run.Add(new MScopeSnapshot());
                run.Add(new CallProcForObjectNestedNoCallback(procId, objectId, mc.ParseSyntax(objectId)));
                run.Add(new MScopeUnsnapshot());
            }
            else if (tagName.Equals("call_proc_for_object_nested_no_callback_on_stack"))
            {
                run.Add(new CallProcForObjectNestedNoCallbackOnStack(procId, objectId, mc.ParseSyntax(objectId)));
            }

            else if (tagName.Equals("call_proc_for_current_object_nested"))
            {
                run.Add(new MScopeSnapshot());
                run.Add(new CallProcForCurrentObjectNested(procId));
                run.Add(new MScopeUnsnapshot());
            }
            else if (tagName.Equals("queue_proc_for_object"))
            {
                run.Add(new QueueProcForObject(procId, objectId, mc.ParseSyntax(objectId)));
            }
            else if (tagName.Equals("queue_proc_for_object_nested"))
            {
                if (callbackProcId < 0)
                    run.Add(new QueueProcForObjectNested(procId, objectId, mc.ParseSyntax(objectId)));
                else
                    run.Add(new QueueProcForObjectNestedWithCb(procId, objectId, mc.ParseSyntax(objectId), callbackProcId, mc.ParseSyntax(callbackObjectId)));
            }
            else if (tagName.Equals("queue_proc_for_current_object"))
            {
                run.Add(new QueueProcForCurrentObject(procId));
            }
            else if (tagName.Equals("queue_proc_for_current_object_nested"))
            {
                if (callbackProcId < 0)
                    run.Add(new QueueProcForCurrentObjectNested(procId));
                else
                    run.Add(new QueueProcForCurrentObjectNestedWithCb(procId, callbackProcId, mc.ParseSyntax(callbackObjectId)));
            }
            else if (tagName.Equals("queue_proc_for_current_object_nested_on_stack"))
            {
                if (callbackProcId < 0)
                {
                    run.Add(new QueueProcForCurrentObjectNestedOnStack(procId));
                }
                else
                {
                    throw new Exception("with cb not handled");
                }

            }
            else
            {
                throw new Exception("NOT SUPPORTED:" + tagName);
            }

            // add a debug print when we exit the proc
            if (mc.globalContext["log_proc_calls"].Equals("1")) run.Add(new TraceProc(mc.ParseSyntax(procNameSyntax), TraceProc.TraceDirection.Exit));
        }
    }

    public class TraceProc : IModuleRun
    {
        public enum TraceDirection { Enter, Exit };
        private readonly IReadString procNameParsed;
        private TraceDirection direction;
        public TraceProc(IReadString procNameParsed, TraceDirection direction)
        {
            this.procNameParsed = procNameParsed;
            this.direction = direction;
        }
        public void Run(ModuleContext mc)
        {
            string procName = this.procNameParsed.Read(mc);
            if (this.direction == TraceDirection.Enter)
            {
                mc.workMgr.mvm.Log("log_proc_calls", "Tracing proc calls: entering " + procName);
    }
            else
            {
                mc.workMgr.mvm.Log("log_proc_calls", "Tracing proc calls: exiting " + procName);
            }
        }
    }

    public class CopyOutputParams : IModuleRun
    {
        private readonly List<IReadString> outputReads;
        private readonly List<IWriteString> outputWrites;
        public CopyOutputParams(List<IReadString> outputReads, List<IWriteString> outputWrites)
        {
            this.outputReads = outputReads;
            this.outputWrites = outputWrites;
        }
        public void Run(ModuleContext mc)
        {
            ModuleContext childModuleContext = new ModuleContext();
            childModuleContext.tempContext = mc.procContext.childProcContext.tempContext;
            for (int i = 0; i < this.outputReads.Count; i++)
            {
                this.outputWrites[i].Write(mc, this.outputReads[i].Read(childModuleContext));
            }
            mc.procContext.childProcContext = null; // allow kid to get GCed
        }
    }

    public class CallProcForCurrentObjectParams : IModuleRun
    {
        private readonly int procId;
        private readonly List<IReadString> inputReads;
        private readonly List<IWriteString> inputWrites;
        private readonly IWriteString procInstIdParsed;
        private readonly bool setupOnly;
        public CallProcForCurrentObjectParams(int procId, List<IReadString> inputReads, List<IWriteString> inputWrites, IWriteString procInstIdParsed, bool setupOnly)
        {
            this.procId = procId;
            this.inputReads = inputReads;
            this.inputWrites = inputWrites;
            this.procInstIdParsed = procInstIdParsed;
            this.setupOnly = setupOnly;
        }
        public void Run(ModuleContext mc)
        {
            // we have a newModule context solely for purposes of setting tempContext
            // scope in the called procContext.
            TempContext childTempContext = new TempContext();
            ModuleContext childModuleContext = new ModuleContext();
            childModuleContext.tempContext = childTempContext;
            for (int i = 0; i < this.inputReads.Count; i++)
            {
                this.inputWrites[i].Write(childModuleContext, this.inputReads[i].Read(mc));
            }
            // create the childs procContext context for it since we are preloading its tempContext.
            mc.procContext.childProcContext = new ProcContext(childTempContext);
            // Advance to the next newModule
            mc.procInst.nextModuleOrder = mc.procDefinition.GetNextModuleOrder(mc.moduleIdx);
            // Setup the procInst for the procContext
            ProcInst newWork = new ProcInst(mc.scheduler.GetProcInfo(this.procId), mc.objectData.objectId);
            newWork.isSync = mc.procInst.isSync;
            newWork.procContext = mc.procContext.childProcContext;
            
            // Setup the procNameSyntax chain
            if (!setupOnly)
            {
                // no cb chain if setup only... 
                newWork.callbackId = mc.workMgr.CreateCallback(mc.procInst);
            }
            // if passed an inst id write it
            if (this.procInstIdParsed != null)
            {
                long callbackId = mc.workMgr.CreateCallback(newWork);
                this.procInstIdParsed.Write(mc, callbackId.ToString());
            }
            if (!this.setupOnly)
            {
                // Put the new procInst in the machine
                mc.worker.ProduceWork(newWork);
                // Make the procContext yield b/c the cur procInst is not finished
                mc.Yield();
            }
        }
     
    }
    public class CallProcForObjectParams : IModuleRun
    {
        private readonly int procId;
        private readonly string objectId;
        private readonly IReadString objectIdParsed;
        private readonly List<IReadString> inputReads;
        private readonly List<IWriteString> inputWrites;
        public CallProcForObjectParams(int procId, string objectId, IReadString objectIdParsed, List<IReadString> inputReads, List<IWriteString> inputWrites)
        {
            this.procId = procId;
            this.objectId = objectId;
            this.objectIdParsed = objectIdParsed;
            this.inputReads = inputReads;
            this.inputWrites = inputWrites;
        }
        public void Run(ModuleContext mc)
        {
            // we have a newModule context solely for purposes of setting tempContext
            // scope in the called procContext.
            TempContext childTempContext = new TempContext();
            ModuleContext childModuleContext = new ModuleContext();
            childModuleContext.tempContext = childTempContext;
            for (int i = 0; i < this.inputReads.Count; i++)
            {
                this.inputWrites[i].Write(childModuleContext, this.inputReads[i].Read(mc));
            }
            // create the childs procContext context for it since we are preloading its tempContext.
            mc.procContext.childProcContext = new ProcContext(childTempContext);
            // Advance to the next newModule
            mc.procInst.nextModuleOrder = mc.procDefinition.GetNextModuleOrder(mc.moduleIdx);
            // Setup the procInst for the procContext
            ProcInst newWork = new ProcInst(mc.scheduler.GetProcInfo(this.procId), this.objectIdParsed.Read(mc));
            newWork.isSync = mc.procInst.isSync;
            newWork.procContext = mc.procContext.childProcContext;
            // Setup the procNameSyntax chain
            newWork.callbackId = mc.workMgr.CreateCallback(mc.procInst);
            // Put the new procInst in the machine
            mc.worker.ProduceWork(newWork);
            // Make the procContext yield b/c the cur procInst is not finished
            mc.Yield();
        }
      
    }

    public class CallDynamicProcForObjectParams : IModuleRun
    {
        private readonly IReadString procNameParsed;
       // private readonly IReadString defaultProcNameParsed;
        private readonly string objectId;
        private readonly IReadString objectIdParsed;
        private readonly List<IReadString> inputReads;
        private readonly List<IWriteString> inputWrites;
        public CallDynamicProcForObjectParams(IReadString procNameParsed, string objectId, IReadString objectIdParsed, List<IReadString> inputReads, List<IWriteString> inputWrites)
        {
            this.procNameParsed = procNameParsed;
           // this.defaultProcNameParsed = defaultProcNameParsed;
            this.objectId = objectId;
            this.objectIdParsed = objectIdParsed;
            this.inputReads = inputReads;
            this.inputWrites = inputWrites;
        }
        public void Run(ModuleContext mc)
        {
            // lookup the initNamespaceProcName
            string procName = this.procNameParsed.Read(mc);
            
            int procId = mc.GetProcId(procName);


            // we have a newModule context solely for purposes of setting tempContext
            // scope in the called procContext.
            TempContext childTempContext = new TempContext();
            ModuleContext childModuleContext = new ModuleContext();
            childModuleContext.tempContext = childTempContext;
            for (int i = 0; i < this.inputReads.Count; i++)
            {
                this.inputWrites[i].Write(childModuleContext, this.inputReads[i].Read(mc));
            }
            // create the childs procContext context for it since we are preloading its tempContext.
            mc.procContext.childProcContext = new ProcContext(childTempContext);
            // Advance to the next newModule
            mc.procInst.nextModuleOrder = mc.procDefinition.GetNextModuleOrder(mc.moduleIdx);
            // Setup the procInst for the procContext
            ProcInst newWork = new ProcInst(mc.scheduler.GetProcInfo(procId), this.objectIdParsed.Read(mc));
            newWork.isSync = mc.procInst.isSync;
            newWork.procContext = mc.procContext.childProcContext;
            // Setup the procNameSyntax chain
            newWork.callbackId = mc.workMgr.CreateCallback(mc.procInst);
            // Put the new procInst in the machine
            mc.worker.ProduceWork(newWork);
            // Make the procContext yield b/c the cur procInst is not finished
            mc.Yield();
        }
    }

    public class CallProcForCurrentObject : IModuleRun
    {
        private readonly int procId;
        private readonly IWriteString procInstIdParsed;
        private readonly bool setupOnly;
        public CallProcForCurrentObject(int procId, IWriteString procInstIdParsed, bool setupOnly)
        {
            this.procId = procId;
            this.procInstIdParsed = procInstIdParsed;
            this.setupOnly = setupOnly;
        }
        public void Run(ModuleContext mc)
        {
            // Advance to the next newModule
            mc.procInst.nextModuleOrder = mc.procDefinition.GetNextModuleOrder(mc.moduleIdx);
            // Setup the procInst for the procContext
            ProcInst newWork = mc.GetProcToProcId(procId,mc.objectData.objectId);
            newWork.isSync = mc.procInst.isSync;
            //newWork.objectId = mc.objectData.objectId;
            newWork.procContext = new ProcContext();
            // Setup the procNameSyntax chain
            if (!setupOnly)
            {
                newWork.callbackId = mc.workMgr.CreateCallback(mc.procInst);
            }
            if (!this.setupOnly)
            {
                // Put the new procInst in the machine
                mc.worker.ProduceWork(newWork);
                // Make the procContext yield b/c the cur procInst is not finished
                mc.Yield();
            }
            // Store the proc inst
            if (this.procInstIdParsed != null)
            {
                long callbackId = mc.workMgr.CreateCallback(newWork);
                this.procInstIdParsed.Write(mc, callbackId.ToString());
            }
        }
    }

    public class CallProcForCurrentObjectNested : IModuleRun
    {
        private readonly int procId;
        public CallProcForCurrentObjectNested(int procId)
        {
            this.procId = procId;
        }
        public void Run(ModuleContext mc)
        {
            mc.CallProcForCurrentObjectNested(this.procId); //added snap/unsnap in setup 
        }
    }

    public class CallProcForObject : IModuleRun
    {
        private readonly int procId;
        private readonly string objectId;
        private readonly IReadString objectIdParsed;
        public CallProcForObject(int procId, string objectId, IReadString objectIdParsed)
        {
            this.procId = procId;
            this.objectId = objectId;
            this.objectIdParsed = objectIdParsed;
        }
        public void Run(ModuleContext mc)
        {
            string objectIdValue = objectIdParsed.Read(mc);
            mc.CallProcForObject(this.procId, objectIdValue);
        }
    }
    public class CallProcForObjectNested : IModuleRun
    {
        private readonly int procId;
        private readonly string objectId;
        private readonly IReadString objectIdParsed;
        public CallProcForObjectNested(int procId, string objectId, IReadString objectIdParsed)
        {
            this.procId = procId;
            this.objectId = objectId;
            this.objectIdParsed = objectIdParsed;
        }
        public void Run(ModuleContext mc)
        {
            string objectIdValue = objectIdParsed.Read(mc);
            mc.CallProcForObjectNested(this.procId, objectIdValue); // added snap/unsnap in setup
        }
    }
    public class CallProcForObjectNestedNoCallbackOnStack : IModuleRun
    {
        private readonly int procId;
        private readonly string objectId;
        private readonly IReadString objectIdParsed;
        public CallProcForObjectNestedNoCallbackOnStack(int procId, string objectId, IReadString objectIdParsed)
        {
            this.procId = procId;
            this.objectId = objectId;
            this.objectIdParsed = objectIdParsed;
        }
        public void Run(ModuleContext mc)
        {
            string objectIdValue = objectIdParsed.Read(mc);
            mc.CallProcForObjectNestedNoCallbackOnStack(this.procId, objectIdValue);
        }
    }
    public class CallProcForObjectNestedNoCallback : IModuleRun
    {
        private readonly int procId;
        private readonly string objectId;
        private readonly IReadString objectIdParsed;
        public CallProcForObjectNestedNoCallback(int procId, string objectId, IReadString objectIdParsed)
        {
            this.procId = procId;
            this.objectId = objectId;
            this.objectIdParsed = objectIdParsed;
        }
        public void Run(ModuleContext mc)
        {
            string objectIdValue = objectIdParsed.Read(mc);
            mc.CallProcForObjectNestedNoCallback(this.procId, objectIdValue);
        }
    }


    public class QueueProcForCurrentObject : IModuleRun
    {
        private readonly int procId;
        public QueueProcForCurrentObject(int procId)
        {
            this.procId = procId;
        }
        public void Run(ModuleContext mc)
        {
            mc.QueueProcForCurrentObject(procId);
        }
    }


    public class QueueProcForObjectNested : IModuleRun
    {
        private readonly int procId;
        private readonly string objectId;
        private readonly IReadString objectIdParsed;

        public QueueProcForObjectNested(int procId, string objectId, IReadString objectIdParsed)
        {
            this.procId = procId;
            this.objectId = objectId;
            this.objectIdParsed = objectIdParsed;
        }
        public void Run(ModuleContext mc)
        {
            mc.QueueProcForObjectNested(procId, objectIdParsed.Read(mc));
        }
    }

    public class QueueProcForObjectNestedWithCb : IModuleRun
    {
        private readonly int procId;
        private readonly string objectId;
        private readonly IReadString objectIdParsed;
        private readonly int cbProcId;
        private readonly IReadString cbObjectIdParsed;
        public QueueProcForObjectNestedWithCb(int procId, string objectId, IReadString objectIdParsed, int cbProcId, IReadString cbObjectIdParsed)
        {
            this.procId = procId;
            this.objectId = objectId;
            this.objectIdParsed = objectIdParsed;
            this.cbProcId = cbProcId;
            this.cbObjectIdParsed = cbObjectIdParsed;
        }
        public void Run(ModuleContext mc)
        {
            string cbObjectIdValue = cbObjectIdParsed.Read(mc);
            mc.QueueProcForObjectNestedWithCb(procId, objectIdParsed.Read(mc), cbProcId, cbObjectIdValue);

        }
    }


    public class QueueProcForCurrentObjectNestedOnStack : IModuleRun
    {
        private readonly int procId;
        public QueueProcForCurrentObjectNestedOnStack(int procId)
        {
            this.procId = procId;
        }
        public void Run(ModuleContext mc)
        {
            mc.QueueProcForCurrentObjectNestedOnStack(procId);
        }
    }


    public class QueueProcForCurrentObjectNested : IModuleRun
    {
        private readonly int procId;
        public QueueProcForCurrentObjectNested(int procId)
        {
            this.procId = procId;
        }
        public void Run(ModuleContext mc)
        {
            mc.QueueProcForCurrentObjectNested(procId);
        }
    }

    public class QueueProcForCurrentObjectNestedWithCb : IModuleRun
    {
        private readonly int procId;
        private readonly int cbProcId;
        private readonly IReadString cbObjectIdParsed;
        public QueueProcForCurrentObjectNestedWithCb(int procId, int cbProcId, IReadString cbObjectIdParsed)
        {
            this.procId = procId;
            this.cbProcId = cbProcId;
            this.cbObjectIdParsed = cbObjectIdParsed;
        }
        public void Run(ModuleContext mc)
        {
            string cbObjectIdValue = cbObjectIdParsed.Read(mc);
            mc.QueueProcForCurrentObjectNestedWithCb(procId, cbProcId, cbObjectIdValue);
        }
    }


    public class QueueProcForObject : IModuleRun
    {
        private readonly int procId;
        private readonly string objectId;
        private readonly IReadString objectIdParsed;
        public QueueProcForObject(int procId, string objectId, IReadString objectIdParsed)
        {
            this.procId = procId;
            this.objectId = objectId;
            this.objectIdParsed = objectIdParsed;
        }
        public void Run(ModuleContext mc)
        {

            string objectIdValue = objectIdParsed.Read(mc);
            mc.QueueProcForObject(procId, objectIdValue);
        }
    }
    }
