using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
using System.Threading;

namespace MVM
{
    /// <summary>
    /// The profiler node sends this message to tell other nodes to create their profiler threads
    /// </summary>
    public class ProfilerInitMessage : MvmMessage
    {
        public int samplingPeriod;
        public int reportingCount;

        public override int Priority
        {
            get { return MessagePriority.InteruptProcCall; }
        }

        public ProfilerInitMessage(MvmMessageDeserializer dummy)
            : base(dummy)
        {
        }

        /// <summary>
        /// Constructor for a new message
        /// </summary>
        /// <param name="indexName"></param>
        public ProfilerInitMessage(int samplingPeriod, int reportingCount)
            : base()
        {
            this.samplingPeriod = samplingPeriod;
            this.reportingCount = reportingCount;
        }

        /// <summary>
        /// Constructor for deserialized message
        /// </summary>
        /// <param name="messageId"></param>
        protected ProfilerInitMessage(long messageId, int samplingPeriod, int reportingCount)
            : base(messageId)
        {
            this.samplingPeriod = samplingPeriod;
            this.reportingCount = reportingCount;
        }

        /// <summary>
        /// Returns the message type
        /// </summary>
        public override byte MessageType { get { return ProfilerInitMessage.StaticMessageType; } }
        public static readonly byte StaticMessageType = 42;

        protected override MvmMessage DeserializeMessagePayload(MessageReceiver receiver, long messageId, BinaryReader breader)
        {
            int samplingPeriod = breader.ReadInt32();
            int reportingCount = breader.ReadInt32();
            return new ProfilerInitMessage(messageId, samplingPeriod, reportingCount);
        }

        protected override void SerializeMessagePayload(BinaryWriter bwriter)
        {
            bwriter.Write(this.samplingPeriod);
            bwriter.Write(this.reportingCount);
        }

        // This message will start the profiler thread on this worker node
        public override void ProcessMessage(MessageReceiver receiver)
        {
            MvmClusterBase cluster = receiver.mvm.mvmCluster;
            lock (cluster.startProfilerEvent)
            {
                if (cluster.profilerThread == null)
                {
                    ProfilerThread profilerThread = new ProfilerThread(cluster, receiver.socketHandler, this.samplingPeriod, this.reportingCount);
                    ThreadStart profilerThreadStart = new ThreadStart(profilerThread.Run);
                    Thread thread = new Thread(profilerThreadStart);
                    thread.Name = "P" + receiver.clientNodeId;
                    thread.Start();
                    cluster.profilerThread = profilerThread;
                }

                // If this is the supernode, also need to make sure the profiler node is running
                if (cluster.IsSuperNode && cluster.GetProfilerNode() == null && !cluster.profilerNodeStarted)
                {
                    ProfilerNodeStarter starter = new ProfilerNodeStarter(receiver.mvmClusterSuper, receiver.socketHandler);
                    ThreadStart starterThreadStart = new ThreadStart(starter.Run);
                    Thread starterThread = new Thread(starterThreadStart);
                    starterThread.Name = "ProfilerNodeStarter";
                    starterThread.Start();
                    cluster.profilerNodeStarted = true;
                }
            }
        }
    }

    public class ProfilerThread
    {
        public volatile int samplingPeriod;  // milliseconds to sleep in between sampling
        public volatile int reportingCount;  // number of times to sample in between reporting results to the profiler node
        MvmClusterBase cluster;
        SocketHandler requestSocketHandler;
        public ProfilerThread(MvmClusterBase cluster, SocketHandler requestSocketHandler, int samplingPeriod, int reportingCount)
        {
            this.cluster = cluster;
            this.requestSocketHandler = requestSocketHandler;
            this.samplingPeriod = samplingPeriod;
            this.reportingCount = reportingCount;
        }
        public void Run()
        {
            List<List<StackFrame>> stackList = new List<List<StackFrame>>();

            // Block until we can see the profiler node
            this.cluster.mvm.Log("Profiler thread waiting to see the profiler node...");
            while (this.cluster.GetProfilerNode() == null)
            {
                Thread.Sleep(1000);
            }
            this.cluster.mvm.Log("Can access the profiler node, so run profiling thread");

            int rCount = this.reportingCount;
            while (true)
            {
                if (this.cluster.profilerActive)  // don't care about locking this
                {
                    foreach (var workerThread in cluster.mvm.workMgr.threadWorkers)
                    {
                        stackList.Add(workerThread.worker.StackTrace);
                    }
                    if (stackList.Count >= rCount)
                    {
                        List<List<StackFrame>> myStackList = stackList;
                        stackList = new List<List<StackFrame>>();
                        this.cluster.GetProfilerNode().SocketHandler.messageOutbox.Add(new ProfilerStatusMessage(myStackList));
                        rCount = this.reportingCount;
                    }
                }
                Thread.Sleep(this.samplingPeriod);
            }
        }
    }

    public class ProfilerNodeStarter
    {
        MvmClusterSuper cluster;
        SocketHandler requestSocketHandler;
        public ProfilerNodeStarter(MvmClusterSuper cluster, SocketHandler requestSocketHandler)
        {
            this.cluster = cluster;
            this.requestSocketHandler = requestSocketHandler;
        }
        public void Run()
        {
            this.cluster.mvm.Log("Supernode is starting up the profiler node...");
            MvmClusterNode node = cluster.RegisterConfiguredNode(new Dictionary<string, string>{
                {"server", MvmEngine.MachineName},
                {"bin", MvmEngine.ExecutableDir},
                {"exe", MvmEngine.ExecutableName},
                {"is64bit", IntPtr.Size == 8 ? "1" : "0"},
                {"is_profiler", "1"}
            });
            cluster.StartupNodes();
            this.cluster.mvm.Log("Done starting the profiler node");
        }
    }
}
