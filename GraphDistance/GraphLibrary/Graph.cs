using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace GraphLibrary
{
    public class Graph
    {
        public Matrix<double> Edges { get; }

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

        public int VerticesCount()
        {
            return Edges.RowCount;
        }

        public override string ToString()
        {
            int n = VerticesCount();

            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"Graph with {n} vertices.\n");
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
    }

    public class CompatibilityGraph : Graph
    {
        private Dictionary<int, (int, int)> nodeMap;
        public CompatibilityGraph(Graph g, Graph h) : base(g.VerticesCount() * h.VerticesCount())
        {
            nodeMap = new Dictionary<int, (int, int)>();
            var nG = g.VerticesCount();
            var nH = h.VerticesCount();

            for (int i = 0; i < nG * nH; i++)
            {
                nodeMap.Add(i, (i / nG, i % nG));
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

                    (var x, var y) = nodeMap[i];
                    (var a, var b) = nodeMap[j];

                    if (Math.Abs(g.Edges[x, a] - h.Edges[y, b]) < Double.Epsilon &&
                        x != a && y != b)
                    {
                        Edges[i, j] = 1;
                    }
                }
            }
        }
    }
}
