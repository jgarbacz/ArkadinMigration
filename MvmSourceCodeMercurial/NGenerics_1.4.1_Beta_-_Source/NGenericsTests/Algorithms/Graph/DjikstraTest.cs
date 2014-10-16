/*  
  Copyright 2007-2009 The NGenerics Team
 (http://code.google.com/p/ngenerics/wiki/Team)

 This program is licensed under the GNU Lesser General Public License (LGPL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://www.gnu.org/copyleft/lesser.html.
*/


using System;
using System.Collections.Generic;
using NGenerics.Algorithms;
using NGenerics.DataStructures.General;
using NUnit.Framework;

namespace NGenerics.Tests.Algorithms.Graph
{
	[TestFixture]
	public class DijkstraTest
    {
        [TestFixture]
        public class DijkstrasAlgorithm
        {
            [Test]
            public void DirectedSimple()
            {
				var graph = new Graph<int>(true);

                var vertex1 = new Vertex<int>(1);
                var vertex2 = new Vertex<int>(2);
                var vertex3 = new Vertex<int>(3);

                graph.AddVertex(vertex1);
                graph.AddVertex(vertex2);
                graph.AddVertex(vertex3);

                graph.AddEdge(vertex1, vertex3, 5);
                graph.AddEdge(vertex1, vertex2, 3);
                graph.AddEdge(vertex2, vertex3, 4);
                graph.AddEdge(vertex3, vertex1, 3);

                var resultGraph = GraphAlgorithms.DijkstrasAlgorithm(graph, vertex1);

                Assert.AreEqual(resultGraph.Edges.Count, 2);

                var edges = GetEdgeList(resultGraph.Edges.GetEnumerator());

                for (var i = 0; i < edges.Count; i++)
                {
                    if ((edges[i].FromVertex.Data == 1) && (edges[i].ToVertex.Data == 2))
                    {
                        Assert.AreEqual(edges[i].Weight, 3);
                        Assert.AreEqual(edges[i].FromVertex.Weight, 0);
                        Assert.AreEqual(edges[i].ToVertex.Weight, 3);
                    }
                    else if ((edges[i].FromVertex.Data == 1) && (edges[i].ToVertex.Data == 3))
                    {
                        Assert.AreEqual(edges[i].Weight, 5);
                        Assert.AreEqual(edges[i].FromVertex.Weight, 0);
                        Assert.AreEqual(edges[i].ToVertex.Weight, 5);
                    }
                    else
                    {
                        throw new Exception("Incorrect edge found for shortest path.");
                    }
                }
            }

            [Test]
            public void DirectedSimple2()
            {
                var graph = new Graph<int>(true);

                var vertex1 = new Vertex<int>(1);
                var vertex2 = new Vertex<int>(2);
                var vertex3 = new Vertex<int>(3);

                graph.AddVertex(vertex1);
                graph.AddVertex(vertex2);
                graph.AddVertex(vertex3);

                graph.AddEdge(vertex1, vertex3, 5);
                graph.AddEdge(vertex1, vertex2, 3);
                graph.AddEdge(vertex2, vertex3, 4);
                graph.AddEdge(vertex3, vertex1, 3);

                var resultGraph = GraphAlgorithms.DijkstrasAlgorithm(graph, vertex2);

                Assert.AreEqual(resultGraph.Edges.Count, 2);

                var edges = GetEdgeList(resultGraph.Edges.GetEnumerator());

                for (var i = 0; i < edges.Count; i++)
                {
                    if ((edges[i].FromVertex.Data == 2) && (edges[i].ToVertex.Data == 3))
                    {
                        Assert.AreEqual(edges[i].Weight, 4);
                        Assert.AreEqual(edges[i].FromVertex.Weight, 0);
                        Assert.AreEqual(edges[i].ToVertex.Weight, 4);
                    }
                    else if ((edges[i].FromVertex.Data == 3) && (edges[i].ToVertex.Data == 1))
                    {
                        Assert.AreEqual(edges[i].Weight, 3);
                        Assert.AreEqual(edges[i].FromVertex.Weight, 4);
                        Assert.AreEqual(edges[i].ToVertex.Weight, 7);
                    }
                    else
                    {
                        throw new Exception("Incorrect edge found for shortest path.");
                    }
                }
            }


