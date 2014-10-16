using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    /// <summary>
    /// Combines features of a queue and a stack built on a LinkedList. This structure
    /// is also sometimes called a double ended queue. LinkedList.Last is next item return
    /// by the queue and LinkedList.First is the next item returned by the stack.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Quack<T> : LinkedList<T>
    {
        # region Queue Methods
        //
        // Summary:
        //     Adds an object to the end of the Quack<T>.
        //
        // Parameters:
        //   item:
        //     The object to add to the Quack<T>. The value can
        //     be null for reference types.
        public void Enqueue(T value)
        {
            this.AddLast(value);
        }
        //
        // Summary:
        //     Removes and returns the object at the beginning of the Quack<T>.
        //
        // Returns:
        //     The object that is removed from the beginning of the Quack<T>.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The Quack<T> is empty.
        public T Dequeue()
        {
            var last = this.Last.Value;
            this.RemoveLast();
            return last;
        }
        //
        // Summary:
        //     Returns the object at the beginning of the Quack<T>
        //     without removing it.
        //
        // Returns:
        //     The object at the beginning of the Quack<T>.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The Quack<T> is empty.
        public T Peek()
        {
            if (this.Count == 0) throw new System.InvalidOperationException("Cannot Peek and empty Quack");
            return this.Last.Value;
        }
        # endregion

        # region Stack Methods
        
        //
        // Summary:
        //     Inserts an object at the top of the Quack<T>.
        //
        // Parameters:
        //   item:
        //     The object to push onto the Quack<T>. The value
        //     can be null for reference types.
        public void StackPush(T value)
        {
            this.AddFirst(value);
        }
        //
        // Summary:
        //     Removes and returns the object at the top of the Quack<T>.
        //
        // Returns:
        //     The object removed from the top of the Quack<T>.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The Quack<T> is empty.
        public T StackPop()
        {
            var first = this.Last.Value;
            this.RemoveFirst();
            return first;
        }
        //
        // Summary:
        //     Returns the object at the top of the Quack<T>
        //     without removing it.
        //
        // Returns:
        //     The object at the top of the Quack<T>.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The Quack<T> is empty.
        public T StackPeek()
        {
            if (this.Count == 0) throw new System.InvalidOperationException("Cannot StackPeek and empty Quack");
            return this.First.Value;
        }
        # endregion
    }
}
