/*  
  Copyright 2007-2009 The NGenerics Team
 (http://code.google.com/p/ngenerics/wiki/Team)

 This program is licensed under the GNU Lesser General Public License (LGPL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://www.gnu.org/copyleft/lesser.html.
*/


using System;
using System.Collections;
using System.Collections.Generic;
using NGenerics.DataStructures.Queues;
using NGenerics.Patterns.Visitor;
using NGenerics.Tests.Util;
using NUnit.Framework;

namespace NGenerics.Tests.DataStructures.Queues {
    [TestFixture]
    public class MaxPriorityQueueTest {
        #region Tests

        [TestFixture]
        public class Accept {

            [Test]
            public void Done() {
                var priorityQueue = GetSimpleTestPriorityQueue();
                var visitor = new CompletedTrackingVisitor<string>();
                priorityQueue.AcceptVisitor(visitor);
            }

            [Test]
            public void Simple() {
                var a = false;
                var b = false;
                var c = false;
                var d = false;
                var e = false;
                var f = false;

                var count = 0;

                var priorityQueue = GetSimpleTestPriorityQueue();
                var visitor = new TrackingVisitor<string>();

                priorityQueue.AcceptVisitor(visitor);

                for (var i = 0; i < visitor.TrackingList.Count; i++) {
                    count++;

                    if (visitor.TrackingList[i] == "a") {
                        a = true;
                    } else if (visitor.TrackingList[i] == "b") {
                        b = true;
                    } else if (visitor.TrackingList[i] == "c") {
                        c = true;
                    } else if (visitor.TrackingList[i] == "d") {
                        d = true;
                    } else if (visitor.TrackingList[i] == "e") {
                        e = true;
                    } else if (visitor.TrackingList[i] == "f") {
                        f = true;
                    }
                }

                Assert.IsTrue(a);
                Assert.IsTrue(b);
                Assert.IsTrue(c);
                Assert.IsTrue(d);
                Assert.IsTrue(e);
                Assert.IsTrue(f);

                Assert.AreEqual(count, 6);
            }

            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ExceptionNullVisitor() {
                var priorityQueue = GetTestPriorityQueue();
                priorityQueue.AcceptVisitor(null);
            }

        }

        [TestFixture]
        public class AddPriorityGroup {

            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ExceptionNullList() {
                var priorityQueue = new PriorityQueue<string, int>(PriorityQueueType.Maximum);

                priorityQueue.AddPriorityGroup(null, 4);
            }

            [Test]
            public void Simple() {
                var priorityQueue = new PriorityQueue<string, int>(PriorityQueueType.Maximum);

                var stringValues = new[] { "a", "b", "c", "d" };
                priorityQueue.AddPriorityGroup(stringValues, 4);

                stringValues = new[] { "e", "f", "g", "h" };
                priorityQueue.AddPriorityGroup(stringValues, 2);

                int priority;

                Assert.AreEqual(priorityQueue.Dequeue(out priority), "a");
                Assert.AreEqual(priority, 4);

                Assert.AreEqual(priorityQueue.Dequeue(out priority), "b");
                Assert.AreEqual(priority, 4);

                Assert.AreEqual(priorityQueue.Dequeue(out priority), "c");
                Assert.AreEqual(priority, 4);

                Assert.AreEqual(priorityQueue.Dequeue(out priority), "d");
                Assert.AreEqual(priority, 4);

                Assert.AreEqual(priorityQueue.Dequeue(out priority), "e");
                Assert.AreEqual(priority, 2);

                Assert.AreEqual(priorityQueue.Dequeue(out priority), "f");
                Assert.AreEqual(priority, 2);

                Assert.AreEqual(priorityQueue.Dequeue(out priority), "g");
                Assert.AreEqual(priority, 2);

                Assert.AreEqual(priorityQueue.Dequeue(out priority), "h");
                Assert.AreEqual(priority, 2);
            }

        }

