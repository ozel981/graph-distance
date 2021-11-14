using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary
{
    public class ExactAlgorithm
    {
        private List<int> maxClique = new List<int>();
        private void PrepareSets(int verticesCount, out List<int> clique, out List<int> rejected, out List<int> candidates)
        {
            clique = new List<int>();
            rejected = new List<int>();
            candidates = new List<int>();
            for (int i = 0; i < verticesCount; i++)
            {
                candidates.Add(i);
            }
        }

        private void FindMaxClique(ref Graph graph, ref List<int> clique, ref List<int> rejected, ref List<int> candidates)
        {
            while(candidates.Any())
            {
                List<int> localRejected = new List<int>(rejected);
                List<int> localCandidates = new List<int>(candidates);
                int vertex = candidates.First();
                clique.Add(vertex);
                candidates.Remove(vertex);
                List<int> neighbours = graph.GetNeighbors(vertex);
                localRejected.RemoveAll((x) => !neighbours.Contains(x));
                localCandidates.RemoveAll((x) => !neighbours.Contains(x));
                if (localRejected.Count == 0 && localCandidates.Count == 0)
                {
                    if (clique.Count > maxClique.Count)
                    {
                        maxClique = new List<int>(clique);
                    }
                } else
                {
                    FindMaxClique(ref graph, ref clique, ref localRejected, ref localCandidates);
                }
                clique.Remove(vertex);
                rejected.Add(vertex);
            }
        }

        public List<int> FindMaximumClique(Graph graph)
        {
            List<int> maximumClique = new List<int>();

            List<int> clique;
            List<int> rejected;
            List<int> candidates;
            PrepareSets(graph.VerticesCount, out clique, out rejected, out candidates);
            FindMaxClique(ref graph, ref clique, ref rejected, ref candidates);

            return maxClique;
        }
    }
}
