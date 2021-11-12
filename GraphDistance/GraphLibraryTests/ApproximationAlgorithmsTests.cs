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
    public class ApproximationAlgorithmsTests
    {
        [Fact]
        public void FindMaximumClique()
        {
            var samples = 100;
            var approximationAlgorithms = new ApproximationAlgorithms(2137);
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

            var actual = approximationAlgorithms.FindMaximumClique(g, samples);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