        [TestFixture]
        public class Add {
            [Test]
            public void Simple() {
                var priorityQueue1 = new PriorityQueue<int, int>(PriorityQueueType.Maximum) { 4 };

                Assert.AreEqual(priorityQueue1.Count, 1);
                Assert.AreEqual(priorityQueue1.Dequeue(), 4);

                priorityQueue1.Add(5);
                priorityQueue1.Add(6, 2);

                Assert.AreEqual(priorityQueue1.Dequeue(), 6);
                Assert.AreEqual(priorityQueue1.Dequeue(), 5);

                priorityQueue1.Add(6, 2);
                priorityQueue1.Add(5);

                Assert.AreEqual(priorityQueue1.Dequeue(), 6);
                Assert.AreEqual(priorityQueue1.Dequeue(), 5);

                var priorityQueue2 = new PriorityQueue<string, int>(PriorityQueueType.Maximum)
                                         {
                                             {"a", 1},
                                             {"b", 2},
                                             {"c", 3},
                                             {"d", 4},
                                             {"e", 5},
                                             {"f", 6},
                                             {"z", 6},
                                             {"y", 5},
                                             {"x", 4},
                                             {"w", 3},
                                             {"v", 2},
                                             {"u", 1},
                                             {"z", 1},
                                             {"y", 2},
                                             {"x", 3},
                                             {"w", 4},
                                             {"v", 5},
                                             {"u", 6}
                                         };

                Assert.AreEqual(priorityQueue2.Count, 18);

                priorityQueue2.Clear();

                Assert.AreEqual(priorityQueue2.Count, 0);
            }

            // Unit test contributed by exyll (see the "PriorityQueue is LIFO instead of FIFO" work item.
            [Test]
            public void FifoSamePriority() {
                var priorityQueue = new PriorityQueue<object, int>(PriorityQueueType.Maximum);

                var o1 = new object();
                var o2 = new object();
                var o3 = new object();

                priorityQueue.Enqueue(o1);
                priorityQueue.Enqueue(o2);
                priorityQueue.Enqueue(o3);

                Assert.AreSame(o1, priorityQueue.Dequeue());
                Assert.AreSame(o2, priorityQueue.Dequeue());
                Assert.AreSame(o3, priorityQueue.Dequeue());
            }
        }

        [TestFixture]
        public class Construction {
            [Test]
            public void Simple() {
                var priorityQueue = new PriorityQueue<string, int>(PriorityQueueType.Maximum);
                Assert.AreEqual(priorityQueue.Count, 0);
            }

            [Test]
            public void WithComparison() {
                var priorityQueue = new PriorityQueue<string, int>(PriorityQueueType.Maximum, (x, y) => (y.CompareTo(x)));
                Assert.AreEqual(priorityQueue.Count, 0);

                priorityQueue.Add("a", 1);
                priorityQueue.Add("b", 2);
                priorityQueue.Add("d", 4);
                priorityQueue.Add("c", 3);

                // The custom comparer switches returns results the other way around than what it should be -
                // The items should come out in a minium fashion.
                Assert.AreEqual(priorityQueue.Dequeue(), "a");
                Assert.AreEqual(priorityQueue.Dequeue(), "b");
                Assert.AreEqual(priorityQueue.Dequeue(), "c");
                Assert.AreEqual(priorityQueue.Dequeue(), "d");
            }


            [Test]
            public void WithIComparer() {
                var priorityQueue = new PriorityQueue<string, int>(PriorityQueueType.Maximum, new SimpleReverseComparer());
                Assert.AreEqual(priorityQueue.Count, 0);

                priorityQueue.Add("a", 1);
                priorityQueue.Add("b", 2);
                priorityQueue.Add("d", 4);
                priorityQueue.Add("c", 3);

                // The custom comparer switches returns results the other way around than what it should be -
                // The items should come out in a minium fashion.
                Assert.AreEqual(priorityQueue.Dequeue(), "a");
                Assert.AreEqual(priorityQueue.Dequeue(), "b");
                Assert.AreEqual(priorityQueue.Dequeue(), "c");
                Assert.AreEqual(priorityQueue.Dequeue(), "d");
            }

            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void WithNonComparableItem() {
                var priorityQueue = new PriorityQueue<string, NonComparable>(PriorityQueueType.Minimum);
                Assert.AreEqual(priorityQueue.Count, 0);

                priorityQueue.Add("a", new NonComparable());
                priorityQueue.Add("b", new NonComparable());
            }

