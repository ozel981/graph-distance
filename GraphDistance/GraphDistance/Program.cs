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

            var watch = Stopwatch.StartNew();
            var c = new CompatibilityGraph(g, h);

            var exactAlgorithms = new ExactAlgorithm();

            var vertices = exactAlgorithms.FindMaximumClique(c);

            c.ReorderAdjacencyMatrix(vertices);

            var result = MatricesDistance.ExtendedTaxicabGeometry(c.G.Edges, c.H.Edges);
            watch.Stop();

            Console.WriteLine("Grafy G i H przed zmianą etykiet wierzchołków");
            Console.WriteLine(TwoGraphsToString(g, h));
            
            Console.WriteLine("Grafy G i H po zmianie etykiet wierzchołków");
            Console.WriteLine(TwoGraphsToString(c.G, c.H));
            Console.WriteLine($"Rozmiar kliki: {vertices.Count}");
            Console.WriteLine($"Wynik dokładnego algorytmu: {result}, czas wykonania: {watch.ElapsedMilliseconds / 1000.0} s");
        }

        static void RunApproximationAlgorithm(Graph g, Graph h, double threshold, int seed)
        {
            Console.WriteLine("\n===== ALGORYTM APROKSYMACYJNY =====\n");
            
            var watch = Stopwatch.StartNew();
            var c = new CompatibilityGraph(g, h);

            var approximationAlgorithms = new ApproximationAlgorithms(seed);

            var vertices = approximationAlgorithms.FindMaximumClique(c, (int)(threshold * c.VerticesCount));

            c.ReorderAdjacencyMatrix(vertices);

            var result = MatricesDistance.ExtendedTaxicabGeometry(c.G.Edges, c.H.Edges);
            watch.Stop();
            Console.WriteLine("Grafy G i H przed zmianą etykiet wierzchołków");
            Console.WriteLine(TwoGraphsToString(g, h));
            
            Console.WriteLine("Grafy G i H po zmianie etykiet wierzchołków");
            Console.WriteLine(TwoGraphsToString(c.G, c.H));
            Console.WriteLine($"Rozmiar kliki: {vertices.Count}");
            Console.WriteLine($"Wynik aproksymacyjnego algorytmu: {result}, czas wykonania: {watch.ElapsedMilliseconds / 1000.0} s");
        }

        static string TwoGraphsToString(Graph g, Graph h)
        {
            int n = g.VerticesCount;
            int m = h.VerticesCount;
            if (m > n)
            {
                (g, h) = (h, g);
                (n, m) = (m, n);
            }

            var stringBuilder = new StringBuilder();
            var gLabel = $"Graf o liczbie {n} wierzchołków";
            stringBuilder.Append(gLabel);
            var gRowLength = Math.Max(n * 3 + 5, gLabel.Length + 5);
            for (int i = 0; i < gRowLength - gLabel.Length; i++)
            {
                stringBuilder.Append(" ");
            }
            stringBuilder.Append($"Graf o liczbie {m} wierzchołków\n");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    stringBuilder.Append($"{g.Edges[i, j]}, ");
                }

                if (i < m)
                {
                    for (int j = 0; j < gRowLength - n * 3; j++)
                    {
                        stringBuilder.Append(" ");
                    }
                    for (int j = 0; j < m; j++)
                    {
                        stringBuilder.Append($"{h.Edges[i, j]}, ");
                    }
                }
                stringBuilder.Append('\n');
            }

            return stringBuilder.ToString();
        }
        static double[,] ReadMatrixFromStdin()
        {
            Console.WriteLine("\nPodaj liczbę wierzchołków grafu:");
            var n = Convert.ToInt32(Console.ReadLine());
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
                return (g, h);
            }
            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(path))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line = streamReader.ReadLine();
                var n = Convert.ToInt32(line);
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
                return (g, h);
            }
        }


        static List<string> ReadAlgorithms()
        {
            Console.WriteLine("\nLista algorytmów:");
            Console.WriteLine("a) algorytm aproksymacyjny");
            Console.WriteLine("d) algorytm dokładny");

            Console.WriteLine("\nPodaj algorytmy oddzielone spacją: ");
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
