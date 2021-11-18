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
            graph = null;
        }

        public bool IsColorExistInSet(Dictionary<int, int> colouring, List<int> neighbours, int color)
        {
            foreach (int neighbour in neighbours)
            {
                if (colouring.ContainsKey(neighbour))
                {
                    if (colouring[neighbour] == color)
                        return true;
                }
            }
            return false;
        }
        public Dictionary<int, int> ColorVertices(ref List<int> verices)
        {
            Dictionary<int, int> colouring = new Dictionary<int, int>();
            foreach (int verice in verices)
            {
                int i = 0;
                while (IsColorExistInSet(colouring,graph.GetNeighbors(verice), i))
                    i++;
                colouring.Add(verice, i);
            }
            verices.Sort((a, b) => {
                if (colouring[a] > colouring[b])
                    return 1;
                else
                    return -1;
            });
            return colouring;
        }

        public void FindMaximumCliqueRef(List<int> candidates, Dictionary<int, int> colouring)
        {
            while (candidates.Any())
            {
                int vertex = candidates.Last();

                candidates.Remove(vertex);

                if (colouring[vertex] + actualClique.Count > maxClique.Count)
                {
                    actualClique.Add(vertex);
                    List<int> newCandidates = new List<int>();
                    foreach (int neighbor in graph.GetNeighbors(vertex))
                    {
                        if (candidates.Contains(neighbor))
                        {
                            newCandidates.Add(neighbor);
                        }
                    }

                    if (newCandidates.Any())
                    {
                        FindMaximumCliqueRef(newCandidates, ColorVertices(ref newCandidates));
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
            List<int> candidates = new List<int>();
            for (int i = 0; i < graph.VerticesCount; i++)
            {
                candidates.Add(i);
            }
            FindMaximumCliqueRef(candidates, ColorVertices(ref candidates));

            return maxClique;
        }
    }
}
