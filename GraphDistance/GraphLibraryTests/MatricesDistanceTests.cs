using FluentAssertions;
using GraphLibrary;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GraphLibraryTests
{
    public class MatricesDistanceTests
    {
        [Fact]
        public void ExtendedTaxicabGeometry()
        {
            var expectedResult = 12;
            var m1 = Matrix<double>.Build.DenseOfArray(new double[,]
            {
                {1, 0, 1 },
                {0, 1, 0 },
                {0, 0, 1 }
            });

            var m2 = Matrix<double>.Build.DenseOfArray(new double[,]
            {
                {0, 1, 0, 1 },
                {0, 0, 0, 1 },
                {0, 1, 1, 0 },
                {1, 1, 1, 1 }
            });

            var result = MatricesDistance.ExtendedTaxicabGeometry(m1, m2);

            result.Should().Equals(expectedResult);
        }
    }
}
