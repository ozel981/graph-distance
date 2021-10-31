using System;
using System.ComponentModel.DataAnnotations;

using MathNet.Numerics.LinearAlgebra;

namespace GraphLibrary
{
    public class Graph
    {
        private Matrix<double> edges;

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

        public Graph NewCompatibilityGraph(Graph g, Graph h)
        {
            throw new NotImplementedException();
        }
        
        public int VerticesCount()
        {
            return this.edges.RowCount;
        }

        public override string ToString()
        {
            return $"Graph with {this.VerticesCount()} vertices.";
        }
    }
}
