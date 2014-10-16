using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using System.Threading;
namespace MVM
{
    public class AsyncShutdown
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        MvmClusterBase mvmCluster;
        //MvmClusterSuper mvmClusterSuper;
        long workId;

        public AsyncShutdown(MvmClusterBase mvmCluster, long workId)
        {
            this.mvmCluster = mvmCluster;
            this.workId = workId;
        }

        public virtual void Shutdown()
        {
            // Stop accepting new clients before we start shutting anything down.
            logger.Info("Shutting down my listener");
            this.mvmCluster.MyListener.StopAcceptingClients();


            // If this node is the super node, shutdown all other known nodes before turning off self
            if (this.mvmCluster.IsSuperNode)
            {
                // I DO NOT THINK THIS CODE SHOULD BE CALLED ANYMORE
                if (1==1) throw new Exception("THIS SHOULD NO LONGER BE USED");
                //this.mvmClusterSuper = this.mvmCluster as MvmClusterSuper;
                //this.mvmClusterSuper.ShutdownSlaves();
                //logger.Info("Shutdown the engine threads");
                //this.mvmCluster.mvm.SetServerModeOff();
                //this.mvmCluster.mvm.workMgr.WakeUp();

                //logger.Info("Super node is now shutdown");
            }
            else
            {
                logger.Info("I am a worker node so I just need to shut myself down.");

                logger.Info("Disconnect all non-super sockets");
                this.mvmCluster.DisconnectNonSuperNodes();

                logger.Info("Shutdown the worker threads");
                this.mvmCluster.mvm.SetServerModeOff();
                this.mvmCluster.mvm.workMgr.ShutdownWorkers();

                if (this.mvmCluster.IsProfilerNode)
                {
                    logger.Info("Dumping profiler report");
                    (this.mvmCluster as MvmClusterProfiler).WriteProfile();
                }

                logger.Info("Send message to super that i am shutdown-super. workId=" + workId);
                ShutdownResponse response = new ShutdownResponse(workId, this.mvmCluster.NodeId);
                this.mvmCluster.SuperNode.SocketHandler.messageSender.SendImmediate(response);

                logger.Info("Disconnect from super");
                this.mvmCluster.SuperNode.Disconnect();

                this.mvmCluster.mvm.ShutdownSlave();
            }
            logger.Info("Shutdown thread '" + Thread.CurrentThread.Name + "' exiting");
        }
    }
}
