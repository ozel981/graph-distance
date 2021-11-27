using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary
{
    public static class GraphMetrics
    {
        public static double ExtendedTaxicabGeometry(Matrix<double> m1, Matrix<double> m2)
        {
            var rowCount = m1.RowCount;
            var colCount = m1.ColumnCount;

            var rowCount2 = m2.RowCount;
            var colCount2 = m2.ColumnCount;

            if (rowCount != colCount || rowCount2 != colCount2)
            {
                throw new ArgumentException($"row count differs from column count");
            }

            if (rowCount < rowCount2)
            {
                var m3 = m1;
                m1 = m2;
                m2 = m3;
            }

            int maxCols = m1.RowCount;
            int minCols = m2.RowCount;
            double sum = 0;

            for (int i = 0; i < maxCols; i++)
            {
                for (int j = 0; j < maxCols; j++)
                {
                    if (j < minCols && i < minCols)
                    {
                        sum += Math.Abs(m1[i, j] - m2[i, j]);
                    }
                    else
                    {
                        sum += Math.Abs(m1[i, j]);
                    }
                }
            }

            sum += maxCols - minCols;

            return sum;
        }
    
        public static double MaximumSubgraphGeometry(int cliqueSize, int n, int m)
        {
            return Math.Round(1.0 - cliqueSize / (double)Math.Max(n, m), 4); 
        }
    }
}
