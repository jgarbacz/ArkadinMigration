/*  
  Copyright 2007-2009 The NGenerics Team
 (http://code.google.com/p/ngenerics/wiki/Team)

 This program is licensed under the GNU Lesser General Public License (LGPL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://www.gnu.org/copyleft/lesser.html.
*/



using NGenerics.DataStructures.General;
using NUnit.Framework;

namespace ExampleLibraryCSharp.DataStructures.General
{
    [TestFixture]
    public class EdgeExamples
    {

        #region Constructor
        [Test]
        public void ConstructorExample()
        {
            // Create the graph to add the edge to.
            var graph = new Graph<int>(true);

            // Create the two vertices that would make 
            // up the edge
            var vertex1 = graph.AddVertex(5);
            var vertex2 = graph.AddVertex(3);

            // Create an edge between the two vertices
            var edge = new Edge<int>(vertex1, vertex2, true);

            // The from vertex would be vertex1
            Assert.AreEqual(edge.FromVertex, vertex1);

            // The to vertex would be vertex2
            Assert.AreEqual(edge.ToVertex, vertex2);

            // The weight of the vertex will default to 0.
            Assert.AreEqual(edge.Weight, 0);
        }
        #endregion

        #region WeightedConstructor
        [Test]
        public void WeightedConstructorExample()
        {
            // Create the graph to add the edge to.
            var graph = new Graph<int>(true);

            // Create the two vertices that would make 
            // up the edge
            var vertex1 = graph.AddVertex(5);
            var vertex2 = graph.AddVertex(3);

            // Create an edge between the two vertices
            // with a weight of 34.5.
            var edge = new Edge<int>(vertex1, vertex2, 34.5, true);

            // The from vertex would be vertex1
            Assert.AreEqual(edge.FromVertex, vertex1);

            // The to vertex would be vertex2
            Assert.AreEqual(edge.ToVertex, vertex2);

            // The weight of the vertex will be 34.5
            Assert.AreEqual(edge.Weight, 34.5);
        }
        #endregion

        #region FromVertex
        [Test]
        public void FromVertexExample()
        {
            // Create the graph to add the edge to.
            var graph = new Graph<int>(true);

            // Create the two vertices that would make 
            // up the edge
            var vertex1 = graph.AddVertex(5);
            var vertex2 = graph.AddVertex(3);

            // Add an edge between the two vertice
            var edge = graph.AddEdge(vertex1, vertex2);

            // The from vertex will be vertex1
            Assert.AreSame(edge.FromVertex, vertex1);
        }
        #endregion

        #region ToVertex
        [Test]
        public void ToVertexExample()
        {
            // Create the graph to add the edge to.
            var graph = new Graph<int>(true);

            // Create the two vertices that would make 
            // up the edge
            var vertex1 = graph.AddVertex(5);
            var vertex2 = graph.AddVertex(3);

            // Add an edge between the two vertice
            var edge = graph.AddEdge(vertex1, vertex2);

            // The to vertex will be vertex2
            Assert.AreSame(edge.ToVertex, vertex2);
        }
        #endregion

        #region GetPartnerVertex
        [Test]
        public void GetPartnerVertexExample()
        {
            // Create the graph to add the edge to.
            var graph = new Graph<int>(true);

            // Create the two vertices that would make 
            // up the edge
            var vertex1 = graph.AddVertex(5);
            var vertex2 = graph.AddVertex(3);

            // Add an edge between the two vertice
            var edge = graph.AddEdge(vertex1, vertex2);

            // The partner of vertex1 will be vertex2
            Assert.AreSame(edge.GetPartnerVertex(vertex1), vertex2);

            // The partner of vertex2 will be vertex1
            Assert.AreSame(edge.GetPartnerVertex(vertex2), vertex1);
        }
        #endregion

        #region IsDirected
        [Test]
        public void IsDirectedExample()
        {
            // Create the graph to add the edge to.
            var graph = new Graph<int>(true);

            // Create the two vertices that would make 
            // up the edge
            var vertex1 = graph.AddVertex(5);
            var vertex2 = graph.AddVertex(3);

            // Add an edge between the two vertice
            var edge = graph.AddEdge(vertex1, vertex2);

            // The edge will be directed, since the
            // graph only accepts directed edges.
            Assert.IsTrue(edge.IsDirected);
        }
        #endregion

        #region Tag
        [Test]
        public void TagExample()
        {
            // Create the two vertices that would make 
            // up the edge
            var vertex1 = new Vertex<int>(5);
            var vertex2 = new Vertex<int>(3);

            // Add an edge between the two vertice
            var edge = new Edge<int>(
                vertex1,     // From Vertex
                vertex2,     // To Vertex
                false   // Undirected edge
            );

            // Add a tag object to the edge
            edge.Tag = 20;

            // The tag on the edge will be equal to 20.
            Assert.AreEqual(edge.Tag, 20);
        }
        #endregion

        #region Weight
        [Test]
        public void WeightExample()
        {
            // Create the two vertices that would make 
            // up the edge
            var vertex1 = new Vertex<int>(5);
            var vertex2 = new Vertex<int>(3);

            // Add an edge between the two vertice
            var edge = new Edge<int>(
                vertex1,     // From Vertex
                vertex2,     // To Vertex
                32.4,   // Weight of the edge
                false   // Undirected edge
            );

            // The weight on the edge, set in the constructor
            // is equal to 32.4.  
            Assert.AreEqual(edge.Weight, 32.4);

            // The weight can also be set with the property
            edge.Weight = 3.67;

            Assert.AreEqual(edge.Weight, 3.67);
        }
        #endregion
    }
}
