using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{

    /// <summary>
    /// This priority queue implementation has the following characteristics.
    /// - Respects absolute priority as an integer where lower values are higher priority then higher values.
    /// - Respects relative priority of HighestBefore, HighestAfter, LowestBefore, LowestAfter, RightBefore, and RightAfter an absolute priority. 
    /// - Thread safe for a single producer and a single consumer to access concurrently.
    /// TBD: structures are setup to for relative priority but need to code the enqueue methods
    /// TBD: right now this does a trivial lock() on the whole object, we can tune this.
    /// TBD: relative priority queues should get purged.
    /// </summary>
    public class ProducerConsumerPriorityQueue<T>
    {
        public class MyQueue : Queue<T>
        {
            public int priority;
            public MyQueue(int priority)
            {
                this.priority = priority;
            }
        }

        // Linked list that holds the priority ordered queues
        private LinkedList<MyQueue> hiToLoQueues = new LinkedList<MyQueue>();

        // First node in priorityQueue that has elements
        private LinkedListNode<MyQueue> headNode = null;

        // Direct link from absolute priority to the linked list node
        // absolutePriorityMapping[priority]=MyQueueNode
        private Dictionary<int, LinkedListNode<MyQueue>> absolutePriorityMapping = new Dictionary<int, LinkedListNode<MyQueue>>();

        // Ordered list of absolute priority nodes ordered by priority.
        private List<LinkedListNode<MyQueue>> hiToLoAbsolutePriorityNodes = new List<LinkedListNode<MyQueue>>();

        // Total count across all priorities.
        public int Count = 0;

        /// <summary>
        /// Adds the message at the passed priority. Updates the head if needed.
        /// </summary>
        /// <param name="item">Item to Enqueue</param>
        /// <param name="priority">Higher int is higher priority</param>
        public void Enqueue(T item, int priority)
        {
            lock (this)
            {
                MyQueue myQueue = null;
                LinkedListNode<MyQueue> myQueueNode;
                if (!absolutePriorityMapping.TryGetValue(priority, out myQueueNode))
                {
                    myQueue = new MyQueue(priority);
                    myQueueNode = new LinkedListNode<MyQueue>(myQueue);
                    absolutePriorityMapping[priority] = myQueueNode;
                    // TBD:  should use binary search on HiToLoAbsolutePriorityNodes
                    var insertBefore = hiToLoAbsolutePriorityNodes
                        .SelectIndexValuePairs()
                        .Where(p => myQueue.priority > p.value.Value.priority)
                        .FirstOrDefault();
                    if (insertBefore == null)
                    {
                        hiToLoQueues.AddLast(myQueueNode);
                        hiToLoAbsolutePriorityNodes.Add(myQueueNode);
                    }
                    else
                    {
                        var insertBeforeIdx = insertBefore.index;
                        var insertBeforeNode = insertBefore.value;
                        hiToLoQueues.AddBefore(insertBeforeNode, myQueueNode);
                        hiToLoAbsolutePriorityNodes.Insert(insertBeforeIdx, insertBeforeNode);
                    }
                }
                else
                {
                    myQueue = myQueueNode.Value;
                }
                myQueue.Enqueue(item);
                Count++;
                // if our message is higher priority then the head, update the head
                if (headNode == null || myQueue.priority > headNode.Value.priority) headNode = myQueueNode;
            }
        }

        /// <summary>
        /// Makes sure the head is the highest priority node with data. This will update
        /// it from its current spot lower priorities if no data at current spot.
        /// </summary>
        /// <param name="currHead"></param>
        /// <returns></returns>
        private LinkedListNode<MyQueue> GetHeadNode(LinkedListNode<MyQueue> currHead)
        {
            if (headNode == null) return null;
            while (currHead.Value.Count == 0)
            {
                currHead = currHead.Next;
                if (currHead == null) return null;
            }
            return currHead;
        }

        /// <summary>
        /// Dequeues the highest priority message or null
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            lock (this)
            {
                headNode = GetHeadNode(headNode);
                if (headNode == null) return default(T);
                T message = headNode.Value.Dequeue();
                Count--;
                return message;
            }
        }
        /// <summary>
        /// Dequeues item if greater than or equal to minPriority.
        /// </summary>
        /// <returns></returns>
        public T DequeueMin(int minPriority)
        {
            lock (this)
            {
                headNode = GetHeadNode(headNode);
                if (headNode == null) return default(T);
                if (headNode.Value.priority < minPriority) return default(T);
                T message = headNode.Value.Dequeue();
                Count--;
                return message;
            }
        }
        /// <summary>
        /// Dequeues item at specificPriority.
        /// </summary>
        /// <returns></returns>
        public T DequeueSpecific(int specificPriority)
        {
            lock (this)
            {
                LinkedListNode<MyQueue> specificQueueNode;
                if (absolutePriorityMapping.TryGetValue(specificPriority, out specificQueueNode))
                {
                    if (specificQueueNode.Value.Count > 0)
                    {
                        Count--; 
                        return specificQueueNode.Value.Dequeue();
                    }
                }
                return default(T);
            }
        }

        /// <summary>
        /// Returns the highest priority item without removing it
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            lock (this)
            {
                if(Count==0) return default(T);
                headNode = GetHeadNode(headNode);
                return headNode.Value.Peek();
            }
        }
    }

    public static class TestMyQueue
    {
        public static void TestProducerConsumerPriorityQueue()
        {
            ProducerConsumerPriorityQueue<string> q = new ProducerConsumerPriorityQueue<string>();
            q.Enqueue("1-1", 1);
            q.Enqueue("1-2", 1);
            q.Enqueue("1-3", 1);
            q.Enqueue("5-1", 5);
            q.Enqueue("5-2", 5);
            q.Enqueue("5-3", 5);
            q.Enqueue("0-1", 0);

            Console.WriteLine("count=" + q.Count);

            string m;
            while ((m = q.Dequeue()) != null)
            {
                Console.WriteLine(m);
            }
            Console.WriteLine("count=" + q.Count);

            q.Enqueue("1-1", 1);
            q.Enqueue("1-2", 1);
            q.Enqueue("1-3", 1);
            q.Enqueue("5-1", 5);
            q.Enqueue("5-2", 5);
            q.Enqueue("5-3", 5);
            q.Enqueue("0-1", 0);

            while ((m = q.DequeueMin(1)) != null)
            {
                Console.WriteLine(m);
            }
        }
    }


}


