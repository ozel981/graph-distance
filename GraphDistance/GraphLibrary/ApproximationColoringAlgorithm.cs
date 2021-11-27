using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary
{
    public class ApproximationColoringAlgorithm
    {
        List<int> maxClique;
        Graph graph;

        public ApproximationColoringAlgorithm()
        {
            maxClique = new List<int>();
        }

        private bool DoesColorExistInSet(Dictionary<int, int> coloring, List<int> neighbours, int color)
        {
            foreach (int neighbour in neighbours)
            {
                if (coloring.ContainsKey(neighbour))
                {
                    if (coloring[neighbour] == color)
                        return true;
                }
            }
            return false;
        }

        private int GetVertexWithMaxColor(Dictionary<int, int> coloring)
        {
            int maxVertex = coloring.First().Key;
            int maxColor = coloring.First().Value;
            foreach (var vertex in coloring)
            {
                if (vertex.Value > maxColor)
                {
                    maxVertex = vertex.Key;
                    maxColor = vertex.Value;
                }
            }
            return maxVertex;
        }

        private Dictionary<int, int> ColorVertices(List<int> vertices)
        {
            var coloring = new Dictionary<int, int>();
            foreach (int vertex in vertices)
            {
                int i = 0;
                while (DoesColorExistInSet(coloring, graph.GetNeighbors(vertex), i))
                    i++;
                coloring.Add(vertex, i);
            }
            return coloring;
        }

        public List<int> FindMaximumClique(Graph graph)
        {
            this.graph = graph;
            var candidates = new List<int>();

            for (int i = 0; i < graph.VerticesCount; i++)
                candidates.Add(i);

            candidates.Sort((a, b) =>
            {
                int aDegree = graph.GetDegree(a);
                int bDegree = graph.GetDegree(b);
                if (aDegree == bDegree)
                    return 0;
                else if (aDegree > bDegree)
                    return 1;
                else
                    return -1;
            });

            Dictionary<int, int> coloring = ColorVertices(candidates);

            while (candidates.Any())
            {
                int vertex = GetVertexWithMaxColor(coloring);
                candidates.Remove(vertex);
                maxClique.Add(vertex);
                List<int> neighbors = graph.GetNeighbors(vertex);
                candidates.RemoveAll((candidate) => !neighbors.Contains(candidate));
                coloring = ColorVertices(candidates);
            }

            return maxClique;
        }
    }
}
