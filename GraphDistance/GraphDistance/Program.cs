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
                Graph h = new Graph(ReadMatrix());
                Console.WriteLine(g);
                Console.WriteLine(h);
                var c = new CompatibilityGraph(g, h);
                Console.WriteLine(c);
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
