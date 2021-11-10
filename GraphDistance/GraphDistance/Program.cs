using System;
using GraphLibrary;

namespace GraphDistance
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Graph g = new Graph(ReadMatrix());
                Console.WriteLine(g);

                var vertices = ApproximationAlgorithm.FindMaximumClique(g, 100);

                foreach(var vertex in vertices)
                {
                    Console.Write($"{vertex}, ");
                }
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"could not create graphs: {e}");
                System.Environment.Exit(1);
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