            private class NonComparable {

            }

            private class SimpleReverseComparer : IComparer<int> {
                #region IComparer<int> Members

                public int Compare(int x, int y) {
                    return y.CompareTo(x);
                }

                #endregion
            }

        }

        [TestFixture]
        public class Count {

            [Test]
            public void Simple() {
                var priorityQueue = new PriorityQueue<int, int>(PriorityQueueType.Maximum);
                Assert.AreEqual(priorityQueue.Count, 0);

                priorityQueue.Add(4);
                Assert.AreEqual(priorityQueue.Count, 1);

                priorityQueue.Add(99);
                Assert.AreEqual(priorityQueue.Count, 2);

                priorityQueue.Clear();
                Assert.AreEqual(priorityQueue.Count, 0);
            }

        }

        [TestFixture]
        public class Contains {

            [Test]
            public void Simple() {
                var priorityQueue = GetTestPriorityQueue();

                Assert.IsTrue(priorityQueue.Contains("a"));
                Assert.IsTrue(priorityQueue.Contains("b"));
                Assert.IsTrue(priorityQueue.Contains("c"));
                Assert.IsTrue(priorityQueue.Contains("d"));
                Assert.IsTrue(priorityQueue.Contains("e"));
                Assert.IsTrue(priorityQueue.Contains("f"));

                Assert.IsFalse(priorityQueue.Contains("g"));
            }

        }

        [TestFixture]
        public class CopyTo {

            [Test]
            public void Simple() {
                var priorityQueue = GetTestPriorityQueue();
                var array = new string[priorityQueue.Count];

                priorityQueue.CopyTo(array, 0);

                var list = new List<string>(array);

                Assert.AreEqual(list.Count, priorityQueue.Count);

                Assert.IsTrue(list.Contains("a"));
                Assert.IsTrue(list.Contains("b"));
                Assert.IsTrue(list.Contains("c"));
                Assert.IsTrue(list.Contains("d"));
                Assert.IsTrue(list.Contains("e"));
                Assert.IsTrue(list.Contains("f"));

                Assert.IsTrue(list.Contains("u"));
                Assert.IsTrue(list.Contains("v"));
                Assert.IsTrue(list.Contains("w"));
                Assert.IsTrue(list.Contains("x"));
                Assert.IsTrue(list.Contains("y"));
                Assert.IsTrue(list.Contains("z"));
            }

            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ExceptionNullArray() {
                var priorityQueue = new PriorityQueue<string, int>(PriorityQueueType.Maximum);
                priorityQueue.CopyTo(null, 0);
            }

            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void ExceptionInvalid1() {
                var priorityQueue = GetTestPriorityQueue();

                var array = new string[priorityQueue.Count - 1];
                priorityQueue.CopyTo(array, 0);
            }

            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void ExceptionInvalid2() {
                var priorityQueue = GetTestPriorityQueue();

                var array = new string[priorityQueue.Count];
                priorityQueue.CopyTo(array, 1);
            }

        }

        [TestFixture]
        public class Dequeue {

            [Test]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ExceptionEmpty() {
                var priorityQueue = new PriorityQueue<string, int>(PriorityQueueType.Maximum);
                priorityQueue.Dequeue();
            }

            [Test]
            public void WithPriority() {
                var priorityQueue = new PriorityQueue<string, int>(PriorityQueueType.Maximum);
                priorityQueue.Enqueue("dog", 2);
                priorityQueue.Enqueue("canary", 1);
                priorityQueue.Enqueue("cat", 3);

                int priority;

                // Peek gives us "cat"
                Assert.AreEqual("cat", priorityQueue.Dequeue(out priority));

                // With priority 3
                Assert.AreEqual(priority, 3);

                // Peek gives us "dog"
                Assert.AreEqual("dog", priorityQueue.Dequeue(out priority));

                // With priority 2
                Assert.AreEqual(priority, 2);

                // Peek gives us "canary"
                Assert.AreEqual("canary", priorityQueue.Dequeue(out priority));

                // With priority 1
                Assert.AreEqual(priority, 1);
            }

