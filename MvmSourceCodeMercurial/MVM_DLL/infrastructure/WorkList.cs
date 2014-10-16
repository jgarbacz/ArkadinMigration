using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MVM
{
    /// <summary>
    /// Thread safe class for managing a workQueue and workStack.
    /// </summary>
    class WorkList
    {
        public SchedulerMaster schedulerMaster;
        public WorkMgr workMgr;

        private Stack<ProcInst> workStack = new Stack<ProcInst>();
        private Queue<ProcInst> workQueue = new Queue<ProcInst>();

        /// <summary>
        /// Dumps text representation to screen
        /// </summary>
        public void PrintString()
        {
            Console.WriteLine(GetString());
        }

        /// <summary>
        /// Returns text representation of contents
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            lock (this)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SYNC WORK:");
                foreach (var work in workQueue)
                {
                    sb.AppendLine(".." + work.ToString(this.schedulerMaster, this.workMgr));
                }
                sb.AppendLine("WORK:");
                foreach (var work in workStack)
                {
                    sb.AppendLine(".." + work.ToString(this.schedulerMaster, this.workMgr));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Returns total work in the worklist
        /// </summary>
        public int Count
        {
            get
            {
                lock (this)
                {
                    return this.workQueue.Count + this.workStack.Count;
                }
            }
        }

        /// <summary>
        /// Add work to stack
        /// </summary>
        /// <param name="work"></param>
        public void ProduceStackWork(ProcInst work)
        {
            lock (this)
            {
                //Console.WriteLine("PRODUCE_STACK, " + procInst.ToString());
                workStack.Push(work);
            }
        }

        /// <summary>
        /// Add work to queue
        /// </summary>
        /// <param name="work"></param>
        public void ProduceQueueWork(ProcInst work)
        {
            lock (this)
            {
                //Console.WriteLine("PRODUCE_QUEUE, " + procInst.ToString());
                workQueue.Enqueue(work);
            }
        }

        /// <summary>
        ///  Returns queue work if any, else stack work if any, else null
        /// </summary>
        /// <returns></returns>
        public ProcInst ConsumeWork()
        {
            lock (this)
            {
                if (workQueue.Count > 0)
                {
                    ProcInst work = workQueue.Dequeue();
                    //Console.WriteLine("CONSUME_QUEUE, " + procInst.ToString());
                    return work;
                }
                if (workStack.Count > 0)
                {
                    ProcInst work = workStack.Pop();
                    //Console.WriteLine("CONSUME_STACK, " + procInst.ToString());
                    return work;
                }
            }
            // otherwise, no procInst right now, return null.
            return null;
        }

    }
}