            [Test]
            public void UndirectedSimple1()
            {
                var graph = new Graph<int>(false);

                var vertex1 = new Vertex<int>(1);
                var vertex2 = new Vertex<int>(2);
                var vertex3 = new Vertex<int>(3);

                graph.AddVertex(vertex1);
                graph.AddVertex(vertex2);
                graph.AddVertex(vertex3);

                graph.AddEdge(vertex1, vertex3, 1);
                graph.AddEdge(vertex1, vertex2, 3);
                graph.AddEdge(vertex2, vertex3, 5);

                var resultGraph = GraphAlgorithms.DijkstrasAlgorithm(graph, vertex1);

                Assert.AreEqual(resultGraph.Edges.Count, 2);

                var edges = GetEdgeList(resultGraph.Edges.GetEnumerator());

                for (var i = 0; i < edges.Count; i++)
                {
                    if ((edges[i].FromVertex.Data == 1) && (edges[i].ToVertex.Data == 2))
                    {
                        Assert.AreEqual(edges[i].Weight, 3);
                        Assert.AreEqual(edges[i].FromVertex.Weight, 0);
                        Assert.AreEqual(edges[i].ToVertex.Weight, 3);
                    }
                    else if ((edges[i].FromVertex.Data == 1) && (edges[i].ToVertex.Data == 3))
                    {
                        Assert.AreEqual(edges[i].Weight, 1);
                        Assert.AreEqual(edges[i].FromVertex.Weight, 0);
                        Assert.AreEqual(edges[i].ToVertex.Weight, 1);
                    }
                    else
                    {
                        throw new Exception("Incorrect edge found for shortest path.");
                    }
                }
            }

            [Test]
            public void UndirectedSimple2()
            {
                var graph = new Graph<int>(false);

                var vertex1 = new Vertex<int>(1);
                var vertex2 = new Vertex<int>(2);
                var vertex3 = new Vertex<int>(3);

                graph.AddVertex(vertex1);
                graph.AddVertex(vertex2);
                graph.AddVertex(vertex3);

                graph.AddEdge(vertex1, vertex3, 1);
                graph.AddEdge(vertex1, vertex2, 3);
                graph.AddEdge(vertex2, vertex3, 5);

                var resultGraph = GraphAlgorithms.DijkstrasAlgorithm(graph, vertex2);

                Assert.AreEqual(resultGraph.Edges.Count, 2);

                var edges = GetEdgeList(resultGraph.Edges.GetEnumerator());

                for (var i = 0; i < edges.Count; i++)
                {
                    if ((edges[i].FromVertex.Data == 2) && (edges[i].ToVertex.Data == 1))
                    {
                        Assert.AreEqual(edges[i].Weight, 3);
                        Assert.AreEqual(edges[i].FromVertex.Weight, 0);
                        Assert.AreEqual(edges[i].ToVertex.Weight, 3);
                    }
                    else if ((edges[i].FromVertex.Data == 1) && (edges[i].ToVertex.Data == 3))
                    {
                        Assert.AreEqual(edges[i].Weight, 1);
                        Assert.AreEqual(edges[i].FromVertex.Weight, 3);
                        Assert.AreEqual(edges[i].ToVertex.Weight, 4);
                    }
                    else
                    {
                        throw new Exception("Incorrect edge found for shortest path.");
                    }
                }
            }

