using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Xml.Linq;
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
                (var g, var h) = ReadGraphs();
                var algorithms = ReadAlgorithms();
                if (algorithms.Contains("a"))
                {
                    var threshold = 0.2;
                    var seed = 123;
                    RunApproximationAlgorithm(g, h, threshold, seed);
                }

                if (algorithms.Contains("k"))
                {
                    RunColoringApproximationAlgorithm(g, h);
                }
                if (algorithms.Contains("d"))
                {
                    RunExactAlgorithm(g, h);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"nie można obliczyć dystansu między grafami: {e.Message}");
            }

            Console.WriteLine("Naciśnij enter aby zakończyć!");
            Console.ReadLine();
        }

        static void RunExactAlgorithm(Graph g, Graph h)
        {
            Console.WriteLine("\n===== ALGORYTM DOKŁADNY =====\n");

            var watch = Stopwatch.StartNew();
            var c = new CompatibilityGraph(g, h);

            var exactAlgorithms = new ExactAlgorithm();

            var vertices = exactAlgorithms.FindMaximumClique(c);

            var result = GraphMetrics.MaximumSubgraphGeometry(vertices.Count, g.VerticesCount, h.VerticesCount);
            watch.Stop();

            Console.WriteLine($"Rozmiar największego wspólnego podgrafu: {vertices.Count}");
            Console.WriteLine("Wierzchołki największego wspólnego podgrafu:");

            Console.WriteLine($"Graf g:");
            foreach (var vertex in vertices)
            {
                var labels = c.NodeMap[vertex];

                Console.Write($"{labels.Item1} ");
            }
            Console.WriteLine();

            Console.WriteLine($"Graf h:");
            foreach (var vertex in vertices)
            {
                var labels = c.NodeMap[vertex];

                Console.Write($"{labels.Item2} ");
            }
            Console.WriteLine();

            Console.WriteLine($"Wynik dokładnego algorytmu: {result}, czas wykonania: {watch.ElapsedMilliseconds / 1000.0} s");
        }

        static void RunApproximationAlgorithm(Graph g, Graph h, double threshold, int seed)
        {
            Console.WriteLine("\n===== ALGORYTM APROKSYMACYJNY =====\n");
            
            var watch = Stopwatch.StartNew();
            var c = new CompatibilityGraph(g, h);

            var approximationAlgorithms = new ApproximationAlgorithms(seed);

            var vertices = approximationAlgorithms.FindMaximumClique(c, (int)(threshold * c.VerticesCount));

            var result = GraphMetrics.MaximumSubgraphGeometry(vertices.Count, g.VerticesCount, h.VerticesCount);
            watch.Stop();

            Console.WriteLine($"Rozmiar największego wspólnego podgrafu: {vertices.Count}");
            Console.WriteLine("Wierzchołki największego wspólnego podgrafu:");

            Console.WriteLine($"Graf g:");
            foreach (var vertex in vertices)
            {
                var labels = c.NodeMap[vertex];

                Console.Write($"{labels.Item1} ");
            }
            Console.WriteLine();

            Console.WriteLine($"Graf h:");
            foreach (var vertex in vertices)
            {
                var labels = c.NodeMap[vertex];

                Console.Write($"{labels.Item2} ");
            }
            Console.WriteLine();

            Console.WriteLine($"Wynik aproksymacyjnego algorytmu: {result}, czas wykonania: {watch.ElapsedMilliseconds / 1000.0} s");
        }
        
        static void RunColoringApproximationAlgorithm(Graph g, Graph h)
        {
            Console.WriteLine("\n===== ALGORYTM APROKSYMACYJNY Z KOLOROWANIEM WIERZCHOŁKÓW =====\n");
            
            var watch = Stopwatch.StartNew();
            var c = new CompatibilityGraph(g, h);

            var approximationAlgorithm = new ApproximationColoringAlgorithm();

            var vertices = approximationAlgorithm.FindMaximumClique(c);

            var result = GraphMetrics.MaximumSubgraphGeometry(vertices.Count, g.VerticesCount, h.VerticesCount);
            watch.Stop();

            Console.WriteLine($"Rozmiar największego wspólnego podgrafu: {vertices.Count}");
            Console.WriteLine("Wierzchołki największego wspólnego podgrafu:");

            Console.WriteLine($"Graf g:");
            foreach (var vertex in vertices)
            {
                var labels = c.NodeMap[vertex];

                Console.Write($"{labels.Item1} ");
            }
            Console.WriteLine();

            Console.WriteLine($"Graf h:");
            foreach (var vertex in vertices)
            {
                var labels = c.NodeMap[vertex];

                Console.Write($"{labels.Item2} ");
            }
            Console.WriteLine();

            Console.WriteLine($"Wynik aproksymacyjnego algorytmu z kolorowaniem: {result}, czas wykonania: {watch.ElapsedMilliseconds / 1000.0} s");
        }

        static double[,] ReadMatrixFromStdin()
        {
            Console.WriteLine("\nPodaj liczbę wierzchołków grafu:");
            var n = Convert.ToInt32(Console.ReadLine());
            if (n < 1)
            {
                throw new Exception("liczba wierzchołków musi być większa od 0");
            }
            double[,] edges = new double[n,n];
            Console.WriteLine("\nPodaj macierz sąsiedztwa grafu:");
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

        static (Graph g, Graph h) ReadGraphs()
        {
            Console.WriteLine("Podaj ścieżkę do pliku z wejściem (jeśli grafy mają być wpisane" +
                              " ręcznie, wciśnij ENTER):");
            var path = Console.ReadLine();
            if (String.IsNullOrEmpty(path))
            {
                var g = new Graph(ReadMatrixFromStdin());
                var h = new Graph(ReadMatrixFromStdin());
                ValidateGraph(g);
                ValidateGraph(h);
                return (g, h);
            }
            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(path))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line = streamReader.ReadLine();
                var n = Convert.ToInt32(line);
                if (n < 1)
                {
                    throw new Exception("liczba wierzchołków musi być większa od 0");
                }
                double[,] edgesG = new double[n,n];
                for (int i = 0; i < n; i++)
                {
                    var row = streamReader.ReadLine()?.Split();
                    if (row.Length != n)
                    {
                        throw new Exception("incorrect number of values in a row");
                    }
                    for (int j = 0; j < n; j++)
                    {
                        edgesG[i, j] = int.Parse(row[j]);
                    }
                }
                line = streamReader.ReadLine();
                var m = Convert.ToInt32(line);
                double[,] edgesH = new double[m,m];
                for (int i = 0; i < m; i++)
                {
                    var row = streamReader.ReadLine()?.Split();
                    if (row.Length != m)
                    {
                        throw new Exception("incorrect number of values in a row");
                    }
                    for (int j = 0; j < m; j++)
                    {
                        edgesH[i, j] = int.Parse(row[j]);
                    }
                }
                var g = new Graph(edgesG);
                var h = new Graph(edgesH);
                ValidateGraph(g);
                ValidateGraph(h);
                return (g, h);
            }
        }

        static void ValidateGraph(Graph g)
        {
            if (g.IsDirected())
            {
                throw new Exception("graf nie może być skierowany");
            }
        }

        static List<string> ReadAlgorithms()
        {
            Console.WriteLine("\nLista algorytmów:");
            Console.WriteLine("a) algorytm aproksymacyjny");
            Console.WriteLine("k) algorytm aproksymacyjny z kolorowaniem wierzchołków");
            Console.WriteLine("d) algorytm dokładny");

            Console.WriteLine("\nPodaj litery oznaczające algorytmy oddzielone spacją: ");
            var algos = Console.ReadLine()?.Split();

            var availableAlgorithms = new List<string> {"a", "d", "k"};
            foreach (var a in algos)
            {
                if (!availableAlgorithms.Contains(a))
                {
                    throw new Exception($"algorytm {a} nie jest obsługiwany");
                }
            }
            return new List<string>(algos);
        }
    }
}