            [Test]
            public void Sequencing() {
                var priorityQueue = GetTestPriorityQueue();

                // z or f
                var str = priorityQueue.Dequeue();
                Assert.IsTrue(((str == "z") || (str == "f")));

                str = priorityQueue.Dequeue();
                Assert.IsTrue(((str == "z") || (str == "f")));

                // y or e 
                str = priorityQueue.Dequeue();
                Assert.IsTrue(((str == "y") || (str == "e")));

                str = priorityQueue.Dequeue();
                Assert.IsTrue(((str == "y") || (str == "e")));

                // x or d
                str = priorityQueue.Dequeue();
                Assert.IsTrue(((str == "x") || (str == "d")));

                str = priorityQueue.Dequeue();
                Assert.IsTrue(((str == "x") || (str == "d")));

                // w or c
                str = priorityQueue.Dequeue();
                Assert.IsTrue(((str == "w") || (str == "c")));

                str = priorityQueue.Dequeue();
                Assert.IsTrue(((str == "w") || (str == "c")));

                // v or b
                str = priorityQueue.Dequeue();
                Assert.IsTrue(((str == "v") || (str == "b")));

                str = priorityQueue.Dequeue();
                Assert.IsTrue(((str == "v") || (str == "b")));

                // u or a
                str = priorityQueue.Dequeue();
                Assert.IsTrue(((str == "u") || (str == "a")));

                str = priorityQueue.Dequeue();
                Assert.IsTrue(((str == "u") || (str == "a")));
            }

        }

        [TestFixture]
        public class DefaultPriority {

            [Test]
            public void Setter() {
                var priorityQueue = new PriorityQueue<string, int>(PriorityQueueType.Maximum);
                Assert.AreEqual(priorityQueue.DefaultPriority, 0);

                priorityQueue.DefaultPriority = 2;
                Assert.AreEqual(priorityQueue.DefaultPriority, 2);

                priorityQueue.DefaultPriority = -1;
                Assert.AreEqual(priorityQueue.DefaultPriority, -1);
            }

            [Test]
            public void Enqueue() {
                var priorityQueue = new PriorityQueue<string, int>(PriorityQueueType.Maximum) { DefaultPriority = 2 };
                priorityQueue.Enqueue("test1");
                priorityQueue.Enqueue("test2", 3);

                Assert.AreEqual(priorityQueue.Dequeue(), "test2");
                Assert.AreEqual(priorityQueue.Dequeue(), "test1");

                priorityQueue.Enqueue("test1");
                priorityQueue.Enqueue("test2", 1);

                Assert.AreEqual(priorityQueue.Dequeue(), "test1");
                Assert.AreEqual(priorityQueue.Dequeue(), "test2");
            }

        }

        [TestFixture]
        public class Enqueue {

            [Test]
            public void Simple() {
                var priorityQueue1 = new PriorityQueue<int, int>(PriorityQueueType.Maximum);
                priorityQueue1.Enqueue(4);

                Assert.AreEqual(priorityQueue1.Count, 1);
                Assert.AreEqual(priorityQueue1.Dequeue(), 4);

                priorityQueue1.Enqueue(5);
                priorityQueue1.Enqueue(6, 2);

                Assert.AreEqual(priorityQueue1.Dequeue(), 6);
                Assert.AreEqual(priorityQueue1.Dequeue(), 5);

                priorityQueue1.Enqueue(6, 2);
                priorityQueue1.Enqueue(5);

                Assert.AreEqual(priorityQueue1.Dequeue(), 6);
                Assert.AreEqual(priorityQueue1.Dequeue(), 5);

                var priorityQueue2 = new PriorityQueue<string, int>(PriorityQueueType.Maximum);

                priorityQueue2.Enqueue("a", 1);
                priorityQueue2.Enqueue("b", 2);
                priorityQueue2.Enqueue("c", 3);
                priorityQueue2.Enqueue("d", 4);
                priorityQueue2.Enqueue("e", 5);
                priorityQueue2.Enqueue("f", 6);

                priorityQueue2.Enqueue("z", 6);
                priorityQueue2.Enqueue("y", 5);
                priorityQueue2.Enqueue("x", 4);
                priorityQueue2.Enqueue("w", 3);
                priorityQueue2.Enqueue("v", 2);
                priorityQueue2.Enqueue("u", 1);

                priorityQueue2.Enqueue("z", 1);
                priorityQueue2.Enqueue("y", 2);
                priorityQueue2.Enqueue("x", 3);
                priorityQueue2.Enqueue("w", 4);
                priorityQueue2.Enqueue("v", 5);
                priorityQueue2.Enqueue("u", 6);

                Assert.AreEqual(priorityQueue2.Count, 18);

                priorityQueue2.Clear();

                Assert.AreEqual(priorityQueue2.Count, 0);
            }

        }

