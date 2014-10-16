using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
using NLog;

namespace MVM
{
    public class LocationInfo
    {
        public DateTime createDate = DateTime.Now;
        public int nodeId = -1;
        public string procName = "";
        public string procNamespace = "";
        public string moduleName = "";
        public string fileName = "";
        public int lineNumber = -1;
        public LocationInfo() { }
        public string label()
        {
            return nodeId.ToString() + ":" + procNamespace.Nvl("") + ":" + procName.Nvl("") + ":" + moduleName.Nvl("") + ":" + lineNumber.ToString();
        }
    }

    public class StackFrame : LocationInfo
    {
        public StackFrame() : base() { }
        public StackFrame(int nodeId, string proc, string nspace, string module, string file, int line)
        {
            this.createDate = DateTime.Now;
            this.nodeId = nodeId;
            this.procName = proc;
            this.procNamespace = nspace;
            this.moduleName = module;
            this.fileName = file;
            this.lineNumber = line;
        }
        public StackFrame(Dictionary<string, string> dict)
        {
            if (dict.ContainsKey("createDate"))
            {
                this.createDate = DateTime.ParseExact(dict["createDate"], MvmClusterProfiler.dateFormat, null);
            }
            if (dict.ContainsKey("nodeId"))
            {
                this.nodeId = dict["nodeId"].ToInt();
            }
            if (dict.ContainsKey("proc"))
            {
                this.procName = dict["proc"];
            }
            if (dict.ContainsKey("namespace"))
            {
                this.procNamespace = dict["namespace"];
            }
            if (dict.ContainsKey("module"))
            {
                this.moduleName = dict["module"];
            }
            if (dict.ContainsKey("file"))
            {
                this.fileName = dict["file"];
            }
            if (dict.ContainsKey("line"))
            {
                this.lineNumber = dict["line"].ToInt();
            }
        }
        public Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            d["createDate"] = this.createDate.ToString(MvmClusterProfiler.dateFormat);
            d["nodeId"] = this.nodeId.ToString();
            d["proc"] = this.procName;
            d["namespace"] = this.procNamespace;
            d["module"] = this.moduleName;
            d["file"] = this.fileName;
            d["line"] = this.lineNumber.ToString();
            return d;
        }
    }

    public class ProfileNode : LocationInfo
    {
        public int depth = 0;
        public int count = 0;  // us and all descendants
        public int descendantCount = 0;  // only descendant nodes
        public Dictionary<string, ProfileNode> childNodes = new Dictionary<string, ProfileNode>();
        public ProfileNode() : base() { }
        public ProfileNode(StackFrame frame)
        {
            this.createDate = frame.createDate;
            this.nodeId = frame.nodeId;
            this.procName = frame.procName;
            this.procNamespace = frame.procNamespace;
            this.moduleName = frame.moduleName;
            this.fileName = frame.fileName;
            this.lineNumber = frame.lineNumber;
        }
        public ProfileNode(ProfileNode node)
        {
            this.count = node.count;
            this.descendantCount = node.descendantCount;
            this.depth = node.depth;
            this.createDate = node.createDate;
            this.nodeId = node.nodeId;
            this.procName = node.procName;
            this.procNamespace = node.procNamespace;
            this.moduleName = node.moduleName;
            this.fileName = node.fileName;
            this.lineNumber = node.lineNumber;
        }
        public virtual void Add(int nodeId, List<StackFrame> descendants)
        {
            this.count++;
            descendants.RemoveAt(0);
            if (descendants.Count > 0)
            {
                this.descendantCount++;
                string label = descendants[0].label();
                if (!childNodes.ContainsKey(label))
                {
                    childNodes[label] = new ProfileNode(descendants[0]);
                    childNodes[label].nodeId = nodeId;
                    childNodes[label].depth = this.depth + 1;
                }
                childNodes[label].Add(nodeId, descendants);
            }
        }
        public void GetResults(int total, List<ProfileNodeSummary> results)
        {
            results.Add(new ProfileNodeSummary(this, total));
            foreach (var child in childNodes)
            {
                child.Value.GetResults(total, results);
            }
        }
    }

    public class ProfileRoot : ProfileNode
    {
        public MvmClusterProfiler mvmClusterProfiler = null;
        public ProfileRoot(int nodeId, MvmClusterProfiler prof)
            : base()
        {
            this.nodeId = nodeId;
            this.mvmClusterProfiler = prof;
        }
        public List<ProfileNodeSummary> Results()
        {
            List<ProfileNodeSummary> results = new List<ProfileNodeSummary>();
            this.GetResults(this.count, results);
            return results;
        }
    }

    public class ProfileNodeSummary : ProfileNode
    {
        public string inclusive;
        public string exclusive;
        public ProfileNodeSummary(ProfileNode node, int total)
            : base(node)
        {
            if (total > 0)
            {
                this.inclusive = (((decimal)node.count) / (decimal)total).ToString("0.00");
                this.exclusive = (((decimal)(node.count - node.descendantCount)) / (decimal)total).ToString("0.00");
            }
            else
            {
                this.inclusive = this.exclusive = "0.00";
            }
        }
    }

    public class MvmClusterProfiler : MvmCluster
    {
        public static string dateFormat = "yyyyMMddHHmmss.fff";

        // Do not change these defaults here -- change them in MStartProfiler (they get copied over in LoadModules)
        public static int DefaultSamplingPeriod = 100;
        public static int DefaultReportingCount = 10;

        public bool attachToExistingMVM = false;
        public AutoResetEvent nodesReadyEvent = new AutoResetEvent(false);
        public Dictionary<int, ProfileRoot> stackInfo = new Dictionary<int, ProfileRoot>();
        public Dictionary<int, StackFrame> dummyFrame = new Dictionary<int, StackFrame>();

        public int profilerOutputPeriod = 60000;  // write profile results this often (in milliseconds)

        public MvmClusterProfiler(MvmEngine mvm, int nodeId)
            : base(mvm, nodeId)
        {
        }

        public void SetupProfiler(MvmEngine mvm, int superNodeId, string superMachine, int superPort, int nodeId, string machine, bool attach)
        {
            this.attachToExistingMVM = attach;

            // Start a thread that periodically writes our output in the background
            if (!this.mvm.globalContext["profiler_output_period"].Equals(""))
            {
                this.profilerOutputPeriod = this.mvm.globalContext["profiler_output_period"].ToInt();
            }
            ProfilerWriteThread profilerThread = new ProfilerWriteThread(this);
            ThreadStart profilerThreadStart = new ThreadStart(profilerThread.Run);
            Thread thread = new Thread(profilerThreadStart);
            thread.Name = "PW" + this.NodeIdStr;
            thread.Start();

            if (!attach)
            {
                this.SetupSlave(superNodeId, superMachine, superPort, nodeId, machine);
                return;
            }

            // Here is the handshaking sequence when connecting to a supernode that is not yet aware of us:
            // - create an MvmClusterNode for the super, and get its SocketHandler (this will connect to the super)
            // - as it accepts the connection, the super will reply with our node_id
            // - start our own listener so the super can talk to us
            // - send the super a message to update our machine/port info
            // - super then pushes updated cluster node info to all nodes (we wait till we've gotten this message)
            // - send message to each node to tell it to start up its profiler thread and begin gathering info
            // - now all slave nodes have what they need to talk to us

            Dictionary<string, string> superNodeInfo = new Dictionary<string, string>();
            superNodeInfo["node_id"] = superNodeId.ToString();
            superNodeInfo["server"] = superMachine;
            superNodeInfo["port"] = superPort.ToString();
            superNodeInfo["is_profiler"] = "0";
            this.SuperNode = new MvmClusterNode(this, superNodeInfo);
            this.AddKnownNode(this.SuperNode);
            SocketHandler superSocket = this.SuperNode.SocketHandler;

            // Listen for incoming connections
            int profilerPort = StartListener();

            // By now we will have a valid nodeId even when attaching to an existing cluster
            this.mvm.nodeId = this.NodeId;
            this.mvm.workMgr.globalContext["node_id"] = this.NodeId.ToString();

            // Update the supernode with our machine/port info
            Dictionary<string, string> profilerNodeInfo = new Dictionary<string, string>();
            profilerNodeInfo["node_id"] = this.NodeId.ToString();
            profilerNodeInfo["server"] = machine;
            profilerNodeInfo["port"] = profilerPort.ToString();
            profilerNodeInfo["is_profiler"] = "1";
            profilerNodeInfo["is64bit"] = IntPtr.Size == 8 ? "1" : "0";
            profilerNodeInfo["pid"] = MvmEngine.ProcessId.ToString();
            superSocket.messageOutbox.Add(new UpdateNodeMessage(profilerNodeInfo));

            this.mvm.Log("Profiler is waiting to get updated node info from supernode...");
            this.nodesReadyEvent.WaitOne();
            this.mvm.Log("Profiler received updated node info from supernode");

            // We are open for business after this point
            if (attach)
            {
                this.ProfileAllNodes();
            }
        }

        public void AddToProfile(int nodeId, List<List<StackFrame>> stackList)
        {
            lock (stackInfo)
            {
                if (!stackInfo.ContainsKey(nodeId))
                {
                    stackInfo[nodeId] = new ProfileRoot(nodeId, this);
                    dummyFrame[nodeId] = new StackFrame();
                    dummyFrame[nodeId].nodeId = nodeId;
                }
                ProfileRoot root = this.stackInfo[nodeId];
                foreach (var stack in stackList)
                {
                    stack.Insert(0, dummyFrame[nodeId]);
                    root.Add(nodeId, stack);
                }
            }
        }

        public void WriteProfile()
        {
            // Write the whole thing to a temp file, then move it, to reduce the probability of
            // incomplete/corrupted output when we're brought down by shutdown/abort.
            string profile = this.mvm.rmpLogDir + Path.DirectorySeparatorChar + "MVM_PROFILE.txt";
            string profileTemp = this.mvm.rmpLogDir + Path.DirectorySeparatorChar + "MVM_PROFILE.temp.txt";
            lock (stackInfo)
            {
                using (TextWriter tw = new StreamWriter(profileTemp))
                {
                    foreach (var n in this.stackInfo)
                    {
                        List<ProfileNodeSummary> results = n.Value.Results();
                        foreach (var node in from r in results orderby r.inclusive, r.exclusive select r)
                        {
                            tw.WriteLine(node.label() + "\t" + node.createDate.ToString(MvmClusterProfiler.dateFormat) + "\t" + node.depth.ToString() + "\t" + node.count.ToString() + "\t" + node.descendantCount.ToString() + "\t" + node.inclusive.ToString() + "\t" + node.exclusive.ToString());
                        }
                    }
                }
                FileUtils2.MoveWithReplace(profileTemp, profile);
            }
        }
    }

    public class ProfilerWriteThread
    {
        public int profilerOutputPeriod;  // milliseconds to sleep in between writing
        MvmClusterProfiler profiler;
        public ProfilerWriteThread(MvmClusterProfiler profiler)
        {
            this.profiler = profiler;
            this.profilerOutputPeriod = profiler.profilerOutputPeriod;
        }
        public void Run()
        {
            while (true)
            {
                Thread.Sleep(this.profilerOutputPeriod);
                this.profiler.WriteProfile();
            }
        }
    }
}
