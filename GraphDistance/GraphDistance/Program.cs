using System;
using System.Collections.Generic;
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
                var algorithms = ReadAlgorithms();
                if (algorithms.Contains("a"))
                {
                    var threshold = 0.2;
                    var seed = 123;
                    RunApproximationAlgorithm(g, h, threshold, seed);
                }

                if (algorithms.Contains("d"))
                {
                    RunExactAlgorithm(g, h);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"nie można obliczyć dystansu między grafami: {e.Message}");
                Environment.Exit(1);
            }
        }

        static void RunExactAlgorithm(Graph g, Graph h)
        {
            Console.WriteLine("\n===== ALGORYTM DOKŁADNY =====\n");

            var c = new CompatibilityGraph(g, h);

            var exactAlgorithms = new ExactAlgorithm();

            var vertices = exactAlgorithms.FindMaximumClique(c);

            c.ReorderAdjacencyMatrix(vertices);

            var result = MatricesDistance.ExtendedTaxicabGeometry(c.G.Edges, c.H.Edges);

            Console.WriteLine("Graf G:");
            Console.WriteLine(g);
            Console.WriteLine(c.G);
            Console.WriteLine("Graf H:");
            Console.WriteLine(h);
            Console.WriteLine(c.H);
            Console.WriteLine($"Rozmiar kliki: {vertices.Count}");
            Console.WriteLine($"Wynik dokładnego algorytmu: {result}");
        }
        static void RunApproximationAlgorithm(Graph g, Graph h, double threshold, int seed)
        {
            Console.WriteLine("\n===== ALGORYTM APROKSYMACYJNY =====\n");
            var c = new CompatibilityGraph(g, h);

            var approximationAlgorithms = new ApproximationAlgorithms(seed);

            var vertices = approximationAlgorithms.FindMaximumClique(c, (int)(threshold * c.VerticesCount));

            c.ReorderAdjacencyMatrix(vertices);

            var result = MatricesDistance.ExtendedTaxicabGeometry(c.G.Edges, c.H.Edges);

            Console.WriteLine("Graf G:");
            Console.WriteLine(g);
            Console.WriteLine(c.G);
            Console.WriteLine("Graf H:");
            Console.WriteLine(h);
            Console.WriteLine(c.H);
            Console.WriteLine($"Rozmiar kliki: {vertices.Count}");
            Console.WriteLine($"Wynik aproksymacyjnego algorytmu: {result}");
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

        static List<string> ReadAlgorithms()
        {
            Console.WriteLine("Lista algorytmów:");
            Console.WriteLine("a) algorytm aproksymacyjny");
            Console.WriteLine("d) algorytm dokładny");

            Console.WriteLine("Podaj algorytmy oddzielone spacją: ");
            var algos = Console.ReadLine()?.Split();

            var availableAlgorithms = new List<string> {"a", "d"};
            foreach (var a in algos)
            {
                if (!availableAlgorithms.Contains(a))
                {
                    throw new Exception($"algorym {a} nie jest obsługiwany");
                }
            }
            return new List<string>(algos);
        }
    }
}