        [TestFixture]
        public class GetPriorityGroup {

            [Test]
            public void Simple() {
                var priorityQueue = GetTestPriorityQueue();

                var list = priorityQueue.GetPriorityGroup(1);

                Assert.IsTrue(list.Contains("a"));
                Assert.IsTrue(list.Contains("u"));

                list = priorityQueue.GetPriorityGroup(2);

                Assert.IsTrue(list.Contains("b"));
                Assert.IsTrue(list.Contains("v"));

                list = priorityQueue.GetPriorityGroup(3);

                Assert.IsTrue(list.Contains("c"));
                Assert.IsTrue(list.Contains("w"));

                list = priorityQueue.GetPriorityGroup(4);

                Assert.IsTrue(list.Contains("d"));
                Assert.IsTrue(list.Contains("x"));

                list = priorityQueue.GetPriorityGroup(5);

                Assert.IsTrue(list.Contains("e"));
                Assert.IsTrue(list.Contains("y"));

                list = priorityQueue.GetPriorityGroup(6);

                Assert.IsTrue(list.Contains("z"));
                Assert.IsTrue(list.Contains("f"));
            }

        }

        [TestFixture]
        public class GetEnumerator {

            [Test]
            public void Simple() {
                var count = 0;
                var a = false;
                var b = false;
                var c = false;
                var d = false;
                var e = false;
                var f = false;

                var priorityQueue = GetSimpleTestPriorityQueue();
                var enumerator = priorityQueue.GetEnumerator();

                while (enumerator.MoveNext()) {
                    count++;

                    if (enumerator.Current == "a") {
                        a = true;
                    } else if (enumerator.Current == "b") {
                        b = true;
                    } else if (enumerator.Current == "c") {
                        c = true;
                    } else if (enumerator.Current == "d") {
                        d = true;
                    } else if (enumerator.Current == "e") {
                        e = true;
                    } else if (enumerator.Current == "f") {
                        f = true;
                    }
                }

                Assert.IsTrue(a);
                Assert.IsTrue(b);
                Assert.IsTrue(c);
                Assert.IsTrue(d);
                Assert.IsTrue(e);
                Assert.IsTrue(f);
                Assert.AreEqual(count, 6);
            }

            [Test]
            public void Interface() {
                var count = 0;
                var a = false;
                var b = false;
                var c = false;
                var d = false;
                var e = false;
                var f = false;

                var priorityQueue = GetSimpleTestPriorityQueue();
                var enumerator = ((IEnumerable)priorityQueue).GetEnumerator();

                while (enumerator.MoveNext()) {
                    count++;

                    if ((string)enumerator.Current == "a") {
                        a = true;
                    } else if ((string)enumerator.Current == "b") {
                        b = true;
                    } else if ((string)enumerator.Current == "c") {
                        c = true;
                    } else if ((string)enumerator.Current == "d") {
                        d = true;
                    } else if ((string)enumerator.Current == "e") {
                        e = true;
                    } else if ((string)enumerator.Current == "f") {
                        f = true;
                    }
                }

                Assert.IsTrue(a);
                Assert.IsTrue(b);
                Assert.IsTrue(c);
                Assert.IsTrue(d);
                Assert.IsTrue(e);
                Assert.IsTrue(f);
                Assert.AreEqual(count, 6);
            }

        }

