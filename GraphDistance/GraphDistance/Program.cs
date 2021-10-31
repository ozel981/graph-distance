using System;
using GraphLibrary;

namespace GraphDistance
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Graph g = new Graph(new double[,]{{0,1},{1,0}});
            Console.WriteLine(g);
        }
    }
}
