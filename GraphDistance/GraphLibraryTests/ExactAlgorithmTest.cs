using FluentAssertions;
using GraphLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GraphLibraryTests
{
    public class ExactAlgorithmTest
    {
        [Fact]
        public void FindMaximumClique0()
        {
            var exactAlgorithms = new ExactAlgorithm();
            var expected = new List<int> { 1, 2, 3, 4 };

            var edges = new double[,]
            {
                { 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 0 },
                { 0, 1, 0, 1, 1, 0 },
                { 0, 1, 1, 0, 1, 0 },
                { 0, 1, 1, 1, 0, 1 },
                { 0, 0, 0, 0, 1, 0 },
            };

            var g = new Graph(edges);

            var actual = exactAlgorithms.FindMaximumClique(g);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void FindMaximumClique1()
        {
            var exactAlgorithms = new ExactAlgorithm();
            var expected = new List<int> { 2, 3, 4 };

            var edges = new double[,]
            {
                { 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0 },
                { 0, 1, 0, 1, 1, 1 },
                { 0, 0, 1, 0, 1, 0 },
                { 0, 0, 1, 1, 0, 0 },
                { 0, 0, 1, 0, 0, 0 },
            };

            var g = new Graph(edges);

            var actual = exactAlgorithms.FindMaximumClique(g);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void FindMaximumClique2()
        {
            var exactAlgorithms = new ExactAlgorithm();
            var expected = new List<int> { 0, 1, 2, 3, 4, 5 };

            var edges = new double[,]
            {
                    { 0, 1, 1, 1, 1, 1, 0, 0, 0 },
                    { 1, 0, 1, 1, 1, 1, 0, 0, 0 },
                    { 1, 1, 0, 1, 1, 1, 0, 0, 0 },
                    { 1, 1, 1, 0, 1, 1, 0, 0, 0 },
                    { 1, 1, 1, 1, 0, 1, 0, 1, 1 },
                    { 1, 1, 1, 1, 1, 0, 1, 0, 0 },
                    { 0, 0, 0, 0, 0, 1, 0, 1, 0 },
                    { 0, 0, 0, 0, 1, 0, 1, 0, 1 },
                    { 0, 0, 0, 0, 1, 0, 0, 1, 0 }
            };

            var g = new Graph(edges);

            var actual = exactAlgorithms.FindMaximumClique(g);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
