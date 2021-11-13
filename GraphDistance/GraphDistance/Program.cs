﻿using System;
using GraphLibrary;
using MathNet.Numerics.LinearAlgebra;

namespace GraphDistance
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var g = new Graph(ReadMatrix());
                var h = new Graph(ReadMatrix());

                Console.WriteLine(g);
                Console.WriteLine(h);

                var c = new CompatibilityGraph(g, h);

                var threshold = 0.1;

                var approximationAlgorithms = new ApproximationAlgorithms();

                var vertices = approximationAlgorithms.FindMaximumClique(c, (int)(threshold * c.VerticesCount));

                c.ReorderAdjacencyMatrix(vertices);

                var result = MatricesDistance.ExtendedTaxicabGeometry(c.G.Edges, c.H.Edges);

                Console.WriteLine(result);

            }
            catch (Exception e)
            {
                Console.WriteLine($"could not create graphs: {e}");
                Environment.Exit(1);
            }
        }

        static double[,] ReadMatrix()
        {
            var n = Convert.ToInt32(Console.ReadLine());
            double[,] edges = new double[n,n];
            for (int i = 0; i < n; i++)
            {
                var row = Console.ReadLine()?.Split();
                if (row.Length != n)
                {
                    throw new Exception("incorrect number of values in a row");
                }
                for (int j = 0; j < n; j++)
                {
                    edges[i, j] = int.Parse(row[j]);
                }
            }
            return edges;
        }
    }
}