        [TestFixture]
        public class GetKeyEnumerator {

            [Test]
            public void Simple() {
                var priorityQueue = GetTestPriorityQueue();
                var enumerator = priorityQueue.GetKeyEnumerator();

                var counter = 0;

                while (enumerator.MoveNext()) {
                    counter++;
                }

                Assert.AreEqual(counter, priorityQueue.Count);
            }

        }

        [TestFixture]
        public class IsReadOnly {

            [Test]
            public void Simple() {
                var priorityQueue = new PriorityQueue<int, int>(PriorityQueueType.Maximum);
                Assert.IsFalse(priorityQueue.IsReadOnly);

                priorityQueue.Add(4);
                Assert.IsFalse(priorityQueue.IsReadOnly);

                priorityQueue.Add(99);
                Assert.IsFalse(priorityQueue.IsReadOnly);

                priorityQueue.Clear();
                Assert.IsFalse(priorityQueue.IsReadOnly);
            }

        }


        [TestFixture]
        public class Peek {

            [Test]
            public void Simple() {
                var priorityQueue = new PriorityQueue<string, int>(PriorityQueueType.Maximum);

                priorityQueue.Enqueue("g", 6);
                Assert.AreEqual(priorityQueue.Peek(), "g");

                priorityQueue.Enqueue("h", 5);
                Assert.AreEqual(priorityQueue.Peek(), "g");

                priorityQueue.Enqueue("i", 7);
                Assert.AreEqual(priorityQueue.Peek(), "i");
            }

            [Test]
            public void SimpleWithPriority() {
                var priorityQueue = new PriorityQueue<string, int>(PriorityQueueType.Maximum);
                int priority;

                priorityQueue.Enqueue("g", 6);

                var item = priorityQueue.Peek(out priority);

                Assert.AreEqual(item, "g");
                Assert.AreEqual(priority, 6);
                Assert.AreEqual(priorityQueue.Count, 1);

                priorityQueue.Enqueue("h", 5);

                item = priorityQueue.Peek(out priority);

                Assert.AreEqual(item, "g");
                Assert.AreEqual(priority, 6);
                Assert.AreEqual(priorityQueue.Count, 2);

                priorityQueue.Enqueue("i", 7);

                item = priorityQueue.Peek(out priority);

                Assert.AreEqual(item, "i");
                Assert.AreEqual(priority, 7);
                Assert.AreEqual(priorityQueue.Count, 3);
            }
        }

        [TestFixture]
        public class RemovePriorityGroup {

            [Test]
            public void WithPriority() {
                var priorityQueue = GetTestPriorityQueue();

                Assert.IsTrue(priorityQueue.RemovePriorityGroup(2));
                Assert.AreEqual(priorityQueue.Count, 10);

                Assert.IsTrue(priorityQueue.RemovePriorityGroup(3));
                Assert.AreEqual(priorityQueue.Count, 8);

                Assert.IsTrue(priorityQueue.RemovePriorityGroup(4));
                Assert.AreEqual(priorityQueue.Count, 6);

                Assert.IsTrue(priorityQueue.RemovePriorityGroup(1));
                Assert.AreEqual(priorityQueue.Count, 4);

                // z or f
                var str = priorityQueue.Dequeue();
                Assert.IsTrue(((str == "z") || (str == "f")));

                str = priorityQueue.Dequeue();
                Assert.IsTrue(((str == "z") || (str == "f")));

                // y or e 
                str = priorityQueue.Dequeue();
                Assert.IsTrue(((str == "y") || (str == "e")));

                str = priorityQueue.Dequeue();
                Assert.IsTrue(((str == "y") || (str == "e")));
            }

        }

        [TestFixture]
        public class Remove {

