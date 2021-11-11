using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace GraphLibrary
{
    public class Graph
    {
        public Matrix<double> edges;

        public Graph(int verticesCount)
        {
            this.edges = Matrix<double>.Build.Dense(verticesCount, verticesCount);
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
            this.edges = Matrix<double>.Build.DenseOfArray(edges);
        }

        public int VerticesCount()
        {
            return this.edges.RowCount;
        }

        public override string ToString()
        {
            int n = VerticesCount();
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"Graph with {n} vertices.\n");
            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    stringBuilder.Append($"{edges[i, j]}, ");
                }
                stringBuilder.Append('\n');
            }

            return stringBuilder.ToString();
        }
    }

    public class CompatibilityGraph : Graph
    {
        private  Dictionary<int, (int, int)> NodeMap;
        public CompatibilityGraph(Graph g, Graph h) : base(g.VerticesCount() * h.VerticesCount())
        {
            NodeMap = new Dictionary<int, (int, int)>();
            var nG = g.VerticesCount();
            var nH = h.VerticesCount();
            for (int i = 0; i < nG * nH; i++)
            {
                NodeMap[i] = (i/nG, i%nG);
            }

            var n = VerticesCount();
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
                    if (Math.Abs(g.edges[x, a] - h.edges[y, b]) < Double.Epsilon &&
                        x != a && y != b)
                    {
                        this.edges[i, j] = 1;
                    }
                }
            }
        }
    }
}
