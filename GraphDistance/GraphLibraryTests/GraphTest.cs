using Xunit;

using GraphLibrary;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using FluentAssertions;

namespace GraphLibraryTests
{
    public class CompatibilityGraphTests
    {
        private readonly int precision = 2;
        
        [Fact]
        public void ReorderAdjacencyMatrix()
        {
            var edges = new double[,]
            {
                {1, 1, 0, 0 },
                {1, 1, 0, 0 },
                {0, 0, 0, 0 },
                {0, 0, 0, 0 }
            };

            var g = new Graph(edges);
            g.ReorderAdjacencyMatrix(new List<int> { 2, 3 });

            var expectedAdjacencyMatrix = Matrix<double>.Build.DenseOfArray(new double[,]
            {
                {0, 0, 0, 0 },
                {0, 0, 0, 0 },
                {0, 0, 1, 1 },
                {0, 0, 1, 1 }
            });

            g.Edges.ToArray().Should().Equals(expectedAdjacencyMatrix.ToArray());
        }

        // This test case is taken from http://www.lsis.org/tuples/workshop/wscp_bgbtp_1.pdf, fig. 2
        [Fact]
        public void ComputeCompatibilityGraph()
        {
            var gEdges = new double[,]
            {
                {0, 1, 0},
                {1, 0, 1},
                {0, 1, 0},
            };
            var g = new Graph(gEdges);
            var hEdges = new double[,]
            {
                {0, 1, 1},
                {1, 0, 1},
                {1, 1, 0},
            };
            var h = new Graph(hEdges);
            var actual = new CompatibilityGraph(g, h);
            var expectedEdges = new double[,]
            {
                {0, 0, 0, 0, 1, 1, 0, 0, 0, },
                {0, 0, 0, 1, 0, 1, 0, 0, 0, },
                {0, 0, 0, 1, 1, 0, 0, 0, 0, },
                {0, 1, 1, 0, 0, 0, 0, 1, 1, },
                {1, 0, 1, 0, 0, 0, 1, 0, 1, },
                {1, 1, 0, 0, 0, 0, 1, 1, 0, },
                {0, 0, 0, 0, 1, 1, 0, 0, 0, },
                {0, 0, 0, 1, 0, 1, 0, 0, 0, },
                {0, 0, 0, 1, 1, 0, 0, 0, 0, },
            };
            AssertMatricesEqual(expectedEdges, actual.Edges.ToArray());
        }
        
        private void AssertMatricesEqual(double[,] exp, double[,] act)
        {
            Assert.Equal(exp.GetLength(0), act.GetLength(0));
            Assert.Equal(exp.GetLength(1), act.GetLength(1));

            for (int i = 0; i < exp.GetLength(0); i++)
            {
                for (int j = 0; j < exp.GetLength(1); j++)
                {
                    Assert.Equal(exp[i, j], act[i, j], precision);
                }
            }
        }
    }
}