            [Test]
            public void Directed2()
            {
                var graph = new Graph<string>(true);

                var vertex1 = new Vertex<string>("a");
                var vertex2 = new Vertex<string>("b");
                var vertex3 = new Vertex<string>("c");
                var vertex4 = new Vertex<string>("d");
                var vertex5 = new Vertex<string>("e");
                var vertex6 = new Vertex<string>("f");

                graph.AddVertex(vertex1);
                graph.AddVertex(vertex2);
                graph.AddVertex(vertex3);
                graph.AddVertex(vertex4);
                graph.AddVertex(vertex5);
                graph.AddVertex(vertex6);

                graph.AddEdge(vertex1, vertex2, 5);
                graph.AddEdge(vertex1, vertex3, 5);
                graph.AddEdge(vertex1, vertex4, 5);
                graph.AddEdge(vertex1, vertex5, 5);
                graph.AddEdge(vertex1, vertex6, 1);

                graph.AddEdge(vertex2, vertex3, 1);

                graph.AddEdge(vertex3, vertex4, 1);

                graph.AddEdge(vertex5, vertex4, 2);

                graph.AddEdge(vertex6, vertex5, 2);
                graph.AddEdge(vertex6, vertex2, 2);

                var resultGraph = GraphAlgorithms.DijkstrasAlgorithm(graph, vertex1);

                Assert.AreEqual(resultGraph.Vertices.Count, 6);
                Assert.AreEqual(resultGraph.Edges.Count, 5);

                var edges = GetEdgeList(resultGraph.Edges.GetEnumerator());

                for (var i = 0; i < edges.Count; i++)
                {
                    if ((edges[i].FromVertex.Data == "a") && (edges[i].ToVertex.Data == "d"))
                    {
                        Assert.AreEqual(edges[i].Weight, 5);
                        Assert.AreEqual(edges[i].FromVertex.Weight, 0);
                        Assert.AreEqual(edges[i].ToVertex.Weight, 5);
                    }
                    else if ((edges[i].FromVertex.Data == "a") && (edges[i].ToVertex.Data == "f"))
                    {
                        Assert.AreEqual(edges[i].Weight, 1);
                        Assert.AreEqual(edges[i].FromVertex.Weight, 0);
                        Assert.AreEqual(edges[i].ToVertex.Weight, 1);
                    }
                    else if ((edges[i].FromVertex.Data == "b") && (edges[i].ToVertex.Data == "c"))
                    {
                        Assert.AreEqual(edges[i].Weight, 1);
                        Assert.AreEqual(edges[i].FromVertex.Weight, 3);
                        Assert.AreEqual(edges[i].ToVertex.Weight, 4);
                    }
                    else if ((edges[i].FromVertex.Data == "f") && (edges[i].ToVertex.Data == "b"))
                    {
                        Assert.AreEqual(edges[i].Weight, 2);
                        Assert.AreEqual(edges[i].FromVertex.Weight, 1);
                        Assert.AreEqual(edges[i].ToVertex.Weight, 3);
                    }
                    else if ((edges[i].FromVertex.Data == "f") && (edges[i].ToVertex.Data == "e"))
                    {
                        Assert.AreEqual(edges[i].Weight, 2);
                        Assert.AreEqual(edges[i].FromVertex.Weight, 1);
                        Assert.AreEqual(edges[i].ToVertex.Weight, 3);
                    }
                    else
                    {
                        throw new Exception("Incorrect edge found for shortest path.");
                    }
                }
            }

