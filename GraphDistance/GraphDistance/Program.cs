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

                var c = new CompatibilityGraph(g, h);
                var c2 = new CompatibilityGraph(g, h);

                var threshold = 0.2;

                var approximationAlgorithms = new ApproximationAlgorithms(123);
                var exactAlgorithms = new ExactAlgorithm();

                var vertices = approximationAlgorithms.FindMaximumClique(c, (int)(threshold * c.VerticesCount));
                var verticesExact = exactAlgorithms.FindMaximumClique(c);

                c.ReorderAdjacencyMatrix(vertices);
                c2.ReorderAdjacencyMatrix(verticesExact);

                var result = MatricesDistance.ExtendedTaxicabGeometry(c.G.Edges, c.H.Edges);
                var result2 = MatricesDistance.ExtendedTaxicabGeometry(c2.G.Edges, c2.H.Edges);

                Console.WriteLine("Graph G:");
                Console.WriteLine(g);
                Console.WriteLine(c.G);
                Console.WriteLine("Graph H:");
                Console.WriteLine(h);
                Console.WriteLine(c.H);
                Console.WriteLine($"Approximate clique size: {vertices.Count}");
                Console.WriteLine($"Approximate algorithm result: {result}");

                Console.WriteLine();
                Console.WriteLine("Graph G:");
                Console.WriteLine(g);
                Console.WriteLine(c2.G);
                Console.WriteLine("Graph H:");
                Console.WriteLine(h);
                Console.WriteLine(c2.H);
                Console.WriteLine($"Exact clique size: {verticesExact.Count}");
                Console.WriteLine($"Exact algorithm result: {result2}");
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
