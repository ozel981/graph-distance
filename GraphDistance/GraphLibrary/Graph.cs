using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace GraphLibrary
{
    public class Graph : ICloneable
    {
        public Matrix<double> Edges { get; protected set; }
        public int VerticesCount => Edges.RowCount;
        public Graph(int verticesCount)
        {
            Edges = Matrix<double>.Build.Dense(verticesCount, verticesCount);
        }

        public Graph(double[,] edges)
        {
            if (edges.Rank != 2)
            {
                throw new ArgumentException($"invalid array rank: expected 2, got {edges.Rank}");
            }

            var rowCount = edges.GetLength(0);
            var colCount = edges.GetLength(1);

            if (rowCount != colCount)
            {
                throw new ArgumentException($"row count ({rowCount}) differs from column count ({colCount})");
            }

            Edges = Matrix<double>.Build.DenseOfArray(edges);
        }

        public List<int> GetNeighbors(int vertex)
        {
            var neighbors = new List<int>();

            for (int i = 0; i < VerticesCount; i++)
            {
                if (Edges[i, vertex] > 0)
                {
                    neighbors.Add(i);
                }
            }

            return neighbors;
        }

        public override string ToString()
        {
            int n = VerticesCount;

            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"Graf o liczbie wierzchołków: {n}\n");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    stringBuilder.Append($"{Edges[i, j]}, ");
                }
                stringBuilder.Append('\n');
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Move given vertices to the front
        /// </summary>
        /// <param name="vertices"></param>
        public virtual void ReorderAdjacencyMatrix(List<int> vertices)
        {
            var newOrder = new List<int>();
            Matrix<double> matrix = Matrix<double>.Build.Dense(VerticesCount, VerticesCount);
            bool[] columnsUsed = new bool[VerticesCount];

            // Swap columns
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i] >= VerticesCount)
                {
                    throw new ArgumentException($"vertex ({vertices[i]}) does not exist in graph");
                }

                matrix.SetColumn(i, Edges.Column(vertices[i]));
                columnsUsed[vertices[i]] = true;
            }

            for (int i = 0; i < VerticesCount; i++)
            {
                if (!columnsUsed[i])
                {
                    newOrder.Add(i);
                }
            }

            for (int i = 0; i < newOrder.Count; i++)
            {
                matrix.SetColumn(i + vertices.Count, Edges.Column(newOrder[i]));
            }

            Matrix<double> helperMatrix = Matrix<double>.Build.DenseOfMatrix(matrix);

            // Swap rows
            for (int i = 0; i < vertices.Count; i++)
            {
                matrix.SetRow(i, helperMatrix.Row(vertices[i]));
            }

            for (int i = 0; i < newOrder.Count; i++)
            {
                matrix.SetRow(i + vertices.Count, helperMatrix.Row(newOrder[i]));
            }

            Edges = matrix;
        }

        public object Clone()
        {
            return new Graph(Edges.Clone().ToArray());
        }

        public bool IsDirected()
        {
            return !this.Edges.Equals(this.Edges.Transpose());
        }
    }

    public class CompatibilityGraph : Graph
    {
        public Graph G { get; }
        public Graph H { get; }

        public Dictionary<int, (int, int)> NodeMap { get; }
        public CompatibilityGraph(Graph g, Graph h) : base(g.VerticesCount * h.VerticesCount)
        {
            G = (Graph)g.Clone();
            H = (Graph)h.Clone();
            NodeMap = new Dictionary<int, (int, int)>();

            var nG = g.VerticesCount;
            var nH = h.VerticesCount;
            var max = Math.Max(nG, nH);
            for (int i = 0; i < nG * nH; i++)
            {
                NodeMap.Add(i, (i / max, i % max));
            }

            UpdateAdjacencyMatrix();
        }

        public override void ReorderAdjacencyMatrix(List<int> vertices)
        {
            var gOrder = new List<int>();
            var hOrder = new List<int>();

            foreach (var vertex in vertices)
            {
                var mapping = NodeMap[vertex];
                gOrder.Add(mapping.Item1);
                hOrder.Add(mapping.Item2);
            }

            G.ReorderAdjacencyMatrix(gOrder);
            H.ReorderAdjacencyMatrix(hOrder);

            Edges = Matrix<double>.Build.Dense(VerticesCount, VerticesCount);
            UpdateAdjacencyMatrix();
        }

        private void UpdateAdjacencyMatrix()
        {
            var n = VerticesCount;

            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    (var x, var y) = NodeMap[i];
                    (var a, var b) = NodeMap[j];

                    if (Math.Abs(G.Edges[x, a] - H.Edges[y, b]) < double.Epsilon &&
                        x != a && y != b)
                    {
                        Edges[i, j] = 1;
                    }
                }
            }
        }
    }
}
