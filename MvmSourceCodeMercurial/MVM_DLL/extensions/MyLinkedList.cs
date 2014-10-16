using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
namespace MVM
{

    // extension methods for LinkedList<T>
    static class MyLinkedList
    {
        /// <summary>
        /// Enqueue as if the linked list is a queue. Adds item to end of list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="LinkedList"></param>
        /// <param name="item"></param>
        public static void Enqueue<T>(this LinkedList<T> linkedList,T item)
        {
            linkedList.AddLast(item);
        }
        /// <summary>
        /// Dequeue as if the linked list is a queue. Returns first item or null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="LinkedList"></param>
        /// <returns></returns>
        public static T Dequeue<T>(this LinkedList<T> linkedList)
        {
            if (linkedList.Count == 0) return default(T);
            T item=linkedList.First.Value;
            linkedList.RemoveFirst();
            return item;
        }
        /// <summary>
        /// Peek as if the linked list is a queue. Returns first item or null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="LinkedList"></param>
        /// <returns></returns>
        public static T Peek<T>(this LinkedList<T> linkedList)
        {
            if (linkedList.Count == 0) return default(T);
            T item = linkedList.First.Value;
            return item;
        }
        /// <summary>
        /// Peek as if the linked list is a queue. Returns Nth item (starting from 1...N) or null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="LinkedList"></param>
        /// <returns></returns>
        public static T Peek<T>(this LinkedList<T> linkedList,int n)
        {
            //Debug.Assert(n != null, "n=["+n+"] must be >=1");
            if (linkedList.Count < n) return default(T);
            var node = linkedList.First;
            for (int i = 1; i < n; i++) node = node.Next;
            T item =  node.Value;
            return item;
        }

        /// <summary>
        /// Makes a shallow copy of the LinkedList.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="LinkedList"></param>
        /// <returns></returns>
        public static LinkedList<T> CopyShallow<T>(this LinkedList<T> LinkedList)
        {
            LinkedList<T> copy = new LinkedList<T>();
            foreach (var item in LinkedList) copy.AddLast(item);
            return copy;
        }

       
    }

}
