using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace GraphLibrary
{
    public class Graph
    {
        public Matrix<double> Edges { get; }

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
            this.Edges = Matrix<double>.Build.DenseOfArray(edges);
        }

        public Graph NewCompatibilityGraph(Graph g, Graph h)
        {
            throw new NotImplementedException();
        }
        
        public int VerticesCount()
        {
            return this.Edges.RowCount;
        }

        public override string ToString()
        {
            int n = VerticesCount();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Graph with {this.VerticesCount()} vertices.\n");

            for(int i=0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    stringBuilder.Append($"{Edges[i, j]}, ");
                }
                stringBuilder.Append('\n');
            }

            return stringBuilder.ToString();
        }
    }
}
