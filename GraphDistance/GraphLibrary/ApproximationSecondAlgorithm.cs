using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary
{
    public class ApproximationSecondAlgorithm
    {
        List<int> maxClique;
        List<int> actualClique;
        Graph graph;

        public ApproximationSecondAlgorithm()
        {
            maxClique = new List<int>();
            actualClique = new List<int>();
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

        private Dictionary<int, int> ColorVertices(ref List<int> vertices)
        {
            var coloring = new Dictionary<int, int>();
            foreach (int vertex in vertices)
            {
                int i = 0;
                while (DoesColorExistInSet(coloring,graph.GetNeighbors(vertex), i))
                    i++;
                coloring.Add(vertex, i);
            }

            vertices.Sort((a, b) => {
                if(coloring[a] == coloring[b])
                    return 0;
                else if (coloring[a] > coloring[b])
                    return 1;
                else
                    return -1;
            });
            return coloring;
        }

        private void FindMaximumCliqueRec(List<int> candidates, Dictionary<int, int> coloring)
        {
            while (candidates.Any())
            {
                int vertex = candidates.Last();

                candidates.Remove(vertex);

                if (coloring[vertex] + actualClique.Count > maxClique.Count)
                {
                    actualClique.Add(vertex);
                    var newCandidates = new List<int>();
                    foreach (int neighbor in graph.GetNeighbors(vertex))
                    {
                        if (candidates.Contains(neighbor))
                        {
                            newCandidates.Add(neighbor);
                        }
                    }

                    if (newCandidates.Any())
                    {
                        FindMaximumCliqueRec(newCandidates, ColorVertices(ref newCandidates));
                    }
                    else if (actualClique.Count > maxClique.Count)
                    {
                        maxClique = new List<int>(actualClique);
                    }
                    actualClique.Remove(vertex);
                }
            }
        }

        public List<int> FindMaximumClique(Graph graph)
        {
            this.graph = graph;
            var candidates = new List<int>();
            for (int i = 0; i < graph.VerticesCount; i++)
            {
                candidates.Add(i);
            }
            FindMaximumCliqueRec(candidates, ColorVertices(ref candidates));

            return maxClique;
        }
    }
}