            [Test]
            public void Simple() {
                var priorityQueue = new PriorityQueue<string, int>(PriorityQueueType.Maximum) { "5" };

                Assert.AreEqual(priorityQueue.Count, 1);
                Assert.IsTrue(priorityQueue.Contains("5"));
                Assert.IsTrue(priorityQueue.Remove("5"));
                Assert.AreEqual(priorityQueue.Count, 0);

                priorityQueue.Add("6");
                priorityQueue.Add("7");
                priorityQueue.Add("2", 4);

                Assert.AreEqual(priorityQueue.Count, 3);
                Assert.IsTrue(priorityQueue.Contains("6"));
                Assert.IsTrue(priorityQueue.Contains("7"));
                Assert.IsTrue(priorityQueue.Contains("2"));

                Assert.IsFalse(priorityQueue.Remove("8"));

                Assert.IsTrue(priorityQueue.Remove("7"));
                Assert.AreEqual(priorityQueue.Count, 2);
                Assert.IsTrue(priorityQueue.Remove("2"));
                Assert.AreEqual(priorityQueue.Count, 1);
                Assert.IsTrue(priorityQueue.Remove("6"));
                Assert.AreEqual(priorityQueue.Count, 0);

                Assert.IsFalse(priorityQueue.Remove("7"));
            }

            [Test]
            public void WithPriority() {
                var priorityQueue = new PriorityQueue<string, int>(PriorityQueueType.Maximum);

                int priority;

                priorityQueue.Add("5", 3);
                Assert.AreEqual(priorityQueue.Count, 1);
                Assert.IsTrue(priorityQueue.Contains("5"));
                Assert.IsTrue(priorityQueue.Remove("5", out priority));
                Assert.AreEqual(priority, 3);

                Assert.AreEqual(priorityQueue.Count, 0);

                priorityQueue.Add("6");
                priorityQueue.Add("7");
                priorityQueue.Add("2", 4);


                Assert.AreEqual(priorityQueue.Count, 3);
                Assert.IsTrue(priorityQueue.Contains("6"));
                Assert.IsTrue(priorityQueue.Contains("7"));
                Assert.IsTrue(priorityQueue.Contains("2"));

                Assert.IsFalse(priorityQueue.Remove("8", out priority));

                Assert.IsTrue(priorityQueue.Remove("7", out priority));
                Assert.AreEqual(priority, 0);
                Assert.AreEqual(priorityQueue.Count, 2);

                Assert.IsTrue(priorityQueue.Remove("2", out priority));
                Assert.AreEqual(priority, 4);

                Assert.AreEqual(priorityQueue.Count, 1);
                Assert.IsTrue(priorityQueue.Remove("6", out priority));
                Assert.AreEqual(priority, 0);
                Assert.AreEqual(priorityQueue.Count, 0);

                Assert.IsFalse(priorityQueue.Remove("7", out priority));
            }

        }

        [TestFixture]
        public class Serialization {

            [Test]
            public void Simple() {
                var priorityQueue = GetTestPriorityQueue();
                var newPriorityQueue = SerializeUtil.BinarySerializeDeserialize(priorityQueue);

                Assert.AreNotSame(priorityQueue, newPriorityQueue);
                Assert.AreEqual(priorityQueue.Count, newPriorityQueue.Count);

                while (priorityQueue.Count > 0) {
                    Assert.AreEqual(priorityQueue.Dequeue(), newPriorityQueue.Dequeue());
                }
            }

        }

        #endregion

        #region Private Members

        private static PriorityQueue<string, int> GetSimpleTestPriorityQueue() {
            return new PriorityQueue<string, int>(PriorityQueueType.Maximum)
                                          {
                                              {"a", 1},
                                              {"b", 2},
                                              {"c", 3},
                                              {"d", 4},
                                              {"e", 5},
                                              {"f", 6}
                                          };
        }

        private static PriorityQueue<string, int> GetTestPriorityQueue() {
            return new PriorityQueue<string, int>(PriorityQueueType.Maximum)
                                          {
                                              {"a", 1},
                                              {"b", 2},
                                              {"c", 3},
                                              {"d", 4},
                                              {"e", 5},
                                              {"f", 6},
                                              {"z", 6},
                                              {"y", 5},
                                              {"x", 4},
                                              {"w", 3},
                                              {"v", 2},
                                              {"u", 1}
                                          };
        }

        #endregion
    }
}