            [Test]
            public void MoreComplicatedDirectedGraph()
            {
                var graph = new Graph<string>(true);


                var vertex1 = new Vertex<string>("a");
                var vertex2 = new Vertex<string>("b");
                var vertex3 = new Vertex<string>("c");
                var vertex4 = new Vertex<string>("d");
                var vertex5 = new Vertex<string>("e");
                var vertex6 = new Vertex<string>("f");
                var vertex7 = new Vertex<string>("g");

                graph.AddVertex(vertex1);
                graph.AddVertex(vertex2);
                graph.AddVertex(vertex3);
                graph.AddVertex(vertex4);
                graph.AddVertex(vertex5);
                graph.AddVertex(vertex6);
                graph.AddVertex(vertex7);

                graph.AddEdge(vertex1, vertex6, 4);

                graph.AddEdge(vertex2, vertex1, 2);
                graph.AddEdge(vertex2, vertex7, 2);

                graph.AddEdge(vertex3, vertex2, 3);

                graph.AddEdge(vertex4, vertex3, 2);
                graph.AddEdge(vertex4, vertex5, 1);

                graph.AddEdge(vertex5, vertex6, 3);
                graph.AddEdge(vertex5, vertex7, 2);

                graph.AddEdge(vertex6, vertex7, 1);

                graph.AddEdge(vertex7, vertex6, 1);
                graph.AddEdge(vertex7, vertex4, 3);
                graph.AddEdge(vertex7, vertex3, 4);
                graph.AddEdge(vertex7, vertex1, 1);

                var resultGraph = GraphAlgorithms.DijkstrasAlgorithm(graph, vertex7);

                Assert.AreEqual(resultGraph.Vertices.Count, 7);
                Assert.AreEqual(resultGraph.Edges.Count, 6);

                var edges = GetEdgeList(resultGraph.Edges.GetEnumerator());

                for (var i = 0; i < edges.Count; i++)
                {
                    if ((edges[i].FromVertex.Data == "g") && (edges[i].ToVertex.Data == "a"))
                    {
                        Assert.AreEqual(edges[i].Weight, 1);
                        Assert.AreEqual(edges[i].FromVertex.Weight, 0);
                        Assert.AreEqual(edges[i].ToVertex.Weight, 1);
                    }
                    else if ((edges[i].FromVertex.Data == "c") && (edges[i].ToVertex.Data == "b"))
                    {
                        Assert.AreEqual(edges[i].Weight, 3);
                        Assert.AreEqual(edges[i].FromVertex.Weight, 4);
                        Assert.AreEqual(edges[i].ToVertex.Weight, 7);
                    }
                    else if ((edges[i].FromVertex.Data == "g") && (edges[i].ToVertex.Data == "c"))
                    {
                        Assert.AreEqual(edges[i].Weight, 4);
                        Assert.AreEqual(edges[i].FromVertex.Weight, 0);
                        Assert.AreEqual(edges[i].ToVertex.Weight, 4);
                    }
                    else if ((edges[i].FromVertex.Data == "g") && (edges[i].ToVertex.Data == "d"))
                    {
                        Assert.AreEqual(edges[i].Weight, 3);
                        Assert.AreEqual(edges[i].FromVertex.Weight, 0);
                        Assert.AreEqual(edges[i].ToVertex.Weight, 3);
                    }
                    else if ((edges[i].FromVertex.Data == "d") && (edges[i].ToVertex.Data == "e"))
                    {
                        Assert.AreEqual(edges[i].Weight, 1);
                        Assert.AreEqual(edges[i].FromVertex.Weight, 3);
                        Assert.AreEqual(edges[i].ToVertex.Weight, 4);
                    }
                    else if ((edges[i].FromVertex.Data == "g") && (edges[i].ToVertex.Data == "f"))
                    {
                        Assert.AreEqual(edges[i].Weight, 1);
                        Assert.AreEqual(edges[i].FromVertex.Weight, 0);
                        Assert.AreEqual(edges[i].ToVertex.Weight, 1);
                    }
                    else
                    {
                        throw new Exception("Incorrect edge found for shortest path.");
                    }
                }
            }

            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ExceptionNullGraph()
            {
                GraphAlgorithms.DijkstrasAlgorithm(null, new Vertex<int>(5));
            }

            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ExceptionNullVertex()
            {
                GraphAlgorithms.DijkstrasAlgorithm(new Graph<int>(true), null);
            }

            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void ExceptionInvalidVertex()
            {
                GraphAlgorithms.DijkstrasAlgorithm(new Graph<int>(true), new Vertex<int>(5));
            }

        }


        #region Private Members

        private static List<Edge<T>> GetEdgeList<T>(IEnumerator<Edge<T>> edges)
        {
            var edgeList = new List<Edge<T>>();

            while (edges.MoveNext())
            {
                edgeList.Add(edges.Current);
            }
            return edgeList;
        }

        #endregion

    }
}
